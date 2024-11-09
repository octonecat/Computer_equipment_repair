using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.BarCode;
using Aspose.BarCode.Generation;

namespace Сomputer_Equipment_Repair
{
    public partial class Main : Form
    {
        string role;
        int user_ID;
        public Main(int userId)
        {

            InitializeComponent();
           
            this.Size = new Size(816, 452);
            user_ID = userId;
            Create_to_request.FlatAppearance.BorderSize = 0;
            Look_to_requests.FlatAppearance.BorderSize = 0;
            button_Compleate.FlatAppearance.BorderSize = 0;
            Exit.FlatAppearance.BorderSize = 0;
            button_His.FlatAppearance.BorderSize = 0;
            try
            {
                SqlConnection MyConnection = new SqlConnection(@"Data Source=ADCLG1; Initial catalog=Computer_Equipment_Repair;Integrated Security=True");
                MyConnection.Open();
                string sql = $"SELECT Name, Surname,role  FROM Roles_and_Users Where userID ={userId} ";

                SqlCommand cmd1 = new SqlCommand(sql, MyConnection);
                string Name_surname = "";
                string Role = "";


                using (SqlDataReader reader = cmd1.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Role = reader["role"].ToString();
                        Name_surname = reader["Name"].ToString() +" "+ reader["Surname"].ToString();

                    }
                }
                MyConnection.Close();

                role = Role;
                switch (role)
                {
                    case "Заказчик":
                        labelQR.Visible=true;
                        pictureBoxQR_CODE.Visible=true;
                        panel_create.Visible = true;
                        panelExit.Location = new Point(31, 308);
                        
                        panel_Compleate.Visible = false;
                        GenerateQRcode();
                        break;
                    case "Оператор":
                        labelQR.Visible = false;
                        pictureBoxQR_CODE.Visible = false;
                        panelExit.Location= new Point(31, 190);
                        panel_Compleate.Location = new Point(31, 250);
                        label_my_requests.Text = "Список заказов:\n-Просматривайте и редактируйте заказы";
                        panel_Compleate.Visible = true;
                        panel_create.Visible = false;
                        break;
                    case "Техник":
                        panel_Compleate.Visible = false;

                        labelQR.Visible = false;
                        pictureBoxQR_CODE.Visible = false;
                        label_my_requests.Text = "Список заказов:\n-Редактируйте комментарии и детали к заказу";
                        label_Create.Text = "Создайте коментарии:\n-Добавте коментарии\nи необходимые детали к заказу ";
                        panel_create.Visible = true;
                        panelExit.Location = new Point(31, 308);
                        break;
                    case "Менеджер":
                        labelQR.Visible = false;
                        pictureBoxQR_CODE.Visible = false;
                        panelExit.Location = new Point(31, 190);
                        label_my_requests.Text = "Список заказов:\n-Просматривайте и редактируйте заказы";
                        panel_Compleate.Location = new Point(31,250);
                        panel_Compleate.Visible = true;
                        panel_create.Visible = false;
                        break;
                }
        
                label_hellow.Text = "Здравстуйте "+Name_surname;
                label_role.Text += Role;
               



            }
            catch(Exception error_conect) {
                MessageBox.Show(error_conect.Message);
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            commisionscs commisionscs = new commisionscs(user_ID, role);
            this.Close();
            commisionscs.Show();

        }
        private void GenerateQRcode()
        {
            if (File.Exists("Generated_QR_Code.png")){
                pictureBoxQR_CODE.Image = Image.FromFile("Generated_QR_Code.png");
            }
            else
            {
               
                BarcodeGenerator barcodeGenerator = new BarcodeGenerator(EncodeTypes.QR);
                barcodeGenerator.CodeText = "https://drive.google.com/drive/folders/12Z6HqS6soiBa9bn3IUfRi_izZeVkCm2m";
                barcodeGenerator.Save("Generated_QR_Code.png", BarCodeImageFormat.Png);
                pictureBoxQR_CODE.Image = Image.FromFile("Generated_QR_Code.png");

            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();  // Закрывает Form2
            Application.OpenForms["Form1"].Show();
        }

        private void Create_to_request_Click(object sender, EventArgs e)
        {
            switch (role)
            {
                case "Заказчик":
                    Create_requests create_Redactor_Requests = new Create_requests(user_ID, false, role);
                    create_Redactor_Requests.Show();
                    this.Hide();
                    break;
                case "Техник":
                    redactor_for_texnic redactor_For_Texnic = new redactor_for_texnic(user_ID, false, role);
                    redactor_For_Texnic.Show();
                    this.Hide();
                    break;


            }
           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            LoginHistory loginHistory = new LoginHistory();
            loginHistory.Show();
            this.Close();
        }

        private void button_Compleate_Click(object sender, EventArgs e)
        {
            Done_request_operator done_Request_Operator = new Done_request_operator(user_ID);
            done_Request_Operator.Show();
            this .Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
 