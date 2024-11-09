using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Сomputer_Equipment_Repair
{
    public partial class LoginHistory : Form
    {
        SqlConnection MyConnection;
        public LoginHistory()
        {
           
            try
            {
                InitializeComponent();
                save.FlatAppearance.BorderSize = 0;
                Exit.FlatAppearance.BorderSize = 0;
                MyConnection = new SqlConnection(@"Data Source=ADCLG1; Initial catalog=Computer_Equipment_Repair;Integrated Security=True");
                MyConnection.Open();
                string sql = $"SELECT * FROM LoginHistory";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, MyConnection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                int much = dataGridView1.RowCount;
                much--;
                lists.Text = "Всего записей: " + much.ToString();
                MyConnection.Close();
            }
            catch (Exception error_load)
            {
                MessageBox.Show(error_load.Message);
            }

        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();  // Закрывает Form2
            Application.OpenForms["Form1"].Show();
        }

        private void save_Click(object sender, EventArgs e)
        {
            MyConnection = new SqlConnection(@"Data Source=ADCLG1; Initial catalog=Computer_Equipment_Repair;Integrated Security=True");
            MyConnection.Open();
            dataGridView1.ClearSelection();
            string sql = $"SELECT * FROM LoginHistory order by  LoginTime DESC ";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, MyConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            int much = dataGridView1.RowCount;
            much--;
            lists.Text = "Всего записей: " + much.ToString();
            MyConnection.Close();
        }
    }
}
