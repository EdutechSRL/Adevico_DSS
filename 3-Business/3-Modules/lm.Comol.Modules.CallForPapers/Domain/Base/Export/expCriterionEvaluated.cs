using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export
{
    [Serializable()]
    public class expCriterionEvaluated : DomainObject<long>
    {
        public virtual long IdCall { get; set; }
        public virtual expCriterion Criterion { get; set; }
        public virtual expEvaluation Evaluation { get; set; }
        //public virtual expSubmission Submission { get; set; }
        public virtual String StringValue { get; set; }
        public virtual Decimal DecimalValue { get; set; }
        public virtual expCriterionOption Option { get; set; }
        public virtual String Comment { get; set; }
        public virtual Boolean IsValueEmpty { get; set; }
        public virtual lm.Comol.Core.Dss.Domain.Templates.ItemRating DssValue { get; set; }
        public virtual Boolean IsCompleted
        {
            get
            {
                return !IsValueEmpty && Criterion != null && (Criterion.CommentType != CommentType.Mandatory || (Criterion.CommentType != CommentType.Mandatory && !String.IsNullOrEmpty(Comment)));
            }
        }
        public expCriterionEvaluated()
        {

        }
    }
}