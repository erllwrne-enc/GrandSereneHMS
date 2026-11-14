using System;
using System.Drawing;
using System.Windows.Forms;

namespace HotelGrandSerene.DashboardModule.UserControlNavigation
{
    public partial class ucFinance : UserControl
    {
        private readonly Random _rand = new Random();

        public ucFinance()
        {
            InitializeComponent();

            // Run configuration and loading directly in constructor
            ConfigureGrid();
            PopulateData();
        }

        private void ConfigureGrid()
        {
            // Prevent WinForms from resetting designer columns
            dgvTransactions.AutoGenerateColumns = false;

            // Hide native column header bar
            dgvTransactions.ColumnHeadersVisible = false;

            // General grid settings
            dgvTransactions.AllowUserToAddRows = false;
            dgvTransactions.AllowUserToDeleteRows = false;
            dgvTransactions.ReadOnly = true;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactions.MultiSelect = false;

            // Row heights for 345px total height
            dgvTransactions.RowTemplate.Height = 48;
            dgvTransactions.ScrollBars = ScrollBars.None;

            // Ensure text color is dark solid slate (not transparent or white)
            dgvTransactions.DefaultCellStyle.ForeColor = Color.FromArgb(30, 41, 59);
            dgvTransactions.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 41, 59);
            dgvTransactions.DefaultCellStyle.SelectionBackColor = Color.FromArgb(241, 245, 249);
            dgvTransactions.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            // Bind dynamic formatting
            dgvTransactions.CellFormatting -= dgvTransactions_CellFormatting;
            dgvTransactions.CellFormatting += dgvTransactions_CellFormatting;
        }

        private void PopulateData()
        {
            dgvTransactions.Rows.Clear();

            string[] guests = { "Sydney Chua", "Shairyll Licunan", "Nathaniel Lalota", "Josh Herrera", "Ana Laboc", "Ephraim Perlas" };
            string[] roomTypes = { "Standard", "Deluxe", "Suite", "Executive" };
            string[] transactionTypes = { "Room Stay", "Room Stay", "Room Stay", "Dining", "Service" };
            int[] roomNumbers = { 101, 109, 205, 214, 312, 418 };

            for (int i = 0; i < 6; i++)
            {
                string guest = guests[i % guests.Length];
                string room = $"{roomNumbers[i % roomNumbers.Length]} {roomTypes[i % roomTypes.Length]}";
                string date = $"Aug {14 - (i / 2)}";
                string type = transactionTypes[_rand.Next(transactionTypes.Length)];
                string amount = $"${_rand.Next(20, 130) * 10:N0}";

                // Explicitly insert into the 5 columns by index
                int rowIndex = dgvTransactions.Rows.Add();
                DataGridViewRow row = dgvTransactions.Rows[rowIndex];

                row.Cells[0].Value = guest;
                row.Cells[1].Value = room;
                row.Cells[2].Value = date;
                row.Cells[3].Value = type;
                row.Cells[4].Value = amount;
            }

            dgvTransactions.ClearSelection();
        }

        private void dgvTransactions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            // Column 3: TYPE -> Accent Blue
            if (e.ColumnIndex == 3)
            {
                e.CellStyle.ForeColor = Color.FromArgb(37, 99, 235);
                e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            }
            // Column 4: AMOUNT -> Bold Dark Text
            else if (e.ColumnIndex == 4)
            {
                e.CellStyle.ForeColor = Color.FromArgb(30, 41, 59);
                e.CellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            }
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e) { }
        private void ucFinance_Load(object sender, EventArgs e) { }
        private void dgvTransactions_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void ucFinance_Load_1(object sender, EventArgs e)
        {

        }
    }
}

