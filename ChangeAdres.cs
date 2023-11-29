using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogixForms
{
    public partial class ChangeAdres : Form
    {
        Dictionary<string, ushort[]> adres;
        List<int> rows;
        string[] name_adr;
        int u;
        SettingsLogix sl;
        public ChangeAdres(List<int> CheckBoxOn, Dictionary<string, ushort[]> adreses, SettingsLogix owner)
        {
            InitializeComponent();
            rows = CheckBoxOn;
            adres = adreses;
            sl = owner;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (int.TryParse(this.textBox2.Text, out u))
            {
                if(textBox1.Text != comboBox1.Items[comboBox1.SelectedIndex].ToString())
                {
                    adres.Remove(comboBox1.Items[comboBox1.SelectedIndex].ToString());
                    adres.Add(textBox1.Text, new ushort[u]);
                    sl.UpdateGrid();
                    Close();
                }
                else
                {
                    adres[textBox1.Text] = new ushort[u];
                    sl.UpdateGrid();
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Проверте длинну");
            }
        }

        private void Cancl_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ChangeAdres_Load(object sender, EventArgs e)
        {
            name_adr = adres.Keys.ToArray();
            foreach (int i in rows)
            {
                comboBox1.Items.Add(name_adr[i]);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            textBox2.Text = adres[comboBox1.Items[comboBox1.SelectedIndex].ToString()].Count().ToString();
        }
    }
}
