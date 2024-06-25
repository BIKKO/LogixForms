using System.Net;
using System.Text.RegularExpressions;

namespace LogixForms
{
    /// <summary>
    /// Родительский класс отрисовок элементов
    /// </summary>
    public abstract class ElementDraw
    {
        protected Size _Size = new Size(35, 25);
        protected Font _Font = new Font("Arial", 10);
        private int[] Timer_control = new int[32];

        /// <summary>
        /// Проверка активноти элемента
        /// </summary>
        /// <param name="st">Адрес</param>
        /// <param name="mas">Название</param>
        /// <returns>Активность</returns>
        protected bool Adres(string st, Dictionary<string, ushort[]> Adr)
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
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        public virtual void Draw(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr)
        {
            return;
        }

        /// <summary>
        /// Отрисовка Тегов и Комментариев
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Teg">Тег</param>
        /// <param name="Com">Сомментарий</param>
        public virtual void DrawTegAndCom(Graphics g,Point point, string Teg, string Com)
        {
            return;
        }

        /// <summary>
        /// Уничтожение объекта
        /// </summary>
        public virtual void Dispose(){ Timer_control = null; }
    }

    /// <summary>
    /// Отрисовк XIC
    /// </summary>
    public class XIC : ElementDraw
    {
        private readonly Bitmap En = NodEn.XIC, Dis = NodDis.XICdis;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        public override void Draw(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr)
        {
            if (Adres(_Adres, Adr))
                g.DrawImage(Dis, new Rectangle(point, _Size));
            else
                g.DrawImage(En, new Rectangle(point, _Size));
            point.Y -= 15;
            point.X -= 15;
            g.DrawString(_Adres, _Font, Brushes.Black, point);
        }
        /// <summary>
        /// Отрисовка Тегов и Комментариев
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Teg">Тег</param>
        /// <param name="Com">Сомментарий</param>
        public override void DrawTegAndCom(Graphics g, Point point, string Teg, string Com)
        {
            point.X -= 15;
            g.DrawString(Teg, new Font("Arial", 10), Brushes.Blue, point);
            point.Y -= 15;
            g.DrawString(Com, new Font("Arial", 10), Brushes.Red, point);
        }

