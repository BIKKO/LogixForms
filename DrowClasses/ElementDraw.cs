using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace LogixForms.DrowClasses
{
    /// <summary>
    /// Родительский класс отрисовок элементов
    /// </summary>
    public abstract class ElementDraw
    {
        protected Size _Size = new Size(35, 25);
        protected System.Drawing.Font _Font = new System.Drawing.Font(Properties.Settings.Default.FontStyle, Properties.Settings.Default.FontSize);
        protected Color Tag = Properties.Settings.Default.CollorTag;
        protected Color Com = Properties.Settings.Default.CollorCom;

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
                    adr = Adr["Timer_control"][ind_1];

                    if ((adr & Bitmask) == Bitmask) return true;
                    return false;
                }
                else if (k.Contains("DN"))
                {
                    Bitmask = 2;
                    ind_1 = int.Parse(k[0]);
                    adr = Adr["Timer_control"][ind_1];

                    if ((adr & Bitmask) == Bitmask) return true;
                    return false;
                }
                else if (k.Contains("TT"))
                {
                    Bitmask = 4;
                    ind_1 = int.Parse(k[0]);
                    adr = Adr["Timer_control"][ind_1];

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
            catch
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
        public virtual void DrawTegAndCom(Graphics g, Point point, string Teg, string Com)
        {
            return;
        }

        public virtual void DrawEl(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr, string[] rang)
        {

        }

        /// <summary>
        /// Уничтожение объекта
        /// </summary>
        public virtual void Dispose() { }
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
            {
                g.FillRectangle(Brushes.LightGreen, new Rectangle(point.X - 20, point.Y + 8, 75, 10));
                g.DrawImage(Dis, new Rectangle(point, _Size));
            }
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
            Point _p = point;
            _p.X -= 15;
            g.DrawString(Teg, _Font, new SolidBrush(Tag), _p);
            _p.Y -= 15;
            g.DrawString(Com, _Font, new SolidBrush(this.Com), _p);
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
            {
                g.FillRectangle(Brushes.LightGreen, new Rectangle(point.X - 20, point.Y + 8, 75, 10));
                g.DrawImage(Dis, new Rectangle(point, _Size));
            }
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
            Point _p = point;
            _p.X -= 15;
            g.DrawString(Teg, _Font, new SolidBrush(Tag), _p);
            _p.Y -= 15;
            g.DrawString(Com, _Font, new SolidBrush(this.Com), _p);
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
            {
                g.FillRectangle(Brushes.LightGreen, new Rectangle(point.X - 20, point.Y + 8, 75, 10));
                g.DrawImage(Dis, new Rectangle(point, _Size));
            }
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
            Point _p = point;
            _p.X -= 15;
            g.DrawString(Teg, _Font, new SolidBrush(Tag), _p);
            _p.Y -= 15;
            g.DrawString(Com, _Font, new SolidBrush(this.Com), _p);
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
            {
                g.FillRectangle(Brushes.LightGreen, new Rectangle(point.X - 20, point.Y + 8, 75, 10));
                g.DrawImage(Dis, new Rectangle(point, _Size));
            }
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
            Point _p = point;
            _p.X -= 15;
            g.DrawString(Teg, _Font, new SolidBrush(Tag), _p);
            _p.Y -= 15;
            g.DrawString(Com, _Font, new SolidBrush(this.Com), _p);
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
            {
                g.FillRectangle(Brushes.LightGreen, new Rectangle(point.X - 20, point.Y + 8, 75, 10));
                g.DrawImage(Dis, new Rectangle(point, _Size));
            }
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
            Point _p = point;
            _p.X -= 15;
            g.DrawString(Teg, _Font, new SolidBrush(Tag), _p);
            _p.Y -= 15;
            g.DrawString(Com, _Font, new SolidBrush(this.Com), _p);
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
    public class TON : ElementDraw
    {
        private readonly Bitmap En = NodEn.Timer___Move, EN_DN_TT = NodEn.EN_DN_TT, EN_DN_TTdis = NodDis.EN_DN_TTdis;
        private readonly System.Drawing.Font iner = new System.Drawing.Font("Arial", 8);

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        /// <param name="r">Список данных</param>
        public override void DrawEl(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr, string[] r)
        {
            string text = "Timer " + _Adres + "\n     "+r[0] + "\n     " +
                Adr["T4"][int.Parse(_Adres.Replace("T4:", ""))] + "\n     " + Adr["T4_c"][int.Parse(_Adres.Replace("T4:", ""))];

            Point _p = point;
            _p.Y -= 20;
            _p.X -= 20;
            _Size = g.MeasureString(text, _Font).ToSize();
            g.DrawImage(En, new Rectangle(_p, _Size));
            g.DrawString(text, _Font, Brushes.Black, _p);
            _p.X += _Size.Width;


            //EN
            _p.Y += 23;
            _Size = new Size(50, 20);
            g.DrawImage(EN_DN_TT, new Rectangle(_p, _Size));
            _p.Y += 2;
            _p.X += 13;
            if ((Adr["Timer_control"][int.Parse(_Adres.Replace("T4:", ""))] & 1) != 0)
            {
                g.DrawString("EN", iner, Brushes.LightGreen, _p);
            }
            else
                g.DrawString("EN", iner, Brushes.Black, _p);

            _p.Y -= 2;
            _p.X -= 13;

            //DN
            _p.Y += 20;
            g.DrawImage(EN_DN_TT, new Rectangle(_p, _Size));
            _p.Y += 2;
            _p.X += 13;
            if ((Adr["Timer_control"][int.Parse(_Adres.Replace("T4:", ""))] & 2) != 0)
            {
                g.DrawString("DN", iner, Brushes.LightGreen, _p);
            }
            else
                g.DrawString("DN", iner, Brushes.Black, _p);
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
            Point _p = point;
            _p.X -= 15;
            g.DrawString(Teg, _Font, new SolidBrush(Tag), _p);
            _p.Y -= 15;
            g.DrawString(Com, _Font, new SolidBrush(this.Com), _p);
        }

        /// <summary>
        /// Уничтожение объекта
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            En.Dispose();
            EN_DN_TT.Dispose();
            EN_DN_TTdis.Dispose();
        }
    }

    /// <summary>
    /// Отрисовка MOV
    /// </summary>
    public class MOV : ElementDraw
    {
        private readonly Bitmap En = NodEn.Timer___Move;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        /// <param name="r">Список данных</param>
        public override void DrawEl(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr, string[] r)
        {
            string text;
            
            Point _p = point;
            int u;
            if (int.TryParse(r[0], out u))
            {
                string[] buf = r[1].Split(":");
                string name = buf[0];
                int str = int.Parse(buf[1]);

                text = "Move \n     ?\n     " + r[0] + "\n     " + r[1] + "\n     " + Adr[name][str];
                _Size = g.MeasureString(text, _Font).ToSize();
                g.DrawImage(En, new Rectangle(_p, _Size));
            }
            else
            {
                text = "Move \n     " + r[0] + "\n     " + Adr[r[0].Split(":")[0]][int.Parse(r[0].Split(":")[1])] + "\n     "
                    + r[1] + "\n     " + Adr[r[1].Split(":")[0]][int.Parse(r[1].Split(":")[1])];

                _Size = g.MeasureString(text, _Font).ToSize();
                g.DrawImage(En, new Rectangle(_p, _Size));
            }
            g.DrawString(text, _Font, Brushes.Black, _p);
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
            Point _p = point;
            _p.X -= 15;
            g.DrawString(Teg, _Font, new SolidBrush(Tag), _p);
            _p.Y -= 15;
            g.DrawString(Com, _Font, new SolidBrush(this.Com), _p);
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
        /// <param name="r">Список данных</param>
        public override void DrawEl(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr, string[] r)
        {
            try
            {
                string text;

                Point _p = point;
                text = "ADD ";
                int u;
                if (int.TryParse(r[0], out u))
                {
                    string[] buf = r[1].Split(":");
                    string name = buf[0];
                    int str = int.Parse(buf[1]);
                    text += "?\n     " + r[0] + "\n     " + r[1] + "\n     " + Adr[name][str];
                    _Size = g.MeasureString(text, _Font).ToSize();
                    g.DrawImage(En, new Rectangle(_p, _Size));
                    
                }
                else
                {

                    text += r[0] + "\n     " + Adr[r[0].Split(":")[0]][int.Parse(r[0].Split(":")[1])] + "\n     "
                        + r[1] + "\n     " + Adr[r[2].Split(":")[0]][int.Parse(r[2].Split(":")[1])];
                    _Size = g.MeasureString(text, _Font).ToSize();
                    g.DrawImage(En, new Rectangle(_p, _Size));
                }
                g.DrawString(text, _Font, Brushes.Black, _p);
            }
            catch
            {
                Debug.Print("DrawEl - ADD");
            }
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
            Point _p = point;
            _p.X -= 15;
            g.DrawString(Teg, _Font, new SolidBrush(Tag), _p);
            _p.Y -= 15;
            g.DrawString(Com, _Font, new SolidBrush(this.Com), _p);
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
    public class DIV : ElementDraw
    {
        private readonly Bitmap En = NodEn.Timer___Move;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        /// <param name="r">Список данных</param>
        public override void DrawEl(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr, string[] r)
        {
            string text = "DIV\n     " + r[0] + "\n     " + r[1] + "\n     " + r[2];
            Point _p = point;
            _p.Y -= 20;
            _p.X -= 45;
            _Size = g.MeasureString(text, _Font).ToSize();
            g.DrawImage(En, new Rectangle(_p, _Size));

            g.DrawString(text, _Font, Brushes.Black, _p);
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
            Point _p = point;
            _p.X -= 15;
            g.DrawString(Teg, _Font, new SolidBrush(Tag), _p);
            _p.Y -= 15;
            g.DrawString(Com, _Font, new SolidBrush(this.Com), _p);
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
    public class MUL : ElementDraw
    {
        private readonly Bitmap En = NodEn.Timer___Move;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        /// <param name="r">Список данных</param>
        public override void DrawEl(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr, string[] r)
        {
            Point _p = point;
            _p.Y -= 20;
            _p.X -= 45;
            string text = "MUL\n     " + r[0] + "\n     " + r[1] + "\n     " + r[2];
            _Size = new Size(120, 75);
            g.DrawImage(En, new Rectangle(_p, _Size));

            g.DrawString(text, _Font, Brushes.Black, _p);
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
            Point _p = point;
            _p.X -= 15;
            g.DrawString(Teg, _Font, new SolidBrush(Tag), _p);
            _p.Y -= 15;
            g.DrawString(Com, _Font, new SolidBrush(this.Com), _p);
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
    public class ABS : ElementDraw
    {
        private readonly Bitmap En = NodEn.Timer___Move;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        /// <param name="r">Список данных</param>
        public override void DrawEl(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr, string[] r)
        {
            string text;

            Point _p = point;
            int u;
            if (int.TryParse(r[0], out u))
            {
                string[] buf = r[1].Split(":");
                string name = buf[0];
                int str = int.Parse(buf[1]);

                text = "ABS \n     ?\n     " + r[0] + "\n     " + r[1] + "\n     " + Adr[name][str];
                _Size = g.MeasureString(text, _Font).ToSize();
                g.DrawImage(En, new Rectangle(_p, _Size));
            }
            else
            {
                text = "ABS \n     " + r[0] + "\n     " + Adr[r[0].Split(":")[0]][int.Parse(r[0].Split(":")[1])] + "\n     "
                    + r[1] + "\n     " + Adr[r[1].Split(":")[0]][int.Parse(r[1].Split(":")[1])];

                _Size = g.MeasureString(text, _Font).ToSize();
                g.DrawImage(En, new Rectangle(_p, _Size));
            }
            g.DrawString(text, _Font, Brushes.Black, _p);
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
            Point _p = point;
            _p.X -= 15;
            g.DrawString(Teg, _Font, new SolidBrush(Tag), _p);
            _p.Y -= 15;
            g.DrawString(Com, _Font, new SolidBrush(this.Com), _p);
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
    public class SCP : ElementDraw
    {
        private readonly Bitmap En = NodEn.Timer___Move;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        /// <param name="r">Список данных</param>
        public override void DrawEl(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr, string[] r)
        {
            string text = "SCP \n     " + r[0] + "\n     " + r[1] + "\n     " + r[2] + "\n     " +
                r[3] + "\n     " + r[4] + "\n     " + r[5];
            Point _p = point;
            _p.Y -= 20;
            _p.X -= 40;
            _Size = g.MeasureString(text, _Font).ToSize();
            g.DrawImage(En, new Rectangle(_p, _Size));

            g.DrawString(text, _Font, Brushes.Black, _p);
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
            Point _p = point;
            _p.X -= 15;
            g.DrawString(Teg, _Font, new SolidBrush(Tag), _p);
            _p.Y -= 15;
            g.DrawString(Com, _Font, new SolidBrush(this.Com), _p);
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
    /// Отрисовка ONS
    /// </summary>
    public class ONS : ElementDraw
    {
        private readonly Bitmap En = NodEn.ONS;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        public override void Draw(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr)
        {
            _Size = new Size(60, 30);
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
            Point _p = point;
            _p.X -= 15;
            g.DrawString(Teg, _Font, new SolidBrush(Tag), _p);
            _p.Y -= 15;
            g.DrawString(Com, _Font, new SolidBrush(this.Com), _p);
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
    /// Отрисовка GEQ
    /// </summary>
    public class GEQ : ElementDraw
    {
        private readonly Bitmap En = NodEn.Timer___Move;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        /// <param name="r">Список данных</param>
        public override void DrawEl(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr, string[] r)
        {
            try
            {
                string text;

                Point _p = point;
                int u;
                if (int.TryParse(r[1], out u))
                {
                    string[] buf = r[0].Split(":");
                    string name = buf[0];
                    int str = int.Parse(buf[1]);

                    text = "GEQ >= \n     ?\n     " + r[1] + "\n     " + r[1] + "\n     " + Adr[name][str];
                    _Size = g.MeasureString(text, _Font).ToSize();
                    g.DrawImage(En, new Rectangle(_p, _Size));
                }
                else
                {
                    text = "GEQ >= \n     " + r[0] + "\n     " + Adr[r[0].Split(":")[0]][int.Parse(r[0].Split(":")[1])] + "\n     "
                        + r[1] + "\n     " + Adr[r[1].Split(":")[0]][int.Parse(r[1].Split(":")[1])];

                    _Size = g.MeasureString(text, _Font).ToSize();
                    g.DrawImage(En, new Rectangle(_p, _Size));
                }
                g.DrawString(text, _Font, Brushes.Black, _p);
            }
            catch
            {
                Debug.Print("DrawEl - GEQ" + _Adres);
            }
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
            Point _p = point;
            _p.X -= 15;
            g.DrawString(Teg, _Font, new SolidBrush(Tag), _p);
            _p.Y -= 15;
            g.DrawString(Com, _Font, new SolidBrush(this.Com), _p);
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
    /// Отрисовка GRT
    /// </summary>
    public class GRT : ElementDraw
    {
        private readonly Bitmap En = NodEn.Timer___Move;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        /// <param name="r">Список данных</param>
        public override void DrawEl(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr, string[] r)
        {
            try
            {
                string text;

                Point _p = point;
                int u;
                if (int.TryParse(r[1], out u))
                {
                    string[] buf = r[0].Split(":");
                    string name = buf[0];
                    int str = int.Parse(buf[1]);

                    text = "GRT > \n     ?\n     " + r[1] + "\n     " + r[1] + "\n     " + Adr[name][str];
                    _Size = g.MeasureString(text, _Font).ToSize();
                    g.DrawImage(En, new Rectangle(_p, _Size));
                }
                else
                {
                    text = "GRT > \n     " + r[0] + "\n     " + Adr[r[0].Split(":")[0]][int.Parse(r[0].Split(":")[1])] + "\n     "
                        + r[1] + "\n     " + Adr[r[1].Split(":")[0]][int.Parse(r[1].Split(":")[1])];

                    _Size = g.MeasureString(text, _Font).ToSize();
                    g.DrawImage(En, new Rectangle(_p, _Size));
                }
                g.DrawString(text, _Font, Brushes.Black, _p);
            }
            catch
            {
                Debug.Print("DrawEl - GRT" + _Adres);
            }
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
            Point _p = point;
            _p.X -= 50;
            g.DrawString(Teg, _Font, new SolidBrush(Tag), _p);
            _p.Y -= 15;
            g.DrawString(Com, _Font, new SolidBrush(this.Com), _p);
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
    /// Отрисовка EQU
    /// </summary>
    public class EQU : ElementDraw
    {
        private readonly Bitmap En = NodEn.Timer___Move;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        /// <param name="r">Список данных</param>
        public override void DrawEl(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr, string[] r)
        {
            try
            {
                string text;

                Point _p = point;
                int u;
                if (int.TryParse(r[1], out u))
                {
                    string[] buf = r[0].Split(":");
                    string name = buf[0];
                    int str = int.Parse(buf[1]);

                    text = "EQU == \n     ?\n     " + r[1] + "\n     " + r[1] + "\n     " + Adr[name][str];
                    _Size = g.MeasureString(text, _Font).ToSize();
                    g.DrawImage(En, new Rectangle(_p, _Size));
                }
                else
                {
                    text = "EQU == \n     " + r[0] + "\n     " + Adr[r[0].Split(":")[0]][int.Parse(r[0].Split(":")[1])] + "\n     "
                        + r[1] + "\n     " + Adr[r[1].Split(":")[0]][int.Parse(r[1].Split(":")[1])];

                    _Size = g.MeasureString(text, _Font).ToSize();
                    g.DrawImage(En, new Rectangle(_p, _Size));
                }
                g.DrawString(text, _Font, Brushes.Black, _p);
            }
            catch
            {
                Debug.Print("DrawEl - EQU" + _Adres);
            }
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
            Point _p = point;
            _p.X -= 50;
            g.DrawString(Teg, _Font, new SolidBrush(Tag), _p);
            _p.Y -= 15;
            g.DrawString(Com, _Font, new SolidBrush(this.Com), _p);
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
    /// Отрисовка NEQ
    /// </summary>
    public class NEQ : ElementDraw
    {
        private readonly Bitmap En = NodEn.Timer___Move;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        /// <param name="r">Список данных</param>
        public override void DrawEl(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr, string[] r)
        {
            try
            {
                string text;

                Point _p = point;
                int u;
                if (int.TryParse(r[1], out u))
                {
                    string[] buf = r[0].Split(":");
                    string name = buf[0];
                    int str = int.Parse(buf[1]);

                    text = "NEQ != \n     ?\n     " + r[1] + "\n     " + r[1] + "\n     " + Adr[name][str];
                    _Size = g.MeasureString(text, _Font).ToSize();
                    g.DrawImage(En, new Rectangle(_p, _Size));
                }
                else
                {
                    text = "NEQ != \n     " + r[0] + "\n     " + Adr[r[0].Split(":")[0]][int.Parse(r[0].Split(":")[1])] + "\n     "
                        + r[1] + "\n     " + Adr[r[1].Split(":")[0]][int.Parse(r[1].Split(":")[1])];

                    _Size = g.MeasureString(text, _Font).ToSize();
                    g.DrawImage(En, new Rectangle(_p, _Size));
                }
                g.DrawString(text, _Font, Brushes.Black, _p);
            }
            catch
            {
                Debug.Print("DrawEl - NEQ" + _Adres);
            }
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
            Point _p = point;
            _p.X -= 50;
            g.DrawString(Teg, _Font, new SolidBrush(Tag), _p);
            _p.Y -= 15;
            g.DrawString(Com, _Font, new SolidBrush(this.Com), _p);
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
    /// Отрисовка LES
    /// </summary>
    public class LES : ElementDraw
    {
        private readonly Bitmap En = NodEn.Timer___Move;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        /// <param name="r">Список данных</param>
        public override void DrawEl(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr, string[] r)
        {
            try
            {
                string text;

                Point _p = point;
                int u;
                if (int.TryParse(r[1], out u))
                {
                    string[] buf = r[0].Split(":");
                    string name = buf[0];
                    int str = int.Parse(buf[1]);

                    text = "LES < \n     ?\n     " + r[1] + "\n     " + r[1] + "\n     " + Adr[name][str];
                    _Size = g.MeasureString(text, _Font).ToSize();
                    g.DrawImage(En, new Rectangle(_p, _Size));
                }
                else
                {
                    text = "LES < \n     " + r[0] + "\n     " + Adr[r[0].Split(":")[0]][int.Parse(r[0].Split(":")[1])] + "\n     "
                        + r[1] + "\n     " + Adr[r[1].Split(":")[0]][int.Parse(r[1].Split(":")[1])];

                    _Size = g.MeasureString(text, _Font).ToSize();
                    g.DrawImage(En, new Rectangle(_p, _Size));
                }
                g.DrawString(text, _Font, Brushes.Black, _p);
            }
            catch
            {
                Debug.Print("DrawEl - LES" + _Adres);
            }
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
            Point _p = point;
            _p.X -= 50;
            g.DrawString(Teg, _Font, new SolidBrush(Tag), _p);
            _p.Y -= 15;
            g.DrawString(Com, _Font, new SolidBrush(this.Com), _p);
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
    /// Отрисовка LEQ
    /// </summary>
    public class LEQ : ElementDraw
    {
        private readonly Bitmap En = NodEn.Timer___Move;

        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        /// <param name="g">Инструмент отрисовки</param>
        /// <param name="_Adres">Адрес</param>
        /// <param name="point">Точка отрисовки</param>
        /// <param name="Adr">Таблица данных</param>
        /// <param name="r">Список данных</param>
        public override void DrawEl(Graphics g, string _Adres, Point point, Dictionary<string, ushort[]> Adr, string[] r)
        {
            try
            {

                Point _p = point;
                _p.Y -= 20;
                _p.X -= 45;
                _Size = new Size(120, 85);
                g.DrawImage(En, new Rectangle(_p, _Size));
                g.DrawString("LEQ <=", _Font, Brushes.Black, _p);
                _p.Y += 10;
                _p.X += 60;
                int u;
                if (int.TryParse(r[1], out u))
                {
                    string[] buf = r[0].Split(":");
                    string name = buf[0];
                    int str = int.Parse(buf[1]);
                    g.DrawString(r[0] + "\n" + Adr[name][str] + "\n" + r[1] + "\n" + r[1], _Font, Brushes.Black, _p);
                }
                else
                    g.DrawString(r[0] + "\n" + Adr[r[0].Split(":")[0]][int.Parse(r[0].Split(":")[1])] + "\n"
                        + r[1] + "\n" + Adr[r[1].Split(":")[0]][int.Parse(r[1].Split(":")[1])], _Font, Brushes.Black, _p);
            }
            catch
            {
                Debug.Print("DrawEl - LEQ" + _Adres);
            }
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
            Point _p = point;
            _p.X -= 50;
            g.DrawString(Teg, _Font, new SolidBrush(Tag), _p);
            _p.Y -= 15;
            g.DrawString(Com, _Font, new SolidBrush(this.Com), _p);
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