using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
     [Serializable]
    public class dtoCommitteeEvaluators : dtoBaseCommittee 
    {
        public virtual List<dtoBaseEvaluatorStatistics> Evaluators { get; set; }
        public dtoCommitteeEvaluators() {
            Evaluators = new List<dtoBaseEvaluatorStatistics>();
        }
    }
}