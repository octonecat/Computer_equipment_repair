using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DatabaseController;

namespace Сomputer_Equipment_Repair
{
    public partial class Done_request_operator : Form
    {
        int userID;
        public Done_request_operator(int user_ID)
        {
            InitializeComponent();
            userID = user_ID;
            save.FlatAppearance.BorderSize=0;
            exit.FlatAppearance.BorderSize=0;
            save.Enabled = false;

            user_ID = userID;

            SqlConnection MyConnection = new SqlConnection(@"Data Source=ADCLG1; Initial catalog=Computer_Equipment_Repair;Integrated Security=True");
            MyConnection.Open();
            SqlCommand cmd1;
            SqlDataReader reader;

            string requests = $"SELECT requestID FROM Requests";
            cmd1 = new SqlCommand(requests, MyConnection);

            reader = cmd1.ExecuteReader();

            List<string> item = new List<string>();

            while (reader.Read())
            {
                item.Add(reader["requestID"].ToString());
            }
            number_request.Items.AddRange(item.ToArray());
            reader.Close();
        }

        private void save_Click(object sender, EventArgs e)
        {
            try {
                DataBaseIntegration.CreateReport(Convert.ToInt32( number_request.Text));
                MessageBox.Show("Заказ завершен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception err) 
            {
                MessageBox.Show(err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            }

        private void exit_Click(object sender, EventArgs e)
        {
            Main main = new Main(userID);
            main.Show();
            this.Close();
        }

        private void number_request_SelectedIndexChanged(object sender, EventArgs e)
        {
            save.Enabled = true;
        }
    }
}
