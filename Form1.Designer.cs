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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.UpdateImage = new System.Windows.Forms.Timer(this.components);
            this.menu = new System.Windows.Forms.Panel();
            this.midpanel = new System.Windows.Forms.MyPanel();
            this.SuspendLayout();
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar1.Location = new System.Drawing.Point(1166, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(25, 537);
            this.vScrollBar1.TabIndex = 0;
            // 
            // UpdateImage
            // 
            this.UpdateImage.Enabled = true;
            this.UpdateImage.Interval = 25;
            this.UpdateImage.Tick += new System.EventHandler(this.UpdateImage_Tick);
            // 
            // menu
            // 
            this.menu.BackColor = System.Drawing.SystemColors.Menu;
            this.menu.Dock = System.Windows.Forms.DockStyle.Top;
            this.menu.ForeColor = System.Drawing.SystemColors.Menu;
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(1166, 27);
            this.menu.TabIndex = 1;
            // 
            // midpanel
            // 
            this.midpanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.midpanel.Location = new System.Drawing.Point(0, 27);
            this.midpanel.Name = "midpanel";
            this.midpanel.Size = new System.Drawing.Size(1166, 510);
            this.midpanel.TabIndex = 2;
            this.midpanel.Paint += new System.Windows.Forms.PaintEventHandler(this.midpanel_Paint);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1191, 537);
            this.Controls.Add(this.midpanel);
            this.Controls.Add(this.menu);
            this.Controls.Add(this.vScrollBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Logix";
            this.ResumeLayout(false);

        }

        #endregion

        private VScrollBar vScrollBar1;
        private System.Windows.Forms.Timer UpdateImage;
        private MyPanel midpanel;
        private Panel menu;
    }

}

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
}