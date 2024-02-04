using GrindscapeServer.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrindscapeServer.UI
{
    public partial class GrindscapeServerMainForm : Form
    {
        public GrindscapeServerMainForm()
        {
            InitializeComponent();
        }

        private void startServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServerMasterController.Instance.StartServer();
        }

        private void stopServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServerMasterController.Instance.StopServer();
        }

        private void GrindscapeServerMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ServerMasterController.Instance.StopServer();
        }
    }
}
