namespace LogixForms
{
    partial class ValueAdres
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
            components = new System.ComponentModel.Container();
            dataGridView1 = new DataGridView();
            Value_type = new ComboBox();
            Adres_name = new ComboBox();
            timer1 = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.Dock = DockStyle.Top;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(342, 384);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            // 
            // Value_type
            // 
            Value_type.FlatStyle = FlatStyle.System;
            Value_type.FormattingEnabled = true;
            Value_type.Items.AddRange(new object[] { "Целочисленный", "Бинарный", "HEX" });
            Value_type.Location = new Point(181, 412);
            Value_type.Name = "Value_type";
            Value_type.Size = new Size(151, 28);
            Value_type.TabIndex = 1;
            Value_type.SelectedIndexChanged += Value_type_SelectedIndexChanged;
            // 
            // Adres_name
            // 
            Adres_name.FlatStyle = FlatStyle.System;
            Adres_name.FormattingEnabled = true;
            Adres_name.Location = new Point(12, 412);
            Adres_name.Name = "Adres_name";
            Adres_name.Size = new Size(151, 28);
            Adres_name.TabIndex = 2;
            Adres_name.Text = "выберите адрес";
            Adres_name.SelectedIndexChanged += Adres_name_SelectedIndexChanged;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 500;
            timer1.Tick += timer1_Tick;
            // 
            // ValueAdres
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(342, 453);
            Controls.Add(Adres_name);
            Controls.Add(Value_type);
            Controls.Add(dataGridView1);
            MinimumSize = new Size(360, 500);
            Name = "ValueAdres";
            Text = "ValueAdre";
            FormClosing += ValueAdres_FormClosing;
            Load += ValueAdres_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private ComboBox Value_type;
        private ComboBox Adres_name;
        private System.Windows.Forms.Timer timer1;
    }
}