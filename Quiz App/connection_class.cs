using Quiz_App.Properties;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Quiz_App
{
    public enum DatabaseMode
    {
        Local,
        Azure,
        Offline
    }

    public static class connection_class
    {
        private const string LocalConnectionName = "quiz_local";
        private const string AzureConnectionName = "quiz_azure";
        private const string AzurePlaceholderToken = "your-server-name.database.windows.net";

        public static DatabaseMode CurrentMode
        {
            get
            {
                string rawValue = Settings.Default.ActiveDatabaseMode;
                return Enum.TryParse(rawValue, true, out DatabaseMode mode)
                    ? mode
                    : DatabaseMode.Local;
            }
        }

        public static string CurrentConnectionString
        {
            get
            {
                if (CurrentMode == DatabaseMode.Offline)
                {
                    return string.Empty;
                }

                SyncActiveConnectionString();
                return Settings.Default.quizAppConnectionString;
            }
        }

        public static string LocalConnectionString => ResolveConnectionString(DatabaseMode.Local);

        public static string AzureConnectionString => ResolveConnectionString(DatabaseMode.Azure);

        public static bool HasAzureConfiguration
        {
            get
            {
                string connectionString = ResolveConnectionString(DatabaseMode.Azure);
                return !string.IsNullOrWhiteSpace(connectionString)
                    && connectionString.IndexOf(AzurePlaceholderToken, StringComparison.OrdinalIgnoreCase) < 0;
            }
        }

        public static SqlConnectionStringBuilder GetConnectionDetails(DatabaseMode mode)
        {
            if (mode == DatabaseMode.Offline)
            {
                return null;
            }

            return new SqlConnectionStringBuilder(ResolveConnectionString(mode));
        }

        public static void Initialize()
        {
            if (CurrentMode != DatabaseMode.Offline)
            {
                SyncActiveConnectionString();
            }
        }

        public static void SetMode(DatabaseMode mode)
        {
            if (mode == DatabaseMode.Azure && !HasAzureConfiguration)
            {
                throw new InvalidOperationException("Azure SQL is not configured yet. Set the Azure server, database, username, and password first.");
            }

            Settings.Default.ActiveDatabaseMode = mode.ToString();

            if (mode != DatabaseMode.Offline)
            {
                SyncActiveConnectionString(mode);
            }

            Settings.Default.Save();

            if (mode != DatabaseMode.Offline)
            {
                TheorySchemaInstaller.TryEnsureTheoryInfrastructure(out _);
            }
        }

        public static void ConfigureLocalConnection(string server, string database, string userId, string password, bool trustServerCertificate = true, bool encrypt = true, bool multipleActiveResultSets = true, int timeoutSeconds = 30)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = database,
                UserID = userId,
                Password = password,
                Encrypt = encrypt,
                TrustServerCertificate = trustServerCertificate,
                MultipleActiveResultSets = multipleActiveResultSets,
                ConnectTimeout = timeoutSeconds
            };

            Settings.Default.LocalConnectionStringOverride = builder.ConnectionString;

            if (CurrentMode == DatabaseMode.Local)
            {
                SyncActiveConnectionString(DatabaseMode.Local);
            }

            Settings.Default.Save();
        }

        public static void ConfigureAzureConnection(string server, string database, string userId, string password, bool trustServerCertificate = false, bool encrypt = true, bool multipleActiveResultSets = false, int timeoutSeconds = 30)
        {
            if (string.IsNullOrWhiteSpace(server))
            {
                throw new ArgumentException("Azure server name is required.", nameof(server));
            }

            string normalizedServer = server.Trim();
            if (!normalizedServer.StartsWith("tcp:", StringComparison.OrdinalIgnoreCase))
            {
                normalizedServer = "tcp:" + normalizedServer;
            }

            if (!normalizedServer.Contains(","))
            {
                normalizedServer += ",1433";
            }

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = normalizedServer,
                InitialCatalog = string.IsNullOrWhiteSpace(database) ? "quizApp" : database.Trim(),
                UserID = userId,
                Password = password,
                Encrypt = encrypt,
                TrustServerCertificate = trustServerCertificate,
                MultipleActiveResultSets = multipleActiveResultSets,
                ConnectTimeout = timeoutSeconds,
                PersistSecurityInfo = false
            };

            Settings.Default.AzureConnectionStringOverride = builder.ConnectionString;

            if (CurrentMode == DatabaseMode.Azure)
            {
                SyncActiveConnectionString(DatabaseMode.Azure);
            }

            Settings.Default.Save();
        }

        public static bool TryOpenConnection(out string message)
        {
            if (CurrentMode == DatabaseMode.Offline)
            {
                message = "Database access is paused on the launch page.";
                return false;
            }

            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                }

                message = $"Connected successfully using {GetModeLabel(CurrentMode)}.";
                return true;
            }
            catch (Exception ex)
            {
                message = $"Connection failed for {GetModeLabel(CurrentMode)}. {ex.Message}";
                return false;
            }
        }

        public static SqlConnection GetConnection()
        {
            if (CurrentMode == DatabaseMode.Offline)
            {
                throw new InvalidOperationException("Database access is paused. Choose Local SQL or Azure SQL from the launch page.");
            }

            return new SqlConnection(CurrentConnectionString);
        }

        public static string GetModeLabel(DatabaseMode mode)
        {
            switch (mode)
            {
                case DatabaseMode.Azure:
                    return "Azure SQL";
                case DatabaseMode.Offline:
                    return "Paused";
                default:
                    return "Local SQL";
            }
        }

        private static void SyncActiveConnectionString()
        {
            SyncActiveConnectionString(CurrentMode);
        }

        private static void SyncActiveConnectionString(DatabaseMode mode)
        {
            if (mode == DatabaseMode.Offline)
            {
                return;
            }

            Settings.Default.quizAppConnectionString = ResolveConnectionString(mode);
        }

        private static string ResolveConnectionString(DatabaseMode mode)
        {
            string overrideValue = mode == DatabaseMode.Azure
                ? Settings.Default.AzureConnectionStringOverride
                : Settings.Default.LocalConnectionStringOverride;

            if (!string.IsNullOrWhiteSpace(overrideValue))
            {
                return overrideValue;
            }

            string connectionName = mode == DatabaseMode.Azure
                ? AzureConnectionName
                : LocalConnectionName;

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connectionName];
            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new ConfigurationErrorsException($"Missing connection string '{connectionName}' in App.config.");
            }

            return settings.ConnectionString;
        }
    }
}
