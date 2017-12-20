using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class TimestampAttribute : BaseUrlMacAttribute
    {
        public virtual TimestampFormat Format { get; set; }
        public virtual String UserFormat { get; set; }

        public TimestampAttribute()
        {
            Type = UrlMacAttributeType.timestamp;
            Format = TimestampFormat.utc;
            UserFormat = "";
        }

        public virtual DateTime? GetDate(string value) {
            DateTime? result = null;
            try
            {
                if (!String.IsNullOrEmpty(value))
                {
                    switch (Format)
                    {
                        case TimestampFormat.utc:
                            result = DateTime.Parse(value);
                            break;
                        case TimestampFormat.aaaammgghhmmss:
                            int year = DateTime.MinValue.Year, month = 1, day = 1, hour = 1, minutes = 1, second = 1;
                            if (value.Length>=4)
                                year = int.Parse(value.Substring(0,4));
                            if (value.Length>=6)
                                month = int.Parse(value.Substring(4,2));
                            if (value.Length >= 8)
                                day = int.Parse(value.Substring(6, 2));
                            if (value.Length >= 10)
                                hour = int.Parse(value.Substring(8, 2));
                            if (value.Length >= 12)
                                minutes = int.Parse(value.Substring(10, 2));
                            if (value.Length >= 14)
                                second = int.Parse(value.Substring(12, 2));
                            result = new DateTime(year, month, day, hour, minutes, second);
                            break;
                    }
                }
            }
            catch (FormatException ex)
            {
                switch (Format)
                {
                    case TimestampFormat.utc:
                        if (value.Contains("."))
                            result = DateTime.Parse(value.Replace(".", ":"));
                        else if (value.Contains(":"))
                            result = DateTime.Parse(value.Replace(":", "."));
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// Converte una stringa con data in formato ISO 8601 in datatime.
        /// </summary>
        /// <param name="ISOString">Stringa con data in formato ISO 8601.</param>
        /// <returns>Il datetime corrispondente</returns>
        public static DateTime FromISO8601ToDateTime(string ISOString)
        {
            return DateTime.Parse(ISOString);
        }

        /// <summary>
        /// Converte un DateTime in stringa formato ISO 8601
        /// </summary>
        /// <param name="Dt">DateTime da formattare</param>
        /// <returns>Stringa corretta</returns>
        public static string FromDataTimeToISO8610(DateTime Dt)
        {
            return Dt.ToString("s");
        }
    } 
}