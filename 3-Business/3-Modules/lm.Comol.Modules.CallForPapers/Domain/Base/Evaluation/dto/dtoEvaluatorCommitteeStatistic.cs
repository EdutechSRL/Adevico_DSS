using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
     [Serializable]
    public class dtoEvaluatorCommitteeStatistic
    {
        public virtual long IdCommittee { get; set; }
        //public virtual long IdAdvCommission { get; set; }
        public virtual long IdEvaluator { get; set; }

        public virtual Boolean IsFuzzy { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual List<dtoCriterion> Criteria { get; set; }
        public virtual List<dtoEvaluation> Evaluations { get; set; }

        public dtoEvaluatorCommitteeStatistic()
        {
            Criteria = new List<dtoCriterion>();
            Evaluations = new List<dtoEvaluation>();
        }
        public long GetEvaluationsCount(EvaluationStatus status){
            return (Evaluations == null ? ((long)0) :  Evaluations.Where(e=> e.Status==status).Count());
        }
    }
}