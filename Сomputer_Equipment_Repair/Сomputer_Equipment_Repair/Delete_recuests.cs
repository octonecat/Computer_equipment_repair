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
    public partial class Delete_recuests : Form
    {
        int user_ID;
        public Delete_recuests(int userID)
        {
            InitializeComponent();
            user_ID = userID;
            exit.FlatAppearance.BorderSize = 0;
            save.FlatAppearance.BorderSize = 0;
            save.Enabled = false;

            SqlConnection MyConnection = new SqlConnection(@"Data Source=ADCLG1; Initial catalog=Computer_Equipment_Repair;Integrated Security=True");
            MyConnection.Open();
            SqlCommand cmd1;
            SqlDataReader reader;

            string requests = $"SELECT requestID FROM Requests where clientID={userID}";
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
            try
            {
                DataBaseIntegration.DeleteRequest(Convert.ToInt32(number_request.Text));
                MessageBox.Show("Заказ удален", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Main main = new Main(user_ID);
            main.Show();
            this.Close();
        }

        private void number_request_SelectedIndexChanged(object sender, EventArgs e)
        {
            save.Enabled = true;
        }
    }
}
