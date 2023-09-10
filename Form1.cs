using Modbus.Device;
using System;
using System.Net.Sockets;
using System.Text;

namespace LogixForms
{
    public partial class Form1 : Form
    {
        // значени€ адресов
        private static ushort[] T4 = new ushort[24];
        private static ushort[] T4_c = new ushort[24];
        private static ushort[] T4_b = new ushort[24];
        private static int[] Timer_control = new int[32];
        private static ushort[] N13 = new ushort[70];
        private static ushort[] N15 = new ushort[70];
        private static ushort[] N18 = new ushort[70];
        private static ushort[] N40 = new ushort[70];
        private static ushort[] B3 = new ushort[70];

        //файл(ddd | ddd - copy)
        private static List<string> TextRangs = new List<string>();//= File.ReadAllLines(@"C:\Users\njnji\Desktop\проеты\matplotlib\ddd", Encoding.UTF8);
        private static int[,] info; //
        private Bitmap XIC = NodEn.XIC, XIO = NodEn.XIO, Timer_Move = NodEn.Timer___Move, EnDnTt = NodEn.EN_DN_TT, OTU = NodEn.OTU,
            OTE = NodEn.OTE, OTL = NodEn.OTE, XICD = NodDis.XICdis, XIOD = NodDis.XIOdis; // загрузка изображений

        private Pen pen_line = new Pen(Brushes.Black); // дл€ отрисовки линий
        private Pen pen_line_sap = new Pen(Brushes.Blue); // дл€ отрисовки линий
        private Pen PenOfPoint = new Pen(Brushes.Red, 7);

        private Font text = new Font("Arial", 10); // текст информации
        private Font RangsFont = new Font("Arial", 12); //текст дл€ номера ранга

        private static int CountElInRang = 13;
        private int left_indent_rang_x = 50;//левый отступ
        private int top_indent_rang = 150;//верхний отступ
        private int scroll_y = 0;//смещение
        private int[] rang_y;
        private int[] PointOfElemetts = new int[CountElInRang + 1];

        private Dictionary<int, int[,]> Info = new Dictionary<int, int[,]>();
        private Dictionary<int, int[]> Start = new Dictionary<int, int[]>();
        //private Dictionary<int, int[]> PointInRangs = new Dictionary<int, int[]>();
        private Dictionary<int, int[]> Stop = new Dictionary<int, int[]>();
        private Dictionary<int, string[]> ElementsRang = new Dictionary<int, string[]>();
        private Dictionary<int, string[]> AdresRang = new Dictionary<int, string[]>();
        private List<int[]> BIGM = new List<int[]>();
        private int isnumber;
        private double Dis;
        private bool OpenFile = false;
        private bool ModbusCl = false;
        private ModbusIpMaster master;
        private Random random = new Random();


        public Form1()
        {
            InitializeComponent();//инициализаци€ формы
            this.MouseWheel += new MouseEventHandler(This_MouseWheel);//подключени€ колЄсика мыши
            pen_line_sap.Width = pen_line.Width = 3;//толщина линий
        }

        private void This_MouseWheel(object sender, MouseEventArgs e)
        {
            int wheel = 0;//прокрутка вверх или вниз
            if (e.Delta > 0)
            {
                //вверх
                wheel = TextRangs.Count % 10 != 0 ? -1 : -10;//если рангов > 10 то -1 иначе -10
            }
            else
            {
                //вниз
                wheel = TextRangs.Count % 10 != 0 ? 1 : -10;//если рангов > 10 то 1 иначе 10
            }
            if (vScrollBar1.Maximum >= vScrollBar1.Value + wheel && vScrollBar1.Minimum <= vScrollBar1.Value + wheel)
                vScrollBar1.Value += wheel;//не выходим ли за приделы scrollbar
            wheel = 0;//одиночное сробатование
        }

        private int Adres(string st, ushort[] mas) //выдает значение бита в массиве
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

        private void AdresUpdate_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 24; i++)
            {
                T4[i] = (ushort)random.Next(0, 100);
                T4_c[i] = (ushort)random.Next(0, 65530);
                Timer_control[i] = 2;
            }

