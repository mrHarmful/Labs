using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1_Pankov_Rational
{
    public static class Util
    {
        public static int GCD(int n, int d)
        {
            while (n * d > 0)
                if (n > d)
                    n %= d;
                else
                    d %= n;
            return n + d;
        }

        public static int GCF(int a, int b)
        {
            return a * b / GCD(a, b);
        }
    }
}
