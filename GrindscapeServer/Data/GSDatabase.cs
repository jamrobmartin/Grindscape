using GrindscapeServer.Systems;
using SQLite;

namespace GrindscapeServer.Data
{

    public class GSDatabase
    {
        private SQLiteConnection? connection;
        private static readonly string DatabasePath = "Master.db";

        public GSDatabase()
        {

        }

        public bool Initialized { get => connection?.Handle != null && !connection.Handle.IsClosed; }

        public void Initialize()
        {
            // If we are still in development, we are just going to wipe and recreate the DB on each start up.
            bool Development = true;
            if (Development)
            {
                // Create the database file
                File.Create(DatabasePath).Close();
            }

            // Establish the connection to the DB
            connection = new SQLiteConnection(DatabasePath);

            // Create the DB tables. This method calls a "IF NOT EXISTS" query so it will only
            // create a new table if one doesnt already exist.
            connection.CreateTable<GSUser>();
            connection.CreateTable<GSCharacter>();

            // We want to make sure that an ADMIN account always exists.
            GSUser user = new()
            {
                Username = "ADMIN",
                Password = "PASSWORD"
            };

            Insert(user);

            GSCharacter character = new()
            {
                UserID = user.UserID,
                Name = "Itachi"
            };

            Insert(character);

        }

        public void Shutdown()
        {
            connection?.Close();
        }

        #region DB Commands

        public int Insert(object obj)
        {
            if (connection == null)
            {
                return -1;
            }

            connection.BeginTransaction();

            int result = 0;
            try
            {
                result = connection.Insert(obj);
            }
            catch (Exception ex)
            {
                LogMessage(ex.ToString(), Logger.LogLevel.Error);
            }

            connection.Commit();

            return result;
        }

        #endregion

        #region Logging

        // Logger Wrapper
        private readonly string SystemName = "GSDatabase";
        // Logs a message. If the level is not set, it is assumed to be info
        private void LogMessage(string message, Logger.LogLevel logLevel = Logger.LogLevel.Info)
        {
            Logger.LoggerMessage loggerMessage = new(logLevel, SystemName, message);
            Logger.Instance.LogMessage(loggerMessage);
        }

        #endregion
    }

}
