using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoWorkingDayHour
    {
        public virtual long Id { get; set; }
        public virtual long IdCalendar { get; set; }
        public virtual long IdProject { get; set; }
        public virtual FlagDayOfWeek DayOfWeek { get; set; }
        public virtual Int32 FromHour { get; set; }
        public virtual Int32 FromMinutes { get; set; }
        public virtual Int32 ToHour { get; set; }
        public virtual Int32 ToMinutes { get; set; }
        public dtoWorkingDayHour()
        {
            FromHour = 8;
            FromMinutes = 0;
            ToHour = 12;
            ToMinutes = 00;
        }
        public dtoWorkingDayHour(Int32 fromHour, Int32 fromMinutes, Int32 toHour, Int32 toMinutes)
        {
            FromHour = fromHour;
            FromMinutes = fromMinutes;
            ToHour = toHour;
            ToMinutes = toMinutes;
        }

        public dtoWorkingDayHour(WorkingDayHour wDay)
        {
            Id = wDay.Id;
            IdCalendar = (wDay.Calendar != null) ? wDay.Calendar.Id : 0;
            IdProject = (wDay.Project != null) ? wDay.Project.Id : 0;
            DayOfWeek = wDay.DayOfWeek;
            FromHour = wDay.FromHour;
            FromMinutes = wDay.FromMinutes;
            ToHour = wDay.ToHour;
            ToMinutes = wDay.ToMinutes;
        }

        public DateTime FromDate()
        {
            return DateTime.MinValue.AddHours(FromHour).AddMinutes(FromMinutes);
        }
        public DateTime ToDate()
        {
            return DateTime.MinValue.AddHours(ToHour).AddMinutes(ToMinutes);
        }
    }
}