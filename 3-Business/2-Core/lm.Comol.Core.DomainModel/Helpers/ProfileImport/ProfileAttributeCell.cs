
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class ProfileAttributeCell 
    {
        bool invalid = false;
        public virtual ProfileAttributeColumn Column { get; set; }
        public virtual ProfileAttributesRow Row { get; set; }
        public virtual String Value { get; set; }
        public virtual Boolean isValid
        {
            get {
                Boolean result = ((Column.AllowEmpty || (!Column.AllowEmpty && !isEmpty))); // && (Column.AllowDuplicate || (!Column.AllowDuplicate && !isDuplicate)));
                switch (Column.Attribute) {
                    case Authentication.ProfileAttributeType.agencyInternalCode:
                        try
                        {
                            long internalId = 0;
                            if (long.TryParse(Value, out internalId) == true)
                                result = result && true;
                            else
                                result = false;
                        }
                        catch (Exception ex) {
                            result = false;
                        }

                        break;
                   case Authentication.ProfileAttributeType.mail:
                        result = result && IsValidEmail(Value);
                        break;

                    case ProfileAttributeType.birthDate:
                        try
                        {
                            DateTime dt = DateTime.Parse(Value);
                            result = true;
                        }
                        catch (Exception)
                        {
                            result = false;
                        }
                        break;
                        
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
        public ProfileAttributeCell() {
            DuplicateOf = new List<Int32>();
        }


        public void SetDuplicatedRows(List<Int32> indexes) {
            {
                DuplicateOf = indexes;
                Row.DuplicatedRows.AddRange(DuplicateOf.Except(Row.DuplicatedRows));

                //Cells.ForEach(c => indexes.AddRange(c.DuplicateOf));
                //return indexes.Distinct().ToList();
            } 
        }

        public static ProfileAttributeCell Update(ProfileAttributeCell cell, ProfileAttributeColumn col, ProfileAttributesRow row)
        {
            cell.Column = col;
            cell.Row = row;
            return cell;
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