using GrindscapeServer.Controller;

namespace GrindscapeServer.UI
{
    public partial class GrindscapeServerMainForm : Form
    {
        public GrindscapeServerMainForm()
        {
            InitializeComponent();

            ServerMasterController.Instance.Initialize();

            ServerMasterController.Instance.SetSystemStatusWindow(this.systemStatusWindow1);

            ServerMasterController.Instance.SetClientStatusWindow(this.clientStatusWindow1);

            Systems.Logger.Instance.RegisterMessageLoggedEventHandler(loggerConsole1.WriteLoggedMessagesToLoggerConsole);
            Systems.Logger.Instance.RegisterMessageLoggedEventHandler(clientConsole1.WriteLoggedMessagesToLoggerConsole);

        }

        private void StartServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServerMasterController.Instance.StartServer();
        }

        private void StopServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServerMasterController.Instance.StopServer();
        }

        private async void GrindscapeServerMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // We are closing so we no longer need to write to the LoggerConsole...
            Systems.Logger.Instance.UnRegisterMessageLoggedEventHandler(loggerConsole1.WriteLoggedMessagesToLoggerConsole);

            // Make the call to stop the server.
            ServerMasterController.Instance.StopServer();

            // Start a task to wait until all Systems are shutdown.
            await Task.Run(() => { ServerMasterController.Instance.WaitUntilAllSystemShutdown(); });


        }
    }
}
