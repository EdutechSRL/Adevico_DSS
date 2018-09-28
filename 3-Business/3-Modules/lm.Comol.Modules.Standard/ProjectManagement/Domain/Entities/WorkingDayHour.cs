using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    public class WorkingDayHour :  DomainBaseObjectLiteMetaInfo<long> 
    {
        public virtual Project Project { get; set; }
        public virtual ProjectCalendar Calendar { get; set; }
        public virtual FlagDayOfWeek DayOfWeek { get; set; }
        public virtual Int32 FromHour { get; set; }
        public virtual Int32 FromMinutes { get; set; }
        public virtual Int32 ToHour { get; set; }
        public virtual Int32 ToMinutes { get; set; }
        public WorkingDayHour()
        {
            FromHour = 8;
            FromMinutes = 0;
            ToHour = 12;
            ToMinutes = 00;
        }
        public WorkingDayHour(Int32 fromHour, Int32 fromMinutes, Int32 toHour, Int32 toMinutes)
        {
            FromHour = fromHour;
            FromMinutes = fromMinutes;
            ToHour = toHour;
            ToMinutes = toMinutes;
        }

        public virtual DateTime FromDate()
        {
            return DateTime.MinValue.AddHours(FromHour).AddMinutes(FromMinutes);
        }
        public virtual DateTime ToDate()
        {
            return DateTime.MinValue.AddHours(ToHour).AddMinutes(ToMinutes);
        }
    }
}