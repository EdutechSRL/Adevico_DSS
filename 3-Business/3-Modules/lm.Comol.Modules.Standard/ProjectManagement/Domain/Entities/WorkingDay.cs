using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    public class WorkingDay
    {
        public virtual WorkingDayHour Start { get; set; }
        public virtual WorkingDayHour End { get; set; }
        public virtual FlagDayOfWeek DayOfWeek { get; set; }
        public virtual Int32 HoursRange
        {
            get
            {
                if (Start != null && End != null && Start.FromDate() <= End.ToDate())
                    return End.ToHour - Start.FromHour;
                else
                    return 8;
            }
        }
        public virtual Int32 MinutesRange
        {
            get
            {
                if (Start != null && End != null && Start.FromDate() <= End.ToDate())
                    return End.ToMinutes - Start.ToMinutes;
                else
                    return 8;
            }
        }
        public WorkingDay()
        {
            DayOfWeek = FlagDayOfWeek.AllWeek;
        }
        public static WorkingDay GetDefault()
        {
            return new WorkingDay()
            {
                Start = new WorkingDayHour(8, 0, 12, 0) { DayOfWeek = FlagDayOfWeek.AllWeek }
                ,
                End = new WorkingDayHour(13, 0, 17, 0) { DayOfWeek = FlagDayOfWeek.AllWeek }
            };
        }
    }
}