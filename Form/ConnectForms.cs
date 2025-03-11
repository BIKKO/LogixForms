namespace LogixForms
{
    public partial class ConnectForms : Form
    {
        MainThread form1;
        private int isnumber;
        private int slave;
        private string ip_and_port;

        public ConnectForms(MainThread owner)
        {
            form1 = owner;
            InitializeComponent();
        }

        /// <summary>
        /// Отмена
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Обработка и передача полученных значений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            byte s;
            try
            {
                string[] ip_buf = { };
                    ip_and_port = IP.Text;
                    ip_buf = ip_and_port.Split('.');
                if (ip_buf.Length == 4 && byte.TryParse(textBox1.Text, out s))
                {
                    foreach (string ip in ip_buf)
                    {
                        if (!int.TryParse(ip, out isnumber)) continue;
                    }

                }
                else
                {
                    if (ip_buf.Length != 4)
                    {
                        MessageBox.Show("Не верно указан IP адрес: 255.255.255.255");
                        IP.Clear();
                        IP.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Не верно указан SlaveID");
                    }
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не верно указан IP адрес: встречено не число");
                return;
            }
            if (s != null && s != 0)
            {
                form1.Con(ip_and_port, Port.Text, ConfigSel.Checked, s);
                Close();
            }
            else
            {
                MessageBox.Show("Не вышло!");
                return;
            }
        }
    }
}
