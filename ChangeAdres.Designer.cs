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
            AdresName = new TextBox();
            AdresCount = new TextBox();
            Save = new Button();
            Cancl = new Button();
            MBAdres = new TextBox();
            comboBox1 = new ComboBox();
            SuspendLayout();
            // 
            // AdresName
            // 
            AdresName.Location = new Point(24, 95);
            AdresName.Name = "AdresName";
            AdresName.Size = new Size(205, 27);
            AdresName.TabIndex = 0;
            // 
            // AdresCount
            // 
            AdresCount.Location = new Point(235, 95);
            AdresCount.Name = "AdresCount";
            AdresCount.Size = new Size(91, 27);
            AdresCount.TabIndex = 1;
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
            // MBAdres
            // 
            MBAdres.Location = new Point(24, 62);
            MBAdres.Name = "MBAdres";
            MBAdres.Size = new Size(91, 27);
            MBAdres.TabIndex = 5;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(24, 25);
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
            Controls.Add(MBAdres);
            Controls.Add(comboBox1);
            Controls.Add(Cancl);
            Controls.Add(Save);
            Controls.Add(AdresCount);
            Controls.Add(AdresName);
            Name = "ChangeAdres";
            Text = "ChangeAdres";
            Load += ChangeAdres_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox AdresName;
        private TextBox AdresCount;
        private Button Save;
        private Button Cancl;
        private TextBox MBAdres;
        private ComboBox comboBox1;
    }
}