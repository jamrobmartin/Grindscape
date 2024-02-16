using GrindscapeServer.Systems;

namespace GrindscapeServer.UI
{
    public partial class LoggerConsole : UserControl
    {
        public LoggerConsole()
        {
            InitializeComponent();
            UpdateButtons();
        }

        private Systems.Logger.LogLevel LogLevel = Systems.Logger.LogLevel.Debug;

        public void SetLogLevel(Systems.Logger.LogLevel LogLevel)
        {
            this.LogLevel = LogLevel;
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            button1.ForeColor = Color.Red;
            button2.ForeColor = Color.Red;
            button3.ForeColor = Color.Red;
            button4.ForeColor = Color.Red;

            button1.Font = new(button1.Font, FontStyle.Regular);
            button2.Font = new(button2.Font, FontStyle.Regular);
            button3.Font = new(button3.Font, FontStyle.Regular);
            button4.Font = new(button4.Font, FontStyle.Regular);

            switch (this.LogLevel)
            {
                case Systems.Logger.LogLevel.Debug:
                    button1.ForeColor = Color.Lime;
                    button1.Font = new(button1.Font, FontStyle.Bold);
                    break;
                case Systems.Logger.LogLevel.Info:
                    button2.ForeColor = Color.Lime;
                    button2.Font = new(button1.Font, FontStyle.Bold);
                    break;
                case Systems.Logger.LogLevel.Warning:
                    button3.ForeColor = Color.Lime;
                    button3.Font = new(button1.Font, FontStyle.Bold);
                    break;
                case Systems.Logger.LogLevel.Error:
                    button4.ForeColor = Color.Lime;
                    button4.Font = new(button1.Font, FontStyle.Bold);
                    break;
                default:
                    break;
            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SetLogLevel(Systems.Logger.LogLevel.Debug);
            UpdateButtons();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            SetLogLevel(Systems.Logger.LogLevel.Info);
            UpdateButtons();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            SetLogLevel(Systems.Logger.LogLevel.Warning);
            UpdateButtons();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            SetLogLevel(Systems.Logger.LogLevel.Error);
            UpdateButtons();
        }

        public void WriteLoggedMessagesToLoggerConsole(object? sender, Logger.LoggerMessageEventArgs e)
        {
            Logger.LoggerMessage message = e.Message;

            // First check if the message is below our logging threshold
            if (message.LogLevel < this.LogLevel)
            {
                // We dont report these, just return;
                return;
            }

            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{message.LogLevel}] [{message.System}] {message.Message}";
            LogLine(logEntry, message.LogLevel);
        }

        private void LogLine(string message, Logger.LogLevel logLevel)
        {
            if (richTextBox1.InvokeRequired)
            {
                void action() { LogLine(message, logLevel); }
                richTextBox1.Invoke(action);
            }
            else
            {
                Color textColor = Color.White;
                switch (logLevel)
                {
                    case Logger.LogLevel.Debug:
                        textColor = Color.Blue;
                        break;
                    case Logger.LogLevel.Info:
                        textColor = Color.Lime;
                        break;
                    case Logger.LogLevel.Warning:
                        textColor = Color.Yellow;
                        break;
                    case Logger.LogLevel.Error:
                        textColor = Color.Red;
                        break;
                    default:
                        break;
                }

                richTextBox1.SelectionStart = richTextBox1.TextLength;
                richTextBox1.SelectionLength = 0;
                richTextBox1.SelectionColor = textColor;
                richTextBox1.AppendText(message + Environment.NewLine);
                richTextBox1.SelectionColor = textColor;
            }
        }
    }
}
