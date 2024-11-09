using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using DatabaseController;

namespace Сomputer_Equipment_Repair
{
    public partial class Form1 : Form
    {
        private Random random = new Random();
        private string captchaText = "";
        int tryes=0;
        public Form1()
        {
            
            InitializeComponent();
            entrance.FlatAppearance.BorderSize = 0;

            Login.Text = "login7";
            Password.Text = "pass7";
            //Убрать!!!!
            textBoxAnswer.Text = captchaText;
        }
        public void LoginHistory() {
            int Suc;
            if (tryes >= 1)
                Suc = 1;
            else Suc=0;
            DataBaseIntegration.AddingToLoginhistory(Login.Text,Suc);
        }

        private void entrance_Click(object sender, EventArgs e)
        {
           
            if (check_capcha())
                try
                {
   
                    if ((Login.Text == "") || (Password.Text == "")) 
                    { MessageBox.Show("Заполните поля!","Пердупреждение",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tryes++;
                    }
                    else
                    {
                        if (DataBaseIntegration.IsPasswordCorrect(Login.Text,Password.Text))
                        {
                            int userId = Convert.ToInt32(DataBaseIntegration.GetUserDatabyLogin( Login.Text, "userID"));
                            LoginHistory();
                            Main main = new Main(userId);
                            main.Show();
                            this.Hide();
                        }
                        else 
                        {
                            if (DataBaseIntegration.IsLoginCorrect(Login.Text))
                            {
                                MessageBox.Show("Вы ввели неверный пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                tryes++;
                            }
                            else { 
                                MessageBox.Show("Вы ввели неверные данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                tryes++;
                            }
                        }
                    }
                    GenerateCaptcha();
                }
                catch { MessageBox.Show("Произошла ошибка! Попробуйте снова.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            else
                MessageBox.Show("Неправильно набранна капча!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void GenerateCaptcha()
        {
            // Создание случайной строки из 5 символов
            captchaText = GenerateRandomText(5);

            // Рендеринг изображения с текстом капчи
            Bitmap captchaImage = new Bitmap(200, 60);  // Изображение размером 200x60
            using (Graphics g = Graphics.FromImage(captchaImage))
            {
                g.Clear(Color.White);  // Задний фон капчи
                using (Font font = new Font("Arial", 24, FontStyle.Bold))
                {
                    g.DrawString(captchaText, font, Brushes.Black, new PointF(10, 10));
                }
                // Добавление некоторых линий для "шума"
                for (int i = 0; i < 10; i++)
                {
                    g.DrawLine(Pens.Gray, random.Next(0, captchaImage.Width), random.Next(0, captchaImage.Height),
                        random.Next(0, captchaImage.Width), random.Next(0, captchaImage.Height));
                }
            }
            pictureBoxCaptcha.Image = captchaImage;  // Отображение капчи
        }

        // Метод для генерации случайного текста
        private string GenerateRandomText(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }
            return new string(result);
        }

        // Метод для проверки ответа
        private bool check_capcha()
        {
            if (tryes == 0)
                return true;
            else if (tryes != 3 && tryes<=2)
            {
                panel2.Visible = true;
                if (textBoxAnswer.Text == captchaText)
                {
                    textBoxAnswer.Clear();
                    return true;
                }
                else
                {
                    tryes++;
                    GenerateCaptcha();
                    textBoxAnswer.Clear();
                    return false;
                }
            }
            else
            {
                
                MessageBox.Show("Превышение попыток!\n Повторная попытка через 3 минуты!","Вход не выполнен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                timer_block.Enabled = true;
                entrance.Enabled = false;
                panel1.Enabled = false;
                if (tryes > 3)
                    panel1.Enabled = false;
                return false;
            }

        }

        private void groupBox_Login_Enter(object sender, EventArgs e)
        {
                    }

        private void button1_Click(object sender, EventArgs e)
        {
            Password.UseSystemPasswordChar = !Password.UseSystemPasswordChar;
           

        }

        private void button2_Click(object sender, EventArgs e)
        {
            GenerateCaptcha();
        }

        private void timer_block_Tick(object sender, EventArgs e)
        {
            entrance.Enabled = true ;
            panel1.Enabled = true ;
            timer_block.Enabled=false;
        }
    }
}

