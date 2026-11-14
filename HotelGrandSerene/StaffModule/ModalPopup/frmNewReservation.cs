using GrandSereneHotel.DatabaseAccess;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

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
            // 1. Basic Field Validation
            if (string.IsNullOrWhiteSpace(txtFirstName?.Text) || string.IsNullOrWhiteSpace(txtLastName?.Text))
            {
                MessageBox.Show("Please enter the customer's First and Last Name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone?.Text))
            {
                MessageBox.Show("Please enter a contact phone number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();

                    string firstName = txtFirstName.Text.Trim();
                    string lastName = txtLastName.Text.Trim();
                    string contactNumber = txtPhone.Text.Trim();
                    string email = txtEmail?.Text?.Trim() ?? "";
                    string address = txtHomeAddress?.Text?.Trim() ?? "N/A";
                    string idType = txtIDType?.Text?.Trim() ?? "Standard ID";
                    string idNumber = txtIDNumber?.Text?.Trim() ?? "N/A";

                    // Update existing rows in TEMP_RESERVATION_HOLD (created during Step 1) with customer info
                    string updateHoldQuery = @"
                        UPDATE TEMP_RESERVATION_HOLD 
                        SET FirstName = @FirstName, 
                            LastName = @LastName, 
                            ContactNumber = @ContactNumber, 
                            Email = @Email, 
                            Address = @Address, 
                            IDType = @IDType, 
                            IDNumber = @IDNumber";

                    using (SqlCommand cmdUpdate = new SqlCommand(updateHoldQuery, connection))
                    {
                        cmdUpdate.Parameters.AddWithValue("@FirstName", firstName);
                        cmdUpdate.Parameters.AddWithValue("@LastName", lastName);
                        cmdUpdate.Parameters.AddWithValue("@ContactNumber", contactNumber);
                        cmdUpdate.Parameters.AddWithValue("@Email", email);
                        cmdUpdate.Parameters.AddWithValue("@Address", address);
                        cmdUpdate.Parameters.AddWithValue("@IDType", idType);
                        cmdUpdate.Parameters.AddWithValue("@IDNumber", idNumber);

                        cmdUpdate.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Reservation details successfully thrown into the temporary hold table!", "Staged Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while pushing data to the temporary hold table:\n\n" + ex.Message,
                                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
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
