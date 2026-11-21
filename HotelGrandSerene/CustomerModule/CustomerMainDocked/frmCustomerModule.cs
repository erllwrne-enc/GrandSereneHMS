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

        private void btnBookRoomPage_Click(object sender, EventArgs e)
        {
            LoadView(new ucCustomerpnlx());
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
    }
}

