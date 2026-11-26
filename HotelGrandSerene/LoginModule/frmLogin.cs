using GrandSereneHotel.DashboardModule.MainDocked; // Namespace for frmMainDashboard
using GrandSereneHotel.DatabaseAccess; // Namespace for DatabaseHelper
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrandSereneHotel.LoginModule
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();

            // Mask password characters
            txtbPassword.UseSystemPasswordChar = true;

            // Set Enter key to trigger sign-in button automatically
            this.AcceptButton = btnSignin;

           
        }

        private void btnSignin_Click(object sender, EventArgs e)
        {
            PerformLogin();
        }


        private bool _busy;   // stays true after a successful login so it can never run twice

        private void PerformLogin()
        {
            if (_busy) return;
            _busy = true;

            string username = txtbUsername.Text.Trim();
            string password = txtbPassword.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter your username.", "Login Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtbUsername.Focus();
                _busy = false;
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter your password.", "Login Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtbPassword.Focus();
                _busy = false;
                return;
            }

            string error;
            if (ValidateUserCredentials(username, password, out error))
            {
                // No "Welcome" pop-up any more: the dashboard opening is the confirmation.
                frmMainDashboard dashboard = new frmMainDashboard();
                this.Hide();
                txtbUsername.Clear();
                txtbPassword.Clear();
                dashboard.Show();
                // _busy intentionally stays true: after Sign Out a NEW frmLogin is created.
            }
            else
            {
                MessageBox.Show(error, "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtbPassword.Clear();
                txtbPassword.Focus();
                _busy = false;
            }
        }

        private bool ValidateUserCredentials(string username, string password, out string error)
        {
            error = "Invalid username or password. Please try again.";
            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(
                        @"SELECT EmployeeID, FirstName, LastName, Role, Password, IsActive
                  FROM EMPLOYEE WHERE Username = @Username", connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read()) return false;

                            int id = reader.GetInt32(0);
                            string fullName = reader.GetString(1) + " " + reader.GetString(2);
                            string role = reader.GetString(3);
                            string storedHash = reader.GetString(4);
                            bool isActive = reader.GetBoolean(5);

                            if (!PasswordHelper.VerifyPassword(password, storedHash)) return false;

                            if (!isActive)
                            {
                                error = "This account has been deactivated. Please contact an administrator.";
                                return false;
                            }

                            CurrentUser.EmployeeId = id;
                            CurrentUser.FullName = fullName;
                            CurrentUser.Role = role;
                            return true;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                error = "Unable to access the employee database.\n\n" + ex.Message;
                return false;
            }
        }
        // --- Event Stubs Preserved for Designer Compatibility ---
        private void txtbUsername_TextChanged(object sender, EventArgs e) { }
        private void guna2HtmlLabel2_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel5_Click(object sender, EventArgs e) { }
        private void txtbPassword_TextChanged(object sender, EventArgs e) { }
        private void btnCreateFirstAdmin_Click(object sender, EventArgs e) { }
    }
}
