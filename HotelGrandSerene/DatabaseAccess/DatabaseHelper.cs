using System.Configuration;
using System.Data.SqlClient;

namespace GrandSereneHotel.DatabaseAccess
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString =
        ConfigurationManager
        .ConnectionStrings["HMSConnection"]
        .ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

    }
}