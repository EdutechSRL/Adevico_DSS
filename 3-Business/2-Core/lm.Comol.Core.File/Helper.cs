using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace lm.Comol.Core.File
{
    public static class Helper
    {
        public static String StringReverse(String ToReverse)
        {
            Array arr = ToReverse.ToCharArray();
            Array.Reverse(arr);// reverse the string
            char[] c = (char[])arr;
            byte[] b = System.Text.Encoding.Default.GetBytes(c);
            return System.Text.Encoding.Default.GetString(b);
        }
        public static String PathCombine(String path1, String path2)
        {
            return Path.Combine(path1, path2);
        }
        public static String GetFileName(String path)
        {
            return System.IO.Path.GetFileName(path);
        }
    }
}
