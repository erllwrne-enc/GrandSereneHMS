using GrandSereneHotel.DatabaseAccess;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;

namespace HotelGrandSerene.DashboardModule.UserControlNavigation
{
    public partial class ucRooms : UserControl
    {
        private ContextMenuStrip cmsRoomStatus;
        private string selectedRoomNumber = string.Empty;
        private readonly List<DateTime?> _dateOptions = new List<DateTime?>();
        public ucRooms()
        {
            InitializeComponent();

            // Bind CellFormatting handler safely
            dgvRoomStatus.CellFormatting -= dgvRoomStatus_CellFormatting;
            dgvRoomStatus.CellFormatting += dgvRoomStatus_CellFormatting;

            
            InitializeStatusContextMenu();
        }

        private void ucRooms_Load(object sender, EventArgs e)
        {
            PopulateComboboxFilters();
            LoadRoomData();
        }

        private void PopulateComboboxFilters()
        {
            // Set up Status filter items
            if (cmbRoomStatus.Items.Count == 0)
            {
                cmbRoomStatus.Items.Add("All Statuses");
                cmbRoomStatus.Items.Add("Available");
                cmbRoomStatus.Items.Add("Occupied");
                cmbRoomStatus.Items.Add("Reserved");
                cmbRoomStatus.Items.Add("Cleaning");
                cmbRoomStatus.Items.Add("Maintenance");
                cmbRoomStatus.SelectedIndex = 0;
            }

            // Populate Room Types dynamically from DB_HMS
            if (cmbRoomType.Items.Count == 0)
            {
                cmbRoomType.Items.Add("All Types");
                try
                {
                    using (SqlConnection connection = DatabaseHelper.GetConnection())
                    {
                        connection.Open();
                        string query = "SELECT DISTINCT TypeName FROM ROOM_TYPE ORDER BY TypeName ASC";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    cmbRoomType.Items.Add(reader["TypeName"].ToString());
                                }
                            }
                        }
                    }
                }
                catch
                {
                    // Fallback defaults if database isn't reached immediately
                    if (cmbRoomType.Items.Count == 1)
                    {
                        cmbRoomType.Items.Add("Standard Room");
                        cmbRoomType.Items.Add("Suite Room");
                    }
                }
                cmbRoomType.SelectedIndex = 0;
                if (guna2ComboBox3.Items.Count == 0)
                {
                    DateTime today = DateTime.Today;

                    guna2ComboBox3.Items.Add("Today (live status)");
                    _dateOptions.Add(null);

                    guna2ComboBox3.Items.Add("Tomorrow");
                    _dateOptions.Add(today.AddDays(1));

                    for (int i = 2; i <= 30; i++)
                    {
                        DateTime d = today.AddDays(i);
                        guna2ComboBox3.Items.Add(d.ToString("ddd, MMM dd"));
                        _dateOptions.Add(d);
                    }

                    guna2ComboBox3.SelectedIndex = 0;
                    guna2ComboBox3.SelectedIndexChanged += (s, e) => ApplyFilters();
                }
            }
        }

        private DateTime? SelectedFilterDate()
        {
            int i = guna2ComboBox3.SelectedIndex;
            if (i <= 0 || i >= _dateOptions.Count) return null;   // 0 = Today (live)
            return _dateOptions[i];
        }
        // --- Multi-Filter Triggers ---

        private void txtbRoomFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ApplyFilters();
            }
        }

        private void cmbRoomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void cmbRoomStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            string roomNumFilter = txtbRoomFilter.Text.Trim();
            if (roomNumFilter == "Search Room Number") roomNumFilter = "";

            string selectedType = cmbRoomType.SelectedItem?.ToString() ?? "All Types";
            if (selectedType == "All Types") selectedType = "";

            string selectedStatus = cmbRoomStatus.SelectedItem?.ToString() ?? "All Statuses";
            if (selectedStatus == "All Statuses") selectedStatus = "";

            LoadRoomData(roomNumFilter, selectedType, selectedStatus, SelectedFilterDate());
        }

        // --- Core Data Loading Procedure ---

        public void LoadRoomData(string filterRoomNumber = "", string filterType = "", string filterStatus = "", DateTime? forDate = null)
        {
            if (dgvRoomStatus.InvokeRequired)
            {
                this.Invoke(new Action(() => LoadRoomData(filterRoomNumber, filterType, filterStatus, forDate)));
                return;
            }

            dgvRoomStatus.Rows.Clear();

            int availableCount = 0;
            int occupiedCount = 0;
            int reservedCount = 0;
            int maintenanceCount = 0;

            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();

                    // EffStatus: for "Today" it is the real room status. For another date it is worked
                    // out from the reservations (Reserved / Available) - Maintenance rooms stay as they are.
                    string query = @"
                SELECT q.RoomNumber, q.TypeName, q.Floor, q.EffStatus AS Status, q.PricePerNight
                FROM (
                    SELECT r.RoomNumber, rt.TypeName, r.Floor, rt.PricePerNight,
                           CASE
                               WHEN @ForDate IS NULL THEN r.Status
                               WHEN r.Status = 'Maintenance' THEN 'Maintenance'
                               WHEN EXISTS (SELECT 1 FROM RESERVATION x
                                            WHERE x.RoomID = r.RoomID AND x.Status <> 'Cancelled'
                                              AND x.CheckInDate <= @ForDate AND x.CheckOutDate > @ForDate)
                                    THEN 'Reserved'
                               ELSE 'Available'
                           END AS EffStatus
                    FROM ROOM r
                    INNER JOIN ROOM_TYPE rt ON r.RoomTypeID = rt.RoomTypeID
                    WHERE (@FilterNum = '' OR r.RoomNumber LIKE '%' + @FilterNum + '%')
                      AND (@FilterType = '' OR rt.TypeName = @FilterType)
                ) q
                WHERE (@FilterStatus = '' OR q.EffStatus = @FilterStatus)
                ORDER BY q.RoomNumber ASC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FilterNum", filterRoomNumber);
                        command.Parameters.AddWithValue("@FilterType", filterType);
                        command.Parameters.AddWithValue("@FilterStatus", filterStatus);

                        SqlParameter dateParam = command.Parameters.Add("@ForDate", SqlDbType.Date);
                        dateParam.Value = forDate.HasValue ? (object)forDate.Value.Date : DBNull.Value;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string roomNumber = reader["RoomNumber"].ToString();
                                string typeName = reader["TypeName"].ToString();
                                string floor = "Floor " + reader["Floor"].ToString();
                                string status = reader["Status"].ToString();
                                decimal price = Convert.ToDecimal(reader["PricePerNight"]);
                                string rate = $"₱{price:N0}/night";
                                string lastCleaned = "Today 08:00";

                                AddRoom(roomNumber, typeName, floor, status, rate, lastCleaned);

                                switch (status)
                                {
                                    case "Available":
                                        availableCount++;
                                        break;
                                    case "Occupied":
                                        occupiedCount++;
                                        break;
                                    case "Reserved":
                                        reservedCount++;
                                        break;
                                    case "Maintenance":
                                    case "Cleaning":
                                        maintenanceCount++;
                                        break;
                                }
                            }
                        }
                    }
                }

                UpdateOverviewCards(availableCount, occupiedCount, reservedCount, maintenanceCount);
                dgvRoomStatus.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load room data from database:\n\n" + ex.Message,
                                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeStatusContextMenu()
        {
            cmsRoomStatus = new ContextMenuStrip();

            ToolStripMenuItem itemAvailable = new ToolStripMenuItem("Mark as Available");
            ToolStripMenuItem itemOccupied = new ToolStripMenuItem("Mark as Occupied");
            ToolStripMenuItem itemReserved = new ToolStripMenuItem("Mark as Reserved");
            ToolStripMenuItem itemMaintenance = new ToolStripMenuItem("Mark as Maintenance");
            ToolStripMenuItem itemCleaning = new ToolStripMenuItem("Mark as Cleaning");

            itemAvailable.Click += (s, e) => UpdateSelectedRoomStatus("Available");
            itemOccupied.Click += (s, e) => UpdateSelectedRoomStatus("Occupied");
            itemReserved.Click += (s, e) => UpdateSelectedRoomStatus("Reserved");
            itemMaintenance.Click += (s, e) => UpdateSelectedRoomStatus("Maintenance");
            itemCleaning.Click += (s, e) => UpdateSelectedRoomStatus("Cleaning");

            cmsRoomStatus.Items.Add(itemAvailable);
            cmsRoomStatus.Items.Add(itemOccupied);
            cmsRoomStatus.Items.Add(itemReserved);
            cmsRoomStatus.Items.Add(itemMaintenance);
            cmsRoomStatus.Items.Add(itemCleaning);
        }
        

        private void dgvRoomStatus_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //      (a future date is a forecast, so changing the real status there would be wrong)
            if (SelectedFilterDate() != null)
            {
                if (e.Button == MouseButtons.Right)
                    MessageBox.Show("Switch the date filter back to \"Today\" to change a room's status.",
                        "Date View", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvRoomStatus.ClearSelection();
                dgvRoomStatus.Rows[e.RowIndex].Selected = true;

                selectedRoomNumber = dgvRoomStatus.Rows[e.RowIndex].Cells["colRoom"].Value?.ToString();

                Point mousePosition = dgvRoomStatus.PointToClient(Cursor.Position);
                cmsRoomStatus.Show(dgvRoomStatus, mousePosition);
            }
        }

        private void UpdateSelectedRoomStatus(string newStatus)
        {
            if (string.IsNullOrEmpty(selectedRoomNumber)) return;

            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "UPDATE ROOM SET Status = @Status WHERE RoomNumber = @RoomNumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Status", newStatus);
                        command.Parameters.AddWithValue("@RoomNumber", selectedRoomNumber);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"Room {selectedRoomNumber} status updated to '{newStatus}'.",
                                            "Status Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            ApplyFilters();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Failed to update room status in database:\n\n" + ex.Message,
                                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateOverviewCards(int available, int occupied, int reserved, int maintenance)
        {
            lblStatAvailable.Text = available.ToString();
            lblStatOccupied.Text = occupied.ToString();
            lblStatReserved.Text = reserved.ToString();
            lblStatMaintenance.Text = maintenance.ToString();
        }

        private void AddRoom(string room, string type, string floor, string status, string rate, string lastCleaned)
        {
            int index = dgvRoomStatus.Rows.Add();
            DataGridViewRow row = dgvRoomStatus.Rows[index];

            row.Cells["colRoom"].Value = room;
            row.Cells["colType"].Value = type;
            row.Cells["colFloor"].Value = floor;
            row.Cells["colStatus"].Value = status;
            row.Cells["colRate"].Value = rate;
            row.Cells["colLastCleaned"].Value = lastCleaned;
        }

        private void dgvRoomStatus_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            if (dgvRoomStatus.Columns[e.ColumnIndex].Name == "colRoom")
            {
                e.CellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                e.CellStyle.ForeColor = Color.FromArgb(30, 41, 59);
            }
            else if (dgvRoomStatus.Columns[e.ColumnIndex].Name == "colStatus")
            {
                string status = e.Value.ToString().Trim();
                e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

                switch (status)
                {
                    case "Available":
                        e.CellStyle.ForeColor = Color.FromArgb(16, 185, 129); // Green
                        break;
                    case "Occupied":
                        e.CellStyle.ForeColor = Color.FromArgb(37, 99, 235);  // Blue
                        break;
                    case "Reserved":
                        e.CellStyle.ForeColor = Color.FromArgb(217, 119, 6);  // Amber
                        break;
                    case "Maintenance":
                    case "Cleaning":
                        e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38);  // Red
                        break;
                }
            }
        }

        // --- Event Stubs Preserved for Designer Compatibility ---
        private void lblStatAvailable_Click(object sender, EventArgs e) { }
        private void lblStatOccupied_Click(object sender, EventArgs e) { }
        private void lblStatReserved_Click(object sender, EventArgs e) { }
        private void lblStatMaintenance_Click(object sender, EventArgs e) { }
        private void dgvRoomStatus_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dgvRoomStatus_CellContentClick_1(object sender, DataGridViewCellEventArgs e) { }
        private void txtbRoomFilter_TextChanged(object sender, EventArgs e) { }
    }
}
