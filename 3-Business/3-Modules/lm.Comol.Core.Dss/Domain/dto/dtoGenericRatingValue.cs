using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain
{
    [Serializable]
    public class dtoGenericRatingValue : DomainBaseObject<long>
    {
        public virtual String Name { get; set; }
        public virtual String ShortName { get; set; }
        public virtual Double Value { get; set; }
        public virtual String FuzzyValue { get; set; }
        public virtual Boolean IsFuzzy { get; set; }

        public dtoGenericRatingValue()
        {

        }

        public static dtoGenericRatingValue Create(lm.Comol.Core.Dss.Domain.Templates.TemplateRatingValue ratingValue, Int32 idLanguage, Int32 idDefaultLanguage)
        {
            dtoGenericRatingValue dto = new dtoGenericRatingValue();
            dto.Id = ratingValue.Id;
            dto.Deleted = ratingValue.Deleted;
            dto.Name = ratingValue.GetTranslation(idLanguage, idDefaultLanguage).Name;
            dto.ShortName = ratingValue.GetTranslation(idLanguage, idDefaultLanguage).ShortName;
            dto.Value = ratingValue.Value;
            dto.FuzzyValue = ratingValue.FuzzyValue;
            dto.IsFuzzy = ratingValue.IsFuzzy;
            return dto;
        }
        public String ToString()
        {
            return String.Format("Id: {0} Name:{1} Value:{2} IsFuzzy:{3}",
                Id, Name, (IsFuzzy ? (!String.IsNullOrEmpty(FuzzyValue) ? FuzzyValue : "") : Value.ToString()), IsFuzzy);
        }
    }
}