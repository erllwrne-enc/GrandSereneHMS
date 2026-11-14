using GrandSereneHotel.LoginModule;
using System;
using System.Windows.Forms;

namespace HotelGrandSerene.LoginModule
{
    public partial class frmAdministratorInitialize : Form
    {
        public frmAdministratorInitialize()
        {
            InitializeComponent();
        }

        private void btnInitialize_Click(object sender, EventArgs e)
        {
            string idNumber = txtbIdNumber.Text.Trim();
            string firstName = txtbFirstName.Text.Trim();
            string lastName = txtbLastName.Text.Trim();
            string username = txtbUsername.Text.Trim(); 
            string contactNumber = txtbContactNumber.Text.Trim();
            string password = txtbCreatePassword.Text.Trim();
            string confirmPassword = txtbConfirmPassword.Text.Trim();

            // 1. Validation for all required fields
            if (string.IsNullOrEmpty(idNumber) || string.IsNullOrEmpty(firstName) ||
                string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill out all required fields.",
                                "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please re-enter your password.",
                                "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtbConfirmPassword.Clear();
                txtbConfirmPassword.Focus();
                return;
            }

            
            try
            {
                EmployeeSetup.CreateFirstAdmin(firstName, lastName, username, password, string.Empty);

                MessageBox.Show($"Administrator initialized successfully!\n\nYour Username is: {username}\n\nYou will now be redirected to the login screen.",
                                "Setup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Admin Setup Failed:\n\n" + ex.Message,
                                "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // UNESD
        private void frmAdministratorInitialize_Load(object sender, EventArgs e) { }
        private void guna2HtmlLabel5_Click(object sender, EventArgs e) { }
        private void TxtbIdNumber_TextChanged(object sender, EventArgs e) { }
        private void txtbFullName_TextChanged(object sender, EventArgs e) { }
        private void txtbCreatePassword_TextChanged(object sender, EventArgs e) { }
        private void txtbConfirmPassword_TextChanged(object sender, EventArgs e) { }
        private void guna2HtmlLabel4_Click(object sender, EventArgs e) { }
        private void guna2TextBox1_TextChanged(object sender, EventArgs e) { }
        private void txtbFirstName_TextChanged(object sender, EventArgs e) { }
        private void txtbLastName_TextChanged(object sender, EventArgs e) { }
        private void txtbContactNumber_TextChanged(object sender, EventArgs e) { }
        
    }
}