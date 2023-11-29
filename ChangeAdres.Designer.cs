namespace LogixForms
{
    partial class ChangeAdres
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
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            Save = new Button();
            Cancl = new Button();
            comboBox1 = new ComboBox();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(24, 95);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(205, 27);
            textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(235, 95);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(91, 27);
            textBox2.TabIndex = 1;
            // 
            // Save
            // 
            Save.Location = new Point(366, 94);
            Save.Name = "Save";
            Save.Size = new Size(94, 29);
            Save.TabIndex = 2;
            Save.Text = "Сохранить";
            Save.UseVisualStyleBackColor = true;
            Save.Click += Save_Click;
            // 
            // Cancl
            // 
            Cancl.Location = new Point(466, 95);
            Cancl.Name = "Cancl";
            Cancl.Size = new Size(94, 29);
            Cancl.TabIndex = 3;
            Cancl.Text = "Отмена";
            Cancl.UseVisualStyleBackColor = true;
            Cancl.Click += Cancl_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(24, 37);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(205, 28);
            comboBox1.TabIndex = 4;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // ChangeAdres
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(582, 139);
            Controls.Add(comboBox1);
            Controls.Add(Cancl);
            Controls.Add(Save);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Name = "ChangeAdres";
            Text = "ChangeAdres";
            Load += ChangeAdres_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private Button Save;
        private Button Cancl;
        private ComboBox comboBox1;
    }
}