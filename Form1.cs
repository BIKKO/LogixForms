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
        public static ushort[] N13 = new ushort[70];
        public static ushort[] N15 = new ushort[70];
        public static ushort[] N18 = new ushort[70];
        public static ushort[] N40 = new ushort[70];
        public static ushort[] B3 = new ushort[70];

        public static List<(int, int)> BST = new List<(int, int)>();
        public static List<(int, int)> NXB = new List<(int, int)>();
        public static string[] File_MB = File.ReadAllLines(@"C:\Users\njnji\Desktop\проеты\matplotlib\ddd", Encoding.UTF8);
        public static int[,] info = new int[File_MB.Length, 3]; //0:SAP_EL_MAX 1:NXB 2:EL
        public Bitmap XIC = NodEn.XIC, XIO = NodEn.XIO, Timer_Move = NodEn.Timer___Move, EnDnTt = NodEn.EN_DN_TT, OTU = NodEn.OTU,
            OTE = NodEn.OTE, OTL = NodEn.OTE;
        public Pen pen_line = new Pen(Brushes.Black);
        private Font text = new Font("Arial", 10);
        private Font RangsFont = new Font("Arial", 12);
        private int left_indent_rang_x = 50;
        private int top_indent_rang = 150;
        int scroll_y = 0;

        public Form1()
        {
            InitializeComponent();
            this.MouseWheel += new MouseEventHandler(This_MouseWheel);
            pen_line.Width = 3;
            
            RangsInfo();
        }

        private void This_MouseWheel(object sender, MouseEventArgs e)
        {
            int wheel = 0;
            if (e.Delta > 0)
            {
                wheel = File_MB.Length%10!=0? -1:-10;
            }
            else
            {
                wheel = File_MB.Length % 10 != 0 ? 1 : -10; ;
            }
            if (vScrollBar1.Maximum >= vScrollBar1.Value + wheel && vScrollBar1.Minimum <= vScrollBar1.Value + wheel)
                vScrollBar1.Value += wheel;
            wheel = 0;
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

        private void UpdateImage_Tick(object sender, EventArgs e)
        {
            Refresh();
            if (midpanel.Height * File_MB.Length / 150 >= 150)
                top_indent_rang = (midpanel.Height * File_MB.Length) / 150;
            else top_indent_rang = 150;
        }

        private void RangsInfo()
        {
            for (int line = 0; line < File_MB.Length; line++)
            {

                string[] rang = File_MB[line].Trim().Split(' ');
                int count_el = 0;
                int count_sap_el = 0;
                int count_nxb = 0;

                foreach (string s in rang)//3
                {
                    if (!s.Contains(':') && (s != "BST" || s != "BND" || s != "NXB"))
                    {
                        try
                        {
                            int.Parse(s);
                        }
                        catch
                        {
                            count_el++;
                        }
                    }
                    else if (s == "NXB") count_nxb++;
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
                            else count_el_sap = 0;
                            if (rang[s] == "BND") break;
                        }
                    }
                }
                else
                {
                    count_sap_el = 1;
                }

                info[line, 0] = count_sap_el;
                info[line, 1] = count_nxb;
                info[line, 2] = count_el;
            }
        }

        private void midpanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int maxY = top_indent_rang * (File_MB.Length-2);
            for (int i = 1; i < File_MB.Length; i++)
                maxY += (top_indent_rang / 2) * info[i - 1, 1];
            PointF Scroll = new PointF(79, 50);
            PointF MaxY = new PointF(79, 70);

            scroll_y = vScrollBar1.Value * (maxY / 100);//прокрутка
            g.DrawString(vScrollBar1.Value.ToString() + '/' + scroll_y.ToString(), RangsFont, Brushes.Black, Scroll);
            //g.DrawString(maxY.ToString(), RangsFont, Brushes.Black, MaxY);

            //вертикаль
            g.DrawLine(pen_line, left_indent_rang_x, 0, left_indent_rang_x, midpanel.Height);
            g.DrawLine(pen_line, midpanel.Width - 2, 0, midpanel.Width - 2, midpanel.Height);

            PointF locationToDrawRangs = new PointF();
            locationToDrawRangs.X = 20;
            locationToDrawRangs.Y = top_indent_rang - scroll_y - 10;
            //горизонталь + номер ранга
            g.DrawString("0", RangsFont, Brushes.Black, locationToDrawRangs);
            g.DrawLine(pen_line, left_indent_rang_x, top_indent_rang - scroll_y, midpanel.Width, top_indent_rang - scroll_y);
            int top_step = top_indent_rang;
            for (int i = 1; i < File_MB.Length; i++)
            {
                top_step += top_indent_rang + ((top_indent_rang / 2) * info[i - 1, 1]);
                locationToDrawRangs.Y = top_step-10 - scroll_y;
                g.DrawString(i.ToString(), RangsFont, Brushes.Black, locationToDrawRangs);
                g.DrawLine(pen_line, left_indent_rang_x, top_step - scroll_y, midpanel.Width, top_step - scroll_y);
            }
            g.DrawString(maxY.ToString()+'/'+top_step.ToString(), RangsFont, Brushes.Black, MaxY);
        }
    }
}