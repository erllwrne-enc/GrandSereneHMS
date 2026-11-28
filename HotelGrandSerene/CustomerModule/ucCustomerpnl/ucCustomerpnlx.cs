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

        // Gets the selected check-in date from the date picker (date for c-i/c-o)
        public DateTime CheckInDate => guna2DateTimePicker1.Value.Date;

        // Gets the selected check-out date from the date picker (date for c-i/c-o)
        public DateTime CheckOutDate => guna2DateTimePicker2.Value.Date;

        // Gets the current number of selected rooms.
        public int RoomsCount => roomsCount;

        // Gets the current number of adults.
        public int AdultsCount => adultsCount;

        // Gets the current number of children.
        public int ChildrenCount => childrenCount;

        //event that notifies the main form when the customer clicks BOOK NOW (the yellow button BOOK NOW)
        public event EventHandler BookNowClicked;

        public ucCustomerpnlx()
        {
            InitializeComponent();

            // Get today actual date (date for c-i/c-o)
            DateTime today = DateTime.Today;

            // Prevent the customer from selecting a check-in date before today (date for c-i/c-o)
            guna2DateTimePicker1.MinDate = today;

            // Prevent the customer from selecting a check-out date before today (date for c-i/c-o)
            guna2DateTimePicker2.MinDate = today;

            // Set the default check-in date to today (date for c-i/c-o)
            guna2DateTimePicker1.Value = today;

            // Set the default check-out date to tomorrow (date for c-i/c-o)
            guna2DateTimePicker2.Value = today.AddDays(1);

            // Listen for changes whenever the customer selects a new check-in date (date for c-i/c-o)
            guna2DateTimePicker1.ValueChanged += guna2DateTimePicker1_ValueChanged;

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

        private void btnBookproceed_Click(object sender, EventArgs e) // This method runs when the customer clicks the BOOK NOW button (the yellow button BOOK NOW)
        {
            // Retrieve the selected check-in date without the time component.
            DateTime checkIn = guna2DateTimePicker1.Value.Date;

            // Retrieve the selected check-out date without the time component.
            DateTime checkOut = guna2DateTimePicker2.Value.Date;

            // Make sure the check-in date is not earlier than today.
            if (checkIn < DateTime.Today)
            {
                MessageBox.Show(
                    "Check-in date cannot be earlier than today.",
                    "Invalid Check-in Date",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return; // Stop the booking process if the date is invalid.
            }

            // Make sure checkout is later than check-in.
            if (checkOut <= checkIn)
            {
                MessageBox.Show(
                    "Check-out date must be later than check-in date.",
                    "Invalid Check-out Date",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return; // Stop the booking process if the date is invalid.
            }

            // Notify the main form that the customer wants to proceed.
            BookNowClicked?.Invoke(this, EventArgs.Empty);
        }
        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // Retrieve the newly selected check-in date.
            DateTime checkIn = guna2DateTimePicker1.Value.Date;

            // The checkout date must be at least one day after check-in.
            DateTime minimumCheckOut = checkIn.AddDays(1);

            // Check whether the current checkout date is invalid.
            if (guna2DateTimePicker2.Value.Date < minimumCheckOut)
            {
                // Automatically move checkout to the day after check-in.
                guna2DateTimePicker2.Value = minimumCheckOut;
            }

            // Update the earliest selectable checkout date.
            guna2DateTimePicker2.MinDate = minimumCheckOut;
        }
    }
}
