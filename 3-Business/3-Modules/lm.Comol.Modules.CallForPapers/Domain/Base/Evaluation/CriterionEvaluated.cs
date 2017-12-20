using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable()]
    public class CriterionEvaluated : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual BaseForPaper Call { get; set; }
        public virtual BaseCriterion Criterion { get; set; }
        public virtual Evaluation Evaluation { get; set; }
        public virtual UserSubmission Submission { get; set; }
        public virtual String StringValue { get; set; }
        public virtual Decimal DecimalValue { get; set; }
        public virtual CriterionOption Option { get; set; }
        public virtual String Comment { get; set; }
        public virtual Boolean IsValueEmpty { get; set; }
        public virtual Boolean IsCompleted
        {
            get
            {
                return !IsValueEmpty && Criterion != null && (Criterion.CommentType != CommentType.Mandatory || (Criterion.CommentType != CommentType.Mandatory && !String.IsNullOrEmpty(Comment)));
            }
        }
        public virtual lm.Comol.Core.Dss.Domain.Templates.ItemRating DssValue { get; set; }

        public CriterionEvaluated()
        {
            Deleted = BaseStatusDeleted.None;
        }
    }
}