namespace LogixForms
{
    struct Branch
    {
        private const byte indent = 15;
        private const byte top_indent = 100;
        private short count_el;
        private Pen pen_line = new Pen(Brushes.Red);
        private int[] PointOfElemetts = new int[14];
        private Pen PenOfPoint = new Pen(Brushes.Brown, 7);
        private const byte wight = 54;
        private Graphics g;
        private Point p;

        public Branch()
        {
            count_el = 0;
            for (int i = 0; i < PointOfElemetts.Length; i++)
            {
                PointOfElemetts[i] = 1300 / 14 * (i + 1) - 70 / 2;
            }
        }

        public Branch(Graphics g, Point StartIndex)
        {
            count_el = 0;
            for (int i = 0; i < PointOfElemetts.Length; i++)
            {
                PointOfElemetts[i] = 1300 / 14 * (i + 1) - 70 / 2;
            }
            this.g = g;
            p = StartIndex;
        }

        public Branch(Graphics g, Point StartIndex, short count)
        {
            this.g = g;
            p = StartIndex;
            count_el = count;
            for (int i = 0; i < PointOfElemetts.Length; i++)
            {
                PointOfElemetts[i] = 1300 / 14 * (i + 1) - 70 / 2;
            }
        }

        public static Branch operator ++(Branch a)
        {
            a.count_el += 1;
            return a;
        }

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

        public static Branch operator +(Branch a, short b)
        {
            a.count_el += b;
            return a;
        }
        
        public static Branch operator +(Branch a, int b)
        {
            a.count_el += (short)b;
            return a;
        }

        public static Branch operator +(Branch a, Branch b)
        {
            a.count_el += b.count_el;
            return a;
        }

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

        public static bool operator >(Branch a, int b) => a.count_el > b;

        public static bool operator <(Branch a, int b) => a.count_el < b;
        
        public int Count
        {
            get
            {
                return count_el;
            }
        }

        public void Update()
        {
            Drow(new Pen(Brushes.White), new Pen(Brushes.White, 7));
        }

        public void DrowBranch()
        {
            
            Drow(new Pen(Brushes.Red), new Pen(Brushes.Brown, 7));
        }

        private void Drow(Pen line, Pen point)
        {
            if (count_el == 0)
            {
                g.DrawLine(line, p.X - indent - (wight * count_el), p.Y + top_indent, p.X + indent + (wight * count_el), p.Y + top_indent);   //горизонт
                g.DrawLine(line, p.X - indent - (wight * count_el), p.Y, p.X - indent - (wight * count_el), p.Y + top_indent);
                g.DrawLine(line, p.X + indent + (wight * count_el), p.Y, p.X + indent + (wight * count_el), p.Y + top_indent);
                g.DrawEllipse(point, p.X, p.Y + top_indent - 2, 4, 4);
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
                g.DrawLine(line, p.X, p.Y + top_indent, PointOfElemetts[count] + 70, p.Y + top_indent);   //горизонт
                g.DrawLine(line, p.X, p.Y, p.X, p.Y + top_indent);//left
                g.DrawLine(line, PointOfElemetts[count] + 70, p.Y, PointOfElemetts[count] + 70, p.Y + top_indent);//right
                count = 0;
                foreach (var item in PointOfElemetts)
                {
                    if (item > p.X - 70)
                    {
                        g.DrawEllipse(point, item + 70, p.Y + top_indent - 2, 4, 4);
                        count++;
                        if (count == count_el) break;
                    }
                }
            }
        }
    }
}
