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

namespace Сomputer_Equipment_Repair
{
    public partial class redactor_for_Operator : Form
    {
        bool Redactor_;
        int user;
        string role_;
        public redactor_for_Operator(int user_ID, string role_id)
        {
            InitializeComponent();
            role_ = role_id;
            save.FlatAppearance.BorderSize = 0; ;
            exit.FlatAppearance.BorderSize= 0;
            try
            {
                SqlConnection MyConnection = new SqlConnection(@"Data Source=ADCLG1; Initial catalog=Computer_Equipment_Repair;Integrated Security=True");
                MyConnection.Open();
                SqlCommand cmd1, cmd2;
                SqlDataReader reader, reder2;
                //userID = user_ID;
                SqlDataAdapter adapter;
                DataSet ds;
                string requests = $"SELECT [Номер заказа],[Статус заказа] FROM For_client";
                string Texnics = $"Select Surname from Users where roleID=2 ";
                cmd1 = new SqlCommand(requests, MyConnection);
                cmd2 = new SqlCommand(Texnics, MyConnection);

                reader = cmd1.ExecuteReader();


                List<string> request = new List<string>();
                List<string> stutus = new List<string>();
                List<string> Surname = new List<string>();

                while (reader.Read())
                {
                    request.Add(reader["Номер заказа"].ToString());
                    stutus.Add(reader["Статус заказа"].ToString());


                }
                reader.Close();
                reder2 = cmd2.ExecuteReader();
                while (reder2.Read())
                {

                    Surname.Add(reder2["Surname"].ToString());

                }
                number_request.Items.AddRange(request.ToArray());
                status.Items.AddRange(stutus.ToArray());
                texnic.Items.AddRange(Surname.ToArray());
                reder2.Close();
                MyConnection.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }    
        public void Update()
        {
            try
            {
                SqlConnection MyConnection = new SqlConnection(@"Data Source=ADCLG1; Initial catalog=Computer_Equipment_Repair;Integrated Security=True");
                MyConnection.Open();
                SqlCommand cmd1, cmd2;
                SqlDataReader reader;
                //userID = user_ID;
                SqlDataAdapter adapter;
                DataSet ds;
                string requests = $"Update Requests set requestsStatusID=(Select requestStatusID From requestStatus where statusName = N\'{status.Text}\')," +
                    $" masterID = (Select userID from Users where Surname= N\'{texnic.Text}\') where requestID =N\'{number_request.Text}\'";
                cmd1 = new SqlCommand(requests, MyConnection);
                cmd1.ExecuteNonQuery();
                MyConnection.Close();
                MessageBox.Show("Обновление успешно");
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);

            }

        }


        private void save_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            commisionscs commisionscs = new commisionscs(user, role_);
            commisionscs.Show();
            this.Close();
        }
    }
}
