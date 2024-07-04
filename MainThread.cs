using Modbus.Device;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace LogixForms
{
    public partial class MainThread : Form
    {
        private static Dictionary<string, ushort[]>? Adr;
        protected static Dictionary<string, ushort>? MB_adres;
        private List<ClassDraw> mainWindows = new List<ClassDraw>();
        private List<int> ConnectedWindows = new List<int>();
        public ModbusIpMaster? master;
        private TcpClient? client;
        private byte slave = 1;
        private int RangAdr;
        private int CfgAdr;
        private readonly Dictionary<byte, string> DataType;
#if DEBUG
        bool DebugFlag = false;
#endif

        public MainThread()
        {
            InitializeComponent();//инициализация формы

            AdresUpdate.Enabled = false;
            AdresUpdate.Interval = 400;
            RangAdr = 8000;

            DataType = new Dictionary<byte, string>
            {
                { 1,"B" },
                { 2,"T" },
                { 3,"C" },
                { 4,"R" },
                { 5,"N" },
                { 6,"F" },
                { 7,"S" },
                { 8,"L" },
                { 9,"MG" },
                { 10,"RI" },
                { 11,"T_c" },
                { 12,"Timer_control" },
            };

            Adr = new Dictionary<string, ushort[]>
            {
                { "T4", new ushort[24] },
                { "T4_c", new ushort[24] },
                { "Timer_control", new ushort[32] },
                { "N13", new ushort[70] },
                { "N15", new ushort[70] },
                { "N18", new ushort[70] },
                { "N40", new ushort[70] },
                { "B3", new ushort[70] }
            };

            MB_adres = new Dictionary<string, ushort>() { {"T4",1300},
                                                        {"T4_c",7000},
                                                        {"Timer_control",6800},
                                                        {"N13",1000},
                                                        {"N15",600},
                                                        {"N18",1200},
                                                        {"N40",2000},
                                                        {"B3",7200},
                                                        };

            //Получение данных из файла сохранения
            Height = Properties.Settings.Default.H;
            Width = Properties.Settings.Default.W;
            string[] name = Properties.Settings.Default.AdresName.Split(',');
            string[] len = Properties.Settings.Default.AdresLen.Split(',');
            string[] adr = Properties.Settings.Default.AdresValue.Split(",");
            RangAdr = Properties.Settings.Default.RangAdr;
            CfgAdr = Properties.Settings.Default.CfgAdr;

            if (name.Length > 1 && adr.Length > 1 && len.Length > 1)
            {
                Adr.Clear();
                MB_adres.Clear();
                for (int i = 0; i < name.Length; i++)
                {
                    Adr.Add(name[i], new ushort[int.Parse(len[i])]);
                    MB_adres.Add(name[i], ushort.Parse(adr[i]));
                }
            }
        }

        /// <summary>
        /// Доступ к значению адреса регистра расположения рангов
        /// </summary>
        public int RangsADR
        {
            get { return RangAdr; }
            set { RangAdr = value; }
        }

        /// <summary>
        /// Доступ к значению адреса регистра расположения конфигурации
        /// </summary>
        public int ConfigAdr
        {
            get { return CfgAdr; }
            set { CfgAdr = value; }
        }
        
        /// <summary>
        /// Доступ к значениям адресов
        /// </summary>
        public Dictionary<string, ushort[]> Data
        {
            get { return Adr; }
            set { Adr = value; }
        }
        
        /// <summary>
        /// Доступ к значениям адресов регистров
        /// </summary>
        public Dictionary<string, ushort> ModBusAdres
        {
            get { return MB_adres; }
            set { MB_adres = value; }
        }

        /// <summary>
        /// Обновление значений адресов в памяти с устройтва
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdresUpdate_Tick(object sender, EventArgs e)
        {
            try
            {
#if DEBUG
                if(DebugFlag) 
                    DebugFlag = false;
#endif
                string[]? adreskey = Adr.Keys.ToArray();
                if (adreskey == null) Debug.Print("Null");
                else
                foreach (string adkey in adreskey)
                {
                    Task.Delay(5);
                    Adr[adkey] = master.ReadHoldingRegisters(slave, MB_adres[adkey], (ushort)Adr[adkey].Length);
                }
            }
            catch
            {
                AdresUpdate.Enabled = false;
                string[] name = Properties.Settings.Default["AdresName"].ToString().Split(',');
                string[] len = Properties.Settings.Default["AdresLen"].ToString().Split(',');
                //string[] adr = Properties.Settings.Default["AdresValue"].ToString().Split(',');
                if (name.Length > 1 && len.Length > 1)
                {
                    Adr.Clear();
                    //MB_adres.Clear();
                    for (int i = 0; i < name.Length; i++)
                    {
                        Adr.Add(name[i], new ushort[int.Parse(len[i])]);
                    }
                }

                if (MessageBox.Show("Соединенрие потеряно!\nПереподключиться?", "Ошибка подключения", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    Close_Click(sender, e);
                    ConnectToolStripMenuItem_Click(sender, e);
                }
                else Close_Click(sender, e);

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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseWheelUpdate_Tick(object sender, EventArgs e)
        {
            if (mainWindows.Count > 0)
            {
                foreach (ClassDraw i in mainWindows) i.EnableScroll = false;
                foreach (TabPage item in Files.TabPages)
                {
                    if(item.Text == Files.TabPages[Files.SelectedIndex].Text) mainWindows[Files.SelectedIndex].EnableScroll = true;
                }
            }
        }

        /// <summary>
        /// Закрытие окна отображения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, EventArgs e)
        {
            try
            {
                MouseWheel -= mainWindows[Files.SelectedIndex].This_MouseWheel;
                if (ConnectedWindows.Contains(Files.SelectedIndex) && ConnectedWindows.Count - 1 == 0)
                {
                    AdresUpdate.Enabled = false;
                    ModBusUpdate.Enabled = false;
                    master = null;
                    ConnectedWindows.Remove(Files.SelectedIndex);
                }
            }
            finally
            {
                ClassDraw cd = mainWindows[Files.SelectedIndex];
                mainWindows.Remove(cd);
                if(Application.OpenForms[Files.SelectedTab.Text] != null)
                    Application.OpenForms[Files.SelectedTab.Text].Close();
                TabPage tp = Files.SelectedTab;
                Files.TabPages.Remove(Files.SelectedTab);
                tp.Dispose();
                cd.Dispose();
                AdresUpdate.Enabled = false;
                ModBusUpdate.Enabled = false;
                master = null;
                GC.Collect();
            }
        }

        /// <summary>
        /// Сохранение программмы на ПК
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = @"C:\Users\PC\Desktop\";
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.DefaultExt = "RangsSave";
            saveFileDialog1.Filter = "My files (*.ldf)|*.ldf|txt files (*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (Files.SelectedTab.Text != saveFileDialog1.FileName.Split('\\')[^1]) Files.SelectedTab.Text = saveFileDialog1.FileName.Split('\\')[^1];
                Stream file = saveFileDialog1.OpenFile();
                StreamWriter sw = new StreamWriter(file);
                if (saveFileDialog1.FileName.Contains(".ldf"))
                {
                    sw.WriteLine(CreateFile.Create(mainWindows[Files.SelectedIndex].GetTextRang, mainWindows[Files.SelectedIndex].GetDataTabl, CreateFile.CreateTEGS(mainWindows[Files.SelectedIndex].GetTegs)));
                    sw.Close();
                    file.Close();
                    return;
                }
                foreach (string rang in mainWindows[Files.SelectedIndex].GetTextRang)
                    sw.WriteLine(rang);
                sw.Close();
                file.Close();
                file.Dispose();
                sw.Dispose();
            }
        }

        /// <summary>
        /// Открытие файла с ПК
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "My files (*.LDF)|*.ldf|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                var tb = new MyTabPage();
                tb.Text = openFileDialog2.FileName.Split('\\')[^1];
                int count = 1;
                foreach (TabPage tab in Files.TabPages)
                {
                    if (tab.Text == tb.Text)
                    {
                        tb.Text = openFileDialog2.FileName.Split('\\')[^1] + $"({count})";
                        count++;
                    }
                }

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
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                var close = new ToolStripMenuItem("Закрыть");
                contextMenu.Items.Add(close);
                tb.ContextMenuStrip = contextMenu;
                contextMenu = null;
                close.Click += Close_Click;
                pan.Controls.Add(vscrol);
                pan.Controls.Add(hScroll);
                tb.Controls.Add(pan);
                Files.TabPages.Add(tb);
                Files.SelectTab(Files.TabCount - 1);
                List<string> Text;
                ClassDraw tab_to_drow;
                if (tb.Text.Contains("ldf"))
                {
                    new Thread(() =>
                    {
                        BeginInvoke(new MethodInvoker(() =>
                        {
                            Dictionary<string, ushort[]> adr = CreateFile.GetData(openFileDialog2.FileName);
                            int wh = Files.SelectedTab.Width;
                            Text = CreateFile.Load(openFileDialog2.FileName, Type.RANG).ToList();
                            Dictionary<string, string[]> teg = CreateFile.GetTegs(openFileDialog2.FileName);
                            if (teg != null)
                                tab_to_drow = new ClassDraw(ref pan, Text, ref vscrol,
                                ref hScroll, ref wh, ref adr, Height, Width, teg);
                            else
                                tab_to_drow = new ClassDraw(ref pan, Text, ref vscrol,
                            ref hScroll, ref wh, ref adr, Height, Width);
                            tab_to_drow.StartDrow();
                            mainWindows.Add(tab_to_drow);
                            MouseWheel += tab_to_drow.This_MouseWheel;
                            teg = null;
                        }
                        ));
                    })
                    {
                        Name = tb.Text,
                        IsBackground = true,
                    }.Start();
                }
                else
                {
                    Text = File.ReadAllLines(openFileDialog2.FileName, Encoding.UTF8).ToList();
                    int wh = Files.SelectedTab.Width;
                    tab_to_drow = new ClassDraw(ref pan, Text, ref vscrol,
                        ref hScroll, ref wh, Height, Width);
                    tab_to_drow.StartDrow();
                    mainWindows.Add(tab_to_drow);
                    MouseWheel += tab_to_drow.This_MouseWheel;
                }
            }
        }

        /// <summary>
        /// Подключение к устройству
        /// </summary>
        /// <param name="ip">IP адрес</param>
        /// <param name="port">Порт</param>
        /// <param name="simulate"></param>
        /// <param name="slave">ID</param>
        public void Con(string ip, string port, bool cfg)
        {
            try
            {
                if (cfg) GetInfoCFG(ip, port);

                List<string> TextRangs = new List<string>();
                client = new TcpClient(ip, int.Parse(port));
                //TcpClients.Add(client);
                master = ModbusIpMaster.CreateIp(client);

                ushort[] inputs;


                for (int j = 0; j < 100; j++)
                {
                    inputs = master.ReadHoldingRegisters(1, (ushort)(j + RangAdr), 120);
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
                string[] adreskey = Adr.Keys.ToArray();
                foreach (string adkey in adreskey)
                {
                    Adr[adkey] = master.ReadHoldingRegisters(1, MB_adres[adkey], (ushort)Adr[adkey].Length);
                }
                AdresUpdate.Enabled = true;
                var tb = new TabPage();
                tb.Text = ip + ':' + port;

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
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                var close = new ToolStripMenuItem("Закрыть");
                contextMenu.Items.Add(close);
                tb.ContextMenuStrip = contextMenu;
                contextMenu = null;
                close.Click += Close_Click;
                pan.Controls.Add(vscrol);
                pan.Controls.Add(hScroll);
                tb.Controls.Add(pan);
                Files.TabPages.Add(tb);
                Files.SelectTab(Files.TabCount - 1);

                new Thread(() =>
                {
                    BeginInvoke(new MethodInvoker(() =>
                    {
                        int wh = Files.SelectedTab.Width;
                        ClassDraw tab_to_drow = new ClassDraw(ref pan, TextRangs, ref vscrol,
                    ref hScroll, ref wh, ref Adr, Height, Width);
                        MouseWheel += tab_to_drow.This_MouseWheel;
                        tab_to_drow.StartDrow();
                        ConnectedWindows.Add(mainWindows.Count);
                        mainWindows.Add(tab_to_drow);
                        ModBusUpdate.Enabled = true;
                        AdresUpdate.Enabled = true;
                    }
                    ));
                })
                {
                    Name = tb.Text,
                    IsBackground = true,
                }.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения. Проверте подключение и повторите попытку.\n" + ex.Message);
                ConnectedWindows.Remove(mainWindows.Count - 1);
                new ConnectForms(this).Show();
            }
        }

        /// <summary>
        /// Получение данных конфигурации
        /// </summary>
        /// <param name="ip">IP адрес</param>
        /// <param name="port">Порт</param>
        private void GetInfoCFG(string ip, string port)
        {
            client = new TcpClient(ip, int.Parse(port));
            master = ModbusIpMaster.CreateIp(client);

            ushort[] inputs;
            string name;
            ushort mbadr;
            ushort len;

            Adr.Clear();
            MB_adres.Clear();

            for (int i = 0; i < 100; i++)
            {
                inputs = master.ReadHoldingRegisters(5, (ushort)(i + CfgAdr), 3);
                if (inputs[0] == 0xffff && inputs[2] == 0)
                {
                    RangAdr = inputs[1];
                    continue;
                }
                if (inputs[0] == 0) return;
                name = DataType[(byte)(inputs[0] >> 8)];
                if ((inputs[0] & 0xff) != 0) name += inputs[0] & 0xff;
                else if (name == "T_c") name = "T4_c";
                mbadr = inputs[1];
                len = inputs[2];

                Adr.Add(name, new ushort[len]);
                MB_adres.Add(name, mbadr);
            }
        }

        /// <summary>
        /// Создание пустого окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
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
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            var close = new ToolStripMenuItem("Закрыть");
            contextMenu.Items.Add(close);
            tb.ContextMenuStrip = contextMenu;
            contextMenu = null;
            close.Click += Close_Click;
            pan.Controls.Add(vscrol);
            pan.Controls.Add(hScroll);
            tb.Controls.Add(pan);
            Files.TabPages.Add(tb);
            Files.SelectTab(Files.TabCount - 1);

            new Thread(() =>
            {
                BeginInvoke(new MethodInvoker(() =>
                {
                    int wh = Files.SelectedTab.Width;
                    List<string> Text = new List<string> { "" };
                    ClassDraw tab_to_drow = new ClassDraw(ref pan, Text, ref vscrol,
                            ref hScroll, ref wh, Height, Width);
                    tab_to_drow.StartDrow();
                    mainWindows.Add(tab_to_drow);
                    MouseWheel += tab_to_drow.This_MouseWheel;
                }
                ));
            })
            {
                Name = tb.Text,
                IsBackground = true,
            }.Start();
        }

        /// <summary>
        /// Вызов окна для ввода данных при подключении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ConnectedWindows.Count < 1)
            {

                if (Application.OpenForms["ConnectForms"] == null)
                {
                    new ConnectForms(this).Show();
                }
            }
        }

        /// <summary>
        /// Вызов окна настроек
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["SettingsLogix"] == null)
            {
                new SettingsLogix(this).Show();
            }
        }

        /// <summary>
        /// Вызов окна справки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Временно ничего нет!");
        }

        /// <summary>
        /// Получение таблицы значений адресов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdresesValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Files.TabCount > 0 && Application.OpenForms[Files.SelectedTab.Text] == null)
            {
                bool flag = ConnectedWindows.Contains(Files.SelectedIndex);
                new ValueAdres(ref mainWindows[Files.SelectedIndex].GetDataTabl, Files.SelectedTab.Text, master, ref MB_adres, ref flag).Show();
            }
        }

        /// <summary>
        /// Сохранение парамметров перед завершением работы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.H = Height;
            Properties.Settings.Default.W = Width;
            Properties.Settings.Default.RangAdr = RangAdr;
            Properties.Settings.Default.CfgAdr = CfgAdr;

            Properties.Settings.Default.AdresName = string.Join(",",MB_adres.Keys);
            Properties.Settings.Default.AdresValue = string.Join(",",MB_adres.Values);
            int[] buf = new int[Adr.Count];
            for (int i = 0; i < Adr.Count; i++) buf[i] = Adr[Adr.Keys.ToArray()[i]].Length;
            Properties.Settings.Default.AdresLen = string.Join(",",string.Join(",",buf));

            Properties.Settings.Default.Save();
        }
    }
}