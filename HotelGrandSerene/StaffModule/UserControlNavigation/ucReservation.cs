using System;
using System.Drawing;
using System.Windows.Forms;
using GrandSereneHotel.DatabaseAccess;
using System.Data.SqlClient;
using GrandSereneHotel.DashboardModule.MainDocked;

namespace HotelGrandSerene.DashboardModule.UserControlNavigation
{
    public partial class ucReservation : UserControl
    {
        public ucReservation()
        {
            InitializeComponent();
            guna2Button1.Click += (s, e) =>
            {
                frmMainDashboard dash = this.FindForm() as frmMainDashboard;
                if (dash != null) dash.StartNewReservation();
            };
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
            dgvReservation.ScrollBars = ScrollBars.Vertical;    // real data can be longer than 6 rows

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

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // One row per reservation (= one room). Newest first.
                    string sql = @"
                SELECT r.ReservationID,
                       CONCAT(c.FirstName, ' ', c.LastName)                       AS GuestName,
                       CONCAT(rm.RoomNumber, ' ', REPLACE(rt.TypeName, ' Room', '')) AS RoomName,
                       r.CheckInDate,
                       r.CheckOutDate,
                       r.Status,
                       DATEDIFF(DAY, r.CheckInDate, r.CheckOutDate) * rt.PricePerNight AS TotalAmount
                FROM RESERVATION r
                JOIN CUSTOMER  c  ON c.CustomerID  = r.CustomerID
                JOIN ROOM      rm ON rm.RoomID     = r.RoomID
                JOIN ROOM_TYPE rt ON rt.RoomTypeID = rm.RoomTypeID
                ORDER BY r.ReservationDate DESC, r.ReservationID DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            AddReservationRow(
                                "R-" + r.GetInt32(0),
                                r.GetString(1),
                                r.GetString(2),
                                r.GetDateTime(3).ToString("MMM dd, yyyy"),
                                r.GetDateTime(4).ToString("MMM dd, yyyy"),
                                r.GetString(5),
                                "\u20B1" + r.GetDecimal(6).ToString("N2"));   // peso sign
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load reservations:\n\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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
                    case "Checked-In":
                        e.CellStyle.ForeColor = Color.FromArgb(37, 99, 235);   // Blue
                        break;
                    case "Checked-Out":
                        e.CellStyle.ForeColor = Color.FromArgb(100, 116, 139); // Muted gray
                        break;
                    case "Cancelled":
                        e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38);   // Red
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
