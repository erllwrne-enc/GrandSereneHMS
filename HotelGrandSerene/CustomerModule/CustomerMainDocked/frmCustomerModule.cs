using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HotelGrandSerene.CustomerModule.ucCustomerpnl;

namespace HotelGrandSerene.CustomerModule
{
    public partial class frmCustomerModule : Form
    {
        // Stores the existing Room Selection page.
        // This prevents previously selected rooms from being lost
        // when the customer clicks Add another room.
        private ucRoomSelectionPage currentRoomPage;

        public frmCustomerModule()
        {
            InitializeComponent();

            // Set greeting and formatted current date on initialization
            SetHeaderGreetingAndDate();

            // Load main customer user control panel
            LoadCustomerPanel();
        }

        private void SetHeaderGreetingAndDate()
        {
            DateTime now = DateTime.Now;

            // 1. Dynamic greeting based on current system hour
            int hour = now.Hour;
            string greeting;

            if (hour >= 5 && hour < 12)
            {
                greeting = "Good Morning!";
            }
            else if (hour >= 12 && hour < 18)
            {
                greeting = "Good Afternoon!";
            }
            else
            {
                greeting = "Good Evening!";
            }

            lblGreetings.Text = greeting;

            // 2. Format date with full day name (e.g., "Saturday, October 17, 2026")
            lblCurrentDate.Text = now.ToString("dddd, MMMM dd, yyyy");
        }

        private void LoadCustomerPanel()
        {
            // Enable scrolling on the host panel
            pnlMaindash.AutoScroll = true;

            // Clear any existing controls
            pnlMaindash.Controls.Clear();

            // Instantiate and add user control
            ucCustomerpnlx customerUc = new ucCustomerpnlx();
            // Subscribe to the BOOK NOW event from the customer UserControl (the yellow button BOOK NOW)
            customerUc.BookNowClicked += CustomerUc_BookNowClicked; // this id meth
            customerUc.Dock = DockStyle.Top; // Docking top allows vertical auto-scroll

            pnlMaindash.Controls.Add(customerUc);
            customerUc.BringToFront();
        }

        // Reusable helper method to clear pnlDashmain and load new UserControls
        private void LoadView(UserControl uc)
        {
            pnlMaindash.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlMaindash.Controls.Add(uc);
            uc.BringToFront();
        }

        // Event handlers
        private void lblCurrentDate_Click(object sender, EventArgs e) { }
        private void pnlMaindash_Paint(object sender, PaintEventArgs e) { }
        private void lblCurrentDate_Click_1(object sender, EventArgs e) { }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void btnBookRoomPage_Click(object sender, EventArgs e) // Returns the customer to the initial booking page. (the book a room button)
        {
            // Create a new customer booking UserControl.
            ucCustomerpnlx customerUc = new ucCustomerpnlx();

            // Subscribe to its BOOK NOW event so navigation works again.
            customerUc.BookNowClicked += CustomerUc_BookNowClicked;

            // Display the customer booking page.
            LoadView(customerUc);
        }

        private void btnReservationPage_Click(object sender, EventArgs e)
        {

        }

        private void btnOrderPage_Click(object sender, EventArgs e)
        {

        }

        private void frmCustomerModule_Load(object sender, EventArgs e)
        {

        }

        // This method runs when the customer clicks BOOK NOW (the yellow button BOOK NOW)
        private void CustomerUc_BookNowClicked(object sender, EventArgs e) // Thia is that methode
        {
            // Get the customer UserControl that raised the event.
            ucCustomerpnlx customerUc = sender as ucCustomerpnlx;

            // Stop if the event sender is not the expected UserControl.
            if (customerUc == null)
            {
                return;
            }

            // Create and store the Room Selection page.
            // We will reuse this same instance when adding another room.
            currentRoomPage = new ucRoomSelectionPage();

            // Subscribe to the event raised when the customer selects rooms.
            currentRoomPage.RoomSelected += RoomPage_RoomSelected;
            // Transfer the customer's selected dates and guest counts.
            currentRoomPage.SetBookingDetails(
                customerUc.CheckInDate,
                customerUc.CheckOutDate,
                customerUc.RoomsCount,
                customerUc.AdultsCount,
                customerUc.ChildrenCount
            );

            // Display the Room Selection page.
            LoadView(currentRoomPage);
        }

        // Handles the event when a customer selects a room.
        private void RoomPage_RoomSelected(object sender, EventArgs e)
        {
            // Get the existing Room Selection page.
            ucRoomSelectionPage roomPage = sender as ucRoomSelectionPage;

            // Stop if the sender is invalid.
            if (roomPage == null)
            {
                return;
            }

            // Create the Checkout UserControl.
            ucCheckoutdetails checkoutPage = new ucCheckoutdetails();

            // Subscribe to the Add another room button event.
            // This connects ucCheckoutdetails to frmCustomerModule.
            checkoutPage.AddAnotherRoomClicked += CheckoutPage_AddAnotherRoomClicked;

            // Subscribe to the Back to room selection button event.
            checkoutPage.BackToRoomSelectionClicked += CheckoutPage_BackToRoomSelectionClicked;

            // Transfer ALL selected rooms and booking dates to checkout.
            checkoutPage.SetCheckoutDetails(
                roomPage.SelectedRooms,
                roomPage.CheckInDate,
                roomPage.CheckOutDate
            );

            // Display the checkout page.
            LoadView(checkoutPage);
        }

        // Runs when the customer clicks Add another room in checkout. coonect this to even in checkout
        private void CheckoutPage_AddAnotherRoomClicked(object sender, EventArgs e)
        {
            // Make sure the existing Room Selection page exists.
            if (currentRoomPage == null)
            {
                return;
            }

            // Activate the mode for selecting one additional room.
            currentRoomPage.PrepareToAddAnotherRoom();

            // Return to the EXISTING Room Selection page.
            // Previously selected rooms remain stored in SelectedRooms.
            LoadView(currentRoomPage);
        }

        // Runs when the customer clicks Back to room selection.
        private void CheckoutPage_BackToRoomSelectionClicked(object sender, EventArgs e)
        {
            // Check whether the existing Room Selection page is available.
            if (currentRoomPage == null)
            {
                return;
            }

            // Return to the existing page without creating a new one.
            LoadView(currentRoomPage);
        }

    }
}

