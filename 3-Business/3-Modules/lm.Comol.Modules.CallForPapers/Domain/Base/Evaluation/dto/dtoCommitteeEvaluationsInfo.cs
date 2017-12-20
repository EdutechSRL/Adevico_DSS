using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
     [Serializable]
    public class dtoCommitteeEvaluationsInfo 
    {
        public virtual long IdCommittee { get; set; }
        public virtual long IdEvaluator { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual long DisplayOrder { get; set; }
        public virtual Dictionary<EvaluationStatus, long> Counters { get; set; }
        public virtual String NavigationUrl { get; set; }
        public virtual Boolean isFuzzy { get; set; }
        public dtoCommitteeEvaluationsInfo()
        {
            Counters = new Dictionary<EvaluationStatus, long>();
        }
        public long GetEvaluationsCount(EvaluationStatus status){
            return (Counters==null || !Counters.ContainsKey(status)) ? 0 : Counters[status];
        }
        public long GetEvaluationsCount()
        {

            return (Counters == null ? 0 : Counters.Values.Select(v=> v).Sum());
        }
    }
}