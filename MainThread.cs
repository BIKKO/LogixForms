using Modbus.Device;
using System.Net.Sockets;
using System.Text;

namespace LogixForms
{
    public partial class MainThread : Form
    {
        // �������� �������
        public static Dictionary<string, ushort[]> Adr = new Dictionary<string, ushort[]>();
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

        //����(ddd | ddd - copy)
        private static List<List<string>> OpenFileOrCon = new List<List<string>>();
        private static List<string> TextRangs = new List<string>();//= File.ReadAllLines(@"C:\Users\njnji\Desktop\������\matplotlib\ddd", Encoding.UTF8);
        private List<VScrollBar> VScrollBarList = new List<VScrollBar>();
        private List<HScrollBar> HScrollBarList = new List<HScrollBar>();
        //private int isnumber;
        private bool OpenFile = false;
        private bool ModbusCl = false;
        private ModbusIpMaster master;
        private bool NotFount = false;
        private List<ClassDrow> mainWindows = new List<ClassDrow>();


        public MainThread()
        {
            //Files.Visible = false;
            InitializeComponent();//������������� �����
            MouseWheel += This_MouseWheel;//����������� ������� ����
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

        private void This_MouseWheel(object sender, MouseEventArgs e)
        {
            int wheel = 0;//��������� ����� ��� ����
            if (e.Delta > 0)
            {
                //�����
                wheel = TextRangs.Count % 10 != 0 ? -25 : -250;//���� ������ > 10 �� -1 ����� -10
            }
            else
            {
                //����
                wheel = TextRangs.Count % 10 != 0 ? 25 : 250;//���� ������ > 10 �� 1 ����� 10
            }
            if (Files.TabCount > 0)
            {
                if (VScrollBarList[Files.SelectedIndex].Maximum >= VScrollBarList[Files.SelectedIndex].Value + wheel && VScrollBarList[Files.SelectedIndex].Minimum <= VScrollBarList[Files.SelectedIndex].Value + wheel)
                    VScrollBarList[Files.SelectedIndex].Value += wheel;//�� ������� �� �� ������� scrollbar
            }
            wheel = 0;//��������� ������������
        }

        private void AdresUpdate_Tick(object sender, EventArgs e)
        {
            Adr["T4"] = master.ReadHoldingRegisters(1, 1300, 24);
            Adr["T4_c"] = master.ReadHoldingRegisters(1, 6800, 24);
            Adr["N13"] = master.ReadHoldingRegisters(1, 10000, 3);
            Adr["N15"] = master.ReadHoldingRegisters(1, 600, 48);
            Adr["N18"] = master.ReadHoldingRegisters(1, 1200, 24);
            Adr["N40"] = master.ReadHoldingRegisters(1, 2000, 1);
            Adr["B3"] = master.ReadHoldingRegisters(1, 7200, 32);

        }

        private void ModBusUpdate_Tick(object sender, EventArgs e)
        {

        }

        private void FileUpdate_Tick(object sender, EventArgs e)
        {
            FileUpdate.Enabled = false;
            OpenFileOrCon[Files.SelectedIndex].Clear();
            foreach (var el in File.ReadAllLines(openFileDialog2.FileName, Encoding.UTF8)) OpenFileOrCon[Files.SelectedIndex].Add(el);
        }

        private void close_Click(object sender, EventArgs e)
        {
            mainWindows[Files.SelectedIndex] = null;
            Files.TabPages.Remove(Files.SelectedTab);
            GC.Collect();
        }

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
                ClassDrow tab_to_drow = new ClassDrow(pan, File.ReadAllLines(openFileDialog2.FileName,
                    Encoding.UTF8).ToList(), vscrol,
                    hScroll, Files, Adr, Height, Width);
                tab_to_drow.StartDrow();
                mainWindows.Add(tab_to_drow);
            }
        }

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

        public void con(string ip, int step, byte slave)
        {
            try
            {
                TextRangs.Clear();
                TcpClient client = new TcpClient(ip, 502);
                master = ModbusIpMaster.CreateIp(client);
                ModBusUpdate.Enabled = true;

                ushort[] inputs;

                for (int j = 0; j < 100; j++)
                {
                    inputs = master.ReadHoldingRegisters(slave, (ushort)(step * j + 8000), 120);
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
                ModbusCl = true;
                var tb = new TabPage();
                tb.Text = ip + ":502";

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
                ClassDrow tab_to_drow = new ClassDrow(pan, TextRangs, Vscrol,
                    hScroll, Files, Adr, Height, Width);
                tab_to_drow.StartDrow();
                mainWindows.Add(tab_to_drow);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������ �����������. �������� ����������� � ��������� �������.");
            }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["SettingsLogix"] == null)
            {
                new SettingsLogix(Adr).Show();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("�������� ������ ���!");
        }

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
            ClassDrow tab_to_drow = new ClassDrow(pan, new List<string> { "" }, Vscrol,
                hScroll, Files, Adr, Height, Width);
            tab_to_drow.StartDrow();
            mainWindows.Add(tab_to_drow);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default["H"] = Height;
            Properties.Settings.Default["W"] = Width;
            Properties.Settings.Default["Adres"] = Adr;
            Properties.Settings.Default.Save();
        }

        private void adresesValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["ValueAdre"] == null)
            {
                new ValueAdres(Adr).Show();
            }
        }
    }
}