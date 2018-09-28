using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.BusinessLogic
{
    public static class DateTimeExt
    {
        public static DateTime ValueOrMinDBTimeValue(DateTime? time)
        {
            return (time != null && time.HasValue) ? time.Value : new DateTime(1753, 1, 1);
        }

        public static DateTime ValueOrMinTimeValue(DateTime? time)
        {
            return (time != null && time.HasValue) ? time.Value : DateTime.MinValue;
        }

        public static DateTime ValueOrNowTimeValue(DateTime? time)
        {
            return (time != null && time.HasValue) ? time.Value : DateTime.Now;
        }

        public static DateTime ValueOrMaxDBTimeValue(DateTime? time)
        {
            return (time != null && time.HasValue) ? time.Value : new DateTime(9999, 12, 31);
        }

        public static DateTime ValueOrMaxTimeValue(DateTime? time)
        {
            return (time != null && time.HasValue) ? time.Value : DateTime.MaxValue;
        }
    }
}
