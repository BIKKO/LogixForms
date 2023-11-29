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
            AddBut = new Button();
            Delite = new Button();
            Ref = new Button();
            dataGridView1 = new DataGridView();
            Выбор = new DataGridViewCheckBoxColumn();
            Names = new DataGridViewTextBoxColumn();
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
            tabControl1.Size = new Size(682, 353);
            tabControl1.TabIndex = 0;
            // 
            // Common
            // 
            Common.Location = new Point(4, 29);
            Common.Name = "Common";
            Common.Padding = new Padding(3);
            Common.Size = new Size(674, 320);
            Common.TabIndex = 0;
            Common.Text = "Общие";
            Common.UseVisualStyleBackColor = true;
            // 
            // Adreses
            // 
            Adreses.Controls.Add(AddBut);
            Adreses.Controls.Add(Delite);
            Adreses.Controls.Add(Ref);
            Adreses.Controls.Add(dataGridView1);
            Adreses.Location = new Point(4, 29);
            Adreses.Name = "Adreses";
            Adreses.Padding = new Padding(3);
            Adreses.Size = new Size(674, 320);
            Adreses.TabIndex = 1;
            Adreses.Text = "Адреса";
            Adreses.UseVisualStyleBackColor = true;
            // 
            // AddBut
            // 
            AddBut.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            AddBut.Location = new Point(409, 279);
            AddBut.Name = "AddBut";
            AddBut.Size = new Size(89, 27);
            AddBut.TabIndex = 3;
            AddBut.Text = "Добавить";
            AddBut.UseVisualStyleBackColor = true;
            AddBut.Click += AddBut_Click;
            // 
            // Delite
            // 
            Delite.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Delite.Enabled = false;
            Delite.Location = new Point(596, 279);
            Delite.Name = "Delite";
            Delite.Size = new Size(73, 27);
            Delite.TabIndex = 2;
            Delite.Text = "Удалить";
            Delite.UseVisualStyleBackColor = true;
            Delite.Click += Delite_Click;
            // 
            // Ref
            // 
            Ref.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Ref.Enabled = false;
            Ref.Location = new Point(504, 279);
            Ref.Name = "Ref";
            Ref.Size = new Size(89, 27);
            Ref.TabIndex = 1;
            Ref.Text = "Изменить";
            Ref.UseVisualStyleBackColor = true;
            Ref.Click += Ref_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Выбор, Names, Len });
            dataGridView1.Location = new Point(3, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(668, 270);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            // 
            // Выбор
            // 
            Выбор.FalseValue = "0";
            Выбор.HeaderText = "";
            Выбор.MinimumWidth = 6;
            Выбор.Name = "Выбор";
            Выбор.ReadOnly = true;
            Выбор.TrueValue = "1";
            Выбор.Width = 24;
            // 
            // Names
            // 
            Names.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Names.HeaderText = "Имя";
            Names.MinimumWidth = 6;
            Names.Name = "Names";
            Names.ReadOnly = true;
            // 
            // Len
            // 
            Len.HeaderText = "Длинна";
            Len.MinimumWidth = 6;
            Len.Name = "Len";
            Len.ReadOnly = true;
            Len.Width = 91;
            // 
            // SettingsLogix
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(682, 353);
            Controls.Add(tabControl1);
            MinimumSize = new Size(700, 400);
            Name = "SettingsLogix";
            Text = "SettingsLogix";
            Load += SettingsLogix_Load;
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
        private Button Delite;
        private Button Ref;
        private Button AddBut;
        private DataGridViewCheckBoxColumn Выбор;
        private DataGridViewTextBoxColumn Names;
        private DataGridViewTextBoxColumn Len;
    }
}