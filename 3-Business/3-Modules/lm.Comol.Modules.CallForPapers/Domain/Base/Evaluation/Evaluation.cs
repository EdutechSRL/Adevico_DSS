using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class Evaluation : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual EvaluationCommittee Committee { get; set; }
        public virtual CallEvaluator Evaluator { get; set; }

        public virtual Advanced.Domain.AdvCommission AdvCommission { get; set; }
        public virtual Advanced.Domain.AdvMember AdvEvaluator { get; set; }
        
        

        public virtual BaseForPaper Call { get; set; }
        public virtual UserSubmission Submission { get; set; }
        public virtual DateTime? EvaluationStartedOn { get; set; }
        public virtual DateTime? EvaluatedOn { get; set; }
        //public virtual IList<EvaluatedCriterion> EvaluatedCriteria { get; set; }
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
        public virtual liteCommunity Community { get; set; }

        public virtual bool IsPassed { get; set; }

        public Evaluation()
        {
            Deleted = BaseStatusDeleted.None;
        }
    }

    [Serializable]
    public enum EvaluationStatus
    {
        None = 0,
        Evaluating = 1,
        Evaluated = 2,
        Invalidated = 4,
        EvaluatorReplacement = 8,
        Confirmed = 16
    }
}