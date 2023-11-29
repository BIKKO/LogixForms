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
    public partial class AddAdres : Form
    {
        Dictionary<string, ushort[]> adres;
        int u;
        SettingsLogix sl;
        public AddAdres(Dictionary<string, ushort[]> adreses, SettingsLogix owner)
        {
            InitializeComponent();
            adres = adreses;
            sl = owner;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox2.Text, out u))
            {
                adres.Add(textBox1.Text, new ushort[u]);
                sl.UpdateGrid();
                Close();
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
    }
}
