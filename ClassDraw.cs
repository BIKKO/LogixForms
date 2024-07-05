using System.Windows.Forms;

namespace LogixForms
{
    /// <summary>
    /// Отрисовка программы
    /// </summary>
    public class ClassDraw
    {
        private List<string> info_file;
        private readonly VScrollBar VScroll;
        private readonly HScrollBar HScroll;
        private int scroll_y = 0;//смещение
        private int scroll_x = 0;
        private int SelectedTab;
        private Dictionary<string, ushort[]> Adr;
        private MyPanel panel;
        private int Height, Width;
        private Rang? rang;
        private Dictionary<string, string[]> Tegs;
        private bool scroll = false;
        private bool Draw_flag = false;
        private MainThread owner;
        private bool Start = false;

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
        /// <param name="_Tegs">Список тегов</param>
        public ClassDraw(ref MyPanel Panel, List<string> File, ref VScrollBar vScroll, 
            ref HScrollBar hScroll, ref int TabWindht, ref Dictionary<string, ushort[]> AdresDir,
            int height, int widht, Dictionary<string, string[]> _Tegs, MainThread main)
        {
            info_file = File;
            VScroll = vScroll;
            HScroll = hScroll;
            SelectedTab = TabWindht;
            Adr = AdresDir;
            panel = Panel;
            Height = height;
            Width = widht;
            Tegs = _Tegs;
            owner = main;
        }

        /// <summary>
        /// Конструктор отрисовки
        /// </summary>
        /// <param name="Panel">Панель, на которой требуется отрисовк программы</param>
        /// <param name="File">Программа</param>
        /// <param name="vScroll">Ссылка на вертикальный ползунок</param>
        /// <param name="hScroll">Ссылка на горизонтальный ползунок</param>
        /// <param name="MyTab">Окно отображения</param>
        /// <param name="height">Высота</param>
        /// <param name="widht">Ширина</param>
        /// <param name="_Tegs">Список тегов</param>
        public ClassDraw(ref MyPanel Panel, List<string> File, ref VScrollBar vScroll,
            ref HScrollBar hScroll, ref int TabWindht, int height, int widht, Dictionary<string, string[]> _Tegs, MainThread main)
        {
            Tegs = _Tegs;
            info_file = File;
            VScroll = vScroll;
            HScroll = hScroll;
            SelectedTab = TabWindht;
            Adr = new Dictionary<string, ushort[]>
            {
                { "T4", new ushort[24] },
                { "T4_c", new ushort[24] },
                { "Timer_control", new ushort[32] },
                { "N13", new ushort[70] },
                { "N15", new ushort[70] },
                { "N18", new ushort[70] },
                { "N40", new ushort[70] },
                { "B3", new ushort[70] }
            };
            panel = Panel;
            Height = height;
            Width = widht;
            owner = main;
        }

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
        public ClassDraw(ref MyPanel Panel, List<string> File, ref VScrollBar vScroll,
            ref HScrollBar hScroll, ref int TabWindht, ref Dictionary<string, ushort[]> AdresDir,
            int height, int widht, MainThread main)
        {
            info_file = File;
            VScroll = vScroll;
            HScroll = hScroll;
            SelectedTab = TabWindht;
            Adr = AdresDir;
            panel = Panel;
            Height = height;
            Width = widht;
            owner = main;
        }

        /// <summary>
        /// Конструктор отрисовки
        /// </summary>
        /// <param name="Panel">Панель, на которой требуется отрисовк программы</param>
        /// <param name="File">Программа</param>
        /// <param name="vScroll">Ссылка на вертикальный ползунок</param>
        /// <param name="hScroll">Ссылка на горизонтальный ползунок</param>
        /// <param name="MyTab">Окно отображения</param>
        /// <param name="height">Высота</param>
        /// <param name="widht">Ширина</param>>
        public ClassDraw(ref MyPanel Panel, List<string> File, ref VScrollBar vScroll,
            ref HScrollBar hScroll, ref int TabWindht, int height, int widht, MainThread main)
        {
            info_file = File;
            VScroll = vScroll;
            HScroll = hScroll;
            SelectedTab = TabWindht;
            Adr = new Dictionary<string, ushort[]>
            {
                { "T4", new ushort[24] },
                { "T4_c", new ushort[24] },
                { "Timer_control", new ushort[32] },
                { "N13", new ushort[70] },
                { "N15", new ushort[70] },
                { "N18", new ushort[70] },
                { "N40", new ushort[70] },
                { "B3", new ushort[70] }
            };
            panel = Panel;
            Height = height;
            Width = widht;
            owner = main;
        }

