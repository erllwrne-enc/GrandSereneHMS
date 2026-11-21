namespace HotelGrandSerene.CustomerModule
{
    partial class frmCustomerModule
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCustomerModule));
            this.pnlToppanel = new Guna.UI2.WinForms.Guna2Panel();
            this.btnOrderPage = new Guna.UI2.WinForms.Guna2Button();
            this.btnReservationPage = new Guna.UI2.WinForms.Guna2Button();
            this.btnBookRoomPage = new Guna.UI2.WinForms.Guna2Button();
            this.lblCurrentDate = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.lblGreetings = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.pnlMaindash = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlToppanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlToppanel
            // 
            this.pnlToppanel.BackColor = System.Drawing.Color.Transparent;
            this.pnlToppanel.BorderColor = System.Drawing.Color.Transparent;
            this.pnlToppanel.BorderThickness = 1;
            this.pnlToppanel.Controls.Add(this.btnOrderPage);
            this.pnlToppanel.Controls.Add(this.btnReservationPage);
            this.pnlToppanel.Controls.Add(this.btnBookRoomPage);
            this.pnlToppanel.Controls.Add(this.lblCurrentDate);
            this.pnlToppanel.Controls.Add(this.guna2PictureBox1);
            this.pnlToppanel.Controls.Add(this.lblGreetings);
            this.pnlToppanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToppanel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(1)))), ((int)(((byte)(101)))));
            this.pnlToppanel.Location = new System.Drawing.Point(0, 0);
            this.pnlToppanel.Name = "pnlToppanel";
            this.pnlToppanel.ShadowDecoration.BorderRadius = 5;
            this.pnlToppanel.ShadowDecoration.Enabled = true;
            this.pnlToppanel.ShadowDecoration.Parent = this.pnlToppanel;
            this.pnlToppanel.Size = new System.Drawing.Size(1902, 129);
            this.pnlToppanel.TabIndex = 2;
            // 
            // btnOrderPage
            // 
            this.btnOrderPage.CheckedState.Parent = this.btnOrderPage;
            this.btnOrderPage.CustomImages.Parent = this.btnOrderPage;
            this.btnOrderPage.FillColor = System.Drawing.Color.Transparent;
            this.btnOrderPage.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOrderPage.ForeColor = System.Drawing.Color.White;
            this.btnOrderPage.HoverState.Parent = this.btnOrderPage;
            this.btnOrderPage.Image = ((System.Drawing.Image)(resources.GetObject("btnOrderPage.Image")));
            this.btnOrderPage.Location = new System.Drawing.Point(1698, 48);
            this.btnOrderPage.Name = "btnOrderPage";
            this.btnOrderPage.ShadowDecoration.Parent = this.btnOrderPage;
            this.btnOrderPage.Size = new System.Drawing.Size(174, 45);
            this.btnOrderPage.TabIndex = 7;
            this.btnOrderPage.Text = "Order Restaurant";
            this.btnOrderPage.Click += new System.EventHandler(this.btnOrderPage_Click);
            // 
            // btnReservationPage
            // 
            this.btnReservationPage.CheckedState.Parent = this.btnReservationPage;
            this.btnReservationPage.CustomImages.Parent = this.btnReservationPage;
            this.btnReservationPage.FillColor = System.Drawing.Color.Transparent;
            this.btnReservationPage.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReservationPage.ForeColor = System.Drawing.Color.White;
            this.btnReservationPage.HoverState.Parent = this.btnReservationPage;
            this.btnReservationPage.Image = ((System.Drawing.Image)(resources.GetObject("btnReservationPage.Image")));
            this.btnReservationPage.Location = new System.Drawing.Point(1556, 48);
            this.btnReservationPage.Name = "btnReservationPage";
            this.btnReservationPage.ShadowDecoration.Parent = this.btnReservationPage;
            this.btnReservationPage.Size = new System.Drawing.Size(130, 45);
            this.btnReservationPage.TabIndex = 6;
            this.btnReservationPage.Text = "Reservations";
            this.btnReservationPage.Click += new System.EventHandler(this.btnReservationPage_Click);
            // 
            // btnBookRoomPage
            // 
            this.btnBookRoomPage.CheckedState.Parent = this.btnBookRoomPage;
            this.btnBookRoomPage.CustomImages.Parent = this.btnBookRoomPage;
            this.btnBookRoomPage.FillColor = System.Drawing.Color.Transparent;
            this.btnBookRoomPage.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBookRoomPage.ForeColor = System.Drawing.Color.White;
            this.btnBookRoomPage.HoverState.Parent = this.btnBookRoomPage;
            this.btnBookRoomPage.Image = ((System.Drawing.Image)(resources.GetObject("btnBookRoomPage.Image")));
            this.btnBookRoomPage.Location = new System.Drawing.Point(1411, 48);
            this.btnBookRoomPage.Name = "btnBookRoomPage";
            this.btnBookRoomPage.ShadowDecoration.Parent = this.btnBookRoomPage;
            this.btnBookRoomPage.Size = new System.Drawing.Size(130, 45);
            this.btnBookRoomPage.TabIndex = 5;
            this.btnBookRoomPage.Text = "Book a Room";
            this.btnBookRoomPage.Click += new System.EventHandler(this.btnBookRoomPage_Click);
            // 
            // lblCurrentDate
            // 
            this.lblCurrentDate.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentDate.ForeColor = System.Drawing.Color.White;
            this.lblCurrentDate.Location = new System.Drawing.Point(44, 71);
            this.lblCurrentDate.Name = "lblCurrentDate";
            this.lblCurrentDate.Size = new System.Drawing.Size(128, 22);
            this.lblCurrentDate.TabIndex = 4;
            this.lblCurrentDate.Text = "guna2HtmlLabel4";
            this.lblCurrentDate.Click += new System.EventHandler(this.lblCurrentDate_Click_1);
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.guna2PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("guna2PictureBox1.Image")));
            this.guna2PictureBox1.Location = new System.Drawing.Point(863, -19);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.Padding = new System.Windows.Forms.Padding(1);
            this.guna2PictureBox1.ShadowDecoration.Parent = this.guna2PictureBox1;
            this.guna2PictureBox1.Size = new System.Drawing.Size(192, 180);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2PictureBox1.TabIndex = 3;
            this.guna2PictureBox1.TabStop = false;
            this.guna2PictureBox1.UseTransparentBackground = true;
            // 
            // lblGreetings
            // 
            this.lblGreetings.BackColor = System.Drawing.Color.Transparent;
            this.lblGreetings.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreetings.ForeColor = System.Drawing.Color.White;
            this.lblGreetings.Location = new System.Drawing.Point(44, 32);
            this.lblGreetings.Name = "lblGreetings";
            this.lblGreetings.Size = new System.Drawing.Size(195, 33);
            this.lblGreetings.TabIndex = 2;
            this.lblGreetings.Text = "guna2HtmlLabel4";
            this.lblGreetings.Click += new System.EventHandler(this.lblCurrentDate_Click);
            // 
            // pnlMaindash
            // 
            this.pnlMaindash.BackColor = System.Drawing.Color.Transparent;
            this.pnlMaindash.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMaindash.FillColor = System.Drawing.Color.SeaShell;
            this.pnlMaindash.Location = new System.Drawing.Point(0, 129);
            this.pnlMaindash.Name = "pnlMaindash";
            this.pnlMaindash.ShadowDecoration.Parent = this.pnlMaindash;
            this.pnlMaindash.Size = new System.Drawing.Size(1902, 904);
            this.pnlMaindash.TabIndex = 3;
            this.pnlMaindash.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlMaindash_Paint);
            // 
            // frmCustomerModule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1902, 1033);
            this.Controls.Add(this.pnlMaindash);
            this.Controls.Add(this.pnlToppanel);
            this.Name = "frmCustomerModule";
            this.Text = "frmCustomerModule";
            this.Load += new System.EventHandler(this.frmCustomerModule_Load);
            this.pnlToppanel.ResumeLayout(false);
            this.pnlToppanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlToppanel;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblGreetings;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Guna.UI2.WinForms.Guna2Panel pnlMaindash;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblCurrentDate;
        private Guna.UI2.WinForms.Guna2Button btnOrderPage;
        private Guna.UI2.WinForms.Guna2Button btnReservationPage;
        private Guna.UI2.WinForms.Guna2Button btnBookRoomPage;
    }
}