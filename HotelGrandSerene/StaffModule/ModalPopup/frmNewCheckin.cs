using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}