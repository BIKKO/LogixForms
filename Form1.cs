using System;
using System.Diagnostics.Metrics;
using System.Text;
using System.Xml.Linq;

namespace LogixForms
{
    public partial class Form1 : Form
    {
        // значения адресов
        private static ushort[] T4 = new ushort[24];
        private static ushort[] T4_c = new ushort[24];
        private static ushort[] T4_b = new ushort[24];
        private static int[] Timer_control = new int[32];
        private static ushort[] N13 = new ushort[70];
        private static ushort[] N15 = new ushort[70];
        private static ushort[] N18 = new ushort[70];
        private static ushort[] N40 = new ushort[70];
        private static ushort[] B3 = new ushort[70];

        /* пока не нужно(возможно вообще не нужно)
        public static List<(int, int)> BST = new List<(int, int)>();
        public static List<(int, int)> NXB = new List<(int, int)>();*/

        //файл(ddd | ddd - copy)
        private static string[] TextRangs = File.ReadAllLines(@"C:\Users\njnji\Desktop\проеты\matplotlib\ddd", Encoding.UTF8);
        private static int[,] info = new int[TextRangs.Length, 3]; //
        private Bitmap XIC = NodEn.XIC, XIO = NodEn.XIO, Timer_Move = NodEn.Timer___Move, EnDnTt = NodEn.EN_DN_TT, OTU = NodEn.OTU,
            OTE = NodEn.OTE, OTL = NodEn.OTE; // загрузка изображений

        private Pen pen_line = new Pen(Brushes.Black); // для отрисовки линий
        private Pen pen_line_sap = new Pen(Brushes.Blue); // для отрисовки линий
        private Pen PenOfPoint = new Pen(Brushes.Red,7);

        private Font text = new Font("Arial", 10); // текст информации
        private Font RangsFont = new Font("Arial", 12); //текст для номера ранга

        private int left_indent_rang_x = 50;//левый отступ
        private int top_indent_rang = 150;//верхний отступ
        private int scroll_y = 0;//смещение
        private int[] rang_y = new int[TextRangs.Length];

        private Dictionary<int, int[]> IndexSap = new Dictionary<int, int[]>();
        private Dictionary<int, int[,]> Info = new Dictionary<int, int[,]>();
        private Dictionary<int, int[]> Start = new Dictionary<int, int[]>();
        private Dictionary<int, int[]> Stop = new Dictionary<int, int[]>();
        private Dictionary<int, string[]> ElementsRang = new Dictionary<int, string[]>();
        private List<int[]> BIGN = new List<int[]>();
        private int isnumber;

        private int CountElInRang = 13;

        public Form1()
        {
            RangsInfo();//получение информации по рангу  ст. 120
            InitializeComponent();//инициализация формы
            this.MouseWheel += new MouseEventHandler(This_MouseWheel);//подключения колёсика мыши
            pen_line_sap.Width = pen_line.Width = 3;//толщина линий
            
            
        }

