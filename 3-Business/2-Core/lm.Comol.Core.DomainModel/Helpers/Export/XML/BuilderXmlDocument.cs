using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace lm.Comol.Core.DomainModel.Helpers.Export
{
    public class BuilderXmlDocument
    {
        private static String DefaultExcelDocument(String content, String styles)
        {
            return "<?xml version=\"1.0\"?>" +
            "\r\n" +
            "<?mso-application progid=\"Excel.Sheet\"?>" +
            "\r\n" +
            "<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"" +
            "\r\n" + " xmlns:o=\"urn:schemas-microsoft-com:office:office\"" +
            "\r\n" + " " +
            "xmlns:x=\"urn:schemas-microsoft-com:office:" + "excel\"" +
            "\r\n" +
            " xmlns:ss=\"urn:schemas-microsoft-com:" + "office:spreadsheet\">" +
            styles +
            "\r\n" + " " +
            content +
            "\r\n" +
            "</Workbook>";
        }
        private static String DefaultStyles()
        {
            return "\r\n" + "<Styles>" + "\r\n" + " " +
            "<Style ss:ID=\"Default\" ss:Name=\"Normal\">" +
            "\r\n" + " " +
            "<Alignment ss:Vertical=\"Bottom\"/>" +
            "\r\n" +
            " <Borders/>" +
            "\r\n" +
            " <Font/>" +
            "\r\n" +
            " <Interior/>" +
            "\r\n" +
            " <NumberFormat/>" +
            "\r\n" +
            " <Protection/>" +
            "\r\n" +
            "</Style>" +
            "\r\n" + " " + "\r\n" +
            " <Style ss:ID=\"Date\">" +
            "\r\n" +
            " <NumberFormat ss:Format=\"d/m/yy\\ h\\.mm;@\"/>" +
            "\r\n" +
            " </Style>" +
            "\r\n" +
            " <Style ss:ID=\"Time\">" +
            "\r\n" +
            " <NumberFormat ss:Format=\"[$-F400]h:mm:ss\\ AM/PM\"/>" +
            "\r\n" +
            " </Style>" +
            "\r\n" +
            "</Styles>";
        }
        private static String UserStyles(List<string> userStyles)
        {
            String returnStyles = "\r\n" + "<Styles>" + "\r\n" + " " +
            "<Style ss:ID=\"Default\" ss:Name=\"Normal\">" +
            "\r\n" + " " +
            "<Alignment ss:Vertical=\"Bottom\"/>" +
            "\r\n" +
            " <Borders/>" +
            "\r\n" +
            " <Font/>" +
            "\r\n" +
            " <Interior/>" +
            "\r\n" +
            " <NumberFormat/>" +
            "\r\n" +
            " <Protection/>" +
            "\r\n" +
            "</Style>" +
            "\r\n" + " " + "\r\n" +
            " <Style ss:ID=\"Date\">" +
            "\r\n" +
            " <NumberFormat ss:Format=\"d/m/yy\\ h\\.mm;@\"/>" +
            "\r\n" +
            " </Style>" +
            "\r\n" +
            " <Style ss:ID=\"Time\">" +
            "\r\n" +
            " <NumberFormat ss:Format=\"[$-F400]h:mm:ss\\ AM/PM\"/>" +
            "\r\n" +
            " </Style>";

            foreach (String style in userStyles){

                returnStyles += "\r\n" + style;
            }
            returnStyles += "\r\n" + "</Styles>";
            return returnStyles;
        }
        public static String AddMain(String Content)
        { 
            return DefaultExcelDocument(Content,DefaultStyles());
        }
        public static String AddMain(String Content, List<string> userStyles)
        {
            String startExcelXML = DefaultExcelDocument(Content, UserStyles(userStyles));
           
            return startExcelXML;
        }
        public static String AddWorkSheet(String Name, String Content)
        { 
         String StrOut= "\r\n" + 
            "<Worksheet ss:Name=\"" + 
            Name +
            "\">" +
            "\r\n" + "\r\n" +
            "<Table>" +
            "\r\n" +
            "<Column ss:AutoFitWidth=\"1\" />" +
            "\r\n" +
            Content +
            "\r\n" +
            "</Table>" +
            "\r\n" + "\r\n" +
            "</Worksheet>" +
            "\r\n";
        return StrOut;
        }
        public static String AddRow(String cells)
        {
            return ("\r\n" + "<Row>" + cells + "\r\n" + "</Row>");
        }
        public static String AddEmptyRow()
        {
            return ("\r\n" + "<Row>" + " " + "\r\n" + "</Row>");
        }
        public static String AddEmptyRowWithCells(Int32 emptyCells)
        {
            string result = "\r\n" + "<Row>";
            (from i in Enumerable.Range(1, emptyCells) select i).ToList().ForEach(i=> result += AddDataEmpty());
            result += "</Row>";

            return result;
        }
        public static String AddEmptyRows(Int32 number)
        {
            number = (number < 0) ? 1 : number;
            string result = "";
            (from i in Enumerable.Range(1, number) select i).ToList().ForEach(i=> result += AddEmptyRow());
            return result;
        }
        public static String AddEmptyRows(Int32 number, Int32 emptyCells)
        {
            number = (number < 0) ? 1 : number;
            string result = "";
            (from i in Enumerable.Range(1, number) select i).ToList().ForEach(i => result += AddEmptyRowWithCells(emptyCells));
            return result;
        }
        //Numerici
        #region "Numerici"
            public static String AddData(Int16 Content)
            {
                return ("\r\n" + "<Cell><Data ss:Type=\"Number\">" + Content.ToString() + "</Data></Cell>");
            }
            public static String AddData(Int32 Content)
            {
                return ("\r\n" + "<Cell><Data ss:Type=\"Number\">" + Content.ToString() + "</Data></Cell>");
            }
            public static String AddData(Int64 Content)
            {
                return ("\r\n" + "<Cell><Data ss:Type=\"Number\">" + Content.ToString() + "</Data></Cell>");
            }
            public static String AddData(Decimal Content)
            {
                return ("\r\n" + "<Cell><Data ss:Type=\"Number\">" + Content.ToString() + "</Data></Cell>");
            }
            public static String AddData(Double Content)
            {
                return ("\r\n" + "<Cell><Data ss:Type=\"Number\">" + Content.ToString() + "</Data></Cell>");
            }
            public static String AddData(Single Content)
            {
                return ("\r\n" + "<Cell><Data ss:Type=\"Number\">" + Content.ToString() + "</Data></Cell>");
            }
            //public static String AddData(DateTime Content)
            //{
            //    return ("\r\n" + "<Cell><Data ss:Type=\"DateTime\">" + String.Format("{0:s}", Content) + "</Data></Cell>");
            //}

            public static String AddData(Int16 Content, String styleName)
            {
                return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "\"><Data ss:Type=\"Number\">" + Content.ToString() + "</Data></Cell>");
            }
            public static String AddData(Int32 Content, String styleName)
            {
                return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "\"><Data ss:Type=\"Number\">" + Content.ToString() + "</Data></Cell>");
            }
            public static String AddData(Int64 Content, String styleName)
            {
                return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "\"><Data ss:Type=\"Number\">" + Content.ToString() + "</Data></Cell>");
            }
            public static String AddData(Decimal Content, String styleName)
            {
                return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "\"><Data ss:Type=\"Number\">" + Content.ToString().Replace(",", ".") + "</Data></Cell>");
            }
            public static String AddData(Double Content, String styleName)
            {
                return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "\"><Data ss:Type=\"Number\">" + Content.ToString().Replace(",",".") + "</Data></Cell>");
            }
            public static String AddData(Single Content, String styleName)
            {
                return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "\"><Data ss:Type=\"Number\">" + Content.ToString().Replace(",",".") + "</Data></Cell>");
            }
            //public static String AddData(DateTime Content, String styleName)
            //{
            //    return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "\"><Data ss:Type=\"DateTime\">" + String.Format("{0:s}", Content) + "</Data></Cell>");
            //}
        #endregion
        //Stringhe e generici
        #region "Stringhe e generici"
            public static String AddDataEmpty()
            {
                return ("\r\n" + "<Cell><Data ss:Type=\"String\"> </Data></Cell>");
            }
            public static String AddData(String Content)
            {
                return ("\r\n" + "<Cell><Data ss:Type=\"String\">" + CheckString(Content) + "</Data></Cell>");
            }
            public static String AddData(Object Content)
            {
                return ("\r\n" + "<Cell><Data ss:Type=\"String\">" + CheckString(Content.ToString()) + "</Data></Cell>");
            }
            public static String AddEmptyData(String styleName)
            {
                return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "\"><Data ss:Type=\"String\"> </Data></Cell>");
            }
            public static String AddData(String Content, String styleName)
            {
                return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "\"><Data ss:Type=\"String\">" + CheckString(Content) + "</Data></Cell>");
            }
            public static String AddData(Object Content, String styleName)
            {
                return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "\"><Data ss:Type=\"String\">" + CheckString(Content.ToString()) + "</Data></Cell>");
            }
            public static String AddData(String Content, String styleName, int colspan)
            {
                return ("\r\n" + "<Cell ss:MergeAcross=\"" + colspan.ToString() + "\" ss:StyleID=\"" + styleName + "\"><Data ss:Type=\"String\">" + CheckString(Content) + "</Data></Cell>");
            }
            public static String AddData(Object Content, String styleName, int colspan)
            {
                return ("\r\n" + "<Cell ss:MergeAcross=\"" + colspan.ToString() + "\" ss:StyleID=\"" + styleName + "\"><Data ss:Type=\"String\">" + CheckString(Content.ToString()) + "</Data></Cell>");
            }


        #endregion

        //DateTime
        #region "DateTime"
            public static String AddData(DateTime Content)
            {
               return ("\r\n" + "<Cell ss:StyleID=\"Date\"><Data ss:Type=\"DateTime\">" + String.Format("{0:s}", Content) + "</Data></Cell>");
            }
            public static String AddData(TimeSpan Content)
            {
                DateTime dt = new DateTime(1899, 12, 31, Content.Hours, Content.Minutes, Content.Seconds);
                return ("\r\n" + "<Cell ss:StyleID=\"Time\"><Data ss:Type=\"DateTime\">" + String.Format("{0:s}", dt) + "</Data></Cell>");
            }
            public static String AddDataDateTime(String Content)
            {
                DateTime dt = System.Convert.ToDateTime(Content);
                return ("\r\n" + "<Cell ss:StyleID=\"Date\"><Data ss:Type=\"DateTime\">" + String.Format("{0:s}", dt) + "</Data></Cell>");
            }

            public static String AddData(DateTime Content, String styleName)
            {
                return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "Date\"><Data ss:Type=\"DateTime\">" + String.Format("{0:s}", Content) + "</Data></Cell>");
            }
            public static String AddData(TimeSpan Content, String styleName)
            {
                DateTime dt = new DateTime(1899, 12, 31, Content.Hours, Content.Minutes, Content.Seconds);
                return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "Time\"><Data ss:Type=\"DateTime\">" + String.Format("{0:s}", dt) + "</Data></Cell>");
            }
            public static String AddDataDateTime(String Content, String styleName)
            {
                DateTime dt = System.Convert.ToDateTime(Content);
                return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "Date\"><Data ss:Type=\"DateTime\">" + String.Format("{0:s}", dt) + "</Data></Cell>");
            }
        #endregion

        //Altri tipi
        #region "OtherType"
            public static String AddData(Boolean Content)
            { 
                if(Content)
                {
                    return ("\r\n" + "<Cell><Data ss:Type=\"Boolean\">1</Data></Cell>");
                }
                else
                {
                    return ("\r\n" + "<Cell><Data ss:Type=\"Boolean\">0</Data></Cell>");
                }
            }
            public static String AddData(Boolean Content,String styleName)
            {
                if (Content)
                {
                    return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "\" ><Data ss:Type=\"Boolean\">1</Data></Cell>");
                }
                else
                {
                    return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "\" ><Data ss:Type=\"Boolean\">0</Data></Cell>");
                }
            }
        #endregion

            //Linked numeric type
        #region "Linked numeric type"
            //public static String AddLinkData(Int16 Content, String Link)
            //{
            //    return ("\r\n" + "<Cell ss:HRef=\"" + Link + "\">" + "<Data ss:Type=\"Number\">") + Content.ToString() + "</Data></Cell>";
            //}
            //public static String AddLinkData(Int32 Content, String Link)
            //{
            //    return ("\r\n" + "<Cell ss:HRef=\"" + Link + "\">" + "<Data ss:Type=\"Number\">") + Content.ToString() + "</Data></Cell>";
            //}
            //public static String AddLinkData(Int64 Content, String Link)
            //{
            //    return ("\r\n" + "<Cell ss:HRef=\"" + Link + "\">" + "<Data ss:Type=\"Number\">") + Content.ToString() + "</Data></Cell>";
            //}
            //public static String AddLinkData(Decimal Content, String Link)
            //{
            //    return ("\r\n" + "<Cell ss:HRef=\"" + Link + "\">" + "<Data ss:Type=\"Number\">") + Content.ToString() + "</Data></Cell>";
            //}
            //public static String AddLinkData(Double Content, String Link)
            //{
            //    return ("\r\n" + "<Cell ss:HRef=\"" + Link + "\">" + "<Data ss:Type=\"Number\">") + Content.ToString() + "</Data></Cell>";
            //}
            //public static String AddLinkData(Single Content, String Link)
            //{
            //    return ("\r\n" + "<Cell ss:HRef=\"" + Link + "\">" + "<Data ss:Type=\"Number\">") + Content.ToString() + "</Data></Cell>";
            //}



            //public static String AddLinkData(Int16 Content, String Link, String styleName)
            //{
            //    return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "\" ss:HRef=\"" + Link + "\">" + "<Data ss:Type=\"Number\">") + Content.ToString() + "</Data></Cell>";
            //}
            //public static String AddLinkData(Int32 Content, String Link, String styleName)
            //{
            //    return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "\" ss:HRef=\"" + Link + "\">" + "<Data ss:Type=\"Number\">") + Content.ToString() + "</Data></Cell>";
            //}
            //public static String AddLinkData(Int64 Content, String Link, String styleName)
            //{
            //    return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "\" ss:HRef=\"" + Link + "\">" + "<Data ss:Type=\"Number\">") + Content.ToString() + "</Data></Cell>";
            //}
            //public static String AddLinkData(Decimal Content, String Link, String styleName)
            //{
            //    return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "\" ss:HRef=\"" + Link + "\">" + "<Data ss:Type=\"Number\">") + Content.ToString() + "</Data></Cell>";
            //}
            //public static String AddLinkData(Double Content, String Link, String styleName)
            //{
            //    return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "\" ss:HRef=\"" + Link + "\">" + "<Data ss:Type=\"Number\">") + Content.ToString() + "</Data></Cell>";
            //}
            //public static String AddLinkData(Single Content, String Link, String styleName)
            //{
            //    return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "\" ss:HRef=\"" + Link + "\">" + "<Data ss:Type=\"Number\">") + Content.ToString() + "</Data></Cell>";
            //}

        #endregion

            //Linked string type
        #region "Linked string type"
        //public static String AddLinkData(String Content, String Link)
        //{
        //    return ("\r\n" + "<Cell ss:HRef=\"" + Link + "\">" + "<Data ss:Type=\"String\">") + CheckString(Content) + "</Data></Cell>";
        //}

        //public static String AddLinkData(Object Content, String Link)
        //{
        //    return ("\r\n" + "<Cell ss:HRef=\"" + Link + "\">" + "<Data ss:Type=\"String\">") + CheckString(Content.ToString()) + "</Data></Cell>";
        //}

        //public static String AddLinkData(String Content, String Link, String styleName)
        //{
        //    return ("\r\n" + "<Cell ss:StyleID=\"" + styleName + "\" ss:HRef=\"" + Link + "\">" + "<Data ss:Type=\"String\">") + CheckString(Content) + "</Data></Cell>";
        //}

        //public static String AddLinkData(Object Content, String Link, String styleName)
        //{
        //    return ("\r\n" + "<Cell  ss:StyleID=\"" + styleName + "\" ss:HRef=\"" + Link + "\">" + "<Data ss:Type=\"String\">") + CheckString(Content.ToString()) + "</Data></Cell>";
        //}
        #endregion
        
        // Altro

        public static String GetInternalLink(String SheetName, String CellName)
        {
            if (CellName == "") { CellName = "A1"; }
            return ("#" + SheetName + "!" + CellName);
        }

        private static String CheckString(String input)
        {
            if (String.IsNullOrEmpty(input))
                return input;
            else
                return System.Web.HttpUtility.HtmlEncode(input);
        }

        #region "Styles"
     
        #endregion 
    }
}
