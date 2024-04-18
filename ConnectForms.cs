using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogixForms
{
    public partial class ConnectForms : Form
    {
        MainThread form1;
        private int isnumber;
        private int slave;
        private string[] ip_and_port = new string[2];
        public ConnectForms(MainThread owner)
        {
            form1 = owner;
            InitializeComponent();
            for (int i = 1; i < 6; i++)
                Slave.Items.Add(i);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            slave = Slave.SelectedIndex + 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ip_and_port = IP.Text.Split(':');
                string[] ip_buf = ip_and_port[0].Split('.');
                if (ip_buf.Length == 4)
                {
                    foreach (string ip in ip_buf)
                    {
                        if (!int.TryParse(ip, out isnumber)) continue;
                    }

                }
                else
                {
                    MessageBox.Show("Не верно указан IP адрес: 255.255.255.255");
                    IP.Clear();
                    IP.Focus();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не верно указан IP адрес: встречено не число");
            }
            if (!int.TryParse(Step.Text, out isnumber)) MessageBox.Show("Введите число");

            if (ip_and_port.Length < 2)
            {
                comboBox1_SelectedIndexChanged(sender, e);
                form1.con(ip_and_port[0], "502", int.Parse(Step.Text), (byte)slave);
                Close();
            }
            else
            {
                comboBox1_SelectedIndexChanged(sender, e);
                form1.con(ip_and_port[0], ip_and_port[1], int.Parse(Step.Text), (byte)slave);
                Close();
            }
        }
    }
}
