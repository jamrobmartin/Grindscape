using GrindscapeServer.Systems;

namespace GrindscapeServer.UI
{
    public partial class SystemStatusWindow : UserControl
    {
        private readonly Dictionary<ISystem, Tuple<Label, Label, Label>> systemLabels = [];


        public SystemStatusWindow()
        {
            InitializeComponent();

            InitializeTable();
        }

        private void InitializeTable()
        {
            // We want to make the first row be a header row

            // Create a list of labels so that we can apply the same properties to each of them more efficiently
            List<Label> labels = [];

            Label nameLabel = new();
            Label statusLabel = new();
            Label typeLabel = new();

            labels.Add(nameLabel);
            labels.Add(statusLabel);
            labels.Add(typeLabel);

            nameLabel.Text = "System Name:";
            statusLabel.Text = "System Status:";
            typeLabel.Text = "System Type:";

            Font font = new("Consolas", 14, FontStyle.Bold);

            foreach (Label label in labels)
            {
                label.Font = font;
                label.Dock = DockStyle.Fill;
                label.BackColor = Color.Gray; label.ForeColor = Color.White;
                label.Margin = new Padding(0);
            }

            // Get the current number or rows, because we want to insert a new row.
            // Right here the current number of rows should just be one
            var currentNumberOfRows = tableLayoutPanel1.RowCount;

            // Increase the maximun number of rows by 1
            tableLayoutPanel1.RowCount++;

            // Insert a new row just before the last row
            tableLayoutPanel1.RowStyles.Insert(currentNumberOfRows - 1, new RowStyle(SizeType.AutoSize));

            // Add the labels to this new row
            tableLayoutPanel1.Controls.Add(nameLabel, 0, currentNumberOfRows);
            tableLayoutPanel1.Controls.Add(statusLabel, 1, currentNumberOfRows);
            tableLayoutPanel1.Controls.Add(typeLabel, 2, currentNumberOfRows);

        }

        public void AddSystem(ISystem system)
        {
            // Add a new row to hold the systems information

            // Create a list of labels so that we can apply the same properties to each of them more efficiently
            List<Label> labels = [];

            Label nameLabel = new();
            Label statusLabel = new();
            Label typeLabel = new();

            labels.Add(nameLabel);
            labels.Add(statusLabel);
            labels.Add(typeLabel);

            nameLabel.Text = system.SystemName;
            statusLabel.Text = system.SystemStatus.ToString();
            typeLabel.Text = system.SystemType.ToString(); // Get the name of the system's type

            Font font = new("Consolas", 14, FontStyle.Bold);

            foreach (Label label in labels)
            {
                label.Font = font;
                label.Dock = DockStyle.Fill;
                label.BackColor = Color.Black; label.ForeColor = Color.White;
                label.Margin = new Padding(0);
            }

            statusLabel.ForeColor = GetColorBySystemStatus(system.SystemStatus);

            // Get the current number or rows, because we want to insert a new row.
            var currentNumberOfRows = tableLayoutPanel1.RowCount;

            // Increase the maximun number of rows by 1
            tableLayoutPanel1.RowCount++;

            // Insert a new row just before the last row
            tableLayoutPanel1.RowStyles.Insert(currentNumberOfRows - 1, new RowStyle(SizeType.AutoSize));

            // Add the labels to this new row
            tableLayoutPanel1.Controls.Add(nameLabel, 0, currentNumberOfRows);
            tableLayoutPanel1.Controls.Add(statusLabel, 1, currentNumberOfRows);
            tableLayoutPanel1.Controls.Add(typeLabel, 2, currentNumberOfRows);

            // Update dictionary to store labels for the system
            systemLabels.Add(system, new Tuple<Label, Label, Label>(nameLabel, statusLabel, typeLabel));

            // Subscribe to SystemStatusChanged event
            system.SystemStatusChanged += SystemStatusChangedHandler;

        }

        private void SystemStatusChangedHandler(object? sender, SystemStatusChangedEventArgs e)
        {
            if (sender is ISystem system && systemLabels.ContainsKey(system))
            {
                UpdateSystemStatus(system, e.NewStatus);
            }
        }

        private void UpdateSystemStatus(ISystem system, ESystemStatus status)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { UpdateSystemStatus(system, status); }));
            }
            else
            {
                // Update status label
                systemLabels[system].Item2.Text = status.ToString();

                // Update status color
                systemLabels[system].Item2.ForeColor = GetColorBySystemStatus(status);
            }
        }

        private static Color GetColorBySystemStatus(ESystemStatus status)
        {
            Color statusColor = Color.Black;

            switch (status)
            {
                case ESystemStatus.Uninitialized:
                case ESystemStatus.Stopped:
                case ESystemStatus.Terminated:
                    statusColor = Color.Red;
                    break;
                case ESystemStatus.Starting:
                case ESystemStatus.Stopping:
                case ESystemStatus.Terminating:
                    statusColor = Color.Yellow;
                    break;
                case ESystemStatus.Initialized:
                case ESystemStatus.Running:
                    statusColor = Color.Lime;
                    break;
                case ESystemStatus.Paused:
                    statusColor = Color.Orange;
                    break;
                default:
                    break;
            }

            return statusColor;
        }
    }
}
