using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    [Serializable(), CLSCompliant(true)]
    public class dtoWeekDayRecurrence
    {
        public DateTime StartDate {get;set;}
        public DateTime EndDate { get; set; }

        public dtoWeekDayRecurrence()
        {
        }
        private dtoWeekDayRecurrence(DateTime oData, List<dtoWeekDay> oAvailables)
	    {
		    StartDate = oData;
		    EndDate = oData;
		    StartDate = StartDate.AddHours((from o in oAvailables where o.DayNumber == (int)oData.DayOfWeek select o.StartHour).FirstOrDefault());
		    StartDate = StartDate.AddMinutes((from o in oAvailables where o.DayNumber == (int)oData.DayOfWeek select o.StartMinutes).FirstOrDefault());
		    EndDate = EndDate.AddHours((from o in oAvailables where o.DayNumber == (int)oData.DayOfWeek select o.EndHour).FirstOrDefault());
		    EndDate = EndDate.AddMinutes((from o in oAvailables where o.DayNumber == (int)oData.DayOfWeek select o.EndMinutes).FirstOrDefault());
	    }
        public static List<dtoWeekDayRecurrence> CreateRecurrency(DateTime startDate, DateTime endDate, List<dtoWeekDay> selectedDays){
            List<dtoWeekDayRecurrence> list = new List<dtoWeekDayRecurrence>();
            List<int> dayNumbers = (from sd in selectedDays select sd.DayNumber).ToList();
            int dayInterval = (int)(endDate - startDate).TotalDays + 1;
            var query = (from sd in Enumerable.Range(0, dayInterval).ToList() select startDate.AddDays(sd));
            query = (from d in query.ToList() where dayNumbers.Contains((int)d.DayOfWeek) select d);

            list = (from q in query select new dtoWeekDayRecurrence(q.Date, selectedDays)).ToList();
            return list;
        }
    }
}