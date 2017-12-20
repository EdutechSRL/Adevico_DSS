using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    [Serializable(), CLSCompliant(true)]
    public class dtoWeekDay
    {
        public bool Selected { get; set; }
        public int DayNumber { get; set; }
        public string DayName { get; set; }
        public int StartHour { get; set; }
        public int StartMinutes { get; set; }
        public int EndHour { get; set; }
        public int EndMinutes {get;set;}
    }

}
