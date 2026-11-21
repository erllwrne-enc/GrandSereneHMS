namespace HotelGrandSerene.DashboardModule.UserControlNavigation
{
    partial class ucStaff
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlOnDutyCard = new Guna.UI2.WinForms.Guna2Panel();
            this.lblOnDutyCount = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.pnlOffDutyCard = new Guna.UI2.WinForms.Guna2Panel();
            this.lblOffDutyCount = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.pnlTotalStaffCard = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTotalStaff = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel3 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.dgvStaff = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colStaffMember = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShift = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSince = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.guna2Panel4 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2HtmlLabel8 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel6 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel5 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel4 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel7 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.txtbNameFilter = new Guna.UI2.WinForms.Guna2TextBox();
            this.pnlOnDutyCard.SuspendLayout();
            this.pnlOffDutyCard.SuspendLayout();
            this.pnlTotalStaffCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStaff)).BeginInit();
            this.guna2Panel4.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlOnDutyCard
            // 
            this.pnlOnDutyCard.BackColor = System.Drawing.Color.Transparent;
            this.pnlOnDutyCard.BorderColor = System.Drawing.Color.Lime;
            this.pnlOnDutyCard.BorderRadius = 20;
            this.pnlOnDutyCard.BorderThickness = 1;
            this.pnlOnDutyCard.Controls.Add(this.lblOnDutyCount);
            this.pnlOnDutyCard.Controls.Add(this.guna2HtmlLabel1);
            this.pnlOnDutyCard.FillColor = System.Drawing.Color.White;
            this.pnlOnDutyCard.Location = new System.Drawing.Point(22, 29);
            this.pnlOnDutyCard.Name = "pnlOnDutyCard";
            this.pnlOnDutyCard.ShadowDecoration.Parent = this.pnlOnDutyCard;
            this.pnlOnDutyCard.Size = new System.Drawing.Size(495, 122);
            this.pnlOnDutyCard.TabIndex = 0;
            // 
            // lblOnDutyCount
            // 
            this.lblOnDutyCount.BackColor = System.Drawing.Color.Transparent;
            this.lblOnDutyCount.Font = new System.Drawing.Font("Segoe UI Semibold", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOnDutyCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblOnDutyCount.Location = new System.Drawing.Point(214, 30);
            this.lblOnDutyCount.Name = "lblOnDutyCount";
            this.lblOnDutyCount.Size = new System.Drawing.Size(21, 47);
            this.lblOnDutyCount.TabIndex = 1;
            this.lblOnDutyCount.Text = "0";
            this.lblOnDutyCount.Click += new System.EventHandler(this.lblOnDutyCount_Click);
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(191, 80);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(79, 30);
            this.guna2HtmlLabel1.TabIndex = 0;
            this.guna2HtmlLabel1.Text = "On Duty";
            this.guna2HtmlLabel1.Click += new System.EventHandler(this.guna2HtmlLabel1_Click);
            // 
            // pnlOffDutyCard
            // 
            this.pnlOffDutyCard.BackColor = System.Drawing.Color.Transparent;
            this.pnlOffDutyCard.BorderColor = System.Drawing.Color.Silver;
            this.pnlOffDutyCard.BorderRadius = 20;
            this.pnlOffDutyCard.BorderThickness = 1;
            this.pnlOffDutyCard.Controls.Add(this.lblOffDutyCount);
            this.pnlOffDutyCard.Controls.Add(this.guna2HtmlLabel2);
            this.pnlOffDutyCard.FillColor = System.Drawing.Color.White;
            this.pnlOffDutyCard.Location = new System.Drawing.Point(536, 29);
            this.pnlOffDutyCard.Name = "pnlOffDutyCard";
            this.pnlOffDutyCard.ShadowDecoration.Parent = this.pnlOffDutyCard;
            this.pnlOffDutyCard.Size = new System.Drawing.Size(495, 122);
            this.pnlOffDutyCard.TabIndex = 1;
            // 
            // lblOffDutyCount
            // 
            this.lblOffDutyCount.BackColor = System.Drawing.Color.Transparent;
            this.lblOffDutyCount.Font = new System.Drawing.Font("Segoe UI Semibold", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOffDutyCount.ForeColor = System.Drawing.Color.DarkGray;
            this.lblOffDutyCount.Location = new System.Drawing.Point(231, 30);
            this.lblOffDutyCount.Name = "lblOffDutyCount";
            this.lblOffDutyCount.Size = new System.Drawing.Size(21, 47);
            this.lblOffDutyCount.TabIndex = 2;
            this.lblOffDutyCount.Text = "0";
            this.lblOffDutyCount.Click += new System.EventHandler(this.lblOffDutyCount_Click);
            // 
            // guna2HtmlLabel2
            // 
            this.guna2HtmlLabel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel2.ForeColor = System.Drawing.Color.DarkGray;
            this.guna2HtmlLabel2.Location = new System.Drawing.Point(209, 80);
            this.guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            this.guna2HtmlLabel2.Size = new System.Drawing.Size(81, 30);
            this.guna2HtmlLabel2.TabIndex = 1;
            this.guna2HtmlLabel2.Text = "Off Duty";
            // 
            // pnlTotalStaffCard
            // 
            this.pnlTotalStaffCard.BackColor = System.Drawing.Color.Transparent;
            this.pnlTotalStaffCard.BorderColor = System.Drawing.Color.Blue;
            this.pnlTotalStaffCard.BorderRadius = 20;
            this.pnlTotalStaffCard.BorderThickness = 1;
            this.pnlTotalStaffCard.Controls.Add(this.lblTotalStaff);
            this.pnlTotalStaffCard.Controls.Add(this.guna2HtmlLabel3);
            this.pnlTotalStaffCard.FillColor = System.Drawing.Color.White;
            this.pnlTotalStaffCard.Location = new System.Drawing.Point(1048, 29);
            this.pnlTotalStaffCard.Name = "pnlTotalStaffCard";
            this.pnlTotalStaffCard.ShadowDecoration.Parent = this.pnlTotalStaffCard;
            this.pnlTotalStaffCard.Size = new System.Drawing.Size(495, 122);
            this.pnlTotalStaffCard.TabIndex = 2;
            // 
            // lblTotalStaff
            // 
            this.lblTotalStaff.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalStaff.Font = new System.Drawing.Font("Segoe UI Semibold", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalStaff.ForeColor = System.Drawing.Color.Navy;
            this.lblTotalStaff.Location = new System.Drawing.Point(231, 30);
            this.lblTotalStaff.Name = "lblTotalStaff";
            this.lblTotalStaff.Size = new System.Drawing.Size(21, 47);
            this.lblTotalStaff.TabIndex = 2;
            this.lblTotalStaff.Text = "0";
            this.lblTotalStaff.Click += new System.EventHandler(this.lblTotalStaff_Click);
            // 
            // guna2HtmlLabel3
            // 
            this.guna2HtmlLabel3.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel3.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel3.ForeColor = System.Drawing.Color.Navy;
            this.guna2HtmlLabel3.Location = new System.Drawing.Point(201, 80);
            this.guna2HtmlLabel3.Name = "guna2HtmlLabel3";
            this.guna2HtmlLabel3.Size = new System.Drawing.Size(96, 30);
            this.guna2HtmlLabel3.TabIndex = 2;
            this.guna2HtmlLabel3.Text = "Total Staff";
            this.guna2HtmlLabel3.Click += new System.EventHandler(this.guna2HtmlLabel3_Click);
            // 
            // dgvStaff
            // 
            this.dgvStaff.AllowUserToAddRows = false;
            this.dgvStaff.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvStaff.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvStaff.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStaff.BackgroundColor = System.Drawing.Color.White;
            this.dgvStaff.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvStaff.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvStaff.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStaff.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvStaff.ColumnHeadersHeight = 27;
            this.dgvStaff.ColumnHeadersVisible = false;
            this.dgvStaff.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStaffMember,
            this.colRole,
            this.colShift,
            this.colSince,
            this.colStatus});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvStaff.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvStaff.EnableHeadersVisualStyles = false;
            this.dgvStaff.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvStaff.Location = new System.Drawing.Point(17, 108);
            this.dgvStaff.Name = "dgvStaff";
            this.dgvStaff.ReadOnly = true;
            this.dgvStaff.RowHeadersVisible = false;
            this.dgvStaff.RowHeadersWidth = 51;
            this.dgvStaff.RowTemplate.Height = 24;
            this.dgvStaff.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStaff.Size = new System.Drawing.Size(1509, 386);
            this.dgvStaff.TabIndex = 4;
            this.dgvStaff.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Default;
            this.dgvStaff.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvStaff.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvStaff.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvStaff.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvStaff.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvStaff.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvStaff.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvStaff.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvStaff.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvStaff.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dgvStaff.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvStaff.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvStaff.ThemeStyle.HeaderStyle.Height = 27;
            this.dgvStaff.ThemeStyle.ReadOnly = true;
            this.dgvStaff.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvStaff.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvStaff.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dgvStaff.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvStaff.ThemeStyle.RowsStyle.Height = 24;
            this.dgvStaff.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvStaff.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvStaff.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStaff_CellContentClick);
            // 
            // colStaffMember
            // 
            this.colStaffMember.HeaderText = "STAFF MEMBER";
            this.colStaffMember.MinimumWidth = 6;
            this.colStaffMember.Name = "colStaffMember";
            this.colStaffMember.ReadOnly = true;
            // 
            // colRole
            // 
            this.colRole.HeaderText = "ROLE";
            this.colRole.MinimumWidth = 6;
            this.colRole.Name = "colRole";
            this.colRole.ReadOnly = true;
            // 
            // colShift
            // 
            this.colShift.HeaderText = "SHIFT";
            this.colShift.MinimumWidth = 6;
            this.colShift.Name = "colShift";
            this.colShift.ReadOnly = true;
            // 
            // colSince
            // 
            this.colSince.HeaderText = "SINCE";
            this.colSince.MinimumWidth = 6;
            this.colSince.Name = "colSince";
            this.colSince.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "STATUS";
            this.colStatus.MinimumWidth = 6;
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // guna2Panel4
            // 
            this.guna2Panel4.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel4.BorderColor = System.Drawing.Color.Gray;
            this.guna2Panel4.BorderRadius = 20;
            this.guna2Panel4.BorderThickness = 1;
            this.guna2Panel4.Controls.Add(this.guna2Panel1);
            this.guna2Panel4.Controls.Add(this.dgvStaff);
            this.guna2Panel4.FillColor = System.Drawing.Color.White;
            this.guna2Panel4.Location = new System.Drawing.Point(22, 263);
            this.guna2Panel4.Name = "guna2Panel4";
            this.guna2Panel4.ShadowDecoration.Parent = this.guna2Panel4;
            this.guna2Panel4.Size = new System.Drawing.Size(1529, 511);
            this.guna2Panel4.TabIndex = 1;
            this.guna2Panel4.UseTransparentBackground = true;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel1.Controls.Add(this.guna2HtmlLabel8);
            this.guna2Panel1.Controls.Add(this.guna2HtmlLabel6);
            this.guna2Panel1.Controls.Add(this.guna2HtmlLabel5);
            this.guna2Panel1.Controls.Add(this.guna2HtmlLabel4);
            this.guna2Panel1.Controls.Add(this.guna2HtmlLabel7);
            this.guna2Panel1.FillColor = System.Drawing.Color.SeaShell;
            this.guna2Panel1.Location = new System.Drawing.Point(3, 48);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.ShadowDecoration.Parent = this.guna2Panel1;
            this.guna2Panel1.Size = new System.Drawing.Size(1523, 54);
            this.guna2Panel1.TabIndex = 5;
            // 
            // guna2HtmlLabel8
            // 
            this.guna2HtmlLabel8.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel8.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel8.ForeColor = System.Drawing.Color.Gray;
            this.guna2HtmlLabel8.Location = new System.Drawing.Point(1227, 18);
            this.guna2HtmlLabel8.Name = "guna2HtmlLabel8";
            this.guna2HtmlLabel8.Size = new System.Drawing.Size(49, 19);
            this.guna2HtmlLabel8.TabIndex = 5;
            this.guna2HtmlLabel8.Text = "STATUS";
            // 
            // guna2HtmlLabel6
            // 
            this.guna2HtmlLabel6.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel6.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel6.ForeColor = System.Drawing.Color.Gray;
            this.guna2HtmlLabel6.Location = new System.Drawing.Point(922, 18);
            this.guna2HtmlLabel6.Name = "guna2HtmlLabel6";
            this.guna2HtmlLabel6.Size = new System.Drawing.Size(39, 19);
            this.guna2HtmlLabel6.TabIndex = 4;
            this.guna2HtmlLabel6.Text = "SINCE";
            // 
            // guna2HtmlLabel5
            // 
            this.guna2HtmlLabel5.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel5.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel5.ForeColor = System.Drawing.Color.Gray;
            this.guna2HtmlLabel5.Location = new System.Drawing.Point(616, 18);
            this.guna2HtmlLabel5.Name = "guna2HtmlLabel5";
            this.guna2HtmlLabel5.Size = new System.Drawing.Size(38, 19);
            this.guna2HtmlLabel5.TabIndex = 3;
            this.guna2HtmlLabel5.Text = "SHIFT";
            // 
            // guna2HtmlLabel4
            // 
            this.guna2HtmlLabel4.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel4.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel4.ForeColor = System.Drawing.Color.Gray;
            this.guna2HtmlLabel4.Location = new System.Drawing.Point(315, 18);
            this.guna2HtmlLabel4.Name = "guna2HtmlLabel4";
            this.guna2HtmlLabel4.Size = new System.Drawing.Size(34, 19);
            this.guna2HtmlLabel4.TabIndex = 2;
            this.guna2HtmlLabel4.Text = "ROLE";
            // 
            // guna2HtmlLabel7
            // 
            this.guna2HtmlLabel7.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel7.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel7.ForeColor = System.Drawing.Color.Gray;
            this.guna2HtmlLabel7.Location = new System.Drawing.Point(14, 18);
            this.guna2HtmlLabel7.Name = "guna2HtmlLabel7";
            this.guna2HtmlLabel7.Size = new System.Drawing.Size(98, 19);
            this.guna2HtmlLabel7.TabIndex = 1;
            this.guna2HtmlLabel7.Text = "STAFF MEMBER";
            // 
            // txtbNameFilter
            // 
            this.txtbNameFilter.BorderColor = System.Drawing.Color.Gray;
            this.txtbNameFilter.BorderRadius = 10;
            this.txtbNameFilter.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtbNameFilter.DefaultText = "";
            this.txtbNameFilter.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtbNameFilter.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtbNameFilter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtbNameFilter.DisabledState.Parent = this.txtbNameFilter;
            this.txtbNameFilter.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtbNameFilter.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtbNameFilter.FocusedState.Parent = this.txtbNameFilter;
            this.txtbNameFilter.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtbNameFilter.HoverState.Parent = this.txtbNameFilter;
            this.txtbNameFilter.Location = new System.Drawing.Point(22, 199);
            this.txtbNameFilter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtbNameFilter.Name = "txtbNameFilter";
            this.txtbNameFilter.PasswordChar = '\0';
            this.txtbNameFilter.PlaceholderText = "Search Name...";
            this.txtbNameFilter.SelectedText = "";
            this.txtbNameFilter.ShadowDecoration.Parent = this.txtbNameFilter;
            this.txtbNameFilter.Size = new System.Drawing.Size(588, 45);
            this.txtbNameFilter.TabIndex = 7;
            // 
            // ucStaff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.Controls.Add(this.txtbNameFilter);
            this.Controls.Add(this.pnlTotalStaffCard);
            this.Controls.Add(this.pnlOffDutyCard);
            this.Controls.Add(this.pnlOnDutyCard);
            this.Controls.Add(this.guna2Panel4);
            this.Name = "ucStaff";
            this.Size = new System.Drawing.Size(1564, 904);
            this.pnlOnDutyCard.ResumeLayout(false);
            this.pnlOnDutyCard.PerformLayout();
            this.pnlOffDutyCard.ResumeLayout(false);
            this.pnlOffDutyCard.PerformLayout();
            this.pnlTotalStaffCard.ResumeLayout(false);
            this.pnlTotalStaffCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStaff)).EndInit();
            this.guna2Panel4.ResumeLayout(false);
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlOnDutyCard;
        private Guna.UI2.WinForms.Guna2Panel pnlOffDutyCard;
        private Guna.UI2.WinForms.Guna2Panel pnlTotalStaffCard;
        private Guna.UI2.WinForms.Guna2DataGridView dgvStaff;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStaffMember;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRole;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShift;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSince;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel4;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel2;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel3;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblOnDutyCount;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblOffDutyCount;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTotalStaff;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel8;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel6;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel5;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel4;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel7;
        private Guna.UI2.WinForms.Guna2TextBox txtbNameFilter;
    }
}
