using HotelGrandSerene.CustomerModule.ucCustomerpnl;
using HotelGrandSerene.DashboardModule.UserControlNavigation;
using HotelGrandSerene.StaffModule.ModalPopup;
using HotelGrandSerene.StaffModule.UserControlNavigation;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GrandSereneHotel.DashboardModule.MainDocked
{
    public partial class frmMainDashboard : Form
    {
        // Real-time clock timer field
        private Timer clockTimer;

        // Tracks whether the user intentionally signed out to prevent Application.Exit() on close
        private bool isLoggingOut = false;

        public frmMainDashboard()
        {
            InitializeComponent();
        }

        private void frmMainDashboard_Load(object sender, EventArgs e)
        {
            // 1. Initial date and 24-hour clock display
            UpdateDateTime();

            // 2. Initialize real-time clock timer (Ticks every 1 second)
            clockTimer = new Timer();
            clockTimer.Interval = 1000;
            clockTimer.Tick += ClockTimer_Tick;
            clockTimer.Start();

            // 3. Trigger default startup view
            btnDashboard.PerformClick();
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            // Format: Thursday, August 14, 2026  |  14:05:09
            lblCurrentDate.Text = DateTime.Now.ToString("dddd, MMMM d, yyyy  |  HH:mm:ss");
        }

        // Reusable helper method to clear pnlDashmain and load new UserControls
        private void LoadView(UserControl uc)
        {
            pnlDashmain.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlDashmain.Controls.Add(uc);
            uc.BringToFront();
        }

        #region --- Navigation Handlers ---

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            LoadView(new ucDashboard());
        }

        private void btnGuests_Click_1(object sender, EventArgs e)
        {
            LoadView(new ucGuest());
        }

        private void btnStaff_Click_1(object sender, EventArgs e)
        {
            ucStaff staffView = new ucStaff();
            staffView.LoadStaffData(); // Explicitly force SQL data fetch before loading view
            LoadView(staffView);
        }


        private void btnFinance_Click_1(object sender, EventArgs e)
        {
            LoadView(new ucFinance());
        }

        private void btnRooms_Click_1(object sender, EventArgs e)
        {
            ucRooms roomsControl = new ucRooms();
            roomsControl.LoadRoomData(); // Explicitly trigger query before loading view
            LoadView(roomsControl);
        }

        private void btnReservation_Click(object sender, EventArgs e)
        {
            LoadView(new ucReservation());
        }

        private void btnNewReservation_Click(object sender, EventArgs e)
        {
            // Create dim overlay form
            Form overlay = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.Black,
                Opacity = 0.50,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual
            };

            // Calculate dimensions to cover the main dashboard form
            overlay.Size = this.Size;
            overlay.Location = this.Location;
            overlay.Show(this);

            try
            {
                // Step 1: Open room selection modal dialog over the dim overlay
                using (frmRoomSelection roomModal = new frmRoomSelection())
                {
                    roomModal.StartPosition = FormStartPosition.CenterParent;

                    if (roomModal.ShowDialog(overlay) == DialogResult.OK)
                    {
                        // Step 2: Swap seamlessly to frmNewReservation in its original natural dimensions
                        using (frmNewReservation resModal = new frmNewReservation())
                        {
                            resModal.StartPosition = FormStartPosition.CenterParent;
                            resModal.ShowDialog(overlay);
                        }
                    }
                }
            }
            finally
            {
                // Clean up overlay after all modals close
                overlay.Dispose();
            }
        }


       // endregion

        // --- Sign Out & Form Lifecycle Handlers ---


        private void PerformSignOut()
        {
            // Ask for confirmation before logging out
            DialogResult result = MessageBox.Show(
                "Are you sure you want to sign out?",
                "Confirm Sign Out",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                // Set sign-out flag so FormClosed doesn't kill the whole app
                isLoggingOut = true;

                // Stop clock timer if active
                if (clockTimer != null)
                {
                    clockTimer.Stop();
                    clockTimer.Dispose();
                }

                // Open new login form instance
                GrandSereneHotel.LoginModule.frmLogin loginForm = new GrandSereneHotel.LoginModule.frmLogin();
                loginForm.Show();

                // Close current dashboard form
                this.Close();
            }
        }

        private void frmMainDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            // If the user closed the window directly (without clicking Sign Out), exit app completely
            if (!isLoggingOut)
            {
                Application.Exit();
            }
        }

        //endregion

        // Unused Navigation Stubs & Designer Placeholders ---

        private void btnReservations_Click(object sender, EventArgs e) { }
        private void btnRooms_Click(object sender, EventArgs e) { }
        private void btnGuests_Click(object sender, EventArgs e) { }
        private void btnFinance_Click(object sender, EventArgs e) { }
        private void btnStaff_Click(object sender, EventArgs e) { }

        private void pnlDashmain_Paint(object sender, PaintEventArgs e) { }
        private void guna2HtmlLabel2_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel3_Click(object sender, EventArgs e) { }
        private void ucDashboardHome_Load(object sender, EventArgs e) { }
        private void lblCurrentDate_Click(object sender, EventArgs e) { }
        private void pnlLeftpanel_Paint(object sender, PaintEventArgs e) { }
        private void PicNotif_Click(object sender, EventArgs e) { }

        #endregion

        private void btnSuperAdmin_Click(object sender, EventArgs e)
        {
            ucSuperAdmin staffControl = new ucSuperAdmin();
            staffControl.LoadStaffData();
            LoadView(staffControl);
        }

        private void cmsProfileMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void picProfile_Click(object sender, EventArgs e)
        {
            cmsProfileMenu.Show(picProfile, new Point(0, picProfile.Height));
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PerformSignOut();
        }
    }
}
