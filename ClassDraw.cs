namespace LogixForms
{
    /// <summary>
    /// Отрисовка программы
    /// </summary>
    public class ClassDraw
    {
        private List<string> info_file;
        private VScrollBar VScroll;
        private HScrollBar HScroll;
        private int scroll_y = 0;//смещение
        private int scroll_x = 0;
        private MyTabControl SelectedTab;
        private Dictionary<string, ushort[]> Adr;
        private MyPanel panel;
        int Height, Width;
        private Rang rang;

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
        public ClassDraw(MyPanel Panel, List<string> File, VScrollBar vScroll, 
            HScrollBar hScroll, MyTabControl MyTab, Dictionary<string, ushort[]> AdresDir,
            int height, int widht)
        {
            info_file = File;
            VScroll = vScroll;
            HScroll = hScroll;
            SelectedTab = MyTab;
            Adr = AdresDir;
            panel = Panel;
            Height = height;
            Width = widht;
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
        public ClassDraw(ref MyPanel Panel, ref List<string> File, ref VScrollBar vScroll,
            ref HScrollBar hScroll, ref MyTabControl MyTab, int height, int widht)
        {
            info_file = File;
            VScroll = vScroll;
            HScroll = hScroll;
            SelectedTab = MyTab;
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
        }

        public ref Dictionary<string, ushort[]> GetAdresTabl
        {
            get { return ref Adr; }
        }

        public void SetAdresTab(ref Dictionary<string, ushort[]> tab)
        {
            Adr = tab;
        }

        /// <summary>
        /// Отслеживание колесика мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void This_MouseWheel(object sender, MouseEventArgs e)
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
            ushort count_rangs = 1;
            foreach (string str in info_file)
            {
                rang = new Rang(g, ref scroll_y, ref scroll_x, y, count_rangs, ref Adr);
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
            Draw(e);
        }

        /// <summary>
        /// Вызов отрисовк
        /// </summary>
        public async void StartDrow()
        {
            panel.Paint += PaintText;
            while(true)
            {
                await Task.Delay(200);

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
            }
        }
    }
}
