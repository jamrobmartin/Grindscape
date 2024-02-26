using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GrindscapeServer.Systems
{
    public enum EClientState
    {
        New,
        Connected,
        Authenticated,
        Closing,
        Closed
    }

    public class GSClientConnectionData(Guid id)
    {
        public Guid ID { get; private set; } = id;

        private IPAddress ipAddress = IPAddress.None;
        private DateTime connectionTime = DateTime.MinValue;
        private DateTime lastActivityTime = DateTime.MinValue;
        private EClientState state = EClientState.New;

        public event EventHandler? LastActivityTimeUpdated;
        public event EventHandler? StateUpdated;

        public IPAddress IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        public DateTime ConnectionTime
        {
            get { return connectionTime; }
            set { connectionTime = value; }
        }

        public DateTime LastActivityTime
        {
            get { return lastActivityTime; }
            set
            {
                lastActivityTime = value;
                OnLastActivityTimeUpdated();
            }
        }

        public EClientState State
        {
            get { return state; }
            set
            {
                state = value;
                OnStateUpdated();
            }
        }

        protected virtual void OnLastActivityTimeUpdated()
        {
            LastActivityTimeUpdated?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnStateUpdated()
        {
            StateUpdated?.Invoke(this, EventArgs.Empty);
        }
    }

    public class GSClient
    {
        // Private Fields
        private TcpClient TcpClient { get; set; }
        private NetworkStream Stream { get; set; }
        private const int BufferSize = 1024;
        private byte[] Buffer { get; set; } = new byte[BufferSize];


        // Public fields

        // ID
        public Guid ID { get; private set; } = Guid.Empty;

        // GSClientConnectionData
        public GSClientConnectionData ConnectionData;

        public GSClient(TcpClient tcpClient)
        {
            // Private fields
            TcpClient = tcpClient;
            Stream = tcpClient.GetStream();

            // Public Fields

            // ID
            ID = Guid.NewGuid();


            // ConnectionData

            // Obtain the IP address
            IPAddress remoteIpAddress = IPAddress.None;
            if (tcpClient.Client.RemoteEndPoint != null)
            {
                IPEndPoint remoteEndPoint = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
                remoteIpAddress = remoteEndPoint.Address;
            }

            ConnectionData = new GSClientConnectionData(ID)
            {
                IPAddress = remoteIpAddress,
                ConnectionTime = DateTime.UtcNow,
                LastActivityTime = DateTime.UtcNow,
                State = EClientState.Connected
            };


            // Client connection established

            LogMessage($"New client connection established!");


            // Begin async reading
            Stream.BeginRead(Buffer, 0, BufferSize, ReadCallback, null);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                int bytesRead = Stream.EndRead(ar);
                if (bytesRead > 0)
                {
                    string receivedData = Encoding.ASCII.GetString(Buffer, 0, bytesRead);
                    LogMessage($"Received: {receivedData}");

                    // Echo the data back to the client
                    byte[] response = Encoding.ASCII.GetBytes(receivedData);
                    Stream.BeginWrite(response, 0, response.Length, SendCallback, null);

                    // Continue receiving data from the client
                    Stream.BeginRead(Buffer, 0, BufferSize, ReadCallback, null);
                }
                else
                {
                    LogMessage($"Client connection closed.");
                    ConnectionData.State = EClientState.Closing;
                    // No data received, close the connection
                    TcpClient.Close();

                }
            }
            catch (Exception ex)
            {

                LogMessage($"{ex.GetType()}: {ex.Message}", Logger.LogLevel.Error);
                LogMessage($"Client connection terminated.");
                ConnectionData.State = EClientState.Closing;
                TcpClient.Close();
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Stream.EndWrite(ar);
            }
            catch (Exception ex)
            {
                LogMessage($"Error: {ex.Message}");
            }
        }

        #region Logging

        // Logger Wrapper

        // Logs a message. If the level is not set, it is assumed to be info
        private void LogMessage(string message, Logger.LogLevel logLevel = Logger.LogLevel.Info)
        {
            Logger.LoggerMessage loggerMessage = new(logLevel, ID.ToString(), message);
            Logger.Instance.LogMessage(loggerMessage);
        }

        #endregion
    }
}
