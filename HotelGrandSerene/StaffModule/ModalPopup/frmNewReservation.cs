using GrandSereneHotel.DatabaseAccess;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace HotelGrandSerene.StaffModule.ModalPopup
{
    public partial class frmNewReservation : Form
    {
        private bool isFullPayment = false;

        // Public property to receive selected room numbers if passed directly
        public List<string> SelectedRoomNumbers { get; private set; } = new List<string>();

        public frmNewReservation()
        {
            InitializeComponent();
        }
        private List<int> _holdIds = new List<int>();
        private const decimal DownPaymentRate = 0.50m;   // 50% down payment - change if your group decides otherwise

        private class HoldInfo
        {
            public int RoomID;
            public DateTime CheckIn;
            public DateTime CheckOut;
            public int Guests;
            public decimal PricePerNight;
        }

        public frmNewReservation(List<int> holdIds) : this()
        {
            if (holdIds != null) _holdIds = holdIds;
        }

        private static object DbVal(string s)
        {
            return string.IsNullOrWhiteSpace(s) ? (object)DBNull.Value : s.Trim();
        }

        // Deletes the holds this form was given (used when the staff cancels)
        private void ReleaseHolds()
        {
            if (_holdIds.Count == 0) return;
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string inList = string.Join(",", _holdIds.Select((id, i) => "@h" + i));
                    using (SqlCommand cmd = new SqlCommand(
                        "DELETE FROM TEMP_RESERVATION_HOLD WHERE TempHoldID IN (" + inList + ")", conn))
                    {
                        for (int i = 0; i < _holdIds.Count; i++)
                            cmd.Parameters.AddWithValue("@h" + i, _holdIds[i]);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { /* not critical: old holds are cleaned up automatically after 30 minutes */ }
            _holdIds.Clear();
        }
        private void frmNewReservation_Load(object sender, EventArgs e)
        {
            // 1. Setup Escaped Birthdate Placeholder
            SetupDatePickerPlaceholders();

            // 2. Default payment type selection (Partial Downpayment)
            SetPaymentType(isFull: false);

            // 3. Set default payment method to Cash safely without firing IndexChanged prematurely
            cmbPaymentMethod.SelectedIndexChanged -= cmbPaymentMethod_SelectedIndexChanged;

            if (cmbPaymentMethod != null && cmbPaymentMethod.Items.Count > 0)
            {
                cmbPaymentMethod.SelectedIndex = 0; // Selects "Cash"
            }

            cmbPaymentMethod.SelectedIndexChanged += cmbPaymentMethod_SelectedIndexChanged;

            // 4. Force hide both detail panels initially on form launch
            HideAllPaymentPanels();
        }

        // --- Escaped DatePicker Placeholder Setup ---
        private void SetupDatePickerPlaceholders()
        {
            if (dtpBirthdate != null)
            {
                dtpBirthdate.Format = DateTimePickerFormat.Custom;
                dtpBirthdate.CustomFormat = "   'Select Birthdate'";
            }
        }
        //

       // --- Payment Type Toggle (Partial vs Full Payment) ---
        private void btnPartialPayment_Click(object sender, EventArgs e)
        {
            SetPaymentType(isFull: false);
        }

        private void btnFullPayment_Click(object sender, EventArgs e)
        {
            SetPaymentType(isFull: true);
        }

        private void SetPaymentType(bool isFull)
        {
            isFullPayment = isFull;

            if (btnPartialPayment != null && btnFullPayment != null)
            {
                if (isFullPayment)
                {
                    btnFullPayment.FillColor = Color.FromArgb(30, 41, 59);
                    btnFullPayment.ForeColor = Color.White;

                    btnPartialPayment.FillColor = Color.FromArgb(241, 245, 249);
                    btnPartialPayment.ForeColor = Color.FromArgb(100, 116, 139);
                }
                else
                {
                    btnPartialPayment.FillColor = Color.FromArgb(30, 41, 59);
                    btnPartialPayment.ForeColor = Color.White;

                    btnFullPayment.FillColor = Color.FromArgb(241, 245, 249);
                    btnFullPayment.ForeColor = Color.FromArgb(100, 116, 139);
                }
            }
        }
        //endregion

       // --- Dynamic Payment Panel Selection ---
        private void cmbPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPaymentMethod == null || cmbPaymentMethod.SelectedItem == null) return;
            string method = cmbPaymentMethod.SelectedItem.ToString().Trim();

            // 1. CASH: Hide both detail panels completely
            if (method.Equals("Cash", StringComparison.OrdinalIgnoreCase))
            {
                HideAllPaymentPanels();
                return;
            }

            // 2. CREDIT / DEBIT CARD: Show pnlPaymentDetails, hide pnlDetails
            if (method.Contains("Credit") || method.Contains("Card"))
            {
                if (pnlPaymentDetails != null) pnlPaymentDetails.Visible = true;
                if (pnlDetails != null) pnlDetails.Visible = false;
            }
            // 3. BANK TRANSFER / E-WALLET: Show pnlDetails, hide pnlPaymentDetails
            else if (method.Contains("Bank") || method.Contains("Transfer") || method.Contains("Wallet"))
            {
                if (pnlPaymentDetails != null) pnlPaymentDetails.Visible = false;
                if (pnlDetails != null) pnlDetails.Visible = true;

                // Auto-select initial index for bank dropdowns if unselected
                Guna2ComboBox bankCombo = GetBankComboBox();
                if (bankCombo != null && bankCombo.Items.Count > 0 && bankCombo.SelectedIndex == -1)
                    bankCombo.SelectedIndex = 0;

                if (cmbBankTransferType != null && cmbBankTransferType.Items.Count > 0 && cmbBankTransferType.SelectedIndex == -1)
                    cmbBankTransferType.SelectedIndex = 0;
            }
        }

        private void HideAllPaymentPanels()
        {
            if (pnlPaymentDetails != null) pnlPaymentDetails.Visible = false;
            if (pnlDetails != null) pnlDetails.Visible = false;
        }

        private Guna2ComboBox GetBankComboBox()
        {
            Control[] found = this.Controls.Find("cmbBankName", true);
            if (found.Length > 0 && found[0] is Guna2ComboBox cb1) return cb1;

            found = this.Controls.Find("guna2ComboBox2", true);
            if (found.Length > 0 && found[0] is Guna2ComboBox cb2) return cb2;

            return null;
        }
        //endregion

        // --- Confirm Staging Push & Cancel Actions ---
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // 1. Basic validation (same checks as before)
            if (string.IsNullOrWhiteSpace(txtFirstName?.Text) || string.IsNullOrWhiteSpace(txtLastName?.Text))
            {
                MessageBox.Show("Please enter the customer's First and Last Name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPhone?.Text))
            {
                MessageBox.Show("Please enter a contact phone number.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_holdIds.Count == 0)
            {
                MessageBox.Show("No rooms were selected. Please start the reservation again.", "No Rooms",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string paymentMethod = cmbPaymentMethod?.SelectedItem?.ToString() ?? "Cash";
            string paymentType = isFullPayment ? "Full Payment" : "Down Payment";
            var reservationIds = new List<int>();

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // One transaction: either EVERYTHING is saved, or NOTHING is.
                    using (SqlTransaction tx = conn.BeginTransaction())
                    {
                        try
                        {
                            // 2. Read the held rooms and their nightly price
                            var holds = new List<HoldInfo>();
                            string inList = string.Join(",", _holdIds.Select((id, i) => "@h" + i));
                            string holdSql = @"
                        SELECT h.RoomID, h.CheckInDate, h.CheckOutDate, h.NumberOfGuests, rt.PricePerNight
                        FROM TEMP_RESERVATION_HOLD h
                        JOIN ROOM r ON r.RoomID = h.RoomID
                        JOIN ROOM_TYPE rt ON rt.RoomTypeID = r.RoomTypeID
                        WHERE h.TempHoldID IN (" + inList + ")";

                            using (SqlCommand cmd = new SqlCommand(holdSql, conn, tx))
                            {
                                for (int i = 0; i < _holdIds.Count; i++)
                                    cmd.Parameters.AddWithValue("@h" + i, _holdIds[i]);

                                using (SqlDataReader rdr = cmd.ExecuteReader())
                                {
                                    while (rdr.Read())
                                    {
                                        holds.Add(new HoldInfo
                                        {
                                            RoomID = rdr.GetInt32(0),
                                            CheckIn = rdr.GetDateTime(1),
                                            CheckOut = rdr.GetDateTime(2),
                                            Guests = rdr.GetInt32(3),
                                            PricePerNight = rdr.GetDecimal(4)
                                        });
                                    }
                                }
                            }

                            if (holds.Count != _holdIds.Count)
                            {
                                tx.Rollback();
                                MessageBox.Show("The room hold has expired. Please select the rooms again.",
                                    "Hold Expired", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            // 3. Make sure nobody booked these rooms in the meantime
                            foreach (HoldInfo h in holds)
                            {
                                using (SqlCommand cmd = new SqlCommand(@"
                            SELECT COUNT(*) FROM RESERVATION WITH (UPDLOCK, HOLDLOCK)
                            WHERE RoomID = @R AND Status <> 'Cancelled'
                              AND CheckInDate < @Out AND CheckOutDate > @In", conn, tx))
                                {
                                    cmd.Parameters.AddWithValue("@R", h.RoomID);
                                    cmd.Parameters.AddWithValue("@In", h.CheckIn);
                                    cmd.Parameters.AddWithValue("@Out", h.CheckOut);

                                    if ((int)cmd.ExecuteScalar() > 0)
                                    {
                                        tx.Rollback();
                                        MessageBox.Show("One of the selected rooms was just booked by someone else. Please select again.",
                                            "Room No Longer Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }
                                }
                            }

                            // 4. Save the customer
                            int customerId;
                            using (SqlCommand cmd = new SqlCommand(@"
                        INSERT INTO CUSTOMER (FirstName, LastName, ContactNumber, Email, Address, IDType, IDNumber)
                        OUTPUT INSERTED.CustomerID
                        VALUES (@First, @Last, @Phone, @Email, @Address, @IDType, @IDNumber)", conn, tx))
                            {
                                cmd.Parameters.AddWithValue("@First", txtFirstName.Text.Trim());
                                cmd.Parameters.AddWithValue("@Last", txtLastName.Text.Trim());
                                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                                cmd.Parameters.AddWithValue("@Email", DbVal(txtEmail?.Text));
                                cmd.Parameters.AddWithValue("@Address", DbVal(txtHomeAddress?.Text));
                                cmd.Parameters.AddWithValue("@IDType", DbVal(txtIDType?.Text));
                                cmd.Parameters.AddWithValue("@IDNumber", DbVal(txtIDNumber?.Text));
                                customerId = (int)cmd.ExecuteScalar();
                            }

                            // 5. One reservation + one payment per room
                            foreach (HoldInfo h in holds)
                            {
                                int nights = (h.CheckOut - h.CheckIn).Days;
                                decimal total = nights * h.PricePerNight;
                                decimal amountPaid = isFullPayment ? total : Math.Round(total * DownPaymentRate, 2);

                                int reservationId;
                                using (SqlCommand cmd = new SqlCommand(@"
                            INSERT INTO RESERVATION (CustomerID, RoomID, CheckInDate, CheckOutDate, NumberOfGuests, Status)
                            OUTPUT INSERTED.ReservationID
                            VALUES (@Cust, @Room, @In, @Out, @Guests, 'Confirmed')", conn, tx))
                                {
                                    cmd.Parameters.AddWithValue("@Cust", customerId);
                                    cmd.Parameters.AddWithValue("@Room", h.RoomID);
                                    cmd.Parameters.AddWithValue("@In", h.CheckIn);
                                    cmd.Parameters.AddWithValue("@Out", h.CheckOut);
                                    cmd.Parameters.AddWithValue("@Guests", h.Guests);
                                    reservationId = (int)cmd.ExecuteScalar();
                                }
                                reservationIds.Add(reservationId);

                                using (SqlCommand cmd = new SqlCommand(@"
                            INSERT INTO PAYMENT (ReservationID, Amount, PaymentMethod, PaymentStatus, PaymentType)
                            VALUES (@Res, @Amount, @Method, 'Paid', @Type)", conn, tx))
                                {
                                    cmd.Parameters.AddWithValue("@Res", reservationId);
                                    cmd.Parameters.AddWithValue("@Amount", amountPaid);
                                    cmd.Parameters.AddWithValue("@Method", paymentMethod);
                                    cmd.Parameters.AddWithValue("@Type", paymentType);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            // 6. The holds have done their job - delete them
                            using (SqlCommand cmd = new SqlCommand(
                                "DELETE FROM TEMP_RESERVATION_HOLD WHERE TempHoldID IN (" + inList + ")", conn, tx))
                            {
                                for (int i = 0; i < _holdIds.Count; i++)
                                    cmd.Parameters.AddWithValue("@h" + i, _holdIds[i]);
                                cmd.ExecuteNonQuery();
                            }

                            tx.Commit();
                        }
                        catch
                        {
                            try { tx.Rollback(); } catch { }
                            throw;   // the outer catch below shows the error message
                        }
                    }
                }

                _holdIds.Clear();
                MessageBox.Show("Reservation confirmed!\nReservation number(s): " + string.Join(", ", reservationIds),
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not save the reservation:\n\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ReleaseHolds();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnCancelR_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to cancel this reservation? Any unsaved details will be lost.",
                "Cancel Reservation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                ReleaseHolds();
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
       //endregion

    // --- Designer Event Stubs ---
        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e) { }
        private void pnlDetails_Paint(object sender, PaintEventArgs e) { }
        private void cmbBankTransferType_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtNotes_TextChanged(object sender, EventArgs e) { }
        private void cmbPrefix_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtFirstName_TextChanged(object sender, EventArgs e) { }
        private void txtLastName_TextChanged(object sender, EventArgs e) { }
        private void txtPhone_TextChanged(object sender, EventArgs e) { }
        private void txtEmail_TextChanged(object sender, EventArgs e) { }
        private void txtNameOnCard_TextChanged(object sender, EventArgs e) { }
        private void txtExpirationDate_TextChanged(object sender, EventArgs e) { }
        private void txtCVV_TextChanged(object sender, EventArgs e) { }
        private void txtCardNumber_TextChanged(object sender, EventArgs e) { }
        private void dtpBirthdate_ValueChanged(object sender, EventArgs e) { }
        private void txtIDType_TextChanged(object sender, EventArgs e) { }
        private void txtIDNumber_TextChanged(object sender, EventArgs e) { }
        private void txtHomeAddress_TextChanged(object sender, EventArgs e) { }

        


        //endregion
    }
}
