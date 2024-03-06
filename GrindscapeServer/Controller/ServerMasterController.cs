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

        #endregion

        #region Public Fields
        public bool Initialized { get; private set; } = false;
        #endregion

        #region Public Methods
        // Public methods to handle server-level operations

        // Initialize the ServerMasterController
        // Called when the application starts
        // This should only get called once per application run
        public void Initialize()
        {
            if (!Initialized)
            {
                // First add all of the systems. 
                AddSystems();

                // Start each system in the order it was added to the MasterSystemList
                foreach (var system in MasterSystemList)
                {
                    system.InitializeSystem();
                    // Display results of start call...
                }

                Initialized = true;
            }
        }

        private void AddSystems()
        {
            // Add all of the Systems

            // Each System needs to implement ISystem interface
            // The order you add the systems is the order in which they will be started.
            // Make sure to add systems that depend on other sytems AFTER the system they depend on

            // Always add the logger first
            AddSystem(Logger.Instance);


            // Most other Systems depend on the Database, so add it second. 
            AddSystem(DatabaseManager.Instance);

            // The ClientManager relies on data from the GameManager
            // You need to add the GameManager before the ClientManager
            AddSystem(GameManager);
            AddSystem(ClientManager);
        }

        // Start all Systems 
        // Called when the Server starts
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
        // Called when the Server stops
        public void StopServer()
        {
            // Stop each system in the reverse order it was added to the MasterSystemList
            foreach (var system in MasterSystemList.Reverse<ISystem>())
            {
                system.StopSystem();
                // Display results of stop call...
            }
        }

        // Shutdown all Systems
        // Called when the application stops
        public void Shutdown()
        {
            StopServer();

            // Shutdown each system in the reverse order it was added to the MasterSystemList
            foreach (var system in MasterSystemList.Reverse<ISystem>())
            {
                system.ShutdownSystem();
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
        public HashSet<string> MasterSystemListNames { get; private set; } = [];

        private void AddSystem(ISystem system)
        {
            // Add the System to the MasterSystemList
            MasterSystemList.Add(system);

            // Add the System Name to the HashSet
            MasterSystemListNames.Add(system.SystemName);
        }

        public void WaitUntilAllSystemShutdown()
        {
            foreach (var system in MasterSystemList.Reverse<ISystem>())
            {
                system.SystemShutdownEvent.WaitOne();
            }
        }

        #endregion

        #region ClientManager
        private readonly ClientManager ClientManager = new();
        private ClientStatusWindow? ClientStatusWindow { get; set; }

        public void SetClientStatusWindow(ClientStatusWindow window)
        {
            ClientStatusWindow = window;

            ClientManager.ClientAdded += ClientStatusWindow.ClientAddedEventHandler;

        }
        #endregion

    }

}
