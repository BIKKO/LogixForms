namespace LogixForms
{


    partial class Form1
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
            Update = new System.Windows.Forms.Timer(components);
            menu = new Panel();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            connectToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            midpanel = new MyPanel();
            Files = new MyTabControl();
            ModBusUpdate = new System.Windows.Forms.Timer(components);
            FileUpdate = new System.Windows.Forms.Timer(components);
            openFileDialog2 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            AdresUpdate = new System.Windows.Forms.Timer(components);
            menu.SuspendLayout();
            menuStrip1.SuspendLayout();
            midpanel.SuspendLayout();
            SuspendLayout();
            // 
            // Update
            // 
            Update.Enabled = true;
            Update.Interval = 25;
            Update.Tick += Update_Tick;
            // 
            // menu
            // 
            menu.BackColor = SystemColors.Menu;
            menu.Controls.Add(menuStrip1);
            menu.Dock = DockStyle.Top;
            menu.ForeColor = SystemColors.Menu;
            menu.Location = new Point(0, 0);
            menu.Name = "menu";
            menu.Size = new Size(959, 27);
            menu.TabIndex = 1;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, settingsToolStripMenuItem, aboutToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(959, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem, connectToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(146, 26);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(146, 26);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // connectToolStripMenuItem
            // 
            connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            connectToolStripMenuItem.Size = new Size(146, 26);
            connectToolStripMenuItem.Text = "Connect";
            connectToolStripMenuItem.Click += connectToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(76, 24);
            settingsToolStripMenuItem.Text = "Settings";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(64, 24);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // midpanel
            // 
            midpanel.Controls.Add(Files);
            midpanel.Dock = DockStyle.Fill;
            midpanel.Location = new Point(0, 27);
            midpanel.Name = "midpanel";
            midpanel.Size = new Size(959, 455);
            midpanel.TabIndex = 2;
            // 
            // Files
            // 
            Files.Dock = DockStyle.Fill;
            Files.Location = new Point(0, 0);
            Files.Name = "Files";
            Files.SelectedIndex = 0;
            Files.Size = new Size(959, 455);
            Files.TabIndex = 0;
            // 
            // ModBusUpdate
            // 
            ModBusUpdate.Interval = 1000;
            ModBusUpdate.Tick += ModBusUpdate_Tick;
            // 
            // FileUpdate
            // 
            FileUpdate.Interval = 1000;
            FileUpdate.Tick += FileUpdate_Tick;
            // 
            // openFileDialog2
            // 
            openFileDialog2.FileName = "openFileDialog1";
            // 
            // AdresUpdate
            // 
            AdresUpdate.Enabled = true;
            AdresUpdate.Interval = 500;
            AdresUpdate.Tick += AdresUpdate_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(959, 482);
            Controls.Add(midpanel);
            Controls.Add(menu);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Logix";
            menu.ResumeLayout(false);
            menu.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            midpanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Timer Update;
        private MyPanel midpanel;
        private Panel menu;
        private System.Windows.Forms.Timer ModBusUpdate;
        private System.Windows.Forms.Timer FileUpdate;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem connectToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private OpenFileDialog openFileDialog2;
        private SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Timer AdresUpdate;
        private MyTabControl Files;
        private MyTabPage tabPage1;
        private Panel panel1;
    }

}
//изменения для нейтролизации мерцания при перерисовке
namespace System.Windows.Forms
{
    class MyPanel : Panel
    {
        public MyPanel()
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
        }

    }

    class MyTabControl : TabControl
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