using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.File
{
    public static class ZeroPadExtensions
    {
        public static String ZeroPad(this String str, int digits)
        {
            return str.PadLeft(digits, '0');
        }

        public static String ZeroPad(this int num, int digits)
        {
            return ZeroPad(num.ToString(), digits);
        }

        public static int CalcDigits(int count)
        {
            return (int)(Math.Log10(count)) + 1;
        }

        public static String ZeroPadFromCount(this String str, int count)
        {
            int digits = CalcDigits(count);
            return ZeroPad(str, digits);
        }

        public static String ZeroPadFromCount(this int num, int count)
        {

            return ZeroPadFromCount(num.ToString(), count);
        }
    }
}