            for (int i = 0; i < 70; i++)
            {
                N13[i] = (ushort)random.Next(0, 65530);
                N15[i] = (ushort)random.Next(0, 65530);
                N18[i] = (ushort)random.Next(0, 65530);
                N40[i] = (ushort)random.Next(0, 65530);
                B3[i] = (ushort)random.Next(0, 65530);
            }
        }

        private void UpdateImage_Tick(object sender, EventArgs e)
        {
            Refresh();
            if (midpanel.Height * TextRangs.Count / 150 >= 150)
            {
                top_indent_rang = (midpanel.Height * TextRangs.Count) / 150;
            }
            else top_indent_rang = 150;            
        }//интервал отрисовки и динамическое изменение верхнего отступа

        private void ModBusUpdate_Tick(object sender, EventArgs e)
        {

        }

        private void FileUpdate_Tick(object sender, EventArgs e)
        {
            TextRangs.Clear();
            foreach (var el in File.ReadAllLines(openFileDialog2.FileName, Encoding.UTF8)) TextRangs.Add(el);
        }

        private void RangsInfo()
        {
            info = new int[TextRangs.Count, 3];
            rang_y = new int[TextRangs.Count];
            for (int line = 0; line < TextRangs.Count; line++)//создание массива из элементов
            {
                string[] rang = TextRangs[line].Trim().Split(' ');
                var element = new string[rang.Length];
                var adres = new string[rang.Length];
                for (var i = 0; i < rang.Length; i++)
                {
                    var el = rang[i];
                    if (!el.Contains(':') && !int.TryParse(el, out isnumber) && !el.Contains('.'))
                    {
                        element[i] = el;
                    }
                    else if (el.Contains(':')) adres[i] = el;
                }
                ElementsRang.Add(line, element.Where(x => !string.IsNullOrEmpty(x)).ToArray());//удаление пустых значений
                AdresRang.Add(line, adres.Where(x => !string.IsNullOrEmpty(x)).ToArray());
            }

            for (int line = 0; line < TextRangs.Count; line++)
            {
                int count_sap = 0;//кол-во веток

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
                                BranchInGropeCount = 0; //счет ветвей сначаладущую номер ветви
                                BranchInGropeMax[LogicGropeNum] = BranchInGropeCount;
                                BranchInGropeNumbers[LogicGropeNum, BranchInGropeCount] = count_sap; //сохранить преды
                                LogicGropeNum++;

                                BranchEnd[count_sap] = bn;
                                count_sap++;
                                BranchStart[count_sap] = bn + 1;
                                break;
                            }
                        case "NXB":
                            {
                                BranchInGropeNumbers[LogicGropeNum, BranchInGropeCount] = count_sap; //сохранить предыдущую номер ветви
                                BranchInGropeCount++;

                                BranchEnd[count_sap] = bn - 1; //previous Rung_logic
                                count_sap++;
                                BranchStart[count_sap] = bn + 1; //next Rung_logic

                                break;
                            }
                        case "BND":
                            {
                                BranchInGropeNumbers[LogicGropeNum, BranchInGropeCount] = count_sap; //сохранить предыдущую номер ветви
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
            Graphics g = e.Graphics;//использование графики

            for (int i = 0; i < PointOfElemetts.Length; i++)
            {
                PointOfElemetts[i] = ((midpanel.Width) / CountElInRang + 1) * (i + 1) - left_indent_rang_x / 2;
            }

            int maxY = top_indent_rang * (TextRangs.Count - 2);//макс кол-во пикселей дл€ scroll_y
            for (int i = 1; i < TextRangs.Count; i++)
                maxY += ((3 * top_indent_rang) / 4) * info[i - 1, 0];
            //точки дл€ текста
            PointF Scroll = new PointF(79, 50);
            PointF MaxY = new PointF(79, 70);

            scroll_y = vScrollBar1.Value * (maxY / 100);//прокрутка
            //вспом. вывод информации
            //g.DrawString(Start[12][0].ToString(), RangsFont, Brushes.Black, Scroll);
            //g.DrawString(maxY.ToString(), RangsFont, Brushes.Black, MaxY);

            //вертикаль (статична)
            g.DrawLine(pen_line, left_indent_rang_x, 0, left_indent_rang_x, midpanel.Height);
            g.DrawLine(pen_line, midpanel.Width - 2, 0, midpanel.Width - 2, midpanel.Height);

            PointF locationToDrawRangs = new PointF();//позици€  номера ранга
            locationToDrawRangs.X = 20;
            locationToDrawRangs.Y = top_indent_rang - scroll_y - 10;
            //отрисовка первого ранга
            g.DrawString("1", RangsFont, Brushes.Black, locationToDrawRangs);
            g.DrawLine(pen_line, left_indent_rang_x, top_indent_rang - scroll_y, midpanel.Width, top_indent_rang - scroll_y);
            for (int i = 0; i < PointOfElemetts.Length - 1; i++)
                g.DrawEllipse(PenOfPoint, left_indent_rang_x + PointOfElemetts[i], top_indent_rang - scroll_y - 2, 4, 4);
            int top_step = top_indent_rang;//верхний отступ отрисовки
            rang_y[0] = top_step;
            for (int rang = 1; rang < TextRangs.Count; rang++)
            {
                var BIGN = Info[rang];
                var LGN = info[rang, 1];
                var lgnRANG = new int[LGN];
                int start = 0;
                int stop = 0;
                var GrupMaxCountEl = new int[LGN + 1];

                top_step += top_indent_rang + (((3 * top_indent_rang) / 4) * info[rang - 1, 0]);//отступ от 0,0
                rang_y[rang] = top_step;
                locationToDrawRangs.Y = top_step - 10 - scroll_y;
                g.DrawString((rang + 1).ToString(), RangsFont, Brushes.Black, locationToDrawRangs);//номер ранга от 1 до *
                g.DrawLine(pen_line, left_indent_rang_x, top_step - scroll_y, midpanel.Width, top_step - scroll_y);//основна€ ветка ранга от 1 до *

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
                        //горизонталь
                        g.DrawLine(pen_line,
                            left_indent_rang_x + PointOfElemetts[start + stop],
                            top_step + lg * ((3 * top_indent_rang) / 4) - scroll_y,
                            left_indent_rang_x + PointOfElemetts[start + GrupMaxCountEl[lgn] + 1 + stop],
                            top_step + lg * ((3 * top_indent_rang) / 4) - scroll_y);//основна€ ветка ранга от 1 до *
                                                                                    //вертикаль
                        g.DrawLine(pen_line,
                            left_indent_rang_x + PointOfElemetts[start + stop],
                            top_step - scroll_y,
                            left_indent_rang_x + PointOfElemetts[start + stop],
                            top_step + lg * ((3 * top_indent_rang) / 4) - scroll_y);//основна€ ветка ранга от 1 до *
                        g.DrawLine(pen_line,
                            left_indent_rang_x + PointOfElemetts[start + GrupMaxCountEl[lgn] + 1 + stop],
                            top_step - scroll_y,
                            left_indent_rang_x + PointOfElemetts[start + GrupMaxCountEl[lgn] + 1 + stop],
                            top_step + lg * ((3 * top_indent_rang) / 4) - scroll_y);//основна€ ветка ранга от 1 до *
                                                                                    //точки
                        for (int i = start + stop; i < start + GrupMaxCountEl[lgn] + 2 + stop; i++)
                            g.DrawEllipse(PenOfPoint, left_indent_rang_x + PointOfElemetts[i], top_step + lg * ((3 * top_indent_rang) / 4) - scroll_y - 2, 4, 4);
                    }
                    stop = start;
                    if (0 == BIGM[rang][lgn] && stop > 0) stop++;
                }

            }
            //вспомогательна€ информаци€ (выводитс€)
            //g.DrawString(info[15,3].ToString(), RangsFont, Brushes.Black, MaxY);
        }

        private void midpanel_Paint(object sender, PaintEventArgs e)
        {
            if (OpenFile || ModbusCl)
            {
                Graphics g = e.Graphics;
                g.Clear(Color.White);
                PaintLines(e);
                for (int rang = 0; rang < TextRangs.Count; rang++)
                {
                    int nxb = 0;
                    int ind = 0;
                    int num = 0;
                    ushort[] mas = new ushort[70];
                    for (var bn = 0; bn < ElementsRang[rang].Length; bn++)
                    {
                        var el = ElementsRang[rang][bn];
                        var adres = AdresRang[rang][num < AdresRang[rang].Length ? num : 0];
                        if (adres.Contains("N13"))
                        {
                            N13.CopyTo(mas, 0);
                        }
                        else if (adres.Contains("N15"))
                        {
                            N15.CopyTo(mas, 0);
                        }
                        else if (adres.Contains("N18"))
                        {
                            N18.CopyTo(mas, 0);
                        }
                        else if (adres.Contains("N40"))
                        {
                            N40.CopyTo(mas, 0);
                        }
                        else if (adres.Contains("B3"))
                        {
                            B3.CopyTo(mas, 0);
                        }
                        var point = new PointF(left_indent_rang_x + PointOfElemetts[ind] - 40, rang_y[rang] + ((3 * top_indent_rang) / 4) * nxb - scroll_y - 50);
                        switch (el)
                        {
                            case "XIO":
                                {
                                    if (Adres(adres, mas) == 0)
                                        g.DrawImage(XIOD, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27, rang_y[rang] + ((3 * top_indent_rang) / 4) * nxb - scroll_y - 25, 54, 50));
                                    else
                                        g.DrawImage(XIO, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27, rang_y[rang] + ((3 * top_indent_rang) / 4) * nxb - scroll_y - 25, 54, 50));
                                    g.DrawString(adres, RangsFont, Brushes.Black, point);
                                    ind++;
                                    num++;
                                    break;
                                }
                            case "XIC":
                                {
                                    if (Adres(adres, mas) == 1)
                                        g.DrawImage(XICD, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27, rang_y[rang] + ((3 * top_indent_rang) / 4) * nxb - scroll_y - 25, 54, 50));
                                    else
                                        g.DrawImage(XIC, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27, rang_y[rang] + ((3 * top_indent_rang) / 4) * nxb - scroll_y - 25, 54, 50));
                                    g.DrawString(adres, RangsFont, Brushes.Black, point);
                                    ind++;
                                    num++;
                                    break;
                                }
                            case "OTE":
                                {
                                    //if (Adres(adres, mas) == 1)
                                    g.DrawImage(OTE, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27, rang_y[rang] + ((3 * top_indent_rang) / 4) * nxb - scroll_y - 25, 54, 50));
                                    g.DrawString(adres, RangsFont, Brushes.Black, point);
                                    ind++;
                                    num++;
                                    break;
                                }
                            case "OTL":
                                {
                                    //if (Adres(adres, mas) == 1)
                                    g.DrawImage(OTL, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27, rang_y[rang] + ((3 * top_indent_rang) / 4) * nxb - scroll_y - 25, 54, 50));
                                    g.DrawString(adres, RangsFont, Brushes.Black, point);
                                    ind++;
                                    num++;
                                    break;
                                }
                            case "OTU":
                                {
                                    //if (Adres(adres, mas) == 1)
                                    g.DrawImage(OTU, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27, rang_y[rang] + ((3 * top_indent_rang) / 4) * nxb - scroll_y - 25, 54, 50));
                                    g.DrawString(adres, RangsFont, Brushes.Black, point);
                                    ind++;
                                    num++;
                                    break;
                                }
                            case "ONS":
                                {
                                    //g.DrawImage(OTU, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27, rang_y[rang] + ((3*top_indent_rang )/ 4) * nxb - scroll_y - 25, 54, 50));
                                    g.DrawString(adres, RangsFont, Brushes.Black, point);
                                    ind++;
                                    num++;
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
            TextRangs.Clear();
            foreach (var el in File.ReadAllLines(openFileDialog2.FileName, Encoding.UTF8)) TextRangs.Add(el);
            ElementsRang.Clear();
            Info.Clear();
            Start.Clear();
            Stop.Clear();
            BIGM.Clear();
            vScrollBar1.Value = 0;
            FileUpdate.Enabled = true;
            RangsInfo();
            ModBusUpdate.Enabled = false;
            OpenFile = true;
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
                foreach (string rang in TextRangs)
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
                ElementsRang.Clear();
                Info.Clear();
                Start.Clear();
                Stop.Clear();
                BIGM.Clear();
                vScrollBar1.Value = 0;

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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ќшибка подключени€. ѕроверте подключение и повторите попытку.\n{ex}");
            }
            ModbusCl = true;
            RangsInfo();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileUpdate.Enabled = false;
            //ModBusUpdate.Enabled = true;

            if (Application.OpenForms["ConnectForms"] == null)
                new ConnectForms(this).Show();
            OpenFile = false;
            TextRangs.Clear();
            ElementsRang.Clear();
            Info.Clear();
            Start.Clear();
            Stop.Clear();
            BIGM.Clear();
            vScrollBar1.Value = 0;
            //con();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("¬ременно ничего нет!");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("¬ременно ничего нет!");
        }

    }
}