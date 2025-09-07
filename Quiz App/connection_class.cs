using System;
using System.Data.SqlClient;

namespace Quiz_App
{
    public static class connection_class
    {
        private static readonly string connectionString =
            "Server=192.168.43.67,1433;" +
            "Database=quizApp;" +
            "User Id=quizusers;" +
            "Password=Jos@56567;" +
            "Encrypt=True;" +
            "TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static string ConnectionString => connectionString;
    }
}
