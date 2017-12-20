using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class dtoCsvSettings
    {
        public virtual TextDelimiter RowDelimeter { get; set; }
        public virtual TextDelimiter ColumnDelimeter { get; set; }
        public virtual Boolean FirstRowColumnNames { get; set; }
        public virtual int RowsToSkip { get; set; }

        public virtual String[] GetColumnDelimiters
        {
            get {
                String[] delimeters = new String[] { GetDelimeterChar(ColumnDelimeter) };
                return delimeters;
            }
        }
        public virtual String[] GetRowDelimiters
        {
            get
            {
                String[]delimeters = new String[] {GetDelimeterChar(RowDelimeter)};
                return delimeters;
            }
        }
        private String GetDelimeterChar(TextDelimiter delimeter)
        {
            switch (delimeter) { 
                case TextDelimiter.Colon:
                    return ":";
                case TextDelimiter.Comma:
                    return ",";
                case TextDelimiter.Semicolon:
                    return ";";
                case TextDelimiter.Tab:
                    return "\t";
                case TextDelimiter.VerticalBar:
                    return "|";
                case TextDelimiter.Cr:
                    return "\r";
                case TextDelimiter.Lf:
                    return "\n";
                case TextDelimiter.CrLf:
                    return "\r\n";
            }
            return "";
        }

        //public virtual char GetColumnDelimeterChar
        //{
        //    get {
        //        return GetDelimeterChar(ColumnDelimeter);
        //    }
        //}
        //public virtual char GetRowDelimeterChar
        //{
        //    get
        //    {
        //        return GetDelimeterChar(RowDelimeter);
        //    }
        //}
        //private virtual char GetDelimeterChar(TextDelimiter delimeter)
        //{
        //    switch (delimeter) { 
        //        case TextDelimiter.Colon:
        //            return ':';
        //        case TextDelimiter.Comma:
        //            return ',';
        //        case TextDelimiter.Semicolon:
        //            return ';';
        //        case TextDelimiter.Tab:
        //            return '\t';
        //        case TextDelimiter.VerticalBar:
        //            return '|';
        //        case TextDelimiter.Cr:
        //            return (char)13; //\r
        //        case TextDelimiter.Lf:
        //            return (char)13;//\n
        //        case TextDelimiter.CrLf:
        //            return '\n';
        //    }
        //    return '';
        //}
    }
}