namespace GrindscapeServer.UI
{
    partial class GrindscapeServerMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            statusStrip1 = new StatusStrip();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            serverToolStripMenuItem = new ToolStripMenuItem();
            startServerToolStripMenuItem = new ToolStripMenuItem();
            stopServerToolStripMenuItem = new ToolStripMenuItem();
            tableLayoutPanel1 = new TableLayoutPanel();
            loggerConsole1 = new LoggerConsole();
            menuStrip1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Location = new Point(0, 936);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1556, 22);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, serverToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1556, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // serverToolStripMenuItem
            // 
            serverToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { startServerToolStripMenuItem, stopServerToolStripMenuItem });
            serverToolStripMenuItem.Name = "serverToolStripMenuItem";
            serverToolStripMenuItem.Size = new Size(51, 20);
            serverToolStripMenuItem.Text = "Server";
            // 
            // startServerToolStripMenuItem
            // 
            startServerToolStripMenuItem.Name = "startServerToolStripMenuItem";
            startServerToolStripMenuItem.Size = new Size(141, 22);
            startServerToolStripMenuItem.Text = "Start Server()";
            startServerToolStripMenuItem.Click += StartServerToolStripMenuItem_Click;
            // 
            // stopServerToolStripMenuItem
            // 
            stopServerToolStripMenuItem.Name = "stopServerToolStripMenuItem";
            stopServerToolStripMenuItem.Size = new Size(141, 22);
            stopServerToolStripMenuItem.Text = "Stop Server()";
            stopServerToolStripMenuItem.Click += StopServerToolStripMenuItem_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(loggerConsole1, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 24);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(1556, 912);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // loggerConsole1
            // 
            loggerConsole1.Dock = DockStyle.Fill;
            loggerConsole1.Location = new Point(3, 3);
            loggerConsole1.Name = "loggerConsole1";
            loggerConsole1.Size = new Size(772, 906);
            loggerConsole1.TabIndex = 0;
            // 
            // GrindscapeServerMainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1556, 958);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "GrindscapeServerMainForm";
            Text = "GrindscapeServerMainForm";
            WindowState = FormWindowState.Maximized;
            FormClosing += GrindscapeServerMainForm_FormClosing;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStrip1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem serverToolStripMenuItem;
        private ToolStripMenuItem startServerToolStripMenuItem;
        private ToolStripMenuItem stopServerToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel1;
        private LoggerConsole loggerConsole1;
    }
}