using System.Diagnostics;
using System.Windows.Forms;

namespace LogixForms
{
    public partial class SettingsLogix : Form
    {
        private Dictionary<string, ushort[]> Data;
        private List<int> rows = new List<int>();
        private string[] name_adr;
        private Dictionary<string, ushort> mb_adres;
        private int rangadr;
        private int cfg;
        private MainThread owner;

        public SettingsLogix(MainThread owner)
        {
            InitializeComponent();
            Data = owner.Data;
            mb_adres = owner.ModBusAdres;
            rangadr = owner.RangsADR;
            cfg = owner.ConfigAdr;
            this.owner = owner;

            TimerDel.Text = owner.AdrUpdateTime.ToString();
        }

        /// <summary>
        /// Обновление таблицы
        /// </summary>
        public void UpdateGrid()
        {
#if DEBUG
            try
            {
                dataGridView1.RowCount = Data.Count+2;
                name_adr = Data.Keys.ToArray();

                dataGridView1.Rows[0].Cells[1].Value = "Ранги";
                dataGridView1.Rows[0].Cells[2].Value = rangadr;

                dataGridView1.Rows[1].Cells[1].Value = "Конфигурация";
                dataGridView1.Rows[1].Cells[2].Value = cfg;

                for (int i = 0; i < Data.Count; i++)
                {
                    dataGridView1.Rows[i+2].Cells[1].Value = name_adr[i];
                    dataGridView1.Rows[i+2].Cells[2].Value = mb_adres[name_adr[i]];
                    dataGridView1.Rows[i+2].Cells[3].Value = Data[name_adr[i]].Length;
                }
            }
            catch
            {
                Debug.Print("Адреса утеряны!!!");
            }
#else
            dataGridView1.RowCount = Data.Count + 2;
            name_adr = Data.Keys.ToArray();

            dataGridView1.Rows[0].Cells[1].Value = "Ранги";
            dataGridView1.Rows[0].Cells[2].Value = rangadr;

            dataGridView1.Rows[1].Cells[1].Value = "Конфигурация";
            dataGridView1.Rows[1].Cells[2].Value = cfg;

            for (int i = 0; i < Data.Count; i++)
            {
                dataGridView1.Rows[i + 2].Cells[1].Value = name_adr[i];
                dataGridView1.Rows[i + 2].Cells[2].Value = mb_adres[name_adr[i]];
                dataGridView1.Rows[i + 2].Cells[3].Value = Data[name_adr[i]].Length;
            }
#endif
        }

