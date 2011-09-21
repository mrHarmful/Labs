using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1_Pankov_Rational
{
    public static class Util
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">n >= 0</param>
        /// <param name="d">d >= 0</param>
        /// <returns></returns>
        public static int GCD(int n, int d)
        {
            while (n!= 0 && d != 0)
                if (n > d)
                    n %= d;
                else
                    d %= n;
            return n + d;
        }

        public static int LCM(int a, int b)
        {
            return a * b / GCD(a, b);
        }
    }
}
