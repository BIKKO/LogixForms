using Modbus.Device;
using Modbus.Extensions.Enron;

namespace LogixForms
{
    public partial class ValueAdres : Form
    {
        private Dictionary<string, ushort[]> Adr;
        private int select_type_index = 0;
        private ModbusIpMaster master;
        private Dictionary<string, ushort> MBAdres;
        private bool ConActiv;
        private byte slave;

        public ValueAdres(ref Dictionary<string, ushort[]> adreses, string NameTab, ModbusIpMaster _master, ref Dictionary<string, ushort> _MBAdres, ref bool _ConActiv, byte slave)
        {
            Adr = adreses;
            InitializeComponent();
            Value_type.SelectedIndex = 0;
            Text += ": " + NameTab;
            this.Name = NameTab;
            master = _master;
            MBAdres = _MBAdres;
            ConActiv = _ConActiv;
            this.slave = slave;
        }

        /// <summary>
        /// Создание и изменение таблицы значаний
        /// </summary>
        private async void CreateTabl()
        {
            switch (Value_type.Items[select_type_index].ToString())
            {
                case "Целочисленный":
                    {
                        dataGridView1.ReadOnly = false;
                        string? s = Adres_name.SelectedItem as string;
                        if (s is null) break;
                        dataGridView1.RowCount = Adr[s].Length;
                        dataGridView1.ColumnCount = 2;
                        dataGridView1.ColumnHeadersVisible = false;
                        for (int i = 0; i < Adr[s].Length; i++)
                        {
                            if (dataGridView1.Rows[i].Cells[1].Value != Adr[s][i].ToString())
                                dataGridView1.Rows[i].Cells[1].Value = Adr[s][i];
                            dataGridView1.Rows[i].Cells[0].Value = s + ':' + i;
                        }
                        break;
                    }
                case "Бинарный":
                    {
                        dataGridView1.ReadOnly = true;
                        string? s = Adres_name.SelectedItem as string;
                        if (s is null) break;
                        dataGridView1.RowCount = Adr[s].Length;
                        dataGridView1.ColumnCount = 17;
                        dataGridView1.ColumnHeadersVisible = true;
                        ushort bin;
                        dataGridView1.Columns[0].Frozen = true;
                        for (int i = 0; i < Adr[s].Length; i++)
                        {
                            dataGridView1.Rows[i].Cells[0].Value = s + ':' + i + '/';
                            bin = Adr[s][i];

                            for (int j = 0; j < 16; j++)
                            {
                                dataGridView1.Columns[j + 1].HeaderText = (15 - j).ToString();
                                if ((bin & 32768 >> j) != 0)
                                {
                                    if (dataGridView1.Rows[i].Cells[j + 1].Value != (object)1)
                                        dataGridView1.Rows[i].Cells[j + 1].Value = 1;
                                }
                                else
                                {
                                    if (dataGridView1.Rows[i].Cells[j + 1].Value != (object)0)
                                        dataGridView1.Rows[i].Cells[j + 1].Value = 0;
                                }
                            }
                        }
                        break;
                    }
                case "HEX":
                    {
                        dataGridView1.ReadOnly = false;
                        string? s = Adres_name.SelectedItem as string;
                        if (s is null) break;
                        dataGridView1.RowCount = Adr[s].Length;
                        dataGridView1.ColumnCount = 2;
                        dataGridView1.ColumnHeadersVisible = false;
                        for (int i = 0; i < Adr[s].Length; i++)
                        {
                            if (dataGridView1.Rows[i].Cells[1].Value != Convert.ToString(Adr[s][i], 16))
                                dataGridView1.Rows[i].Cells[1].Value = Convert.ToString(Adr[s][i], 16);
                            dataGridView1.Rows[i].Cells[0].Value = i;
                        }
                        break;
                    }
                default: break;
            }
            await Task.Delay(1);
        }

        /// <summary>
        /// Конвертация в целое число
        /// </summary>
        /// <param name="Binari"></param>
        /// <returns></returns>
        static private int ContvertDec(string Binari)
        {
            int dec = 0;
            for (int i = 0; i < Binari.Length; i++)
            {
                dec += (int)(Math.Pow(2, i) * int.Parse(Binari[15 - i].ToString()));
            }
            return dec;
        }

