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

            // 4. Start with empty boxes and a hint instead of the sample text
            guna2TextBox1.Text = "";
            guna2TextBox1.PlaceholderText = "e.g. R-10001";

            // 5. Enter = confirm, Esc = cancel (this window has no close button)
            this.AcceptButton = guna2Button1;
            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape) this.Close();
            };
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

                string error = StayService.CheckIn(reservationId);
                if (error != null)
                {
                    MessageBox.Show(error, "Check-in", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show(
                    "Check-in complete!\n\nGuest: " + info.GuestName +
                    "\nRoom: " + info.RoomNumber + " (" + info.TypeName + ")" +
                    "\nBalance due at check-out: \u20B1" + info.Balance.ToString("N2"),
                    "Checked In", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not check in:\n\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
