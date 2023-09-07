using System;
using System.Diagnostics.Metrics;
using System.Text;
using System.Xml.Linq;

namespace LogixForms
{
    public partial class Form1 : Form
    {
        // �������� �������
        private static ushort[] T4 = new ushort[24];
        private static ushort[] T4_c = new ushort[24];
        private static ushort[] T4_b = new ushort[24];
        private static int[] Timer_control = new int[32];
        private static ushort[] N13 = new ushort[70];
        private static ushort[] N15 = new ushort[70];
        private static ushort[] N18 = new ushort[70];
        private static ushort[] N40 = new ushort[70];
        private static ushort[] B3 = new ushort[70];

        /* ���� �� �����(�������� ������ �� �����)
        public static List<(int, int)> BST = new List<(int, int)>();
        public static List<(int, int)> NXB = new List<(int, int)>();*/

        //����(ddd | ddd - copy)
        private static string[] TextRangs;//= File.ReadAllLines(@"C:\Users\njnji\Desktop\������\matplotlib\ddd", Encoding.UTF8);
        private static int[,] info; //
        private Bitmap XIC = NodEn.XIC, XIO = NodEn.XIO, Timer_Move = NodEn.Timer___Move, EnDnTt = NodEn.EN_DN_TT, OTU = NodEn.OTU,
            OTE = NodEn.OTE, OTL = NodEn.OTE; // �������� �����������

        private Pen pen_line = new Pen(Brushes.Black); // ��� ��������� �����
        private Pen pen_line_sap = new Pen(Brushes.Blue); // ��� ��������� �����
        private Pen PenOfPoint = new Pen(Brushes.Red, 7);

        private Font text = new Font("Arial", 10); // ����� ����������
        private Font RangsFont = new Font("Arial", 12); //����� ��� ������ �����

        private static int CountElInRang = 13;
        private int left_indent_rang_x = 50;//����� ������
        private int top_indent_rang = 150;//������� ������
        private int scroll_y = 0;//��������
        private int[] rang_y;
        private int[] PointOfElemetts = new int[CountElInRang + 1];

        private Dictionary<int, int[,]> Info = new Dictionary<int, int[,]>();
        private Dictionary<int, int[]> Start = new Dictionary<int, int[]>();
        //private Dictionary<int, int[]> PointInRangs = new Dictionary<int, int[]>();
        private Dictionary<int, int[]> Stop = new Dictionary<int, int[]>();
        private Dictionary<int, string[]> ElementsRang = new Dictionary<int, string[]>();
        private List<int[]> BIGM = new List<int[]>();
        private int isnumber;
        private bool OpenFile = false;


        public Form1()
        {
            InitializeComponent();//������������� �����
            this.MouseWheel += new MouseEventHandler(This_MouseWheel);//����������� ������� ����
            pen_line_sap.Width = pen_line.Width = 3;//������� �����
        }

        private void This_MouseWheel(object sender, MouseEventArgs e)
        {
            if (OpenFile)
            {
                int wheel = 0;//��������� ����� ��� ����
                if (e.Delta > 0)
                {
                    //�����
                    wheel = TextRangs.Length % 10 != 0 ? -1 : -10;//���� ������ > 10 �� -1 ����� -10
                }
                else
                {
                    //����
                    wheel = TextRangs.Length % 10 != 0 ? 1 : -10;//���� ������ > 10 �� 1 ����� 10
                }
                if (vScrollBar1.Maximum >= vScrollBar1.Value + wheel && vScrollBar1.Minimum <= vScrollBar1.Value + wheel)
                    vScrollBar1.Value += wheel;//�� ������� �� �� ������� scrollbar
                wheel = 0;//��������� ������������
            }
        }

        private int Adres(string st, ushort[] mas) //������ �������� ���� � �������
        {
            string[] k = new string[2];
            int Bitmask = 0;
            int ind_1;
            int adr;

            if (st.Contains("N13")) k = st.Replace("N13:", "").Split('/');
            if (st.Contains("N15")) k = st.Replace("N15:", "").Split('/');
            if (st.Contains("N18")) k = st.Replace("N18:", "").Split('/');
            if (st.Contains("N40")) k = st.Replace("N40:", "").Split('/');
            if (st.Contains("B3")) k = st.Replace("B3:", "").Split('/');
            if (st.Contains("T4")) k = st.Replace("T4:", "").Split('/');

            if (k.Contains("EN"))
            {
                Bitmask = 1;
                ind_1 = int.Parse(k[0]);
                adr = Timer_control[ind_1];

                if ((adr & Bitmask) == Bitmask) return 1;
                return 0;
            }
            else if (k.Contains("DN"))
            {
                Bitmask = 2;
                ind_1 = int.Parse(k[0]);
                adr = Timer_control[ind_1];

                if ((adr & Bitmask) == Bitmask) return 1;
                return 0;
            }
            else if (k.Contains("TT"))
            {
                Bitmask = 4;
                ind_1 = int.Parse(k[0]);
                adr = Timer_control[ind_1];

                if ((adr & Bitmask) == Bitmask) return 1;
                return 0;
            }
            else
            {
                Bitmask = 1 << int.Parse(k[1]);

                ind_1 = int.Parse(k[0]);
                adr = mas[ind_1];

                if ((adr & Bitmask) == Bitmask) return 1;
                return 0;
            }
        }

