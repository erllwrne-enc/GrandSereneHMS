using GrandSereneHotel.DatabaseAccess;
using GrandSereneHotel.LoginModule;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HotelGrandSerene.StaffModule.ModalPopup
{
    public partial class frmAccountCreator : Form
    {
        public frmAccountCreator()
        {
            InitializeComponent();
            this.Load += frmAccountCreator_Load;     // <-- the missing connection
        }

        private void frmAccountCreator_Load(object sender, EventArgs e)
        {
            // The designer disables the username box; turn it on. @staffN is only a suggestion.
            txtUsername.Enabled = true;
            txtUsername.ReadOnly = false;

            cmbRole.Items.Clear();
            foreach (string r in Roles.All) cmbRole.Items.Add(r);
            cmbRole.SelectedIndex = 0;                                                  // Front Desk
            if (cmbAccountStatus.Items.Count > 0) cmbAccountStatus.SelectedIndex = 0;   // Active

            txtUsername.Text = GetNextUsernamePreview();
            txtNewPassword.Text = "Testing";
            txtNewPassword.UseSystemPasswordChar = false;
        }

        private void SetDefaultComboBoxSelections()
        {
            // Select first option ("Receptionist") from designer items if available
            if (cmbRole != null && cmbRole.Items.Count > 0 && cmbRole.SelectedIndex < 0)
            {
                cmbRole.SelectedIndex = 0;
            }

            // Select first option ("Active") from designer items if available
            if (cmbAccountStatus != null && cmbAccountStatus.Items.Count > 0 && cmbAccountStatus.SelectedIndex < 0)
            {
                cmbAccountStatus.SelectedIndex = 0;
            }
        }

        private string GetNextUsernamePreview()
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT ISNULL(MAX(EmployeeID), 0) + 1 FROM EMPLOYEE";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int nextId = Convert.ToInt32(command.ExecuteScalar());
                        return $"@staff{nextId}";
                    }
                }
            }
            catch
            {
                return "@staff1";
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string contactNumber = txtContactNumber.Text.Trim();
            string rawPassword = string.IsNullOrWhiteSpace(txtNewPassword.Text) ? "Testing" : txtNewPassword.Text.Trim();
            string role = cmbRole.SelectedItem?.ToString() ?? Roles.FrontDesk;
            string statusStr = cmbAccountStatus.SelectedItem?.ToString() ?? "Active";
            bool isActive = statusStr.Equals("Active", StringComparison.OrdinalIgnoreCase);

            // Validation Checks
            if (string.IsNullOrEmpty(firstName))
            {
                MessageBox.Show("Please enter a first name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFirstName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("Please enter a last name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLastName.Focus();
                return;
            }

            try
            {
                string hashedPassword = PasswordHelper.HashPassword(rawPassword);
                string createdUsername = "";

                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();

                    // Insert record and retrieve the exact inserted username output
                    string insertQuery = @"
                        INSERT INTO EMPLOYEE (FirstName, LastName, Username, Password, ContactNumber, Role, IsActive)
                        OUTPUT INSERTED.Username
                        VALUES (@FirstName, @LastName, @Username, @Password, @ContactNumber, @Role, @IsActive)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        string targetUsername = string.IsNullOrWhiteSpace(txtUsername.Text)
                        ? GetNextUsernamePreview()
                        : txtUsername.Text.Trim();

                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Username", targetUsername);
                        command.Parameters.AddWithValue("@Password", hashedPassword);
                        command.Parameters.AddWithValue("@ContactNumber", string.IsNullOrEmpty(contactNumber) ? (object)DBNull.Value : contactNumber);
                        command.Parameters.AddWithValue("@Role", role);
                        command.Parameters.AddWithValue("@IsActive", isActive);

                        object insertedValue = command.ExecuteScalar();
                        createdUsername = insertedValue?.ToString() ?? targetUsername;
                    }
                }

                // Show confirmation message with newly created username and clear-text password
                string successMessage = $"Staff Account Successfully Created!\n\n" +
                                       $"Username: {createdUsername}\n" +
                                       $"Password: {rawPassword}\n\n" +
                                       $"Please note down these credentials for initial log in.";

                MessageBox.Show(successMessage, "Account Created", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 2627 || sqlEx.Number == 2601)
            {
                MessageBox.Show("That username is already taken. Please choose another one.",
                    "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to create employee account:\n\n" + ex.Message,
                                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
