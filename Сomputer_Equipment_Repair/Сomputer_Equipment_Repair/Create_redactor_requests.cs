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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Сomputer_Equipment_Repair
{
    public partial class Create_requests : Form
    {
        int userID;
        string role_;
        bool redactor_;
        public Create_requests(int user_ID, bool redactor, string role_id)
        {
            role_= role_id;
            this.redactor_ = redactor;
            userID = user_ID;
            InitializeComponent();
            exit.FlatAppearance.BorderSize = 0;
            save.FlatAppearance.BorderSize= 0;
            if (redactor)
            {
                label_stut.Text = "Редактирование";
            }
            else {
                label_stut.Text = "Создание";
            }


            comboBox_Models.TextChanged += (s, ev) => CheckFields();
            problem.TextChanged += (s, ev) => CheckFields();
            save.Enabled = false; // Кнопка заблокирована по умолчанию
            try
            {
                SqlConnection MyConnection = new SqlConnection(@"Data Source=ADCLG1; Initial catalog=Computer_Equipment_Repair;Integrated Security=True");
                MyConnection.Open();
                SqlCommand cmd1;
                SqlDataReader reader;
                if (redactor)
                {
                    SqlDataReader reader2;
                    nubber_of_request.Visible = true;
                    string requests = $"SELECT [Номер заказа] FROM For_client where [Статус заказа]='Новая заявка' and [Клиент]={user_ID}";
                    cmd1 = new SqlCommand(requests, MyConnection);
                    label_request.Visible = true;
                    reader2 = cmd1.ExecuteReader();

                    List<string> item = new List<string>();

                    while (reader2.Read())
                    {
                        item.Add(reader2["Номер заказа"].ToString());
                    }
                    nubber_of_request.Items.AddRange(item.ToArray());
                   
                    reader2.Close();
                }
                string sql = "SELECT computerTechModel FROM computerTechModel";
                cmd1 = new SqlCommand(sql, MyConnection);
                reader = cmd1.ExecuteReader();

                List<string> Models = new List<string>();

                while (reader.Read())
                {
                    Models.Add(reader["computerTechModel"].ToString());
                }

                comboBox_Models.Items.AddRange(Models.ToArray());

                // Закрываем SqlDataReader и соединение
                reader.Close();
                MyConnection.Close();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
             }


        }

        private void exit_Click(object sender, EventArgs e)
        {
            commisionscs commisionscs = new commisionscs(userID, role_);
            commisionscs.Show();
            this.Close();  // Закрывает Form2
        }

        private void CheckFields()
        {
            // Проверяем, что все нужные элементы не пустые
            bool allFilled = !string.IsNullOrWhiteSpace(comboBox_Models.Text) &&
                             !string.IsNullOrWhiteSpace(problem.Text);  

            // Включаем кнопку, если все поля заполнены
            save.Enabled = allFilled;
        }
        public void Create()
        {
            try
            {
                SqlConnection MyConnection = new SqlConnection(@"Data Source=ADCLG1; Initial catalog=Computer_Equipment_Repair;Integrated Security=True");
                MyConnection.Open();
                string ComDel2 = $"INSERT INTO Requests (startDate,computerTechModelID,problemDescription, requestsStatusID, clientID) values (GETDATE(), (Select computerTechModelID from computerTechModel where computerTechModel =N\'{comboBox_Models.Text}\' ),N\'{problem.Text}\',3, {userID})";
                SqlCommand cmd2 = new SqlCommand(ComDel2, MyConnection);
                cmd2.ExecuteNonQuery();
                MyConnection.Close();
                MessageBox.Show("Заявка отправлена","Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception error_for_registate_request)
            {
                MessageBox.Show(error_for_registate_request.Message);
            }
        } 
        public void Redactor()
        {
            try
            {
                SqlConnection MyConnection = new SqlConnection(@"Data Source=ADCLG1; Initial catalog=Computer_Equipment_Repair;Integrated Security=True");
                MyConnection.Open();
                string ComDel2 = $"Update Requests set [startDate] = GETDATE(),computerTechModelID = (Select computerTechModelID from computerTechModel where computerTechModel=N\'{comboBox_Models.Text}\' ),problemDescription= N\'{problem.Text}\' where requestID = N\'{nubber_of_request.Text}\'";
                SqlCommand cmd2 = new SqlCommand(ComDel2, MyConnection);
                cmd2.ExecuteNonQuery();
                MyConnection.Close();
                MessageBox.Show("Обновление успешно");
            }
            catch (Exception error_for_registate_request)
            {
                MessageBox.Show(error_for_registate_request.Message);
            }
        }
        private void save_Click(object sender, EventArgs e)
        {
                if (redactor_)
                    Redactor();
                else Create();
            
        }

        private void Create_redactor_requests_Load(object sender, EventArgs e)
        {
            // Подписка на события
            comboBox_Models.TextChanged += (s, ev) => CheckFields();
            problem.TextChanged += (s, ev) => CheckFields();

            // Начальное состояние кнопки — заблокировано
            save.Enabled = false;
        }
    }
}
