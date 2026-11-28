using GrandSereneHotel.DatabaseAccess;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace HotelGrandSerene.StaffModule.ModalPopup
{
    public partial class frmRoomSelection : Form
    {
        private DateTime checkInDate = DateTime.Now.Date;
        private DateTime checkOutDate = DateTime.Now.Date.AddDays(1);
        private int allowedRoomCount = 1;
        private int adultsCount = 1;
        private int childrenCount = 0;
        private bool isHandlingSelection = false;
        private ContextMenuStrip roomContextMenu;

        public List<string> SelectedRoomNumbers { get; private set; } = new List<string>();
        public List<int> HoldIDs { get; private set; } = new List<int>();
        public frmRoomSelection() : this(DateTime.Now.Date, DateTime.Now.Date.AddDays(1), 1, 1, 0) { }

        public frmRoomSelection(DateTime checkIn, DateTime checkOut, int rooms, int adults, int children)
        {
            InitializeComponent();
            checkInDate = checkIn;
            checkOutDate = checkOut;
            allowedRoomCount = rooms;
            adultsCount = adults;
            childrenCount = children;

            dgvRoomStatus.CellFormatting += dgvRoomStatus_CellFormatting;
            dgvRoomStatus.SelectionChanged += dgvRoomStatus_SelectionChanged;
            SetupContextMenu();
        }

        private void frmRoomSelection_Load(object sender, EventArgs e)
        {
            if (pnlGuestCountDrop != null) pnlGuestCountDrop.Visible = false;
            SyncGuestCounterUI();
            this.Click += (s, ev) => { if (pnlGuestCountDrop != null) pnlGuestCountDrop.Visible = false; };

            SetupDatePickers();
            SetupRoomGrid();
            PopulateComboboxFilters();

            if (cmbRoomType != null) cmbRoomType.SelectedIndexChanged += (s, ev) => ApplyFilters();
            if (cmbRoomStatus != null) cmbRoomStatus.SelectedIndexChanged += (s, ev) => ApplyFilters();

            ApplyFilters();
        }

        #region --- Context Menu & Selection ---
        private void SetupContextMenu()
        {
            roomContextMenu = new ContextMenuStrip();
            ToolStripMenuItem selectItem = new ToolStripMenuItem("Select this room");
            selectItem.Click += (s, ev) => { };
            roomContextMenu.Items.Add(selectItem);

            dgvRoomStatus.ContextMenuStrip = roomContextMenu;
            dgvRoomStatus.MouseUp += (s, e) => {
                if (e.Button == MouseButtons.Right)
                {
                    var hit = dgvRoomStatus.HitTest(e.X, e.Y);
                    if (hit.RowIndex >= 0)
                    {
                        DataGridViewRow clickedRow = dgvRoomStatus.Rows[hit.RowIndex];
                        if (!clickedRow.Selected)
                        {
                            if (dgvRoomStatus.SelectedRows.Count < allowedRoomCount)
                            {
                                clickedRow.Selected = true;
                            }
                            else if (allowedRoomCount == 1)
                            {
                                dgvRoomStatus.ClearSelection();
                                clickedRow.Selected = true;
                            }
                            else
                            {
                                MessageBox.Show($"You can only select up to {allowedRoomCount} room(s) based on your guest counter.",
                                                "Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            };
        }

        private void dgvRoomStatus_SelectionChanged(object sender, EventArgs e)
        {
            if (isHandlingSelection || dgvRoomStatus == null) return;
            if (dgvRoomStatus.SelectedRows.Count > allowedRoomCount)
            {
                isHandlingSelection = true;
                MessageBox.Show($"You can only select up to {allowedRoomCount} room(s) based on your guest counter.", "Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                for (int i = allowedRoomCount; i < dgvRoomStatus.SelectedRows.Count; i++)
                    dgvRoomStatus.SelectedRows[i].Selected = false;
                isHandlingSelection = false;
            }
        }
        #endregion

        #region --- UI Setup & Filters ---
        private void SetupDatePickers()
        {
            if (dtpCheckIn != null) { dtpCheckIn.Value = checkInDate; dtpCheckIn.Format = DateTimePickerFormat.Long; dtpCheckIn.ValueChanged += (s, e) => { checkInDate = dtpCheckIn.Value; ApplyFilters(); }; }
            if (dtpCheckOut != null) { dtpCheckOut.Value = checkOutDate; dtpCheckOut.Format = DateTimePickerFormat.Long; dtpCheckOut.ValueChanged += (s, e) => { checkOutDate = dtpCheckOut.Value; ApplyFilters(); }; }
        }

        private void SetupRoomGrid()
        {
            if (dgvRoomStatus != null)
            {
                dgvRoomStatus.MultiSelect = true;
                dgvRoomStatus.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvRoomStatus.ReadOnly = true;
                dgvRoomStatus.AllowUserToAddRows = false;
            }
        }

        private void PopulateComboboxFilters()
        {
            if (cmbRoomStatus != null && cmbRoomStatus.Items.Count == 0)
            {
                cmbRoomStatus.Items.AddRange(new object[] { "All Statuses", "Available", "Occupied", "Reserved", "Maintenance" });
                cmbRoomStatus.SelectedIndex = 0;
            }
            if (cmbRoomType != null && cmbRoomType.Items.Count == 0)
            {
                cmbRoomType.Items.Add("All Types");
                try
                {
                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT TypeName FROM ROOM_TYPE ORDER BY TypeName ASC", conn))
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                            while (rdr.Read()) cmbRoomType.Items.Add(rdr["TypeName"].ToString());
                    }
                }
                catch { cmbRoomType.Items.AddRange(new object[] { "Standard Room", "Suite Room" }); }
                cmbRoomType.SelectedIndex = 0;
            }
        }

        private void ApplyFilters()
        {
            string type = cmbRoomType?.SelectedItem?.ToString() == "All Types" ? "" : cmbRoomType?.SelectedItem?.ToString() ?? "";
            string status = cmbRoomStatus?.SelectedItem?.ToString() == "All Statuses" ? "" : cmbRoomStatus?.SelectedItem?.ToString() ?? "";
            LoadRoomData("", type, status, checkInDate, checkOutDate);
        }

        public void LoadRoomData(string roomNum = "", string type = "", string status = "", DateTime? checkIn = null, DateTime? checkOut = null)
        {
            if (dgvRoomStatus == null) return;
            if (dgvRoomStatus.InvokeRequired) { Invoke(new Action(() => LoadRoomData(roomNum, type, status, checkIn, checkOut))); return; }

            dgvRoomStatus.Rows.Clear();
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT r.RoomNumber, rt.TypeName, r.Floor, r.Status, rt.PricePerNight
                                     FROM ROOM r INNER JOIN ROOM_TYPE rt ON r.RoomTypeID = rt.RoomTypeID
                                     WHERE (@FNum = '' OR r.RoomNumber LIKE '%' + @FNum + '%')
                                       AND (@FType = '' OR rt.TypeName = @FType)
                                       AND (@FStatus = '' OR r.Status = @FStatus)
                                       AND NOT EXISTS (SELECT 1 FROM RESERVATION res WHERE res.RoomID = r.RoomID AND (res.Status IS NULL OR res.Status != 'Cancelled') AND res.CheckInDate < @CheckOut AND res.CheckOutDate > @CheckIn)
                                     ORDER BY r.RoomNumber ASC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FNum", roomNum);
                        cmd.Parameters.AddWithValue("@FType", type);
                        cmd.Parameters.AddWithValue("@FStatus", status);
                        cmd.Parameters.AddWithValue("@CheckIn", checkIn ?? checkInDate);
                        cmd.Parameters.AddWithValue("@CheckOut", checkOut ?? checkOutDate);

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                int idx = dgvRoomStatus.Rows.Add();
                                var row = dgvRoomStatus.Rows[idx];
                                row.Cells["colRoom"].Value = rdr["RoomNumber"].ToString();
                                row.Cells["colType"].Value = rdr["TypeName"].ToString();
                                row.Cells["colFloor"].Value = "Floor " + rdr["Floor"].ToString();
                                row.Cells["colStatus"].Value = rdr["Status"].ToString();
                                row.Cells["colRate"].Value = $"₱{Convert.ToDecimal(rdr["PricePerNight"]):N0}/night";
                                row.Cells["colLastCleaned"].Value = "Today 08:00";
                            }
                        }
                    }
                }
                dgvRoomStatus.ClearSelection();
            }
            catch (Exception ex) { MessageBox.Show("Error loading rooms:\n" + ex.Message, "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
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
                string s = e.Value.ToString().Trim();
                e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                e.CellStyle.ForeColor = s == "Available" ? Color.FromArgb(16, 185, 129) :
                                        s == "Occupied" ? Color.FromArgb(37, 99, 235) :
                                        s == "Reserved" ? Color.FromArgb(217, 119, 6) : Color.FromArgb(220, 38, 38);
            }
        }
        #endregion

       // --- Guest Counter Controls ---
        private void SyncGuestCounterUI()
        {
            if (lblRoomCount != null) lblRoomCount.Text = allowedRoomCount.ToString();
            if (lblAdultCount != null) lblAdultCount.Text = adultsCount.ToString();
            if (lblChildCount != null) lblChildCount.Text = childrenCount.ToString();
            if (btnGuestCounter != null) btnGuestCounter.Text = $"{allowedRoomCount} Room{(allowedRoomCount > 1 ? "s" : "")}, {adultsCount} Adult{(adultsCount > 1 ? "s" : "")}, {childrenCount} Child{(childrenCount != 1 ? "ren" : "")}";
        }

        private void btnGuestCounter_Click(object sender, EventArgs e) { if (pnlGuestCountDrop != null) { pnlGuestCountDrop.Visible = !pnlGuestCountDrop.Visible; if (pnlGuestCountDrop.Visible) pnlGuestCountDrop.BringToFront(); } }
        private void btnRoomsPlus_Click(object sender, EventArgs e) { allowedRoomCount++; SyncGuestCounterUI(); }
        private void btnRoomsMinus_Click(object sender, EventArgs e) { if (allowedRoomCount > 1) { allowedRoomCount--; SyncGuestCounterUI(); if (dgvRoomStatus.SelectedRows.Count > allowedRoomCount) dgvRoomStatus_SelectionChanged(sender, e); } }
        private void btnAdultsPlus_Click(object sender, EventArgs e) { adultsCount++; SyncGuestCounterUI(); }
        private void btnAdultMinus_Click(object sender, EventArgs e) { if (adultsCount > 1) { adultsCount--; SyncGuestCounterUI(); } }
        private void btnChildPlus_Click(object sender, EventArgs e) { childrenCount++; SyncGuestCounterUI(); }
        private void btnChildMinus_Click(object sender, EventArgs e) { if (childrenCount > 0) { childrenCount--; SyncGuestCounterUI(); } }
       //endregion

        // --- Proceed Button & Modal Swap Transition ---
        private void btnProceed_Click(object sender, EventArgs e)
        {
            if (dgvRoomStatus == null || dgvRoomStatus.SelectedRows.Count == 0 || dgvRoomStatus.SelectedRows.Count != allowedRoomCount)
            {
                MessageBox.Show($"Please select exactly {allowedRoomCount} room(s) before proceeding.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SelectedRoomNumbers.Clear();
            foreach (DataGridViewRow row in dgvRoomStatus.SelectedRows)
                if (row.Cells["colRoom"]?.Value != null) SelectedRoomNumbers.Add(row.Cells["colRoom"].Value.ToString());

            int totalGuests = adultsCount + childrenCount;
            HoldIDs.Clear();
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // Throw away holds that were abandoned more than 30 minutes ago
                    using (SqlCommand cmdClean = new SqlCommand(
                        "DELETE FROM TEMP_RESERVATION_HOLD WHERE CreatedAt < DATEADD(MINUTE, -30, GETDATE())", conn))
                    {
                        cmdClean.ExecuteNonQuery();
                    }

                    foreach (string roomNum in SelectedRoomNumbers)
                    {
                        int roomID = 0;
                        using (SqlCommand cmdId = new SqlCommand("SELECT RoomID FROM ROOM WHERE RoomNumber = @Num", conn))
                        {
                            cmdId.Parameters.AddWithValue("@Num", roomNum);
                            object res = cmdId.ExecuteScalar();
                            if (res != null) roomID = Convert.ToInt32(res);
                        }

                        if (roomID > 0)
                        {
                            using (SqlCommand cmdIns = new SqlCommand(
                                @"INSERT INTO TEMP_RESERVATION_HOLD (RoomID, CheckInDate, CheckOutDate, NumberOfGuests)
                      OUTPUT INSERTED.TempHoldID
                      VALUES (@RID, @In, @Out, @Guests)", conn))
                            {
                                cmdIns.Parameters.AddWithValue("@RID", roomID);
                                cmdIns.Parameters.AddWithValue("@In", checkInDate);
                                cmdIns.Parameters.AddWithValue("@Out", checkOutDate);
                                cmdIns.Parameters.AddWithValue("@Guests", totalGuests);
                                HoldIDs.Add((int)cmdIns.ExecuteScalar());   // remember this hold's ID
                            }
                        }
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("Failed to save room hold:\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }


        private void btnClose_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }
        //endregion
    }
}
