namespace LogixForms
{
    /// <summary>
    /// Конструктор и отрисовка ветви
    /// </summary>
    struct Branch
    {
        private const byte indent = 15;
        private const byte top_indent = 120;
        private short count_el;
        private int[] PointOfElemetts = new int[14];
        private const byte wight = 54;
        private int h;
        private Graphics g;
        private Point p;
        private int scrollX, scrollY;

        /// <summary>
        /// Инициализация ветви
        /// </summary>
        /// <param name="g">Отображение графики</param>
        /// <param name="StartIndex">Точка начала отрисовки</param>
        /// <param name="scrollY">Вертикальная прокрутка</param>
        /// <param name="scrollX">Горизонтальная прокрутка</param>
        public Branch(Graphics g, Point StartIndex, ref int scrollY, ref int scrollX)
        {
            this.scrollX = scrollX;
            this.scrollY = scrollY;
            count_el = 0;
            for (int i = 0; i < PointOfElemetts.Length; i++)
            {
                PointOfElemetts[i] = 1300 / 14 * (i + 1) - 70 / 2;
            }
            this.g = g;
            p = StartIndex;
            h = 1;
        }

        /// <summary>
        /// Инициализация ветви
        /// </summary>
        /// <param name="g">Отображение графики</param>
        /// <param name="StartIndex">Точка начала отрисовки</param>
        /// <param name="scrollY">Вертикальная прокрутка</param>
        /// <param name="scrollX">Горизонтальная прокрутка</param>
        /// <param name="count">Кол-во эелементов в ветке</param>
        public Branch(Graphics g, Point StartIndex, ref int scrollY, ref int scrollX, short count)
        {
            this.scrollX = scrollX;
            this.scrollY = scrollY;
            this.g = g;
            h = 1;
            p = StartIndex;
            count_el = count;
            for (int i = 0; i < PointOfElemetts.Length; i++)
            {
                PointOfElemetts[i] = 1300 / 14 * (i + 1) - 70 / 2;
            }
        }

        /// <summary>
        /// Очистка нарисованной ветви
        /// </summary>
        public void ClearBranch() => Drow(new Pen(Brushes.White), new Pen(Brushes.White, 7));

        /// <summary>
        /// Нарисовать вевь
        /// </summary>
        public void DrowBranch()
        {
            Drow(new Pen(Brushes.Red), new Pen(Brushes.Brown, 7));
        }

        /// <summary>
        /// Отрисовка ветви
        /// </summary>
        /// <param name="line">Инструмент отрисовки линий ветви</param>
        /// <param name="point">Инструмент отрисовки точек</param>
        private void Drow(Pen line, Pen point)
        {
            if (count_el == 0)
            {
                g.DrawLine(line, p.X - indent +scrollX, p.Y + top_indent - scrollY, p.X + indent  + scrollX, p.Y + top_indent - scrollY);   //горизонт
                //g.DrawString($"y: {p.Y + top_indent}\nx:{p.X + indent + (wight * count_el)}", new Font("Arial", 10), Brushes.Red, p.X + indent + scrollX+20, p.Y + top_indent - scrollY);
                g.DrawLine(line, p.X - indent + scrollX, p.Y - scrollY, p.X - indent + scrollX, p.Y + top_indent*h - scrollY);
                g.DrawLine(line, p.X + indent + scrollX, p.Y- scrollY, p.X + indent + scrollX, p.Y + top_indent*h - scrollY);
                g.DrawEllipse(point, p.X + scrollX, p.Y + top_indent - 2 - scrollY, 4, 4);
            }
            else
            {
                byte count = 0;
                for (int i = 0; i < PointOfElemetts.Length; i++)
                {
                    if (PointOfElemetts[i] == p.X - 70)
                    {
                        count = (byte)(i+count_el + 1);
                        break;
                    }
                }
                g.DrawLine(line, p.X + scrollX, p.Y + top_indent - scrollY, PointOfElemetts[count] + 70 + scrollX, p.Y + top_indent - scrollY);   //горизонт
                //g.DrawString($"y: {p.Y + top_indent}\nx:{PointOfElemetts[count] + 70}", new Font("Arial", 10), Brushes.Red, PointOfElemetts[count] + 70 + scrollX + 20, p.Y + top_indent - scrollY);
                g.DrawLine(line, p.X + scrollX, p.Y - scrollY, p.X+scrollX, p.Y + top_indent-scrollY);//left
                g.DrawLine(line, PointOfElemetts[count] + 70 + scrollX, p.Y-scrollY, PointOfElemetts[count] + 70+scrollX, p.Y + top_indent-scrollY);//right
                count = 0;
                foreach (var item in PointOfElemetts)
                {
                    if (item > p.X - 70)
                    {
                        g.DrawEllipse(point, item + 70 + scrollX, p.Y + top_indent - 2 - scrollY, 4, 4);
                        count++;
                        if (count == count_el) break;
                    }
                }
            }
        }

        /// <summary>
        /// добавление елемента в ветку
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Branch operator ++(Branch a)
        {
            a.count_el += 1;
            return a;
        }

        /// <summary>
        /// Вычет елемента из ветка
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        /// <exception cref="Отрицальное значение"></exception>
        public static Branch operator --(Branch a)
        {
            if (a.count_el - 1 >= 0)
            {
                a.count_el -= 1;
                return a;
            }
            else
            {
                throw new Exception("Negative value");
            }
        }

        /// <summary>
        /// добавление елемента в ветку
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Branch operator +(Branch a, short b)
        {
            a.count_el += b;
            return a;
        }

        /// <summary>
        /// добавление елемента в ветку
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Branch operator +(Branch a, int b)
        {
            a.count_el += (short)b;
            return a;
        }

        /// <summary>
        /// добавление елемента в ветку
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Branch operator +(Branch a, Branch b)
        {
            a.count_el += b.count_el;
            return a;
        }

        /// <summary>
        /// Вычет елемента из ветка
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Branch operator -(Branch a, short b)
        {
            if (a.count_el - b >= 0)
            {
                a.count_el -= b;
                return a;
            }
            else
            {
                throw new Exception("Negative value");
            }
        }

        /// <summary>
        /// Вычет елемента из ветка
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Branch operator -(Branch a, Branch b)
        {
            if (a.count_el - b.count_el >= 0)
            {
                a.count_el -= b.count_el;
                return a;
            }
            else
            {
                throw new Exception("Negative value");
            }
        }


        /// <summary>
        /// Вычет елемента из ветка
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Branch operator -(Branch a, int b)
        {
            if (a.count_el - b >= 0)
            {
                a.count_el -= (short)b;
                return a;
            }
            else
            {
                throw new Exception("Negative value");
            }
        }

        /// <summary>
        /// Сравнение кол-ва эл. с числом
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >(Branch a, int b) => a.count_el > b;

        /// <summary>
        /// Сравнение кол-ва эл. с числом
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <(Branch a, int b) => a.count_el < b;
        
        /// <summary>
        /// Получение кол-ва эл.
        /// </summary>
        public int Count
        {
            get
            {
                return count_el;
            }
        }

        /// <summary>
        /// Задание и получение высоты ветви
        /// </summary>
        public int H
        {
            get { return h; }
            set
            {
                if(value >0) h = value;
            }
        }
    }
}