        /// <summary>
        /// Получение адресов
        /// </summary>
        public ref Dictionary<string, ushort[]> GetDataTabl
        {
            get { return ref Adr; }
        }

        /// <summary>
        /// Полчение текста рангов
        /// </summary>
        public string[] GetTextRang => info_file.ToArray();

        /// <summary>
        /// Получение тегов
        /// </summary>
        public Dictionary<string, string[]> GetTegs => Tegs;

        /// <summary>
        /// Установление значения данныйх
        /// </summary>
        /// <param name="tab">Новые данные</param>
        public void SetAdresTab(Dictionary<string, ushort[]> tab)
        {
            Adr = tab;
        }

        /// <summary>
        /// Отключение от соытия прокрутки колесика мыши
        /// </summary>
        public bool EnableScroll
        {
            set { if (scroll != value) scroll = value; }
        }

        /// <summary>
        /// Отслеживание колесика мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void This_MouseWheel(object sender, MouseEventArgs e)
        {
            if (scroll)
            {
                int wheel = 0;//прокрутка вверх или вниз
                if (e.Delta > 0)
                {
                    //вверх
                    wheel = -(int)(VScroll.Maximum * 0.05);//если рангов > 10 то -1 иначе -10
                    //MessageBox.Show("Test");
                }
                else
                {
                    //вниз
                    wheel = (int)(VScroll.Maximum * 0.05);//если рангов > 10 то 1 иначе 10
                }
                if (VScroll.Maximum >= VScroll.Value + wheel && VScroll.Minimum <= VScroll.Value + wheel)
                    VScroll.Value += wheel;//не выходим ли за приделы scrollbar
                wheel = 0;//одиночное сробатование
            }
        }

        /// <summary>
        /// Отрисовка программы
        /// </summary>
        /// <param name="e"></param>
        private void Draw(object sender, PaintEventArgs e)
        {
            HScroll.Maximum = panel.Width - SelectedTab + 36;
            HScroll.Minimum = 0;

            scroll_y = VScroll.Value;//прокрутка
            scroll_x = panel.Width > 1300 ? HScroll.Value = 0 : -HScroll.Value;
            Graphics g = e.Graphics;//использование графики
            int y = 0;
            ushort count_rangs = 1;
            foreach (string str in info_file)
            {
                rang = new Rang(g, ref scroll_y, ref scroll_x, y, count_rangs, ref Adr, Tegs);
                rang.Draw(str);
                y = rang.Max;
                count_rangs++;
            }
            //rang = null;
            VScroll.Maximum = y - panel.Height+60>0? y - panel.Height + 160: 0;
            //g.DrawString($"scroll: {VScroll.Value} MaxScroll: {VScroll.Maximum}", RangsFont, Brushes.Red, 300, 200);
        }

        /// <summary>
        /// Вызов отрисовк
        /// </summary>
        public async void StartDrow()
        {
            panel.BackColor = Color.White;
            panel.Paint += Draw;
            Start = true;
            while(Start)
            {
                await Task.Delay(60);
                if (Draw_flag) owner.SetColorDraw = Color.Lime;
                else owner.SetColorDraw = Color.White;
                panel.Refresh();
                panel.Height = Height - 20;

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
                Draw_flag = !Draw_flag;
            }
        }

        /// <summary>
        /// Уничтожение экземпляра данного класса
        /// </summary>
        public void Dispose()
        {
            Start = false;
            Draw_flag = false;
            owner.SetColorDraw = Color.White;
            panel.Paint -= Draw;
            panel.Dispose();
            VScroll.Dispose();
            HScroll.Dispose();
            info_file.Clear();
            info_file = null;
            if(Tegs != null)
            Tegs.Clear();
            Tegs = null;
            if(rang != null)
            rang.Dispose();
        }
    }
}
