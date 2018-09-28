using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    public class CPMresult
    {
        public CPMstatus Status { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
        public Double ProjectLength { get; set; }
        public IList<dtoCPMactivity> Activities { get; set; }
        public IList<dtoCPMactivity> Critical { get; set; }
        public IList<DateTime> AvailableDates { get; set; }
        public Boolean isDeadlined { get; set; }
        public IList<dtoCPMactivity> DeadlinedActivities { get; set; }
    }
}
