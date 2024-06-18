using Microsoft.VisualBasic.Devices;
using System.Text.RegularExpressions;

namespace LogixForms
{
    /// <summary>
    /// Отрисовка программы
    /// </summary>
    public class ClassDrow
    {
        private Bitmap XIC = NodEn.XIC, XIO = NodEn.XIO, Timer_Move = NodEn.Timer___Move, EnDnTt = NodEn.EN_DN_TT, OTU = NodEn.OTU,
            OTE = NodEn.OTE, OTL = NodEn.OTL, XICD = NodDis.XICdis, XIOD = NodDis.XIOdis; // загрузка изображений
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
        private MyTabControl SelectedTab;
        private Font RangsFont = new Font("Arial", 12); //текст для номера ранга
        private int[] Timer_control = new int[32];
        private Dictionary<string, ushort[]> Adr;
        private MyPanel panel;
        int Height, Width;

        /// <summary>
        /// Конструктор отрисовки
        /// </summary>
        /// <param name="Panel">Панель, на которой требуется отрисовк программы</param>
        /// <param name="File">Программа</param>
        /// <param name="vScroll">Ссылка на вертикальный ползунок</param>
        /// <param name="hScroll">Ссылка на горизонтальный ползунок</param>
        /// <param name="MyTab">Окно отображения</param>
        /// <param name="AdresDir">Список адресов</param>
        /// <param name="height">Высота</param>
        /// <param name="widht">Ширина</param>
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

        /*private void RangsInfo()
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
        }*/

        /// <summary>
        /// Отрисовка программы
        /// </summary>
        /// <param name="e"></param>
        private void Draw(PaintEventArgs e)
        {
            HScroll.Maximum = panel.Width - SelectedTab.Width + 36;
            HScroll.Minimum = 0;

            scroll_y = VScroll.Value;//прокрутка
            scroll_x = panel.Width > 1300 ? HScroll.Value = 0 : -HScroll.Value;
            Graphics g = e.Graphics;//использование графики
            int y = 0;
            Rang rang;
            ushort count_rangs = 1;
            foreach (string str in info_file)
            {
                rang = new Rang(g, ref scroll_y, ref scroll_x, y, count_rangs, Adr);
                rang.Draw(str);
                y = rang.Max;
                count_rangs++;
            }
            rang = null;
            VScroll.Maximum = y - panel.Height+60>0? y - panel.Height + 60: 0;
            //g.DrawString($"scroll: {VScroll.Value} MaxScroll: {VScroll.Maximum}", RangsFont, Brushes.Red, 300, 200);
        }

        /// <summary>
        /// Нанесение адресов и имен
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PaintText(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            //RangsInfo();
            Draw(e);
            return;
            /*for (int rang = 0; rang < info_file.Count; rang++)
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
            }*/
        }

        /// <summary>
        /// Вызов отрисовк
        /// </summary>
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
