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
            // Application.Run(new frmAdministratorInitialize());
             //Application.Run(new frmLogin());
            Application.Run(new frmCustomerModule());
            //  Application.Run(new frmMainDashboard());


        }
    }
}