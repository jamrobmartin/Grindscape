using GrindscapeServer.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrindscapeServer.Controller
{
    public class ServerMasterController
    {
        #region Singleton
        private static readonly Lazy<ServerMasterController> lazyInstance =
            new Lazy<ServerMasterController>(() => new ServerMasterController());

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

            // Start GameManagerThread
            GameManagerThread.Start();

            // Start ClientManagerThread
            ClientManagerThread.Start();

            
        }

        public void StopServer()
        {
            // Stop the server logic

            // Stop GameManagerThread
            GameManagerThread.Stop();

            // Stop ClientManagerThread
            ClientManagerThread.Stop();
        }

        // Add more methods as needed for server-level operations
        #endregion

        #region GameManagerThread

        private GameManagerThread GameManagerThread = new GameManagerThread();

        #endregion

        #region ClientManagerThread

        private ClientManagerThread ClientManagerThread = new ClientManagerThread();

        #endregion
    }

}
