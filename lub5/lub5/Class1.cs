using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lub5
{
    public class Class1
    {
        public string FIo()
        {
            string fio = Console.ReadLine();
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
            string DataR = Console.ReadLine();
            string s = "";
            var dFormat = "dd.mm.yyyy";
            DateTime ddate;
            if (DateTime.TryParseExact(DataR, dFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out ddate))
            {
                if (ddate <= DateTime.Today) s = "verno";
                else s = "neverno";
            }
            else s = "neverno";
            return s;
        }

        public string Tel()
        {
            string num = Console.ReadLine();
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
            string mail = Console.ReadLine();
            string prov = @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)";
            string res = "";
            if (Regex.IsMatch(mail, prov)) res = "verno";
            else res = "neverno";
            return res;
        }
    }
}
