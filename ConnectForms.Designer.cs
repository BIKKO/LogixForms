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
            button1 = new Button();
            button2 = new Button();
            IP = new TextBox();
            Step = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            Slave = new ComboBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(99, 144);
            button1.Name = "button1";
            button1.Size = new Size(111, 34);
            button1.TabIndex = 0;
            button1.Text = "Connect";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(216, 144);
            button2.Name = "button2";
            button2.Size = new Size(111, 34);
            button2.TabIndex = 1;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // IP
            // 
            IP.Location = new Point(64, 18);
            IP.Name = "IP";
            IP.Size = new Size(263, 27);
            IP.TabIndex = 2;
            IP.Text = "127.0.0.1";
            // 
            // Step
            // 
            Step.Location = new Point(64, 51);
            Step.Name = "Step";
            Step.Size = new Size(263, 27);
            Step.TabIndex = 3;
            Step.Text = "240";
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
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 60);
            label2.Name = "label2";
            label2.Size = new Size(39, 20);
            label2.TabIndex = 6;
            label2.Text = "Step";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 91);
            label3.Name = "label3";
            label3.Size = new Size(44, 20);
            label3.TabIndex = 7;
            label3.Text = "Slave";
            // 
            // Slave
            // 
            Slave.FormattingEnabled = true;
            Slave.Location = new Point(64, 91);
            Slave.Name = "Slave";
            Slave.Size = new Size(61, 28);
            Slave.TabIndex = 8;
            Slave.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // ConnectForms
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(345, 190);
            Controls.Add(Slave);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(Step);
            Controls.Add(IP);
            Controls.Add(button2);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
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
        private TextBox Step;
        private Label label1;
        private Label label2;
        private Label label3;
        private ComboBox Slave;
    }
}