        private void This_MouseWheel(object sender, MouseEventArgs e)
        {
            int wheel = 0;//прокрутка вверх или вниз
            if (e.Delta > 0)
            {
                //вверх
                wheel = TextRangs.Length%10!=0? -1:-10;//если рангов > 10 то -1 иначе -10
            }
            else
            {
                //вниз
                wheel = TextRangs.Length % 10 != 0 ? 1 : -10;//если рангов > 10 то 1 иначе 10
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

        private void UpdateImage_Tick(object sender, EventArgs e)
        {
            Refresh();
            if (midpanel.Height * TextRangs.Length / 150 >= 150)
            {
                top_indent_rang = (midpanel.Height * TextRangs.Length) / 150;
            }
            else top_indent_rang = 150;
        }//интервал отрисовки и динамическое изменение верхнего отступа

        private void RangsInfo()
        {
            for(int line = 0;line < TextRangs.Length;line++)//создание массива из элементов
            {
                string[] rang = TextRangs[line].Trim().Split(' ');
                var element = new string[rang.Length];
                for (var i = 0; i < rang.Length;i++)
                {
                    var el = rang[i];
                    if (!el.Contains(':') && !int.TryParse(el, out isnumber))
                    {
                        element[i] = el;
                    }
                }
                ElementsRang.Add(line, element.Where(x => !string.IsNullOrEmpty(x)).ToArray());//удаление пустых значений
            }

            for(int line = 0; line < TextRangs.Length; line++)//массив из индексов старта и конца
            {
                var element = new int[ElementsRang[line].Length];
                for (int j = 0;j < ElementsRang[line].Length;j++)
                {
                    if (ElementsRang[line][j] == "BST" || ElementsRang[line][j] == "BND")
                    {
                        element[j] = j;
                    }
                    else element[j] = 0;
                }
                IndexSap.Add(line, element);
            }

            for (int line = 0; line < TextRangs.Length; line++)
            {

                string[] rang = ElementsRang[line];//ранг
                int count_el = 0;//кол-во ел в ранге
                int count_sap_el = 0;//макс кол-во ел. ветки
                int count_nxb = 0;// макс. длинна ветки вниз
                int count_sap = 0;//кол-во веток
                int logic_grup = 0;//кол-во групп веток

                foreach (var s in rang)//2
                {
                    if (s != "BST" || s != "BND" || s != "NXB")
                    {
                        if (!int.TryParse(s, out isnumber)) count_el++;
                    }
                    else if (s == "NXB") count_nxb++;//1
                }

                if (rang.Contains("BST"))//информация по рангу
                {
                    int count_el_sap = 0;
                    int buf_sap = -1;

                    for (int s = 0; s < rang.Length; s++)
                    {
                        if (s > buf_sap)
                        {
                            if (rang[s] == "BST")
                            {
                                for (int b = s + 1; b < rang.Length; b++)
                                {
                                    if (!rang[b].Contains(':'))
                                    {
                                        if (rang[b] != "NXB" && rang[b] != "BND") count_el_sap++;
                                        else
                                        {
                                            buf_sap = b - 1;
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (rang[s] == "NXB")
                            {
                                count_nxb++;
                                for (int b = s + 1; b < rang.Length; b++)
                                {
                                    if (!rang[b].Contains(':'))
                                    {
                                        if (rang[b] != "NXB" && rang[b] != "BND") count_el_sap++;
                                        else
                                        {
                                            buf_sap = b - 1;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (count_sap_el < count_el_sap)
                            {
                                count_sap_el = count_el_sap;
                                count_el_sap = 0;
                            }
                            else count_el_sap = 0;//0
                            if (rang[s] == "BND") break;
                        }
                    }
                }
                else
                {
                    count_sap_el = 1;
                }
                /*
                if (rang.Contains("BST"))
                {
                    if (Array.IndexOf(rang, "BST") != Array.LastIndexOf(rang, "BST"))
                    {
                        if (Array.IndexOf(rang, "BND") < Array.LastIndexOf(rang, "BST")) count_sap = 2;
                        else count_sap = 1;
                    }
                    else
                        count_sap = 1;
                }
                else 
                    count_sap = 0;*/

                int BranchInGropeCount = 0;
                int LogicGropeNum = 0;
                var BranchInGropeMax = new int[8];
                var BranchStart = new int[8];
                var BranchEnd = new int[8];
                var BranchInGropeNumbers = new int[10, 8];

                for(int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        BranchInGropeMax[j] = -256;
                        BranchInGropeNumbers[i,j] = -256;
                        BranchEnd[j] = -256;
                        if(j>0)
                        BranchStart[j] = -256;
                    }
                }


                for (var bn = 0;bn < ElementsRang[line].Length;bn++)
                {
                    var el = ElementsRang[line][bn];
                    switch (el)
                    {
                        case "BST":
                            {
                                BranchInGropeCount = 0; //счет ветвей сначаладущую номер ветви
                                BranchInGropeMax[LogicGropeNum] = BranchInGropeCount;
                                BranchInGropeNumbers[LogicGropeNum ,BranchInGropeCount] = count_sap; //сохранить преды
                                LogicGropeNum++;
                             
                                BranchEnd[count_sap] = bn;
                                count_sap ++;
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
                                if(count_sap == 0)
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
                BIGN.Add(BranchInGropeMax);
            }
        }

        private void PaintLines(PaintEventArgs e)
        {
            Graphics g = e.Graphics;//использование графики
            var PointOfElemetts = new int[CountElInRang+1];
            
            for(int i = 0; i < PointOfElemetts.Length; i++)
            {
                PointOfElemetts[i] = ((midpanel.Width)/ CountElInRang+1) *(i+1)-left_indent_rang_x/2;
            }

            int maxY = top_indent_rang * (TextRangs.Length - 2);//макс кол-во пикселей для scroll_y
            for (int i = 1; i < TextRangs.Length; i++)
                maxY += (top_indent_rang / 2) * info[i - 1, 0];
            //точки для текста
            PointF Scroll = new PointF(79, 50);
            PointF MaxY = new PointF(79, 70);

            scroll_y = vScrollBar1.Value * (maxY / 100);//прокрутка
            //вспом. вывод информации
            //g.DrawString(Start[12][0].ToString(), RangsFont, Brushes.Black, Scroll);
            //g.DrawString(maxY.ToString(), RangsFont, Brushes.Black, MaxY);

            //вертикаль (статична)
            g.DrawLine(pen_line, left_indent_rang_x, 0, left_indent_rang_x, midpanel.Height);
            g.DrawLine(pen_line, midpanel.Width - 2, 0, midpanel.Width - 2, midpanel.Height);

            PointF locationToDrawRangs = new PointF();//позиция  номера ранга
            locationToDrawRangs.X = 20;
            locationToDrawRangs.Y = top_indent_rang - scroll_y - 10;
            //отрисовка первого ранга
            g.DrawString("1", RangsFont, Brushes.Black, locationToDrawRangs);
            g.DrawLine(pen_line, left_indent_rang_x, top_indent_rang - scroll_y, midpanel.Width, top_indent_rang - scroll_y);
            for (int i = 0;i< PointOfElemetts.Length-1; i++)
            g.DrawEllipse(PenOfPoint, left_indent_rang_x+PointOfElemetts[i], top_indent_rang - scroll_y-2, 4, 4);
            int top_step = top_indent_rang;//верхний отступ отрисовки
            rang_y[0] = top_step;
            for (int rang = 1; rang < TextRangs.Length; rang++)
            {
                var BIGN = Info[rang];
                var LGN = info[rang, 1];
                var lgnRANG = new int[LGN];
                int start = 0;
                var GrupMaxCountEl = new int[LGN+1];
                
                top_step += top_indent_rang + ((top_indent_rang / 2) * info[rang-1, 0]);//отступ от 0,0
                locationToDrawRangs.Y = top_step - 10 - scroll_y;
                g.DrawString((rang+1).ToString(), RangsFont, Brushes.Black, locationToDrawRangs);//номер ранга от 1 до *
                g.DrawLine(pen_line, left_indent_rang_x, top_step - scroll_y, midpanel.Width, top_step - scroll_y);//основная ветка ранга от 1 до *
                for (int i = 0; i < CountElInRang+1; i++)
                    g.DrawEllipse(PenOfPoint, left_indent_rang_x + PointOfElemetts[i], top_step - scroll_y - 2, 4, 4);
                for(int grup = 0; grup < LGN;  grup++)
                {
                    int max = 0;
                    int buf;
                    for(int k = 0; k < 8; k++)
                    {
                        if(BIGN[grup, k] != -256)
                        {
                            var BN = BIGN[grup, k];
                            buf = Math.Abs(Stop[rang][BN] - Start[rang][BN]);
                            max = max>buf? max : buf;
                        }
                        else
                        {
                            BIGN[grup, k] = 0;
                        }
                    }
                    GrupMaxCountEl[grup] = max!=0? max: 1;
                }
                
                for(int lgn = 0; lgn < LGN; lgn++)
                {
                    int s = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        if (BIGN[lgn, i] != 0) s++;
                    }
                    
                    for (int j = 0; j < info[rang, 2]; j++)
                    {
                        if (BIGN[lgn, j] != -256)
                        {

                        }
                            start = Math.Abs(Stop[rang][0] - Start[rang][0]);
                        for(int lg = 0;lg < s; lg++)
                        {
                            //горизонталь
                            g.DrawLine(pen_line_sap,
                                left_indent_rang_x + PointOfElemetts[start],
                                top_step + lg * (top_indent_rang / 2) - scroll_y,
                                left_indent_rang_x + PointOfElemetts[start+GrupMaxCountEl[lgn+1]+2],
                                top_step + lg * (top_indent_rang / 2) - scroll_y);//основная ветка ранга от 1 до *
                            //вертикаль
                            g.DrawLine(pen_line_sap,
                                left_indent_rang_x + PointOfElemetts[start],
                                top_step - scroll_y,
                                left_indent_rang_x + PointOfElemetts[start],
                                top_step + lg * (top_indent_rang / 2) - scroll_y);//основная ветка ранга от 1 до *
                            g.DrawLine(pen_line_sap,
                                left_indent_rang_x + PointOfElemetts[start + GrupMaxCountEl[lgn + 1] + 2],
                                top_step - scroll_y,
                                left_indent_rang_x + PointOfElemetts[start + GrupMaxCountEl[lgn + 1] + 2],
                                top_step + lg * (top_indent_rang / 2) - scroll_y);//основная ветка ранга от 1 до *
                                                                                  //точки
                            for (int i = start; i < start+GrupMaxCountEl[lgn+1]+2 + 1; i++)
                            g.DrawEllipse(PenOfPoint, left_indent_rang_x + PointOfElemetts[i], top_step + (lg) * (top_indent_rang / 2) - scroll_y - 2, 4, 4);
                        }
                    }
                }

            }
            //вспомогательная информация (выводится)
            //g.DrawString(info[15,3].ToString(), RangsFont, Brushes.Black, MaxY);

            /*for (int rang = 0;rang < TextRangs.Length; rang++)
            {
                //отрисовка веток
                if (ElementsRang[rang].Contains("BST"))
                {
                    int xstep = left_indent_rang_x;
                    string[] elRang = TextRangs[rang].Trim().Split(' ');

                    foreach(string el in elRang)
                    {
                        if (el == "BST")
                        {
                            for(int sap = 0; sap< info[rang, 3]; sap++)
                            {
                                int ystep = top_indent_rang / 2;
                                for (int y = 0; y < info[rang, 1]; y++)
                                {
                                    g.DrawLine(pen_line_sap, xstep, rang_y[rang] + ystep - scroll_y, xstep + (info[rang, 0] * 54) + 40, rang_y[rang] + ystep - scroll_y);//горизонталь
                                    ystep += top_indent_rang / 2;
                                }
                                xstep += ((midpanel.Width - left_indent_rang_x) / (info[rang, 2] - info[rang, 0])) + 27;
                            }
                            
                        }
                        else
                        {
                            xstep += ((midpanel.Width - left_indent_rang_x) / (info[rang,2] - info[rang,0]))+27;
                        }
                    }
                }
                else continue;
            }*/
        }

        private void midpanel_Paint(object sender, PaintEventArgs e)
        {
            PaintLines(e);

        }
    }
}