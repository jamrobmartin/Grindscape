using GrindscapeServer.Systems;

namespace GrindscapeServer.UI
{
    public class ClientStatusWindowElement : UserControl
    {
        private readonly Dictionary<string, Label> labelDictionary = [];

        private static readonly int ID_Width = 400;
        private static readonly int IPAddress_Width = 200;
        private static readonly int ConnectionTime_Width = 200;
        private static readonly int LastActivityTime_Width = 200;
        private static readonly int State_Width = 200;

        private readonly int[] columnWidths = [
            ID_Width,
            IPAddress_Width,
            ConnectionTime_Width,
            LastActivityTime_Width,
            State_Width
                    ];

        private static readonly string ID_String = "ID";
        private static readonly string IPAddress_String = "IPAddress";
        private static readonly string ConnectionTime_String = "ConnectionTime";
        private static readonly string LastActivityTime_String = "LastActivityTime";
        private static readonly string State_String = "State";

        private readonly string[] columnStrings = [
            ID_String,
            IPAddress_String,
            ConnectionTime_String,
            LastActivityTime_String,
            State_String
                    ];

        public string ID
        {
            get
            {
                return labelDictionary[ID_String].Text;
            }

            set
            {
                InvokeUpdateLabel(labelDictionary[ID_String], value);
            }
        }

        public string IPAddress
        {
            get
            {
                return labelDictionary[IPAddress_String].Text;
            }

            set
            {
                InvokeUpdateLabel(labelDictionary[IPAddress_String], value);
            }
        }

        public string ConnectionTime
        {
            get
            {
                return labelDictionary[ConnectionTime_String].Text;
            }

            set
            {
                InvokeUpdateLabel(labelDictionary[ConnectionTime_String], value);
            }
        }

        public string LastActivityTime
        {
            get
            {
                return labelDictionary[LastActivityTime_String].Text;
            }

            set
            {
                InvokeUpdateLabel(labelDictionary[LastActivityTime_String], value);
            }
        }

        public string State
        {
            get
            {
                return labelDictionary[State_String].Text;
            }

            set
            {
                InvokeUpdateLabel(labelDictionary[State_String], value);
            }
        }

        public ClientStatusWindowElement(bool isHeader = false)
        {
            Panel panel = GenerateNewPanel(isHeader);
            this.Controls.Add(panel);
            this.AutoSize = true;
            this.Margin = new Padding(0);
            this.Padding = new Padding(0);
            this.BackColor = Color.White;

            if (isHeader)
            {
                SetHeaderLabels();
            }
        }

        private TableLayoutPanel GenerateNewTable(bool isHeader = false)
        {
            float fontSize = 14;
            Font font = new("Consolas", fontSize, FontStyle.Bold);

            int fontToPointHeight = (int)Math.Ceiling(fontSize * 4 / 3);
            int height = fontToPointHeight + 2; // Add 2 to account for cell border style


            TableLayoutPanel table = new()
            {
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            // Reset Row and Column Styles
            table.RowStyles.Clear();
            table.ColumnStyles.Clear();

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, fontToPointHeight));



            int totalWidth = 1; // Start with 1 to account for cell border style

            for (int i = 0; i < columnWidths.Length; i++)
            {
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, columnWidths[i]));
                Label label = new()
                {
                    Text = "NEW",
                    Dock = DockStyle.Fill,
                    Font = font,
                    ForeColor = Color.White,
                    BackColor = isHeader ? Color.Gray : Color.Black,
                    TextAlign = ContentAlignment.BottomCenter,
                    Margin = new Padding(0, 0, 0, 0)
                };

                table.Controls.Add(label, i, 0);


                totalWidth += columnWidths[i];
                totalWidth += 1; // Add one for cell border style

