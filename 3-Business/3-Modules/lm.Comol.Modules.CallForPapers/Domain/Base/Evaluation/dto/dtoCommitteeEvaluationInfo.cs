using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
     [Serializable]
    public class dtoCommitteeEvaluationInfo 
    {
        public virtual long IdCommittee { get; set; }
        public virtual String Name { get; set; }
        public virtual long DisplayOrder { get; set; }
        public virtual EvaluationStatus Status { get; set; }
        public virtual int MinValue { get; set; }
        public dtoCommitteeEvaluationInfo()
        {
            Status = EvaluationStatus.None;
        }
    }
}