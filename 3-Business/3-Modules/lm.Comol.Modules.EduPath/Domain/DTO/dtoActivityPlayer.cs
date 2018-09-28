using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoActivityPlayer
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public StatusStatistic StatusStatistic { get; set; }
        public long ParentUnitId { get; set; }

        public dtoActivityPlayer()
        { }
    }
}
