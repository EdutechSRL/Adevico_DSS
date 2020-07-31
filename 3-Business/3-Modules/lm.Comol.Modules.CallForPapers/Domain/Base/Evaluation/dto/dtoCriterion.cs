using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export;
using lm.Comol.Core.Dss.Domain.Templates;
namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
     [Serializable]
    public class dtoCriterion :dtoBase 
    {
        public virtual long IdCommittee { get; set; }
        public virtual long IdCall { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual CommentType CommentType { get; set; }
        public virtual CriterionType Type { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual Boolean HasEvaluations { get; set; }
        public virtual Boolean HasInEvaluations { get; set; }
        public virtual dtoItemWeightSettings WeightSettings { get; set; }
        public virtual dtoItemMethodSettings MethodSettings { get; set; }

        public virtual lm.Comol.Core.Dss.Domain.dtoGenericRatingSet GetDssRatingSet
        {
            get
            {
                if (UseDss)
                {
                    return new Core.Dss.Domain.dtoGenericRatingSet()
                    {
                        Id = IdRatingSet,
                        IsFuzzy = IsFuzzy,
                        Name = "",
                        Values = (Options == null ? new List<Core.Dss.Domain.dtoGenericRatingValue>() : Options.Select(o => o.ToGenericRatingSet()).ToList())
                    };
                }
                else
                    return null;
            }
        }

        public virtual List<dtoCriterionOption> Options { get; set; }
        public virtual Boolean isStringRange { get { return (Options != null); } }
        public virtual Decimal DecimalMinValue { get; set; }
        public virtual Decimal DecimalMaxValue { get; set; }
        public virtual Int32 CommentMaxLength { get; set; }
        public virtual Int32 MinOption { get; set; }
        public virtual Int32 MaxOption { get; set; }
        public virtual Int32 MaxLength { get; set; }
        public virtual Boolean IsFuzzy { get; set; }
        public virtual Boolean UseDss { get; set; }
        public virtual long IdRatingSet { get; set; }
        
       public virtual Boolean HasDssErrors()
        {
            return UseDss && (MethodSettings.Error != Core.Dss.Domain.DssError.None || WeightSettings.Error != Core.Dss.Domain.DssError.None
                || (!MethodSettings.IsFuzzyMethod && IsFuzzy));
        }
       public virtual List<lm.Comol.Core.Dss.Domain.DssError> GetDssErrors()
       {
           List<lm.Comol.Core.Dss.Domain.DssError> errors = new List<Core.Dss.Domain.DssError>();
           if (UseDss)
           {
               if ((MethodSettings.Error & Core.Dss.Domain.DssError.MissingMethod) > 0)
                   errors.Add(lm.Comol.Core.Dss.Domain.DssError.MissingMethod);
               if ((MethodSettings.Error & Core.Dss.Domain.DssError.MissingRatingSet) > 0)
                   errors.Add(lm.Comol.Core.Dss.Domain.DssError.MissingRatingSet);
               if ((WeightSettings.Error & Core.Dss.Domain.DssError.MissingWeight) > 0)
                   errors.Add(lm.Comol.Core.Dss.Domain.DssError.MissingWeight);
               if ((WeightSettings.Error & Core.Dss.Domain.DssError.InvalidWeight) > 0)
                   errors.Add(lm.Comol.Core.Dss.Domain.DssError.InvalidWeight);
               if (!MethodSettings.IsFuzzyMethod && IsFuzzy)
                   errors.Add(lm.Comol.Core.Dss.Domain.DssError.InvalidType);
           }
           return errors;
       }
        public dtoCriterion()
            : base()
        {
            WeightSettings = new lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightSettings();
            MethodSettings = new lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings();
        }

        public dtoCriterion(long id, String name, int display)
            : base()
        {
            Id = id;
            DisplayOrder = display;
            Name = name;
            WeightSettings = new lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightSettings();
            MethodSettings = new lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings();
        }
        public dtoCriterion(BaseCriterion criterion)
            : this(criterion.Id, criterion.Name, criterion.DisplayOrder)
        {
            CreateDto(criterion);
        }

        private void CreateDto(BaseCriterion criterion)
        {
            if (criterion.Committee != null)
            {
                IdCommittee = criterion.Committee.Id;
                if (criterion.Committee.UseDss)
                {
                    MethodSettings = dtoItemMethodSettings.Create(criterion.Committee.MethodSettings, criterion.Committee.MethodSettings.UseManualWeights);
                }
            }
            Deleted = criterion.Deleted;
            Description = criterion.Description;
            CommentType = criterion.CommentType;
            Type = criterion.Type;
            if (criterion.UseDss)
                WeightSettings = dtoItemWeightSettings.Create(criterion.WeightSettings, criterion.Committee.MethodSettings.UseManualWeights);
            UseDss = criterion.UseDss;
            switch (criterion.Type) { 
                case CriterionType.IntegerRange:
                case CriterionType.DecimalRange:
                    DecimalMinValue = ((NumericRangeCriterion)criterion).DecimalMinValue;
                    DecimalMaxValue = ((NumericRangeCriterion)criterion).DecimalMaxValue;
                    break;
                case  CriterionType.StringRange:
                    MaxOption = ((StringRangeCriterion)criterion).MaxOption;
                    MinOption = ((StringRangeCriterion)criterion).MinOption;
                    Options = ((StringRangeCriterion)criterion).Options.Where(o=>o.Deleted== BaseStatusDeleted.None).OrderBy(o => o.DisplayOrder).ThenBy(o => o.Name).Select(o => new dtoCriterionOption(o)).ToList();
                    break;
                case  CriterionType.Textual:
                    var curType = criterion.GetType().ToString();

                    MaxLength = ((TextualCriterion)criterion).MaxLength;
                    break;
                case CriterionType.RatingScale:
                    IsFuzzy =((DssCriterion)criterion).IsFuzzy;
                    IdRatingSet = ((DssCriterion)criterion).IdRatingSet;
                    Options = ((DssCriterion)criterion).Options.Where(o => o.Deleted == BaseStatusDeleted.None).OrderBy(o => o.DisplayOrder).ThenBy(o => o.Name).Select(o => new dtoCriterionOption(o)).ToList();
                    break;
                case CriterionType.RatingScaleFuzzy:
                    IsFuzzy =((DssCriterion)criterion).IsFuzzy;
                    IdRatingSet = ((DssCriterion)criterion).IdRatingSet;
                    Options = ((DssCriterion)criterion).Options.Where(o => o.Deleted == BaseStatusDeleted.None).OrderBy(o => o.DisplayOrder).ThenBy(o => o.Name).Select(o => new dtoCriterionOption(o)).ToList();
                    break;
            }
            CommentMaxLength = criterion.CommentMaxLength;
        }

        public dtoCriterion(expCriterion criterion)
            : this(criterion.Id, criterion.Name, criterion.DisplayOrder)
        {
            CreateDto(criterion);
        }
        private void CreateDto(expCriterion criterion)
        {
            if (criterion.Committee != null)
            {
                IdCommittee = criterion.Committee.Id;
                if (criterion.Committee.UseDss)
                    MethodSettings = dtoItemMethodSettings.Create(criterion.Committee.MethodSettings, criterion.Committee.MethodSettings.UseManualWeights);
            }
            if (criterion.UseDss)
                WeightSettings = dtoItemWeightSettings.Create(criterion.WeightSettings, criterion.Committee.MethodSettings.UseManualWeights);
            Deleted = BaseStatusDeleted.None;
            Description = criterion.Description;
            CommentType = criterion.CommentType;
            Type = criterion.Type;
            UseDss = criterion.UseDss;
            switch (criterion.Type)
            {
                case CriterionType.IntegerRange:
                case CriterionType.DecimalRange:
                    DecimalMinValue = criterion.DecimalMinValue;
                    DecimalMaxValue = criterion.DecimalMaxValue;
                    break;
                case CriterionType.StringRange:
                    MaxOption = criterion.MaxOption;
                    MinOption = criterion.MinOption;
                    Options = criterion.Options.Where(o => o.Deleted == BaseStatusDeleted.None).OrderBy(o => o.DisplayOrder).ThenBy(o => o.Name).Select(o => new dtoCriterionOption(o,criterion.Id)).ToList();
                    break;
                case CriterionType.Textual:
                    MaxLength = criterion.MaxLength;
                    break;
                case CriterionType.RatingScale:
                    IsFuzzy = false;
                    IdRatingSet = criterion.IdRatingSet;
                    Options = criterion.Options.Where(o => o.Deleted == BaseStatusDeleted.None).OrderBy(o => o.DisplayOrder).ThenBy(o => o.Name).Select(o => new dtoCriterionOption(o, criterion.Id)).ToList();
                    break;
                case CriterionType.RatingScaleFuzzy:
                    IsFuzzy = true;
                    IdRatingSet = criterion.IdRatingSet;
                    Options = criterion.Options.Where(o => o.Deleted == BaseStatusDeleted.None).OrderBy(o => o.DisplayOrder).ThenBy(o => o.Name).Select(o => new dtoCriterionOption(o, criterion.Id)).ToList();
                    break;
            }
            CommentMaxLength = criterion.CommentMaxLength;
        }

        public lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase ToWeightItem(Dictionary<long, String> weights, Boolean isFuzzy, Boolean isOrderedItem = false)
        {
            lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase weight = new Core.Dss.Domain.Templates.dtoItemWeightBase();
            weight.IdObject = Id;
            weight.IsFuzzyValue = isFuzzy;
            weight.Name = Name;
            weight.OrderedItem = isOrderedItem;
            weight.Value = "";
            if (weights.ContainsKey(Id))
                weight.Value = weights[Id];
            return weight;
        }
    }
}