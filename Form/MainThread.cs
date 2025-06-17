using LogixForms.DrowClasses;
using Microsoft.VisualBasic.Devices;
using Modbus.Device;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LogixForms
{
    public partial class MainThread : Form
    {
        private static Dictionary<string, ushort[]>? Adr;
        protected static Dictionary<string, ushort>? MB_adres;
        private List<ClassDraw> mainWindows;
        private List<int> ConnectedWindows;
        public ModbusIpMaster? master;
        private TcpClient? client;
        private byte slave = 1;
        private int RangAdr;
        private int CfgAdr;
        private readonly Dictionary<byte, string> DataType;
        private bool Con_flag = false;
        private List<Thread> opens;
        private Point mousePos;
        private bool SaveFlag;
#if DEBUG
        bool DebugFlag = false;
#endif

        /// <summary>
        /// Инициализация класса
        /// </summary>
        public MainThread()
        {
            InitializeComponent();//инициализация формы

            AdresUpdate.Enabled = false;
            mousePos = new Point();
            AdresUpdate.Interval = 350;
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

            opens = new List<Thread>();

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

            mainWindows = new List<ClassDraw>();
            ConnectedWindows = new List<int>();
        }

        public Color SetColorDraw
        {
            set { toolStripTextBox1.BackColor = value; }
        }

        public int AdrUpdateTime
        {
            get { return AdresUpdate.Interval; }
            set { AdresUpdate.Interval = value; }
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
        /// Поис ошибок перед сохранением изменений в тексте ранга
        /// </summary>
        /// <param name="Text">Новый текст ранга</param>
        /// <returns>Проверенный текст ранга</returns>
        /// <exception cref="Exception">Ошибки в синтаксисе текста</exception>
        private string SerchErr(string Text)
        {
            string[] mnimonk =
            {
                "XIO",
                "XIC",
                "OTE",
                "OTL",
                "OTU",
                "ONS",
                "TON",
                "MOV",
                "ADD",
                "GEQ",
                "GRT",
                "EQU",
                "NEQ",
                "LES",
                "LEQ",
                "DIV",
                "MUL",
                "ABS",
                "SCP",
                "MSG",
                "BST",
                "NXB",
                "BND",
            };
            byte branch_start_count = 0;
            byte branch_end_count = 0;
            byte branch_next_count = 0;

            if (Text[0] != ' ' && Text[^1] != ' ') throw new Exception("Ошибка пробелов");
            foreach (string item in Text.Trim().Split(" "))
            {
                if (mnimonk.Contains(item))
                {
                    if (item == "BST") branch_start_count++;
                    if (item == "BND") branch_end_count++;
                    if (item == "NXB") branch_next_count++;
                }
                else
                {

                }
            }
            if (branch_start_count != branch_end_count) throw new Exception("Ошибка ветвей(ветвь без начала или конца)");
            if (branch_start_count > 0 && branch_next_count == 0) throw new Exception("Ошибка ветвей(ветвь без перехода)");

            return Text.ToUpper();
        }

        /// <summary>
        /// Создание нового окна
        /// </summary>
        /// <param name="Name">Имя файла</param>
        /// <param name="Text">Текст ранга</param>
        /// <param name="adr">Список адресов</param>
        /// <param name="teg">Теги и коментарии</param>
        /// <param name="tag">Полный путь к файлу, сохраняемы в tabpage в поле tag</param>
        private void CreateWin(string Name, List<string> Text, Dictionary<string, ushort[]> adr = null, Dictionary<string, string[]> teg = null, string tag = null)
        {
            var tb = new MyTabPage();
            int count = 1;
            tb.Text = Name;
            foreach (TabPage tab in Files.TabPages)
            {
                if (tab.Text == Name)
                {
                    tb.Text = Name + $"{count}";
                    count++;
                }
            }
            tb.Tag = tag;

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

            pan.MouseDoubleClick += SelectRang_DuobleClik;
            pan.MouseMove += Pan_MouseMove;
            pan.Click += Pan_Click;
            vscrol.Scroll += Vscrol_Scroll;

            pan.Controls.Add(vscrol);
            pan.Controls.Add(hScroll);
            tb.Controls.Add(pan);
            Files.TabPages.Add(tb);
            Files.SelectTab(Files.TabCount - 1);
            Thread t;
            t = new Thread(() =>
            {
                BeginInvoke(new MethodInvoker(() =>
                {
                    int wh = Files.SelectedTab.Width;
                    ClassDraw tab_to_drow;
                    if (adr == null)
                        tab_to_drow = new ClassDraw(ref pan, Text, ref vscrol,
                            ref hScroll, ref wh, Height, Width, this);
                    else
                    {
                        if (teg != null)
                            tab_to_drow = new ClassDraw(ref pan, Text, ref vscrol,
                            ref hScroll, ref wh, ref adr, Height, Width, teg, this);
                        else
                            tab_to_drow = new ClassDraw(ref pan, Text, ref vscrol,
                        ref hScroll, ref wh, ref adr, Height, Width, this);
                    }

                    tab_to_drow.StartDrow();

                    mainWindows.Add(tab_to_drow);
                    MouseWheel += tab_to_drow.This_MouseWheel;
                }
                ));
            });
            opens.Add(t);
            t.Start();
        }

        /// <summary>
        /// Подключение к устройству
        /// </summary>
        /// <param name="ip">IP адрес</param>
        /// <param name="port">Порт</param>
        /// <param name="simulate"></param>
        /// <param name="slave">ID</param>
        public void Con(string ip, string port, bool cfg, byte slave)
        {
            try
            {
                List<string> TextRangs = null;
                if (cfg) GetInfoCFG(ip, port);
                this.slave = slave;
                TextRangs = new List<string>();
                client = new TcpClient(ip, int.Parse(port));
                master = ModbusIpMaster.CreateIp(client);

                ushort[] inputs;


                for (int j = 0; j < 100; j++)
                {
                    inputs = master.ReadHoldingRegisters(slave, (ushort)(j + RangAdr), 120);
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
                    Adr[adkey] = master.ReadHoldingRegisters(slave, MB_adres[adkey], (ushort)Adr[adkey].Length);
                }
                AdresUpdate.Enabled = true;

                string name = ip + ':' + port;
                CreateWin(name, TextRangs, Adr);
                ConnectedWindows.Add(mainWindows.Count);
                ModBusUpdate.Enabled = true;
                AdresUpdate.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения. Проверте подключение и повторите попытку.\n" + ex.Message);
                ConnectedWindows.Remove(mainWindows.Count - 1);
                new ConnectForms(this).Show();
            }
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
                if (DebugFlag)
                    DebugFlag = false;
#endif
                string[]? adreskey = Adr.Keys.ToArray();
                if (adreskey == null) Debug.Print("Null");
                else
                    foreach (string adkey in adreskey)
                    {
                        if (Con_flag) toolStripTextBox2.BackColor = Color.Red;
                        else toolStripTextBox2.BackColor = Color.Yellow;
                        Task.Delay(5);
                        Adr[adkey] = master.ReadHoldingRegisters(slave, MB_adres[adkey], (ushort)Adr[adkey].Length);
                    }
                Con_flag = !Con_flag;
            }
            catch
            {
                AdresUpdate.Enabled = false;
                toolStripTextBox2.BackColor = Color.White;
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
        /// Отклюение прокрутки в фоновых окнах
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseWheelUpdate_Tick(object sender, EventArgs e)
        {
            if (mainWindows.Count > 0)
            {
                for(int count = 0; count < Files.TabPages.Count; count++)
                {
                    if (Files.TabPages[count].Text == Files.TabPages[Files.SelectedIndex].Text) mainWindows[Files.SelectedIndex].EnableScroll = true;
                    else mainWindows[count].EnableScroll = false;
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
            TabPage tab = Files.SelectedTab;
            if (tab.Text.Contains(" *"))
            {
                if (MessageBox.Show("Сохранить изменения?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Debug.WriteLine("Save");
                    SaveToolStripMenuItem_Click(sender, e);
                    if(!SaveFlag) return;
                }
            }
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
                toolStripTextBox2.BackColor = Color.White;
                ClassDraw cd = mainWindows[Files.SelectedIndex];
                mainWindows.Remove(cd);
                if (Application.OpenForms[Files.SelectedTab.Text] != null)
                    Application.OpenForms[Files.SelectedTab.Text].Close();
                TabPage tp = Files.SelectedTab;
                Files.TabPages.Remove(Files.SelectedTab);
                tp.Dispose();
                cd.Dispose();
                AdresUpdate.Enabled = false;
                ModBusUpdate.Enabled = false;
                master = null;
                Con_flag = false;
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
            SaveFlag = false;
            saveFileDialog1.InitialDirectory = @"C:\Users\PC\Desktop\";
            saveFileDialog1.RestoreDirectory = true;
            //saveFileDialog1.DefaultExt = "RangsSave";
            saveFileDialog1.FileName = Files.SelectedTab.Text.Replace(" *", "").Split('.')[0];
            saveFileDialog1.Filter = "My files (*.ldf)|*.ldf|txt files (*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (Files.SelectedTab.Text != saveFileDialog1.FileName.Split('\\')[^1]) Files.SelectedTab.Text = saveFileDialog1.FileName.Split('\\')[^1];
                Stream file = saveFileDialog1.OpenFile();
                StreamWriter sw = new StreamWriter(file);
                if (saveFileDialog1.FileName.Contains(".ldf"))
                {
                    if (mainWindows[Files.SelectedIndex].GetTegs != null)
                        sw.WriteLine(CreateFile.Create(mainWindows[Files.SelectedIndex].GetTextRang, mainWindows[Files.SelectedIndex].GetDataTabl, CreateFile.CreateTEGS(mainWindows[Files.SelectedIndex].GetTegs)));
                    else
                        sw.WriteLine(CreateFile.Create(mainWindows[Files.SelectedIndex].GetTextRang, mainWindows[Files.SelectedIndex].GetDataTabl));
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
                SaveFlag = true;
            }
            else
            {
                return;
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
                string Name = openFileDialog2.FileName.Split('\\')[^1];

                string Tag = openFileDialog2.FileName;

                List<string> Text;
                if (Tag.Contains("ldf"))
                {
                    Dictionary<string, string[]> teg = CreateFile.GetTegs(openFileDialog2.FileName);
                    Dictionary<string, ushort[]> adr = CreateFile.GetData(openFileDialog2.FileName);
                    Text = CreateFile.Load(openFileDialog2.FileName, Type.RANG).ToList();

                    CreateWin(Name, Text, adr, teg, Tag);
                }
                else
                {
                    Text = File.ReadAllLines(openFileDialog2.FileName, Encoding.UTF8).ToList();

                    CreateWin(Name, Text, tag: Tag);
                }
            }
        }

        /// <summary>
        /// Сброс выбранного ранга
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pan_Click(object? sender, EventArgs e)
        {
            (sender as MyPanel).Focus();
        }

        /// <summary>
        /// Создание пустого окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateWin("NewFile", new List<string> { "" });
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
        /// Вывод на экран текста ранга по двойному щелчку мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectRang_DuobleClik(object sender, EventArgs e)
        {
            List<StructRangsObl> obls = mainWindows[Files.SelectedIndex].rangsObls;
            Point mouse = mousePos;

            MyPanel panel = sender as MyPanel;
            VScrollBar scrollBar = panel.Controls[0] as VScrollBar;
            TextBox textBox;

            foreach (StructRangsObl obl in obls)
            {
                if (obl.Y <= mouse.Y + scrollBar.Value && obl.H >= mouse.Y + scrollBar.Value)
                {
                    Debug.WriteLine(obls.IndexOf(obl));
                    textBox = new TextBox()
                    {
                        Location = mouse,
                        Size = new Size(125, 27),
                        Name = obls.IndexOf(obl).ToString(),
                    };

                    Debug.WriteLine(obls.IndexOf(obl) + " Create");

                    textBox.TextChanged += TextBox_TextChanged;
                    textBox.Text = mainWindows[Files.SelectedIndex].GetTextRang[obls.IndexOf(obl)];
                    textBox.KeyDown += RangTextBox_KeyDown;
                    textBox.LostFocus += TextBox_LostFocus;
                    panel.Controls.Add(textBox);
                    textBox.Focus();
                }
            }
        }

        /// <summary>
        /// Сброс выбранного ранга
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Vscrol_Scroll(object? sender, ScrollEventArgs e)
        {
            (sender as VScrollBar).Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pan_MouseMove(object? sender, MouseEventArgs e)
        {
            mousePos = new Point(e.X, e.Y);
        }

        /// <summary>
        /// Закрытие окна с текстом при потери фокуса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_LostFocus(object? sender, EventArgs e)
        {
            TextBox textb = sender as TextBox;
            Control pan = textb.Controls.Owner;
            pan.Controls.Remove(textb);
            textb.Dispose();
            Debug.WriteLine(textb.Name + " Dispose");
        }

        /// <summary>
        /// Подстройка текстового оля под содержимое
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object? sender, EventArgs e)
        {
            TextBox textb = sender as TextBox;

            using (Graphics g = textb.CreateGraphics())
            {
                SizeF textSize = g.MeasureString(textb.Text, textb.Font);
                if ((int) textSize.Width >= 125)
                textb.Width = (int)textSize.Width;
            }
        }

        /// <summary>
        /// Подтверждение изменений в тексте ранга; Закрытие тестового поля 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RangTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            TextBox textb = sender as TextBox;
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    string Text = SerchErr(textb.Text.ToUpper());
                    mainWindows[Files.SelectedIndex].SetNewTextRang(int.Parse(textb.Name), Text);
                    textb.Dispose();
                    TabPage tab = Files.SelectedTab;
                    tab.Text += " *";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (e.KeyCode == Keys.Escape)
            {
                textb.Dispose();
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

            Properties.Settings.Default.AdresName = string.Join(",", MB_adres.Keys);
            Properties.Settings.Default.AdresValue = string.Join(",", MB_adres.Values);
            int[] buf = new int[Adr.Count];
            for (int i = 0; i < Adr.Count; i++) buf[i] = Adr[Adr.Keys.ToArray()[i]].Length;
            Properties.Settings.Default.AdresLen = string.Join(",", string.Join(",", buf));

            Properties.Settings.Default.Save();
        }
    }
}