using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    public class ProjectCalendar : DomainBaseObjectLiteMetaInfo<long> 
    {
        private WorkingDay defaultWorkingDay { get; set; }
        public virtual Project Project { get; set; }
        public virtual ProjectResource Resource { get; set; }
        public virtual String Name { get; set; }
        public virtual CalendarType Type { get; set; }
        public virtual IList<ProjectDateException> DateExceptions { get; set; }
        public virtual IList<WorkingDayHour> WorkingDaysHour { get; set; }
        public virtual FlagDayOfWeek DaysOfWeek { get; set; }
        public virtual WorkingDay DefaultWorkingDay
        {
            get
            {
                if (defaultWorkingDay==null && (WorkingDaysHour==null || !WorkingDaysHour.Where(w=> w.Deleted== BaseStatusDeleted.None).Any()))
                    defaultWorkingDay = WorkingDay.GetDefault();
                return defaultWorkingDay;
            }
        }

        public ProjectCalendar()
        {
            DateExceptions = new List<ProjectDateException>();
            WorkingDaysHour = new List<WorkingDayHour>();
            defaultWorkingDay = WorkingDay.GetDefault();
        }

        public static List<WorkingDayHour> GetDefaultWorkingDayHours(FlagDayOfWeek days,ProjectCalendar calendar = null, Project project=null)
        {
            List<WorkingDayHour> items = new List<WorkingDayHour>();
            foreach(FlagDayOfWeek fEnum in (from e in Enum.GetValues(typeof(FlagDayOfWeek)).OfType<FlagDayOfWeek>() where e != FlagDayOfWeek.Weekend && e != FlagDayOfWeek.None && e!= FlagDayOfWeek.WorkWeek  && e!= FlagDayOfWeek.AllWeek select e)){
                if(days.HasFlag(fEnum)){
                    items.Add(new WorkingDayHour(8,0,12,0) { DayOfWeek = fEnum, Project=project , Calendar= calendar });
                    items.Add(new WorkingDayHour(13, 0, 17, 0) { DayOfWeek = fEnum, Project = project, Calendar = calendar });
                }
            }
            return items;
        }
    }
}