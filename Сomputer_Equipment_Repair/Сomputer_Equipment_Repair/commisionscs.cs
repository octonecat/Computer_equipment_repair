using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;
using System.Data.Common;

namespace Сomputer_Equipment_Repair
{
    public partial class commisionscs : Form
    {
        int userID;
        string role_;
        SqlConnection MyConnection = new SqlConnection(@"Data Source=ADCLG1; Initial catalog=Computer_Equipment_Repair;Integrated Security=True");
        SqlDataAdapter adapter;
        DataSet ds;
        string sql;
        public commisionscs(int user_ID,string role)
        {
            InitializeComponent();
            role_ = role;
            userID = user_ID;
            button_delete.FlatAppearance.BorderSize = 0;
            button_Fillter.FlatAppearance.BorderSize = 0;
            button_order.FlatAppearance.BorderSize = 0;
            button_redactor.FlatAppearance.BorderSize = 0;
            button3.FlatAppearance.BorderSize = 0;
           
           
            MyConnection.Open();
            userID = user_ID;
           
            try
            {
                switch (role)
                {
                    case "Менеджер":
                        button_delete.Visible = true;
                        sql = $"SELECT   [Номер заказа],[Статус заказа],[Дата начала],[Дата окончания],Тип,Модель,[Описание проблемы],[Сообщения мастера],[Фамилия мастера],[Имя мастера],[Отчество мастера],[телефон мастера] FROM for_c where Клиент  ={user_ID}";
                        adapter = new SqlDataAdapter(sql, MyConnection);
                        ds = new DataSet();
                        adapter.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                        break;
                    case "Оператор":
                        sql = $"SELECT  [Номер заказа],[Статус заказа],[Дата начала],[Дата окончания],Тип,Модель,[Описание проблемы],[Сообщения мастера],[Фамилия мастера],[Имя мастера],[Отчество мастера],[телефон мастера] FROM for_c";
                        adapter = new SqlDataAdapter(sql, MyConnection);
                        ds = new DataSet();
                        adapter.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                        break;
                    case "Техник":
                        sql = $"SELECT   [Номер заказа],Тип,Модель,[Описание проблемы],[Сообщения мастера],Детали FROM for_c where Техник = {user_ID} ";
                        adapter = new SqlDataAdapter(sql, MyConnection);
                        ds = new DataSet();
                        adapter.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                        break;
                    case "Заказчик":
                        button_delete.Visible = true;
                        sql = $"SELECT   [Номер заказа],[Статус заказа],[Дата начала],[Дата окончания],Тип,Модель,[Описание проблемы],[Сообщения мастера],[Фамилия мастера],[Имя мастера],[Отчество мастера],[телефон мастера] FROM for_c where Клиент  ={user_ID}";
                        adapter = new SqlDataAdapter(sql, MyConnection);
                        ds = new DataSet();
                        adapter.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                        break;
                }
                MyConnection.Close();

                int much = dataGridView1.RowCount;
                much--;
                lists.Text = "Всего записей: " + much.ToString();
            }
            catch(Exception error_load)
            {
                MessageBox.Show(error_load.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Main main = new Main(userID);
            main.Show();
            this.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (role_)
            {
                case "Заказчик":
                    Create_requests create_Redactor_Requests = new Create_requests(userID, true, role_);
                    create_Redactor_Requests.Show();
                    this.Close();
                    break;
                case "Техник":
                    redactor_for_texnic  redactor_For_Texnic = new redactor_for_texnic(userID, true, role_);
                    redactor_For_Texnic.Show();
                    this.Close();
                    break;
                default:
                    redactor_for_Operator redactor_For_Operator = new redactor_for_Operator(userID, role_);
                    redactor_For_Operator.Show();
                    this.Hide();
                    break;
            }

        }

        private void button_order_Click(object sender, EventArgs e)
        {
            try
            {
                switch (role_)
                {
                    case "Техник":
                        sql = $"SELECT   [Номер заказа],[Статус заказа],[Дата начала],[Дата окончания],Тип,Модель,[Описание проблемы],[Сообщения мастера],[Фамилия мастера],[Имя мастера],[Отчество мастера],[телефон мастера] FROM for_c where Техник = {userID} order by [{comboBox_Order.Text}] ";
                        adapter = new SqlDataAdapter(sql, MyConnection);
                        ds = new DataSet();
                        adapter.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                        break;
                    case "Заказчик":
                        sql = $"SELECT  [Номер заказа],[Статус заказа],[Дата начала],[Дата окончания],Тип,Модель,[Описание проблемы],[Сообщения мастера],[Фамилия мастера],[Имя мастера],[Отчество мастера],[телефон мастера] FROM for_c where Клиент ={userID} order by [{comboBox_Order.Text}]";
                        adapter = new SqlDataAdapter(sql, MyConnection);
                        ds = new DataSet();
                        adapter.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                        break;
                    default:
                        sql = $"SELECT * FROM for_c order by {comboBox_Order.Text}";
                        adapter = new SqlDataAdapter(sql, MyConnection);
                        ds = new DataSet();
                        adapter.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                        break;
                }
                int much = dataGridView1.RowCount;
                much--;
                lists.Text = "Всего записей: " + much.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void comboBox_Filter_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox_Order_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (comboBox_Order.Text != null)
            {
                comboBox_Filter.Items.Clear();
                button_Fillter.Enabled = true;
                comboBox_Filter.Enabled = true;
                MyConnection.Open();
                SqlDataReader reader;

                string  requests = $"SELECT [{comboBox_Order.Text}] FROM For_client";
                SqlCommand cmd1 = new SqlCommand(requests, MyConnection);

                reader = cmd1.ExecuteReader();

                List<string> item = new List<string>();

                while (reader.Read())
                {
                    item.Add(reader[$"{comboBox_Order.Text}"].ToString());
                }
                comboBox_Filter.Items.AddRange(item.ToArray());
                reader.Close();
                MyConnection.Close();
            }
            else
            {
                
                button_Fillter.Enabled = false;
                comboBox_Filter.Enabled = false;
            }
           

        }

        private void button_Fillter_Click(object sender, EventArgs e)
        {
            try
            {
                switch (role_)
                {
                    case "Техник":
                        sql = $"SELECT   [Номер заказа],[Статус заказа],[Дата начала],[Дата окончания],Тип,Модель,[Описание проблемы],[Сообщения мастера] FROM For_client where Техник = {userID} and [{comboBox_Order.Text}] = '{comboBox_Filter.Text}' ";
                        adapter = new SqlDataAdapter(sql, MyConnection);
                        ds = new DataSet();
                        adapter.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                        break;
                    case "Заказчик":
                        sql = $"SELECT  [Номер заказа],[Статус заказа],[Дата начала],[Дата окончания],Тип,Модель,[Описание проблемы],[Сообщения мастера] FROM For_client where Клиент ={userID} and [{comboBox_Order.Text}] = '{comboBox_Filter.Text}'";
                        adapter = new SqlDataAdapter(sql, MyConnection);
                        ds = new DataSet();
                        adapter.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                        break;
                    default:
                        sql = $"SELECT * FROM For_client where  [{comboBox_Order.Text}] = [{comboBox_Filter.Text}]";
                        adapter = new SqlDataAdapter(sql, MyConnection);
                        ds = new DataSet();
                        adapter.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                        break;
                }
                int much = dataGridView1.RowCount;
                much--;
                lists.Text = "Всего записей: " + much.ToString();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox_Order_TextChanged(object sender, EventArgs e)
        {
            comboBox_Filter.Items.Clear();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            Delete_recuests delete_Recuests = new Delete_recuests(userID);
            delete_Recuests.Show();
            this.Close();
        }
    }
}
