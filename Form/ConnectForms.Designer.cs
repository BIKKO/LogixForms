namespace LogixForms
{
    partial class ConnectForms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectForms));
            button1 = new Button();
            button2 = new Button();
            IP = new TextBox();
            label1 = new Label();
            label4 = new Label();
            Port = new TextBox();
            ConfigSel = new CheckBox();
            label2 = new Label();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(96, 122);
            button1.Name = "button1";
            button1.Size = new Size(109, 34);
            button1.TabIndex = 0;
            button1.Text = "Connect";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(213, 122);
            button2.Name = "button2";
            button2.Size = new Size(109, 34);
            button2.TabIndex = 1;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // IP
            // 
            IP.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            IP.Location = new Point(59, 15);
            IP.Name = "IP";
            IP.Size = new Size(108, 30);
            IP.TabIndex = 2;
            IP.Text = "127.0.0.1";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 18);
            label1.Name = "label1";
            label1.Size = new Size(21, 20);
            label1.TabIndex = 5;
            label1.Text = "IP";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(185, 18);
            label4.Name = "label4";
            label4.Size = new Size(35, 20);
            label4.TabIndex = 10;
            label4.Text = "Port";
            // 
            // Port
            // 
            Port.Location = new Point(242, 15);
            Port.Name = "Port";
            Port.Size = new Size(75, 27);
            Port.TabIndex = 9;
            Port.Text = "502";
            // 
            // ConfigSel
            // 
            ConfigSel.AutoSize = true;
            ConfigSel.Location = new Point(21, 92);
            ConfigSel.Name = "ConfigSel";
            ConfigSel.Size = new Size(212, 24);
            ConfigSel.TabIndex = 11;
            ConfigSel.Text = "Запросить конфигурацию";
            ConfigSel.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 62);
            label2.Name = "label2";
            label2.Size = new Size(59, 20);
            label2.TabIndex = 13;
            label2.Text = "SlaveID";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(77, 59);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(21, 27);
            textBox1.TabIndex = 12;
            textBox1.Text = "1";
            // 
            // ConnectForms
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(331, 168);
            Controls.Add(label2);
            Controls.Add(textBox1);
            Controls.Add(ConfigSel);
            Controls.Add(label4);
            Controls.Add(Port);
            Controls.Add(label1);
            Controls.Add(IP);
            Controls.Add(button2);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "ConnectForms";
            Text = "Connect";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private TextBox IP;
        private Label label1;
        private Label label4;
        private TextBox Port;
        private CheckBox ConfigSel;
        private Label label2;
        private TextBox textBox1;
    }
}