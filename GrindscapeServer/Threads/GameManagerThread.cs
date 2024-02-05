using GrindscapeServer.Systems;


namespace GrindscapeServer.Threads
{
    public class GameManagerThread
    {
        private Thread thread;
        private bool isRunning;
        private readonly object lockObject = new();

        public GameManagerThread()
        {
            thread = new Thread(GameManagementLoop);
            isRunning = false;
        }

        public ThreadState Start()
        {
            lock (lockObject)
            {
                if (!isRunning)
                {
                    isRunning = true;
                    thread = new Thread(GameManagementLoop);
                    thread.Start();
                    LogMessage(Logger.LogLevel.Info, $"GameManagementLoop Started!");
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
                    LogMessage(Logger.LogLevel.Info, $"GameManagementLoop Stopped!");
                }
            }

            return thread.ThreadState;
        }

        private void GameManagementLoop()
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

        private readonly string LoggerSystem = "GameManagerThread";

        private void LogMessage(Logger.LogLevel logLevel, string message)
        {
            Logger.LoggerMessage loggerMessage = new(logLevel, LoggerSystem, message);
            Logger.Instance.LogMessage(loggerMessage);
        }

        #endregion
    }

}
