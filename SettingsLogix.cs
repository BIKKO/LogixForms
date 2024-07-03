using System.Diagnostics;
using System.Windows.Forms;

namespace LogixForms
{
    public partial class SettingsLogix : Form
    {
        private Dictionary<string, ushort[]> adres;
        private List<int> rows = new List<int>();
        private string[] name_adr;
        Dictionary<string, ushort> adres_adr;
        private int rangadr;
        private int cfg;

        public SettingsLogix(ref Dictionary<string, ushort[]> adreses, ref Dictionary<string, ushort> mbadres, ref int rang_adr, ref int cfg_adr)
        {
            InitializeComponent();
            adres = adreses;
            adres_adr = mbadres;
            rangadr = rang_adr;
            cfg = cfg_adr;
        }

        /// <summary>
        /// Обновление таблицы
        /// </summary>
        public void UpdateGrid()
        {
            #if DEBUG
            try
            {
                dataGridView1.RowCount = adres.Count;
                name_adr = adres.Keys.ToArray();

                dataGridView1.Rows[0].Cells[1].Value = "Регистры";
                dataGridView1.Rows[0].Cells[2].Value = rangadr;

                dataGridView1.Rows[1].Cells[1].Value = "Конфигурация";
                dataGridView1.Rows[1].Cells[2].Value = cfg;

                for (int i = 2; i < adres.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[1].Value = name_adr[i];
                    dataGridView1.Rows[i].Cells[2].Value = adres_adr[name_adr[i]];
                    dataGridView1.Rows[i].Cells[3].Value = adres[name_adr[i]].Length;
                }
            }
            catch
            {
                Debug.Print("Адреса утеряны!!!");
            }
            #else
                dataGridView1.RowCount = adres.Count;
                name_adr = adres.Keys.ToArray();
                for (int i = 0; i < adres.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[1].Value = name_adr[i];
                    dataGridView1.Rows[i].Cells[2].Value = adres_adr[name_adr[i]];
                    dataGridView1.Rows[i].Cells[3].Value = adres[name_adr[i]].Length;
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
                    if (row.Index == e.RowIndex && e.RowIndex != 0 && e.RowIndex != 1)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            row.Cells[0].Value = false;
                            rows.Remove(e.RowIndex);
                        }
                        else
                        {
                            row.Cells[0].Value = true;
                            rows.Add(e.RowIndex);
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
                adres.Remove(name_adr[ind]);
                adres_adr.Remove(name_adr[ind]);
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
                new AddAdres(adres, adres_adr, this).Show();
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

            foreach (string key in adres_adr.Keys)
            {
                _data += key + ":" + adres_adr[key] +';' + adres[key].Length +"\n";
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
                    adres_adr.Clear();
                    adres.Clear();
                    for (int i = 2; i < _data.Length; i++)
                    {
                        buf = _data[i].Split(":")[1].Split(';');
                        adres_adr.Add(_data[i].Split(":")[0], ushort.Parse(buf[0]));
                        adres.Add(_data[i].Split(":")[0], new ushort[40]);
                    }
                }
                else if (mbres == DialogResult.No)
                {
                    for (int i = 2; i < _data.Length; i++)
                    {
                        buf = _data[i].Split(":");
                        if (!adres_adr.ContainsKey(buf[0]))
                        {
                            buf = buf[1].Split(';');
                            adres_adr.Add(_data[i].Split(":")[0], ushort.Parse(buf[0]));
                            adres.Add(_data[i].Split(":")[0], new ushort[40]);
                        }
                    }
                }
                UpdateGrid();
            }
        }
    }
}
