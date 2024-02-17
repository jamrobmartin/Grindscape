using GrindscapeServer.Systems;
using GrindscapeServer.UI;

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

        }

        // Public static property to get the instance
        public static ServerMasterController Instance { get; private set; } = lazyInstance.Value;

        #endregion

        #region Private Fields

        // System Threads
        private readonly GameManager GameManager = new();
        private readonly ClientManager ClientManager = new();

        #endregion

        #region Public Fields
        public bool Initialized { get; private set; } = false;
        #endregion

        #region Public Methods
        // Public methods to handle server-level operations

        // Initialize the ServerMasterController
        public void Initialize()
        {
            if (!Initialized)
            {
                // Add all of the Systems

                // Each System needs to implement ISystem interface
                // The order you add the systems is the order in which they will be started.
                // Make sure to add systems that depend on other sytems AFTER the system they depend on

                // Always add the logger first
                AddSystem(Logger.Instance);

                // The ClientManager relies on data from the GameManager
                // You need to add the GameManager before the ClientManager
                AddSystem(GameManager);
                AddSystem(ClientManager);

                Initialized = true;
            }
        }

        // Start all Systems 
        public void StartServer()
        {
            // Start each system in the order it was added to the MasterSystemList
            foreach (var system in MasterSystemList)
            {
                system.StartSystem();
                // Display results of start call...
            }
        }

        // Stop all Systems 
        public void StopServer()
        {
            // Stop each system in the reverse order it was added to the MasterSystemList
            foreach (var system in MasterSystemList.Reverse<ISystem>())
            {
                system.StopSystem();
                // Display results of stop call...
            }
        }

        // Add more methods as needed for server-level operations
        #endregion

        #region SystemStatus

        private SystemStatusWindow? SystemStatusWindow { get; set; }

        public void SetSystemStatusWindow(SystemStatusWindow window)
        {
            SystemStatusWindow = window;

            // For all of the systems
            foreach (var system in MasterSystemList)
            {
                // Add the System to the SystemStatusWindow
                SystemStatusWindow.AddSystem(system);
            }
        }

        private List<ISystem> MasterSystemList { get; set; } = [];

        private void AddSystem(ISystem system)
        {
            // Add the System to the MasterSystemList
            MasterSystemList.Add(system);

        }

        public void WaitUntilAllSystemShutdown()
        {
            foreach (var system in MasterSystemList.Reverse<ISystem>())
            {
                system.SystemShutdownEvent.WaitOne();
            }
        }

        #endregion

    }

}
