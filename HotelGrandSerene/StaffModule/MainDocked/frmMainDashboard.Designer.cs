namespace GrandSereneHotel.DashboardModule.MainDocked
{
    public partial class frmMainDashboard
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainDashboard));
            this.pnlDashmain = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlToppanel = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.picProfile = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.PicNotif = new Guna.UI2.WinForms.Guna2PictureBox();
            this.btnNewReservation = new Guna.UI2.WinForms.Guna2Button();
            this.guna2HtmlLabel2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblCurrentDate = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel3 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.pnlLeftpanel = new Guna.UI2.WinForms.Guna2Panel();
            this.btnSuperAdmin = new Guna.UI2.WinForms.Guna2Button();
            this.btnStaff = new Guna.UI2.WinForms.Guna2Button();
            this.btnFinance = new Guna.UI2.WinForms.Guna2Button();
            this.btnGuests = new Guna.UI2.WinForms.Guna2Button();
            this.btnRooms = new Guna.UI2.WinForms.Guna2Button();
            this.btnReservation = new Guna.UI2.WinForms.Guna2Button();
            this.btnDashboard = new Guna.UI2.WinForms.Guna2Button();
            this.cmsProfileMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.signOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlToppanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picProfile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicNotif)).BeginInit();
            this.pnlLeftpanel.SuspendLayout();
            this.cmsProfileMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDashmain
            // 
            this.pnlDashmain.BackColor = System.Drawing.Color.Transparent;
            this.pnlDashmain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlDashmain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDashmain.FillColor = System.Drawing.Color.SeaShell;
            this.pnlDashmain.Location = new System.Drawing.Point(254, 105);
            this.pnlDashmain.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlDashmain.Name = "pnlDashmain";
            this.pnlDashmain.ShadowDecoration.BorderRadius = 10;
            this.pnlDashmain.ShadowDecoration.Color = System.Drawing.SystemColors.ControlDarkDark;
            this.pnlDashmain.ShadowDecoration.Enabled = true;
            this.pnlDashmain.ShadowDecoration.Parent = this.pnlDashmain;
            this.pnlDashmain.Size = new System.Drawing.Size(1172, 734);
            this.pnlDashmain.TabIndex = 2;
            this.pnlDashmain.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDashmain_Paint);
            // 
            // pnlToppanel
            // 
            this.pnlToppanel.BackColor = System.Drawing.Color.White;
            this.pnlToppanel.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.pnlToppanel.BorderThickness = 1;
            this.pnlToppanel.Controls.Add(this.guna2PictureBox1);
            this.pnlToppanel.Controls.Add(this.picProfile);
            this.pnlToppanel.Controls.Add(this.PicNotif);
            this.pnlToppanel.Controls.Add(this.btnNewReservation);
            this.pnlToppanel.Controls.Add(this.guna2HtmlLabel2);
            this.pnlToppanel.Controls.Add(this.guna2HtmlLabel1);
            this.pnlToppanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToppanel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(1)))), ((int)(((byte)(101)))));
            this.pnlToppanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(1)))), ((int)(((byte)(101)))));
            this.pnlToppanel.Location = new System.Drawing.Point(0, 0);
            this.pnlToppanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlToppanel.Name = "pnlToppanel";
            this.pnlToppanel.ShadowDecoration.BorderRadius = 5;
            this.pnlToppanel.ShadowDecoration.Enabled = true;
            this.pnlToppanel.ShadowDecoration.Parent = this.pnlToppanel;
            this.pnlToppanel.Size = new System.Drawing.Size(1426, 105);
            this.pnlToppanel.TabIndex = 1;
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.guna2PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("guna2PictureBox1.Image")));
            this.guna2PictureBox1.Location = new System.Drawing.Point(31, -12);
            this.guna2PictureBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.ShadowDecoration.Parent = this.guna2PictureBox1;
            this.guna2PictureBox1.Size = new System.Drawing.Size(101, 98);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2PictureBox1.TabIndex = 7;
            this.guna2PictureBox1.TabStop = false;
            this.guna2PictureBox1.UseTransparentBackground = true;
            // 
            // picProfile
            // 
            this.picProfile.BackColor = System.Drawing.Color.Transparent;
            this.picProfile.FillColor = System.Drawing.Color.White;
            this.picProfile.Location = new System.Drawing.Point(1324, 34);
            this.picProfile.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.picProfile.Name = "picProfile";
            this.picProfile.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.picProfile.ShadowDecoration.Parent = this.picProfile;
            this.picProfile.Size = new System.Drawing.Size(32, 32);
            this.picProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picProfile.TabIndex = 5;
            this.picProfile.TabStop = false;
            this.picProfile.Click += new System.EventHandler(this.picProfile_Click);
            // 
            // PicNotif
            // 
            this.PicNotif.BackColor = System.Drawing.Color.Transparent;
            this.PicNotif.Image = ((System.Drawing.Image)(resources.GetObject("PicNotif.Image")));
            this.PicNotif.Location = new System.Drawing.Point(1256, 34);
            this.PicNotif.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PicNotif.Name = "PicNotif";
            this.PicNotif.ShadowDecoration.Parent = this.PicNotif;
            this.PicNotif.Size = new System.Drawing.Size(31, 32);
            this.PicNotif.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicNotif.TabIndex = 4;
            this.PicNotif.TabStop = false;
            this.PicNotif.UseTransparentBackground = true;
            this.PicNotif.Click += new System.EventHandler(this.PicNotif_Click);
            // 
            // btnNewReservation
            // 
            this.btnNewReservation.BackColor = System.Drawing.Color.Transparent;
            this.btnNewReservation.BorderRadius = 10;
            this.btnNewReservation.CheckedState.Parent = this.btnNewReservation;
            this.btnNewReservation.CustomImages.Parent = this.btnNewReservation;
            this.btnNewReservation.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(1)))), ((int)(((byte)(101)))));
            this.btnNewReservation.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewReservation.ForeColor = System.Drawing.Color.White;
            this.btnNewReservation.HoverState.Parent = this.btnNewReservation;
            this.btnNewReservation.Image = ((System.Drawing.Image)(resources.GetObject("btnNewReservation.Image")));
            this.btnNewReservation.ImageOffset = new System.Drawing.Point(-4, 0);
            this.btnNewReservation.Location = new System.Drawing.Point(1053, 34);
            this.btnNewReservation.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnNewReservation.Name = "btnNewReservation";
            this.btnNewReservation.ShadowDecoration.Parent = this.btnNewReservation;
            this.btnNewReservation.Size = new System.Drawing.Size(168, 32);
            this.btnNewReservation.TabIndex = 3;
            this.btnNewReservation.Text = "New Reservation";
            this.btnNewReservation.TextOffset = new System.Drawing.Point(2, 0);
            this.btnNewReservation.Click += new System.EventHandler(this.btnNewReservation_Click);
            // 
            // guna2HtmlLabel2
            // 
            this.guna2HtmlLabel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel2.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel2.Location = new System.Drawing.Point(146, 57);
            this.guna2HtmlLabel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            this.guna2HtmlLabel2.Size = new System.Drawing.Size(39, 17);
            this.guna2HtmlLabel2.TabIndex = 1;
            this.guna2HtmlLabel2.Text = "HOTEL";
            this.guna2HtmlLabel2.Click += new System.EventHandler(this.guna2HtmlLabel2_Click);
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel1.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(146, 34);
            this.guna2HtmlLabel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(91, 17);
            this.guna2HtmlLabel1.TabIndex = 0;
            this.guna2HtmlLabel1.Text = "GRAND SERENE";
            // 
            // lblCurrentDate
            // 
            this.lblCurrentDate.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(1)))), ((int)(((byte)(101)))));
            this.lblCurrentDate.Location = new System.Drawing.Point(22, 81);
            this.lblCurrentDate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblCurrentDate.Name = "lblCurrentDate";
            this.lblCurrentDate.Size = new System.Drawing.Size(101, 17);
            this.lblCurrentDate.TabIndex = 2;
            this.lblCurrentDate.Text = "guna2HtmlLabel4";
            this.lblCurrentDate.Click += new System.EventHandler(this.lblCurrentDate_Click);
            // 
            // guna2HtmlLabel3
            // 
            this.guna2HtmlLabel3.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel3.Font = new System.Drawing.Font("Bodoni MT", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(1)))), ((int)(((byte)(101)))));
            this.guna2HtmlLabel3.Location = new System.Drawing.Point(36, 46);
            this.guna2HtmlLabel3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.guna2HtmlLabel3.Name = "guna2HtmlLabel3";
            this.guna2HtmlLabel3.Size = new System.Drawing.Size(178, 30);
            this.guna2HtmlLabel3.TabIndex = 1;
            this.guna2HtmlLabel3.Text = "Today\'s Overview";
            this.guna2HtmlLabel3.Click += new System.EventHandler(this.guna2HtmlLabel3_Click);
            // 
            // pnlLeftpanel
            // 
            this.pnlLeftpanel.BackColor = System.Drawing.Color.SeaShell;
            this.pnlLeftpanel.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.pnlLeftpanel.BorderThickness = 1;
            this.pnlLeftpanel.Controls.Add(this.btnSuperAdmin);
            this.pnlLeftpanel.Controls.Add(this.btnStaff);
            this.pnlLeftpanel.Controls.Add(this.btnFinance);
            this.pnlLeftpanel.Controls.Add(this.btnGuests);
            this.pnlLeftpanel.Controls.Add(this.btnRooms);
            this.pnlLeftpanel.Controls.Add(this.lblCurrentDate);
            this.pnlLeftpanel.Controls.Add(this.btnReservation);
            this.pnlLeftpanel.Controls.Add(this.guna2HtmlLabel3);
            this.pnlLeftpanel.Controls.Add(this.btnDashboard);
            this.pnlLeftpanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeftpanel.Location = new System.Drawing.Point(0, 105);
            this.pnlLeftpanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlLeftpanel.Name = "pnlLeftpanel";
            this.pnlLeftpanel.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlLeftpanel.ShadowDecoration.BorderRadius = 20;
            this.pnlLeftpanel.ShadowDecoration.Enabled = true;
            this.pnlLeftpanel.ShadowDecoration.Parent = this.pnlLeftpanel;
            this.pnlLeftpanel.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(0);
            this.pnlLeftpanel.Size = new System.Drawing.Size(254, 734);
            this.pnlLeftpanel.TabIndex = 0;
            this.pnlLeftpanel.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlLeftpanel_Paint);
            // 
            // btnSuperAdmin
            // 
            this.btnSuperAdmin.BackColor = System.Drawing.Color.Transparent;
            this.btnSuperAdmin.BorderRadius = 10;
            this.btnSuperAdmin.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnSuperAdmin.CheckedState.Parent = this.btnSuperAdmin;
            this.btnSuperAdmin.CustomImages.Parent = this.btnSuperAdmin;
            this.btnSuperAdmin.FillColor = System.Drawing.Color.Transparent;
            this.btnSuperAdmin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSuperAdmin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(1)))), ((int)(((byte)(101)))));
            this.btnSuperAdmin.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(177)))), ((int)(((byte)(75)))));
            this.btnSuperAdmin.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnSuperAdmin.HoverState.Parent = this.btnSuperAdmin;
            this.btnSuperAdmin.Image = ((System.Drawing.Image)(resources.GetObject("btnSuperAdmin.Image")));
            this.btnSuperAdmin.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnSuperAdmin.ImageSize = new System.Drawing.Size(25, 25);
            this.btnSuperAdmin.Location = new System.Drawing.Point(16, 575);
            this.btnSuperAdmin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSuperAdmin.Name = "btnSuperAdmin";
            this.btnSuperAdmin.ShadowDecoration.Parent = this.btnSuperAdmin;
            this.btnSuperAdmin.Size = new System.Drawing.Size(220, 54);
            this.btnSuperAdmin.TabIndex = 8;
            this.btnSuperAdmin.Text = "Super Admin";
            this.btnSuperAdmin.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnSuperAdmin.TextOffset = new System.Drawing.Point(15, 0);
            this.btnSuperAdmin.Click += new System.EventHandler(this.btnSuperAdmin_Click);
            // 
            // btnStaff
            // 
            this.btnStaff.BackColor = System.Drawing.Color.Transparent;
            this.btnStaff.BorderRadius = 10;
            this.btnStaff.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnStaff.CheckedState.Parent = this.btnStaff;
            this.btnStaff.CustomImages.Parent = this.btnStaff;
            this.btnStaff.FillColor = System.Drawing.Color.Transparent;
            this.btnStaff.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStaff.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(1)))), ((int)(((byte)(101)))));
            this.btnStaff.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(177)))), ((int)(((byte)(75)))));
            this.btnStaff.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnStaff.HoverState.Parent = this.btnStaff;
            this.btnStaff.Image = ((System.Drawing.Image)(resources.GetObject("btnStaff.Image")));
            this.btnStaff.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnStaff.ImageSize = new System.Drawing.Size(25, 25);
            this.btnStaff.Location = new System.Drawing.Point(16, 513);
            this.btnStaff.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnStaff.Name = "btnStaff";
            this.btnStaff.ShadowDecoration.Parent = this.btnStaff;
            this.btnStaff.Size = new System.Drawing.Size(220, 54);
            this.btnStaff.TabIndex = 7;
            this.btnStaff.Text = "Staff";
            this.btnStaff.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnStaff.TextOffset = new System.Drawing.Point(15, 0);
            this.btnStaff.Click += new System.EventHandler(this.btnStaff_Click_1);
            // 
            // btnFinance
            // 
            this.btnFinance.BackColor = System.Drawing.Color.Transparent;
            this.btnFinance.BorderRadius = 10;
            this.btnFinance.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnFinance.CheckedState.Parent = this.btnFinance;
            this.btnFinance.CustomImages.Parent = this.btnFinance;
            this.btnFinance.FillColor = System.Drawing.Color.Transparent;
            this.btnFinance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFinance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(1)))), ((int)(((byte)(101)))));
            this.btnFinance.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(177)))), ((int)(((byte)(75)))));
            this.btnFinance.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnFinance.HoverState.Parent = this.btnFinance;
            this.btnFinance.Image = ((System.Drawing.Image)(resources.GetObject("btnFinance.Image")));
            this.btnFinance.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnFinance.ImageSize = new System.Drawing.Size(25, 25);
            this.btnFinance.Location = new System.Drawing.Point(16, 436);
            this.btnFinance.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnFinance.Name = "btnFinance";
            this.btnFinance.ShadowDecoration.Parent = this.btnFinance;
            this.btnFinance.Size = new System.Drawing.Size(220, 54);
            this.btnFinance.TabIndex = 6;
            this.btnFinance.Text = "Finance";
            this.btnFinance.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnFinance.TextOffset = new System.Drawing.Point(15, 0);
            this.btnFinance.Click += new System.EventHandler(this.btnFinance_Click_1);
            // 
            // btnGuests
            // 
            this.btnGuests.BackColor = System.Drawing.Color.Transparent;
            this.btnGuests.BorderRadius = 10;
            this.btnGuests.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnGuests.CheckedState.Parent = this.btnGuests;
            this.btnGuests.CustomImages.Parent = this.btnGuests;
            this.btnGuests.FillColor = System.Drawing.Color.Transparent;
            this.btnGuests.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuests.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(1)))), ((int)(((byte)(101)))));
            this.btnGuests.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(177)))), ((int)(((byte)(75)))));
            this.btnGuests.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnGuests.HoverState.Parent = this.btnGuests;
            this.btnGuests.Image = ((System.Drawing.Image)(resources.GetObject("btnGuests.Image")));
            this.btnGuests.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnGuests.ImageSize = new System.Drawing.Size(25, 25);
            this.btnGuests.Location = new System.Drawing.Point(16, 364);
            this.btnGuests.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnGuests.Name = "btnGuests";
            this.btnGuests.ShadowDecoration.Parent = this.btnGuests;
            this.btnGuests.Size = new System.Drawing.Size(220, 54);
            this.btnGuests.TabIndex = 5;
            this.btnGuests.Text = "Guests";
            this.btnGuests.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnGuests.TextOffset = new System.Drawing.Point(15, 0);
            this.btnGuests.Click += new System.EventHandler(this.btnGuests_Click_1);
            // 
            // btnRooms
            // 
            this.btnRooms.BackColor = System.Drawing.Color.Transparent;
            this.btnRooms.BorderRadius = 10;
            this.btnRooms.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnRooms.CheckedState.Parent = this.btnRooms;
            this.btnRooms.CustomImages.Parent = this.btnRooms;
            this.btnRooms.FillColor = System.Drawing.Color.Transparent;
            this.btnRooms.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRooms.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(1)))), ((int)(((byte)(101)))));
            this.btnRooms.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(177)))), ((int)(((byte)(75)))));
            this.btnRooms.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnRooms.HoverState.Parent = this.btnRooms;
            this.btnRooms.Image = ((System.Drawing.Image)(resources.GetObject("btnRooms.Image")));
            this.btnRooms.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnRooms.ImageSize = new System.Drawing.Size(25, 25);
            this.btnRooms.Location = new System.Drawing.Point(16, 290);
            this.btnRooms.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRooms.Name = "btnRooms";
            this.btnRooms.ShadowDecoration.Parent = this.btnRooms;
            this.btnRooms.Size = new System.Drawing.Size(220, 54);
            this.btnRooms.TabIndex = 4;
            this.btnRooms.Text = "Rooms";
            this.btnRooms.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnRooms.TextOffset = new System.Drawing.Point(15, 0);
            this.btnRooms.Click += new System.EventHandler(this.btnRooms_Click_1);
            // 
            // btnReservation
            // 
            this.btnReservation.BackColor = System.Drawing.Color.Transparent;
            this.btnReservation.BorderRadius = 10;
            this.btnReservation.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnReservation.CheckedState.Parent = this.btnReservation;
            this.btnReservation.CustomImages.Parent = this.btnReservation;
            this.btnReservation.FillColor = System.Drawing.Color.Transparent;
            this.btnReservation.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReservation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(1)))), ((int)(((byte)(101)))));
            this.btnReservation.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(177)))), ((int)(((byte)(75)))));
            this.btnReservation.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnReservation.HoverState.Parent = this.btnReservation;
            this.btnReservation.Image = ((System.Drawing.Image)(resources.GetObject("btnReservation.Image")));
            this.btnReservation.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnReservation.ImageSize = new System.Drawing.Size(25, 25);
            this.btnReservation.Location = new System.Drawing.Point(16, 221);
            this.btnReservation.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnReservation.Name = "btnReservation";
            this.btnReservation.ShadowDecoration.Parent = this.btnReservation;
            this.btnReservation.Size = new System.Drawing.Size(220, 54);
            this.btnReservation.TabIndex = 3;
            this.btnReservation.Text = "Reservations";
            this.btnReservation.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnReservation.TextOffset = new System.Drawing.Point(15, 0);
            this.btnReservation.Click += new System.EventHandler(this.btnReservation_Click);
            // 
            // btnDashboard
            // 
            this.btnDashboard.BackColor = System.Drawing.Color.Transparent;
            this.btnDashboard.BorderRadius = 10;
            this.btnDashboard.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnDashboard.Checked = true;
            this.btnDashboard.CheckedState.Parent = this.btnDashboard;
            this.btnDashboard.CustomImages.Parent = this.btnDashboard;
            this.btnDashboard.FillColor = System.Drawing.Color.Transparent;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDashboard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(1)))), ((int)(((byte)(101)))));
            this.btnDashboard.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(177)))), ((int)(((byte)(75)))));
            this.btnDashboard.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnDashboard.HoverState.Parent = this.btnDashboard;
            this.btnDashboard.Image = ((System.Drawing.Image)(resources.GetObject("btnDashboard.Image")));
            this.btnDashboard.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnDashboard.ImageSize = new System.Drawing.Size(25, 25);
            this.btnDashboard.Location = new System.Drawing.Point(16, 150);
            this.btnDashboard.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.ShadowDecoration.Parent = this.btnDashboard;
            this.btnDashboard.Size = new System.Drawing.Size(220, 54);
            this.btnDashboard.TabIndex = 2;
            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnDashboard.TextOffset = new System.Drawing.Point(15, 0);
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            // 
            // cmsProfileMenu
            // 
            this.cmsProfileMenu.BackColor = System.Drawing.Color.SeaShell;
            this.cmsProfileMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsProfileMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.signOutToolStripMenuItem});
            this.cmsProfileMenu.Name = "cmsProfileMenu";
            this.cmsProfileMenu.ShowImageMargin = false;
            this.cmsProfileMenu.Size = new System.Drawing.Size(96, 26);
            this.cmsProfileMenu.Opening += new System.ComponentModel.CancelEventHandler(this.cmsProfileMenu_Opening);
            // 
            // signOutToolStripMenuItem
            // 
            this.signOutToolStripMenuItem.Name = "signOutToolStripMenuItem";
            this.signOutToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.signOutToolStripMenuItem.Text = "Sign Out";
            this.signOutToolStripMenuItem.Click += new System.EventHandler(this.signOutToolStripMenuItem_Click);
            // 
            // frmMainDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1426, 839);
            this.Controls.Add(this.pnlDashmain);
            this.Controls.Add(this.pnlLeftpanel);
            this.Controls.Add(this.pnlToppanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "frmMainDashboard";
            this.Text = "frmMainDashboard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMainDashboard_Load);
            this.pnlToppanel.ResumeLayout(false);
            this.pnlToppanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picProfile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicNotif)).EndInit();
            this.pnlLeftpanel.ResumeLayout(false);
            this.pnlLeftpanel.PerformLayout();
            this.cmsProfileMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlDashmain;
        private Guna.UI2.WinForms.Guna2Panel pnlToppanel;
        private Guna.UI2.WinForms.Guna2Panel pnlLeftpanel;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2Button btnDashboard;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel2;
        private Guna.UI2.WinForms.Guna2Button btnReservation;
        private Guna.UI2.WinForms.Guna2Button btnStaff;
        private Guna.UI2.WinForms.Guna2Button btnFinance;
        private Guna.UI2.WinForms.Guna2Button btnGuests;
        private Guna.UI2.WinForms.Guna2Button btnRooms;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel3;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblCurrentDate;
        private Guna.UI2.WinForms.Guna2Button btnNewReservation;
        private Guna.UI2.WinForms.Guna2CirclePictureBox picProfile;
        private Guna.UI2.WinForms.Guna2PictureBox PicNotif;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Guna.UI2.WinForms.Guna2Button btnSuperAdmin;
        private System.Windows.Forms.ContextMenuStrip cmsProfileMenu;
        private System.Windows.Forms.ToolStripMenuItem signOutToolStripMenuItem;
    }
}