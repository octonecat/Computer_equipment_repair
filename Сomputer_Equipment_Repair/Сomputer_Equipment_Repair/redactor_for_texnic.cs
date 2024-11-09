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
    public partial class redactor_for_texnic : Form
    {
        bool Redactor_;
        string role_;
        int user_ID;


        public redactor_for_texnic(int userID, bool Redactor, string role_id)
        {
            InitializeComponent();
            exit.FlatAppearance.BorderSize = 0;
            save.FlatAppearance.BorderSize = 0;
            if (Redactor)
            {
                label1.Text = "Редактирование";
            }
            else
            {
                label1.Text = "Создание";
            }
            try
            {
                user_ID = userID;
                role_ = role_id;
                Redactor_=Redactor;
                SqlConnection MyConnection = new SqlConnection(@"Data Source=ADCLG1; Initial catalog=Computer_Equipment_Repair;Integrated Security=True");
                MyConnection.Open();
                SqlCommand cmd1, cmd2;
                SqlDataReader reader, reader2;

                string requests = $"SELECT requestID FROM Requests where masterID={userID}";
                cmd1 = new SqlCommand(requests, MyConnection);

                reader = cmd1.ExecuteReader();

                List<string> item = new List<string>();

                while (reader.Read())
                {
                    item.Add(reader["requestID"].ToString());
                }
                Number_request.Items.AddRange(item.ToArray());
                reader.Close();


                string details = $"SELECT repairParts FROM repairParts";
                cmd2 = new SqlCommand(details, MyConnection);

                reader2 = cmd2.ExecuteReader();

                List<string> detail= new List<string>();

                while (reader2.Read())
                {
                    detail.Add(reader2["repairParts"].ToString());
                }
                choose_details.Items.AddRange(detail.ToArray());
                MyConnection.Close();
                reader2.Close();

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        public void Insert() {
            try {
                SqlConnection MyConnection = new SqlConnection(@"Data Source=ADCLG1; Initial catalog=Computer_Equipment_Repair;Integrated Security=True");
                MyConnection.Open();
                SqlCommand cmd1,cmd2;
                SqlDataReader reader;
                //userID = user_ID;
                SqlDataAdapter adapter;
                DataSet ds;
                string requests = $"Insert repairPartsRequests (repairPartsID,RequestsID) Values ( (Select repairPartsID From repairParts where repairParts = N\'{choose_details.Text}\'), {Convert.ToInt32(Number_request.Text)}) ";
                string comments = $"Insert comments (requestID,message) VALUES ({Convert.ToInt32(Number_request.Text)},  N\'{problem.Text}\' )";
                cmd1 = new SqlCommand(requests, MyConnection);
                cmd2 = new SqlCommand(comments, MyConnection);
                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                MyConnection.Close();
                MessageBox.Show("Добавлено","Информация",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                string requests = $"Update repairPartsRequests set repairPartsID=(Select repairPartsID From repairParts where repairParts = N'{choose_details.Text}') where RequestsID = N\'{Number_request.Text}\' ";
                string comments = $"Update comments set message = N'{problem.Text}' where requestID= N'{Number_request.Text}'";
                cmd1 = new SqlCommand(requests, MyConnection);
                cmd2 = new SqlCommand(comments, MyConnection);
                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                MyConnection.Close();
                MessageBox.Show("Обнавлено!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            if(Redactor_)
                Update();
            else
                Insert();





        }

        private void exit_Click(object sender, EventArgs e)
        {
            commisionscs commisionscs = new commisionscs(user_ID, role_);
            this.Close();
            commisionscs.Show();
        }
    }
}
