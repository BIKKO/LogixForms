using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogixForms
{
    public partial class ValueAdres : Form
    {
        private int h;
        private int w;
        private Dictionary<string, ushort[]> Adr;
        private int select_type_index = 0;
        public ValueAdres(Dictionary<string, ushort[]> adreses)
        {
            Adr = adreses;
            InitializeComponent();
            h = Height;
            w = Width;
            Value_type.SelectedIndex = 0;
        }

        static private int ContvertDec(string Binari)
        {
            int dec = 0;
            for(int i = 0; i < Binari.Length; i++)
            {
                dec +=(int)(Math.Pow(2, i) * int.Parse(Binari[15-i].ToString()));
            }
            return dec;
        }

        private void Value_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            select_type_index = Value_type.SelectedIndex;
            Adres_name_SelectedIndexChanged(sender, e);
        }

        private void Adres_name_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (Value_type.Items[select_type_index].ToString())
            {
                case "Целочисленный":
                    {
                        var s = Adres_name.SelectedItem as string;
                        if (s is null) break;
                        dataGridView1.RowCount = Adr[s].Length;
                        dataGridView1.ColumnCount = 2;
                        dataGridView1.ColumnHeadersVisible = false;
                        for (int i = 0; i < Adr[s].Length; i++)
                        {
                            dataGridView1.Rows[i].Cells[1].Value = Adr[s][i];
                            dataGridView1.Rows[i].Cells[0].Value = s + ':' + i;
                        }
                        break;
                    }
                case "Бинарный":
                    {
                        var s = Adres_name.SelectedItem as string;
                        if (s is null) break;
                        dataGridView1.RowCount = Adr[s].Length;
                        dataGridView1.ColumnCount = 17;
                        dataGridView1.ColumnHeadersVisible = true;
                        for (int i = 0; i < Adr[s].Length; i++)
                        {
                            dataGridView1.Rows[i].Cells[0].Value = s + ':' + i + '/';
                            string bin = Convert.ToString(Adr[s][i], 2);
                            bin = new string('0', 16 - bin.Length) + bin;
                            for (int j = 1; j < 17; j++)
                            {
                                dataGridView1.Columns[j].HeaderText = (16 - j).ToString();
                                dataGridView1.Rows[i].Cells[j].Value = bin[j - 1];
                            }
                        }
                        break;
                    }
                case "HEX":
                    {
                        var s = Adres_name.SelectedItem as string;
                        if (s is null) break;
                        dataGridView1.RowCount = Adr[s].Length;
                        dataGridView1.ColumnCount = 2;
                        dataGridView1.ColumnHeadersVisible = false;
                        for (int i = 0; i < Adr[s].Length; i++)
                        {
                            dataGridView1.Rows[i].Cells[1].Value = Convert.ToString(Adr[s][i], 16);
                            dataGridView1.Rows[i].Cells[0].Value = i;
                        }
                        break;
                    }
                default: break;
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            switch (Value_type.Items[select_type_index].ToString())
            {
                case "Целочисленный":
                    {
                        var cel = dataGridView1.CurrentCellAddress;
                        Adr[Adres_name.SelectedItem.ToString()][cel.Y] = ushort.Parse(dataGridView1.Rows[cel.Y].Cells[1].Value.ToString());
                        break;
                    }
                case "Бинарный":
                    {
                        var cel = dataGridView1.CurrentCellAddress;
                        string s = "";
                        for (int i = 1; i < 17; i++)
                        {
                            s += dataGridView1.Rows[cel.Y].Cells[i].Value.ToString();
                        }
                        //MessageBox.Show(ContvertDec(s).ToString());
                        Adr[Adres_name.SelectedItem.ToString()][cel.Y] = (ushort)ContvertDec(s);
                        break;
                    }
                case "HEX":
                    {
                        var cel = dataGridView1.CurrentCellAddress;
                        Adr[Adres_name.SelectedItem.ToString()][cel.Y] = ushort.Parse(dataGridView1.Rows[cel.Y].Cells[1].Value.ToString(),System.Globalization.NumberStyles.HexNumber);
                        break;
                    }
                default: break;
            }
        }

        private void ValueAdres_Load(object sender, EventArgs e)
        {
            int n = 0;
            foreach (var s in Adr.Keys)
            {
                //MessageBox.Show(s);
                Adres_name.Items.Add(s);
                n++;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Adr = adreses;
        }
    }
}
