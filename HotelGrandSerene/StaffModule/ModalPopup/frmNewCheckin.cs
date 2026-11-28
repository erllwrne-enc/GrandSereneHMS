using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrandSereneHotel.DatabaseAccess;

namespace HotelGrandSerene.DashboardModule.ModalPopup
{
    public partial class frmNewCheckin : Form
    {
        // The reservation that was found (null if the window was cancelled)
        public StayInfo FoundStay { get; private set; }

        public frmNewCheckin()
        {
            InitializeComponent();

            // 1. Ensure transparency is off so the form stays solid white
            this.AllowTransparency = false;
            this.BackColor = Color.White;

            // 2. Set form window borders and position
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;

            // 3. Apply rounded corners
            guna2Elipse1.TargetControl = this;
            guna2Elipse1.BorderRadius = 20;

            // 4. The designer pre-fills all three boxes with sample text
            //    ("ex. DTR-12342", "ex. Dela Cruz", "ex. Juan"). Clear them and use hints instead.
            guna2TextBox1.Text = "";
            guna2TextBox1.PlaceholderText = "e.g. R-10001";
            guna2TextBox2.Text = "";
            guna2TextBox2.PlaceholderText = "e.g. Dela Cruz";
            guna2TextBox3.Text = "";
            guna2TextBox3.PlaceholderText = "e.g. Juan";

            guna2Button1.Text = "Load Reservation";

            // 5. Enter = load, Esc = cancel (this window has no close button)
            this.AcceptButton = guna2Button1;
            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape) this.Close();
            };
            this.Shown += (s, e) => guna2TextBox1.Focus();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            int reservationId;
            if (!StayService.TryParseCode(guna2TextBox1.Text, out reservationId))
            {
                MessageBox.Show("Please enter a valid order code, for example R-10001.",
                    "Check-in", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                guna2TextBox1.Focus();
                return;
            }

            string lastName = guna2TextBox2.Text.Trim();
            string firstName = guna2TextBox3.Text.Trim();
            if (lastName == "" || firstName == "")
            {
                MessageBox.Show("Please enter the guest's last name and first name.",
                    "Check-in", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                StayInfo info = StayService.Find(reservationId);

                // Same message for "no such code" and "wrong name" so codes can't be guessed
                if (info == null ||
                    !string.Equals(info.LastName, lastName, StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(info.FirstName, firstName, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("No reservation matches that order code and guest name.",
                        "Check-in", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (info.Status != "Confirmed" && info.Status != "Pending")
                {
                    MessageBox.Show("This reservation can't be checked in (status: " + info.Status + ").",
                        "Check-in", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Found: hand it to the Dashboard. Nothing is saved here.
                FoundStay = info;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not look up the reservation:\n\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
