using System;
using System.Drawing;
using System.Windows.Forms;

namespace HotelGrandSerene.DashboardModule.UserControlNavigation
{
    public partial class ucReservation : UserControl
    {
        public ucReservation()
        {
            InitializeComponent();

            ConfigureGrid();
            LoadReservationData();
        }

        private void ConfigureGrid()
        {
            // 1. Column Header Setup & Titles
            dgvReservation.AutoGenerateColumns = false;
            dgvReservation.ColumnHeadersVisible = false;
            dgvReservation.ColumnHeadersHeight = 42;

            if (dgvReservation.Columns.Count >= 7)
            {
                dgvReservation.Columns[0].HeaderText = "ID";
                dgvReservation.Columns[1].HeaderText = "GUEST";
                dgvReservation.Columns[2].HeaderText = "ROOM";
                dgvReservation.Columns[3].HeaderText = "CHECK-IN";
                dgvReservation.Columns[4].HeaderText = "CHECK-OUT";
                dgvReservation.Columns[5].HeaderText = "STATUS";
                dgvReservation.Columns[6].HeaderText = "AMOUNT";
            }

            // 2. CRITICAL: Horizontal-Only Grid Lines (Zero Vertical Lines)
            dgvReservation.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvReservation.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvReservation.AllowUserToResizeColumns = false;

            dgvReservation.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            
            dgvReservation.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;

            // 3. Header Styling matching Figma design
            Color lightHeaderBg = Color.FromArgb(248, 250, 252);
            Color headerTextColor = Color.FromArgb(148, 163, 184); // Muted gray text

            dgvReservation.ThemeStyle.HeaderStyle.BackColor = lightHeaderBg;
            dgvReservation.ThemeStyle.HeaderStyle.ForeColor = headerTextColor;
            dgvReservation.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);

            // 4. Separator Line Colors & Row Heights
            Color rowSeparatorColor = Color.FromArgb(231, 229, 255);
            dgvReservation.GridColor = rowSeparatorColor;
            
            

            dgvReservation.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvReservation.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;

            dgvReservation.RowTemplate.Height = 52;
            dgvReservation.ScrollBars = ScrollBars.None;

            // 5. Interaction Rules
            dgvReservation.AllowUserToAddRows = false;
            dgvReservation.AllowUserToDeleteRows = false;
            dgvReservation.ReadOnly = true;
            dgvReservation.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReservation.MultiSelect = false;

            dgvReservation.DefaultCellStyle.ForeColor = Color.FromArgb(30, 41, 59);
            dgvReservation.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 41, 59);
            dgvReservation.DefaultCellStyle.SelectionBackColor = Color.FromArgb(241, 245, 249);
            dgvReservation.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            // 6. Bind Cell Formatting
            dgvReservation.CellFormatting -= dgvReservation_CellFormatting;
            dgvReservation.CellFormatting += dgvReservation_CellFormatting;
        }

        private void LoadReservationData()
        {
            dgvReservation.Rows.Clear();

            // Populate exact 6 records from the Figma mockup:
            // (ID, GUEST, ROOM, CHECK-IN, CHECK-OUT, STATUS, AMOUNT)
            AddReservationRow("R-1042", "Ron Palay", "101 Standard", "Aug 14", "Aug 17", "Confirmed", "$420");
            AddReservationRow("R-1043", "Chester Peralta", "205 Deluxe", "Aug 14", "Aug 16", "Confirmed", "$380");
            AddReservationRow("R-1044", "Ginger Rubin", "312 Suite", "Aug 14", "Aug 19", "VIP", "$1,250");
            AddReservationRow("R-1045", "Marjorie Bayonbon", "418 Executive", "Aug 14", "Aug 15", "Pending", "$210");
            AddReservationRow("R-1046", "Nayomi Esguerra", "109 Standard", "Aug 12", "Aug 14", "Checkout", "$280");
            AddReservationRow("R-1047", "Iversonne Cornoda", "214 Deluxe", "Aug 11", "Aug 14", "Checkout", "$540");

            dgvReservation.ClearSelection();
        }

        private void AddReservationRow(string id, string guest, string room, string checkIn, string checkOut, string status, string amount)
        {
            int rowIndex = dgvReservation.Rows.Add();
            DataGridViewRow row = dgvReservation.Rows[rowIndex];

            row.Cells[0].Value = id;
            row.Cells[1].Value = guest;
            row.Cells[2].Value = room;
            row.Cells[3].Value = checkIn;
            row.Cells[4].Value = checkOut;
            row.Cells[5].Value = status;

            if (dgvReservation.Columns.Count > 6)
            {
                row.Cells[6].Value = amount;
            }
        }

        private void dgvReservation_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            // Column 0: ID -> Muted Slate Gray Text
            if (e.ColumnIndex == 0)
            {
                e.CellStyle.ForeColor = Color.FromArgb(148, 163, 184);
                e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            }
            // Column 1: GUEST -> Bold Dark Slate Text
            else if (e.ColumnIndex == 1)
            {
                e.CellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                e.CellStyle.ForeColor = Color.FromArgb(30, 41, 59);
            }
            // Column 5: STATUS -> Dynamic colors based on mockup
            else if (e.ColumnIndex == 5)
            {
                string status = e.Value.ToString().Trim();
                e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

                switch (status)
                {
                    case "Confirmed":
                        e.CellStyle.ForeColor = Color.FromArgb(16, 185, 129); // Green
                        break;
                    case "VIP":
                        e.CellStyle.ForeColor = Color.FromArgb(217, 119, 6);  // Amber/Gold
                        break;
                    case "Pending":
                        e.CellStyle.ForeColor = Color.FromArgb(37, 99, 235);  // Blue
                        break;
                    case "Checkout":
                        e.CellStyle.ForeColor = Color.FromArgb(100, 116, 139); // Muted Gray
                        break;
                }
            }
            // Column 6: AMOUNT -> Bold Dark Slate Text
            else if (e.ColumnIndex == 6)
            {
                e.CellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                e.CellStyle.ForeColor = Color.FromArgb(30, 41, 59);
            }
        }

        // Empty event stubs required by ucReservation.Designer.cs
        private void dgvReservation_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void ucReservation_Load(object sender, EventArgs e) { }
    }
}
