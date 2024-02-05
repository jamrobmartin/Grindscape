using GrindscapeServer.Controller;

namespace GrindscapeServer.UI
{
    public partial class GrindscapeServerMainForm : Form
    {
        public GrindscapeServerMainForm()
        {
            InitializeComponent();
        }

        private void StartServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServerMasterController.Instance.StartServer();
        }

        private void StopServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServerMasterController.Instance.StopServer();
        }

        private void GrindscapeServerMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ServerMasterController.Instance.StopServer();
        }
    }
}
