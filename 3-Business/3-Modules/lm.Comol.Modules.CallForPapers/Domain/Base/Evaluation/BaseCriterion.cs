using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dss.Domain.Templates;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class BaseCriterion : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }

        public virtual EvaluationCommittee Committee { get; set; }
        public virtual Advanced.Domain.AdvCommission AdvCommitee { get; set; }

        public virtual CommentType CommentType { get; set; }
        public virtual Int32 CommentMaxLength { get; set; }
        public virtual CriterionType Type { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual Boolean UseDss { get; set; }
        
        public virtual ItemWeightSettings WeightSettings { get; set; }
        public virtual ItemMethodSettings MethodSettings { get; set; }



        public BaseCriterion() {
            WeightSettings = new ItemWeightSettings();
            MethodSettings = new ItemMethodSettings();
            UseDss = false;
        }

        public virtual Boolean HasDssInvalidMethod()
        {
            return (!MethodSettings.InheritsFromFather && (MethodSettings.IdMethod < 1 || MethodSettings.IdRatingSet < 1));
        }
        public virtual Boolean HasDssInvalidWeight()
        {
            return (WeightSettings.IdRatingValue < 1)
                    || (((WeightSettings.RatingType & Core.Dss.Domain.RatingType.intermediateValues) > 0) && WeightSettings.IdRatingValueEnd < 1)
                    || ((WeightSettings.RatingType & Core.Dss.Domain.RatingType.extended) > 0 && WeightSettings.IdRatingValueEnd < 1);
        }
        public virtual Boolean HasDssErrors()
        {
            return UseDss && (HasDssInvalidMethod() || HasDssInvalidWeight());
        }
    }
    [Serializable]
    public class TextualCriterion : BaseCriterion
    {
        public virtual Int32 MaxLength { get; set; }
        public TextualCriterion()
        {
            Type = CriterionType.Textual;
        }
    }
    [Serializable]
    public class BoolCriterion : BaseCriterion
    {
        public BoolCriterion()
        {
            Type = CriterionType.Boolean;
        }
    }

    [Serializable]
    public class NumericRangeCriterion : BaseCriterion
    {
        public virtual Decimal DecimalMaxValue { get; set; }
        public virtual Decimal DecimalMinValue { get; set; }
        public NumericRangeCriterion()
        {

        }
        public NumericRangeCriterion(Decimal minValue, Decimal maxValue, CriterionType type)
        {
            DecimalMaxValue = maxValue;
            DecimalMinValue = minValue;
            Type = type;
        }
    }

    [Serializable]
    public class StringRangeCriterion : BaseCriterion
    {
        public virtual IList<CriterionOption> Options { get; set; }
        public virtual Int32 MaxOption { get; set; }
        public virtual Int32 MinOption { get; set; }
        public StringRangeCriterion()
        {
            Options = new List<CriterionOption>();
            Type = CriterionType.StringRange;
        }
    }

     [Serializable]
    public class DssCriterion : BaseCriterion
    {
        public virtual IList<CriterionOption> Options { get; set; }
        public virtual Boolean IsFuzzy { get; set; }
        public virtual long IdRatingSet { get; set; }
        public DssCriterion()
        {
            Options = new List<CriterionOption>();
            Type = CriterionType.RatingScale;
        }
        public virtual Boolean HasDssFuzzyErrors(lm.Comol.Core.Dss.Domain.Templates.ItemMethodSettings settings)
        {
            return UseDss && (!MethodSettings.IsFuzzyMethod && IsFuzzy);
        }
    }
    

    [Serializable]
    public class CriterionOption : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual String Name { get; set; }
        public virtual String ShortName { get; set; }

        
        public virtual Decimal Value { get; set; }
        public virtual long DisplayOrder { get; set; }
        public virtual BaseCriterion Criterion { get; set; }

        public virtual long IdRatingSet { get; set; }
        public virtual long IdRatingValue { get; set; }
        public virtual Boolean IsFuzzy { get; set; }
        public virtual Boolean UseDss { get; set; }
        public virtual Double DoubleValue { get; set; }
        public virtual String FuzzyValue { get; set; }

        public CriterionOption()
        {
            Name = "";
            ShortName = "";
            Value = 0;
        }
    }


    [Serializable]
    public enum CommentType
    {
        None = 0,
        Allowed = 1,
        Mandatory = 2
    }

    [Serializable]
    public enum CriterionType
    {
        None = 0,
        Textual = 1,
        IntegerRange = 2,
        DecimalRange = 3,
        StringRange = 4,
        RatingScale = 5,
        RatingScaleFuzzy =6,
        Boolean = 10
    }
}