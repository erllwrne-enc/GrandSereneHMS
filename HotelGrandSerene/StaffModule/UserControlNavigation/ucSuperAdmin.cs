using GrandSereneHotel.DatabaseAccess;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using HotelGrandSerene.StaffModule.ModalPopup;

namespace HotelGrandSerene.StaffModule.UserControlNavigation
{
    public partial class ucSuperAdmin : UserControl
    {
        private ContextMenuStrip cmsStaffGrid;

        public ucSuperAdmin()
        {
            InitializeComponent();

            // Bind grid event handlers safely
            dgvStaff.CellFormatting -= dgvStaff_CellFormatting;
            dgvStaff.CellFormatting += dgvStaff_CellFormatting;

            dgvStaff.CellMouseClick -= dgvStaff_CellMouseClick;
            dgvStaff.CellMouseClick += dgvStaff_CellMouseClick;

            dgvStaff.CellDoubleClick -= dgvStaff_CellDoubleClick;
            dgvStaff.CellDoubleClick += dgvStaff_CellDoubleClick;

            InitializeStaffContextMenu();
        }

        private void ucSuperAdmin_Load(object sender, EventArgs e)
        {
            LoadStaffData();
        }

        private void InitializeStaffContextMenu()
        {
            cmsStaffGrid = new ContextMenuStrip();

            ToolStripMenuItem itemEditStaff = new ToolStripMenuItem("Edit Staff Details");
            itemEditStaff.Click += itemEditStaff_Click;

            cmsStaffGrid.Items.Add(itemEditStaff);
        }

        private void dgvStaff_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvStaff.ClearSelection();
                dgvStaff.Rows[e.RowIndex].Selected = true;