                labelDictionary.Add(columnStrings[i], label);
            }






            table.Size = new Size(totalWidth, height);



            return table;
        }

        private Panel GenerateNewPanel(bool isHeader = false)
        {
            Panel panel = new()
            {
                BackColor = Color.White,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            TableLayoutPanel table = GenerateNewTable(isHeader);
            //table.Dock = DockStyle.Fill;

            panel.Controls.Add(table);            

            return panel;
        }

        private void SetHeaderLabels()
        {
            for (int i = 0; i < columnStrings.Length; i++)
            {
                labelDictionary[columnStrings[i]].Text = columnStrings[i];
            }
        }

        private static void InvokeUpdateLabel(Label label, string text)
        {
            if (label.InvokeRequired)
            {
                label.Invoke(new Action(() => { InvokeUpdateLabel(label, text); }));
            }
            else
            {
                label.Text = text;
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // ClientStatusWindowElement
            // 
            Name = "ClientStatusWindowElement";
            Size = new Size(1547, 933);
            ResumeLayout(false);
        }
    }

    public partial class ClientStatusWindow : UserControl
    {
        private readonly Dictionary<Guid, ClientStatusWindowElement> ClientStatusWindowElementsDictionary = [];

        public ClientStatusWindow()
        {
            InitializeComponent();

            InvokeAddHeader();
        }

        private void InvokeAddHeader()
        {
            if (flowLayoutPanel1.InvokeRequired)
            {
                flowLayoutPanel1.Invoke(new Action(() => { InvokeAddHeader(); }));
            }
            else
            {
                AddHeader();
            }
        }

        private void AddHeader()
        {
            ClientStatusWindowElement headerElement = new(true);

            flowLayoutPanel1.Controls.Add(headerElement);
        }

        private void InvokeAddClient(GSClient client)
        {
            if (flowLayoutPanel1.InvokeRequired)
            {
                flowLayoutPanel1.Invoke(new Action(() => { InvokeAddClient(client); }));
            }
            else
            {
                AddClient(client);
            }
        }

        private void AddClient(GSClient client)
        {
            ClientStatusWindowElement newElement = new(false)
            {
                ID = client.ID.ToString(),
                IPAddress = client.ConnectionData.IPAddress.ToString(),
                ConnectionTime = client.ConnectionData.ConnectionTime.ToString("yy/MM/dd HH:mm:ss"),
                LastActivityTime = client.ConnectionData.LastActivityTime.ToString("yy/MM/dd HH:mm:ss"),
                State = client.ConnectionData.State.ToString()
            };

            flowLayoutPanel1.Controls.Add(newElement);

            ClientStatusWindowElementsDictionary.Add(client.ID, newElement);
        }

        private void InvokeRemoveClient(Guid ID)
        {
            if (flowLayoutPanel1.InvokeRequired)
            {
                flowLayoutPanel1.Invoke(new Action(() => { InvokeRemoveClient(ID); }));
            }
            else
            {
                flowLayoutPanel1.Controls.Remove(ClientStatusWindowElementsDictionary[ID]);
                ClientStatusWindowElementsDictionary.Remove(ID);
            }
        }

        public void ClientAddedEventHandler(object? _, GSClient client)
        {
            InvokeAddClient(client);

            client.ConnectionData.StateUpdated += ConnectionData_StateUpdated;
            client.ConnectionData.LastActivityTimeUpdated += ConnectionData_LastActivityTimeUpdated;
        }

        private void ConnectionData_LastActivityTimeUpdated(object? sender, EventArgs e)
        {
            if (sender != null)
            {
                GSClientConnectionData data = (GSClientConnectionData)sender;

                if (ClientStatusWindowElementsDictionary.TryGetValue(data.ID, out ClientStatusWindowElement? value))
                {
                    value.LastActivityTime = data.LastActivityTime.ToString("yy/MM/dd HH:mm:ss");
                }
            }
        }

        private void ConnectionData_StateUpdated(object? sender, EventArgs e)
        {
            if (sender != null)
            {
                GSClientConnectionData data = (GSClientConnectionData)sender;

                if (ClientStatusWindowElementsDictionary.TryGetValue(data.ID, out ClientStatusWindowElement? value))
                {
                    value.State = data.State.ToString();
                }

                // If the State was changed to Closed, kick off a backgroudn worker to remove it
                if (data.State == EClientState.Closed)
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
            await Task.Delay(5000);
            InvokeRemoveClient(ID);
        }
    }
}
