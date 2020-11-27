using System;
using System.Collections.Generic;
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
            else MessageBox.Show("Данные сохранены!");
        }
    }
}
