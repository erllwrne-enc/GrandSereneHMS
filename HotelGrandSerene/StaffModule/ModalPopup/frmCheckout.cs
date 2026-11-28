// It has no designer: the whole window is built in code, styled like frmNewCheckin.

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
        private readonly Label lblDetails = new Label();
        private readonly Label lblMethod = new Label();
        private readonly Guna2ComboBox cmbMethod = new Guna2ComboBox();
        private readonly Guna2Button btnConfirm = new Guna2Button();
        private readonly Guna2Button btnCancel = new Guna2Button();
        private readonly Guna2Elipse elipse = new Guna2Elipse();

        private StayInfo current;   // the reservation that was found

        public frmCheckout()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.ClientSize = new Size(466, 480);
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) this.Close(); };

            elipse.TargetControl = this;
            elipse.BorderRadius = 20;

            Label title = new Label();
            title.Text = "Guest Check-out";
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

            lblDetails.Font = new Font("Segoe UI", 10F);
            lblDetails.ForeColor = Color.FromArgb(40, 40, 60);
            lblDetails.Location = new Point(22, 136);
            lblDetails.Size = new Size(405, 180);
            lblDetails.Text = "Enter the order code and press Find.";

            lblMethod.Text = "Payment method for the remaining balance";
            lblMethod.Font = new Font("Segoe UI", 8F);
            lblMethod.ForeColor = SystemColors.ControlDarkDark;
            lblMethod.AutoSize = true;
            lblMethod.Location = new Point(22, 326);
            lblMethod.Visible = false;

            cmbMethod.BorderRadius = 10;
            cmbMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMethod.Font = new Font("Segoe UI", 10F);
            cmbMethod.ItemHeight = 28;
            cmbMethod.Location = new Point(22, 346);
            cmbMethod.Size = new Size(405, 36);
            cmbMethod.Items.AddRange(new object[] { "Cash", "Credit / Debit Card", "Bank Transfer  / E-wallet" });
            cmbMethod.SelectedIndex = 0;
            cmbMethod.Visible = false;

            StyleButton(btnConfirm, "Confirm Check-out", Navy, Color.White);
            btnConfirm.Location = new Point(22, 398);
            btnConfirm.Size = new Size(405, 38);
            btnConfirm.Enabled = false;
            btnConfirm.Click += (s, e) => DoCheckout();

            StyleButton(btnCancel, "Cancel", Color.White, Color.Gray);
            btnCancel.BorderThickness = 1;
            btnCancel.BorderColor = Color.Silver;
            btnCancel.Location = new Point(22, 442);
            btnCancel.Size = new Size(405, 30);
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.Add(title);
            this.Controls.Add(codeCaption);
            this.Controls.Add(txtCode);
            this.Controls.Add(btnFind);
            this.Controls.Add(lblDetails);
            this.Controls.Add(lblMethod);
            this.Controls.Add(cmbMethod);
            this.Controls.Add(btnConfirm);
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

        private static string Peso(decimal amount)
        {
            return "\u20B1" + amount.ToString("N2");
        }

        private void DoFind()
        {
            current = null;
            btnConfirm.Enabled = false;
            btnConfirm.Text = "Confirm Check-out";
            lblMethod.Visible = false;
            cmbMethod.Visible = false;

            int reservationId;
            if (!StayService.TryParseCode(txtCode.Text, out reservationId))
            {
                lblDetails.Text = "Please enter a valid order code, for example R-10001.";
                return;
            }

            try
            {
                StayInfo info = StayService.Find(reservationId);
                if (info == null)
                {
                    lblDetails.Text = "No reservation found for that order code.";
                    return;
                }

                string text =
                    "Guest:  " + info.GuestName + "\r\n" +
                    "Room:  " + info.RoomNumber + " (" + info.TypeName + ")\r\n" +
                    "Stay:  " + info.CheckIn.ToString("MMM dd") + " - " + info.CheckOut.ToString("MMM dd, yyyy") +
                        " (" + info.Nights + " night" + (info.Nights == 1 ? "" : "s") + ")\r\n" +
                    "Status:  " + info.Status + "\r\n\r\n" +
                    "Total:  " + Peso(info.Total) + "\r\n" +
                    "Paid:  " + Peso(info.Paid) + "\r\n" +
                    "Balance due:  " + Peso(info.Balance);

                if (info.Status != "Checked-In")
                {
                    text += "\r\n\r\nThis guest is not checked in, so they can't be checked out.";
                    lblDetails.Text = text;
                    return;
                }

                lblDetails.Text = text;
                current = info;
                btnConfirm.Enabled = true;

                if (info.Balance > 0m)
                {
                    lblMethod.Visible = true;
                    cmbMethod.Visible = true;
                    btnConfirm.Text = "Collect " + Peso(info.Balance) + " and Check-out";
                }
            }
            catch (Exception ex)
            {
                lblDetails.Text = "Could not look up the reservation:\r\n" + ex.Message;
            }
        }

        private void DoCheckout()
        {
            if (current == null) return;

            string method = cmbMethod.SelectedItem != null ? cmbMethod.SelectedItem.ToString() : "Cash";

            if (current.Balance > 0m)
            {
                DialogResult ok = MessageBox.Show(
                    "Collect " + Peso(current.Balance) + " by " + method + " and check out " + current.GuestName + "?",
                    "Confirm Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ok != DialogResult.Yes) return;
            }

            decimal collected;
            string error = StayService.CheckOut(current.ReservationId, method, out collected);
            if (error != null)
            {
                MessageBox.Show(error, "Check-out", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(
                "Check-out complete!\n\nGuest: " + current.GuestName +
                "\nRoom " + current.RoomNumber + " is now marked 'Cleaning'." +
                (collected > 0m ? "\nCollected: " + Peso(collected) : ""),
                "Checked Out", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}