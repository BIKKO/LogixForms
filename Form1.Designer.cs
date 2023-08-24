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
            vScrollBar1 = new VScrollBar();
            UpdateImage = new System.Windows.Forms.Timer(components);
            menu = new Panel();
            midpanel = new MyPanel();
            SuspendLayout();
            // 
            // vScrollBar1
            // 
            vScrollBar1.Dock = DockStyle.Right;
            vScrollBar1.LargeChange = 1;
            vScrollBar1.Location = new Point(1166, 0);
            vScrollBar1.Name = "vScrollBar1";
            vScrollBar1.Size = new Size(25, 537);
            vScrollBar1.TabIndex = 0;
            // 
            // UpdateImage
            // 
            UpdateImage.Enabled = true;
            UpdateImage.Interval = 25;
            UpdateImage.Tick += UpdateImage_Tick;
            // 
            // menu
            // 
            menu.BackColor = SystemColors.Menu;
            menu.Dock = DockStyle.Top;
            menu.ForeColor = SystemColors.Menu;
            menu.Location = new Point(0, 0);
            menu.Name = "menu";
            menu.Size = new Size(1166, 27);
            menu.TabIndex = 1;
            // 
            // midpanel
            // 
            midpanel.Dock = DockStyle.Fill;
            midpanel.Location = new Point(0, 27);
            midpanel.Name = "midpanel";
            midpanel.Size = new Size(1166, 510);
            midpanel.TabIndex = 2;
            midpanel.Paint += midpanel_Paint;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(1191, 537);
            Controls.Add(midpanel);
            Controls.Add(menu);
            Controls.Add(vScrollBar1);
            Name = "Form1";
            Text = "Logix";
            ResumeLayout(false);
        }

        #endregion

        private VScrollBar vScrollBar1;
        private System.Windows.Forms.Timer UpdateImage;
        private MyPanel midpanel;
        private Panel menu;
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
}