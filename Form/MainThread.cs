using LogixForms.HelperClasses;
using LogixForms.HelperClasses.DrowClasses;
using Modbus.Device;
using Modbus.Extensions.Enron;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Net.Sockets;
using System.Runtime.ConstrainedExecution;
using System.Text;
using TextBox = System.Windows.Forms.TextBox;

namespace LogixForms
{
    public partial class MainThread : Form
    {
        private static Dictionary<string, ushort[]>? Adr;
        protected static Dictionary<string, ushort>? MB_adres;
        private List<string> commands = [""];
        private byte number_com = 0;
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
        private CheckingTheCorrectness correctness;
        private string CopyCutBuffer;

#if DEBUG
        bool DebugFlag = false;
#endif

        /// <summary>
        /// Инициализация класса
        /// </summary>
        public MainThread()
        {
            InitializeComponent();//инициализация формы

            correctness = new(100, 100, 100, CheckingType.Local);
            CopyCutBuffer = string.Empty;
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

        /// <summary>
        /// Эмпровезированная лампочка, которая моргает
        /// </summary>
        public Color SetColorDraw
        {
            set { toolStripTextBox1.BackColor = value; }
        }

        /// <summary>
        /// Установка времени опроса устройства
        /// </summary>
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
            if (Text[0] != ' ' && Text[^1] != ' ')
            {
                MessageBox.Show("Код ошибки 20.\nОшибка пробелов", "Ошибка правельноси ранга!", MessageBoxButtons.OK);
                throw new();
            }

            sbyte result = correctness.CheckRangText(Text);

            if (result != 0)
            {
                string mess = ErrorMessenger((ushort)result);
                MessageBox.Show(mess, "Ошибка правельноси ранга!", MessageBoxButtons.OK);
                throw new();
            }
            else
                return Text.ToUpper();
        }

        /// <summary>
        /// Создание нового окна
        /// </summary>
        /// <param name="Name">Имя файла</param>
        /// <param name="Text">Текст ранга</param>
        /// <param name="adr">Список адресов</param>
        /// <param name="teg">Теги и коментарии</param>
        /// <param name="tag">Полный путь к файлу, сохраняемы в tabpage в поле tag; для нлайн режима указывать "Online"</param>
        /// <param name="tabType">Тип испольуемого источника(локально из файла или в онлайн режиме)</param>
        private void CreateWin(string Name, List<string> Text, Dictionary<string, ushort[]> adr = null,
            Dictionary<string, string[]> teg = null, string tag = null, TabPageType tabType = TabPageType.Local)
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

