using System;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;

namespace ConsoleApplication2
{
    class xCalc
    {
        static public string mul_div(string str)
        { // умножение и деление
            double na, nb;
            Regex reg = new Regex(@"(\-*[\d,]+)([\*\/]{1})(\-*[\d,]+)");
            Match mat = reg.Match(str);
            if (mat.Groups.Count == 4)
            {
                na = double.Parse(mat.Groups[1].ToString());
                nb = double.Parse(mat.Groups[3].ToString());
                if (mat.Groups[2].ToString()[0] == '*')
                    na *= nb;
                else if (mat.Groups[2].ToString()[0] == '/')
                    na /= nb;
                return str.Replace(mat.Groups[0].ToString(), na.ToString());
            }
            return str;
        }

        static public string add_sub(string str)
        { // сложение и вычитание
            double na, nb;
            Regex reg = new Regex(@"(\-*[\d,]+)([\+\-]{1})(\-*[\d,]+)");
            Match mat = reg.Match(str);
            if (mat.Groups.Count > 2)
            {
                na = double.Parse(mat.Groups[1].ToString());
                nb = double.Parse(mat.Groups[3].ToString());
                if (mat.Groups[2].ToString()[0] == '+')
                    na += nb;
                else if (mat.Groups[2].ToString()[0] == '-')
                    na -= nb;
                return str.Replace(mat.Groups[0].ToString(), na.ToString());
            }
            return str;
        }

        static public string scoba(string str)
        { // разбор скобок
            char[] buf = { '*', '/' };
            Regex reg = new Regex(@"\(([^\(\)]+)\)");
            if (!reg.IsMatch(str))
                return str;
            Match mat = reg.Match(str);
            string cm = mat.Groups[1].ToString();
            while (cm.IndexOfAny(buf) != -1)
                cm = xCalc.mul_div(cm);
            Regex test = new Regex(@"^(\-*[\d,]+)$");
            while (!test.IsMatch(cm))
                cm = xCalc.add_sub(cm);
            str = str.Replace(mat.Groups[0].ToString(), cm);
            return scoba(str);
        }

        static public double calc(string str)
        { // функция для расчёта
            str = scoba(str);
            char[] buf = { '*', '/' };
            while (str.IndexOfAny(buf) != -1)
                str = xCalc.mul_div(str);
            Regex test = new Regex(@"^(\-*[\d,]+)$");
            while (!test.IsMatch(str))
                str = xCalc.add_sub(str);
            return double.Parse(str);
        }
    };

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("{1}\t{0}", xCalc.calc("49/7+20*5-400"), "49/7+20*5-400");
            Console.WriteLine("{1}\t{0}", xCalc.calc("5*6+8*6"), "5*6+8*6");
            Console.WriteLine("{1}\t{0}", xCalc.calc("(5-4*(10-8))*(4-3)"), "(5-4*(10-8))*(4-3)");
            Console.WriteLine("{1}\t{0}", xCalc.calc("-7+8/2*(-200-100*2)"), "-7+8/2*(-200-100*2)");
            Console.WriteLine("{1}\t{0}\n\n", xCalc.calc("-4/-4+-5/-5"), "-4/-4+-5/-5");
            Console.ReadKey();
        }
    }
}