                Point mousePosition = dgvStaff.PointToClient(Cursor.Position);
                cmsStaffGrid.Show(dgvStaff, mousePosition);
            }
        }

        private void dgvStaff_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            dgvStaff.ClearSelection();
            dgvStaff.Rows[e.RowIndex].Selected = true;
            itemEditStaff_Click(sender, EventArgs.Empty);
        }

        private void itemEditStaff_Click(object sender, EventArgs e)
        {
           

            if (dgvStaff.SelectedRows.Count == 0) return;

            DataGridViewRow selectedRow = dgvStaff.SelectedRows[0];

            // 1. Extract values from the right-clicked row
            int employeeId = selectedRow.Tag != null ? Convert.ToInt32(selectedRow.Tag) : 0;
            string staffName = GetCellValue(selectedRow, "colStaffMember", 0);
            string role = GetCellValue(selectedRow, "colRole", 1);
            string status = GetCellValue(selectedRow, "colAccountStatus", 3);
            string contact = GetCellValue(selectedRow, "colContact", 4);

            // 2. Launch dark backdrop overlay
            using (Form overlay = new Form())
            {
                overlay.StartPosition = FormStartPosition.Manual;
                overlay.FormBorderStyle = FormBorderStyle.None;
                overlay.Opacity = 0.50d; // 50% dark overlay
                overlay.BackColor = Color.Black;
                overlay.WindowState = FormWindowState.Maximized;
                overlay.ShowInTaskbar = false;
                overlay.Show();

                // 3. Launch modal popup centered over dark overlay
                using (frmMasterModifier modifierModal = new frmMasterModifier())
                {
                    modifierModal.TargetEmployeeID = employeeId;
                    modifierModal.SelectedStaffName = staffName;
                    modifierModal.SelectedRole = role;
                    modifierModal.SelectedStatus = status;
                    modifierModal.SelectedContact = contact;

                    modifierModal.StartPosition = FormStartPosition.CenterScreen;

                    // 4. If user saves changes, auto-refresh dgvStaff
                    if (modifierModal.ShowDialog(overlay) == DialogResult.OK)
                    {
                        LoadStaffData();
                    }
                }

                overlay.Close();
            }
        }

        private string GetCellValue(DataGridViewRow row, string colName, int defaultIndex)
        {
            if (dgvStaff.Columns.Contains(colName))
                return row.Cells[colName].Value?.ToString() ?? "";

            if (row.Cells.Count > defaultIndex)
                return row.Cells[defaultIndex].Value?.ToString() ?? "";

            return "";
        }

        // Action button to trigger account creation modal
        private void btnModifyAddAcc_Click(object sender, EventArgs e)
        {
            if (dgvStaff.SelectedRows.Count > 0)
            {
                string name = GetCellValue(dgvStaff.SelectedRows[0], "colStaffMember", 0);
                DialogResult choice = MessageBox.Show(
                    "Do you want to EDIT \"" + name + "\"?\n\nYes = edit this account\nNo = create a NEW account\nCancel = do nothing",
                    "Modify or Add Account", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (choice == DialogResult.Cancel) return;
                if (choice == DialogResult.Yes) { itemEditStaff_Click(sender, e); return; }
                dgvStaff.ClearSelection();   // "No": fall through and create a new account
            }
            using (Form overlay = new Form())
            {
                overlay.StartPosition = FormStartPosition.Manual;
                overlay.FormBorderStyle = FormBorderStyle.None;
                overlay.Opacity = 0.50d;
                overlay.BackColor = Color.Black;
                overlay.WindowState = FormWindowState.Maximized;
                overlay.ShowInTaskbar = false;
                overlay.Show();

                using (frmAccountCreator creatorModal = new frmAccountCreator())
                {
                    creatorModal.StartPosition = FormStartPosition.CenterScreen;

                    if (creatorModal.ShowDialog(overlay) == DialogResult.OK)
                    {
                        // Automatically refresh staff grid after adding new employee
                        LoadStaffData();
                    }
                }

                overlay.Close();
            }
        }

        public void LoadStaffData()
        {
            if (dgvStaff.InvokeRequired)
            {
                this.Invoke(new Action(LoadStaffData));
                return;
            }

            dgvStaff.Rows.Clear();

            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            EmployeeID,
                            CONCAT(FirstName, ' ', LastName) AS StaffName,
                            Role,
                            ISNULL(ContactNumber, 'N/A') AS ContactNumber,
                            ISNULL(IsActive, 1) AS IsActive
                        FROM EMPLOYEE
                        ORDER BY EmployeeID ASC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int employeeId = Convert.ToInt32(reader["EmployeeID"]);
                                string staffName = reader["StaffName"].ToString();
                                string role = reader["Role"].ToString();
                                string shift = "Day Shift";
                                string status = Convert.ToBoolean(reader["IsActive"]) ? "Active" : "Inactive";
                                string contact = reader["ContactNumber"].ToString();

                                AddStaffRow(employeeId, staffName, role, shift, status, contact);
                            }
                        }
                    }
                }

                dgvStaff.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load staff records from database:\n\n" + ex.Message,
                                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddStaffRow(int employeeId, string staffName, string role, string shift, string status, string contact)
        {
            int index = dgvStaff.Rows.Add();
            DataGridViewRow row = dgvStaff.Rows[index];

            // Attach EmployeeID to the row's Tag property for querying
            row.Tag = employeeId;

            if (dgvStaff.Columns.Contains("colStaffMember")) row.Cells["colStaffMember"].Value = staffName;
            else row.Cells[0].Value = staffName;

            if (dgvStaff.Columns.Contains("colRole")) row.Cells["colRole"].Value = role;
            else row.Cells[1].Value = role;

            if (dgvStaff.Columns.Contains("colShift")) row.Cells["colShift"].Value = shift;
            else row.Cells[2].Value = shift;

            if (dgvStaff.Columns.Contains("colAccountStatus")) row.Cells["colAccountStatus"].Value = status;
            else row.Cells[3].Value = status;

            if (dgvStaff.Columns.Contains("colContact")) row.Cells["colContact"].Value = contact;
            else row.Cells[4].Value = contact;
        }

        private void dgvStaff_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            if (dgvStaff.Columns[e.ColumnIndex].Name == "colStaffMember" || e.ColumnIndex == 0)
            {
                e.CellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                e.CellStyle.ForeColor = Color.FromArgb(30, 41, 59);
            }
            else if (dgvStaff.Columns[e.ColumnIndex].Name == "colAccountStatus" || e.ColumnIndex == 3)
            {
                string status = e.Value.ToString().Trim();
                e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

                if (status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.ForeColor = Color.FromArgb(16, 185, 129);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38);
                }
            }
        }
        

        private void ucSuperAdmin_Load_1(object sender, EventArgs e)
        {

        }
    }
}
