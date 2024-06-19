namespace LogixForms
{
    public partial class ChangeAdres : Form
    {
        private Dictionary<string, ushort[]> adres;
        private List<int> rows;
        private string[] name_adr;
        private int u;
        private ushort m;
        private SettingsLogix sl;
        Dictionary<string, ushort> MB_AdresList;

        public ChangeAdres(List<int> CheckBoxOn, Dictionary<string, ushort[]> adreses, Dictionary<string, ushort> mbaders, SettingsLogix owner)
        {
            InitializeComponent();
            rows = CheckBoxOn;
            adres = adreses;
            sl = owner;
            MB_AdresList = mbaders;
            name_adr = new string[] { };
        }


        /// <summary>
        /// Сохранение изменений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, EventArgs e)
        {
            if (int.TryParse(this.AdresCount.Text, out u) && ushort.TryParse(this.MBAdres.Text, out m))
            {
                if(AdresName.Text != comboBox1.Items[comboBox1.SelectedIndex].ToString())
                {
                    adres.Remove(comboBox1.Items[comboBox1.SelectedIndex].ToString());
                    adres.Add(AdresName.Text, new ushort[u]);
                    MB_AdresList.Remove(comboBox1.Items[comboBox1.SelectedIndex].ToString());
                    MB_AdresList.Add(AdresName.Text, m);
                    sl.UpdateGrid();
                    Close();
                }
                else
                {
                    adres[AdresName.Text] = new ushort[u];
                    MB_AdresList[AdresName.Text] = m;
                    sl.UpdateGrid();
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Проверте длинну");
            }
        }

        /// <summary>
        /// Отмена измененй
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancl_Click(object sender, EventArgs e)
        {
            Close();
        }


        /// <summary>
        /// Подгрузка адресов из памяти
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeAdres_Load(object sender, EventArgs e)
        {
            
            name_adr = adres.Keys.ToArray();
            foreach (int i in rows)
            {
                comboBox1.Items.Add(name_adr[i]);
            }
        }

        /// <summary>
        /// Выделение выбранного адреса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdresName.Text = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            AdresCount.Text = adres[comboBox1.Items[comboBox1.SelectedIndex].ToString()].Count().ToString();
            MBAdres.Text = MB_AdresList[comboBox1.Items[comboBox1.SelectedIndex].ToString()].ToString();
        }
    }
}
