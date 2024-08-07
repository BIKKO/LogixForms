﻿namespace LogixForms
{
    public partial class AddAdres : Form
    {
        Dictionary<string, ushort[]> adres;
        Dictionary<string, ushort> MbAdress;
        int u;
        int s;
        SettingsLogix sl;

        public AddAdres(Dictionary<string, ushort[]> adreses, Dictionary<string, ushort> MbAdres, SettingsLogix owner)
        {
            InitializeComponent();
            adres = adreses;
            MbAdress = MbAdres;
            sl = owner;
        }

        /// <summary>
        /// Сохранение нового адреса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox2.Text, out u) && int.TryParse(textBox3.Text, out s))
            {
                adres.Add(textBox1.Text, new ushort[u]);
                MbAdress.Add(textBox1.Text, ushort.Parse(textBox3.Text));
                sl.UpdateGrid();
                Close();
            }
            else
            {
                MessageBox.Show("Проверте длинну");
            }
        }

        /// <summary>
        /// Отмена создания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancl_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
