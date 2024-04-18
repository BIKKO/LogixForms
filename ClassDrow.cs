using System;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace LogixForms
{
    public class ClassDrow
    {
        private Bitmap XIC = NodEn.XIC, XIO = NodEn.XIO, Timer_Move = NodEn.Timer___Move, EnDnTt = NodEn.EN_DN_TT, OTU = NodEn.OTU,
            OTE = NodEn.OTE, OTL = NodEn.OTE, XICD = NodDis.XICdis, XIOD = NodDis.XIOdis; // загрузка изображений
        private List<string> info_file;
        private int isnumber;
        private  int[,] info;
        private int[] rang_y;
        private List<string[]> ElementsRang;
        private List<string[]> AdresRang;
        private List<int[]> BIGM;
        private Dictionary<int, int[,]> Info;
        private Dictionary<int, int[]> Start;
        private Dictionary<int, int[]> Stop;
        private  int CountElInRang = 13;
        private int[] PointOfElemetts;
        private int left_indent_rang_x = 50;
        private int top_indent_rang = 150;
        private VScrollBar VScroll;
        private HScrollBar HScroll;
        private int scroll_y = 0;//смещение
        private int scroll_x = 0;
        private Pen pen_line = new Pen(Brushes.Black); // для отрисовки линий
        private Pen PenOfPoint = new Pen(Brushes.Red, 7);
        private MyTabControl SelectedTab;
        private Font RangsFont = new Font("Arial", 12); //текст для номера ранга
        private int[] Timer_control = new int[32];
        private Dictionary<string, ushort[]> Adr;
        private MyPanel panel;
        int Height, Width;
        private Regex mask = new Regex(@"(\s\S*:\S*/?\s*|\d?0.[0-9]*\s?\d?0.[0-9]*\s?\d?0.[0-9]*\s?)");
        private List<byte>BSTList = new List<byte>();
        private int StopSap = 0;

        public ClassDrow(MyPanel Panel, List<string> File, VScrollBar vScroll, 
            HScrollBar hScroll, MyTabControl MyTab, Dictionary<string, ushort[]> AdresDir,
            int height, int widht)
        {
            info_file = File;
            ElementsRang = new List<string[]>();
            AdresRang = new List<string[]>();
            info = new int[info_file.Count, 3];
            rang_y = new int[info_file.Count];
            BIGM = new List<int[]>();
            Info = new Dictionary<int, int[,]>();
            Start = new Dictionary<int, int[]>();
            Stop = new Dictionary<int, int[]>();
            PointOfElemetts = new int[CountElInRang + 1];
            VScroll = vScroll;
            HScroll = hScroll;
            SelectedTab = MyTab;
            Adr = AdresDir;
            panel = Panel;
            Height = height;
            Width = widht;
        }

        private bool Adres(string st, string mas) //выдает значение бита в массиве
        {
            try
            {
                if (mas == "") return false;
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

                    if ((adr & Bitmask) == Bitmask) return true;
                    return false;
                }
                else if (k.Contains("DN"))
                {
                    Bitmask = 2;
                    ind_1 = int.Parse(k[0]);
                    adr = Timer_control[ind_1];

                    if ((adr & Bitmask) == Bitmask) return true;
                    return false;
                }
                else if (k.Contains("TT"))
                {
                    Bitmask = 4;
                    ind_1 = int.Parse(k[0]);
                    adr = Timer_control[ind_1];

                    if ((adr & Bitmask) == Bitmask) return true;
                    return false;
                }
                else
                {
                    Bitmask = 1 << int.Parse(k[1]);

                    ind_1 = int.Parse(k[0]);
                    adr = Adr[mas][ind_1];

                    if ((adr & Bitmask) == Bitmask) return true;
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void RangsInfo()
        {
            Info.Clear();
            Start.Clear();
            Stop.Clear();
            BIGM.Clear();
            ElementsRang.Clear();
            AdresRang.Clear();
            for (int line = 0; line < info_file.Count; line++)//создание массива из элементов
            {
                string[] rang = info_file[line].Trim().Split(' ');
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
                ElementsRang.Add(element.Where(x => !string.IsNullOrEmpty(x)).ToArray());//удаление пустых значений
                AdresRang.Add(adres.Where(x => !string.IsNullOrEmpty(x)).ToArray());
            }

            for (int line = 0; line < info_file.Count; line++)
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

        private void DrowSap(PaintEventArgs e, string[] Rang, int IndexStart, int Start_Y, int StartSap, int maxCountEl)
        {
            int Index_StartBranch = IndexStart;
            Graphics g = e.Graphics;
            for (int index = StartSap+1; index < Rang.Length; index++)
            {
                string el = Rang[index];
                if (el == "BND")
                {//вертикаль
                    g.DrawLine(pen_line, left_indent_rang_x + PointOfElemetts[IndexStart+ maxCountEl+1] + scroll_x,
                        -top_indent_rang - scroll_y + Start_Y,left_indent_rang_x + PointOfElemetts[IndexStart + maxCountEl+1] + scroll_x,
                        - scroll_y + Start_Y);
                    //горизонталь
                    g.DrawLine(pen_line, left_indent_rang_x + scroll_x + PointOfElemetts[IndexStart],
                        Start_Y - scroll_y, left_indent_rang_x + scroll_x + PointOfElemetts[IndexStart + maxCountEl + 1],
                        Start_Y - scroll_y);

                    StopSap = index-1;

                    BSTList.Remove((byte)IndexStart);
                }
                else if (el == "NXB")
                {
                    //леваый спуск
                    g.DrawLine(pen_line, left_indent_rang_x + PointOfElemetts[IndexStart] + scroll_x,
                        -scroll_y + Start_Y, left_indent_rang_x + PointOfElemetts[IndexStart] + scroll_x,
                        -scroll_y + Start_Y + top_indent_rang);
                    //правый спуск
                    g.DrawLine(pen_line, left_indent_rang_x + PointOfElemetts[IndexStart + maxCountEl + 1] + scroll_x,
                        - scroll_y + Start_Y, left_indent_rang_x + PointOfElemetts[IndexStart + maxCountEl + 1] + scroll_x,
                        - scroll_y + Start_Y + top_indent_rang);
                    //горизонталь
                    g.DrawLine(pen_line, left_indent_rang_x + scroll_x + PointOfElemetts[IndexStart],
                        Start_Y - scroll_y, left_indent_rang_x + scroll_x + PointOfElemetts[IndexStart + maxCountEl + 1],
                        Start_Y - scroll_y);

                    DrowSap(e, Rang, BSTList[^1], Start_Y + top_indent_rang, index, maxCountEl);
                    index = StopSap;
                }
                else
                {
                    Index_StartBranch++;
                    if (el == "BST")
                    {
                        BSTList.Add((byte)index);
                    }
                    else
                    switch (el)
                    {
                        case "XIO":
                            {

                                g.DrawImage(XIO, new Rectangle(left_indent_rang_x + PointOfElemetts[Index_StartBranch] - 27 + scroll_x,  - scroll_y - 25 + Start_Y, 54, 50));


                                break;
                            }
                        case "XIC":
                            {

                                g.DrawImage(XIC, new Rectangle(left_indent_rang_x + PointOfElemetts[Index_StartBranch] - 27 + scroll_x,  - scroll_y - 25 + Start_Y, 54, 50));

                                break;
                            }
                        case "OTE":
                            {
                                //if (Adres(adres, mas))
                                g.DrawImage(OTE, new Rectangle(left_indent_rang_x + PointOfElemetts[Index_StartBranch] - 27 + scroll_x, -scroll_y - 25 + Start_Y, 54, 50));

                                break;
                            }
                        case "OTL":
                            {
                                //if (Adres(adres, mas))
                                g.DrawImage(OTL, new Rectangle(left_indent_rang_x + PointOfElemetts[Index_StartBranch] - 27 + scroll_x, -scroll_y - 25 + Start_Y, 54, 50));

                                break;
                            }
                        case "OTU":
                            {
                                //if (Adres(adres, mas))
                                g.DrawImage(OTU, new Rectangle(left_indent_rang_x + PointOfElemetts[Index_StartBranch] - 27 + scroll_x, -scroll_y - 25 + Start_Y, 54, 50));

                                break;
                            }
                        case "ONS":
                            {
                                g.DrawImage(OTU, new Rectangle(left_indent_rang_x + PointOfElemetts[Index_StartBranch] - 27 + scroll_x, -scroll_y - 25 + Start_Y, 54, 50));

                                break;
                            }
                        case "TON":
                            {
                                g.DrawImage(Timer_Move, new Rectangle(left_indent_rang_x + PointOfElemetts[11] - 37 + scroll_x, scroll_y - 25, 75, 50));

                                break;
                            }
                        case "MOV":
                            {
                                g.DrawImage(Timer_Move, new Rectangle(left_indent_rang_x + PointOfElemetts[11] - 37 + scroll_x, scroll_y - 25, 75, 50));

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

        private void PaintLines(PaintEventArgs e)
        {
            HScroll.Maximum = panel.Width - SelectedTab.Width + 36;
            HScroll.Minimum = 0;

            scroll_y = VScroll.Value;//прокрутка
            scroll_x = panel.Width > 1300 ? HScroll.Value = 0 : -HScroll.Value;
            Graphics g = e.Graphics;//использование графики
            for (int i = 0; i < PointOfElemetts.Length; i++)
            {
                PointOfElemetts[i] = ((panel.Width) / CountElInRang + 1) * (i + 1) - left_indent_rang_x / 2;
            }
            /*

            int maxY = top_indent_rang * (info_file.Count - 2);//макс кол-во пикселей для scroll_y
            for (int i = 0; i < info_file.Count; i++)
                maxY += ((3 * top_indent_rang) / 4) * info[i, 0];

            //точки для текста
            //PointF Scroll = new PointF(79, 50);
            //PointF MaxY = new PointF(79, 70);

            VScroll.Maximum = maxY - top_indent_rang / 2;

            //вспом. вывод информации
            //g.DrawString((TabPanel[SelectedTab.SelectedIndex][1] > 1300 ? 0 : -HScroll[SelectedTab.SelectedIndex].Value).ToString(), RangsFont, Brushes.Black, Scroll);
            //g.DrawString(SelectedTab.Width.ToString(), RangsFont, Brushes.Black, MaxY);

            //вертикаль (статична)
            g.DrawLine(pen_line, left_indent_rang_x + scroll_x, 0, left_indent_rang_x + scroll_x, maxY);
            g.DrawLine(pen_line, panel.Width - 2 + scroll_x, 0, panel.Width - 2 + scroll_x, maxY);

            PointF locationToDrawRangs = new PointF();//позиция  номера ранга
            locationToDrawRangs.X = 20 + scroll_x;
            locationToDrawRangs.Y = top_indent_rang - scroll_y - 10;

            ///отрисовка первого ранга
            g.DrawString("1", RangsFont, Brushes.Black, locationToDrawRangs);
            g.DrawLine(pen_line, left_indent_rang_x + scroll_x, top_indent_rang - scroll_y, TabPanel[SelectedTab.SelectedIndex][1] - 4 + scroll_x, top_indent_rang - scroll_y);
            for (int i = 0; i < PointOfElemetts.Length - 1; i++)
                g.DrawEllipse(PenOfPoint, left_indent_rang_x + PointOfElemetts[i] + scroll_x, top_indent_rang - scroll_y - 2, 4, 4);
            
            //верхний отступ отрисовки
            //rang_y[0] = top_step;
            int top_step = top_indent_rang;
            for (int rang = 0; rang < info_file.Count; rang++)
            {
                var BIGN = Info[rang];
                var LGN = info[rang, 1];
                int start = 0;
                int stop = 0;
                var GrupMaxCountEl = new int[LGN + 1];

                if (rang == 0)
                {
                    g.DrawString("1", RangsFont, Brushes.Black, locationToDrawRangs);
                    g.DrawLine(pen_line, left_indent_rang_x + scroll_x, top_indent_rang - scroll_y, panel.Width - 4 + scroll_x, top_indent_rang - scroll_y);
                    for (int i = 0; i < PointOfElemetts.Length - 1; i++)
                        g.DrawEllipse(PenOfPoint, left_indent_rang_x + PointOfElemetts[i] + scroll_x, top_indent_rang - scroll_y - 2, 4, 4);
                }
                else
                    top_step += top_indent_rang + (((3 * top_indent_rang) / 4) * info[rang - 1, 0]);//отступ от 0,0
                rang_y[rang] = top_step;
                locationToDrawRangs.Y = top_step - 10 - scroll_y;
                g.DrawString((rang + 1).ToString(), RangsFont, Brushes.Black, locationToDrawRangs);//номер ранга от 1 до *
                g.DrawLine(pen_line, left_indent_rang_x + scroll_x, top_step - scroll_y, panel.Width - 4, top_step - scroll_y);//основная ветка ранга от 1 до *

                for (int i = 0; i < CountElInRang + 1; i++)
                    g.DrawEllipse(PenOfPoint, left_indent_rang_x + PointOfElemetts[i] + scroll_x, top_step - scroll_y - 2, 4, 4);

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
                            left_indent_rang_x + PointOfElemetts[start + stop] + scroll_x,
                            top_step + lg * ((3 * top_indent_rang) / 4) - scroll_y,
                            left_indent_rang_x + PointOfElemetts[start + GrupMaxCountEl[lgn] + 1 + stop] + scroll_x,
                            top_step + lg * ((3 * top_indent_rang) / 4) - scroll_y);//основная ветка ранга от 1 до *
                                                                                    //вертикаль
                        g.DrawLine(pen_line,
                            left_indent_rang_x + PointOfElemetts[start + stop] + scroll_x,
                            top_step - scroll_y,
                            left_indent_rang_x + PointOfElemetts[start + stop] + scroll_x,
                            top_step + lg * ((3 * top_indent_rang) / 4) - scroll_y);//основная ветка ранга от 1 до *
                        g.DrawLine(pen_line,
                            left_indent_rang_x + PointOfElemetts[start + GrupMaxCountEl[lgn] + 1 + stop] + scroll_x,
                            top_step - scroll_y,
                            left_indent_rang_x + PointOfElemetts[start + GrupMaxCountEl[lgn] + 1 + stop] + scroll_x,
                            top_step + lg * ((3 * top_indent_rang) / 4) - scroll_y);//основная ветка ранга от 1 до *
                                                                                    //точки
                        for (int i = start + stop; i < start + GrupMaxCountEl[lgn] + 2 + stop; i++)
                            g.DrawEllipse(PenOfPoint, left_indent_rang_x + PointOfElemetts[i] + scroll_x, top_step + lg * ((3 * top_indent_rang) / 4) - scroll_y - 2, 4, 4);
                    }
                    stop = start;
                    if (0 == BIGM[rang][lgn] && stop > 0) stop++;
                }

            }
            //вспомогательная информация (выводится)
            //g.DrawString(info[15,3].ToString(), RangsFont, Brushes.Black, MaxY);*/
            //Новая отрисовка рангов \/
            g.DrawLine(pen_line, left_indent_rang_x + scroll_x, top_indent_rang - scroll_y, panel.Width - 4 + scroll_x, top_indent_rang - scroll_y);
            for (int i = 0; i < PointOfElemetts.Length - 1; i++) //
                g.DrawEllipse(PenOfPoint, left_indent_rang_x + PointOfElemetts[i] + scroll_x, top_indent_rang - scroll_y - 2, 4, 4);
            for (int rang = 0; rang < info_file.Count; rang++)
            {
                string[] rang_text = mask.Replace(info_file[rang]," ").Trim().Split(" ");
                for(int index = 0;  index < rang_text.Length; index++)
                {
                    string el = rang_text[index];
                    if (el == "BST")
                    {
                        BSTList.Add((byte)index);
                    }
                    else if (el == "NXB")
                    {
                        g.DrawLine(pen_line, left_indent_rang_x + PointOfElemetts[BSTList[^1]] + scroll_x,
                            top_indent_rang - scroll_y, left_indent_rang_x + PointOfElemetts[BSTList[^1]] + scroll_x,
                            2 * top_indent_rang - scroll_y);
                        DrowSap(e, rang_text, BSTList[^1], 2 * top_indent_rang, index, info[rang,0]);
                        index = StopSap;
                    }
                    else
                    {
                        switch (el)
                        {
                            case "XIO":
                                {
                                    
                                        g.DrawImage(XIO, new Rectangle(left_indent_rang_x + PointOfElemetts[index] - 27 + scroll_x,((4 * top_indent_rang) / 4) - scroll_y - 25, 54, 50));
                                    

                                    break;
                                }
                            case "XIC":
                                {
                                    
                                        g.DrawImage(XIC, new Rectangle(left_indent_rang_x + PointOfElemetts[index] - 27 + scroll_x, ((4 * top_indent_rang) / 4) - scroll_y - 25, 54, 50));
   
                                    break;
                                }
                            case "OTE":
                                {
                                    //if (Adres(adres, mas))
                                    g.DrawImage(OTE, new Rectangle(left_indent_rang_x + PointOfElemetts[index] - 27 + scroll_x, ((4 * top_indent_rang) / 4) - scroll_y - 25, 54, 50));
       
                                    break;
                                }
                            case "OTL":
                                {
                                    //if (Adres(adres, mas))
                                    g.DrawImage(OTL, new Rectangle(left_indent_rang_x + PointOfElemetts[index] - 27 + scroll_x, ((4 * top_indent_rang) / 4) - scroll_y - 25, 54, 50));
                 
                                    break;
                                }
                            case "OTU":
                                {
                                    //if (Adres(adres, mas))
                                    g.DrawImage(OTU, new Rectangle(left_indent_rang_x + PointOfElemetts[index] - 27 + scroll_x, ((4 * top_indent_rang) / 4) - scroll_y - 25, 54, 50));
                
                                    break;
                                }
                            case "ONS":
                                {
                                    g.DrawImage(OTU, new Rectangle(left_indent_rang_x + PointOfElemetts[index] - 27 + scroll_x, ((4 * top_indent_rang) / 4) - scroll_y - 25, 54, 50));
                    
                                    break;
                                }
                            case "TON":
                                {
                                    g.DrawImage(Timer_Move, new Rectangle(left_indent_rang_x + PointOfElemetts[11] - 37 + scroll_x,  scroll_y - 25, 75, 50));
                            
                                    break;
                                }
                            case "MOV":
                                {
                                    g.DrawImage(Timer_Move, new Rectangle(left_indent_rang_x + PointOfElemetts[11] - 37 + scroll_x, scroll_y - 25, 75, 50));
                                   
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

        private void PaintText(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            RangsInfo();
            PaintLines(e);
            return;
            for (int rang = 0; rang < info_file.Count; rang++)
            {
                int nxb = 0;
                int ind = 0;
                int num = 0;
                var mas = "";
                for (var bn = 0; bn < ElementsRang[rang].Length; bn++)
                {
                    var el = ElementsRang[rang][bn];
                    var adres = AdresRang[rang][num < AdresRang[rang].Length ? num : 0];
                    if (adres.Contains("N13"))
                    {
                        mas = "N13";
                    }
                    else if (adres.Contains("N15"))
                    {
                        mas = "N15";
                    }
                    else if (adres.Contains("N18"))
                    {
                        mas = "N18";
                    }
                    else if (adres.Contains("N40"))
                    {
                        mas = "N40";
                    }
                    else if (adres.Contains("B3"))
                    {
                        mas = "B3";
                    }
                    var point = new PointF(left_indent_rang_x + PointOfElemetts[ind] - 40 + scroll_x, rang_y[rang] + ((3 * top_indent_rang) / 4) * nxb - scroll_y - 50);
                    switch (el)
                    {
                        case "XIO":
                            {
                                if (Adres(adres, mas))
                                    g.DrawImage(XIOD, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27 + scroll_x, rang_y[rang] + ((3 * top_indent_rang) / 4) * nxb - scroll_y - 25, 54, 50));
                                else
                                    g.DrawImage(XIO, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27 + scroll_x, rang_y[rang] + ((3 * top_indent_rang) / 4) * nxb - scroll_y - 25, 54, 50));
                                g.DrawString(adres, RangsFont, Brushes.Black, point);
                                ind++;
                                num++;
                                break;
                            }
                        case "XIC":
                            {
                                if (Adres(adres, mas))
                                    g.DrawImage(XICD, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27 + scroll_x, rang_y[rang] + ((3 * top_indent_rang) / 4) * nxb - scroll_y - 25, 54, 50));
                                else
                                    g.DrawImage(XIC, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27 + scroll_x, rang_y[rang] + ((3 * top_indent_rang) / 4) * nxb - scroll_y - 25, 54, 50));
                                g.DrawString(adres, RangsFont, Brushes.Black, point);
                                ind++;
                                num++;
                                break;
                            }
                        case "OTE":
                            {
                                //if (Adres(adres, mas))
                                g.DrawImage(OTE, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27 + scroll_x, rang_y[rang] + ((3 * top_indent_rang) / 4) * nxb - scroll_y - 25, 54, 50));
                                g.DrawString(adres, RangsFont, Brushes.Black, point);
                                ind++;
                                num++;
                                break;
                            }
                        case "OTL":
                            {
                                //if (Adres(adres, mas))
                                g.DrawImage(OTL, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27 + scroll_x, rang_y[rang] + ((3 * top_indent_rang) / 4) * nxb - scroll_y - 25, 54, 50));
                                g.DrawString(adres, RangsFont, Brushes.Black, point);
                                ind++;
                                num++;
                                break;
                            }
                        case "OTU":
                            {
                                //if (Adres(adres, mas))
                                g.DrawImage(OTU, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27 + scroll_x, rang_y[rang] + ((3 * top_indent_rang) / 4) * nxb - scroll_y - 25, 54, 50));
                                g.DrawString(adres, RangsFont, Brushes.Black, point);
                                ind++;
                                num++;
                                break;
                            }
                        case "ONS":
                            {
                                g.DrawImage(OTU, new Rectangle(left_indent_rang_x + PointOfElemetts[ind] - 27+ scroll_x, rang_y[rang] + ((3*top_indent_rang )/ 4) * nxb - scroll_y - 25, 54, 50));
                                g.DrawString(adres, RangsFont, Brushes.Black, point);
                                ind++;
                                num++;
                                break;
                            }
                        case "TON":
                            {
                                g.DrawImage(Timer_Move, new Rectangle(left_indent_rang_x + PointOfElemetts[11] - 37 + scroll_x, rang_y[rang] - scroll_y - 25, 75, 50));
                                ind++;
                                break;
                            }
                        case "MOV":
                            {
                                g.DrawImage(Timer_Move, new Rectangle(left_indent_rang_x + PointOfElemetts[11] - 37 + scroll_x, rang_y[rang] - scroll_y - 25, 75, 50));
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

        public async void StartDrow()
        {
            panel.Paint += PaintText;
            while(true)
            {
                await Task.Delay(60);
                panel.Refresh();
                panel.Height = Height - 20;

                if (panel.Height * info_file.Count / 150 >= 150)
                {
                    top_indent_rang = (panel.Height * info_file.Count) / 150;
                }
                else top_indent_rang = 150;

                if (Width - 50 >= 1300)
                {
                    panel.Width = Width - 50;
                    if (HScroll.Visible)
                        HScroll.Visible = false;
                }
                else
                {
                    panel.Width = 1300;
                    if (!HScroll.Visible)
                        HScroll.Visible = true;
                }
                Width = panel.Width;
            }
        }
    }
}
