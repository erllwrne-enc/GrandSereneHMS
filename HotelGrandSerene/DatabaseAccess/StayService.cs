// All the database work for checking guests in and out lives here, so the
// dialogs stay small. Both operations run as ONE transaction (all or nothing).
//
// Status words used (keep the whole team on these):
//   Reservation: Confirmed -> Checked-In -> Checked-Out        (or Cancelled)
//   Room:        Available -> Occupied -> Cleaning -> Available
//   (Housekeeping marks a Cleaning room as Available from the Rooms tab.)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace GrandSereneHotel.DatabaseAccess
{
    public class StayInfo
    {
        public int ReservationId;
        public int RoomId;
        public string RoomNumber = "";
        public string TypeName = "";
        public string FirstName = "";
        public string LastName = "";
        public DateTime CheckIn;
        public DateTime CheckOut;
        public string Status = "";
        public decimal Total;     // nights x price per night
        public decimal Paid;      // payments already recorded as 'Paid'
        public string PaymentMethod = "";   // method of the most recent payment ("" if none)

        public decimal Balance { get { return Math.Max(0m, Total - Paid); } }
        public string GuestName { get { return FirstName + " " + LastName; } }
        public int Nights { get { return (CheckOut.Date - CheckIn.Date).Days; } }
    }

    public static class StayService
    {
        // Accepts "R-10001", "r-10001" or "10001"
        public static bool TryParseCode(string text, out int reservationId)
        {
            string t = (text ?? "").Trim();
            if (t.StartsWith("R-", StringComparison.OrdinalIgnoreCase)) t = t.Substring(2);
            return int.TryParse(t, out reservationId);
        }

        // Returns null when the reservation does not exist. May throw SqlException.
        public static StayInfo Find(int reservationId)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                return Load(conn, null, reservationId);
            }
        }

        private static StayInfo Load(SqlConnection conn, SqlTransaction tx, int reservationId)
        {
            const string sql = @"
                SELECT r.ReservationID, r.RoomID, rm.RoomNumber, rt.TypeName,
                       c.FirstName, c.LastName, r.CheckInDate, r.CheckOutDate, r.Status,
                       DATEDIFF(DAY, r.CheckInDate, r.CheckOutDate) * rt.PricePerNight AS Total,
                       ISNULL((SELECT SUM(p.Amount) FROM PAYMENT p
                               WHERE p.ReservationID = r.ReservationID AND p.PaymentStatus = 'Paid'), 0) AS Paid,
                       ISNULL((SELECT TOP 1 p.PaymentMethod FROM PAYMENT p
                               WHERE p.ReservationID = r.ReservationID
                               ORDER BY p.PaymentID DESC), '') AS PayMethod
                FROM RESERVATION r
                JOIN CUSTOMER  c  ON c.CustomerID  = r.CustomerID
                JOIN ROOM      rm ON rm.RoomID     = r.RoomID
                JOIN ROOM_TYPE rt ON rt.RoomTypeID = rm.RoomTypeID
                WHERE r.ReservationID = @Id";

            using (SqlCommand cmd = new SqlCommand(sql, conn, tx))
            {
                cmd.Parameters.AddWithValue("@Id", reservationId);
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return null;
                    return new StayInfo
                    {
                        ReservationId = r.GetInt32(0),
                        RoomId = r.GetInt32(1),
                        RoomNumber = r.GetString(2),
                        TypeName = r.GetString(3),
                        FirstName = r.GetString(4),
                        LastName = r.GetString(5),
                        CheckIn = r.GetDateTime(6),
                        CheckOut = r.GetDateTime(7),
                        Status = r.GetString(8),
                        Total = r.GetDecimal(9),
                        Paid = r.GetDecimal(10),
                        PaymentMethod = r.GetString(11)
                    };
                }
            }
        }

        // Returns null on success, otherwise a message to show the staff member.
        public static string CheckIn(int reservationId)
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (SqlTransaction tx = conn.BeginTransaction())
                    {
                        try
                        {
                            int roomId;

                            // Only a Confirmed/Pending reservation can be checked in
                            using (SqlCommand cmd = new SqlCommand(
                                @"UPDATE RESERVATION SET Status = 'Checked-In'
                                  OUTPUT INSERTED.RoomID
                                  WHERE ReservationID = @Id AND Status IN ('Confirmed', 'Pending')", conn, tx))
                            {
                                cmd.Parameters.AddWithValue("@Id", reservationId);
                                object result = cmd.ExecuteScalar();
                                if (result == null)
                                {
                                    tx.Rollback();
                                    return "This reservation can't be checked in. It may already be checked in, checked out, or cancelled.";
                                }
                                roomId = Convert.ToInt32(result);
                            }

                            using (SqlCommand cmd = new SqlCommand(
                                "UPDATE ROOM SET Status = 'Occupied' WHERE RoomID = @Room", conn, tx))
                            {
                                cmd.Parameters.AddWithValue("@Room", roomId);
                                cmd.ExecuteNonQuery();
                            }

                            tx.Commit();
                            return null;
                        }
                        catch
                        {
                            try { tx.Rollback(); } catch { }
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Database error:\n\n" + ex.Message;
            }
        }

        // Collects any remaining balance, marks the stay finished and the room 'Cleaning'.
        // Returns null on success, otherwise a message. 'collected' = amount paid just now.
        public static string CheckOut(int reservationId, string paymentMethod, out decimal collected)
        {
            collected = 0m;
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (SqlTransaction tx = conn.BeginTransaction())
                    {
                        try
                        {
                            StayInfo info = Load(conn, tx, reservationId);
                            if (info == null)
                            {
                                tx.Rollback();
                                return "Reservation not found.";
                            }
                            if (info.Status != "Checked-In")
                            {
                                tx.Rollback();
                                return "This guest is not checked in (status: " + info.Status + ").";
                            }

                            decimal balance = info.Balance;

                            // The database refuses payments of 0, so only record a real balance
                            if (balance > 0m)
                            {
                                using (SqlCommand cmd = new SqlCommand(
                                    @"INSERT INTO PAYMENT (ReservationID, Amount, PaymentMethod, PaymentStatus, PaymentType)
                                      VALUES (@Res, @Amount, @Method, 'Paid', 'Balance Payment')", conn, tx))
                                {
                                    cmd.Parameters.AddWithValue("@Res", reservationId);
                                    cmd.Parameters.AddWithValue("@Amount", balance);
                                    cmd.Parameters.AddWithValue("@Method", string.IsNullOrWhiteSpace(paymentMethod) ? "Cash" : paymentMethod);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            using (SqlCommand cmd = new SqlCommand(
                                "UPDATE RESERVATION SET Status = 'Checked-Out' WHERE ReservationID = @Id", conn, tx))
                            {
                                cmd.Parameters.AddWithValue("@Id", reservationId);
                                cmd.ExecuteNonQuery();
                            }

                            using (SqlCommand cmd = new SqlCommand(
                                "UPDATE ROOM SET Status = 'Cleaning' WHERE RoomID = @Room", conn, tx))
                            {
                                cmd.Parameters.AddWithValue("@Room", info.RoomId);
                                cmd.ExecuteNonQuery();
                            }

                            tx.Commit();
                            collected = balance;
                            return null;
                        }
                        catch
                        {
                            try { tx.Rollback(); } catch { }
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                collected = 0m;
                return "Database error:\n\n" + ex.Message;
            }
        }
    }
}