        /// <summary>
        /// Создание таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsLogix_Load(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        /// <summary>
        /// Сохранение и отображение chekbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Index == e.RowIndex && e.RowIndex > 1)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            row.Cells[0].Value = false;
                            rows.Remove(e.RowIndex);
                        }
                        else
                        {
                            row.Cells[0].Value = true;
                            rows.Add(e.RowIndex - 2);
                        }
                    }
                    else continue;
                }
            }
            if (rows.Count > 0)
            {
                if (!Delite.Enabled)
                {
                    Delite.Enabled = true;
                }
            }
            else
            {
                Delite.Enabled = false;
            }
        }

        /// <summary>
        /// Удаление выбранного адреса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delite_Click(object sender, EventArgs e)
        {
            foreach (int ind in rows)
            {
                dataGridView1.Rows[ind].Cells[0].Value = false;
                Data.Remove(name_adr[ind]);
                mb_adres.Remove(name_adr[ind]);
            }
            rows.Clear();
            Delite.Enabled = false;
            UpdateGrid();
        }

        /// <summary>
        /// Добавление нового адреса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBut_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["AddAdres"] == null)
            {
                new AddAdres(Data, mb_adres, this).Show();
            }
        }

        /// <summary>
        /// Выгрузка адресов и имен
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UPload_Click(object sender, EventArgs e)
        {
            string _data = "";

            _data += "RANDS:" + rangadr + "\n";
            _data += "CONFIG:" + (cfg - 1) + "\n";

            foreach (string key in mb_adres.Keys)
            {
                _data += key + ":" + mb_adres[key] + ';' + this.Data[key].Length + "\n";
            }
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.Filter = "ModBus config (*.MBcfg)|*.MBcfg";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                CreateFile.ToFile(saveFileDialog1.FileName, _data);
            }
        }

        /// <summary>
        /// Загрузка адресов и имен
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Load_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "ModBus config (*.MBcfg)|*.MBcfg";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] _data = File.ReadAllLines(openFileDialog1.FileName).Where(x => x != "").ToArray();
                string[] buf;
                var mbres = MessageBox.Show("Заменить имеющиеся данные на полученные из файла?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (mbres == DialogResult.Yes)
                {
                    rangadr = int.Parse(_data[0].Split(':')[1]);
                    cfg = int.Parse(_data[1].Split(':')[1]);
                    mb_adres.Clear();
                    this.Data.Clear();
                    for (int i = 2; i < _data.Length; i++)
                    {
                        buf = _data[i].Split(":")[1].Split(';');
                        mb_adres.Add(_data[i].Split(":")[0], ushort.Parse(buf[0]));
                        this.Data.Add(_data[i].Split(":")[0], new ushort[int.Parse(buf[1])]);
                    }
                }
                else if (mbres == DialogResult.No)
                {
                    for (int i = 2; i < _data.Length; i++)
                    {
                        buf = _data[i].Split(":");
                        if (!mb_adres.ContainsKey(buf[0]))
                        {
                            buf = buf[1].Split(';');
                            mb_adres.Add(_data[i].Split(":")[0], ushort.Parse(buf[0]));
                            this.Data.Add(_data[i].Split(":")[0], new ushort[int.Parse(buf[1])]);
                        }
                    }
                }
                UpdateGrid();
            }
        }

        /// <summary>
        /// Сохранене ихменений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > 0)
            {
                //Сохранение ранов и регистров
                if (e.ColumnIndex == 2 && e.RowIndex < 2)
                {
                    if (e.RowIndex == 0)
                    {
                        if (dataGridView1.Rows[e.RowIndex].Cells[2].Value != null)
                            owner.RangsADR = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                        else
                            dataGridView1.Rows[e.RowIndex].Cells[2].Value = owner.RangsADR;
                    }
                    else
                    {
                        if (dataGridView1.Rows[e.RowIndex].Cells[2].Value != null)
                            owner.ConfigAdr = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                        else
                            dataGridView1.Rows[e.RowIndex].Cells[2].Value = owner.ConfigAdr;
                    }

                }
                else if (e.ColumnIndex == 1 && e.RowIndex < 2)
                {
                    if (e.RowIndex == 0)
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[1].Value = "Ранги";
                    }
                    else
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[1].Value = "Конфигурация";
                    }
                }
                else if (e.ColumnIndex == 3 && e.RowIndex < 2)
                    dataGridView1.Rows[e.RowIndex].Cells[3].Value = null;

                else// Остальные изм
                {
                    if (e.ColumnIndex == 1)
                    {
                        Dictionary<string, ushort[]> buf_data = new Dictionary<string, ushort[]>();
                        Dictionary<string, ushort> buf_mb = new Dictionary<string, ushort>();

                        string[] key = Data.Keys.ToArray();
                        for (int i = 0; i < key.Length; i++)
                        {
                            string buf = dataGridView1.Rows[i + 2].Cells[1].Value.ToString();
                            if (buf == key[i].ToString())
                            {
                                buf_data.Add(key[i], Data[key[i]]);
                                buf_mb.Add(key[i], mb_adres[key[i]]);
                            }
                            else
                            {
                                buf_data.Add(buf, Data[key[i]]);
                                buf_mb.Add(buf, mb_adres[key[i]]);
                            }
                        }
                        owner.Data = buf_data;
                        owner.ModBusAdres = buf_mb;
                        buf_mb = null;
                        buf_data = null;
                    }
                    else if (e.ColumnIndex == 2)
                    {
                        ushort u;
                        string buf = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                        if (ushort.TryParse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(), out u))
                        {
                            mb_adres[buf] = u;
                        }
                        else dataGridView1.Rows[e.RowIndex].Cells[2].Value = mb_adres[buf];
                    }
                    else if (e.ColumnIndex == 3)
                    {
                        ushort u;
                        string buf = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                        if (ushort.TryParse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString(), out u))
                        {
                            Data[buf] = new ushort[u];
                        }
                        else dataGridView1.Rows[e.RowIndex].Cells[3].Value = Data[buf];
                    }
                }
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            var but = sender as Button;
            int u;
            if (but.Name == "SaveNet")
            {
                if (int.TryParse(TimerDel.Text, out u))
                {
                    owner.AdrUpdateTime = u;
                }
                else
                {
                    TimerDel.Text = owner.AdrUpdateTime.ToString();
                }
            }
            else
            {

            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            var but = sender as Button;

        }
    }
}