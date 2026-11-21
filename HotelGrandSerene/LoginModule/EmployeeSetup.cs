using GrandSereneHotel.DatabaseAccess;
using GrandSereneHotel.LoginModule;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandSereneHotel.LoginModule
{
    public static class EmployeeSetup
    {
        public static void CreateFirstAdmin(
            string firstName,
            string lastName,
            string username,
            string password,
            string contactNumber)
        {
            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrEmpty(password))
            {
                throw new ArgumentException(
                    "Name, username, and password are required.");
            }

            string passwordHash =
                PasswordHelper.HashPassword(password);

            using (SqlConnection connection =
                   DatabaseHelper.GetConnection())
            {
                connection.Open();

                using (SqlTransaction transaction =
                       connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command =
                               new SqlCommand(@"
SET XACT_ABORT ON;

IF EXISTS (SELECT 1 FROM EMPLOYEE WITH (UPDLOCK, HOLDLOCK))
    THROW 50001, 'An employee account already exists.', 1;

INSERT INTO EMPLOYEE
    (FirstName, LastName, Username, Password, Role, ContactNumber)
VALUES
    (@FirstName, @LastName, @Username, @Password, 'Super Admin', @ContactNumber);",
                               connection, transaction))
                        {
                            command.Parameters.AddWithValue(
                                "@FirstName", firstName.Trim());

                            command.Parameters.AddWithValue(
                                "@LastName", lastName.Trim());

                            command.Parameters.AddWithValue(
                                "@Username", username.Trim());

                            command.Parameters.AddWithValue(
                                "@Password", passwordHash);

                            command.Parameters.AddWithValue(
                                "@ContactNumber",
                                string.IsNullOrWhiteSpace(contactNumber)
                                    ? (object)DBNull.Value
                                    : contactNumber.Trim());

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public static bool AnyEmployeeExists()
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM EMPLOYEE", connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }
    }
}