        /// <summary>
        /// Уничтожение объекта
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            En.Dispose();
            Dis.Dispose();
        }
    }

    /// <summary>
    /// Отрисовк XIO
    /// </summary>
    public class XIO : ElementDraw
    {
        private readonly Bitmap En = NodEn.XIO, Dis = NodDis.XIOdis;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        public override void Draw(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr)
        {
            if (!Adres(_Adres, Adr))
                g.DrawImage(Dis, new Rectangle(point, _Size));
            else
                g.DrawImage(En, new Rectangle(point, _Size));
            point.Y -= 15;
            point.X -= 15;
            g.DrawString(_Adres, _Font, Brushes.Black, point);
        }
        /// <summary>
        /// Отрисовка Тегов и Комментариев
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Teg">Тег</param>
        /// <param name="Com">Сомментарий</param>
        public override void DrawTegAndCom(Graphics g, Point point, string Teg, string Com)
        {
            point.X -= 15;
            g.DrawString(Teg, new Font("Arial", 10), Brushes.Blue, point);
            point.Y -= 15;
            g.DrawString(Com, new Font("Arial", 10), Brushes.Red, point);
        }

        /// <summary>
        /// Уничтожение объекта
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            En.Dispose();
            Dis.Dispose();
        }
    }

    /// <summary>
    /// Отрисовк OTE
    /// </summary>
    public class OTE : ElementDraw
    {
        private readonly Bitmap En = NodEn.OTE, Dis = NodDis.OTEdis;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        public override void Draw(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr)
        {
            if (Adres(_Adres, Adr))
                g.DrawImage(Dis, new Rectangle(point, _Size));
            else
                g.DrawImage(En, new Rectangle(point, _Size));
            point.Y -= 15;
            point.X -= 15;
            g.DrawString(_Adres, _Font, Brushes.Black, point);
        }
        /// <summary>
        /// Отрисовка Тегов и Комментариев
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Teg">Тег</param>
        /// <param name="Com">Сомментарий</param>
        public override void DrawTegAndCom(Graphics g, Point point, string Teg, string Com)
        {
            point.X -= 15;
            g.DrawString(Teg, new Font("Arial", 10), Brushes.Blue, point);
            point.Y -= 15;
            g.DrawString(Com, new Font("Arial", 10), Brushes.Red, point); ;
        }

        /// <summary>
        /// Уничтожение объекта
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            En.Dispose();
            Dis.Dispose();
        }
    }

    /// <summary>
    /// Отрисовк OTL
    /// </summary>
    public class OTL : ElementDraw
    {
        private readonly Bitmap En = NodEn.OTL, Dis = NodDis.OTLdis;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        public override void Draw(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr)
        {
            if (Adres(_Adres, Adr))
                g.DrawImage(Dis, new Rectangle(point, _Size));
            else
                g.DrawImage(En, new Rectangle(point, _Size));
            point.Y -= 15;
            point.X -= 15;
            g.DrawString(_Adres, _Font, Brushes.Black, point);
        }
        /// <summary>
        /// Отрисовка Тегов и Комментариев
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Teg">Тег</param>
        /// <param name="Com">Сомментарий</param>
        public override void DrawTegAndCom(Graphics g, Point point, string Teg, string Com)
        {
            point.X -= 15;
            g.DrawString(Teg, new Font("Arial", 10), Brushes.Blue, point);
            point.Y -= 15;
            g.DrawString(Com, new Font("Arial", 10), Brushes.Red, point);
        }

        /// <summary>
        /// Уничтожение объекта
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            En.Dispose();
            Dis.Dispose();
        }
    }

    /// <summary>
    /// Отрисовк OTU
    /// </summary>
    public class OTU : ElementDraw
    {
        private readonly Bitmap En = NodEn.OTU, Dis = NodDis.OTUdis;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        public override void Draw(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr)
        {
            if (Adres(_Adres, Adr))
                g.DrawImage(Dis, new Rectangle(point, _Size));
            else
                g.DrawImage(En, new Rectangle(point, _Size));
            point.Y -= 15;
            point.X -= 15;
            g.DrawString(_Adres, _Font, Brushes.Black, point);
        }
        /// <summary>
        /// Отрисовка Тегов и Комментариев
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Teg">Тег</param>
        /// <param name="Com">Сомментарий</param>
        public override void DrawTegAndCom(Graphics g, Point point, string Teg, string Com)
        {
            point.X -= 15;
            g.DrawString(Teg, new Font("Arial", 10), Brushes.Blue, point);
            point.Y -= 15;
            g.DrawString(Com, new Font("Arial", 10), Brushes.Red, point);
        }

        /// <summary>
        /// Уничтожение объекта
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            En.Dispose();
            Dis.Dispose();
        }
    }

    /// <summary>
    /// Отрисовк TON
    /// </summary>
    public class Timer : ElementDraw
    {
        private readonly Bitmap En = NodEn.Timer___Move;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        public override void Draw(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr)
        {
            Point _p = point;
            _p.Y -= 20;
            _p.X -= 20;
            _Size = new Size(120, 75);
            g.DrawImage(En, new Rectangle(_p, _Size));
            g.DrawString("Timer " + _Adres, _Font, Brushes.Black, _p);
        }
        /// <summary>
        /// Отрисовка Тегов и Комментариев
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Teg">Тег</param>
        /// <param name="Com">Сомментарий</param>
        public override void DrawTegAndCom(Graphics g, Point point, string Teg, string Com)
        {
            point.X -= 15;
            g.DrawString(Teg, new Font("Arial", 10), Brushes.Blue, point);
            point.Y -= 15;
            g.DrawString(Com, new Font("Arial", 10), Brushes.Red, point);
        }

        /// <summary>
        /// Уничтожение объекта
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            En.Dispose();
        }
    }

    /// <summary>
    /// Отрисовка MOV
    /// </summary>
    public class Move : ElementDraw
    {
        private readonly Bitmap En = NodEn.Timer___Move;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        public override void Draw(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr)
        {
            Point _p = point;
            _p.Y += 10;
            _p.X -= 20;
            _Size = new Size(120, 75);
            g.DrawImage(En, new Rectangle(_p, _Size));
            point.Y -= 15;
            g.DrawString(_Adres, _Font, Brushes.Black, point);
        }
        /// <summary>
        /// Отрисовка Тегов и Комментариев
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Teg">Тег</param>
        /// <param name="Com">Сомментарий</param>
        public override void DrawTegAndCom(Graphics g, Point point, string Teg, string Com)
        {
            point.X -= 15;
            g.DrawString(Teg, new Font("Arial", 10), Brushes.Blue, point);
            point.Y -= 15;
            g.DrawString(Com, new Font("Arial", 10), Brushes.Red, point);
        }

        /// <summary>
        /// Уничтожение объекта
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            En.Dispose();
        }
    }

    /// <summary>
    /// Отрисовка ADD
    /// </summary>
    public class Add : ElementDraw
    {
        private readonly Bitmap En = NodEn.Timer___Move;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        public override void Draw(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr)
        {
            Point _p = point;
            _p.Y += 10;
            _p.X -= 20;
            _Size = new Size(120, 75);
            g.DrawImage(En, new Rectangle(_p, _Size));
            point.Y -= 15;
            g.DrawString(_Adres, _Font, Brushes.Black, point);
        }
        /// <summary>
        /// Отрисовка Тегов и Комментариев
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Teg">Тег</param>
        /// <param name="Com">Сомментарий</param>
        public override void DrawTegAndCom(Graphics g, Point point, string Teg, string Com)
        {
            point.X -= 15;
            g.DrawString(Teg, new Font("Arial", 10), Brushes.Blue, point);
            point.Y -= 15;
            g.DrawString(Com, new Font("Arial", 10), Brushes.Red, point);
        }

        /// <summary>
        /// Уничтожение объекта
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            En.Dispose();
        }
    }
}