        private void UpdateImage_Tick(object sender, EventArgs e)
        {
            if (OpenFile)
            {

                Refresh();
                if (midpanel.Height * TextRangs.Length / 150 >= 150)
                {
                    top_indent_rang = (midpanel.Height * TextRangs.Length) / 150;
                }
                else top_indent_rang = 150;
            }
        }//�������� ��������� � ������������ ��������� �������� �������

        private void ModBusUpdate_Tick(object sender, EventArgs e)
        {

        }

        private void FileUpdate_Tick(object sender, EventArgs e)
        {
            TextRangs = File.ReadAllLines(openFileDialog2.FileName, Encoding.UTF8);
        }

        private void RangsInfo()
        {
            info = new int[TextRangs.Length, 3];
            rang_y = new int[TextRangs.Length];
            for (int line = 0; line < TextRangs.Length; line++)//�������� ������� �� ���������
            {
                string[] rang = TextRangs[line].Trim().Split(' ');
                var element = new string[rang.Length];
                for (var i = 0; i < rang.Length; i++)
                {
                    var el = rang[i];
                    if (!el.Contains(':') && !int.TryParse(el, out isnumber))
                    {
                        element[i] = el;
                    }
                }
                ElementsRang.Add(line, element.Where(x => !string.IsNullOrEmpty(x)).ToArray());//�������� ������ ��������
            }

            for (int line = 0; line < TextRangs.Length; line++)
            {
                int count_sap = 0;//���-�� �����

                int BranchInGropeCount = 0;
                int LogicGropeNum = 0;
                var BranchInGropeMax = new int[8];
                var BranchStart = new int[8];
                var BranchEnd = new int[8];
                var BranchInGropeNumbers = new int[10, 8];

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        BranchInGropeMax[j] = -256;
                        BranchInGropeNumbers[i, j] = -256;
                        BranchEnd[j] = -256;
                        if (j > 0)
                            BranchStart[j] = -256;
                    }
                }

                for (var bn = 0; bn < ElementsRang[line].Length; bn++)
                {
                    var el = ElementsRang[line][bn];
                    switch (el)
                    {
                        case "BST":
                            {
                                BranchInGropeCount = 0; //���� ������ ������������ ����� �����
                                BranchInGropeMax[LogicGropeNum] = BranchInGropeCount;
                                BranchInGropeNumbers[LogicGropeNum, BranchInGropeCount] = count_sap; //��������� �����
                                LogicGropeNum++;

                                BranchEnd[count_sap] = bn;
                                count_sap++;
                                BranchStart[count_sap] = bn + 1;
                                break;
                            }
                        case "NXB":
                            {
                                BranchInGropeNumbers[LogicGropeNum, BranchInGropeCount] = count_sap; //��������� ���������� ����� �����
                                BranchInGropeCount++;

                                BranchEnd[count_sap] = bn - 1; //previous Rung_logic
                                count_sap++;
                                BranchStart[count_sap] = bn + 1; //next Rung_logic

                                break;
                            }
                        case "BND":
                            {
                                BranchInGropeNumbers[LogicGropeNum, BranchInGropeCount] = count_sap; //��������� ���������� ����� �����
                                //BranchInGropeCount++;
                                BranchInGropeMax[LogicGropeNum] = BranchInGropeCount;
                                LogicGropeNum++;

                                BranchEnd[count_sap] = bn - 1;
                                count_sap++;
                                BranchStart[count_sap] = bn + 1;

                                break;
                            }
                        default:
                            {
                                if (count_sap == 0)
                                {
                                    BranchInGropeMax[LogicGropeNum] = 0;
                                    BranchEnd[0] = bn;
                                }
                                break;
                            }
                    }
                }

