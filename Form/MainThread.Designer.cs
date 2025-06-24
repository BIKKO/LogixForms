namespace LogixForms
{


    partial class MainThread
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainThread));
            ModBusUpdate = new System.Windows.Forms.Timer(components);
            MouseWheelUpdate = new System.Windows.Forms.Timer(components);
            openFileDialog2 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            AdresUpdate = new System.Windows.Forms.Timer(components);
            MemoryClear = new System.Windows.Forms.Timer(components);
            Files = new MyTabControl();
            panel1 = new Panel();
            panel2 = new Panel();
            button_cansel = new Button();
            button_undo = new Button();
            button_upload = new Button();
            button_accept = new Button();
            menu = new Panel();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            connectToolStripMenuItem = new ToolStripMenuItem();
            adresesValuesToolStripMenuItem = new ToolStripMenuItem();
            openConsoleToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            toolStripTextBox1 = new ToolStripTextBox();
            toolStripTextBox2 = new ToolStripTextBox();
            splitContainer1 = new SplitContainer();
            textBox1 = new TextBox();
            panel4 = new Panel();
            Console_textBox = new TextBox();
            label1 = new Label();
            panel3 = new Panel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            menu.SuspendLayout();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // ModBusUpdate
            // 
            ModBusUpdate.Interval = 1000;
            ModBusUpdate.Tick += ModBusUpdate_Tick;
            // 
            // MouseWheelUpdate
            // 
            MouseWheelUpdate.Enabled = true;
            MouseWheelUpdate.Interval = 5;
            MouseWheelUpdate.Tick += MouseWheelUpdate_Tick;
            // 
            // openFileDialog2
            // 
            openFileDialog2.FileName = "openFileDialog1";
            // 
            // AdresUpdate
            // 
            AdresUpdate.Interval = 200;
            AdresUpdate.Tick += AdresUpdate_Tick;
            // 
            // MemoryClear
            // 
            MemoryClear.Enabled = true;
            MemoryClear.Interval = 10000;
            MemoryClear.Tick += MemoryClear_Tick;
            // 
            // Files
            // 
            Files.Dock = DockStyle.Fill;
            Files.Location = new Point(0, 0);
            Files.Name = "Files";
            Files.SelectedIndex = 0;
            Files.Size = new Size(1171, 328);
            Files.TabIndex = 2;
            Files.SelectedIndexChanged += TabPag_SelectedIndex;
            Files.KeyDown += MyTabPage_KeyDown;
            // 
            // panel1
            // 
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1171, 34);
            panel1.TabIndex = 3;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.Menu;
            panel2.Controls.Add(button_cansel);
            panel2.Controls.Add(button_undo);
            panel2.Controls.Add(button_upload);
            panel2.Controls.Add(button_accept);
            panel2.Controls.Add(menu);
            panel2.Dock = DockStyle.Fill;
            panel2.ForeColor = SystemColors.Menu;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1171, 34);
            panel2.TabIndex = 0;
            // 
            // button_cansel
            // 
            button_cansel.BackColor = SystemColors.Menu;
            button_cansel.Enabled = false;
            button_cansel.Font = new Font("Segoe UI Black", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 204);
            button_cansel.ForeColor = SystemColors.AppWorkspace;
            button_cansel.Location = new Point(383, 5);
            button_cansel.Name = "button_cansel";
            button_cansel.Size = new Size(29, 29);
            button_cansel.TabIndex = 6;
            button_cansel.Text = "↩";
            button_cansel.UseVisualStyleBackColor = false;
            button_cansel.EnabledChanged += button_cansel_EnabledChanged;
            button_cansel.Click += button_cansel_Click;
            // 
            // button_undo
            // 
            button_undo.BackColor = SystemColors.Menu;
            button_undo.Enabled = false;
            button_undo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            button_undo.ForeColor = SystemColors.AppWorkspace;
            button_undo.Location = new Point(418, 5);
            button_undo.Name = "button_undo";
            button_undo.Size = new Size(29, 29);
            button_undo.TabIndex = 5;
            button_undo.Text = "↪";
            button_undo.UseVisualStyleBackColor = false;
            button_undo.EnabledChanged += button_undo_EnabledChanged;
            button_undo.Click += button_undo_Click;
            // 
            // button_upload
            // 
            button_upload.BackColor = Color.Silver;
            button_upload.Enabled = false;
            button_upload.ForeColor = Color.Lime;
            button_upload.Image = Properties.Resources.UploadArrou1;
            button_upload.Location = new Point(297, 5);
            button_upload.Name = "button_upload";
            button_upload.Size = new Size(29, 29);
            button_upload.TabIndex = 4;
            button_upload.UseVisualStyleBackColor = false;
            button_upload.Click += button_upload_Click;
            // 
            // button_accept
            // 
            button_accept.BackColor = Color.Silver;
            button_accept.Enabled = false;
            button_accept.ForeColor = Color.Lime;
            button_accept.Image = Properties.Resources.Galochka;
            button_accept.Location = new Point(332, 5);
            button_accept.Name = "button_accept";
            button_accept.Size = new Size(29, 29);
            button_accept.TabIndex = 3;
            button_accept.UseVisualStyleBackColor = false;
            button_accept.Click += button_accept_Click;
            // 
            // menu
            // 
            menu.BackColor = SystemColors.Menu;
            menu.Controls.Add(menuStrip1);
            menu.Dock = DockStyle.Left;
            menu.ForeColor = SystemColors.Menu;
            menu.Location = new Point(0, 0);
            menu.Name = "menu";
            menu.Size = new Size(291, 34);
            menu.TabIndex = 2;
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.Menu;
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, settingsToolStripMenuItem, aboutToolStripMenuItem, toolStripTextBox1, toolStripTextBox2 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(291, 31);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, newToolStripMenuItem, saveToolStripMenuItem, connectToolStripMenuItem, adresesValuesToolStripMenuItem, openConsoleToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 27);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+O";
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openToolStripMenuItem.Size = new Size(242, 26);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += OpenToolStripMenuItem_Click;
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            newToolStripMenuItem.Size = new Size(242, 26);
            newToolStripMenuItem.Text = "New";
            newToolStripMenuItem.Click += NewToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(242, 26);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += SaveToolStripMenuItem_Click;
            // 
            // connectToolStripMenuItem
            // 
            connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            connectToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.C;
            connectToolStripMenuItem.Size = new Size(242, 26);
            connectToolStripMenuItem.Text = "Connect";
            connectToolStripMenuItem.Click += ConnectToolStripMenuItem_Click;
            // 
            // adresesValuesToolStripMenuItem
            // 
            adresesValuesToolStripMenuItem.Name = "adresesValuesToolStripMenuItem";
            adresesValuesToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D;
            adresesValuesToolStripMenuItem.Size = new Size(242, 26);
            adresesValuesToolStripMenuItem.Text = "Adreses values";
            adresesValuesToolStripMenuItem.Click += AdresesValuesToolStripMenuItem_Click;
            // 
            // openConsoleToolStripMenuItem
            // 
            openConsoleToolStripMenuItem.Name = "openConsoleToolStripMenuItem";
            openConsoleToolStripMenuItem.Size = new Size(242, 26);
            openConsoleToolStripMenuItem.Text = "Console";
            openConsoleToolStripMenuItem.Click += openConsoleToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(76, 27);
            settingsToolStripMenuItem.Text = "Settings";
            settingsToolStripMenuItem.Click += SettingsToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(64, 27);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += AboutToolStripMenuItem_Click;
            // 
            // toolStripTextBox1
            // 
            toolStripTextBox1.BackColor = Color.White;
            toolStripTextBox1.Enabled = false;
            toolStripTextBox1.Name = "toolStripTextBox1";
            toolStripTextBox1.ReadOnly = true;
            toolStripTextBox1.Size = new Size(27, 27);
            // 
            // toolStripTextBox2
            // 
            toolStripTextBox2.BackColor = Color.White;
            toolStripTextBox2.Enabled = false;
            toolStripTextBox2.Name = "toolStripTextBox2";
            toolStripTextBox2.ReadOnly = true;
            toolStripTextBox2.Size = new Size(27, 27);
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 34);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(Files);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = SystemColors.ControlLight;
            splitContainer1.Panel2.Controls.Add(textBox1);
            splitContainer1.Panel2.Controls.Add(panel4);
            splitContainer1.Size = new Size(1171, 536);
            splitContainer1.SplitterDistance = 328;
            splitContainer1.TabIndex = 6;
            // 
            // textBox1
            // 
            textBox1.Dock = DockStyle.Fill;
            textBox1.Location = new Point(0, 0);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(1171, 181);
            textBox1.TabIndex = 3;
            // 
            // panel4
            // 
            panel4.BackColor = SystemColors.ControlLightLight;
            panel4.Controls.Add(Console_textBox);
            panel4.Controls.Add(label1);
            panel4.Dock = DockStyle.Bottom;
            panel4.Location = new Point(0, 181);
            panel4.Name = "panel4";
            panel4.Size = new Size(1171, 23);
            panel4.TabIndex = 2;
            // 
            // Console_textBox
            // 
            Console_textBox.BackColor = SystemColors.ControlLightLight;
            Console_textBox.BorderStyle = BorderStyle.None;
            Console_textBox.Dock = DockStyle.Fill;
            Console_textBox.Font = new Font("Segoe UI", 10F);
            Console_textBox.Location = new Point(19, 0);
            Console_textBox.Name = "Console_textBox";
            Console_textBox.Size = new Size(1152, 23);
            Console_textBox.TabIndex = 0;
            Console_textBox.KeyDown += Console_textBox_KeyDown;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Left;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(19, 20);
            label1.TabIndex = 1;
            label1.Text = ">";
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.Menu;
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 570);
            panel3.Name = "panel3";
            panel3.Size = new Size(1171, 26);
            panel3.TabIndex = 7;
            // 
            // MainThread
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(1171, 596);
            Controls.Add(splitContainer1);
            Controls.Add(panel1);
            Controls.Add(panel3);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainThread";
            Text = "Logix";
            FormClosing += Form1_FormClosing;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            menu.ResumeLayout(false);
            menu.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Timer ModBusUpdate;
        private System.Windows.Forms.Timer MouseWheelUpdate;
        private OpenFileDialog openFileDialog2;
        private SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Timer AdresUpdate;
        private System.Windows.Forms.Timer MemoryClear;
        private MyTabControl Files;
        private Panel panel1;
        private Panel panel2;
        private Panel menu;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem connectToolStripMenuItem;
        private ToolStripMenuItem adresesValuesToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripTextBox toolStripTextBox1;
        private ToolStripTextBox toolStripTextBox2;
        private Button button_accept;
        private Button button_upload;
        private Button button_cansel;
        private Button button_undo;
        private SplitContainer splitContainer1;
        private Panel panel3;
        private TextBox Console_textBox;
        private Panel panel4;
        private Label label1;
        private TextBox textBox1;
        private ToolStripMenuItem openConsoleToolStripMenuItem;
    }

}
//изменения для нейтролизации мерцания при перерисовке
namespace System.Windows.Forms
{
    public class MyPanel : Panel
    {
        public MyPanel()
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
        }

    }

    public class MyTabControl : TabControl
    {
        public MyTabControl()
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
        }
    }

    class MyTabPage : TabPage
    {
        public MyTabPage()
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
        }
    }
}