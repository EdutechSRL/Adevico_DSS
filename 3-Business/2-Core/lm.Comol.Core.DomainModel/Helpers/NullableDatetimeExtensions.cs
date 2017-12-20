using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    public static class NullableDatetimeExtensions
    {
        public static String ToShortDateString(this DateTime? date)
        {
            if (date != null && date.HasValue)
            {
                return date.Value.ToShortDateString();
            }
            else
            {
                return "";
            }
        }

        public static String ToShortTimeString(this DateTime? date)
        {
            if (date != null && date.HasValue)
            {
                return date.Value.ToShortTimeString();
            }
            else
            {
                return "";
            }
        }

        public static String ToLongDateString(this DateTime? date)
        {
            if (date != null && date.HasValue)
            {
                return date.Value.ToLongDateString();
            }
            else
            {
                return "";
            }
        }

        public static String ToLongTimeString(this DateTime? date)
        {
            if (date != null && date.HasValue)
            {
                return date.Value.ToLongTimeString();
            }
            else
            {
                return "";
            }
        }
    }
}
