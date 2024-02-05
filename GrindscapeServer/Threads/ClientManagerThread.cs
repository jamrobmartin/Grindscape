using GrindscapeServer.Systems;

namespace GrindscapeServer.Threads
{
    public class ClientManagerThread
    {
        private Thread thread;
        private bool isRunning;
        private readonly object lockObject = new();

        public ClientManagerThread()
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
                    LogMessage(Logger.LogLevel.Info, $"ClientManagementLoop Started!");
                }
            }

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
                    LogMessage(Logger.LogLevel.Info, $"ClientManagementLoop Stopped!");
                }
            }

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

        private readonly string LoggerSystem = "ClientManagerThread";

        private void LogMessage(Logger.LogLevel logLevel, string message)
        {
            Logger.LoggerMessage loggerMessage = new(logLevel, LoggerSystem, message);
            Logger.Instance.LogMessage(loggerMessage);
        }

        #endregion
    }

}
