using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
     [Serializable]
    public class dtoWorkingDay
    {
        public virtual dtoWorkingDayHour DefaultStart { get; set; }
        public virtual dtoWorkingDayHour DefaultEnd { get; set; }
        public virtual FlagDayOfWeek DayOfWeek { get; set; }
        public virtual Int32 HoursRange
        {
            get
            {
                if (DefaultStart != null && DefaultEnd != null && DefaultStart.FromDate() <= DefaultEnd.ToDate())
                    return DefaultEnd.ToHour - DefaultStart.FromHour;
                else
                    return 8;
            }
        }
        public virtual Int32 MinutesRange
        {
            get
            {
                if (DefaultStart != null && DefaultEnd != null && DefaultStart.FromDate() <= DefaultEnd.ToDate())
                    return DefaultEnd.ToMinutes - DefaultStart.ToMinutes;
                else
                    return 8;
            }
        }
        public dtoWorkingDay()
        {
            DayOfWeek = FlagDayOfWeek.AllWeek;
        }

        public static dtoWorkingDay GetDefault()
        {
            return  new dtoWorkingDay()
            {
                DefaultStart = new dtoWorkingDayHour(8, 0, 12, 0) { DayOfWeek = FlagDayOfWeek.AllWeek }
                ,
                DefaultEnd = new dtoWorkingDayHour(13, 0, 17, 0) { DayOfWeek = FlagDayOfWeek.AllWeek }
            };
        }
    }
}