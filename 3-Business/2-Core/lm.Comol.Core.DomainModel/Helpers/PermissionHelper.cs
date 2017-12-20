
using System;
namespace lm.Comol.Core.DomainModel
{
    public class PermissionHelper
	{
        public static String LongToBinStr(long value)
         {
            return Convert.ToString(value, 2);
            }

        public static long BinStrToLong(String value)
        {
            return Convert.ToInt64(value, 2);
        }
        public static long BinToLong(Boolean[] values)
        {
           String str = "";
            foreach (Boolean b in values)
            {
                str += b ? "1" : "0";
            }

            return BinStrToLong(str);
        }
        public static Boolean[] LongToBin(long value)
        {
            char[] str = LongToBinStr(value).ToCharArray();

            Boolean[] b = new Boolean[str.Length];

            for (int i = 0; i < b.Length; i++)
            {
                b[i] = str[i] == '1';
            }

            return b;
        }
		public static bool CheckPermissionSoft(long expected, long actual)
		{
			return (expected & actual) > 0;
		}
		public static bool CheckPermissionStrict(long expected, long actual)
		{
			return (expected & actual) == expected;
		}

	}
}



//Namespace lm.Elle3.Core.PermissionUtility
//{
//    Public Static Class PermissionHelper
//    {
//       



//        



//    }
//}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik, @toddanglin
//Facebook: facebook.com/telerik
//=======================================================
