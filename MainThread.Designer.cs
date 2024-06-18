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
            menu = new Panel();
            XIO_el = new Button();
            XIC_el = new Button();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            connectToolStripMenuItem = new ToolStripMenuItem();
            adresesValuesToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            ModBusUpdate = new System.Windows.Forms.Timer(components);
            FileUpdate = new System.Windows.Forms.Timer(components);
            openFileDialog2 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            AdresUpdate = new System.Windows.Forms.Timer(components);
            MemoryClear = new System.Windows.Forms.Timer(components);
            Files = new MyTabControl();
            menu.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menu
            // 
            menu.BackColor = SystemColors.Menu;
            menu.Controls.Add(XIO_el);
            menu.Controls.Add(XIC_el);
            menu.Controls.Add(menuStrip1);
            menu.Dock = DockStyle.Top;
            menu.ForeColor = SystemColors.Menu;
            menu.Location = new Point(0, 0);
            menu.Name = "menu";
            menu.Size = new Size(919, 41);
            menu.TabIndex = 1;
            // 
            // XIO_el
            // 
            XIO_el.Image = NodEn.XIC;
            XIO_el.Location = new Point(114, 51);
            XIO_el.Name = "XIO_el";
            XIO_el.Size = new Size(70, 46);
            XIO_el.TabIndex = 2;
            XIO_el.UseVisualStyleBackColor = true;
            // 
            // XIC_el
            // 
            XIC_el.Image = NodEn.XIO;
            XIC_el.Location = new Point(54, 51);
            XIC_el.Name = "XIC_el";
            XIC_el.Size = new Size(63, 46);
            XIC_el.TabIndex = 1;
            XIC_el.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, settingsToolStripMenuItem, aboutToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(919, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, newToolStripMenuItem, saveToolStripMenuItem, connectToolStripMenuItem, adresesValuesToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+O";
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openToolStripMenuItem.Size = new Size(242, 26);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            newToolStripMenuItem.Size = new Size(242, 26);
            newToolStripMenuItem.Text = "New";
            newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(242, 26);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // connectToolStripMenuItem
            // 
            connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            connectToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            connectToolStripMenuItem.Size = new Size(242, 26);
            connectToolStripMenuItem.Text = "Connect";
            connectToolStripMenuItem.Click += connectToolStripMenuItem_Click;
            // 
            // adresesValuesToolStripMenuItem
            // 
            adresesValuesToolStripMenuItem.Name = "adresesValuesToolStripMenuItem";
            adresesValuesToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D;
            adresesValuesToolStripMenuItem.Size = new Size(242, 26);
            adresesValuesToolStripMenuItem.Text = "Adreses values";
            adresesValuesToolStripMenuItem.Click += adresesValuesToolStripMenuItem_Click;
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
            // ModBusUpdate
            // 
            ModBusUpdate.Interval = 1000;
            ModBusUpdate.Tick += ModBusUpdate_Tick;
            // 
            // FileUpdate
            // 
            FileUpdate.Interval = 2000;
            FileUpdate.Tick += FileUpdate_Tick;
            // 
            // openFileDialog2
            // 
            openFileDialog2.FileName = "openFileDialog1";
            // 
            // AdresUpdate
            // 
            AdresUpdate.Enabled = true;
            AdresUpdate.Interval = 300;
            AdresUpdate.Tick += AdresUpdate_Tick;
            // 
            // MemoryClear
            // 
            MemoryClear.Enabled = true;
            MemoryClear.Interval = 3000;
            MemoryClear.Tick += MemoryClear_Tick;
            // 
            // Files
            // 
            Files.Dock = DockStyle.Fill;
            Files.Location = new Point(0, 41);
            Files.Name = "Files";
            Files.SelectedIndex = 0;
            Files.Size = new Size(919, 423);
            Files.TabIndex = 2;
            // 
            // MainThread
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(919, 464);
            Controls.Add(Files);
            Controls.Add(menu);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "MainThread";
            Text = "Logix";
            FormClosing += Form1_FormClosing;
            menu.ResumeLayout(false);
            menu.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
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
        private System.Windows.Forms.Timer MemoryClear;
        private Panel panel1;
        private MyTabControl Files;
        private ToolStripMenuItem newToolStripMenuItem;
        private Button XIO_el;
        private Button XIC_el;
        private ToolStripMenuItem adresesValuesToolStripMenuItem;
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