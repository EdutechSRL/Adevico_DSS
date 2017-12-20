using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
     [Serializable]
    public class dtoCriterionEvaluated : dtoBase 
    {
        #region "Base"
            public virtual long IdValueCriterion { get; set; }
            public virtual dtoCriterion Criterion { get; set; }
            public virtual long IdOption { get; set; }
            public virtual String StringValue { get; set; }
            public virtual String Comment { get; set; }
            public virtual Decimal DecimalValue { get; set; }
            public virtual lm.Comol.Core.Dss.Domain.Templates.dtoItemRating DssValue { get; set; }
        #endregion
        #region "Criterion"
            public virtual long IdCriterion {
                get{
                    return (Criterion == null) ? 0 : Criterion.Id;  
                }
            }
            public virtual Boolean CommentMandatory
            {
                get
                {
                    return (Criterion == null) ? false : (Criterion.CommentType== CommentType.Mandatory);
                }
            }
         #endregion
        #region "Display"
            public virtual FieldError CriterionError { get; set; }
            public virtual String DisplayId
            {
                get
                {
                    return (IdCriterion > 0) ? "C" + IdCriterion.ToString() : "G" + IdCriterion.ToString();
                }
            }
            public virtual EvaluationStatus Status
            {
                get
                {
                    return (IsValueEmpty) ? EvaluationStatus.None : (IsValidForEvaluation) ? EvaluationStatus.Evaluated : EvaluationStatus.Evaluating;
                }
            }
            public virtual displayAs DisplayAs { get; set; }
        #endregion

        #region "Validation"
            public virtual Boolean IsValidForEvaluation
            {
                get {
                    Boolean valid = (Criterion != null && (Criterion.CommentType != CommentType.Mandatory || (Criterion.CommentType == CommentType.Mandatory && !String.IsNullOrEmpty(Comment))));
                    if (valid)
                        valid = !IsValueEmpty & IsValidForCriterionSaving;
                    return valid;
                }
            }
            public virtual Boolean IsValidForCriterionSaving
            {
                get
                {
                    Boolean valid = true;
                    switch (Criterion.Type)
                    {
                        case CriterionType.DecimalRange:
                        case CriterionType.IntegerRange:
                            valid = (DecimalValue <= Criterion.DecimalMaxValue && DecimalValue >= Criterion.DecimalMinValue);
                            break;
                        case CriterionType.StringRange:
                            valid = (IdOption > 0);
                            break;
                        case CriterionType.Textual:
                            valid = !String.IsNullOrEmpty(StringValue);
                            break;
                        case CriterionType.RatingScaleFuzzy:
                        case CriterionType.RatingScale:
                            valid = (DssValue != null);
                            if (valid)
                            {
                                switch (DssValue.RatingType)
                                {
                                    case Core.Dss.Domain.RatingType.extended:
                                    case Core.Dss.Domain.RatingType.intermediateValues:
                                        valid = (DssValue.IdRatingValue > 0 && DssValue.IdRatingValueEnd>0);
                                        break;
                                    case Core.Dss.Domain.RatingType.simple:
                                        valid = (DssValue.IdRatingValue > 0);
                                        break;
                                }
                            }
                            break;
                        default:
                            valid = true;
                            break;
                    }
                    return valid;
                }
            }
            public virtual Boolean IsValueEmpty { get; set; }
            public virtual Boolean Ignore { get; set; }
        #endregion
          
         
            //    public virtual Boolean IsEmpty { get; set; }
    //    public virtual Boolean IsStarted
    //    {
    //        get
    //        {
    //            return !IsEmpty && (Criterion.CommentType == CommentType.None || !String.IsNullOrEmpty(Comment));
    //        }
    //    }
    //    public virtual Boolean Evaluated { get; set; }
    //    public virtual Boolean IsCompleted { get {
    //        return Evaluated && (!CommentMandatory || (CommentMandatory && !String.IsNullOrEmpty(Comment)));
    //    } }

        public dtoCriterionEvaluated()
            : base()
        {
            IsValueEmpty = true;
            CriterionError = Domain.FieldError.None;
        }
        public dtoCriterionEvaluated(dtoCriterion criterion)
            : base()
        {
            IsValueEmpty = true;
            Criterion = criterion;
            CriterionError = Domain.FieldError.None;
        }
        public dtoCriterionEvaluated(dtoCriterion criterion, CriterionEvaluated valueItem)
            : this(criterion)
        {
            IsValueEmpty = true;
            if (valueItem != null)
            {
                IdValueCriterion = valueItem.Id;
                StringValue = valueItem.StringValue;
                DecimalValue = valueItem.DecimalValue;
                IdOption = (valueItem.Option == null) ? 0 : valueItem.Option.Id;
                Comment = valueItem.Comment;
                IsValueEmpty = valueItem.IsValueEmpty;
                if (criterion.UseDss)
                    DssValue = lm.Comol.Core.Dss.Domain.Templates.dtoItemRating.Create(valueItem.DssValue);
            }
        }
        public dtoCriterionEvaluated(dtoCriterion criterion, expCriterionEvaluated valueItem)
            : this(criterion)
        {
            IsValueEmpty = true;
            if (valueItem != null)
            {
                IdValueCriterion = valueItem.Id;
                StringValue = valueItem.StringValue;
                DecimalValue = valueItem.DecimalValue;
                IdOption = (valueItem.Option == null) ? 0 : valueItem.Option.Id;
                Comment = valueItem.Comment;
                IsValueEmpty = valueItem.IsValueEmpty;
                if (criterion.UseDss)
                    DssValue = lm.Comol.Core.Dss.Domain.Templates.dtoItemRating.Create(valueItem.DssValue);
            }
        }
        public dtoCriterionEvaluated(dtoCriterion criterion, Boolean ignore)
            : this(criterion)
        {
            Ignore = ignore;
        }
        public void SetError(Dictionary<long, FieldError> errors)
        {
            if (errors == null)
                CriterionError = Domain.FieldError.None;
            else if (errors.ContainsKey(IdCriterion))
                CriterionError = errors[IdCriterion];
            else
                CriterionError = Domain.FieldError.None;
        }
        public void SetValue(CriterionEvaluated valueItem)
        {
            IsValueEmpty = true;
            if (valueItem != null)
            {
                IdValueCriterion = valueItem.Id;
                StringValue = valueItem.StringValue;
                DecimalValue = valueItem.DecimalValue;
                IdOption = (valueItem.Option == null) ? 0 : valueItem.Option.Id;
                Comment = valueItem.Comment;
                IsValueEmpty = valueItem.IsValueEmpty;
                if (valueItem.Criterion.UseDss)
                    DssValue = lm.Comol.Core.Dss.Domain.Templates.dtoItemRating.Create(valueItem.DssValue);
            }
        }

        public static dtoCriterionEvaluated GetEvaluationPlaceHolder(String comment, Int32 displayOrder){
            dtoCriterionEvaluated criterion = new dtoCriterionEvaluated();
            criterion.Comment = comment;
            criterion.CriterionError = FieldError.None;
            criterion.IsValueEmpty = false;
            criterion.Criterion = new dtoCriterion(0, "", displayOrder);
            criterion.Criterion.Type = CriterionType.None;
            criterion.Criterion.CommentType = CommentType.Allowed;
            criterion.Criterion.CommentMaxLength = 300000;
            return criterion;
        }

        public String DecimalValueToString()
        {
            Decimal fractional = DecimalValue - Math.Floor(DecimalValue);
            return (fractional == 0) ? String.Format("{0:N0}", DecimalValue) : String.Format("{0:N2}", DecimalValue);
        }
        //public String GetValue()
        //{
        //    switch(Field.Type){
        //        case  FieldType.Note:
        //            return "";
        //        case FieldType.DropDownList:
        //        case FieldType.RadioButtonList:
        //         case FieldType.CheckboxList:
        //            String rValue = "";
        //            if (!String.IsNullOrEmpty(Value))
        //            {
        //                List<String> mValue = Value.Split('|').ToList();
        //                foreach (dtoFieldOption option in Field.Options.Where(o => mValue.Contains(o.Id.ToString())).ToList())
        //                {
        //                    rValue += ", " + option.Name;
        //                }
        //                if (!String.IsNullOrEmpty(rValue))
        //                    rValue = rValue.Remove(0, 2);
        //            }
        //            return rValue;
        //        case FieldType.FileInput:
        //            return IdLink.ToString();
        //        default :
        //            return "";
        //    }

        //}

        public void UpdateValue(expCriterionEvaluated valueItem)
        {
            IsValueEmpty = true;
            if (valueItem != null)
            {
                IdValueCriterion = valueItem.Id;
                StringValue = valueItem.StringValue;
                DecimalValue = valueItem.DecimalValue;
                IdOption = (valueItem.Option == null) ? 0 : valueItem.Option.Id;
                Comment = valueItem.Comment;
                IsValueEmpty = valueItem.IsValueEmpty;
                DssValue = lm.Comol.Core.Dss.Domain.Templates.dtoItemRating.Create(valueItem.DssValue); 
            }
        }
    }
}