using System;
using System.Drawing;
using System.Windows.Forms;
using GrandSereneHotel.DatabaseAccess;
using System.Data.SqlClient;

namespace HotelGrandSerene.DashboardModule.UserControlNavigation
{
    public partial class ucFinance : UserControl
    {
        private readonly Random _rand = new Random();

        public ucFinance()
        {
            InitializeComponent();

            // Run configuration and loading directly in constructor
            ConfigureGrid();
            PopulateData();
        }

        private void ConfigureGrid()
        {
            // Prevent WinForms from resetting designer columns
            dgvTransactions.AutoGenerateColumns = false;

            // Hide native column header bar
            dgvTransactions.ColumnHeadersVisible = false;

            // General grid settings
            dgvTransactions.AllowUserToAddRows = false;
            dgvTransactions.AllowUserToDeleteRows = false;
            dgvTransactions.ReadOnly = true;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactions.MultiSelect = false;

            // Row heights for 345px total height
            dgvTransactions.RowTemplate.Height = 48;
            dgvTransactions.ScrollBars = ScrollBars.Vertical;

            // Ensure text color is dark solid slate (not transparent or white)
            dgvTransactions.DefaultCellStyle.ForeColor = Color.FromArgb(30, 41, 59);
            dgvTransactions.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 41, 59);
            dgvTransactions.DefaultCellStyle.SelectionBackColor = Color.FromArgb(241, 245, 249);
            dgvTransactions.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            // Bind dynamic formatting
            dgvTransactions.CellFormatting -= dgvTransactions_CellFormatting;
            dgvTransactions.CellFormatting += dgvTransactions_CellFormatting;
        }

        private static string Peso(decimal amount)
        {
            return "\u20B1" + amount.ToString("N2");
        }
        private void PopulateData()
        {
            dgvTransactions.Rows.Clear();

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // ---- Recent transactions (every payment, newest first) ----
                    string listSql = @"
                SELECT TOP 100
                       CONCAT(c.FirstName, ' ', c.LastName),
                       CONCAT(rm.RoomNumber, ' ', REPLACE(rt.TypeName, ' Room', '')),
                       p.PaymentDate, p.PaymentType, p.Amount
                FROM PAYMENT p
                JOIN RESERVATION r ON r.ReservationID = p.ReservationID
                JOIN CUSTOMER c    ON c.CustomerID    = r.CustomerID
                JOIN ROOM rm       ON rm.RoomID       = r.RoomID
                JOIN ROOM_TYPE rt  ON rt.RoomTypeID   = rm.RoomTypeID
                ORDER BY p.PaymentDate DESC, p.PaymentID DESC";

                    using (SqlCommand cmd = new SqlCommand(listSql, conn))
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            int rowIndex = dgvTransactions.Rows.Add();
                            DataGridViewRow row = dgvTransactions.Rows[rowIndex];
                            row.Cells[0].Value = r.GetString(0);                              // guest
                            row.Cells[1].Value = r.GetString(1);                              // room
                            row.Cells[2].Value = r.GetDateTime(2).ToString("MMM dd, yyyy");   // date
                            row.Cells[3].Value = r.IsDBNull(3) ? "" : r.GetString(3);         // type
                            row.Cells[4].Value = Peso(r.GetDecimal(4));                       // amount
                        }
                    }

                    // ---- MONTHLY REVENUE: payments received this month ----
                    using (SqlCommand cmd = new SqlCommand(@"
                SELECT ISNULL(SUM(Amount), 0) FROM PAYMENT
                WHERE PaymentStatus = 'Paid'
                  AND YEAR(PaymentDate) = YEAR(GETDATE()) AND MONTH(PaymentDate) = MONTH(GETDATE())", conn))
                    {
                        guna2HtmlLabel4.Text = Peso(Convert.ToDecimal(cmd.ExecuteScalar()));
                    }

                    // ---- OUTSTANDING: unpaid balance on bookings that are not finished/cancelled ----
                    using (SqlCommand cmd = new SqlCommand(@"
                SELECT ISNULL(SUM(CASE WHEN t.Total - t.Paid > 0 THEN t.Total - t.Paid ELSE 0 END), 0)
                FROM (SELECT DATEDIFF(DAY, r.CheckInDate, r.CheckOutDate) * rt.PricePerNight AS Total,
                             ISNULL((SELECT SUM(p.Amount) FROM PAYMENT p
                                     WHERE p.ReservationID = r.ReservationID AND p.PaymentStatus = 'Paid'), 0) AS Paid
                      FROM RESERVATION r
                      JOIN ROOM rm      ON rm.RoomID     = r.RoomID
                      JOIN ROOM_TYPE rt ON rt.RoomTypeID = rm.RoomTypeID
                      WHERE r.Status IN ('Pending', 'Confirmed', 'Checked-In')) t", conn))
                    {
                        guna2HtmlLabel2.Text = Peso(Convert.ToDecimal(cmd.ExecuteScalar()));
                    }

                    // ---- AVG. DAILY RATE: average price per booked night ----
                    using (SqlCommand cmd = new SqlCommand(@"
                SELECT ISNULL(SUM(DATEDIFF(DAY, r.CheckInDate, r.CheckOutDate) * rt.PricePerNight)
                              / NULLIF(SUM(DATEDIFF(DAY, r.CheckInDate, r.CheckOutDate)), 0), 0)
                FROM RESERVATION r
                JOIN ROOM rm      ON rm.RoomID     = r.RoomID
                JOIN ROOM_TYPE rt ON rt.RoomTypeID = rm.RoomTypeID
                WHERE r.Status <> 'Cancelled'", conn))
                    {
                        guna2HtmlLabel5.Text = Peso(Convert.ToDecimal(cmd.ExecuteScalar()));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load finance data:\n\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dgvTransactions.ClearSelection();
        }

        private void dgvTransactions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            // Column 3: TYPE -> Accent Blue
            if (e.ColumnIndex == 3)
            {
                e.CellStyle.ForeColor = Color.FromArgb(37, 99, 235);
                e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            }
            // Column 4: AMOUNT -> Bold Dark Text
            else if (e.ColumnIndex == 4)
            {
                e.CellStyle.ForeColor = Color.FromArgb(30, 41, 59);
                e.CellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            }
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e) { }
        private void ucFinance_Load(object sender, EventArgs e) { }
        private void dgvTransactions_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void ucFinance_Load_1(object sender, EventArgs e)
        {

        }
    }
}

