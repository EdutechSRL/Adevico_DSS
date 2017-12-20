using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export;
namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class dtoCriterionOption : dtoBase
    {
        public virtual String Name { get; set; }
        public virtual String ShortName { get; set; }
        
        public virtual Decimal Value { get; set; }
        public virtual long DisplayOrder { get; set; }
        public virtual long IdCriterion { get; set; }
        public virtual long IdRatingSet { get; set; }
        public virtual long IdRatingValue { get; set; }
        public virtual Boolean IsFuzzy { get; set; }
        public virtual Boolean UseDss { get; set; }
        public virtual Double DoubleValue { get; set; }
        public virtual String FuzzyValue { get; set; }

        public dtoCriterionOption()
            : base()
        {
            ShortName = "";
        }

        //public dtoCriterionOption(long id, String name, Decimal value, long display)
        //    : base()
        //{
        //    Id = id;
        //    DisplayOrder = display;
        //    Name = name;
        //    Value = value;
        //}
        //public dtoCriterionOption(long id, String name, Decimal value, long display, long idCriterion)
        //    : this(id, name, value, display)
        //{
        //    IdCriterion = idCriterion;
        //}
        public dtoCriterionOption(CriterionOption option)
        {
            Deleted = option.Deleted;
            BaseInitialize(option.Id, option.Name, option.ShortName, option.Value, option.DisplayOrder, (option.Criterion != null ? option.Criterion.Id : 0));
            if (option.UseDss)
                DssInitialize(option.IdRatingSet, option.IdRatingValue, option.IsFuzzy, option.DoubleValue, option.FuzzyValue);
        }
        public dtoCriterionOption(expCriterionOption option, long idCriterion)
            : this()
        {
            BaseInitialize(option.Id, option.Name,option.ShortName ,option.Value, option.DisplayOrder, idCriterion);
            if (option.UseDss)
                DssInitialize(option.IdRatingSet, option.IdRatingValue, option.IsFuzzy, option.DoubleValue, option.FuzzyValue);
        }
        private void BaseInitialize(long id, String name,String shortName, Decimal value, long display,long idCriterion)
        {
            Id = id;
            Name = name;
            ShortName = shortName;
            Value = value;
            DisplayOrder = display;
            Deleted = BaseStatusDeleted.None;
            IdCriterion = idCriterion;
        }
        private void DssInitialize(long idRatingSet, long  idRatingValue, Boolean isFuzzy, Double value, String  fuzzyValue)
        {
            IdRatingSet = idRatingSet;
            IdRatingValue = idRatingValue;
            IsFuzzy = isFuzzy;
            UseDss = true;
            DoubleValue = value;
            FuzzyValue = fuzzyValue;

        }
        public lm.Comol.Core.Dss.Domain.dtoGenericRatingValue ToGenericRatingSet()
        {
            lm.Comol.Core.Dss.Domain.dtoGenericRatingValue item = new lm.Comol.Core.Dss.Domain.dtoGenericRatingValue();
            item.IsFuzzy = IsFuzzy;
            item.FuzzyValue = FuzzyValue;
            item.Id = IdRatingValue;
            item.Name = Name;
            item.Value = DoubleValue;
            item.FuzzyValue = FuzzyValue;
            item.ShortName = ShortName;
            return item;
        }
        public String ValueToString()
        {
            if (UseDss)
            {
                return "";
            }
            else
            {
                Decimal fractional = Value - Math.Floor(Value);
                return (fractional == 0) ? String.Format("{0:N0}", Value) : String.Format("{0:N2}", Value);
            }
        }
    }
}