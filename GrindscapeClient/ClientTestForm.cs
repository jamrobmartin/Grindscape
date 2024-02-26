using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace GrindscapeClient
{
    public partial class ClientTestForm : Form
    {
        public ClientTestForm()
        {
            InitializeComponent();
        }

        private void LogInToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string serverAddress = "127.0.0.1";
            int port = 8523;

            try
            {
                TcpClient client = new(serverAddress, port);
                NetworkStream stream = client.GetStream();

                string message = $"Ping";
                byte[] data = Encoding.ASCII.GetBytes(message);

                stream.Write(data, 0, data.Length);
                Debug.WriteLine($"Sent: {message}");

                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }



        }

        public bool Connected { get; private set; } = false;
        private TcpClient? Client;
        public NetworkStream? Stream;

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!Connected)
            {
                string serverAddress = "127.0.0.1";
                int port = 8523;
                Client = new TcpClient(serverAddress, port);
                Stream = Client.GetStream();

                Connected = true;
                label2.Text = "Connected";
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                Stream?.Close();
                Client?.Close();

                Connected = false;
                label2.Text = "Not Connected";
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void SendMessage()
        {
            if (Connected)
            {
                string message = textBox2.Text;
                textBox2.Clear();
                byte[] data = Encoding.ASCII.GetBytes(message);

                textBox1.AppendText("[TX] " + message + Environment.NewLine);
                Stream?.Write(data, 0, data.Length);

                Stream?.Read(data, 0, data.Length);

                string echo = Encoding.ASCII.GetString(data);

                textBox1.AppendText("[RX] " + echo + Environment.NewLine);
            }
            else
            {
                MessageBox.Show("The TcpClient is not currently connected.", "Error", MessageBoxButtons.OK);
            }
        }

        private void ClientTestForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ((char)Keys.Enter))
            {
                e.Handled = true;
                SendMessage();
            }
        }

        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ((char)Keys.Enter))
            {
                e.Handled = true;
                SendMessage();
            }
        }
    }
}