            if (tabType == TabPageType.Online)
                tb.Tag = "Online";
            else
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
                Tag = tabType == TabPageType.Online ? "Online" : "",
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
                    Files.SelectTab(Files.TabCount - 1);
                }
                ));
            });
            opens.Add(t);
            t.Start();
        }

        private void TabPag_SelectedIndex(object? sender, EventArgs e)
        {
            if (Files.TabPages.Count < 1) return;
            ClassDraw selectTab = mainWindows[Files.SelectedIndex];

            if (selectTab.CanselCount > 0)
                button_cansel.Enabled = true;
            else
                button_cansel.Enabled = false;

            if (selectTab.UndoCount > 0)
                button_undo.Enabled = true;
            else
                button_undo.Enabled = false;
        }

        /// <summary>
        /// Создание нового окна
        /// </summary>
        /// <param name="Name">Отображаемое имя</param>
        /// <param name="Tag">Полное имя(для файла - путь)</param>
        private void CreateWinWhishOpen(string Name, string Tag)
        {
            List<string> Text;
            if (Tag.Contains("ldf"))
            {
                Dictionary<string, string[]> teg = CreateFile.GetTegs(Tag);
                Dictionary<string, ushort[]> adr = CreateFile.GetData(Tag);
                Text = CreateFile.Load(Tag, HelperClasses.Type.RANG).ToList();

                CreateWin(Name, Text, adr, teg, Tag);
            }
            else
            {
                Text = File.ReadAllLines(Tag, Encoding.UTF8).ToList();

                CreateWin(Name, Text, tag: Tag);
            }
        }

        /// <summary>
        /// Чтение ModBus регистров: текст рангов, значение адресов
        /// </summary>
        /// <returns>Список строк рангов</returns>
        private List<string> ReadMBRegisters()
        {
            ushort[] inputs;

            List<string> TextRangs = new List<string>();

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

            return TextRangs;
        }

        private string FindDifferences(int NumberRung, string OldString, string NewString)
        {
            //формат: #номер ранга&индекс начала отличия&отличающаяя cтрока^&индекс начала отличия&отличающаяя cтрока... и т.д
            //#12^&4&XIC N15/3:12^&14&5

            string result = NumberRung.ToString() + "#";

            if (OldString.Trim() == "" && NewString.Trim() != "")
                return result + "^&0&" + NewString;
            if (NewString.Trim() == "")
                return result + "^&0&";

            // надо придумать алгоритм по поиску различных чстй в строках

            return result;
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
                for (int count = 0; count < Files.TabPages.Count; count++)
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
            if (tab.Text.Contains("*"))
            {
                if (MessageBox.Show("В программу были внесеы изменения.\nСохранить изменения?", "Сохаение изменений", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Debug.WriteLine("Save");
                    SaveToolStripMenuItem_Click(sender, e);
                    if (!SaveFlag) return;
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

                if (Files.TabPages.Count == 0)
                {
                    button_accept.Enabled = false;
                    button_upload.Enabled = false;
                    button_cansel.Enabled = false;
                    button_undo.Enabled = false;
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
        /// Сохранение программмы на ПК
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Files.TabPages.Count <= 0) return;

            string[] newtext = null;
            if ((Files.SelectedTab.Tag as string) == "Online")
            {
                newtext = new string[mainWindows[Files.SelectedIndex].GetTextRang.Length];
                for (int i = 0; i < mainWindows[Files.SelectedIndex].GetTextRang.Length; i++)
                {
                    string str = mainWindows[Files.SelectedIndex].GetTextRang[i];
                    if (str.Contains("#"))
                    {
                        mainWindows[Files.SelectedIndex].SetNewTextRang(i, str.Split("#")[0]);
                        newtext[i] = str.Split("#")[^1];
                    }
                    else
                        newtext[i] = str;

                }
            }

            SaveFlag = false;
            saveFileDialog1.InitialDirectory = @"C:\Users\PC\Desktop\";
            saveFileDialog1.RestoreDirectory = true;
            //saveFileDialog1.DefaultExt = "RangsSave";
            saveFileDialog1.FileName = Files.SelectedTab.Text.Replace(" *", "").Split('.')[0];
            saveFileDialog1.Filter = "My files (*.ldf)|*.ldf|txt files (*.txt)|*.txt";
            //saveFileDialog1.ShowDialog();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (Files.SelectedTab.Text != saveFileDialog1.FileName.Split('\\')[^1] &&
                    (Files.SelectedTab.Tag as string) != "Online") Files.SelectedTab.Text = saveFileDialog1.FileName.Split('\\')[^1];
                else
                    Files.SelectedTab.Text = Files.SelectedTab.Text.Replace("*", "");
                Stream file = saveFileDialog1.OpenFile();
                StreamWriter sw = new StreamWriter(file);
                if (saveFileDialog1.FileName.Contains(".ldf"))
                {
                    if (mainWindows[Files.SelectedIndex].GetTegs != null)
                        sw.WriteLine(CreateFile.Create(newtext.Equals(null) ? mainWindows[Files.SelectedIndex].GetTextRang : newtext, mainWindows[Files.SelectedIndex].GetDataTabl, CreateFile.CreateTEGS(mainWindows[Files.SelectedIndex].GetTegs)));
                    else
                        sw.WriteLine(CreateFile.Create(newtext.Equals(null) ? mainWindows[Files.SelectedIndex].GetTextRang : newtext, mainWindows[Files.SelectedIndex].GetDataTabl));
                    sw.Close();
                    file.Close();
                    if ((Files.SelectedTab.Tag as string) != "Online") return;
                }
                else
                {
                    foreach (string rang in newtext.Equals(null) ? mainWindows[Files.SelectedIndex].GetTextRang : newtext)
                        sw.WriteLine(rang);
                    sw.Close();
                    file.Close();
                    file.Dispose();
                    sw.Dispose();
                    SaveFlag = true;
                }

                if ((Files.SelectedTab.Tag as string) == "Online")
                    CreateWinWhishOpen(saveFileDialog1.FileName.Split('\\')[^1], saveFileDialog1.FileName);
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
                CreateWinWhishOpen(Name, Tag);
            }
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
                if (cfg) GetInfoCFG(ip, port);
                this.slave = slave;
                client = new TcpClient(ip, int.Parse(port));
                master = ModbusIpMaster.CreateIp(client);

                List<string> TextRangs = ReadMBRegisters();
                AdresUpdate.Enabled = true;

                string name = ip + ':' + port;
                CreateWin(name, TextRangs, Adr, tabType: TabPageType.Online);
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
                new ValueAdres(ref mainWindows[Files.SelectedIndex].GetDataTabl, Files.SelectedTab.Text, master, ref MB_adres, ref flag, slave).Show();
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
                        Tag = panel.Tag == "Online" ? panel.Tag : "",
                    };

                    Debug.WriteLine(obls.IndexOf(obl) + " Create");

                    textBox.TextChanged += TextBox_TextChanged;
                    textBox.Text = mainWindows[Files.SelectedIndex].GetTextRang[obls.IndexOf(obl)]
                        .Split("#")[^1];
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
        /// Отслеживание позиции курсора мыши
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
                if ((int)textSize.Width >= 125)
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

                    ClassDraw selectTab = mainWindows[Files.SelectedIndex];
                    string oldtext = selectTab.GetTextRang[int.Parse(textb.Name)];

                    if (oldtext.Contains("#"))
                        selectTab.CanselPush = textb.Name + "#" + oldtext.Split("#")[1];
                    else
                        selectTab.CanselPush = textb.Name + "#" + oldtext;

                    if (textb.Tag == "Online")
                    {
                        selectTab.SetNewTextRangInOnlineMode(int.Parse(textb.Name), Text);

                        button_upload.Enabled = true;
                    }
                    else
                        selectTab.SetNewTextRang(int.Parse(textb.Name), Text);

                    textb.Dispose();
                    TabPage tab = Files.SelectedTab;
                    tab.Text += (tab.Text.Contains("*") ? "" : "*");

                    button_cansel.Enabled = true;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Код ошибки"))
                    {
                        MessageBox.Show(ex.Message, "Ошибка правельноси ранга!", MessageBoxButtons.OK);
                        textb.Focus();
    
                    }
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

        /// <summary>
        /// Отправка сообщения о сохранении изменений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_accept_Click(object sender, EventArgs e)
        {
            master.WriteMultipleRegistersAsync(slave, 8200, [54321]);

            Thread.Sleep(100);

            mainWindows[Files.SelectedIndex].SetNewAllTextRang(ReadMBRegisters());

            button_accept.Enabled = false;
        }

        /// <summary>
        /// Отправка нового текста ранга на устройство
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_upload_Click(object sender, EventArgs e)
        {
            int first = -1;
            int count_sharp = 0;
            string[] rangs = mainWindows[Files.SelectedIndex].GetTextRang;

            for (int i = 0; i < rangs.Length; i++)
            {
                if (rangs[i].Contains("#"))
                {
                    if (first == -1)
                        first = i;
                    count_sharp++;
                }
            }

            string line = $"%{first} {rangs[first].Split("#")[^1].Trim()} ";
            List<ushort> rang = new List<ushort>();
            int count;
            ushort temp;
            count = 0;
            temp = 0;
            for (int i = 0; i < 240;)
            {
                if (line[i] != 0 && i < line.Length)
                {
                    temp = line[i];
                    i++;
                }
                if (i < line.Length)
                {
                    temp |= (ushort)(line[i] << 8);
                    i++;
                    rang.Add(temp);
                    count++;
                }
                if (i == line.Length) break;
            }

            master.WriteMultipleRegistersAsync(slave, 8400, rang.ToArray());

            Thread.Sleep(25);
            ushort result = master.ReadHoldingRegisters(slave, 8600, 1)[0];
            if (result == 0)
            {
                if (count_sharp <= 1)
                {
                    button_upload.Enabled = false;
                }
                button_accept.Enabled = true;
                count_sharp--;

                return;
            }
            ErrorMessenger(result);
        }

        /// <summary>
        /// Оповещение об ошибках
        /// </summary>
        /// <param name="ErrorCode">Код ошибки</param>
        private static string ErrorMessenger(ushort ErrorCode)
        {
            string error_coment = string.Empty;
            switch (ErrorCode)
            {
                case 1:
                    error_coment = "Код ошибки 1.\nКоличество концов должно быть равным количеству начал ветвей.";
                    break;
                case 2:
                    error_coment = "Код ошибки 2.\nКоличество ONS должен быть только 1 или 0.";
                    break;
                case 3:
                    error_coment = "Код ошибки 3.\nПревышение размерности группы параметров (не правильная адресация).";
                    break;
                case 4:
                    error_coment = "Код ошибки 4.\nКоличество битов должно быть 0-15 (не правильная адресация).";
                    break;
                case 5:
                    error_coment = "Код ошибки 5.\nНет такой группы параметров (не правильная адресация).";
                    break;
                case 6:
                    error_coment = "Код ошибки 6.\nРезультат не должен быть числом.";
                    break;
                case 8:
                    error_coment = "Код ошибки 8.\nНе все слова в строке прошли проверку (не правильное написание ключевых слов или адресов).";
                    break;
                case 9:
                    error_coment = "Код ошибки 9.\nНеправильный TimeBase в таймере.";
                    break;
                case 10:
                    error_coment = "Код ошибки 10.\nPreset не может быть нулевым в таймере.";
                    break;
                case 11:
                    error_coment = "Код ошибки 11.\nПервый символ перед номером ранга должен быть \'%\'"; 

                    break;
                case 12:
                    error_coment = "Код ошибки 12.\nНеправильный код операции в MSG.";
                        
                    break;
                case 13:
                    error_coment = "Код ошибки 13.\nНеправильный второстепенные параметры в MSG.";
                        
                    break;
                case 14:
                    error_coment = "Код ошибки 14.\nНеправильный синтакс IP адреса в MSG.";
                       ;
                    break;
                case 15:
                    error_coment = "Код ошибки 15.\nНеправильный MB адрес в MSG.";
                        
                    break;
                case 16:
                    error_coment = "Код ошибки 16.\nНеправильный колл. регистров для обмена в MSG.";
                        
                    break;
                case 17:
                    error_coment = "Код ошибки 17.\nНеправильный TimeOut MTO  в MSG.";
                        
                    break;
                case 18:
                    error_coment = "Код ошибки 18.\nНеправильный NOD (ID) в MSG.";
                        
                    break;
                case 19:
                    error_coment = "Код ошибки 19.\nНеправильный Port в MSG.";
                        
                    break;
                default:
                    error_coment = $"Код ошибки {ErrorCode}.\nНеизвестная ошибка.";
                        
                    break;
            }

            return error_coment;
        }

        /// <summary>
        /// Обработка нажатия на кнопку отмены
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_cansel_Click(object sender, EventArgs e)
        {
            if (Files.TabPages.Count < 1) return;
            ClassDraw selectTab = mainWindows[Files.SelectedIndex];

            if (selectTab.CanselCount == 0) return;

            string buf = selectTab.CanselPop;

            if (buf.Equals(string.Empty)) return;

            int num_rang = int.Parse(buf.Split("#")[0]);
            string oldrang = buf.Split("#")[1];

            if (buf.Split("#").Length != 3)
            {
                if (num_rang >= selectTab.GetTextRang.Length)
                    buf = " ";
                else
                    buf = selectTab.GetTextRang[num_rang];

                if (buf.Contains("#"))
                    buf = buf.Split("#")[1];

                selectTab.UndoPush = num_rang + "#" + buf;

                if (Files.SelectedTab.Tag == "Online")
                {
                    if (selectTab.SetNewTextRangInOnlineMode(num_rang, oldrang) == 1)
                        button_upload.Enabled = false;
                    else
                        button_upload.Enabled = true;
                }
                else
                {
                    selectTab.SetNewTextRang(num_rang, oldrang);
                }
            }
            else if (buf.Split("#")[^1] == "deleted")
            {
                if (num_rang >= selectTab.GetTextRang.Length)
                    buf = " ";
                else
                    buf = selectTab.GetTextRang[num_rang];

                if (buf.Contains("#"))
                    buf = buf.Split("#")[1];

                selectTab.UndoPush = num_rang + "#" + buf + "#deleted";

                if (Files.SelectedTab.Tag == "Online")
                {
                    if (selectTab.AddNewTextRangInOnlineMode(oldrang, num_rang) == 1)
                        button_upload.Enabled = false;
                    else
                        button_upload.Enabled = true;
                }
                else
                {
                    selectTab.AddNewTextRang(oldrang, num_rang);
                }
            }
            else if(buf.Split("#")[^1] == "add")
            {
                if (buf.Contains("#"))
                    buf = buf.Split("#")[1];

                selectTab.UndoPush = num_rang + "#" + buf + "#add";
                selectTab.DeleteRang(num_rang);
            }

            if (selectTab.CanselCount < 1)
                button_cansel.Enabled = false;

            if (selectTab.UndoCount >= 1)
                button_undo.Enabled = true;
        }

        /// <summary>
        /// Обработка нажатия на кнопку возврата
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_undo_Click(object sender, EventArgs e)
        {
            if (Files.TabPages.Count < 1) return;

            ClassDraw selectTab = mainWindows[Files.SelectedIndex];

            if (selectTab.UndoCount == 0) return;

            string buf = selectTab.UndoPop;

            if (buf.Equals(string.Empty)) return;

            int num_rang = int.Parse(buf.Split("#")[0]);
            string oldrang = buf.Split("#")[1];
            //ffffffff
            if (buf.Split("#").Length != 3)
            {
                if (num_rang >= selectTab.GetTextRang.Length)
                    buf = " ";
                else
                    buf = selectTab.GetTextRang[num_rang];

                if (buf.Contains("#"))
                    buf = buf.Split("#")[1];

                selectTab.CanselPush = num_rang + "#" + buf + "#deleted";

                if (Files.SelectedTab.Tag == "Online")
                {
                    if (selectTab.SetNewTextRangInOnlineMode(num_rang, oldrang) == 1)
                        button_upload.Enabled = false;
                    else
                        button_upload.Enabled = true;
                }
                else
                {
                    selectTab.SetNewTextRang(num_rang, oldrang);
                }
            }
            else if (buf.Split("#")[^1] == "deleted")
            {
                if(num_rang >= selectTab.GetTextRang.Length)
                    buf = " ";
                else
                    buf = selectTab.GetTextRang[num_rang];

                if (buf.Contains("#"))
                    buf = buf.Split("#")[1];

                selectTab.CanselPush = num_rang + "#" + buf + "#deleted";

                if (Files.SelectedTab.Tag == "Online")
                {
                    if (selectTab.AddNewTextRangInOnlineMode(oldrang, num_rang) == 1)  //replace to delete
                        button_upload.Enabled = false;
                    else
                        button_upload.Enabled = true;
                }
                else
                {
                    selectTab.DeleteRang(num_rang);
                }
            }
            else if(buf.Split("#")[^1] == "add")
            {
                if (buf.Contains("#"))
                    buf = buf.Split("#")[1];

                selectTab.CanselPush = num_rang + "#" + buf + "#add";

                if (Files.SelectedTab.Tag == "Online")
                {
                    if (selectTab.AddNewTextRangInOnlineMode(oldrang, num_rang) == 1)
                        button_upload.Enabled = false;
                    else
                        button_upload.Enabled = true;
                }
                else
                {
                    selectTab.AddNewTextRang(oldrang, num_rang);
                }
            }

            if (selectTab.UndoCount < 1)
                button_undo.Enabled = false;

            if (selectTab.CanselCount >= 1)
                button_cansel.Enabled = true;
        }

        /// <summary>
        /// Отследивание нажатий клавиш в области Files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyTabPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.U && e.Modifiers == Keys.Control)
                button_undo_Click(sender, e);
            else if (e.KeyCode == Keys.Z && e.Modifiers == Keys.Control)
                button_cansel_Click(sender, e);
        }

        /// <summary>
        /// Затимнене при выключении элемента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_cansel_EnabledChanged(object sender, EventArgs e)
        {
            if (button_cansel.Enabled)
                button_cansel.ForeColor = SystemColors.ActiveCaptionText;
            else
                button_cansel.ForeColor = SystemColors.AppWorkspace;
        }

        /// <summary>
        /// Обработка нажатия на кнопку отмены
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_undo_EnabledChanged(object sender, EventArgs e)
        {
            if (button_undo.Enabled)
                button_undo.ForeColor = SystemColors.ActiveCaptionText;
            else
                button_undo.ForeColor = SystemColors.AppWorkspace;
        }

        private void Console_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //textBox1.AppendText(Console_textBox.Text);
                textBox1.AppendText(Console_textBox.Text + "\r\n");
                commands.Add(Console_textBox.Text);

                if (Console_textBox.Text[0] == '/')
                    ConsoleComand(Console_textBox.Text);
                else
                    ConsoleFunction(Console_textBox.Text);


                    Console_textBox.Text = "";
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (number_com >= 1)
                {
                    //Down.Push(Console_textBox.Text);
                    number_com--;
                    Console_textBox.Text = commands[number_com];
                    Console_textBox.SelectionStart = commands[number_com].Length;
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (number_com < commands.Count-1)
                {
                    //Up.Push(Console_textBox.Text);
                    number_com++;
                    Console_textBox.Text = commands[number_com];
                    Console_textBox.SelectionStart = commands[number_com].Length;
                }
            }
        }

        private void ConsoleComand(string Comand)
        {
            string[] args = Comand.Split(" ");
            ClassDraw selectTab = null;
            if (Files.TabPages.Count > 0)
                selectTab = mainWindows[Files.SelectedIndex]; // НАДО ДОДЕЛАТЬ ClassDraw, ПОСКОЛЬКУ НЕ ВСЕ РЕШЕНИЯ ЕСТЬ ДЛЯ ОНЛАЙН РЕЖИМА
            switch (args[0])
            {
                case "/getrang":
                    byte num_rang;
                    if (!byte.TryParse(args[1], out num_rang))
                    {
                        textBox1.AppendText("Ошибка аргумента.\r\n");
                        break;
                    }

                    if (mainWindows.Count > 0 && Files.TabPages.Count > 0)
                    {
                        if (num_rang < mainWindows[Files.SelectedIndex].GetTextRang.Length)
                            textBox1.AppendText(args[1] + ": " + mainWindows[Files.SelectedIndex].GetTextRang[num_rang] + "\r\n");
                    }
                    else
                        textBox1.AppendText("Ошибка! Нет открытых источников.\r\n");
                    
                    break;//done
                case "/setbit":
                    if (Files.TabPages.Count <= 0)
                    {
                        textBox1.AppendText("Ошибка! Нет открытых источников.\r\n");
                        break;
                    }

                    string[] adr_key = mainWindows[Files.SelectedIndex].GetDataTabl.Keys.ToArray();
                    if (!adr_key.Contains(args[1].Split(":")[0]))
                    {
                        textBox1.AppendText("Неверно указан адрес.\r\n");
                        break;
                    }
                    byte bit;
                    if (!byte.TryParse(args[2], out bit))
                    {
                        textBox1.AppendText("Неверно указан бит.\r\n");
                        break;
                    }
                    if (bit > 1 && bit < 0)
                    {
                        textBox1.AppendText("Неверно указан бит.\r\n");
                        break;
                    }

                    string[] mas_bit = args[1].Split(":")[1].Split('/');

                    byte adres;
                    if (!byte.TryParse(mas_bit[0], out adres))
                    {
                        textBox1.AppendText("Неверно указан адрес.\r\n");
                        break;
                    }
                    ushort adr = mainWindows[Files.SelectedIndex].GetDataTabl[args[1].Split(":")[0]][adres];

                    if (!byte.TryParse(mas_bit[1], out adres))
                    {
                        textBox1.AppendText("Неверно указан адрес.\r\n");
                        break;
                    }

                    if(adres > 15 || adres < 0)
                    {
                        textBox1.AppendText("Неверно указан бит.\r\n");
                        break;
                    }

                    if ((adr & 1 << adres) == 1 << adres)
                    {
                        if (bit == 0)
                            adr = (ushort)(adr ^ (1 << adres));
                    }
                    else
                    {
                        if (bit == 1)
                            adr = (ushort)(adr | (1 << adres));
                    }

                    adres = byte.Parse(mas_bit[0]);
                    mainWindows[Files.SelectedIndex].GetDataTabl[args[1].Split(":")[0]][adres] = adr;

                    if (Files.SelectedTab.Tag == "Online")
                    {
                        master.WriteSingleRegister(slave, (ushort)(MB_adres[args[1].Split(":")[0]] + adres), mainWindows[Files.SelectedIndex].GetDataTabl[args[1].Split(":")[0]][adres]);
                    }

                    break;//done
                case "/setrang":
                    if (Files.TabPages.Count <= 0)
                    {
                        textBox1.AppendText("Ошибка! Нет открытых источников.\r\n");
                        break;
                    }

                    byte num;
                    if (!byte.TryParse(args[1], out num))
                    {
                        textBox1.AppendText("Ошибка аргумента.\r\n");
                        break;
                    }

                    if (num > mainWindows[Files.SelectedIndex].GetTextRang.Length)
                    {
                        textBox1.AppendText("Ошибка аргумента.\r\n");
                        break;
                    }

                    try
                    {
                        string Text = SerchErr(Comand.Split("\"")[1].ToUpper());

                        string oldtext = selectTab.GetTextRang[num];

                        if (oldtext.Contains("#"))
                            selectTab.CanselPush = num + "#" + oldtext.Split("#")[1];
                        else
                            selectTab.CanselPush = num + "#" + oldtext;

                        if (Files.SelectedTab.Tag == "Online")
                        {
                            selectTab.SetNewTextRangInOnlineMode(num, Text);

                            button_upload.Enabled = true;
                        }
                        else
                            selectTab.SetNewTextRang(num, Text);

                        TabPage tab = Files.SelectedTab;
                        tab.Text += (tab.Text.Contains("*") ? "" : "*");

                        button_cansel.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Код ошибки"))
                        {
                            textBox1.AppendText(ex.Message + "\r\nОшибка правельноси ранга!\r\n");
                        }
                    }

                    break;//done
                case "/addrang":
                    if (Files.TabPages.Count <= 0)
                    {
                        textBox1.AppendText("Ошибка! Нет открытых источников.\r\n");
                        break;
                    }

                    try
                    {
                        if (Comand != "/addrang")
                        {
                            string Text = SerchErr(Comand.Split("\"")[1].ToUpper());

                            if (sbyte.TryParse(args[1], out sbyte snum))
                            {
                                if (snum < 0)
                                {
                                    textBox1.AppendText("Ошибка аргумента.\r\n");
                                    break;
                                }

                                selectTab.CanselPush = snum + "#" + Text + "#add";
                                if (Files.SelectedTab.Tag == "Online")
                                {
                                    selectTab.AddNewTextRangInOnlineMode(Text, snum);

                                    button_upload.Enabled = true;
                                }
                                else
                                    selectTab.AddNewTextRang(Text, snum);

                                break;
                            }

                            selectTab.CanselPush = selectTab.GetTextRang.Length + "#" + Text + "#add";
                            if (Files.SelectedTab.Tag == "Online")
                            {
                                selectTab.AddNewTextRangInOnlineMode(Text);

                                button_upload.Enabled = true;
                            }
                            else
                                selectTab.AddNewTextRang(Text);
                        }
                        else
                        {
                            selectTab.CanselPush = selectTab.GetTextRang.Length + "##add";
                            if (Files.SelectedTab.Tag == "Online")
                            {
                                selectTab.AddNewTextRangInOnlineMode("");

                                button_upload.Enabled = true;
                            }
                            else
                                selectTab.AddNewTextRang("");
                        }

                        TabPage tab = Files.SelectedTab;
                        tab.Text += (tab.Text.Contains("*") ? "" : "*");

                        button_cansel.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Код ошибки"))
                        {
                            textBox1.AppendText(ex.Message + "\r\nОшибка правельноси ранга!\r\n");
                        }
                    }
                    break;//done
                case "/deleterang":
                    if (Files.TabPages.Count <= 0)
                    {
                        textBox1.AppendText("Ошибка! Нет открытых источников.\r\n");
                        break;
                    }

                    if (!byte.TryParse(args[1], out num))
                    {
                        textBox1.AppendText("Неверно указан бит.\r\n");
                        break;
                    }

                    if (num > selectTab.GetTextRang.Length)
                    {
                        textBox1.AppendText("Неверно указан бит.\r\n");
                        break;
                    }

                    selectTab.CanselPush = num + "#" +selectTab.GetTextRang[num] + "#deleted";

                    selectTab.DeleteRang(num);

                    button_cansel.Enabled = true;
                    break;//done
                case "/copy":
                    if (Files.TabPages.Count <= 0)
                    {
                        textBox1.AppendText("Ошибка! Нет открытых источников.\r\n");
                        break;
                    }

                    if (!byte.TryParse(args[1], out num))
                    {
                        textBox1.AppendText("Ошибка аргумента.\r\n");
                        break;
                    }

                    CopyCutBuffer = selectTab.GetTextRang[num];
                    textBox1.AppendText($"Скопированно: {CopyCutBuffer}\r\n");

                    break;//done
                case "/paste":
                    if (Files.TabPages.Count <= 0)
                    {
                        textBox1.AppendText("Ошибка! Нет открытых источников.\r\n");
                        break;
                    }

                    if (!byte.TryParse(args[1], out num))
                    {
                        textBox1.AppendText("Ошибка аргумента.\r\n");
                        break;
                    }

                    if(!string.IsNullOrEmpty(CopyCutBuffer))
                        ConsoleComand($"/addrang {num} \"{CopyCutBuffer}\"");
                    textBox1.AppendText($"Вставлено: {CopyCutBuffer}\r\n");

                    break;//done
                case "/cut":
                    if (Files.TabPages.Count <= 0)
                    {
                        textBox1.AppendText("Ошибка! Нет открытых источников.\r\n");
                        break;
                    }

                    if (!byte.TryParse(args[1], out num))
                    {
                        textBox1.AppendText("Ошибка аргумента.\r\n");
                        break;
                    }

                    CopyCutBuffer = selectTab.GetTextRang[num];
                    ConsoleComand($"/deleterang {num}");
                    textBox1.AppendText($"Вырезано: {CopyCutBuffer}\r\n");

                    break;//done
                case "/push":
                    if (Files.TabPages.Count <= 0)
                    {
                        textBox1.AppendText("Ошибка! Нет открытых источников.\r\n");
                        break;
                    }
                    if (Files.SelectedTab.Tag != "Online")
                    {
                        textBox1.AppendText("Ошибка! Данная команда работает только в онлайн режиме.\r\n");
                        break;
                    }

                    if (button_upload.Enabled)
                        button_upload_Click(new(), new EventArgs());

                    break;//done
                case "/sendsave":
                    if (Files.TabPages.Count <= 0)
                    {
                        textBox1.AppendText("Ошибка! Нет открытых источников.\r\n");
                        break;
                    }
                    if (Files.SelectedTab.Tag != "Online")
                    {
                        textBox1.AppendText("Ошибка! Данная команда работает только в онлайн режиме.\r\n");
                        break;
                    }

                    if (button_accept.Enabled)
                        button_accept_Click(new(), new EventArgs());

                    break;//done
                case "/open":
                    if(args.Length > 1)
                    {
                        try
                        {
                            string path = Comand.Replace("/open ", "");

                            if (path.Contains("\""))
                                path = path.Split("\"")[1];

                            string Name = path.Split('\\')[^1];

                            string Tag = path;
                            CreateWinWhishOpen(Name, Tag);
                        }
                        catch
                        {
                            textBox1.AppendText("Ошибка пути. Проверьте правильность пути или наличие файла.\r\n");
                            break;
                        }
                    }
                    else
                    {
                        OpenToolStripMenuItem_Click(new(), new EventArgs());
                    }
                    break;//done
                case "/save":
                    if (Files.TabPages.Count <= 0)
                    {
                        textBox1.AppendText("Ошибка! Нет открытых источников.\r\n");
                        break;
                    }

                    SaveToolStripMenuItem_Click(new(), new EventArgs());

                    break;//done
                case "/new":
                    NewToolStripMenuItem_Click(new(), new EventArgs());
                    break;//done
                case "/connect":
                    if (ConnectedWindows.Count >= 1) break;

                    if (args.Length > 1)
                    {
                        string ip_and_port = args[1].Split(":")[0];
                        byte s;
                        try
                        {
                            string[] ip_buf = { };
                            ip_buf = ip_and_port.Split('.');
                            if (ip_buf.Length == 4 && byte.TryParse(args[2], out s))
                            {
                                foreach (string ip in ip_buf)
                                {
                                    if (!int.TryParse(ip, out _)) continue;
                                }

                            }
                            else
                            {
                                if (ip_buf.Length != 4)
                                {
                                    textBox1.AppendText("Не верно указан IP адрес: 255.255.255.255");
                                }
                                else
                                {
                                    textBox1.AppendText("Не верно указан SlaveID");
                                }
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            textBox1.AppendText("Не верно указан IP адрес: встречено не число");
                            break;
                        }
                        if (s != null && s != 0)
                        {
                            Con(ip_and_port, args[1].Split(":")[1], false, s);
                        }
                        else
                        {
                            textBox1.AppendText("Не вышло!");
                            break;
                        }
                    }
                    else
                        ConnectToolStripMenuItem_Click(new(), new EventArgs());

                    break;//done
                case "/clear":
                    textBox1.Text = "";
                    break;//done
                case "/cansel":
                    if (Files.TabPages.Count <= 0)
                    {
                        textBox1.AppendText("Ошибка! Нет открытых источников.\r\n");
                        break;
                    }
                    if(button_cansel.Enabled)
                        button_cansel_Click(new(), new EventArgs());
                    break;//done
                case "/undo":
                    if (Files.TabPages.Count <= 0)
                    {
                        textBox1.AppendText("Ошибка! Нет открытых источников.\r\n");
                        break;
                    }
                    if(button_undo.Enabled)
                        button_undo_Click(new(), new EventArgs());
                    break;//done
                case "/close":
                    if (Files.TabPages.Count <= 0)
                    {
                        textBox1.AppendText("Ошибка! Нет открытых источников.\r\n");
                        break;
                    }
                    Close_Click(new(), new EventArgs());
                    break;//done
                case "/exit":
                    Application.Exit();
                    break;//done
                case "/help":
                    textBox1.AppendText("Список доступных команд:\r\n");
                    textBox1.AppendText("\t/getrang [номер ранга от 0] - вывод текста указанного ранга.\r\n" );
                    textBox1.AppendText("\t/setbit [адрес] <0|1> - установление значение бита.\r\n");
                    textBox1.AppendText("\t/setrang [номер ранга от 0] \"<Текст ранга>\" - установление значение бита.\r\n");
                        textBox1.AppendText("\t/addrang - оlбавление пустого ранга в конец.\r\n");
                        textBox1.AppendText("\t/addrang \"<Текст ранга>\" - lобавление ранга в конец.\r\n");
                        textBox1.AppendText("\t/addrang [номер ранга с 0] \"<Текст ранга>\" - lобавление ранга на указанную позицию.\r\n");
                    textBox1.AppendText("\t/copy [номер ранга с 0] - копирование ранга из указанной позиции.\r\n");
                    textBox1.AppendText("\t/cut [номер ранга с 0] - вырезать ранг из указанной позиции.\r\n");
                    textBox1.AppendText("\t/paste [номер ранга с 0] - вставка ранга на указанную позицию.\r\n");

                    textBox1.AppendText("\t/open {полный путь к файлу} - открытие файла программы.\r\n");
                    textBox1.AppendText("\t/connect {<ip:port> <slaveID>} - подключение к устройству.\r\n");
                    textBox1.AppendText("\t/push - отправка первого найденного изменения на устройство.\r\n");
                    textBox1.AppendText("\t/sendsave - записть изменения на устройстве.\r\n");
                    textBox1.AppendText("\t/save - сохранение программы в файл.\r\n");
                    textBox1.AppendText("\t/new - создание новой программы.\r\n");

                    textBox1.AppendText("\t/clear - очистка консоли.\r\n");
                    textBox1.AppendText("\t/cansel - отмена поледнего изменения.\r\n");
                    textBox1.AppendText("\t/undo - возврат последнего изменения.\r\n");
                    textBox1.AppendText("\t/close - закрытие текущей вкладки.\r\n");
                    textBox1.AppendText("\t/exit - выход из приложения.\r\n");

                    textBox1.AppendText("\t/help - информация.\r\n" );
                    break;
                default:
                    textBox1.AppendText("Неизвесная команда. Используте /help для справки.\r\n");
                    break;
            }
        }

        private void ConsoleFunction(string Actions)
        {

        }
    }
}