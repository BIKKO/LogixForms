using System.Text;
using System.Xml.Linq;

namespace LogixForms
{
    public partial class Form1 : Form
    {
        public static ushort[] T4 = new ushort[24];
        public static ushort[] T4_c = new ushort[24];
        public static ushort[] T4_b = new ushort[24];
        public static int[] Timer_control = new int[32];
        //public static ConsoleKeyInfo cki;
        public static ushort[] N13 = new ushort[70];
        public static ushort[] N15 = new ushort[70];
        public static ushort[] N18 = new ushort[70];
        public static ushort[] N40 = new ushort[70];
        public static ushort[] B3 = new ushort[70];
        public static List<(int, int)> BST = new List<(int, int)>();
        public static List<(int, int)> NXB = new List<(int, int)>();
        public string[] File_MB = File.ReadAllLines(@"C:\Users\njnji\Desktop\проеты\matplotlib\ddd - copy", Encoding.UTF8);

        public Bitmap XIC = NodEn.XIC, XIO = NodEn.XIO, Timer_Move = NodEn.Timer___Move, EnDnTt = NodEn.EN_DN_TT, OTU = NodEn.OTU,
            OTE = NodEn.OTE, OTL = NodEn.OTE;
        public Pen pen_line = new Pen(Brushes.Black);
        private Font text = new Font("Arial", 10);
        private Font Rangs = new Font("Arial", 12);
        private int left_indent_rang_x = 50;
        private int right_indent_rang_x = 45;
        private int top_indent_rang = 150;
        int scroll_y = 0;

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint, true);
            UpdateStyles();
            pen_line.Width = 3;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

        }

        private static int Adres(string st, ushort[] mas) //выдает значение бита в массиве
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
            if ((Height * File_MB.Length) / 150 >= 150)
                top_indent_rang = (Height * File_MB.Length) / 150;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            PointF Scroll= new PointF(79, 50);
            scroll_y = vScrollBar1.Value *(((File_MB.Length+2) * top_indent_rang)/100);//прокрутка
            //g.DrawString(scroll_y.ToString(), Rangs, Brushes.Black, Scroll);

            //вертикаль
            g.DrawLine(pen_line, left_indent_rang_x, 0, left_indent_rang_x, Height);
            g.DrawLine(pen_line, Width - right_indent_rang_x-2, 0, Width - right_indent_rang_x-2, Height);

            PointF locationToDrawRangs = new PointF();
            locationToDrawRangs.X = 20;
            //горизонталь + номер ранга
            for (int i = 1; i < File_MB.Length + 1; i++)
            {
                locationToDrawRangs.Y = ((top_indent_rang * i) - 10) - scroll_y;
                g.DrawString((i - 1).ToString(), Rangs, Brushes.Black, locationToDrawRangs);
                g.DrawLine(pen_line, left_indent_rang_x, (top_indent_rang * i) - scroll_y, Width - left_indent_rang_x + 5, (top_indent_rang * i) - scroll_y);
                
                string[] element = File_MB[i - 1].Trim().Split(' ');
                if (element.Contains("BST"))
                {
                    int max_count_el_sap = 0;
                    int count_sap = 0;
                    int count_st = 0;
                    int count_nx = 0;
                    int count_end = 0;
                    string s;
                    int buf = 0;
                    for (int k = 0; k<element.Length; k++)
                    {
                        s = element[k];
                        if (s == "BST")
                        {
                            count_st++;
                            count_sap++;
                            if (count_sap > 1)
                            {
                                int[] mas_max_count_el_sap = new int[count_sap];
                            }
                        }
                        else if (s == "NXB")
                        {
                            count_nx++;
                            for (int l = k; k < element.Length; l++)
                            {
                                if (element[l] != "NXB" || element[l] != "END" || element[l] != "BST")
                                {
                                    if (!element[l].Contains(':'))
                                        max_count_el_sap++;
                                }
                                else if (element[l] == "NXB")
                                {
                                    max_count_el_sap = 0;
                                    count_nx++;
                                    break;
                                }
                                else
                                {
                                    count_end++;
                                    buf = l;
                                    break;
                                }
                            }
                        }
                        else if (k < buf) continue;
                    }
                    max_count_el_sap /= 2;
                }
                else
                {
                    for (int j = 0; j < element.Length; j++)
                    {
                        string el = element[j];
                        int step = j * (Width - left_indent_rang_x + 5) / element.Length;
                        if (el == "XIO")
                        {
                            g.DrawImage(XIO, new Rectangle(left_indent_rang_x + 20 + step, ((top_indent_rang * i)) - 20 - scroll_y, 54, 50));
                        }
                        else if (el == "XIC")
                        {
                            g.DrawImage(XIC, new Rectangle(left_indent_rang_x + 20 + step, ((top_indent_rang * i)) - 20 - scroll_y, 54, 50));
                        }
                        else if (el == "OTU")
                        {
                            g.DrawImage(OTU, new Rectangle(left_indent_rang_x + 20 + step, ((top_indent_rang * i)) - 20 - scroll_y, 63, 50));
                        }
                        else if (el == "OTE")
                        {
                            g.DrawImage(OTE, new Rectangle(left_indent_rang_x + 20 + step, ((top_indent_rang * i)) - 20 - scroll_y, 63, 50));
                        }
                        else if (el == "OTL")
                        {
                            g.DrawImage(OTL, new Rectangle(left_indent_rang_x + 20 + step, ((top_indent_rang * i)) - 20 - scroll_y, 63, 50));
                        }
                        else
                            continue;
                    }
                }

            }
        }
    }
}