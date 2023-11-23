namespace LogixForms
{
    partial class SettingsLogix
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
            tabControl1 = new TabControl();
            Common = new TabPage();
            Adreses = new TabPage();
            dataGridView1 = new DataGridView();
            Выбор = new DataGridViewCheckBoxColumn();
            Name = new DataGridViewTextBoxColumn();
            Len = new DataGridViewTextBoxColumn();
            tabControl1.SuspendLayout();
            Adreses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(Common);
            tabControl1.Controls.Add(Adreses);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 450);
            tabControl1.TabIndex = 0;
            // 
            // Common
            // 
            Common.Location = new Point(4, 29);
            Common.Name = "Common";
            Common.Padding = new Padding(3);
            Common.Size = new Size(792, 417);
            Common.TabIndex = 0;
            Common.Text = "Общие";
            Common.UseVisualStyleBackColor = true;
            // 
            // Adreses
            // 
            Adreses.Controls.Add(dataGridView1);
            Adreses.Location = new Point(4, 29);
            Adreses.Name = "Adreses";
            Adreses.Padding = new Padding(3);
            Adreses.Size = new Size(792, 417);
            Adreses.TabIndex = 1;
            Adreses.Text = "Адреса";
            Adreses.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Выбор, Name, Len });
            dataGridView1.Dock = DockStyle.Top;
            dataGridView1.Location = new Point(3, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(786, 356);
            dataGridView1.TabIndex = 0;
            // 
            // Выбор
            // 
            Выбор.HeaderText = "";
            Выбор.MinimumWidth = 6;
            Выбор.Name = "Выбор";
            Выбор.ReadOnly = true;
            Выбор.Width = 6;
            // 
            // Name
            // 
            Name.HeaderText = "Имя";
            Name.MinimumWidth = 6;
            Name.Name = "Name";
            Name.Width = 68;
            // 
            // Len
            // 
            Len.HeaderText = "Длинна";
            Len.MinimumWidth = 6;
            Len.Name = "Len";
            Len.Width = 91;
            // 
            // SettingsLogix
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Text = "SettingsLogix";
            tabControl1.ResumeLayout(false);
            Adreses.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage Common;
        private TabPage Adreses;
        private DataGridView dataGridView1;
        private DataGridViewCheckBoxColumn Выбор;
        private DataGridViewTextBoxColumn Name;
        private DataGridViewTextBoxColumn Len;
    }
}