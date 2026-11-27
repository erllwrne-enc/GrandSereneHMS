using GrandSereneHotel.DashboardModule.MainDocked;
using GrandSereneHotel.DatabaseAccess;
using GrandSereneHotel.LoginModule;
using HotelGrandSerene.CustomerModule;
using HotelGrandSerene.LoginModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelGrandSerene

{
    internal static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if NETCOREAPP || NET5_0_OR_GREATER
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
#else
            SetProcessDPIAware(); // fallback for .NET Framework
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);

            bool hasEmployees;
            try
            {
                hasEmployees = EmployeeSetup.AnyEmployeeExists();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot reach the database.\n\n" + ex.Message +
                                "\n\nCheck the connection string in App.config and that SQL Server is running.",
                                "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // =====================================================================
            // OPTIONAL DIAGNOSTIC (for the "EMPLOYEE table is empty in SSMS" problem)
            // Paste into Program.Main right after the "hasEmployees" try/catch, run once,
            // then delete it. It shows EXACTLY which server + database the app is using.
            // Needs:   using GrandSereneHotel.DatabaseAccess;   at the top of Program.cs
            // =====================================================================

            using (var c = DatabaseHelper.GetConnection())
            {
                c.Open();
                using (var cmd = new System.Data.SqlClient.SqlCommand("SELECT @@SERVERNAME + '  /  ' + DB_NAME()", c))
                    MessageBox.Show("The app is using:\n\n" + cmd.ExecuteScalar(), "Diagnostic");
            }

            // Only show the "create first admin" screen when there are NO accounts at all
            // (a fallback in case 03_seed_admin.sql was never run).
            if (!hasEmployees)
            {
                Application.Run(new frmAdministratorInitialize());
                if (!EmployeeSetup.AnyEmployeeExists()) return;   // closed without creating one
            }

            Application.Run(new frmLogin());

            //Application.Run(new frmCustomerModule());
            //Application.Run(new frmMainDashboard());


        }
    }
}