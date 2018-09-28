using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoItemWithStatistic
    {
        public long Id { get; set; }
        public long parentId { get; set; }
        public String Name { get; set; }
        public Int64 Completion { get; set; }
        public Int64 Weight { get; set; }
        public Int16 Mark { get; set; }
        public Int64 MinCompletion { get; set; }
        public Int16 MinMark { get; set; }
        public Int16 OnlyCompletedMandatory { get; set; }
        public Int16 TotMandatory { get; set; }
        public Int16 OnlyPassedMandatory { get; set; }
        public Int16 CompletedPassedMandatory { get; set; } 
        public StatusStatistic StatusStat { get; set; }
        public Status Status { get; set; }
        public bool canUpdate { get; set; }
        public IList<dtoItemWithStatistic> Children { get; set; }
      

        public dtoItemWithStatistic() { }



    }
}
