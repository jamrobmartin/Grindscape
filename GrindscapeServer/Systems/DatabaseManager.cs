using GrindscapeServer.Data;

namespace GrindscapeServer.Systems
{
    public class DatabaseManager : ISystem
    {
        #region SystemInterface
        public string SystemName => "DatabaseManager";

        public ESystemType SystemType => ESystemType.Singleton;

        private ESystemStatus _systemStatus = ESystemStatus.Uninitialized;
        public ESystemStatus SystemStatus
        {
            get => _systemStatus;
            set
            {
                if (_systemStatus != value)
                {
                    _systemStatus = value;
                    OnSystemStatusChanged(new SystemStatusChangedEventArgs(value));
                }
            }
        }

        public event EventHandler<SystemStatusChangedEventArgs>? SystemStatusChanged;

        protected virtual void OnSystemStatusChanged(SystemStatusChangedEventArgs e)
        {
            SystemStatusChanged?.Invoke(this, e);
        }

        public ManualResetEvent SystemShutdownEvent { get; set; } = new ManualResetEvent(false);

        public void InitializeSystem()
        {
            Initialize();
        }

        public void StartSystem()
        {
            // This System is always on, so nothing to do on Start/Stop
        }

        public void StopSystem()
        {
            // This System is always on, so nothing to do on Start/Stop

        }

        public void ShutdownSystem()
        {
            SystemShutdownEvent.Reset();
            Shutdown();
        }

        public void UpdateSystemState()
        {

            SystemStatus = Database.Initialized ? ESystemStatus.Initialized : ESystemStatus.Uninitialized;

            if (SystemStatus == ESystemStatus.Uninitialized)
            {
                SystemShutdownEvent.Set();
            }
        }
        #endregion

        #region Singleton
        private static readonly Lazy<DatabaseManager> instance = new(() => new DatabaseManager(), LazyThreadSafetyMode.ExecutionAndPublication);
        public static DatabaseManager Instance { get; private set; } = instance.Value;
        #endregion

        // Private Fields
        private readonly GSDatabase Database = new();

        public bool Initialized { get; private set; } = false;

        private void Initialize()
        {
            Database.Initialize();
            UpdateSystemState();

            LogMessage("Database Initialized!");

        }

        private void Shutdown()
        {
            Database.Shutdown();
            UpdateSystemState();

            LogMessage("Database Shutdown!");
        }


        #region Logging

        // Logger Wrapper

        // Logs a message. If the level is not set, it is assumed to be info
        private void LogMessage(string message, Logger.LogLevel logLevel = Logger.LogLevel.Info)
        {
            Logger.LoggerMessage loggerMessage = new(logLevel, SystemName, message);
            Logger.Instance.LogMessage(loggerMessage);
        }

        #endregion


        #region DatabaseClass




        #endregion

    }
}
