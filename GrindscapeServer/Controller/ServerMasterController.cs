using GrindscapeServer.Systems;
using GrindscapeServer.Threads;
using System.Diagnostics;

namespace GrindscapeServer.Controller
{
    public class ServerMasterController
    {
        #region Singleton
        private static readonly Lazy<ServerMasterController> lazyInstance =
            new(() => new ServerMasterController());

        // Private constructor to prevent external instantiation
        private ServerMasterController()
        {
            // Initialize the controller
        }

        // Public static property to get the instance
        public static ServerMasterController Instance { get; private set; } = lazyInstance.Value;

        #endregion

        #region Public Methods
        // Public methods to handle server-level operations
        public void StartServer()
        {
            // Start the server logic

            try
            {
                // Systems

                // Logger
                bool LoggerInitialized = Logger.Instance.Initialize();
                if (!LoggerInitialized)
                {
                    throw new Exception("Logger Failed to Initialize");
                }


                // Threads

                // Start GameManagerThread
                GameManagerThread.Start();

                // Start ClientManagerThread
                ClientManagerThread.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }




        }

        public void StopServer()
        {
            // Stop the server logic

            try
            {
                // Threads

                // Stop GameManagerThread
                GameManagerThread.Stop();

                // Stop ClientManagerThread
                ClientManagerThread.Stop();

                // Systems

                // Logger
                Logger.Instance.Shutdown();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        // Add more methods as needed for server-level operations
        #endregion

        #region GameManagerThread

        private readonly GameManagerThread GameManagerThread = new();

        #endregion

        #region ClientManagerThread

        private readonly ClientManagerThread ClientManagerThread = new();

        #endregion

        #region Debugging

        // This event handler isnt used currently because we have instantiated a LoggerConsole user control in the main form to see LoggerMessages
        private void WriteLoggedMessagesToDebug(object? sender, Logger.LoggerMessageEventArgs e)
        {
            Logger.LoggerMessage message = e.Message;
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{message.LogLevel}] [{message.System}] {message.Message}";
            Debug.WriteLine(logEntry);
        }

        #endregion

    }

}
