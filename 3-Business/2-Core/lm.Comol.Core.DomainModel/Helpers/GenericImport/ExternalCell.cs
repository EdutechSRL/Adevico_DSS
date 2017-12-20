
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class ExternalCell
    {
        bool invalid = false;
        public virtual ExternalColumn Column { get; set; }
        public virtual ExternalRow Row { get; set; }
        public virtual String Value { get; set; }

        public virtual Boolean isValid
        {
            get {
                Boolean result = ((Column.AllowEmpty || (!Column.AllowEmpty && !isEmpty))); 
                try{
                    switch (Column.InputType) {
                        case InputType.int16:
                            Int16 tmp16 = 0;
                            if (Int16.TryParse(Value, out tmp16) == true)
                                result = result && true;
                            else
                                result = false;
                            break;
                        case InputType.int32:
                            Int32 tmp32 = 0;
                            if (Int32.TryParse(Value, out tmp32) == true)
                                result = result && true;
                            else
                                result = false;
                            break;
                        case InputType.int64:
                            Int64 tmp64 = 0;
                            if (Int64.TryParse(Value, out tmp64) == true)
                                result = result && true;
                            else
                                result = false;
                            break;
                        case InputType.datetime:
                            DateTime tmp;

                            if (DateTime.TryParse(Value,out tmp) == true)
                                result = result && true;
                            else
                                result = false;
                            break;
                        case InputType.mail:
                            result = result && IsValidEmail(Value);
                            break;
                    }
                }
                catch( Exception ex){
                    result = false;
                }

            
                return result; //(!Column.AllowEmpty && !isEmpty) && (!Column.AllowDuplicate && !isDuplicate);
            }
        }
        public virtual Boolean isEmpty
        {
            get
            {
                return string.IsNullOrEmpty(Value);
            }
        }
        public virtual Boolean isDuplicate {
            get { return DuplicateOf.Any(); }
        }
        public virtual Boolean isDBduplicate { get; set; }
        public virtual List<Int32> DuplicateOf { get; set; }
        public ExternalCell()
        {
            DuplicateOf = new List<Int32>();
        }

        public void SetDuplicatedRows(List<Int32> indexes) {
            {
                DuplicateOf = indexes;
                Row.DuplicatedRows.AddRange(DuplicateOf.Except(Row.DuplicatedRows));
            } 
        }
        private bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper);
            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,
                   @"^(?("")(""[^""]+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17}))$",
                   RegexOptions.IgnoreCase);
        }
        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
    }
}