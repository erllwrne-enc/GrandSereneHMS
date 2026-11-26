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

namespace HotelGrandSerene.DashboardModule.UserControlNavigation
{
    public partial class ucDashboard : UserControl
    {
        public ucDashboard()
        {
            InitializeComponent();
            guna2Button1.Click += btnNewcheckin_Click;                          // "Confirm Check-in" card button
            guna2Button2.Click += (s, e) => ShowModal(new frmCheckout());       // "Confirm Check-out" card button
            guna2Button3.Click += (s, e) => ShowModal(new frmCheckout());       // "Find Control Number" button
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

            int[] bookingTrends = { 10, 15, 8, 22, 18, 30, 25, 35, 28, 40 };
            DrawSparkline(e.Graphics, panel, bookingTrends, Color.FromArgb(212, 175, 55)); // Gold Accent
        }

        // Occupancy Rate Sparkline (pnlSparklineOccu)
        private void pnlSparklineOccu_Paint(object sender, PaintEventArgs e)
        {
            Control panel = sender as Control;
            if (panel == null) return;

            int[] occupancyTrends = { 40, 55, 60, 50, 70, 65, 80, 85, 75, 90 };
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

        private void btnNewcheckin_Click(object sender, EventArgs e)
        {
            ShowModal(new frmNewCheckin());
        }

        private void ShowModal(Form dialog)
        {
            // Dim overlay behind the dialog (same look as the reservation windows)
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

            try
            {
                dialog.ShowDialog(overlay.Visible ? overlay : (IWin32Window)this);
            }
            finally
            {
                dialog.Dispose();
                overlay.Dispose();
            }
        }
    }
}
