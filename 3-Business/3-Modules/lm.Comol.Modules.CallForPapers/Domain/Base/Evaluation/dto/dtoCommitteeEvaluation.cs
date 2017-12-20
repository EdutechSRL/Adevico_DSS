using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class dtoCommitteeEvaluation : dtoCommitteeEvaluationInfo
    {
        public virtual dtoEvaluation Evaluation { get; set; }
        public virtual double AverageRating { get { return Evaluation.AverageRating; } }
        public virtual double SumRating { get { return Evaluation.SumRating; } }
        public virtual dtoDssRating DssRating { get { return Evaluation.DssRating; } }


        public dtoCommitteeEvaluation()
        {
            Status = EvaluationStatus.None;
        }
    }
}