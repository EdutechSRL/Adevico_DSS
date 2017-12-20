using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using System.IO;

namespace lm.Comol.Core.DomainModel.Helpers.Export
{
    [Serializable]
    public class ExportCsvBaseHelper
    {
        #region "Constants"
            public const string dEndRowChars = "\r\n";
            public const string dStringDelimiter = "\"";
            public const string dEndFieldItem = ";";
            public const char dCharDelimiter = (char)34;
            public const char dEndCharFieldItem =(char)59;
        #endregion

        #region ""
            public string EndRowItem { get; set; }
            public string StringDelimiter { get; set; }
            public string EndFieldItem { get; set; }
            public char CharDelimiter { get; set; }
            public char EndCharFieldItem { get; set; }
        #endregion
        
        public ExportCsvBaseHelper() {
            EndRowItem = dEndRowChars;
            StringDelimiter = dStringDelimiter;
            EndFieldItem = dEndFieldItem;
            CharDelimiter = dCharDelimiter;
            EndCharFieldItem = dEndCharFieldItem;
        }
        public ExportCsvBaseHelper(String endRow, char delimiter, char endField)
        {
            EndRowItem = endRow;
            StringDelimiter = delimiter.ToString();
            EndFieldItem = endField.ToString();
            CharDelimiter = delimiter;
            EndCharFieldItem = endField;
        }

        public String AppendRow()
        {
            return EndRowItem;
        }
        public String AddRow(String item)
        {
            return item+EndRowItem;
        }
        public String AppendRow(int number)
        {
            string rows = "";
            for(int i=1; i<= number;i++){
                rows += EndRowItem;
            }
            return rows;
        }
        public String AppendItem(String lineText, Boolean addFieldDelimiter = true )
        {
            if (string.IsNullOrEmpty(lineText))
                return EndFieldItem;
            else{
                return dStringDelimiter + lineText.Replace(dStringDelimiter, "''").Replace(EndRowItem, " ") + dStringDelimiter + ((addFieldDelimiter) ? EndFieldItem : "");
            }
        }

