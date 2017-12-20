using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Helpers.Export
{
    //INTANDO COSì!  SOLO X TEST!!!
    public class TagReplacer
    {
        public static String PageNumberCurrent = "[Document.PageCurrent]";
        public static String CreateDate = "[Document.CreateDate]";
        public static String CreateTime = "[Document.CreateTime]";
        //public static String PageNumberTotal = "[Document.PageTotal]";

        public static String ReplaceAll(
            String InputText,
            int CurrentPage)
        {
            String output = InputText.Replace(PageNumberCurrent, CurrentPage.ToString()).Replace(CreateDate, DateTime.Now.ToString("dd/MM/yyyy")).Replace(CreateTime, DateTime.Now.ToString("hh:mm"));
            return output;
        }

        public static Boolean HasTag(String text)
        {
            if (text.Contains(PageNumberCurrent) || text.Contains(CreateDate))
                return true;
            //if (text.Contains(PageNumberTotal))
            //    return true;

            return false;
        }
    }


}
