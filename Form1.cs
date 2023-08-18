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
        public string[] File_MB = File.ReadAllLines(@"C:\Users\njnji\Desktop\проеты\matplotlib\ddd", Encoding.UTF8);

        public Bitmap XIC = NodEn.XIC, XIO = NodEn.XIO, Timer_Move = NodEn.Timer___Move, EnDnTt = NodEn.EN_DN_TT;
        public Pen pen_line = new Pen(Brushes.Black);
        private Font text = new Font("Arial", 10);
        private Font Rangs = new Font("Arial", 12);
        private int left_indent_rang_x = 50;
        private int right_indent_rang_x = 45;
        private int top_indent_rang = 150;

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint, true);

            UpdateStyles();
            pen_line.Width = 3;
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
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            
            int scroll_y = vScrollBar1.Value * (int)(File_MB.Length * 1.55);//прокрутка

            /*g.DrawImage(XIC, new Rectangle(0, 0 - x, 54, 50));
            g.DrawImage(XIO, new Rectangle(0, 50 - x, 54, 50));
            g.DrawImage(Timer_Move, new Rectangle(125, 0 - x, 75 * 2, 50 * 2));
            // текст
            
            g.DrawString("test", text, Brushes.Black, 127, 4 - x);
            string text = "Text";
            SizeF textSize = e.Graphics.MeasureString(text, Font);
            
            e.Graphics.DrawString(text, Font, Brushes.Black, locationToDraw);

            //линия ранга
            g.DrawLine(pen_line, 0, 50 - x, Width, 50 - x);*/

            //вертикаль
            g.DrawLine(pen_line, left_indent_rang_x, 0, left_indent_rang_x, Height);
            g.DrawLine(pen_line, Width - right_indent_rang_x, 0, Width - right_indent_rang_x, Height);

            PointF locationToDrawRangs = new PointF();
            locationToDrawRangs.X = 20;
            string rang = "";
            //горизонталь + номер ранга
            for (int i = 1; i < File_MB.Length+1; i++)
            {
                rang = "" + (i - 1);
                locationToDrawRangs.Y = ((top_indent_rang * i) -10 ) - scroll_y;
                g.DrawString(rang, Rangs, Brushes.Black, locationToDrawRangs);
                g.DrawLine(pen_line, left_indent_rang_x, (top_indent_rang * i) - scroll_y, Width - left_indent_rang_x+5, (top_indent_rang * i) - scroll_y);
            }
        }
    }
}