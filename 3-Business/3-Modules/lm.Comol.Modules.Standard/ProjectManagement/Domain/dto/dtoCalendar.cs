using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoCalendar 
    {
        public virtual long Id { get; set; }
        public virtual long IdResource { get; set; }
        public virtual String Name { get; set; }
        public virtual CalendarType Type { get; set; }
        public virtual FlagDayOfWeek DaysOfWeek { get; set; }
        public virtual List<dtoDateException> DateExceptions { get; set; }
        public virtual List<dtoWorkingDayHour> WorkingDaysHour { get; set; }
        public virtual dtoWorkingDay DefaultWorkingDay { get; set; }

        public dtoCalendar()
        {
            DateExceptions = new List<dtoDateException>();
            WorkingDaysHour = new List<dtoWorkingDayHour>();
            DefaultWorkingDay = new dtoWorkingDay(){ DefaultStart= new dtoWorkingDayHour(8, 0, 12, 0) { DayOfWeek = FlagDayOfWeek.AllWeek }
                , DefaultEnd= new dtoWorkingDayHour(13, 0, 17, 0) { DayOfWeek = FlagDayOfWeek.AllWeek }};
        }

        public static List<dtoWorkingDayHour> GetDefaultWorkingDayHours(FlagDayOfWeek days, long idCalendar=0)
        {
            List<dtoWorkingDayHour> items = new List<dtoWorkingDayHour>();
            foreach(FlagDayOfWeek fEnum in (from e in Enum.GetValues(typeof(FlagDayOfWeek)).OfType<FlagDayOfWeek>() where e != FlagDayOfWeek.Weekend && e != FlagDayOfWeek.None && e!= FlagDayOfWeek.WorkWeek  && e!= FlagDayOfWeek.AllWeek select e)){
                if(days.HasFlag(fEnum)){
                    items.Add(new dtoWorkingDayHour(8, 0, 12, 0) { DayOfWeek = fEnum, IdCalendar = idCalendar });
                    items.Add(new dtoWorkingDayHour(13, 0, 17, 0) { DayOfWeek = fEnum, IdCalendar = idCalendar });
                }
            }
            return items;
        }

        public dtoCalendar(ProjectCalendar calendar, Boolean loadExcpetions = false) : this()
        {
            WorkingDaysHour = (calendar.WorkingDaysHour != null && calendar.WorkingDaysHour.Where(w => w.Deleted == BaseStatusDeleted.None).Any()) ? calendar.WorkingDaysHour.Where(w => w.Deleted == BaseStatusDeleted.None).Select(w => new dtoWorkingDayHour(w)).ToList() : new List<dtoWorkingDayHour>();
            if (loadExcpetions)
                DateExceptions = (calendar.DateExceptions != null && calendar.DateExceptions.Where(w => w.Deleted == BaseStatusDeleted.None).Any()) ? calendar.DateExceptions.Where(e => e.Deleted == BaseStatusDeleted.None).Select(e => new dtoDateException(e)).ToList() : new List<dtoDateException>();
        }
    }
}