        /// <summary>
        /// Выбор системы счисления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Value_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Select();
            select_type_index = Value_type.SelectedIndex;
            Adres_name_SelectedIndexChanged(sender, e);
        }

        /// <summary>
        /// Показ выбранного адреса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Adres_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateTabl();
            dataGridView1.Select();
        }

        /// <summary>
        /// Изменение значения в памяти
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            switch (Value_type.Items[select_type_index].ToString())
            {
                case "Целочисленный":
                    {
                        dataGridView1.ReadOnly = false;
                        var cel = dataGridView1.CurrentCellAddress;
                        Adr[Adres_name.SelectedItem.ToString()][cel.Y] = ushort.Parse(dataGridView1.Rows[cel.Y].Cells[1].Value.ToString());
                        if (master != null && ConActiv)
                        {
                            master.WriteSingleRegister(slave, (ushort)(MBAdres[Adres_name.SelectedItem.ToString()] + cel.Y), Adr[Adres_name.SelectedItem.ToString()][cel.Y]);
                        }
                        break;
                    }
                case "Бинарный":
                    {
                        dataGridView1.ReadOnly = true;
                        var cel = dataGridView1.CurrentCellAddress;
                        string s = "";
                        for (int i = 1; i < 17; i++)
                        {
                            s += dataGridView1.Rows[cel.Y].Cells[i].Value.ToString();
                        }
                        //MessageBox.Show(ContvertDec(s).ToString());
                        Adr[Adres_name.SelectedItem.ToString()][cel.Y] = (ushort)ContvertDec(s);
                        if (master != null && ConActiv)
                        {
                            //master.WriteMultipleRegisters(1, MBAdres[Adres_name.SelectedItem.ToString()], Adr[Adres_name.SelectedItem.ToString()]);
                            master.WriteSingleRegister(slave, (ushort)(MBAdres[Adres_name.SelectedItem.ToString()]+ cel.Y), Adr[Adres_name.SelectedItem.ToString()][cel.Y]);
                        }
                        break;
                    }
                case "HEX":
                    {
                        dataGridView1.ReadOnly = false;
                        var cel = dataGridView1.CurrentCellAddress;
                        Adr[Adres_name.SelectedItem.ToString()][cel.Y] = ushort.Parse(dataGridView1.Rows[cel.Y].Cells[1].Value.ToString(), System.Globalization.NumberStyles.HexNumber);
                        if (master != null && ConActiv)
                        {
                            master.WriteSingleRegister(slave, (ushort)(MBAdres[Adres_name.SelectedItem.ToString()] + cel.Y), Adr[Adres_name.SelectedItem.ToString()][cel.Y]);
                        }
                        break;
                    }
                default: break;
            }
        }

        /// <summary>
        /// Загрузка значений из адресов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Завершение просмотра
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValueAdres_FormClosing(object sender, FormClosingEventArgs e)
        {
            dataGridView1.Dispose();
            GC.Collect();
        }

        /// <summary>
        /// Изминение по двойнму клику на ячейке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Value_type.Items[select_type_index].ToString() == "Бинарный")
            {
                dataGridView1.ReadOnly = true;
                try
                {
                    //изенение бита на противоположный
                    if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "0")
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 1;//дд смену по дабл клику
                    else
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
                    //сохранение в памяти
                    int cel = dataGridView1.CurrentCellAddress.Y;
                    string s = "";
                    for (int i = 1; i < 17; i++)
                    {
                        s += dataGridView1.Rows[cel].Cells[i].Value.ToString();
                    }
                    Adr[Adres_name.SelectedItem.ToString()][cel] = (ushort)ContvertDec(s);
                    if (master != null && ConActiv)
                    {
                        master.WriteSingleRegister(slave, (ushort)(MBAdres[Adres_name.SelectedItem.ToString()] + cel), Adr[Adres_name.SelectedItem.ToString()][cel]);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Обновление таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            CreateTabl();
        }
    }
}
