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
            components = new System.ComponentModel.Container();
            tabControl1 = new TabControl();
            Common = new TabPage();
            groupBox2 = new GroupBox();
            SaveSet = new Button();
            CancelSet = new Button();
            groupBox1 = new GroupBox();
            SaveNet = new Button();
            CancelNet = new Button();
            label1 = new Label();
            TimerDel = new TextBox();
            Adreses = new TabPage();
            LoadCFG = new Button();
            UPLoad = new Button();
            AddBut = new Button();
            Delite = new Button();
            dataGridView1 = new DataGridView();
            Выбор = new DataGridViewCheckBoxColumn();
            Names = new DataGridViewTextBoxColumn();
            MB_Adres = new DataGridViewTextBoxColumn();
            Len = new DataGridViewTextBoxColumn();
            timer1 = new System.Windows.Forms.Timer(components);
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            tabControl1.SuspendLayout();
            Common.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
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
            Common.Controls.Add(groupBox2);
            Common.Controls.Add(groupBox1);
            Common.Location = new Point(4, 29);
            Common.Name = "Common";
            Common.Padding = new Padding(3);
            Common.Size = new Size(674, 320);
            Common.TabIndex = 0;
            Common.Text = "Общие";
            Common.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(SaveSet);
            groupBox2.Controls.Add(CancelSet);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(327, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(344, 314);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Общие настройки";
            // 
            // SaveSet
            // 
            SaveSet.Enabled = false;
            SaveSet.Location = new Point(138, 281);
            SaveSet.Name = "SaveSet";
            SaveSet.Size = new Size(97, 28);
            SaveSet.TabIndex = 5;
            SaveSet.Tag = "Set";
            SaveSet.Text = "Сохранить";
            SaveSet.UseVisualStyleBackColor = true;
            SaveSet.Click += Save_Click;
            // 
            // CancelSet
            // 
            CancelSet.Enabled = false;
            CancelSet.Location = new Point(241, 281);
            CancelSet.Name = "CancelSet";
            CancelSet.Size = new Size(97, 28);
            CancelSet.TabIndex = 4;
            CancelSet.Tag = "Set";
            CancelSet.Text = "Отмена";
            CancelSet.UseVisualStyleBackColor = true;
            CancelSet.Click += Cancel_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(SaveNet);
            groupBox1.Controls.Add(CancelNet);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(TimerDel);
            groupBox1.Dock = DockStyle.Left;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(324, 314);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Настройки сети";
            // 
            // SaveNet
            // 
            SaveNet.Location = new Point(118, 281);
            SaveNet.Name = "SaveNet";
            SaveNet.Size = new Size(97, 28);
            SaveNet.TabIndex = 3;
            SaveNet.Tag = "Net";
            SaveNet.Text = "Сохранить";
            SaveNet.UseVisualStyleBackColor = true;
            SaveNet.Click += Save_Click;
            // 
            // CancelNet
            // 
            CancelNet.Location = new Point(221, 281);
            CancelNet.Name = "CancelNet";
            CancelNet.Size = new Size(97, 28);
            CancelNet.TabIndex = 2;
            CancelNet.Tag = "Net";
            CancelNet.Text = "Отмена";
            CancelNet.UseVisualStyleBackColor = true;
            CancelNet.Click += Cancel_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 28);
            label1.Name = "label1";
            label1.Size = new Size(131, 20);
            label1.TabIndex = 1;
            label1.Text = "Интервал опроса";
            // 
            // TimerDel
            // 
            TimerDel.Location = new Point(6, 52);
            TimerDel.Name = "TimerDel";
            TimerDel.Size = new Size(71, 27);
            TimerDel.TabIndex = 0;
            // 
            // Adreses
            // 
            Adreses.Controls.Add(LoadCFG);
            Adreses.Controls.Add(UPLoad);
            Adreses.Controls.Add(AddBut);
            Adreses.Controls.Add(Delite);
            Adreses.Controls.Add(dataGridView1);
            Adreses.Location = new Point(4, 29);
            Adreses.Name = "Adreses";
            Adreses.Padding = new Padding(3);
            Adreses.Size = new Size(674, 320);
            Adreses.TabIndex = 1;
            Adreses.Text = "Адреса";
            Adreses.UseVisualStyleBackColor = true;
            // 
            // LoadCFG
            // 
            LoadCFG.Location = new Point(364, 279);
            LoadCFG.Name = "LoadCFG";
            LoadCFG.Size = new Size(125, 27);
            LoadCFG.TabIndex = 5;
            LoadCFG.Text = "Загрузить";
            LoadCFG.UseVisualStyleBackColor = true;
            LoadCFG.Click += Load_Click;
            // 
            // UPLoad
            // 
            UPLoad.Location = new Point(236, 279);
            UPLoad.Name = "UPLoad";
            UPLoad.Size = new Size(125, 27);
            UPLoad.TabIndex = 4;
            UPLoad.Text = "Выгрузить";
            UPLoad.UseVisualStyleBackColor = true;
            UPLoad.Click += UPload_Click;
            // 
            // AddBut
            // 
            AddBut.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            AddBut.Location = new Point(501, 279);
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
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Выбор, Names, MB_Adres, Len });
            dataGridView1.Location = new Point(3, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(668, 270);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            // 
            // Выбор
            // 
            Выбор.FalseValue = "0";
            Выбор.HeaderText = "";
            Выбор.MinimumWidth = 6;
            Выбор.Name = "Выбор";
            Выбор.TrueValue = "1";
            Выбор.Width = 6;
            // 
            // Names
            // 
            Names.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Names.HeaderText = "Имя";
            Names.MinimumWidth = 6;
            Names.Name = "Names";
            // 
            // MB_Adres
            // 
            MB_Adres.HeaderText = "Адрес";
            MB_Adres.MinimumWidth = 6;
            MB_Adres.Name = "MB_Adres";
            MB_Adres.Width = 80;
            // 
            // Len
            // 
            Len.HeaderText = "Длинна";
            Len.MinimumWidth = 6;
            Len.Name = "Len";
            Len.Width = 91;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1000;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
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
            Common.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
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
        private Button AddBut;
        private DataGridViewCheckBoxColumn Выбор;
        private DataGridViewTextBoxColumn Names;
        private DataGridViewTextBoxColumn Len;
        private DataGridViewTextBoxColumn MB_Adres;
        private System.Windows.Forms.Timer timer1;
        private Button LoadCFG;
        private Button UPLoad;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private GroupBox groupBox2;
        private GroupBox groupBox1;
        private Button SaveNet;
        private Button CancelNet;
        private Label label1;
        private TextBox TimerDel;
        private Button SaveSet;
        private Button CancelSet;
    }
}