                info[line, 0] = BranchInGropeMax.Max();
                info[line, 1] = LogicGropeNum;
                info[line, 2] = count_sap;
                Info.Add(line, BranchInGropeNumbers);
                Start.Add(line, BranchStart);
                Stop.Add(line, BranchEnd);
                BIGM.Add(BranchInGropeMax);
            }
        }

        private void PaintLines(PaintEventArgs e)
        {
            Graphics g = e.Graphics;//������������� �������

            for (int i = 0; i < PointOfElemetts.Length; i++)
            {
                PointOfElemetts[i] = ((midpanel.Width) / CountElInRang + 1) * (i + 1) - left_indent_rang_x / 2;
            }

            int maxY = top_indent_rang * (TextRangs.Length - 2);//���� ���-�� �������� ��� scroll_y
            for (int i = 1; i < TextRangs.Length; i++)
                maxY += (top_indent_rang / 2) * info[i - 1, 0];
            //����� ��� ������
            PointF Scroll = new PointF(79, 50);
            PointF MaxY = new PointF(79, 70);

            scroll_y = vScrollBar1.Value * (maxY / 100);//���������
            //�����. ����� ����������
            //g.DrawString(Start[12][0].ToString(), RangsFont, Brushes.Black, Scroll);
            //g.DrawString(maxY.ToString(), RangsFont, Brushes.Black, MaxY);

            //��������� (��������)
            g.DrawLine(pen_line, left_indent_rang_x, 0, left_indent_rang_x, midpanel.Height);
            g.DrawLine(pen_line, midpanel.Width - 2, 0, midpanel.Width - 2, midpanel.Height);

            PointF locationToDrawRangs = new PointF();//�������  ������ �����
            locationToDrawRangs.X = 20;
            locationToDrawRangs.Y = top_indent_rang - scroll_y - 10;
            //��������� ������� �����
            g.DrawString("1", RangsFont, Brushes.Black, locationToDrawRangs);
            g.DrawLine(pen_line, left_indent_rang_x, top_indent_rang - scroll_y, midpanel.Width, top_indent_rang - scroll_y);
            for (int i = 0; i < PointOfElemetts.Length - 1; i++)
                g.DrawEllipse(PenOfPoint, left_indent_rang_x + PointOfElemetts[i], top_indent_rang - scroll_y - 2, 4, 4);
            int top_step = top_indent_rang;//������� ������ ���������
            rang_y[0] = top_step;
            for (int rang = 1; rang < TextRangs.Length; rang++)
            {
                var BIGN = Info[rang];
                var LGN = info[rang, 1];
                var lgnRANG = new int[LGN];
                int start = 0;
                int stop = 0;
                var GrupMaxCountEl = new int[LGN + 1];

                top_step += top_indent_rang + ((top_indent_rang / 2) * info[rang - 1, 0]);//������ �� 0,0
                rang_y[rang] = top_step;
                locationToDrawRangs.Y = top_step - 10 - scroll_y;
                g.DrawString((rang + 1).ToString(), RangsFont, Brushes.Black, locationToDrawRangs);//����� ����� �� 1 �� *
                g.DrawLine(pen_line, left_indent_rang_x, top_step - scroll_y, midpanel.Width, top_step - scroll_y);//�������� ����� ����� �� 1 �� *

                for (int i = 0; i < CountElInRang + 1; i++)
                    g.DrawEllipse(PenOfPoint, left_indent_rang_x + PointOfElemetts[i], top_step - scroll_y - 2, 4, 4);

                for (int grup = 0; grup < LGN; grup++)
                {
                    int max = 0;
                    int buf = 0;
                    for (int k = 0; k < 8; k++)
                    {
                        if (BIGN[grup, k] != -256)
                        {
                            var BN = BIGN[grup, k];
                            buf = Math.Abs(Stop[rang][BN] - Start[rang][BN]) + 1;
                            max = max > buf ? max : buf;
                        }
                        else
                        {
                            break;
                            //BIGN[grup, k] = 0;
                        }
                    }
                    GrupMaxCountEl[grup] = max != 0 ? max : 1;
                }

                for (int lgn = 0; lgn < LGN; lgn++)
                {
                    for (int lg = 1; lg < BIGM[rang][lgn] + 1; lg++)
                    {
                        start = Math.Abs(Stop[rang][BIGN[lgn, lg - 1]] - Start[rang][BIGN[lgn, lg - 1]]) + GrupMaxCountEl[lgn - 1] - 1 + (stop != 0 ? stop - 1 : stop);
                        //�����������
                        g.DrawLine(pen_line_sap,
                            left_indent_rang_x + PointOfElemetts[start + stop],
                            top_step + lg * (top_indent_rang / 2) - scroll_y,
                            left_indent_rang_x + PointOfElemetts[start + GrupMaxCountEl[lgn] + 1 + stop],
                            top_step + lg * (top_indent_rang / 2) - scroll_y);//�������� ����� ����� �� 1 �� *
                                                                              //���������
                        g.DrawLine(pen_line_sap,
                            left_indent_rang_x + PointOfElemetts[start + stop],
                            top_step - scroll_y,
                            left_indent_rang_x + PointOfElemetts[start + stop],
                            top_step + lg * (top_indent_rang / 2) - scroll_y);//�������� ����� ����� �� 1 �� *
                        g.DrawLine(pen_line_sap,
                            left_indent_rang_x + PointOfElemetts[start + GrupMaxCountEl[lgn] + 1 + stop],
                            top_step - scroll_y,
                            left_indent_rang_x + PointOfElemetts[start + GrupMaxCountEl[lgn] + 1 + stop],
                            top_step + lg * (top_indent_rang / 2) - scroll_y);//�������� ����� ����� �� 1 �� *
                                                                              //�����
                        for (int i = start + stop; i < start + GrupMaxCountEl[lgn] + 2 + stop; i++)
                            g.DrawEllipse(PenOfPoint, left_indent_rang_x + PointOfElemetts[i], top_step + lg * (top_indent_rang / 2) - scroll_y - 2, 4, 4);
                    }
                    stop = start;
                    if (0 == BIGM[rang][lgn] && stop > 0) stop++;
                }

            }
            //��������������� ���������� (���������)
            //g.DrawString(info[15,3].ToString(), RangsFont, Brushes.Black, MaxY);
        }

        private void midpanel_Paint(object sender, PaintEventArgs e)
        {
            if(OpenFile)
            {
                Graphics g = e.Graphics;
                g.Clear(Color.White);
                PaintLines(e);
                for (int rang = 0; rang < TextRangs.Length; rang++)
                {
                    int nxb = 0;
                    int ind = 0;
                    for (var bn = 0; bn < ElementsRang[rang].Length; bn++)
                    {
                        var el = ElementsRang[rang][bn];
                        switch (el)
                        {
                            case "XIO":
                                {
                                    g.DrawImage(XIO, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27, rang_y[rang] + (top_indent_rang / 2) * nxb - scroll_y - 25, 54, 50));
                                    ind++;
                                    break;
                                }
                            case "XIC":
                                {
                                    g.DrawImage(XIC, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27, rang_y[rang] + (top_indent_rang / 2) * nxb - scroll_y - 25, 54, 50));
                                    ind++;
                                    break;
                                }
                            case "OTE":
                                {
                                    g.DrawImage(OTE, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27, rang_y[rang] + (top_indent_rang / 2) * nxb - scroll_y - 25, 54, 50));
                                    ind++;
                                    break;
                                }
                            case "OTL":
                                {
                                    g.DrawImage(OTL, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27, rang_y[rang] + (top_indent_rang / 2) * nxb - scroll_y - 25, 54, 50));
                                    ind++;
                                    break;
                                }
                            case "OTU":
                                {
                                    g.DrawImage(OTU, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27, rang_y[rang] + (top_indent_rang / 2) * nxb - scroll_y - 25, 54, 50));
                                    ind++;
                                    break;
                                }
                            case "ONS":
                                {
                                    //g.DrawImage(OTU, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27, rang_y[rang] + (top_indent_rang / 2) * nxb - scroll_y - 25, 54, 50));
                                    ind++;
                                    break;
                                }
                            case "TON":
                                {
                                    g.DrawImage(Timer_Move, new Rectangle(left_indent_rang_x + PointOfElemetts[11] - 37, rang_y[rang] - scroll_y - 25, 75, 50));
                                    ind++;
                                    break;
                                }
                            case "MOV":
                                {
                                    g.DrawImage(Timer_Move, new Rectangle(left_indent_rang_x + PointOfElemetts[11] - 37, rang_y[rang] - scroll_y - 25, 75, 50));
                                    ind++;
                                    break;
                                }
                            case "BST":
                                {
                                    ind++;
                                    break;
                                }
                            case "NXB":
                                {
                                    nxb++;
                                    ind--;
                                    break;
                                }
                            case "BND":
                                {
                                    ind++;
                                    nxb = 0;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
            TextRangs = File.ReadAllLines(openFileDialog2.FileName, Encoding.UTF8);
            ElementsRang.Clear();
            Info.Clear();
            Start.Clear();
            Stop.Clear();
            BIGM.Clear();
            vScrollBar1.Value = 0;
            FileUpdate.Enabled = true;
            RangsInfo();
            OpenFile = true;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}