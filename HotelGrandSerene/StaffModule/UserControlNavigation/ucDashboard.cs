using HotelGrandSerene.DashboardModule.ModalPopup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrandSereneHotel.DatabaseAccess;
using System.Data.SqlClient;

namespace HotelGrandSerene.DashboardModule.UserControlNavigation
{
    public partial class ucDashboard : UserControl
    {
        private StayInfo _pendingCheckIn;     // the reservation currently shown in the Check-in card
        private StayInfo _pendingCheckOut;    // the reservation currently shown in the Check-out card
        private int[] _bookingTrend = new int[10];
        private int[] _occupancyTrend = new int[10];
        public ucDashboard()
        {
            InitializeComponent();
            guna2Button1.Click += (s, e) => ConfirmCheckIn();     // "Confirm Check-in"
            guna2Button2.Click += (s, e) => ConfirmCheckOut();    // "Confirm Check-out"
            guna2Button3.Click += (s, e) => FindForCheckOut();    // "Find Control Number"
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime) LoadStats();   // skip while editing in the designer
        }

        // TOTAL BOOKINGS  = all reservations that are not cancelled
        // OCCUPANCY RATE  = rooms booked for TONIGHT / all rooms
        // Graphs          = last 10 days (bookings MADE per day / occupancy % per day)
        private void LoadStats()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    int totalRooms;
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM ROOM", conn))
                        totalRooms = Convert.ToInt32(cmd.ExecuteScalar());

                    int totalBookings;
                    using (SqlCommand cmd = new SqlCommand(
                        "SELECT COUNT(*) FROM RESERVATION WHERE Status <> 'Cancelled'", conn))
                        totalBookings = Convert.ToInt32(cmd.ExecuteScalar());

                    int[] booked = new int[10];
                    int[] occupied = new int[10];

                    string trendSql = @"
                ;WITH d AS (
                    SELECT CAST(DATEADD(DAY, -9, GETDATE()) AS date) AS DayDate
                    UNION ALL
                    SELECT DATEADD(DAY, 1, DayDate) FROM d WHERE DayDate < CAST(GETDATE() AS date)
                )
                SELECT d.DayDate,
                       (SELECT COUNT(*) FROM RESERVATION r
                         WHERE CAST(r.ReservationDate AS date) = d.DayDate AND r.Status <> 'Cancelled') AS Booked,
                       (SELECT COUNT(*) FROM RESERVATION r
                         WHERE r.Status <> 'Cancelled'
                           AND r.CheckInDate <= d.DayDate AND r.CheckOutDate > d.DayDate) AS Occupied
                FROM d
                ORDER BY d.DayDate
                OPTION (MAXRECURSION 20)";

