using System.Text.RegularExpressions;

namespace LogixForms
{
    internal class Rang
    {
        private Bitmap XIC = NodEn.XIC, XIO = NodEn.XIO, Timer_Move = NodEn.Timer___Move, EnDnTt = NodEn.EN_DN_TT, OTU = NodEn.OTU,
            OTE = NodEn.OTE, OTL = NodEn.OTE, XICD = NodDis.XICdis, XIOD = NodDis.XIOdis; // загрузка изображений
        private Pen pen_line = new Pen(Brushes.Blue);
        private Pen PenOfPoint = new Pen(Brushes.Yellow, 7);
        private Regex mask = new Regex(@"(\s\S*:\S*/?\s*|\d?0.[0-9]*\s?\d?0.[0-9]*\s?\d?0.[0-9]*\s?)");
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


        public Rang(Graphics graf, ref int scrollY, ref int scrollX, int start)
        {
            this.scrollY = scrollY;
            this.scrollX = scrollX;
            startY = start;
            g = graf;
            MaxYRangs = top_indent_rang + startY;
            MaxYBranch = 0;

            for (int i = 0; i < PointOfElemetts.Length; i++)
            {
                PointOfElemetts[i] = 1300 / 14 * (i + 1) - left_indent_rang_x / 2;
            }

            g.DrawLine(pen_line, left_indent_rang_x + scrollX, top_indent_rang - scrollY + startY, 1300 - 4 + scrollX, top_indent_rang - scrollY + startY);
            for (int i = 0; i < PointOfElemetts.Length - 1; i++) //
                g.DrawEllipse(PenOfPoint, left_indent_rang_x + PointOfElemetts[i] + scrollX, top_indent_rang - scrollY - 2 + startY, 4, 4);
        }

        public int Max { get { return Math.Max(MaxYRangs, MaxYBranch); } }

        public void Draw(string RangText)
        {
            int count_of_branch = 0;
            Point p = new Point();
            bool BranchStart = false;
            Branch branch = new Branch();
            string[] rang_text = mask.Replace(RangText, " ").Trim().Split(" ");
            int x_branch = 0;
            int drow_ind = 0;
            for (int index = 0; index < rang_text.Length; index++)
            {
                string el = rang_text[index];
                if (el == "BST")
                {
                    p = new Point(left_indent_rang_x + PointOfElemetts[drow_ind], startY + top_indent_rang);
                    x_branch = drow_ind + 1;
                    branch = new Branch(g, p);
                    BranchStart = true;
                    count_of_branch++;
                }
                else if (el == "NXB")
                {
                    int [] inf = DrowSap(rang_text, index + 1, p.Y + top_indent, x_branch, p, count_of_branch, branch.Count);
                    index = inf[0];
                    branch.Update();
                    if (branch < inf[1]) branch = branch - branch + Math.Max(branch.Count, inf[1]);
                    branch += inf[4];
                    branch.DrowBranch();
                    BranchStart = Convert.ToBoolean(inf[2]);
                    drow_ind = Math.Max(drow_ind+1, inf[3]);
                    count_of_branch = 0;
                    continue;
                }
                else
                {
                    if (BranchStart)
                    {
                        branch.Update();
                        branch++;
                        branch.DrowBranch();
                    }
                    switch (el)
                    {
                        case "XIO":
                            {

                                g.DrawImage(XIO, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 54, 50));


                                break;
                            }
                        case "XIC":
                            {

                                g.DrawImage(XIC, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 54, 50));

                                break;
                            }
                        case "OTE":
                            {
                                //if (Adres(adres, mas))
                                g.DrawImage(OTE, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 54, 50));

                                break;
                            }
                        case "OTL":
                            {
                                //if (Adres(adres, mas))
                                g.DrawImage(OTL, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 54, 50));

                                break;
                            }
                        case "OTU":
                            {
                                //if (Adres(adres, mas))
                                g.DrawImage(OTU, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 54, 50));

                                break;
                            }
                        case "ONS":
                            {
                                g.DrawImage(OTU, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 54, 50));

                                break;
                            }
                        case "TON":
                            {
                                g.DrawImage(Timer_Move, new Rectangle(left_indent_rang_x + PointOfElemetts[index == rang_text.Length - 1 ? 12 : drow_ind] - 37 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 75, 50));

                                break;
                            }
                        case "MOV":
                            {
                                g.DrawImage(Timer_Move, new Rectangle(left_indent_rang_x + PointOfElemetts[index == rang_text.Length - 1 ? 12 : drow_ind] - 37 + scrollX, ((4 * top_indent_rang) / 4) + startY - scrollY - 25, 75, 50));

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

        private int[] DrowSap(string[] rang_text, int IndexStart, int Start_Y, int Start_X, Point point, int CountOfBranch, int CountInBranch)
        {
            int count_of_branch = CountOfBranch;
            Point p = point;
            bool BranchStart = false;
            Branch branch = new Branch(g, p, (short)CountInBranch);
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
                    branch = new Branch(g, p);
                    BranchStart = true;
                    count_of_branch++;
                }
                else if (el == "NXB")
                {
                    p.Y += top_indent;
                    branch = new Branch(g, p, (short)CountInBranch);
                    int[] inf = DrowSap(rang_text, index + 1, Start_Y + top_indent, x_branch, p, count_of_branch, CountInBranch);
                    index = inf[0];
                    branch.Update();
                    if (branch < inf[1]) branch = branch - branch + Math.Max(branch.Count, inf[1]);
                    if (count_of_branch == 1) branch.DrowBranch();
                    BranchStart = Convert.ToBoolean(inf[2]);
                    drow_ind = Math.Max(drow_ind+1, inf[3]);
                    if (inf[4] == 0) return inf;
                    continue;
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
                        branch.Update();
                        branch++;
                        branch.DrowBranch();
                    }
                    switch (el)
                    {
                        case "XIO":
                            {

                                g.DrawImage(XIO, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, Start_Y + startY - scrollY - 25, 54, 50));


                                break;
                            }
                        case "XIC":
                            {

                                g.DrawImage(XIC, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, Start_Y + startY - scrollY - 25, 54, 50));

                                break;
                            }
                        case "OTE":
                            {
                                //if (Adres(adres, mas))
                                g.DrawImage(OTE, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, Start_Y + startY - scrollY - 25, 54, 50));

                                break;
                            }
                        case "OTL":
                            {
                                //if (Adres(adres, mas))
                                g.DrawImage(OTL, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, Start_Y + startY - scrollY - 25, 54, 50));

                                break;
                            }
                        case "OTU":
                            {
                                //if (Adres(adres, mas))
                                g.DrawImage(OTU, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, Start_Y + startY - scrollY - 25, 54, 50));

                                break;
                            }
                        case "ONS":
                            {
                                g.DrawImage(OTU, new Rectangle(left_indent_rang_x + PointOfElemetts[drow_ind] - 27 + scrollX, Start_Y + startY - scrollY - 25, 54, 50));

                                break;
                            }
                        case "TON":
                            {
                                g.DrawImage(Timer_Move, new Rectangle(left_indent_rang_x + PointOfElemetts[index == rang_text.Length-1?12: drow_ind] - 37 + scrollX, Start_Y + startY - scrollY - 25, 75, 50));

                                break;
                            }
                        case "MOV":
                            {
                                g.DrawImage(Timer_Move, new Rectangle(left_indent_rang_x + PointOfElemetts[index == rang_text.Length - 1 ? 12 : drow_ind] - 37 + scrollX, Start_Y + startY - scrollY - 25, 75, 50));

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

            throw new Exception("Not met BND");
        }
    }
}
