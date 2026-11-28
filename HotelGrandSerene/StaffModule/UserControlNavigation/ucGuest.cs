using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using GrandSereneHotel.DatabaseAccess;
using System.Data.SqlClient;

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
            dgvGuests.ScrollBars = ScrollBars.Vertical;

            dgvGuests.GridColor = Color.FromArgb(231, 229, 255);
            dgvGuests.DefaultCellStyle.ForeColor = Color.FromArgb(30, 41, 59);
            dgvGuests.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 41, 59);
            dgvGuests.DefaultCellStyle.SelectionBackColor = Color.FromArgb(241, 245, 249);
            dgvGuests.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            // 5. Use CellFormatting (EXACT SAME AS UCFINANCE)
            dgvGuests.CellFormatting -= dgvGuests_CellFormatting;
            dgvGuests.CellFormatting += dgvGuests_CellFormatting;
        }
        private const int VipMinStays = 5;   // a guest is "VIP" after this many stays - your group can change it
        private void PopulateData()
        {
            dgvGuests.Rows.Clear();

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT CONCAT(c.FirstName, ' ', c.LastName) AS GuestName,
                       c.Email, c.ContactNumber,
                       lr.RoomNumber, lr.CheckInDate,
                       st.Stays
                FROM CUSTOMER c
                OUTER APPLY (SELECT TOP 1 rm.RoomNumber, r.CheckInDate
                             FROM RESERVATION r JOIN ROOM rm ON rm.RoomID = r.RoomID
                             WHERE r.CustomerID = c.CustomerID AND r.Status <> 'Cancelled'
                             ORDER BY r.CheckInDate DESC, r.ReservationID DESC) lr
                OUTER APPLY (SELECT COUNT(*) AS Stays FROM RESERVATION r
                             WHERE r.CustomerID = c.CustomerID AND r.Status <> 'Cancelled') st
                ORDER BY c.CustomerID DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            AddGuestRow(
                                r.GetString(0),
                                r.IsDBNull(1) ? "" : r.GetString(1),
                                r.IsDBNull(2) ? "" : r.GetString(2),
                                r.IsDBNull(3) ? "" : r.GetString(3),
                                r.IsDBNull(4) ? "" : r.GetDateTime(4).ToString("MMM dd, yyyy"),
                                r.GetInt32(5) >= VipMinStays ? "VIP" : "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load guests:\n\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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

