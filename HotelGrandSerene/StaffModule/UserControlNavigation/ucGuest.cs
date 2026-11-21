using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace HotelGrandSerene.CustomerModule.ucCustomerpnl
{
    public partial class ucGuest : UserControl
    {
        public ucGuest()
        {
            InitializeComponent();

            // Setup grid matching ucFinance exactly
            ConfigureGrid();
            PopulateData();
        }

        private void ConfigureGrid()
        {
            // 1. Same exact property configuration as ucFinance
            dgvGuests.AutoGenerateColumns = false;
            dgvGuests.ColumnHeadersVisible = false;

            // 2. Exact property that hides vertical lines in ucFinance
            dgvGuests.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvGuests.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvGuests.AllowUserToResizeColumns = false;

            // 3. Grid behavior rules
            dgvGuests.AllowUserToAddRows = false;
            dgvGuests.AllowUserToDeleteRows = false;
            dgvGuests.ReadOnly = true;
            dgvGuests.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGuests.MultiSelect = false;

            // 4. Dimensions and Colors matching ucFinance
            dgvGuests.RowTemplate.Height = 52;
            dgvGuests.ScrollBars = ScrollBars.None;

            dgvGuests.GridColor = Color.FromArgb(231, 229, 255);
            dgvGuests.DefaultCellStyle.ForeColor = Color.FromArgb(30, 41, 59);
            dgvGuests.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 41, 59);
            dgvGuests.DefaultCellStyle.SelectionBackColor = Color.FromArgb(241, 245, 249);
            dgvGuests.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            // 5. Use CellFormatting (EXACT SAME AS UCFINANCE)
            dgvGuests.CellFormatting -= dgvGuests_CellFormatting;
            dgvGuests.CellFormatting += dgvGuests_CellFormatting;
        }

        private void PopulateData()
        {
            dgvGuests.Rows.Clear();

            AddGuestRow("Sydney Chua", "demo@email.com", "09123456789", "101", "Aug 14", "");
            AddGuestRow("Shairyll Licunan", "demo@email.com", "09123456789", "205", "Aug 14", "VIP");
            AddGuestRow("Josh Herrera", "demo@email.com", "09123456789", "312", "Aug 14", "VIP");
            AddGuestRow("Nathaniel Lalota", "demo@email.com", "09123456789", "418", "Aug 14", "");
            AddGuestRow("Ana Laboc", "demo@email.com", "09123456789", "214", "Aug 11", "");
            AddGuestRow("Ephraim Perlas", "demo@email.com", "09123456789", "109", "Aug 14", "");

            dgvGuests.ClearSelection();
        }

        private void AddGuestRow(string name, string email, string phone, string room, string checkIn, string vipStatus)
        {
            int rowIndex = dgvGuests.Rows.Add();
            DataGridViewRow row = dgvGuests.Rows[rowIndex];

            row.Cells[0].Value = name;
            row.Cells[1].Value = email;
            row.Cells[2].Value = phone;
            row.Cells[3].Value = room;
            row.Cells[4].Value = checkIn;

            if (dgvGuests.Columns.Count > 5)
            {
                row.Cells[5].Value = vipStatus;
            }
        }

        private void dgvGuests_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            // Column 0 (GUEST Name) - Bold Slate
            if (e.ColumnIndex == 0)
            {
                e.CellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                e.CellStyle.ForeColor = Color.FromArgb(30, 41, 59);
            }
            // Column 5 (VIP Badge Column) - Bold Gold Text
            else if (e.ColumnIndex == 5 && e.Value.ToString() == "VIP")
            {
                e.CellStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                e.CellStyle.ForeColor = Color.FromArgb(180, 83, 9);
                e.CellStyle.BackColor = Color.FromArgb(254, 243, 199);
            }
        }

        private void dgvGuests_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void ucGuest_Load(object sender, EventArgs e) { }
    }
}