                    using (SqlCommand cmd = new SqlCommand(trendSql, conn))
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        int i = 0;
                        while (r.Read() && i < 10)
                        {
                            booked[i] = r.GetInt32(1);
                            occupied[i] = r.GetInt32(2);
                            i++;
                        }
                    }

                    int[] occupancyPercent = new int[10];
                    for (int i = 0; i < 10; i++)
                    {
                        occupancyPercent[i] = totalRooms > 0
                            ? (int)Math.Min(100, Math.Round(100.0 * occupied[i] / totalRooms))
                            : 0;
                    }

                    lblBookingCount.Text = totalBookings.ToString();
                    lblOccupancy.Text = occupancyPercent[9] + "%";       // index 9 = today

                    _bookingTrend = booked;
                    _occupancyTrend = occupancyPercent;
                }

                pnlSparklineTB.Invalidate();
                pnlSparklineOccu.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load dashboard numbers:\n\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ucDashboard_Load(object sender, EventArgs e)
        {
            // Initial data loads or stat refreshes
        }

        #region --- 1. Sparkline Mini-Graphs ---

        // Total Bookings Sparkline (pnlSparklineTB)
        private void pnlSparklineTB_Paint(object sender, PaintEventArgs e)
        {
            Control panel = sender as Control;
            if (panel == null) return;

            int[] bookingTrends = _bookingTrend;
            DrawSparkline(e.Graphics, panel, bookingTrends, Color.FromArgb(212, 175, 55)); // Gold Accent
        }

        // Occupancy Rate Sparkline (pnlSparklineOccu)
        private void pnlSparklineOccu_Paint(object sender, PaintEventArgs e)
        {
            Control panel = sender as Control;
            if (panel == null) return;

            int[] occupancyTrends = _occupancyTrend;
            DrawSparkline(e.Graphics, panel, occupancyTrends, Color.FromArgb(0, 122, 204)); // Blue Accent
        }

        // Reusable Helper Method to Draw Sparklines
        private void DrawSparkline(Graphics g, Control panel, int[] dataPoints, Color lineColor)
        {
            if (dataPoints == null || dataPoints.Length < 2) return;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (Pen sparkPen = new Pen(lineColor, 2f)) // 2px line thickness
            {
                float widthStep = (float)panel.Width / (dataPoints.Length - 1);

                int maxVal = 1;
                foreach (int val in dataPoints)
                {
                    if (val > maxVal) maxVal = val;
                }

                float padding = 4f;
                float usableHeight = panel.Height - (padding * 2);

                for (int i = 0; i < dataPoints.Length - 1; i++)
                {
                    float x1 = i * widthStep;
                    float y1 = (panel.Height - padding) - ((float)dataPoints[i] / maxVal * usableHeight);

                    float x2 = (i + 1) * widthStep;
                    float y2 = (panel.Height - padding) - ((float)dataPoints[i + 1] / maxVal * usableHeight);

                    g.DrawLine(sparkPen, x1, y1, x2, y2);
                }
            }
        }

        #endregion

        #region --- 2. Feedback Ratings Doughnut Chart ---

        private void pnlRatings_Paint(object sender, PaintEventArgs e)
        {
            Control panel = sender as Control;
            if (panel == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Example Customer Feedback Breakdown (5-Star down to 1-Star)
            float[] ratingValues = { 65f, 20f, 10f, 3f, 2f };
            Color[] sliceColors = {
                Color.FromArgb(46, 204, 113),  // 5 Star (Green)
                Color.FromArgb(52, 152, 219),  // 4 Star (Blue)
                Color.FromArgb(241, 196, 15),  // 3 Star (Yellow)
                Color.FromArgb(230, 126, 34),  // 2 Star (Orange)
                Color.FromArgb(231, 76, 60)    // 1 Star (Red)
            };

            float total = 0;
            foreach (float val in ratingValues) total += val;
            if (total == 0) return;

            // Calculate chart bounds
            int padding = 10;
            int diameter = Math.Min(panel.Width, panel.Height) - (padding * 2);
            int x = (panel.Width - diameter) / 2;
            int y = (panel.Height - diameter) / 2;
            Rectangle boundingBox = new Rectangle(x, y, diameter, diameter);

            float startAngle = -90f; // Start at top (12 o'clock)

            // Draw Pie Slices
            for (int i = 0; i < ratingValues.Length; i++)
            {
                float sweepAngle = (ratingValues[i] / total) * 360f;
                using (SolidBrush brush = new SolidBrush(sliceColors[i]))
                {
                    g.FillPie(brush, boundingBox, startAngle, sweepAngle);
                }
                startAngle += sweepAngle;
            }

            // Cut out center to convert pie into a doughnut chart
            int holeDiameter = (int)(diameter * 0.55); // Center hole size (55%)
            int holeX = (panel.Width - holeDiameter) / 2;
            int holeY = (panel.Height - holeDiameter) / 2;
            Rectangle holeBox = new Rectangle(holeX, holeY, holeDiameter, holeDiameter);

            Color parentBg = panel.Parent != null ? panel.Parent.BackColor : panel.BackColor;
            using (SolidBrush bgBrush = new SolidBrush(parentBg))
            {
                g.FillEllipse(bgBrush, holeBox);
            }
        }

        #endregion

        #region --- 3. Modal Popup Handler ---

        #endregion

        #region --- Designer Placeholders & Event Redirects ---

        // Secondary handler redirects if linked in Designer
        private void pnlSparkline_Paint_1(object sender, PaintEventArgs e) => pnlSparklineTB_Paint(sender, e);
        private void pnlSparklineOccu_Paint_1(object sender, PaintEventArgs e) => pnlSparklineOccu_Paint(sender, e);

        // Empty event stubs
        private void guna2Panel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2HtmlLabel5_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel6_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel4_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel7_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel8_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel12_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel22_Click(object sender, EventArgs e) { }
        private void pnlSparkline_Click(object sender, EventArgs e) { }

        #endregion

        private static string Peso(decimal amount)
        {
            return "\u20B1" + amount.ToString("N2");
        }

        // "+ New Check-in": find the reservation, then show it in the card
        private void btnNewcheckin_Click(object sender, EventArgs e)
        {
            frmNewCheckin dlg = new frmNewCheckin();
            if (ShowModal(dlg) == DialogResult.OK && dlg.FoundStay != null)
            {
                _pendingCheckIn = dlg.FoundStay;
                FillCheckInCard(_pendingCheckIn);
            }
        }

        // "Find Control Number": find the guest, then show them in the Check-out card
        private void FindForCheckOut()
        {
            frmCheckout dlg = new frmCheckout();
            if (ShowModal(dlg) == DialogResult.OK && dlg.FoundStay != null)
            {
                _pendingCheckOut = dlg.FoundStay;
                FillCheckOutCard(_pendingCheckOut);
            }
        }

        // "Confirm Check-in": the real check-in happens here
        private void ConfirmCheckIn()
        {
            if (_pendingCheckIn == null)
            {
                MessageBox.Show("Click \"+ New Check-in\" first to load the guest's reservation.",
                    "Check-in", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            StayInfo info = _pendingCheckIn;

            // Outside the booked dates? Let the staff decide (handy for demos too).
            DateTime today = DateTime.Today;
            if (today < info.CheckIn.Date || today >= info.CheckOut.Date)
            {
                DialogResult go = MessageBox.Show(
                    "This reservation is for " + info.CheckIn.ToString("MMM dd, yyyy") + " to " +
                    info.CheckOut.ToString("MMM dd, yyyy") + ".\nToday is outside that period.\n\nCheck in anyway?",
                    "Dates Don't Match", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (go != DialogResult.Yes) return;
            }

            string error = StayService.CheckIn(info.ReservationId);
            if (error != null)
            {
                MessageBox.Show(error, "Check-in", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(
                "Check-in complete!\n\nGuest: " + info.GuestName +
                "\nRoom: " + info.RoomNumber + " (" + info.TypeName + ")" +
                "\nBalance due at check-out: " + Peso(info.Balance),
                "Checked In", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ClearCheckInCard();
            LoadStats();
        }

        // "Confirm Check-out": the real check-out happens here
        private void ConfirmCheckOut()
        {
            if (_pendingCheckOut == null)
            {
                MessageBox.Show("Click \"Find Control Number\" first to load the guest.",
                    "Check-out", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Re-read it: the balance may have changed since the card was filled
                StayInfo info = StayService.Find(_pendingCheckOut.ReservationId);
                if (info == null || info.Status != "Checked-In")
                {
                    MessageBox.Show("This guest is no longer checked in. The card will be cleared.",
                        "Check-out", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ClearCheckOutCard();
                    return;
                }

                string method = "Cash";
                if (info.Balance > 0m)
                {
                    method = AskPaymentMethod(info.GuestName, info.Balance);
                    if (method == null) return;          // staff pressed Cancel
                }

                decimal collected;
                string error = StayService.CheckOut(info.ReservationId, method, out collected);
                if (error != null)
                {
                    MessageBox.Show(error, "Check-out", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show(
                    "Check-out complete!\n\nGuest: " + info.GuestName +
                    "\nRoom " + info.RoomNumber + " is now marked 'Cleaning'." +
                    (collected > 0m ? "\nCollected: " + Peso(collected) + " (" + method + ")" : ""),
                    "Checked Out", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearCheckOutCard();
                LoadStats();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not check out:\n\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Small window asking how the remaining balance is paid. Returns null if cancelled.
        private string AskPaymentMethod(string guest, decimal balance)
        {
            using (Form f = new Form())
            {
                f.Text = "Collect Balance";
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.MaximizeBox = false;
                f.MinimizeBox = false;
                f.ShowInTaskbar = false;
                f.ClientSize = new Size(360, 150);

                Label lbl = new Label();
                lbl.Text = guest + " has a balance of " + Peso(balance) + ".\nHow is it being paid?";
                lbl.Location = new Point(16, 14);
                lbl.Size = new Size(328, 40);

                ComboBox cmb = new ComboBox();
                cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                cmb.Location = new Point(16, 62);
                cmb.Width = 328;
                cmb.Items.AddRange(new object[] { "Cash", "Credit / Debit Card", "Bank Transfer / E-wallet" });
                cmb.SelectedIndex = 0;

                Button ok = new Button();
                ok.Text = "Collect && Check-out";
                ok.DialogResult = DialogResult.OK;
                ok.Location = new Point(120, 105);
                ok.Size = new Size(140, 30);

                Button cancel = new Button();
                cancel.Text = "Cancel";
                cancel.DialogResult = DialogResult.Cancel;
                cancel.Location = new Point(268, 105);
                cancel.Size = new Size(76, 30);

                f.Controls.Add(lbl);
                f.Controls.Add(cmb);
                f.Controls.Add(ok);
                f.Controls.Add(cancel);
                f.AcceptButton = ok;
                f.CancelButton = cancel;

                return f.ShowDialog(this.FindForm()) == DialogResult.OK ? cmb.SelectedItem.ToString() : null;
            }
        }

        // ---- The two cards. Check-in card = guna2HtmlLabel5..13, check-out card = 14..21 ----

        private void FillCheckInCard(StayInfo i)
        {
            guna2HtmlLabel5.Text = "Last Name: " + i.LastName;
            guna2HtmlLabel7.Text = "First Name: " + i.FirstName;
            guna2HtmlLabel9.Text = "Control Number: R-" + i.ReservationId;
            guna2HtmlLabel8.Text = "Room Number: " + i.RoomNumber + " (" + i.TypeName + ")";
            guna2HtmlLabel11.Text = "Check-in Date: " + i.CheckIn.ToString("MMM dd, yyyy");
            guna2HtmlLabel12.Text = "Check-out Date: " + i.CheckOut.ToString("MMM dd, yyyy");
            guna2HtmlLabel10.Text = "Payment Method: " + (string.IsNullOrEmpty(i.PaymentMethod) ? "-" : i.PaymentMethod);
            guna2HtmlLabel13.Text = "Total Amount: " + Peso(i.Total) + "   (paid " + Peso(i.Paid) + ")";
        }

        private void ClearCheckInCard()
        {
            _pendingCheckIn = null;
            guna2HtmlLabel5.Text = "Last Name: ";
            guna2HtmlLabel7.Text = "First Name:";
            guna2HtmlLabel9.Text = "Control Number";
            guna2HtmlLabel8.Text = "Room Number:";
            guna2HtmlLabel11.Text = "Check-in Date:";
            guna2HtmlLabel12.Text = "Check-out Date";
            guna2HtmlLabel10.Text = "Payment Method:";
            guna2HtmlLabel13.Text = "Total Amount:";
        }

        private void FillCheckOutCard(StayInfo i)
        {
            guna2HtmlLabel21.Text = "Last Name: " + i.LastName;
            guna2HtmlLabel20.Text = "First Name: " + i.FirstName;
            guna2HtmlLabel18.Text = "Control Number: R-" + i.ReservationId;
            guna2HtmlLabel19.Text = "Room Number: " + i.RoomNumber + " (" + i.TypeName + ")";
            guna2HtmlLabel16.Text = "Check-in Date: " + i.CheckIn.ToString("MMM dd, yyyy");
            guna2HtmlLabel15.Text = "Check-out Date: " + i.CheckOut.ToString("MMM dd, yyyy");
            guna2HtmlLabel17.Text = "Payment Method: " + (string.IsNullOrEmpty(i.PaymentMethod) ? "-" : i.PaymentMethod);
            guna2HtmlLabel14.Text = "Total Balance: " + Peso(i.Balance) + "   (total " + Peso(i.Total) + ")";
        }

        private void ClearCheckOutCard()
        {
            _pendingCheckOut = null;
            guna2HtmlLabel21.Text = "Last Name: ";
            guna2HtmlLabel20.Text = "First Name:";
            guna2HtmlLabel18.Text = "Control Number";
            guna2HtmlLabel19.Text = "Room Number:";
            guna2HtmlLabel16.Text = "Check-in Date:";
            guna2HtmlLabel15.Text = "Check-out Date";
            guna2HtmlLabel17.Text = "Payment Method:";
            guna2HtmlLabel14.Text = "Total Balance:";
        }

        // Shows a window over a dim overlay and returns how it was closed
        private DialogResult ShowModal(Form dialog)
        {
            Form overlay = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.Black,
                Opacity = 0.50,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual
            };

            Form mainForm = this.FindForm();
            if (mainForm != null)
            {
                overlay.Size = mainForm.Size;
                overlay.Location = mainForm.Location;
                overlay.Show(mainForm);
            }

            DialogResult result = DialogResult.Cancel;
            try
            {
                result = dialog.ShowDialog(overlay.Visible ? overlay : (IWin32Window)this);
            }
            finally
            {
                dialog.Dispose();
                overlay.Dispose();
            }
            return result;
        }

    }
}
