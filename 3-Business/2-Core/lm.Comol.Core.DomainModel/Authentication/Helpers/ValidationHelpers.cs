using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace lm.Comol.Core.Authentication.Helpers
{
    public class ValidationHelpers
    {
        private const String validationString = @"^(?("")(""[^""]+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17}))$";
        private Boolean invalid { get; set; }
        private Boolean ValidateEmail(string strIn, String rExpression = validationString)
        {
            Boolean invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper);
            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn, rExpression, RegexOptions.IgnoreCase);
            //return Regex.IsMatch(strIn,
            //       @"^(?("")(""[^""]+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
            //       @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17}))$",
            //       RegexOptions.IgnoreCase);
        }
        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
                invalid = false;
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

        public static Boolean Mail(String mail){
            ValidationHelpers h = new ValidationHelpers();
            return h.ValidateEmail(mail);
        }
        public static Boolean Mail(String mail, String rExpression)
        {
            ValidationHelpers h = new ValidationHelpers();
            return h.ValidateEmail(mail, (String.IsNullOrEmpty(rExpression) ? validationString : rExpression));
        }
    }
}