        public void AppendToRow(StringBuilder sBuilder, String lineText, Boolean addFieldDelimiter = true)
        {
            if (!String.IsNullOrEmpty(lineText))
            {
                sBuilder.Append(dStringDelimiter);
                sBuilder.Append(lineText.Replace(dStringDelimiter, "''").Replace(EndRowItem, " "));
                sBuilder.Append(dStringDelimiter);
            }
            if (addFieldDelimiter)
                sBuilder.Append(EndFieldItem);
            sBuilder.Append(EndRowItem);
        }
        public void AppendToRow(StringBuilder sBuilder, Int32 value, Boolean addFieldDelimiter = true)
        {
            sBuilder.Append(value);
            sBuilder.Append(((addFieldDelimiter) ? EndFieldItem : ""));
            sBuilder.Append(EndRowItem);
        }
        public void AppendToRow(StringBuilder sBuilder, Int64 value, Boolean addFieldDelimiter = true)
        {
            sBuilder.Append(value);
            sBuilder.Append(((addFieldDelimiter) ? EndFieldItem : ""));
            sBuilder.Append(EndRowItem);
        }
        public void AddEmptyRow(StringBuilder sBuilder, Int32 number = 1)
        {
            if (number > 1)
                Enumerable.Range(1, number - 1).ToList().ForEach(x => sBuilder.Append(EndRowItem));
            else
                sBuilder.Append(EndRowItem);
        }
        public void AddField(StringBuilder sBuilder, String lineText, Boolean addFieldDelimiter = true)
        {
            if (string.IsNullOrEmpty(lineText))
                sBuilder.Append(EndFieldItem);
            else
            {
                sBuilder.Append(dStringDelimiter);
                sBuilder.Append(lineText.Replace(dStringDelimiter, "''").Replace(EndRowItem, " "));
                sBuilder.Append(dStringDelimiter);
                sBuilder.Append(((addFieldDelimiter) ? EndFieldItem : ""));
            }
        }
        public void AddField(StringBuilder sBuilder, Double value, Boolean addFieldDelimiter = true)
        {
            //sBuilder.Append(dStringDelimiter);
            sBuilder.Append(value);
            //sBuilder.Append(dStringDelimiter);
            sBuilder.Append(((addFieldDelimiter) ? EndFieldItem : ""));
        }
        public void AddField(StringBuilder sBuilder, Int64 value, Boolean addFieldDelimiter = true)
        {
            sBuilder.Append(dStringDelimiter);
            sBuilder.Append(value);
            sBuilder.Append(dStringDelimiter);
            sBuilder.Append(((addFieldDelimiter) ? EndFieldItem : ""));
        }
        public void AddField(StringBuilder sBuilder, Int32 value, Boolean addFieldDelimiter = true)
        {
           // sBuilder.Append(dStringDelimiter);
            sBuilder.Append(value);
            //sBuilder.Append(dStringDelimiter);
            sBuilder.Append(((addFieldDelimiter) ? EndFieldItem : ""));
        }
        public void AddField(StringBuilder sBuilder, Decimal? item, Boolean addFieldDelimiter = true)
        {
            //sBuilder.Append(dStringDelimiter);
            if (item.HasValue)
                sBuilder.Append(item.Value.ToString());
            //sBuilder.Append(dStringDelimiter);
            if(addFieldDelimiter)
                sBuilder.Append(EndFieldItem);
        }
        public void AddField(StringBuilder sBuilder, DateTime? date, String dateTimeFormat, Boolean addFieldDelimiter = true)
        {
            //sBuilder.Append(dStringDelimiter);
            sBuilder.Append((date.HasValue) ? date.Value.ToString(dateTimeFormat) : "");
            //sBuilder.Append(dStringDelimiter);
            sBuilder.Append(((addFieldDelimiter) ? EndFieldItem : ""));
        }
        public void AddEmptyField(StringBuilder sBuilder, Int32 number = 1, Boolean addFieldDelimiter = true)
        {
            if (number > 1)
                Enumerable.Range(1, number-1).ToList().ForEach(x=> sBuilder.Append(EndFieldItem));
            if (addFieldDelimiter)
                sBuilder.Append(EndFieldItem);
        }

        public void AppendToRow(ref String rowFields, String lineText, Boolean addFieldDelimiter = true)
        {
            AddField(rowFields,lineText, addFieldDelimiter);
            rowFields += EndRowItem;
        }
        public void AddField(String rowFields, String lineText, Boolean addFieldDelimiter = true)
        {
            if (!String.IsNullOrEmpty(lineText))
                rowFields += dStringDelimiter + lineText.Replace(dStringDelimiter, "''").Replace(EndRowItem, " ") + dStringDelimiter;
            if (addFieldDelimiter)
                rowFields += EndFieldItem;
        }

        public String AddEmptyField(Int32 number = 1, Boolean addFieldDelimiter = true)
        {
            String result = "";
            if (number > 1)
                result = String.Join("", Enumerable.Range(1, number - 1).Select(n => EndFieldItem).ToList());
            if (addFieldDelimiter)
                result +=EndFieldItem;
            return result;
        }
        public String AppendItem(Double value)
        {
            return value.ToString() + EndFieldItem;
        }
        public String AppendItem(Int64 value)
        {
            return value.ToString() + EndFieldItem;
        }
        public String AppendItem(Int32 value)
        {
            return value.ToString() + EndFieldItem;
        }
        public String AppendItem(DateTime? date)
        {
            if (date.HasValue)
                return date.Value.ToShortDateString() + " " + date.Value.ToShortTimeString() + EndFieldItem;
            else
                return  EndFieldItem;
        }
        public String AppendItem(Decimal? item)
        {
            if (item.HasValue)
                return item.Value.ToString() + EndFieldItem;
            else
                return EndFieldItem;
        }
    }
}