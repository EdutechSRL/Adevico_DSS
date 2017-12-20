using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dss.Domain.Templates;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export
{
    [Serializable]
    public class expCriterion : DomainObject<long>, IEquatable<expCriterion>
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual expCommittee Committee { get; set; }
        public virtual CommentType CommentType { get; set; }
        public virtual Int32 CommentMaxLength { get; set; }
        public virtual CriterionType Type { get; set; }
        public virtual int DisplayOrder { get; set; }
        //TextualCriterion
        public virtual Int32 MaxLength { get; set; }
        //NumericRangeCriterion
        public virtual Decimal DecimalMaxValue { get; set; }
        public virtual Decimal DecimalMinValue { get; set; }
        //StringRangeCriterion
        public virtual IList<expCriterionOption> Options { get; set; }
        public virtual Int32 MaxOption { get; set; }
        public virtual Int32 MinOption { get; set; }

        public virtual Boolean UseDss { get; set; }
        public virtual ItemWeightSettings WeightSettings { get; set; }
        public virtual ItemMethodSettings MethodSettings { get; set; }
        public virtual lm.Comol.Core.Dss.Domain.RatingType RatingType { get; set; }
        public virtual Boolean IsFuzzy { get; set; }
        public virtual long IdRatingSet { get; set; }
        public expCriterion()
        {
            Options = new List<expCriterionOption>();
            WeightSettings = new ItemWeightSettings();
            MethodSettings = new ItemMethodSettings();
            UseDss = false;
        }
        public virtual bool Equals(expCriterion other)
        {
            return (other!= null && this.Id == other.Id);
        }
    }

    [Serializable]
    public class expCriterionOption : DomainBaseObject<long>
    {
        public virtual String Name { get; set; }
        public virtual String ShortName { get; set; }
        public virtual Decimal Value { get; set; }
        public virtual long IdRatingSet { get; set; }
        public virtual long IdRatingValue { get; set; }
        public virtual Boolean IsFuzzy { get; set; }
        public virtual Boolean UseDss { get; set; }
        public virtual Double DoubleValue { get; set; }
        public virtual String FuzzyValue { get; set; }

        public virtual long DisplayOrder { get; set; }

        public expCriterionOption()
        {
            Name = "";
            Value = 0;
        }
    }
}