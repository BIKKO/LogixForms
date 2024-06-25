using System;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LogixForms
{
    /// <summary>
    /// Отрисовка рангов
    /// </summary>
    internal class Rang
    {
        private readonly Bitmap XIC = NodEn.XIC, XIO = NodEn.XIO, Timer_Move = NodEn.Timer___Move, EnDnTt = NodEn.EN_DN_TT, OTU = NodEn.OTU,
            OTE = NodEn.OTE, OTL = NodEn.OTL, XICD = NodDis.XICdis, XIOD = NodDis.XIOdis; // загрузка изображений
        private readonly Pen pen_line = new Pen(Brushes.Blue);
        private readonly Pen PenOfPoint = new Pen(Brushes.Yellow, 7);
        private readonly Regex mask = new Regex(@"((\s?[A-Z]\d?\d?\d?:\d?\d?\d?)(/\b?\b?)?\s?)|[0.0-9999.0]");// Выделение мномоник
        private readonly Regex maskAdr = new Regex(@"\s?([A-Z]{3})\s?");// Выделение Адрес
        private const int left_indent_rang_x = 70;
        private const int top_indent_rang = 150;
        private int startY;
        private int scrollY;
        private int scrollX;
        private int[] PointOfElemetts = new int[14];
        private Graphics g;
        private const byte top_indent = 100;
        private int MaxYRangs;
        private int MaxYBranch;
        private ushort Number;
        private Dictionary<string, ushort[]> Adr;
        Dictionary<string, string[]> Tegs;
        private int[] Timer_control = new int[32];
        private string[] TextRang;

        /// <summary>
        /// Инициализация конструктора ранга
        /// </summary>
        /// <param name="graf">Отображение графики</param>
        /// <param name="scrollY">Вертикальная прокрутка</param>
        /// <param name="scrollX">Горизонтальная прокрутка</param>
        /// <param name="start">Старт по координате Y</param>
        /// <param name="num">Номер ранга</param>
        /// <param name="AdresDir">Список адресов</param>
        public Rang(Graphics graf, ref int scrollY, ref int scrollX, int start, ushort num, ref Dictionary<string, ushort[]> AdresDir)
        {
            Adr = AdresDir;
            Number = num;
            this.scrollY = scrollY;
            this.scrollX = scrollX;
            startY = start;
            g = graf;
            MaxYRangs = top_indent_rang + startY;
            MaxYBranch = 0;

            ReadyStart();
        }


        public Rang(Graphics graf, ref int scrollY, ref int scrollX, int start, ushort num, ref Dictionary<string, ushort[]> AdresDir, Dictionary<string, string[]> _Tegs)
        {
            Tegs = _Tegs;
            Adr = AdresDir;
            Number = num;
            this.scrollY = scrollY;
            this.scrollX = scrollX;
            startY = start;
            g = graf;
            MaxYRangs = top_indent_rang + startY;
            MaxYBranch = 0;

            ReadyStart();
        }

        /// <summary>
        /// Подготовка необходимых компонентов
        /// </summary>
        private void ReadyStart()
        {
            for (int i = 0; i < PointOfElemetts.Length; i++)
            {
                PointOfElemetts[i] = 1300 / 14 * (i + 1) - left_indent_rang_x / 2;
            }

            g.DrawLine(pen_line, left_indent_rang_x + scrollX, top_indent_rang - scrollY + startY, 1300 - 4 + scrollX, top_indent_rang - scrollY + startY);
            for (int i = 0; i < PointOfElemetts.Length - 1; i++)
                g.DrawEllipse(PenOfPoint, left_indent_rang_x + PointOfElemetts[i] + scrollX, top_indent_rang - scrollY - 2 + startY, 4, 4);
        }

        /// <summary>
        /// Максимальный Y ранга
        /// </summary>
        public int Max { get { return Math.Max(MaxYRangs, MaxYBranch); } }

        /// <summary>
        /// Отрисовка ранга
        /// </summary>
        /// <param name="RangText">Текст ранга</param>
        public void Draw(string RangText)
        {
            DrawLine(RangText);
            DrawElem(RangText);
        }

        /// <summary>
        /// Проверка активноти элемента
        /// </summary>
        /// <param name="st">Адрес</param>
        /// <param name="mas">Название</param>
        /// <returns>Активность</returns>
        private bool Adres(string st)
        {
            try
            {
                string mas = new Regex(@":\w*(/(\w*)?)?").Replace(st, "");
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

        /// <summary>
        /// Отображение основной ветви ранга
        /// </summary>
        /// <param name="RangText">Тект ранга</param>
        private void DrawLine(string RangText)
        {
            int count_of_branch = 0;
            Point p = new Point();
            bool BranchStart = false;
            Branch branch = new Branch();
            string[] rang_text = mask.Replace(RangText, " ").Trim().Split(' ').Where(a => a != "" && a != "DN" && a != "EN" && a != "TT").ToArray();
            int x_branch = 0;
            int drow_ind = 0;
            for (int index = 0; index < rang_text.Length; index++)
            {
                string el = rang_text[index];
                if (el == "BST")
                {
                    p = new Point(left_indent_rang_x + PointOfElemetts[drow_ind], startY + top_indent_rang);
                    x_branch = drow_ind + 1;
                    branch = new Branch(g, p, ref scrollY, ref scrollX);
                    BranchStart = true;
                    count_of_branch++;
                }
                else if (el == "NXB")
                {
                    int[] inf = DrowSap(rang_text, index + 1, p.Y + top_indent, x_branch, p, count_of_branch, branch.Count);
                    index = inf[0];
                    branch.ClearBranch();
                    if (branch < inf[1]) branch = branch - branch + Math.Max(branch.Count, inf[1]);
                    branch += inf[4];
                    branch.DrowBranch();
                    BranchStart = Convert.ToBoolean(inf[2]);
                    drow_ind = Math.Max(drow_ind + 1, inf[3]);
                    count_of_branch = 0;
                    continue;
                }
                else
                {
                    if (BranchStart)
                    {
                        branch.ClearBranch();
                        branch++;
                        branch.DrowBranch();
                    }
                }
                drow_ind++;
            }

            g.DrawLine(pen_line, left_indent_rang_x + scrollX, startY - scrollY - 50, left_indent_rang_x + scrollX, Max - scrollY);
            g.DrawString(Number.ToString(), new Font("Arial", 12), Brushes.DimGray, (int)(left_indent_rang_x * .4) + scrollX, startY + top_indent_rang - 10 - scrollY);
        }

        /// <summary>
        /// Метод для рекурсивного отображения рангов
        /// </summary>
        /// <param name="rang_text">Массив мномоник ранга</param>
        /// <param name="IndexStart">Индекс, проболжения перебора массива рангов</param>
        /// <param name="Start_Y">Y координата горизонта ветви</param>
        /// <param name="Start_X">X координата горизонта ветви</param>
        /// <param name="point">Точка отрисовки ветви</param>
        /// <param name="CountOfBranch">Кол-во ветвей</param>
        /// <param name="CountInBranch">Кол-во эл в ветве</param>
        /// <returns>Индек массива, кол-во эл в ветви, -, индекс остановки в точках, кол-во веток - 1</returns>
        /// <exception cref="Не найден конец ветви">г</exception>
        private int[] DrowSap(string[] rang_text, int IndexStart, int Start_Y, int Start_X, Point point, int CountOfBranch, int CountInBranch)
        {
            int count_of_branch = CountOfBranch;
            Point p = point;
            bool BranchStart = false;
            Branch branch = new Branch(g, p, ref scrollY, ref scrollX, (short)CountInBranch);
            int x_branch = Start_X;
            int drow_ind = Start_X;
            int count_el_in_branch = 0;
            for (int index = IndexStart; index < rang_text.Length; index++)
            {
                string el = rang_text[index];
                if (el == "BST")
                {//добавить отрисовку влженных ветвей через кол-во переносов во вложенной ветке
                    p = new Point(left_indent_rang_x + PointOfElemetts[drow_ind], Start_Y);
                    x_branch = drow_ind + 1;
                    branch = new Branch(g, p, ref scrollY, ref scrollX);
                    BranchStart = true;
                    count_of_branch++;
                }
                else if (el == "NXB")
                {
                    p.Y += top_indent;
                    int[] inf;
                    if (count_of_branch < 2)//одиночная ветвь с глубоким переносом
                    {
                        inf = DrowSap(rang_text, index + 1, Start_Y + top_indent, x_branch, p, count_of_branch, CountInBranch);
                        branch = new Branch(g, p, ref scrollY, ref scrollX, (short)CountInBranch);
                        count_of_branch--;
                        index = inf[0];
                        branch.ClearBranch();
                        if (branch < inf[1]) branch = branch - branch + Math.Max(branch.Count, inf[1]);
                        branch.DrowBranch();
                        BranchStart = Convert.ToBoolean(inf[2]);
                        drow_ind = Math.Max(drow_ind + 1, inf[3]);
                        if (count_of_branch == inf[4]) return inf;
                        continue;
                    }
                    else//ветвь на ветви
                    {
                        inf = DrowSap(rang_text, index + 1, Start_Y + top_indent, x_branch, p, count_of_branch, 1);
                        p.Y -= top_indent;
                        branch = new Branch(g, p, ref scrollY, ref scrollX);
                        count_of_branch--;
                        index = inf[0];
                        branch.ClearBranch();
                        if (branch < inf[1]) branch = branch - branch + Math.Max(branch.Count, inf[1]);
                        branch.DrowBranch();
                        BranchStart = Convert.ToBoolean(inf[2]);
                        drow_ind = Math.Max(drow_ind+1, inf[3]);
                        if (count_of_branch == inf[4]) return inf;
                        continue;
                    }
                }
                else if(el == "BND")
                {
                    int ind3 = drow_ind;
                    MaxYBranch = Math.Max(MaxYBranch, Start_Y);
                    return new int[] { index, count_el_in_branch, 0, ind3, count_of_branch-1};
                }
                else
                {
                    count_el_in_branch++;
                    if (BranchStart)
                    {
                        branch.ClearBranch();
                        branch++;
                        branch.DrowBranch();
                    }
                }
                drow_ind++;
            }
            //return new int[] { rang_text.Length, count_el_in_branch, 0, drow_ind, count_of_branch - 1 };
            throw new Exception("Not met BND");
        }

        /// <summary>
        /// Отрисовка элементов рнга
        /// </summary>
        /// <param name="RangText">Текст ранга</param>
        private void DrawElem(string RangText)
        {
            string[] adress = maskAdr.Replace(RangText, " ").Trim().Split(' ').Where(a => a != "" && a != "DN" && a != "EN" && a != "TT").ToArray();
            double u;
            TextRang = adress.Where(x => !double.TryParse(new Regex(@"(:\d*.?\d*)").Replace(x, "").Replace('.', ','), out u)).ToArray();

            int enumer_el = 0;
            int count_of_branch = 0;
            Point p = new Point();
            bool BranchStart = false;
            int branch = 0;
            string[] rang_text = mask.Replace(RangText, " ").Trim().Split(' ').Where(a => a != "" && a != "DN" && a != "EN" && a != "TT").ToArray();
            int x_branch = 0;
            int drow_ind = 0;
            for (int index = 0; index < rang_text.Length; index++)
            {
                string el = rang_text[index];
                if (el == "BST")
                {
                    p = new Point(left_indent_rang_x + PointOfElemetts[drow_ind], startY + top_indent_rang);
                    x_branch = drow_ind + 1;
                    branch = 0;
                    BranchStart = true;
                    count_of_branch++;
                }
                else if (el == "NXB")
                {
                    int[] inf = DrawElemInSap(rang_text, TextRang, enumer_el, index + 1, p.Y + top_indent, x_branch, p, count_of_branch, branch);
                    enumer_el = inf[2];
                    index = inf[0];
                    if (branch < inf[1]) branch = branch - branch + Math.Max(branch, inf[1]);
                    branch += inf[4];
                    BranchStart = Convert.ToBoolean(inf[2]);
                    drow_ind = Math.Max(drow_ind + 1, inf[3]);
                    count_of_branch = 0;
                    continue;
                }
                else
                {
                    if (BranchStart) branch++;
                    switch (el)
                    {
                        case "XIO":
                            {
                                if (!Adres(TextRang[enumer_el]))
                                    g.DrawImage(XIOD, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 54, 50));
                                else
                                    g.DrawImage(XIO, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 54, 50));

                                //g.DrawImage(XIO, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 54, 50));
                                //g.DrawString($"y: {((4 * top_indent_rang) / 4) + startY - 25}", new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX + 20, ((4 * top_indent_rang) / 4) + startY - scrollY - 25);
                                g.DrawString(TextRang[enumer_el], new Font("Arial", 10), Brushes.Black, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 40);
                                if(Tegs != null)
                                    if (Tegs.ContainsKey(TextRang[enumer_el]))
                                    {
                                        g.DrawString(Tegs[TextRang[enumer_el]][0], new Font("Arial", 10), Brushes.Blue, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 60);
                                        g.DrawString(Tegs[TextRang[enumer_el]][1], new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 78);
                                    }
                                enumer_el++;
                                break;
                            }
                        case "XIC":
                            {
                                if (Adres(TextRang[enumer_el]))
                                    g.DrawImage(XICD, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 54, 50));
                                else
                                    g.DrawImage(XIC, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 54, 50));
                                //g.DrawImage(XIC, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 54, 50));
                                //g.DrawString($"y: {((4 * top_indent_rang) / 4) + startY - 25}", new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX + 20, ((4 * top_indent_rang) / 4) + startY - scrollY - 25);
                                g.DrawString(TextRang[enumer_el], new Font("Arial", 10), Brushes.Black, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 40);
                                if (Tegs != null)
                                    if (Tegs.ContainsKey(TextRang[enumer_el]))
                                    {
                                        g.DrawString(Tegs[TextRang[enumer_el]][0], new Font("Arial", 10), Brushes.Blue, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 60);
                                        g.DrawString(Tegs[TextRang[enumer_el]][1], new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 78);
                                    }
                                enumer_el++;
                                break;
                            }
                        case "OTE":
                            {
                                //if (Adres(TextRang[enumer_el]))
                                g.DrawImage(OTE, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 54, 50));
                                g.DrawString(TextRang[enumer_el], new Font("Arial", 10), Brushes.Black, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 40);
                                if (Tegs != null)
                                    if (Tegs.ContainsKey(TextRang[enumer_el]))
                                    {
                                        g.DrawString(Tegs[TextRang[enumer_el]][0], new Font("Arial", 10), Brushes.Blue, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 60);
                                        g.DrawString(Tegs[TextRang[enumer_el]][1], new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 78);
                                    }
                                enumer_el++;
                                //g.DrawString($"y: {((4 * top_indent_rang) / 4) + startY - 25}", new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX + 20, ((4 * top_indent_rang) / 4) + startY - scrollY - 25);
                                break;
                            }
                        case "OTL":
                            {
                                //if (Adres(TextRang[enumer_el]))
                                g.DrawImage(OTL, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 54, 50));
                                g.DrawString(TextRang[enumer_el], new Font("Arial", 10), Brushes.Black, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 40);
                                if (Tegs != null)
                                    if (Tegs.ContainsKey(TextRang[enumer_el]))
                                    {
                                        g.DrawString(Tegs[TextRang[enumer_el]][0], new Font("Arial", 10), Brushes.Blue, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 60);
                                        g.DrawString(Tegs[TextRang[enumer_el]][1], new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 78);
                                    }
                                enumer_el++;
                                //g.DrawString($"y: {((4 * top_indent_rang) / 4) + startY - 25}", new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX + 20, ((4 * top_indent_rang) / 4) + startY - scrollY - 25);
                                break;
                            }
                        case "OTU":
                            {
                                //if (Adres(TextRang[enumer_el]))
                                g.DrawImage(OTU, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 54, 50));
                                g.DrawString(TextRang[enumer_el], new Font("Arial", 10), Brushes.Black, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 40);
                                if (Tegs != null)
                                    if (Tegs.ContainsKey(TextRang[enumer_el]))
                                    {
                                        g.DrawString(Tegs[TextRang[enumer_el]][0], new Font("Arial", 10), Brushes.Blue, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 60);
                                        g.DrawString(Tegs[TextRang[enumer_el]][1], new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 78);
                                    }
                                enumer_el++;//g.DrawString($"y: {((4 * top_indent_rang) / 4) + startY - 25}", new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX + 20, ((4 * top_indent_rang) / 4) + startY - scrollY - 25);
                                break;
                            }
                        case "ONS":
                            {
                                g.DrawImage(OTU, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 54, 50));
                                //g.DrawString($"y: {((4 * top_indent_rang) / 4) + startY - 25}", new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX + 20, ((4 * top_indent_rang) / 4) + startY - scrollY - 25);
                                g.DrawString(TextRang[enumer_el], new Font("Arial", 10), Brushes.Black, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 40);
                                if (Tegs != null)
                                    if (Tegs.ContainsKey(TextRang[enumer_el]))
                                    {
                                        g.DrawString(Tegs[TextRang[enumer_el]][0], new Font("Arial", 10), Brushes.Blue, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 60);
                                        g.DrawString(Tegs[TextRang[enumer_el]][1], new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 78);
                                    }
                                enumer_el++;
                                break;
                            }
                        case "TON":
                            {
                                g.DrawImage(Timer_Move, new Rectangle(left_indent_rang_x + PointOfElemetts[index == rang_text.Length - 1 ? 12 : drow_ind] - 37 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 75, 50));
                                g.DrawString(TextRang[enumer_el], new Font("Arial", 10), Brushes.Black, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 40);
                                if (Tegs != null)
                                    if (Tegs.ContainsKey(TextRang[enumer_el]))
                                    {
                                        g.DrawString(Tegs[TextRang[enumer_el]][0], new Font("Arial", 10), Brushes.Blue, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 60);
                                        g.DrawString(Tegs[TextRang[enumer_el]][1], new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 78);
                                    }
                                enumer_el++;
                                break;
                            }
                        case "MOV":
                            {
                                g.DrawImage(Timer_Move, new Rectangle(left_indent_rang_x + PointOfElemetts[index == rang_text.Length - 1 ? 12 : drow_ind] - 37 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 75, 50));
                                g.DrawString(TextRang[enumer_el], new Font("Arial", 10), Brushes.Black, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 40);
                                if (Tegs != null)
                                    if (Tegs.ContainsKey(TextRang[enumer_el]))
                                    {
                                        g.DrawString(Tegs[TextRang[enumer_el]][0], new Font("Arial", 10), Brushes.Blue, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 60);
                                        g.DrawString(Tegs[TextRang[enumer_el]][1], new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, ((4 * top_indent_rang) / 4) + startY - scrollY - 78);
                                    }
                                enumer_el++;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
                drow_ind++;
            }
        }

        /// <summary>
        /// Рекурсиный метод отрисовки элементов ветвей ранга
        /// </summary>
        /// <param name="rang_text">Массив элементов</param>
        /// <param name="IndexStart">Старт перебора массива</param>
        /// <param name="Start_Y">Старт отрисовки по Y</param>
        /// <param name="Start_X">Старт отрисовки по X</param>
        /// <param name="point">Точка старта ветви</param>
        /// <param name="CountOfBranch">Кол-во ветвей</param>
        /// <param name="CountInBranch">Кол-во элеменов в ветви</param>
        /// <returns>индекс конца перебора, кол-во эл в полученной ветви, ~, точка отрисовки, кол-во ветвей - 1</returns>
        /// <exception cref="Не найден конец ветви"></exception>
        private int[] DrawElemInSap(string[] rang_text, string[] adres_text,int enumel, int IndexStart, int Start_Y, int Start_X, Point point, int CountOfBranch, int CountInBranch)
        {
            int enum_el = enumel;
            int count_of_branch = CountOfBranch;
            Point p = point;
            bool BranchStart = false;
            int branch = 0;
            int x_branch = Start_X;
            int drow_ind = Start_X;
            int count_el_in_branch = 0;
            for (int index = IndexStart; index < rang_text.Length; index++)
            {
                string el = rang_text[index];
                if (el == "BST")
                {
                    p = new Point(left_indent_rang_x + PointOfElemetts[drow_ind], Start_Y);
                    x_branch = drow_ind + 1;
                    branch = 0;
                    BranchStart = true;
                    count_of_branch++;
                }
                else if (el == "NXB")
                {

                    p.Y += top_indent;
                    int[] inf;
                    if (count_of_branch < 2)
                    {
                        inf = DrawElemInSap(rang_text, adres_text, enum_el, index + 1, Start_Y + top_indent, x_branch, p, count_of_branch, CountInBranch);
                        enum_el = inf[2];
                        branch = CountInBranch;
                        count_of_branch--;
                        index = inf[0];
                        if (branch < inf[1]) branch = branch - branch + Math.Max(branch, inf[1]);
                        BranchStart = Convert.ToBoolean(inf[2]);
                        drow_ind = Math.Max(drow_ind + 1, inf[3]);
                        if (count_of_branch == inf[4]) return inf;
                        continue;
                    }
                    else
                    {
                        inf = DrawElemInSap(rang_text, adres_text, enum_el, index + 1, Start_Y + top_indent, x_branch, p, count_of_branch, 1);
                        enum_el = inf[2];
                        p.Y -= top_indent;
                        branch = 0;
                        count_of_branch--;
                        index = inf[0];
                        if (branch < inf[1]) branch = branch - branch + Math.Max(branch, inf[1]);
                        BranchStart = Convert.ToBoolean(inf[2]);
                        drow_ind = Math.Max(drow_ind + 1, inf[3]);
                        if (count_of_branch == inf[4]) return inf;
                        continue;
                    }
                }
                else if (el == "BND")
                {
                    int ind3 = drow_ind;
                    MaxYBranch = Math.Max(MaxYBranch, Start_Y);
                    return new int[] { index, count_el_in_branch, enum_el, ind3, count_of_branch - 1 };
                }
                else
                {
                    count_el_in_branch++;
                    if (BranchStart) branch++;
                    switch (el)
                    {
                        case "XIO":
                            {
                                if (!Adres(TextRang[enum_el]))
                                    g.DrawImage(XIOD, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, Start_Y - 25 - scrollY, 54, 50));
                                else
                                    g.DrawImage(XIO, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, Start_Y - 25 - scrollY, 54, 50));
                                g.DrawString(TextRang[enum_el], new Font("Arial", 10), Brushes.Black, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX-35, Start_Y - scrollY-40);
                                if (Tegs != null)
                                    if (Tegs.ContainsKey(TextRang[enum_el]))
                                    {
                                        g.DrawString(Tegs[TextRang[enum_el]][0], new Font("Arial", 10), Brushes.Blue, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, Start_Y - scrollY - 60);
                                        g.DrawString(Tegs[TextRang[enum_el]][1], new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35,  Start_Y - scrollY - 78);
                                    }
                                enum_el++;
                                //g.DrawString($"y: {Start_Y - 25}", new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX + 20, Start_Y - 25 - scrollY);

                                break;
                            }
                        case "XIC":
                            {
                                if (Adres(TextRang[enum_el]))
                                    g.DrawImage(XICD, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, Start_Y - 25 - scrollY, 54, 50));
                                else
                                    g.DrawImage(XIC, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, Start_Y - 25 - scrollY, 54, 50));
                                g.DrawString(TextRang[enum_el], new Font("Arial", 10), Brushes.Black, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX-35, Start_Y - scrollY-40);
                                if (Tegs != null)
                                    if (Tegs.ContainsKey(TextRang[enum_el]))
                                    {
                                        g.DrawString(Tegs[TextRang[enum_el]][0], new Font("Arial", 10), Brushes.Blue, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35,  Start_Y - scrollY - 60);
                                        g.DrawString(Tegs[TextRang[enum_el]][1], new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35,  Start_Y - scrollY - 78);
                                    }
                                enum_el++;
                                //g.DrawString($"y: {Start_Y - 25}", new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX + 20, Start_Y - 25 - scrollY);
                                break;
                            }
                        case "OTE":
                            {
                                //if (Adres(adres, mas))
                                g.DrawImage(OTE, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, Start_Y - 25 - scrollY, 54, 50));
                                //g.DrawString($"y: {Start_Y - 25}", new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX + 20, Start_Y - 25 - scrollY);
                                g.DrawString(TextRang[enum_el], new Font("Arial", 10), Brushes.Black, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, Start_Y - scrollY-40);
                                if (Tegs != null)
                                    if (Tegs.ContainsKey(TextRang[enum_el]))
                                    {
                                        g.DrawString(Tegs[TextRang[enum_el]][0], new Font("Arial", 10), Brushes.Blue, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35,  Start_Y - scrollY - 60);
                                        g.DrawString(Tegs[TextRang[enum_el]][1], new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35,  Start_Y - scrollY - 78);
                                    }
                                enum_el++;
                                break;
                            }
                        case "OTL":
                            {
                                //if (Adres(adres, mas))
                                g.DrawImage(OTL, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, Start_Y - 25 - scrollY, 54, 50));
                                //g.DrawString($"y: {Start_Y - 25}", new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX + 20, Start_Y - 25 - scrollY);
                                g.DrawString(TextRang[enum_el], new Font("Arial", 10), Brushes.Black, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, Start_Y - scrollY-40);
                                if (Tegs != null)
                                    if (Tegs.ContainsKey(TextRang[enum_el]))
                                    {
                                        g.DrawString(Tegs[TextRang[enum_el]][0], new Font("Arial", 10), Brushes.Blue, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35,  Start_Y - scrollY - 60);
                                        g.DrawString(Tegs[TextRang[enum_el]][1], new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35,  Start_Y - scrollY - 78);
                                    }
                                enum_el++;
                                break;
                            }
                        case "OTU":
                            {
                                //if (Adres(adres, mas))
                                g.DrawImage(OTU, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, Start_Y - 25 - scrollY, 54, 50));
                                //g.DrawString($"y: {Start_Y - 25}", new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX + 20, Start_Y - 25 - scrollY);
                                g.DrawString(TextRang[enum_el], new Font("Arial", 10), Brushes.Black, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, Start_Y - scrollY-40);
                                if (Tegs != null)
                                    if (Tegs.ContainsKey(TextRang[enum_el]))
                                    {
                                        g.DrawString(Tegs[TextRang[enum_el]][0], new Font("Arial", 10), Brushes.Blue, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35,  Start_Y - scrollY - 60);
                                        g.DrawString(Tegs[TextRang[enum_el]][1], new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35,  Start_Y - scrollY - 78);
                                    }
                                enum_el++;
                                break;
                            }
                        case "ONS":
                            {
                                g.DrawImage(OTU, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, Start_Y - 25 - scrollY, 54, 50));
                                //g.DrawString($"y: {Start_Y - 25}", new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX + 20, Start_Y - 25 - scrollY);
                                g.DrawString(TextRang[enum_el], new Font("Arial", 10), Brushes.Black, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, Start_Y - scrollY-40);
                                if (Tegs != null)
                                    if (Tegs.ContainsKey(TextRang[enum_el]))
                                    {
                                        g.DrawString(Tegs[TextRang[enum_el]][0], new Font("Arial", 10), Brushes.Blue, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35,  Start_Y - scrollY - 60);
                                        g.DrawString(Tegs[TextRang[enum_el]][1], new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35,  Start_Y - scrollY - 78);
                                    }
                                enum_el++;
                                break;
                            }
                        case "TON":
                            {
                                g.DrawImage(Timer_Move, new Rectangle(left_indent_rang_x + PointOfElemetts[index == rang_text.Length - 1 ? 12 : drow_ind] - 37 + scrollX, Start_Y - 25 - scrollY, 75, 50));
                                g.DrawString(TextRang[enum_el], new Font("Arial", 10), Brushes.Black, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, Start_Y - scrollY-40);
                                if (Tegs != null)
                                    if (Tegs.ContainsKey(TextRang[enum_el]))
                                    {
                                        g.DrawString(Tegs[TextRang[enum_el]][0], new Font("Arial", 10), Brushes.Blue, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35,  Start_Y - scrollY - 60);
                                        g.DrawString(Tegs[TextRang[enum_el]][1], new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35,  Start_Y - scrollY - 78);
                                    }
                                enum_el++;
                                break;
                            }
                        case "MOV":
                            {
                                g.DrawImage(Timer_Move, new Rectangle(left_indent_rang_x + PointOfElemetts[index == rang_text.Length - 1 ? 12 : drow_ind] - 37 + scrollX, Start_Y - 25 - scrollY, 75, 50));
                                g.DrawString(TextRang[enum_el], new Font("Arial", 10), Brushes.Black, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35, Start_Y - scrollY-40);
                                if (Tegs != null)
                                    if (Tegs.ContainsKey(TextRang[enum_el]))
                                    {
                                        g.DrawString(Tegs[TextRang[enum_el]][0], new Font("Arial", 10), Brushes.Blue, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35,  Start_Y - scrollY - 60);
                                        g.DrawString(Tegs[TextRang[enum_el]][1], new Font("Arial", 10), Brushes.Red, left_indent_rang_x + PointOfElemetts[drow_ind] + scrollX - 35,  Start_Y - scrollY - 78);
                                    }
                                enum_el++;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
                drow_ind++;
            }
            //return new int[] { rang_text.Length, count_el_in_branch, 0, drow_ind, count_of_branch - 1 };
            throw new Exception("Not met BND");
        }

        /// <summary>
        /// Уничтожение экземпляра данного класса
        /// </summary>
        public void Dispose()
        {
            Adr.Clear();
            Adr = null;
            Tegs.Clear();
            Tegs = null;
        }
    }
}
