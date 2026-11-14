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

            // Wire KeyDown events for smooth Enter-key execution
            if (txtbUsername != null)
            {
                txtbUsername.KeyDown -= txtbUsername_KeyDown;
                txtbUsername.KeyDown += txtbUsername_KeyDown;
            }

            if (txtbPassword != null)
            {
                txtbPassword.KeyDown -= txtbPassword_KeyDown;
                txtbPassword.KeyDown += txtbPassword_KeyDown;
            }
        }

        private void btnSignin_Click(object sender, EventArgs e)
        {
            PerformLogin();
        }

        private void txtbUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent Windows beep sound
                PerformLogin();
            }
        }

        private void txtbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent Windows beep sound
                PerformLogin();
            }
        }

        private void PerformLogin()
        {
            string username = txtbUsername.Text.Trim();
            string password = txtbPassword.Text.Trim();

            // 1. Input Validation
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter your username.", "Login Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtbUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter your password.", "Login Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtbPassword.Focus();
                return;
            }

            // 2. Security / Authentication Check
            if (ValidateUserCredentials(username, password))
            {
                MessageBox.Show("Login successful! Welcome to Hotel Grand Serene.", "Access Granted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 3. Open Main Dashboard first
                frmMainDashboard dashboard = new frmMainDashboard();

                // Hide login window
                this.Hide();

                // 4. Clear textboxes AFTER hiding so TextChanged events don't trigger validation popups
                txtbUsername.Clear();
                txtbPassword.Clear();

                // Show dashboard
                dashboard.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtbPassword.Clear();
                txtbPassword.Focus();
            }
        }


        /// <summary>
        /// Validates username and password against allowed credentials.
        /// </summary>
        private bool ValidateUserCredentials(string username, string password)
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT Password 
                        FROM EMPLOYEE 
                        WHERE Username = @Username";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        object result = command.ExecuteScalar();
                        if (result == null || result == DBNull.Value) return false;

                        string storedHash = result.ToString();
                        return PasswordHelper.VerifyPassword(password, storedHash);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Unable to access the employee database.\n\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
