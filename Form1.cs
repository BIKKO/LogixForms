using System.Xml.Linq;

namespace LogixForms
{
    public partial class Form1 : Form
    {
        public Bitmap XIC = NodEn.XIC, XIO = NodEn.XIO, Timer_Move = NodEn.Timer___Move, EnDnTt = NodEn.EN_DN_TT;
        public Pen pen_line = new Pen(Brushes.Black);
        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint, true);

            UpdateStyles();
            pen_line.Width = 2;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int scroll_y = vScrollBar1.Value;
            /*g.DrawImage(XIC, new Rectangle(0, 0 - x, 54, 50));
            g.DrawImage(XIO, new Rectangle(0, 50 - x, 54, 50));
            g.DrawImage(Timer_Move, new Rectangle(125, 0 - x, 75 * 2, 50 * 2));
            // текст
            Font text = new Font("Arial", 20);
            g.DrawString("test", text, Brushes.Black, 127, 4 - x);
            string text = "Text";
            SizeF textSize = e.Graphics.MeasureString(text, Font);
            PointF locationToDraw = new PointF();
            locationToDraw.X = (PB.Width / 2) - (textSize.Width / 2);
            locationToDraw.Y = (PB.Height / 2) - (textSize.Height / 2);
            e.Graphics.DrawString(text, Font, Brushes.Black, locationToDraw);

            //линия ранга
            g.DrawLine(pen_line, 0, 50 - x, Width, 50 - x);*/
        }
    }
}