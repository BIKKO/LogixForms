using Modbus.Device;
using System.Net.Sockets;
using System.Text;

namespace LogixForms
{
    public partial class MainThread : Form
    {
        // значения адресов
        private static Dictionary<string, ushort[]> Adr = new Dictionary<string, ushort[]>();
        private static Dictionary<string, ushort> MB_adres = new Dictionary<string, ushort>() { {"T4",1300},
                                                                                                {"T4_c",7000},
                                                                                                {"Timer_control",6800},
                                                                                                {"N13",1000},
                                                                                                {"N15",600},
                                                                                                {"N18",1200},
                                                                                                {"N40",2000},
                                                                                                {"B3",7200},
                                                                                                };

        /*
        private static ushort[] T4 = new ushort[24];
        private static ushort[] T4_c = new ushort[24];
        private static ushort[] T4_b = new ushort[24];
        private static int[] Timer_control = new int[32];
        private static ushort[] N13 = new ushort[70];
        private static ushort[] N15 = new ushort[70];
        private static ushort[] N18 = new ushort[70];
        private static ushort[] N40 = new ushort[70];
        private static ushort[] B3 = new ushort[70];*/

        //файл(ddd | ddd - copy)
        private static List<List<string>> OpenFileOrCon = new List<List<string>>();
        private static List<string> TextRangs = new List<string>();//= File.ReadAllLines(@"C:\Users\njnji\Desktop\проеты\matplotlib\ddd", Encoding.UTF8);
        private List<VScrollBar> VScrollBarList = new List<VScrollBar>();
        private List<HScrollBar> HScrollBarList = new List<HScrollBar>();
        //private int isnumber;
        private bool OpenFile = false;
        private bool ModbusCl = false;
        private ModbusIpMaster master;
        private bool NotFount = false;
        private List<ClassDrow> mainWindows = new List<ClassDrow>();
        private List<TcpClient> TcpClients = new List<TcpClient>();
        private TcpClient client;


        public MainThread()
        {
            //Files.Visible = false;
            InitializeComponent();//инициализация формы
            MouseWheel += This_MouseWheel;//подключения колёсика мыши
            Height = int.Parse(Properties.Settings.Default["H"].ToString());
            Width = int.Parse(Properties.Settings.Default["W"].ToString());
            //Adr = (Dictionary<string, ushort[]>)Properties.Settings.Default["Adres"];
            Adr.Add("T4", new ushort[24]);
            Adr.Add("T4_c", new ushort[24]);
            Adr.Add("T4_b", new ushort[24]);
            Adr.Add("Timer_control", new ushort[32]);
            Adr.Add("N13", new ushort[70]);
            Adr.Add("N15", new ushort[70]);
            Adr.Add("N18", new ushort[70]);
            Adr.Add("N40", new ushort[70]);
            Adr.Add("B3", new ushort[70]);
            AdresUpdate.Enabled = false;
        }

        /// <summary>
        /// Обработчик прокрутки колесика мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void This_MouseWheel(object sender, MouseEventArgs e)
        {
            int wheel = 0;//прокрутка вверх или вниз
            if (e.Delta > 0)
            {
                //вверх
                wheel = -(int)(VScrollBarList[Files.SelectedIndex].Maximum*0.05);//если рангов > 10 то -1 иначе -10
                //MessageBox.Show("Test");
            }
            else
            {
                //вниз
                wheel = (int)(VScrollBarList[Files.SelectedIndex].Maximum * 0.05);//если рангов > 10 то 1 иначе 10
            }
            if (Files.TabCount > 0)
            {
                if (VScrollBarList[Files.SelectedIndex].Maximum >= VScrollBarList[Files.SelectedIndex].Value + wheel && VScrollBarList[Files.SelectedIndex].Minimum <= VScrollBarList[Files.SelectedIndex].Value + wheel)
                    VScrollBarList[Files.SelectedIndex].Value += wheel;//не выходим ли за приделы scrollbar
            }
            wheel = 0;//одиночное сробатование
        }

        /// <summary>
        /// Обновление значений адресов в памяти с устройтва
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdresUpdate_Tick(object sender, EventArgs e)
        {
            string[] adreskey = Adr.Keys.ToArray();
            foreach (string adkey in adreskey)
            {
                Adr[adkey] = master.ReadHoldingRegisters(1, MB_adres[adkey], (ushort)Adr[adkey].Length);
            }
        }

