// It has no designer: the whole window is built in code, styled like frmNewCheckin.

// This window now only FINDS a guest who is checked in. It does NOT check anyone out.
// The Dashboard reads FoundStay, shows the details in the "Guest Check-out" card,
// and the check-out happens when the staff presses "Confirm Check-out" there.

using Guna.UI2.WinForms;
using GrandSereneHotel.DatabaseAccess;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HotelGrandSerene.DashboardModule.ModalPopup
{
    public class frmCheckout : Form
    {
        private static readonly Color Navy = Color.FromArgb(6, 1, 101);

        private readonly Guna2TextBox txtCode = new Guna2TextBox();
        private readonly Guna2Button btnFind = new Guna2Button();
        private readonly Label lblMessage = new Label();
        private readonly Guna2Button btnCancel = new Guna2Button();
        private readonly Guna2Elipse elipse = new Guna2Elipse();

        // The reservation that was found (null if the window was cancelled)
        public StayInfo FoundStay { get; private set; }

        public frmCheckout()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.ClientSize = new Size(466, 290);
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) this.Close(); };
            this.Shown += (s, e) => txtCode.Focus();

            elipse.TargetControl = this;
            elipse.BorderRadius = 20;

            Label title = new Label();
            title.Text = "Find Guest for Check-out";
            title.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            title.ForeColor = Navy;
            title.AutoSize = true;
            title.Location = new Point(22, 20);

            Label codeCaption = new Label();
            codeCaption.Text = "Order Code";
            codeCaption.Font = new Font("Segoe UI", 8F);
            codeCaption.ForeColor = SystemColors.ControlDarkDark;
            codeCaption.AutoSize = true;
            codeCaption.Location = new Point(22, 62);

            txtCode.BorderRadius = 10;
            txtCode.Font = new Font("Segoe UI", 10F);
            txtCode.PlaceholderText = "e.g. R-10001";
            txtCode.Location = new Point(22, 82);
            txtCode.Size = new Size(300, 36);
            txtCode.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; DoFind(); }
            };

            StyleButton(btnFind, "Find", Navy, Color.White);
            btnFind.Location = new Point(332, 82);
            btnFind.Size = new Size(95, 36);
            btnFind.Click += (s, e) => DoFind();

            lblMessage.Font = new Font("Segoe UI", 10F);
            lblMessage.ForeColor = Color.FromArgb(40, 40, 60);
            lblMessage.Location = new Point(22, 135);
            lblMessage.Size = new Size(405, 90);
            lblMessage.Text = "Enter the order code of a guest who is checked in. " +
                              "Their details will be loaded into the Guest Check-out card.";

            StyleButton(btnCancel, "Cancel", Color.White, Color.Gray);
            btnCancel.BorderThickness = 1;
            btnCancel.BorderColor = Color.Silver;
            btnCancel.Location = new Point(22, 240);
            btnCancel.Size = new Size(405, 30);
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.Add(title);
            this.Controls.Add(codeCaption);
            this.Controls.Add(txtCode);
            this.Controls.Add(btnFind);
            this.Controls.Add(lblMessage);
            this.Controls.Add(btnCancel);
        }

        private static void StyleButton(Guna2Button b, string text, Color fill, Color fore)
        {
            b.Text = text;
            b.BorderRadius = 10;
            b.FillColor = fill;
            b.ForeColor = fore;
            b.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
        }

        private void ShowMessage(string text)
        {
            lblMessage.ForeColor = Color.FromArgb(185, 28, 28);
            lblMessage.Text = text;
        }

        private void DoFind()
        {
            int reservationId;
            if (!StayService.TryParseCode(txtCode.Text, out reservationId))
            {
                ShowMessage("Please enter a valid order code, for example R-10001.");
                return;
            }

            try
            {
                StayInfo info = StayService.Find(reservationId);

                if (info == null)
                {
                    ShowMessage("No reservation found for that order code.");
                    return;
                }
                if (info.Status != "Checked-In")
                {
                    ShowMessage("This guest is not checked in (status: " + info.Status +
                                "), so they can't be checked out.");
                    return;
                }

                // Found: hand it to the Dashboard. Nothing is saved here.
                FoundStay = info;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                ShowMessage("Could not look up the reservation:\r\n" + ex.Message);
            }
        }
    }
}
