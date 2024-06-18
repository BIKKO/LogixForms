using Microsoft.VisualBasic.Devices;
using Modbus.Device;
using System.Net.Sockets;
using System.Text;

namespace LogixForms
{
    public partial class MainThread : Form
    {
        // �������� �������
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
        //����(ddd | ddd - copy)
        private static List<List<string>> OpenFileOrCon = new List<List<string>>();
        private static List<string> TextRangs = new List<string>();//= File.ReadAllLines(@"C:\Users\njnji\Desktop\������\matplotlib\ddd", Encoding.UTF8);
        private List<VScrollBar> VScrollBarList = new List<VScrollBar>();
        private List<HScrollBar> HScrollBarList = new List<HScrollBar>();
        private ModbusIpMaster master;
        private List<ClassDraw> mainWindows = new List<ClassDraw>();
        private List<TcpClient> TcpClients = new List<TcpClient>();
        private TcpClient client;


        public MainThread()
        {
            InitializeComponent();//������������� �����
            Height = int.Parse(Properties.Settings.Default["H"].ToString());
            Width = int.Parse(Properties.Settings.Default["W"].ToString());
            Adr.Add("T4", new ushort[24]);
            Adr.Add("T4_c", new ushort[24]);
            Adr.Add("Timer_control", new ushort[32]);
            Adr.Add("N13", new ushort[70]);
            Adr.Add("N15", new ushort[70]);
            Adr.Add("N18", new ushort[70]);
            Adr.Add("N40", new ushort[70]);
            Adr.Add("B3", new ushort[70]);
            AdresUpdate.Enabled = false;
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
                Adr[adkey] = master.ReadHoldingRegisters(1, MB_adres[adkey], (ushort)Adr[adkey].Length);
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
            OpenFileOrCon[Files.SelectedIndex].Clear();
            foreach (var el in File.ReadAllLines(openFileDialog2.FileName, Encoding.UTF8)) OpenFileOrCon[Files.SelectedIndex].Add(el);
        }

        /// <summary>
        /// �������� ���� �����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void close_Click(object sender, EventArgs e)
        {
            MouseWheel -= mainWindows[Files.SelectedIndex].This_MouseWheel;
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
        /// �������� ����� � ��
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
                var close = new ToolStripMenuItem("�������");
                contextMenu.Items.Add(close);
                tb.ContextMenuStrip = contextMenu;
                close.Click += close_Click;
                pan.Controls.Add(vscrol);
                pan.Controls.Add(hScroll);
                tb.Controls.Add(pan);
                Files.TabPages.Add(tb);
                Files.SelectTab(Files.TabCount - 1);
                ClassDraw tab_to_drow = new ClassDraw(pan, File.ReadAllLines(openFileDialog2.FileName,
                    Encoding.UTF8).ToList(), vscrol,
                    hScroll, Files, Adr, Height, Width);
                tab_to_drow.StartDrow();
                mainWindows.Add(tab_to_drow);
                MouseWheel += tab_to_drow.This_MouseWheel;
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
        /// ����������� � ����������
        /// </summary>
        /// <param name="ip">IP �����</param>
        /// <param name="port">����</param>
        /// <param name="step">��� ���������� ModBus �������(������������� 1 ��� ���������� � ����������)</param>
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
                var close = new ToolStripMenuItem("�������");
                contextMenu.Items.Add(close);
                tb.ContextMenuStrip = contextMenu;
                close.Click += close_Click;
                pan.Controls.Add(Vscrol);
                pan.Controls.Add(hScroll);
                tb.Controls.Add(pan);
                Files.TabPages.Add(tb);
                Files.SelectTab(Files.TabCount - 1);
                ClassDraw tab_to_drow = new ClassDraw(pan, TextRangs, Vscrol,
                    hScroll, Files, Adr, Height, Width);
                tab_to_drow.StartDrow();
                mainWindows.Add(tab_to_drow);
                MouseWheel += tab_to_drow.This_MouseWheel;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������ �����������. �������� ����������� � ��������� �������.");
            }
        }

        /// <summary>
        /// ����� ���� ��� ����� ������ ��� �����������
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
        /// �������� ������� ����
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
            var close = new ToolStripMenuItem("�������");
            contextMenu.Items.Add(close);
            tb.ContextMenuStrip = contextMenu;
            close.Click += close_Click;
            pan.Controls.Add(Vscrol);
            pan.Controls.Add(hScroll);
            tb.Controls.Add(pan);
            Files.TabPages.Add(tb);
            Files.SelectTab(Files.TabCount - 1);
            ClassDraw tab_to_drow = new ClassDraw(pan, new List<string> { "" }, Vscrol,
                hScroll, Files, Adr, Height, Width);
            tab_to_drow.StartDrow();
            mainWindows.Add(tab_to_drow);
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

        /// <summary>
        /// ��������� ������� �������� �������
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