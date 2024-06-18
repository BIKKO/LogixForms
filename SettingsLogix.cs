namespace LogixForms
{
    public partial class SettingsLogix : Form
    {
        private Dictionary<string, ushort[]> adres;
        private List<int> rows = new List<int>();
        private string[] name_adr;
        Dictionary<string, ushort> adres_adr;
        public SettingsLogix(Dictionary<string, ushort[]> adreses, Dictionary<string, ushort> mbadres)
        {
            InitializeComponent();
            adres = adreses;
            adres_adr = mbadres;
        }


        public void UpdateGrid()
        {
            dataGridView1.RowCount = adres.Count;
            name_adr = adres.Keys.ToArray();
            for (int i = 0; i < adres.Count; i++)
            {
                dataGridView1.Rows[i].Cells[1].Value = name_adr[i];
                dataGridView1.Rows[i].Cells[2].Value = adres_adr[name_adr[i]];
                dataGridView1.Rows[i].Cells[3].Value = adres[name_adr[i]].Length;
            }
        }

        private void SettingsLogix_Load(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Index == e.RowIndex)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value))
                    {
                        row.Cells[0].Value = false;
                        rows.Remove(e.RowIndex);
                    }
                    else
                    {
                        row.Cells[0].Value = true;
                        rows.Add(e.RowIndex);
                    }
                }
                else continue;
            }
            if (rows.Count > 0)
            {
                if (!Delite.Enabled)
                {
                    Delite.Enabled = true;
                    Ref.Enabled = true;
                }
            }
            else
            {
                Delite.Enabled = false;
                Ref.Enabled = false;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Index == e.RowIndex)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            row.Cells[0].Value = false;
                            rows.Remove(e.RowIndex);
                        }
                        else
                        {
                            row.Cells[0].Value = true;
                            rows.Add(e.RowIndex);
                        }
                    }
                    else continue;
                }
            }
            if (rows.Count > 0)
            {
                if (!Delite.Enabled)
                {
                    Delite.Enabled = true;
                    Ref.Enabled = true;
                }
            }
            else
            {
                Delite.Enabled = false;
                Ref.Enabled = false;
            }
        }

        /// <summary>
        /// Показ адреса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ref_Click(object sender, EventArgs e)
        {
            
            if (Application.OpenForms["ChangeAdres"] == null)
            {
                new ChangeAdres(rows,adres, adres_adr,this).Show();
            }
        }

        /// <summary>
        /// Удаление выбранного адреса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delite_Click(object sender, EventArgs e)
        {
            foreach (int ind in rows)
            {
                dataGridView1.Rows[ind].Cells[0].Value = false;
                adres.Remove(name_adr[ind]);
            }
            rows.Clear();
            Delite.Enabled = false;
            Ref.Enabled = false;
            UpdateGrid();
        }

        /// <summary>
        /// Добавление нового адреса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBut_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["AddAdres"] == null)
            {
                new AddAdres(adres, adres_adr, this).Show();
            }
        }
    }
}
