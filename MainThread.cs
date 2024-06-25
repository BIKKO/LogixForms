using Microsoft.VisualBasic.Devices;
using Modbus.Device;
using System.Net.Sockets;
using System.Text;

namespace LogixForms
{
    public partial class MainThread : Form
    {
        // �������� �������
        private static Dictionary<string, ushort[]> Adr;
        protected static Dictionary<string, ushort> MB_adres;
        public ModbusIpMaster master;
        private List<ClassDraw> mainWindows = new List<ClassDraw>();
        private List<int> ConnectedWindows = new List<int>();
        private TcpClient client;
        private byte slave = 1;


        public MainThread()
        {
            InitializeComponent();//������������� �����
            Height = int.Parse(Properties.Settings.Default["H"].ToString());
            Width = int.Parse(Properties.Settings.Default["W"].ToString());
            AdresUpdate.Enabled = false;
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
        }

        /// <summary>
        /// ���������� �������� ������� � ������ � ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdresUpdate_Tick(object sender, EventArgs e)
        {
            string[] adreskey = Adr.Keys.ToArray();
            foreach (string adkey in adreskey)
            {
                //Thread.Sleep(100);
                Adr[adkey] = master.ReadHoldingRegisters(slave, MB_adres[adkey], (ushort)Adr[adkey].Length);
            }
        }

        /// <summary>
        /// ������ ������ ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemoryClear_Tick(object sender, EventArgs e)
        {
            GC.Collect();
        }

        /// <summary>
        /// ���������� �������� � ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModBusUpdate_Tick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// ���������� ��������� � �����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileUpdate_Tick(object sender, EventArgs e)
        {
            FileUpdate.Enabled = false;
        }

        /// <summary>
        /// �������� ���� �����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void close_Click(object sender, EventArgs e)
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
                TabPage tp = Files.SelectedTab;
                Files.TabPages.Remove(Files.SelectedTab);
                tp.Dispose();
                cd.Dispose();
                GC.Collect();
            }
        }

        /// <summary>
        /// �������� ����� � ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "My files (*.LDF)|*.ldf|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                var tb = new MyTabPage();
                tb.Text = openFileDialog2.FileName.Split('\\')[^1];

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
                var close = new ToolStripMenuItem("�������");
                contextMenu.Items.Add(close);
                tb.ContextMenuStrip = contextMenu;
                contextMenu = null;
                close.Click += close_Click;
                pan.Controls.Add(vscrol);
                pan.Controls.Add(hScroll);
                tb.Controls.Add(pan);
                Files.TabPages.Add(tb);
                Files.SelectTab(Files.TabCount - 1);
                List<string> Text;
                ClassDraw tab_to_drow;
                if (tb.Text.Contains("ldf"))
                {
                    new Thread(()=>
                    {
                        BeginInvoke(new MethodInvoker(() =>
                        {
                            Dictionary<string, ushort[]> adr = CreateFile.GetData(openFileDialog2.FileName);
                            int wh = Files.SelectedTab.Width;
                            Text = CreateFile.Load(openFileDialog2.FileName, Type.RANG).ToList();
                            tab_to_drow = new ClassDraw(ref pan, Text, ref vscrol,
                            ref hScroll, ref wh, ref adr, Height, Width, CreateFile.GetTegs(openFileDialog2.FileName));
                            tab_to_drow.StartDrow();
                            mainWindows.Add(tab_to_drow);
                            MouseWheel += tab_to_drow.This_MouseWheel;
                        }
                        ));
                    }).Start();
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
        /// ���������� ���������� �� ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
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
        /// ����������� � ����������
        /// </summary>
        /// <param name="ip">IP �����</param>
        /// <param name="port">����</param>
        /// <param name="simulate"></param>
        /// <param name="slave">ID</param>
        public void con(string ip, string port, byte slave)
        {
            try
            {
                List<string> TextRangs = new List<string>();
                client = new TcpClient(ip, int.Parse(port));
                //TcpClients.Add(client);
                master = ModbusIpMaster.CreateIp(client);

                ushort[] inputs;

                for (int j = 0; j < 100; j++)
                {
                    inputs = master.ReadHoldingRegisters(1, (ushort)(j + 8000), 120);
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
                var close = new ToolStripMenuItem("�������");
                contextMenu.Items.Add(close);
                tb.ContextMenuStrip = contextMenu;
                contextMenu = null;
                close.Click += close_Click;
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
                }).Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������ �����������. �������� ����������� � ��������� �������.");
                ConnectedWindows.Remove(mainWindows.Count-1);
                new ConnectForms(this).Show();
            }
        }

        /// <summary>
        /// �������� ������� ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
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
            var close = new ToolStripMenuItem("�������");
            contextMenu.Items.Add(close);
            tb.ContextMenuStrip = contextMenu;
            contextMenu = null;
            close.Click += close_Click;
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
            }).Start();
        }

        /// <summary>
        /// ����� ���� ��� ����� ������ ��� �����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ConnectedWindows.Count < 1)
            {
                FileUpdate.Enabled = false;
                //ModBusUpdate.Enabled = true;

                if (Application.OpenForms["ConnectForms"] == null)
                {
                    new ConnectForms(this).Show();
                    //con();
                }
            }
        }

        /// <summary>
        /// ����� ���� ��������
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
        /// ����� ���� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("�������� ������ ���!");
        }

        /// <summary>
        /// ��������� ������� �������� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void adresesValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Files.TabCount > 0 && Application.OpenForms[Files.SelectedTab.Text] == null)
            {
                bool flag = ConnectedWindows.Contains(Files.SelectedIndex);
                new ValueAdres(ref mainWindows[Files.SelectedIndex].GetDataTabl, Files.SelectedTab.Text, master, ref MB_adres, ref flag).Show();
            }
        }

        /// <summary>
        /// ���������� ����������� ����� ����������� ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default["H"] = Height;
            Properties.Settings.Default["W"] = Width;
            Properties.Settings.Default.Save();
        }
    }
}