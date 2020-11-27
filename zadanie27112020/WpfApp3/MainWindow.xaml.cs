using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public string FIo()
        {
            string fio = FIO.Text;
            int k = fio.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length;
            int vr = 0;
            char[] w = fio.ToCharArray();
            string prov = "";
            if (fio.Take(1).All(c => char.IsUpper(c))) vr = 1;
            for (int i = 0; i < w.Length; i++)
            {
                if (w[i] == ' ' && w[i + 1] >= 'А' && w[i + 1] <= 'Я') vr++;
            }
            if (k == 2 || k == 3)
            {
                if (vr == k) prov = "verno";
                else prov = "neverno";
            }
            else prov = "neverno";
            return prov;
        }

        public string Data()
        {
            string s = "";
            var dFormat = "dd.mm.yyyy";
            DateTime ddate;
            if (DateTime.TryParseExact(DataR.Text, dFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out ddate))
            {
                if (ddate <= DateTime.Today) s = "verno";
                else s = "neverno";
            }
            else s = "neverno";
            return s;
        }

        public string Tel()
        {
            string num = Num.Text;
            string num2 = "";
            string r = "";
            char[] c = num.ToCharArray();
            int kol = num.Length - 1;
            if (kol <= 11)
            {
                for (int i = 2; i < 12; i++)
                {
                    num2 += c[i];
                    if (num2.Take(10).All(p => char.IsNumber(p))) r = "verno";
                    else r = "neverno";
                }
            }
            else r = "neverno";
            return r;
        }

        public string Email()
        {
            string mail = Mail.Text;
            string prov = @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)";
            string res = "";
            if (Regex.IsMatch(mail, prov)) res = "verno";
            else res = "neverno";
            return res;
        }

        public int Prov()
        {
            int b = 0;
            string s = "";
            string fio = FIo();
            if (fio != "verno") b = 1;
            string dt = Data();
            if (dt != "verno") b = 2;
            string tel = Tel();
            if (tel != "verno") b = 3;
            string mail = Email();
            if (mail != "verno") b = 4;
            return b;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Prov() == 1) MessageBox.Show("Ошибка в вводе ФИО!");
            else if (Prov() == 2) MessageBox.Show("Ошибка в вводе Даты Рождения!");
            else if (Prov() == 3) MessageBox.Show("Ошибка в вводе Телефона!");
            else if (Prov() == 4) MessageBox.Show("Ошибка в вводе E-mail!");
            else
            {
                //Добавить
                DateTime dataz = DateTime.Today;
                string textz = TextZ.Text;
                string fio = FIO.Text;
                string datar = DataR.Text;
                string phone = Num.Text;
                string email = Mail.Text;
                string ssql = $"INSERT INTO Zaiavki (DataZaiavki,TextZaiavki,FIO,DataR,Phone,Email) VALUES ('{dataz}','{textz}','{fio}','{datar}','{phone}','{email}')"; 
                string connectionString = @"Data Source=DESKTOP-CDQ6MB6\SQLEXPRESS;Initial Catalog=zaivki;Integrated Security=True"; 
                SqlConnection conn = new SqlConnection(connectionString); 
                conn.Open();
                SqlCommand command = new SqlCommand(ssql, conn);
                MessageBox.Show("Данные добавлены");
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Info.Text +=  "Id заявки: " + reader[0] + "\nДата: " + reader[1].ToString() + "\nЗаявка: " + reader[2].ToString() + "\nФИО: " + reader[3].ToString() + "\nДата Рождения: " + reader[4].ToString() + "\nТелефон: " + reader[5].ToString() + "\nE-mail: " + reader[6].ToString() + "\n";
                }
                reader.Close();
                TextZ.Clear();
                FIO.Clear();
                DataR.Clear();
                Num.Clear();
                Mail.Clear();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Вывести
            Info.Clear();
            string ssql = $"SELECT  * FROM Zaiavki "; 
            string connectionString = @"Data Source=DESKTOP-CDQ6MB6\SQLEXPRESS;Initial Catalog=zaivki;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString); 
            conn.Open();
            SqlCommand command = new SqlCommand(ssql, conn);
            SqlDataReader reader = command.ExecuteReader(); 
            while (reader.Read()) 
            {
                Info.Text += "Id заявки: " + reader[0] + "\nДата заявки: " + reader[1].ToString() + "\nЗаявка: " + reader[2].ToString() + "\nФИО: " + reader[3].ToString() + "\nДата Рождения: " + reader[4].ToString() + "\nТелефон: " + reader[5].ToString() + "\nE-mail: " + reader[6].ToString() + "\n" + "\n";
            }
            reader.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Удалить
            string ssql = @"DELETE FROM Zaiavki WHERE id = @Id";
            string connectionString = @"Data Source=DESKTOP-CDQ6MB6\SQLEXPRESS;Initial Catalog=zaivki;Integrated Security=True"; 
            SqlConnection conn = new SqlConnection(connectionString); 
            SqlCommand command = new SqlCommand(ssql, conn);
            command.Parameters.Add("@Id", SqlDbType.Int).Value = Convert.ToInt32(ID1.Text);
            conn.Open();
            command.ExecuteNonQuery();
            MessageBox.Show("Данные удалены");
            ID1.Clear();
        }

        int click;

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //Редактировать
            string connectionString = @"Data Source=DESKTOP-CDQ6MB6\SQLEXPRESS;Initial Catalog=zaivki;Integrated Security=True";
            if (!(String.IsNullOrWhiteSpace(ID1.Text)))
            {
                click++;
                if (click % 2 == 1)
                {
                    SqlConnection conn = new SqlConnection(connectionString);
                    string ssql = $"SELECT  * FROM Zaiavki WHERE id = @id";
                    conn.Open();
                    SqlCommand command = new SqlCommand(ssql, conn);
                    command.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(ID1.Text);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        TextZ.Text = reader[2].ToString();
                        FIO.Text = reader[3].ToString();
                        DataR.Text = reader[4].ToString();
                        Num.Text = reader[5].ToString();
                        Mail.Text = reader[6].ToString();
                    }
                    reader.Close();       
                }
                else if(click % 2 == 0)
                {
                    string ssql2 = @"UPDATE Zaiavki SET [TextZaiavki] = @TextZaiavki WHERE id = @id";
                    string ssql3 = @"UPDATE Zaiavki SET [FIO] = @FIO WHERE id = @id";
                    string ssql4 = @"UPDATE Zaiavki SET [DataR] = @DataR WHERE id = @id";
                    string ssql5 = @"UPDATE Zaiavki SET [Phone] = @Num WHERE id = @id";
                    string ssql6 = @"UPDATE Zaiavki SET [Email] = @Mail WHERE id = @id";
                    SqlConnection conn2 = new SqlConnection(connectionString);
                    SqlConnection conn3 = new SqlConnection(connectionString);
                    SqlConnection conn4 = new SqlConnection(connectionString);
                    SqlConnection conn5 = new SqlConnection(connectionString);
                    SqlConnection conn6 = new SqlConnection(connectionString);

                    conn2.Open();
                    SqlCommand command2 = new SqlCommand(ssql2, conn2);
                    command2.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(ID1.Text);
                    command2.Parameters.Add("@TextZaiavki", SqlDbType.NVarChar).Value = TextZ.Text;
                    SqlDataReader reader2 = command2.ExecuteReader(); 
                    while (reader2.Read()) 
                    {
                        Info.Text += "Id заявки: " + reader2[0] + "\nДата заявки: " + reader2[1].ToString() + "\nЗаявка: " + reader2[2].ToString() + "\nФИО: " + reader2[3].ToString() + "\nДата Рождения: " + reader2[4].ToString() + "\nТелефон: " + reader2[5].ToString() + "\nE-mail: " + reader2[6].ToString() + "\n";
                    }
                    reader2.Close();

                    conn3.Open();
                    SqlCommand command3 = new SqlCommand(ssql3, conn3);
                    command3.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(ID1.Text);
                    command3.Parameters.Add("@FIO", SqlDbType.NVarChar).Value = FIO.Text;
                    SqlDataReader reader3 = command3.ExecuteReader();
                    while (reader3.Read())
                    {
                        Info.Text += "Id заявки: " + reader3[0] + "\nДата заявки: " + reader3[1].ToString() + "\nЗаявка: " + reader3[2].ToString() + "\nФИО: " + reader3[3].ToString() + "\nДата Рождения: " + reader3[4].ToString() + "\nТелефон: " + reader3[5].ToString() + "\nE-mail: " + reader3[6].ToString() + "\n";
                    }
                    reader3.Close();

                    conn4.Open();
                    SqlCommand command4 = new SqlCommand(ssql4, conn4);
                    command4.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(ID1.Text);
                    command4.Parameters.Add("@DataR", SqlDbType.NVarChar).Value = DataR.Text;
                    SqlDataReader reader4 = command4.ExecuteReader();
                    while (reader4.Read())
                    {
                        Info.Text += "Id заявки: " + reader4[0] + "\nДата заявки: " + reader4[1].ToString() + "\nЗаявка: " + reader4[2].ToString() + "\nФИО: " + reader4[3].ToString() + "\nДата Рождения: " + reader4[4].ToString() + "\nТелефон: " + reader4[5].ToString() + "\nE-mail: " + reader4[6].ToString() + "\n";
                    }
                    reader4.Close();

                    conn5.Open();
                    SqlCommand command5 = new SqlCommand(ssql5, conn5);
                    command5.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(ID1.Text);
                    command5.Parameters.Add("@Num", SqlDbType.NVarChar).Value = Num.Text;
                    SqlDataReader reader5 = command5.ExecuteReader();
                    while (reader5.Read())
                    {
                        Info.Text += "Id заявки: " + reader5[0] + "\nДата заявки: " + reader5[1].ToString() + "\nЗаявка: " + reader5[2].ToString() + "\nФИО: " + reader5[3].ToString() + "\nДата Рождения: " + reader5[4].ToString() + "\nТелефон: " + reader5[5].ToString() + "\nE-mail: " + reader5[6].ToString() + "\n";
                    }
                    reader5.Close();

                    conn6.Open();
                    SqlCommand command6 = new SqlCommand(ssql6, conn6);
                    command6.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(ID1.Text);
                    command6.Parameters.Add("@Mail", SqlDbType.NVarChar).Value = Mail.Text;
                    SqlDataReader reader6 = command6.ExecuteReader();
                    while (reader6.Read())
                    {
                        Info.Text += "Id заявки: " + reader6[0] + "\nДата заявки: " + reader6[1].ToString() + "\nЗаявка: " + reader6[2].ToString() + "\nФИО: " + reader6[3].ToString() + "\nДата Рождения: " + reader6[4].ToString() + "\nТелефон: " + reader6[5].ToString() + "\nE-mail: " + reader6[6].ToString() + "\n";
                    }
                    reader6.Close();

                    MessageBox.Show("Данные изменены");
                    ID1.Clear();
                    Info.Clear();
                    TextZ.Clear();
                    FIO.Clear();
                    DataR.Clear();
                    Num.Clear();
                    Mail.Clear();
                }
            }
            else MessageBox.Show("Ошибка!\nВведите ID!");
        }

    }
}
