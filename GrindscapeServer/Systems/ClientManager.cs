
namespace GrindscapeServer.Systems
{
    public class ClientManager : ISystem
    {
        #region SystemInterface
        public string SystemName => "ClientManager";

        public ESystemType SystemType => ESystemType.Thread;

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

        }

        public void StartSystem()
        {
            Start();
        }

        public void StopSystem()
        {
            SystemShutdownEvent.Reset();
            Stop();
        }

        public void UpdateSystemState()
        {
            SystemStatus = isRunning ? ESystemStatus.Running : ESystemStatus.Stopped;

            if (SystemStatus == ESystemStatus.Stopped)
            {
                SystemShutdownEvent.Set();
            }
        }
        #endregion

        private Thread thread;
        private bool isRunning;
        private readonly object lockObject = new();

        public ClientManager()
        {
            thread = new Thread(ClientManagementLoop);
            isRunning = false;
        }

        public ThreadState Start()
        {
            lock (lockObject)
            {
                if (!isRunning)
                {
                    isRunning = true;
                    thread = new Thread(ClientManagementLoop);
                    thread.Start();
                    LogMessage($"ClientManagementLoop Started!");
                }
            }

            UpdateSystemState();

            // ThreadState may not be accurate, consider removing
            return thread.ThreadState;
        }

        public ThreadState Stop()
        {
            lock (lockObject)
            {
                if (isRunning)
                {
                    isRunning = false;
                    // Implement logic to gracefully stop the client manager thread
                    thread.Join(); // Wait for the thread to finish
                    LogMessage($"ClientManagementLoop Stopped!");
                }
            }

            UpdateSystemState();

            return thread.ThreadState;
        }

        private void ClientManagementLoop()
        {
            while (isRunning)
            {
                // Client management logic goes here
                // This loop continues until isRunning is set to false
                Thread.Sleep(1000);
            }
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
    }

}