        /// <summary>
        /// Таймер оистки кучи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemoryClear_Tick(object sender, EventArgs e)
        {
            GC.Collect();
        }

        /// <summary>
        /// Обновление програмы с устройства
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModBusUpdate_Tick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Обновление программы с файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileUpdate_Tick(object sender, EventArgs e)
        {
            FileUpdate.Enabled = false;
            OpenFileOrCon[Files.SelectedIndex].Clear();
            foreach (var el in File.ReadAllLines(openFileDialog2.FileName, Encoding.UTF8)) OpenFileOrCon[Files.SelectedIndex].Add(el);
        }

        /// <summary>
        /// Закрытие окна отображения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void close_Click(object sender, EventArgs e)
        {
            mainWindows[Files.SelectedIndex] = null;
            mainWindows.Remove(mainWindows[Files.SelectedIndex]);
            if (TcpClients.Count > 0)
            {
                TcpClients[0] = null;
                TcpClients.Clear();
            }
            Files.TabPages.Remove(Files.SelectedTab);
            GC.Collect();
        }

        /// <summary>
        /// Открытие файла с ПК
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                var tb = new MyTabPage();
                tb.Text = openFileDialog2.FileName;

                VScrollBar vscrol = new()
                {
                    Dock = DockStyle.Right,
                    Width = 20,
                    Maximum = 100,
                    Minimum = 0,
                    Value = 0
                };
                MyPanel pan = new()
                {
                    Dock = DockStyle.Fill,
                    Height = Height - 20,
                    Width = 1300 - 50,
                };
                HScrollBar hScroll = new()
                {
                    Dock = DockStyle.Bottom,
                    Height = 20,
                    Maximum = pan.Width,
                    Minimum = 0,
                };
                VScrollBarList.Add(vscrol);
                HScrollBarList.Add(hScroll);
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                var close = new ToolStripMenuItem("Закрыть");
                contextMenu.Items.Add(close);
                tb.ContextMenuStrip = contextMenu;
                close.Click += close_Click;
                pan.Controls.Add(vscrol);
                pan.Controls.Add(hScroll);
                tb.Controls.Add(pan);
                Files.TabPages.Add(tb);
                Files.SelectTab(Files.TabCount - 1);
                ClassDrow tab_to_drow = new ClassDrow(pan, File.ReadAllLines(openFileDialog2.FileName,
                    Encoding.UTF8).ToList(), vscrol,
                    hScroll, Files, Adr, Height, Width);
                tab_to_drow.StartDrow();
                mainWindows.Add(tab_to_drow);
            }
        }

        /// <summary>
        /// Сохранение программмы на ПК
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = @"C:\Users\PC\Desktop\";
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.DefaultExt = "RandsSave";
            saveFileDialog1.Filter = "txt files (*.txt) | *.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream file = saveFileDialog1.OpenFile();
                StreamWriter sw = new StreamWriter(file);
                foreach (string rang in OpenFileOrCon[Files.SelectedIndex])
                    sw.WriteLine(rang);
                sw.Close();
                file.Close();
            }
        }

        /// <summary>
        /// Подключение к устройству
        /// </summary>
        /// <param name="ip">IP адрес</param>
        /// <param name="port">Порт</param>
        /// <param name="step">Шаг считывания ModBus адресов(устанавливать 1 при считывании с устройства)</param>
        /// <param name="slave">ID</param>
        public void con(string ip, string port, int step, byte slave)
        {
            try
            {
                TextRangs.Clear();
                client = new TcpClient(ip, int.Parse(port));
                TcpClients.Add(client);
                master = ModbusIpMaster.CreateIp(client);
                ModBusUpdate.Enabled = true;

                ushort[] inputs;

                for (int j = 0; j < 100; j++)
                {
                    inputs = master.ReadHoldingRegisters(slave, (ushort)(j + 8000), 120);
                    string g = "";
                    int len = 0;
                    int buf;

                    for (int i = 0; i < 240; i++)
                    {
                        if (inputs[i] != 0)
                        {
                            buf = (inputs[i] & 0xff);
                            if (buf != 0)
                            {
                                g += (char)((char)inputs[i] & 0xff);
                                len++;
                            }
                            buf = (inputs[i] >> 8);
                            if (buf != 0)
                            {
                                g += (char)((char)inputs[i] >> 8);
                                len++;
                            }
                        }
                        else break;
                    }
                    if (len != 0) TextRangs.Add(g);
                    else break;
                }
                /*
                string[] adreskey = Adr.Keys.ToArray();
                foreach (string adkey in adreskey)
                {
                    Adr[adkey] = master.ReadHoldingRegisters(slave, MB_adres[adkey], (ushort)Adr[adkey].Length);
                }*/

                ModbusCl = true;
                var tb = new TabPage();
                tb.Text = ip + ':' + port;

                VScrollBar Vscrol = new()
                {
                    Dock = DockStyle.Right,
                    Width = 20,
                    Maximum = 100,
                    Minimum = 0,
                    Value = 0
                };
                MyPanel pan = new()
                {
                    Dock = DockStyle.Fill,
                    Height = Height - 20,
                    Width = 1300 - 50,
                };
                HScrollBar hScroll = new()
                {
                    Dock = DockStyle.Bottom,
                    Height = 20,
                    Maximum = pan.Width,
                    Minimum = 0,
                };
                VScrollBarList.Add(Vscrol);
                HScrollBarList.Add(hScroll);
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                var close = new ToolStripMenuItem("Закрыть");
                contextMenu.Items.Add(close);
                tb.ContextMenuStrip = contextMenu;
                close.Click += close_Click;
                pan.Controls.Add(Vscrol);
                pan.Controls.Add(hScroll);
                tb.Controls.Add(pan);
                Files.TabPages.Add(tb);
                Files.SelectTab(Files.TabCount - 1);
                ClassDrow tab_to_drow = new ClassDrow(pan, TextRangs, Vscrol,
                    hScroll, Files, Adr, Height, Width);
                tab_to_drow.StartDrow();
                mainWindows.Add(tab_to_drow);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения. Проверте подключение и повторите попытку.");
            }
        }

        /// <summary>
        /// Вызов окна для ввода данных при подключении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TcpClients.Count < 1)
            {
                FileUpdate.Enabled = false;
                //ModBusUpdate.Enabled = true;

                if (Application.OpenForms["ConnectForms"] == null)
                {
                    new ConnectForms(this).Show();
                    OpenFile = false;
                    //con();
                }
            }
        }

        /// <summary>
        /// Вызов окна настроек
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["SettingsLogix"] == null)
            {
                new SettingsLogix(Adr, MB_adres).Show();
            }
        }

        /// <summary>
        /// Вызов окна справки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Временно ничего нет!");
        }

        /// <summary>
        /// Создание пустого окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tb = new MyTabPage();
            tb.Text = openFileDialog2.FileName;

            VScrollBar Vscrol = new()
            {
                Dock = DockStyle.Right,
                Width = 20,
                Maximum = 100,
                Minimum = 0,
                Value = 0
            };
            MyPanel pan = new()
            {
                Dock = DockStyle.Fill,
                Height = Height - 20,
                Width = 1300 - 50,
            };
            HScrollBar hScroll = new()
            {
                Dock = DockStyle.Bottom,
                Height = 20,
                Maximum = pan.Width,
                Minimum = 0,
            };
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            var close = new ToolStripMenuItem("Закрыть");
            contextMenu.Items.Add(close);
            tb.ContextMenuStrip = contextMenu;
            close.Click += close_Click;
            pan.Controls.Add(Vscrol);
            pan.Controls.Add(hScroll);
            tb.Controls.Add(pan);
            Files.TabPages.Add(tb);
            Files.SelectTab(Files.TabCount - 1);
            ClassDrow tab_to_drow = new ClassDrow(pan, new List<string> { "" }, Vscrol,
                hScroll, Files, Adr, Height, Width);
            tab_to_drow.StartDrow();
            mainWindows.Add(tab_to_drow);
        }

        /// <summary>
        /// Сохранение парамметров перед завершением работы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default["H"] = Height;
            Properties.Settings.Default["W"] = Width;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Получение таблицы значений адресов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void adresesValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["ValueAdre"] == null)
            {
                new ValueAdres(Adr).Show();
            }
        }
    }
}