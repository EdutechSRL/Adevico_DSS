using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoTimeGroup
    {
        public TimeGroup TimeLine { get; set; }
        public long FromTicks { get; set; }
        public long ToTicks { get; set; }
        public Int32 Month { get; set; }
        public Int32 Year { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        public String ToString() {
            return FromDate.ToShortDateString() + " - " + ToDate.ToShortDateString() + " : " + TimeLine.ToString() + " diff= " + (ToTicks - FromTicks).ToString("0,0");
        }
    }
}