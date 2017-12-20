using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
     [Serializable]
    public class dtoBaseEvaluatorStatistics : dtoBaseCommitteeMember 
    {
        public virtual long IdCommittee { get; set; }
        public virtual Dictionary<EvaluationStatus, long> Counters { get; set; }
        public virtual displayAs Display { get; set; }
        public dtoBaseEvaluatorStatistics() {
            Counters = new Dictionary<EvaluationStatus, long>();
            Display = displayAs.item;
        }

        public dtoBaseEvaluatorStatistics(CommitteeMember member) : this() {
            this.IdCommittee = member.Committee.Id;
            this.IdMembership = member.Id;
            this.IdCallEvaluator = (member.Evaluator != null) ? member.Evaluator.Id : 0;
            if (member.Evaluator != null && member.Evaluator.Person !=null)
            {
                this.IdPerson = member.Evaluator.Person.Id;
                this.DisplayName = member.Evaluator.Person.SurnameAndName;
            }
            this.ReplacedBy = member.ReplacedBy;
            this.ReplacingUser = member.ReplacingUser;
            this.ReplacedByEvaluator = member.ReplacedByEvaluator;
            this.ReplacingEvaluator = member.ReplacingEvaluator;
            this.Status = member.Status;
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