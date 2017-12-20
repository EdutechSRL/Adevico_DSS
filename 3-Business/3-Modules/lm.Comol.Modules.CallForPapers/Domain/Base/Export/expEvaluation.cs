using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export
{
    [Serializable]
    public class expEvaluation : DomainObject<long>
    {
        public virtual long IdCall { get; set; }

        public virtual expCommittee Committee { get; set; }
        public virtual expEvaluator Evaluator { get; set; }

        public virtual Advanced.Domain.AdvCommission AdvCommission { get; set; }
        public virtual Advanced.Domain.AdvMember AdvEvaluator { get; set; }

        public virtual expSubmission Submission { get; set; }
        public virtual long IdSubmission { get; set; }
        public virtual long IdEvaluator { get; set; }
        public virtual long IdCommittee { get; set; }
        public virtual DateTime? EvaluationStartedOn { get; set; }
        public virtual DateTime? EvaluatedOn { get; set; }
        public virtual IList<expCriterionEvaluated> EvaluatedCriteria { get; set; }
        public virtual double AverageRating { get; set; }
        public virtual double SumRating { get; set; }
        public virtual bool BoolRating { get; set; }

        public virtual Boolean UseDss { get; set; }
        public virtual Boolean DssIsFuzzy { get; set; }
        public virtual double DssRanking { get; set; }
        public virtual double DssValue { get; set; }
        public virtual String DssValueFuzzy { get; set; }
        
        public virtual Boolean Evaluated { get; set; }
        public virtual String Comment { get; set; }
        public virtual EvaluationStatus Status { get; set; }
        public virtual DateTime? LastUpdateOn { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }

        public virtual bool IsPassed { get; set; }

        public expEvaluation()
        {
            EvaluatedCriteria = new List<expCriterionEvaluated>();
        }
    }
}