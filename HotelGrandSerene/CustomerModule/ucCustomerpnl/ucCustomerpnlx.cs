using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelGrandSerene.CustomerModule.ucCustomerpnl
{
    public partial class ucCustomerpnlx : UserControl
    {
        // Internal counter values
        private int roomsCount = 1;
        private int adultsCount = 1;
        private int childrenCount = 0;

        public ucCustomerpnlx()
        {
            InitializeComponent();

            // Set default visibility & initial display state
            pnlGuestCountDrop.Visible = false;
            SyncUI();

            // Hide dropdown when clicking anywhere on the UserControl background
            this.Click += ucCustomerpnlx_Click;
        }

        // Centralized helper to update all labels and summary button text at once
        private void SyncUI()
        {
            lblRoomCount.Text = roomsCount.ToString();
            lblAdultCount.Text = adultsCount.ToString();
            lblChildCount.Text = childrenCount.ToString();

            string roomStr = $"{roomsCount} Room{(roomsCount > 1 ? "s" : "")}";
            string adultStr = $"{adultsCount} Adult{(adultsCount > 1 ? "s" : "")}";
            string childStr = $"{childrenCount} Child{(childrenCount != 1 ? "ren" : "")}";

            btnGuestCounter.Text = $"{roomStr}, {adultStr}, {childStr}";
        }

        // Toggle dropdown panel on/off
        private void btnGuestCounter_Click(object sender, EventArgs e)
        {
            pnlGuestCountDrop.Visible = !pnlGuestCountDrop.Visible;
            if (pnlGuestCountDrop.Visible)
            {
                pnlGuestCountDrop.BringToFront();
            }
        }

        // Auto-close dropdown when clicking outside
        private void ucCustomerpnlx_Click(object sender, EventArgs e)
        {
            if (pnlGuestCountDrop.Visible)
            {
                pnlGuestCountDrop.Visible = false;
            }
        }

        // ================= ROOMS LOGIC =================
        private void btnRoomsPlus_Click(object sender, EventArgs e)
        {
            roomsCount++;
            SyncUI();
        }

        private void btnRoomsMinus_Click(object sender, EventArgs e) // Ensure this is wired to your Rooms (-) button
        {
            if (roomsCount > 1) // Minimum 1 Room
            {
                roomsCount--;
                SyncUI();
            }
        }

        // ================= ADULTS LOGIC =================
        private void btnAdultsPlus_Click(object sender, EventArgs e) // Ensure this is wired to your Adults (+) button
        {
            adultsCount++;
            SyncUI();
        }

        private void btnAdultMinus_Click(object sender, EventArgs e) // Ensure this is wired to your Adults (-) button
        {
            if (adultsCount > 1) // Minimum 1 Adult
            {
                adultsCount--;
                SyncUI();
            }
        }

        // ================= CHILDREN LOGIC =================
        private void btnChildPlus_Click(object sender, EventArgs e) // Ensure this is wired to your Children (+) button
        {
            childrenCount++;
            SyncUI();
        }

        private void btnChildMinus_Click(object sender, EventArgs e) // Ensure this is wired to your Children (-) button
        {
            if (childrenCount > 0) // Minimum 0 Children
            {
                childrenCount--;
                SyncUI();
            }
        }

        

        // Standard designer backward-compatibility wrappers
        private void guna2Button5_Click(object sender, EventArgs e) => btnRoomsMinus_Click(sender, e);
        private void guna2Button7_Click(object sender, EventArgs e) => btnAdultsPlus_Click(sender, e);
        private void guna2Button1_Click(object sender, EventArgs e) => btnChildPlus_Click(sender, e);

        // Unused handlers
        private void guna2Button2_Click(object sender, EventArgs e) { }
        private void lblRoomCount_Click(object sender, EventArgs e) { }
        private void lblAdultCount_Click(object sender, EventArgs e) { }
        private void lblChildCount_Click(object sender, EventArgs e) { }
        private void pnlGuestCountDrop_Paint(object sender, PaintEventArgs e) { }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnBookproceed_Click(object sender, EventArgs e)
        {
            
        }
    }
}
