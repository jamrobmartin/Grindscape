namespace GrindscapeServer.UI
{
    partial class LoggerConsole
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            richTextBox1 = new RichTextBox();
            groupBox1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel1);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1107, 1005);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Logger Console";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(richTextBox1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 19);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1101, 983);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 4;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.Controls.Add(button1, 0, 0);
            tableLayoutPanel2.Controls.Add(button2, 1, 0);
            tableLayoutPanel2.Controls.Add(button3, 2, 0);
            tableLayoutPanel2.Controls.Add(button4, 3, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(1095, 44);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // button1
            // 
            button1.BackColor = Color.Black;
            button1.Dock = DockStyle.Fill;
            button1.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            button1.ForeColor = Color.Lime;
            button1.Location = new Point(3, 3);
            button1.Name = "button1";
            button1.Size = new Size(267, 38);
            button1.TabIndex = 0;
            button1.Text = "Debug";
            button1.UseVisualStyleBackColor = false;
            button1.Click += Button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.Black;
            button2.Dock = DockStyle.Fill;
            button2.Font = new Font("Segoe UI", 14F);
            button2.ForeColor = Color.Red;
            button2.Location = new Point(276, 3);
            button2.Name = "button2";
            button2.Size = new Size(267, 38);
            button2.TabIndex = 1;
            button2.Text = "Info";
            button2.UseVisualStyleBackColor = false;
            button2.Click += Button2_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.Black;
            button3.Dock = DockStyle.Fill;
            button3.Font = new Font("Segoe UI", 14F);
            button3.ForeColor = Color.Red;
            button3.Location = new Point(549, 3);
            button3.Name = "button3";
            button3.Size = new Size(267, 38);
            button3.TabIndex = 2;
            button3.Text = "Warning";
            button3.UseVisualStyleBackColor = false;
            button3.Click += Button3_Click;
            // 
            // button4
            // 
            button4.BackColor = Color.Black;
            button4.Dock = DockStyle.Fill;
            button4.Font = new Font("Segoe UI", 14F);
            button4.ForeColor = Color.Red;
            button4.Location = new Point(822, 3);
            button4.Name = "button4";
            button4.Size = new Size(270, 38);
            button4.TabIndex = 3;
            button4.Text = "Error";
            button4.UseVisualStyleBackColor = false;
            button4.Click += Button4_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = Color.Black;
            richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.Font = new Font("Consolas", 14F, FontStyle.Bold);
            richTextBox1.ForeColor = SystemColors.Info;
            richTextBox1.Location = new Point(3, 53);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(1095, 927);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            // 
            // LoggerConsole
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox1);
            Name = "LoggerConsole";
            Size = new Size(1107, 1005);
            groupBox1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private RichTextBox richTextBox1;
    }
}
