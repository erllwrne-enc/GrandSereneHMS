using GrandSereneHotel.DatabaseAccess;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace HotelGrandSerene.DashboardModule.UserControlNavigation
{
    public partial class ucStaff : UserControl
    {
        public ucStaff()
        {
            InitializeComponent();
            ConfigureGrid();

            // Wire up real-time search filter handler
            if (txtbNameFilter != null)
            {
                txtbNameFilter.TextChanged -= txtbNameFilter_TextChanged;
                txtbNameFilter.TextChanged += txtbNameFilter_TextChanged;
            }

            // Execute data load directly on control creation
            LoadStaffData();
        }

        private void ucStaff_Load(object sender, EventArgs e)
        {
            LoadStaffData();
        }

        private void ConfigureGrid()
        {
            // 1. Prevent auto-generating columns & hide native header bar
            dgvStaff.AutoGenerateColumns = false;
            dgvStaff.ColumnHeadersVisible = false;

            // 2. Exact properties that disable vertical lines and enable horizontal lines only
            dgvStaff.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvStaff.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvStaff.AllowUserToResizeColumns = false;

            // 3. Behavioral settings
            dgvStaff.AllowUserToAddRows = false;
            dgvStaff.AllowUserToDeleteRows = false;
            dgvStaff.ReadOnly = true;
            dgvStaff.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStaff.MultiSelect = false;

            // 4. Dimensions and line colors matching ucFinance
            dgvStaff.RowTemplate.Height = 52;
            dgvStaff.ScrollBars = ScrollBars.None;
            dgvStaff.GridColor = Color.FromArgb(231, 229, 255);
            dgvStaff.DefaultCellStyle.ForeColor = Color.FromArgb(30, 41, 59);
            dgvStaff.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 41, 59);
            dgvStaff.DefaultCellStyle.SelectionBackColor = Color.FromArgb(241, 245, 249);
            dgvStaff.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            // 5. Lightweight CellFormatting
            dgvStaff.CellFormatting -= dgvStaff_CellFormatting;
            dgvStaff.CellFormatting += dgvStaff_CellFormatting;
        }

        public void LoadStaffData()
        {
            if (dgvStaff.InvokeRequired)
            {
                this.Invoke(new Action(LoadStaffData));
                return;
            }

            dgvStaff.Rows.Clear();

            int totalStaff = 0;
            int onDutyCount = 0;
            int offDutyCount = 0;

            string filterKeyword = txtbNameFilter != null ? txtbNameFilter.Text.Trim() : "";

            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();

                    // Query supports filtering by First Name, Last Name, or Role
                    string query = @"
                        SELECT 
                            EmployeeID,
                            CONCAT(FirstName, ' ', LastName) AS StaffName,
                            Role,
                            ISNULL(IsActive, 1) AS IsActive
                        FROM EMPLOYEE
                        WHERE (@Filter = '' 
                           OR FirstName LIKE @FilterPattern 
                           OR LastName LIKE @FilterPattern 
                           OR Role LIKE @FilterPattern)
                        ORDER BY EmployeeID ASC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Filter", filterKeyword);
                        command.Parameters.AddWithValue("@FilterPattern", $"%{filterKeyword}%");

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int employeeId = Convert.ToInt32(reader["EmployeeID"]);
                                string staffName = reader["StaffName"].ToString();
                                string role = reader["Role"].ToString();
                                string shift = "Morning";
                                string sinceYear = "2024";

                                bool isActive = Convert.ToBoolean(reader["IsActive"]);
                                string status = isActive ? "On Duty" : "Off Duty";

                                totalStaff++;
                                if (isActive) onDutyCount++;
                                else offDutyCount++;

                                int rowIndex = dgvStaff.Rows.Add(staffName, role, shift, sinceYear, status);
                                dgvStaff.Rows[rowIndex].Tag = employeeId;
                            }
                        }
                    }
                }

                // Update summary dashboard counter labels as standard integers
                if (lblTotalStaff != null) lblTotalStaff.Text = totalStaff.ToString();
                if (lblOnDutyCount != null) lblOnDutyCount.Text = onDutyCount.ToString();
                if (lblOffDutyCount != null) lblOffDutyCount.Text = offDutyCount.ToString();

                dgvStaff.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load dashboard staff records from database:\n\n" + ex.Message,
                                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- REAL-TIME SEARCH EVENT HANDLER ---
        private void txtbNameFilter_TextChanged(object sender, EventArgs e)
        {
            LoadStaffData();
        }

        private void dgvStaff_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            // Column 0: STAFF MEMBER Name -> Bold Solid Slate
            if (e.ColumnIndex == 0)
            {
                e.CellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                e.CellStyle.ForeColor = Color.FromArgb(30, 41, 59);
            }
            // Column 4: STATUS -> "On Duty" (Emerald Green) vs "Off Duty" (Slate Gray)
            else if (e.ColumnIndex == 4)
            {
                string status = e.Value.ToString();
                bool isOnDuty = status.Equals("On Duty", StringComparison.OrdinalIgnoreCase);
                e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                e.CellStyle.ForeColor = isOnDuty ? Color.FromArgb(16, 185, 129) : Color.FromArgb(100, 116, 139);
            }
        }

        // Event stubs linked by ucStaff.Designer.cs
        private void lblOffDutyCount_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel3_Click(object sender, EventArgs e) { }
        private void dgvStaff_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void guna2HtmlLabel1_Click(object sender, EventArgs e) { }
        private void lblOnDutyCount_Click(object sender, EventArgs e) { }
        private void lblTotalStaff_Click(object sender, EventArgs e) { }
    }
}
