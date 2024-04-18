namespace LogixForms
{
    partial class AddAdres
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
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            Cancl = new Button();
            Save = new Button();
            textBox3 = new TextBox();
            SuspendLayout();
            // 
            // textBox2
            // 
            textBox2.Location = new Point(182, 12);
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "Длинна";
            textBox2.Size = new Size(91, 27);
            textBox2.TabIndex = 5;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 12);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Название";
            textBox1.Size = new Size(151, 27);
            textBox1.TabIndex = 4;
            // 
            // Cancl
            // 
            Cancl.Location = new Point(182, 95);
            Cancl.Name = "Cancl";
            Cancl.Size = new Size(94, 29);
            Cancl.TabIndex = 7;
            Cancl.Text = "Отмена";
            Cancl.UseVisualStyleBackColor = true;
            Cancl.Click += Cancl_Click;
            // 
            // Save
            // 
            Save.Location = new Point(69, 95);
            Save.Name = "Save";
            Save.Size = new Size(94, 29);
            Save.TabIndex = 6;
            Save.Text = "Сохранить";
            Save.UseVisualStyleBackColor = true;
            Save.Click += Save_Click;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(182, 45);
            textBox3.Name = "textBox3";
            textBox3.PlaceholderText = "Адрес";
            textBox3.Size = new Size(91, 27);
            textBox3.TabIndex = 8;
            // 
            // AddAdres
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(291, 136);
            Controls.Add(textBox3);
            Controls.Add(Cancl);
            Controls.Add(Save);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Name = "AddAdres";
            Text = "AddAdres";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox textBox2;
        private TextBox textBox1;
        private Button Cancl;
        private Button Save;
        private TextBox textBox3;
    }
}