// ONE place that defines the roles and who may see what.
// To change who can open a tab, edit the CanXxx lines at the bottom - nothing else.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelGrandSerene.LoginModule
{
    internal class Roles
    {
    }
}
namespace GrandSereneHotel.LoginModule
{
    public static class Roles
    {
        public const string FrontDesk = "Front Desk";
        public const string Housekeeping = "Housekeeping";
        public const string Manager = "Manager";      // admin
        public const string SuperAdmin = "Super Admin";  // IT / Coordinator

        // Lowest access FIRST, so a brand-new account defaults to the safest role.
        public static readonly string[] All = { FrontDesk, Housekeeping, Manager, SuperAdmin };
    }

    // Who is logged in right now. Filled in by frmLogin after a successful login.
    public static class CurrentUser
    {
        public static int EmployeeId;
        public static string FullName = "";
        public static string Role = "";

        public static bool Is(params string[] roles)
        {
            if (string.IsNullOrEmpty(Role)) return false;
            foreach (string r in roles)
                if (string.Equals(Role, r, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        // ---- Access rules (edit these to match what your leader decides) ----
        public static bool CanViewDashboard { get { return Is(Roles.SuperAdmin, Roles.Manager, Roles.FrontDesk); } }
        public static bool CanViewReservations { get { return Is(Roles.SuperAdmin, Roles.Manager, Roles.FrontDesk); } }
        public static bool CanCreateReservation { get { return Is(Roles.SuperAdmin, Roles.Manager, Roles.FrontDesk); } }
        public static bool CanViewRooms { get { return Is(Roles.SuperAdmin, Roles.Manager, Roles.FrontDesk, Roles.Housekeeping); } }
        public static bool CanViewGuests { get { return Is(Roles.SuperAdmin, Roles.Manager, Roles.FrontDesk); } }
        public static bool CanViewFinance { get { return Is(Roles.SuperAdmin, Roles.Manager); } }
        public static bool CanViewStaff { get { return Is(Roles.SuperAdmin, Roles.Manager); } }
        public static bool CanManageAccounts { get { return Is(Roles.SuperAdmin); } }   // the "Super Admin" tab
    }
}