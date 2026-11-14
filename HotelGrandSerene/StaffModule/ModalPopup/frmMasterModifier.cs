using GrandSereneHotel.DatabaseAccess;
using GrandSereneHotel.LoginModule;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HotelGrandSerene.StaffModule.ModalPopup
{
    public partial class frmMasterModifier : Form
    {
        // Public properties passed from ucSuperAdmin
        public int TargetEmployeeID { get; set; }
        public string SelectedStaffName { get; set; }
        public string SelectedRole { get; set; }
        public string SelectedStatus { get; set; }
        public string SelectedContact { get; set; }

        public frmMasterModifier()
        {
            InitializeComponent();
        }

        private void frmMasterModifier_Load(object sender, EventArgs e)
        {
            // 1. Populate Dropdown Options
            PopulateComboBoxOptions();

            // 2. Set Placeholders & Field States
            ConfigureControlPlaceholders();

            // 3. Load database values or fallback to passed properties
            if (TargetEmployeeID > 0)
            {
                FetchAndLoadStaffDetails(TargetEmployeeID);
            }
            else
            {
                PopulateFieldsFromProperties();
            }
        }

        private void PopulateComboBoxOptions()
        {
            // Populate Role ComboBox
            if (cmbRole != null)
            {
                cmbRole.Items.Clear();
                cmbRole.Items.Add("Receptionist");
                cmbRole.Items.Add("Housekeeping");
                cmbRole.Items.Add("Manager");
                cmbRole.Items.Add("Super Admin");
            }

            // Populate Account Status ComboBox
            if (cmbAccountStatus != null)
            {
                cmbAccountStatus.Items.Clear();
                cmbAccountStatus.Items.Add("Active");
                cmbAccountStatus.Items.Add("Inactive");
            }
        }

        private void ConfigureControlPlaceholders()
        {
            // Lock Username field from editing
            txtUsername.ReadOnly = true;

            // Attempt setting Guna2TextBox PlaceholderText dynamically if using Guna2 controls
            SetPlaceholderText(txtFirstName, "First Name");
            SetPlaceholderText(txtLastName, "Last Name");
            SetPlaceholderText(txtUsername, "Username");
            SetPlaceholderText(txtContactNumber, "Contact Number");
            SetPlaceholderText(txtNewPassword, "New Password (Optional)");
        }

        private void SetPlaceholderText(Control control, string placeholder)
        {
            if (control == null) return;

            // Reflection check for Guna2TextBox PlaceholderText property
            var prop = control.GetType().GetProperty("PlaceholderText");
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(control, placeholder, null);
            }
        }

        private void FetchAndLoadStaffDetails(int employeeId)
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = @"
                        SELECT FirstName, LastName, Username, ContactNumber, Role, ISNULL(IsActive, 1) AS IsActive
                        FROM EMPLOYEE
                        WHERE EmployeeID = @EmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", employeeId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtFirstName.Text = reader["FirstName"].ToString();
                                txtLastName.Text = reader["LastName"].ToString();
                                txtUsername.Text = reader["Username"].ToString();
                                txtContactNumber.Text = reader["ContactNumber"] != DBNull.Value ? reader["ContactNumber"].ToString() : "";

                                string role = reader["Role"].ToString();
                                bool isActive = Convert.ToBoolean(reader["IsActive"]);

                                SetComboBoxSelected(cmbRole, role);
                                SetComboBoxSelected(cmbAccountStatus, isActive ? "Active" : "Inactive");
                            }
                        }
                    }
                }
            }
            catch
            {
                PopulateFieldsFromProperties();
            }

            txtNewPassword.Text = "";
        }

        private void PopulateFieldsFromProperties()
        {
            if (!string.IsNullOrEmpty(SelectedStaffName))
            {
                string[] parts = SelectedStaffName.Split(new[] { ' ' }, 2);
                txtFirstName.Text = parts.Length > 0 ? parts[0] : "";
                txtLastName.Text = parts.Length > 1 ? parts[1] : "";
            }

            txtContactNumber.Text = SelectedContact ?? "";
            SetComboBoxSelected(cmbRole, SelectedRole);
            SetComboBoxSelected(cmbAccountStatus, SelectedStatus ?? "Active");
            txtNewPassword.Text = "";
        }

        private void SetComboBoxSelected(ComboBox cmb, string value)
        {
            if (cmb == null || string.IsNullOrEmpty(value)) return;

            foreach (var item in cmb.Items)
            {
                if (item.ToString().Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    cmb.SelectedItem = item;
                    return;
                }
            }

            if (cmb.Items.Count > 0 && cmb.SelectedIndex < 0)
            {
                cmb.SelectedIndex = 0;
            }
        }

        // --- SAVE CHANGES BUTTON ---
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (TargetEmployeeID <= 0)
            {
                MessageBox.Show("Invalid staff record selected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string selectedRole = cmbRole.SelectedItem?.ToString() ?? "Receptionist";

                    string query = @"
                UPDATE EMPLOYEE 
                SET FirstName = @FirstName,
                    LastName = @LastName,
                    ContactNumber = @ContactNumber,
                    Role = @Role
                WHERE EmployeeID = @EmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                        command.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                        command.Parameters.AddWithValue("@ContactNumber", txtContactNumber.Text.Trim());
                        command.Parameters.AddWithValue("@Role", selectedRole);
                        command.Parameters.AddWithValue("@EmployeeID", TargetEmployeeID);

                        command.ExecuteNonQuery();
                    }

                    // Password reset using the correct 'Password' column name
                    if (!string.IsNullOrWhiteSpace(txtNewPassword.Text))
                    {
                        string passwordHash = PasswordHelper.HashPassword(txtNewPassword.Text.Trim());
                        string pwdQuery = "UPDATE EMPLOYEE SET Password = @Password WHERE EmployeeID = @EmployeeID";

                        using (SqlCommand pwdCmd = new SqlCommand(pwdQuery, connection))
                        {
                            pwdCmd.Parameters.AddWithValue("@Password", passwordHash);
                            pwdCmd.Parameters.AddWithValue("@EmployeeID", TargetEmployeeID);
                            pwdCmd.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("Staff details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save changes to database:\n\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // --- CANCEL BUTTON (Closes modal without saving) ---
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // --- DEACTIVATE BUTTON (Acts as Cancel/Close for now) ---
        private void btnDeactivate_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // --- TOP RIGHT CLOSE ICON (x) ---
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
