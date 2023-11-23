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
    public partial class SettingsLogix : Form
    {
        public SettingsLogix(Dictionary<string, ushort[]> adreses)
        {
            InitializeComponent();
        }
    }
}
