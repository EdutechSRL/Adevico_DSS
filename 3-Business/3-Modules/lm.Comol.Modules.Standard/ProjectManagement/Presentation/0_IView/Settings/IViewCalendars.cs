using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.ProjectManagement.Domain; 

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation 
{
    public interface IViewCalendars : IViewBaseEdit
    {
        Boolean AllowSave { get; set; }
        Boolean AllowAdd { get; set; }
        String GetDefaultCalendarName();


        void DisplayUnknownProject();
        void LoadProjectAvailableDays(FlagDayOfWeek days);
        void LoadCalendars(List<dtoCalendar> calendars);
    }
}