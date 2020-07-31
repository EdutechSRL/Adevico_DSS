using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export
{
    [Serializable]
    public class expCommittee : DomainObject<long>
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual long IdCall { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual IList<expCriterion> Criteria { get; set; }
        public virtual IList<expEvaluation> Evaluations { get; set; }
        public virtual IList<expCommitteeSubmitterType> SubmitterTypes { get; set; }
        public virtual List<expEvaluator> Evaluators
        {
            get
            {
                return (Evaluations == null) ? new List<expEvaluator>() : Evaluations.Where(e => e.Evaluator != null).Select(e => e.Evaluator).Distinct().ToList();
            }
        }
        public virtual Boolean UseDss { get; set; }
        public virtual lm.Comol.Core.Dss.Domain.Templates.ItemMethodSettings MethodSettings { get; set; }
        public virtual lm.Comol.Core.Dss.Domain.Templates.ItemWeightSettings WeightSettings { get; set; }
        
        public virtual Boolean ForAllSubmittersType { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public expCommittee()
        {
            Criteria = new List<expCriterion>();
            Evaluations = new List<expEvaluation>();
            SubmitterTypes = new List<expCommitteeSubmitterType>();
        }

        public virtual List<expEvaluation> GetEvaluations(expEvaluator evaluator)
        {
            return (Evaluations == null) ? new List<expEvaluation>() : Evaluations.Where(e => e.Evaluator == evaluator && e.Submission != null).OrderBy(e=> e.Submission.Id).ToList();
        }
        public virtual List<expEvaluation> GetEvaluations(long idEvaluator)
        {
            return (Evaluations == null) ? new List<expEvaluation>() : Evaluations.Where(e => e.Evaluator != null && e.Evaluator.Id == idEvaluator && e.Submission != null).OrderBy(e => e.Submission.Id).ToList();
        }
    }
}