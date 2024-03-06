using System.Net;
using System.Net.Sockets;

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

        public void ShutdownSystem()
        {

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

        private readonly List<GSClient> ClientList = [];
        public event EventHandler<GSClient>? ClientAdded;
        public void OnClientAdded(GSClient Client)
        {
            ClientAdded?.Invoke(this, Client);
        }

        private readonly int DefaultPort = 8523;
        private TcpListener Listener = new(IPAddress.Any, 9999);

        public ClientManager()
        {
            thread = new Thread(ClientManagementLoop);
            isRunning = false;
        }

        public void Start()
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

        }

        public void Stop()
        {
            lock (lockObject)
            {
                if (isRunning)
                {
                    isRunning = false;
                    // Implement logic to gracefully stop the client manager thread
                    thread.Join(); // Wait for the thread to finish

                    ClientList.Clear();

                    LogMessage($"ClientManagementLoop Stopped!");
                }
            }

            UpdateSystemState();

        }

        private void ClientManagementLoop()
        {
            ClientList.Clear();

            TcpListener tcpListener = new(IPAddress.Any, DefaultPort);
            Listener = tcpListener;

            Listener.Start();
            LogMessage($"Started Listening on DefaultPort {DefaultPort}.");

            while (isRunning)
            {
                // Client management logic goes here
                try
                {
                    // Set a timeout for accepting connections
                    int AcceptClientTimeoutSeconds = 1;

                    // Begin Accepting TCP Client asynchronously and capture the IAsyncResult
                    var beginAcceptAsyncResult = Listener.BeginAcceptTcpClient(null, null);

                    // Wait until either a client is accepted, or the timeout is reached
                    // true if a signal was received
                    // false if the timeout was hit
                    bool BeginAcceptSignalReceived = beginAcceptAsyncResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(AcceptClientTimeoutSeconds));

                    if (BeginAcceptSignalReceived)
                    {
                        ProcessAcceptAsyncResult(beginAcceptAsyncResult);
                    }
                    else
                    {
                        HandleAcceptTimeout();
                    }

                }
                catch (Exception ex)
                {
                    LogMessage($"Error: {ex.Message}", Logger.LogLevel.Error);
                }

            }

            Listener.Stop();
        }

        private void ProcessAcceptAsyncResult(IAsyncResult beginAcceptAsyncResult)
        {

            // If a client was accepted, then finish the accept and process it
            TcpClient client = Listener.EndAcceptTcpClient(beginAcceptAsyncResult);

            IPAddress address = IPAddress.None;
            if (client.Client.RemoteEndPoint != null)
            {
                IPEndPoint remoteEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
                address = remoteEndPoint.Address;
            }
            LogMessage($"Accepted Client Connection from {{{address}}}.");

            // Process client connection
            ProcessNewClient(client);
        }

        private void ProcessNewClient(TcpClient client)
        {
            // Create a new GSClient object.
            GSClient newClient = new(client);

            // Add the client to the ClientList
            ClientList.Add(newClient);

            // The Client Manager needs to know if the state has been updated,
            // so register an event handler.
            newClient.ConnectionData.StateUpdated += ConnectionData_StateUpdated;

            // Fire the Client Added event
            OnClientAdded(newClient);
        }

        private void ConnectionData_StateUpdated(object? sender, EventArgs e)
        {
            if (sender != null)
            {
                GSClientConnectionData data = (GSClientConnectionData)sender;

                // If the State was changed to Closing, kick off a backgroudn worker to remove it
                if (data.State == EClientState.Closing)
                {
                    Task.Run(async () =>
                    {
                        await ProcessClientClosing(data.ID);
                    });
                }
            }
        }

        private async Task ProcessClientClosing(Guid ID)
        {
            // This method gets called when a client's state has been changed to closing
            // When this happens. We want to allow time for a graceful close, so we delay 
            // by 5 seconds, and then update the client state to closed.
            await Task.Delay(5000);
            GSClient? client = ClientList.Find(x => x.ID == ID);
            if (client != null)
            {
                client.ConnectionData.State = EClientState.Closed;
            }
        }

        private void HandleAcceptTimeout()
        {
            // This method is called when Listener.BeginAcceptTcpClient() times out.
            // This means that the Listener didn't receive any new connection attempts
            // during the timeout window.
            // We use that as an indicator that we have some time to perform
            // some clean up activities while waiting for new client connections

            // Clean up the ClientList
            PruneClientList();

        }

        private void PruneClientList()
        {
            // This method removes any closed clients
            int numberOfRemovedClients = ClientList.RemoveAll(x => x.ConnectionData.State == EClientState.Closed);

            if (numberOfRemovedClients > 0)
            {
                LogMessage($"PruneClientList removed {numberOfRemovedClients} clients.");
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
