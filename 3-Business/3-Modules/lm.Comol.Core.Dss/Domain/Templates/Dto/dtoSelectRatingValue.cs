using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain.Templates
{
    [Serializable]
    public class dtoSelectRatingValue : DomainBaseObject<long>
    {
        public virtual long IdMethod { get; set; }
        public virtual long IdRatingSet { get; set; }
        public virtual ItemObjectTranslation Translation { get; set; }
        public virtual String Name { get { return (Translation == null ? "" : Translation.Name); } }
        public virtual String ShortName { get { return (Translation == null ? "" : Translation.ShortName); } }
        public virtual Double Value { get; set; }
        public virtual String FuzzyValue { get; set; }
        public virtual Boolean IsFuzzy { get; set; }

        public dtoSelectRatingValue()
        {
            Translation = new ItemObjectTranslation();
        }

        public static dtoSelectRatingValue Create(long idRatingSet,TemplateRatingValue ratingValue, Int32 idLanguage, Int32 idDefaultLanguage)
        {
            dtoSelectRatingValue dto = new dtoSelectRatingValue();
            dto.Id = ratingValue.Id;
            dto.Deleted = ratingValue.Deleted;
            dto.Translation = ratingValue.GetTranslation(idLanguage, idDefaultLanguage);
            dto.IdRatingSet = idRatingSet;
            dto.Value = ratingValue.Value;
            dto.FuzzyValue = ratingValue.FuzzyValue;
            dto.IsFuzzy = ratingValue.IsFuzzy;
            return dto;
        }
        public dtoSelectRatingValue Copy(long idMethod,long idRatingSet)
        {
            dtoSelectRatingValue dto = new dtoSelectRatingValue();
            dto.Id = Id;
            dto.Deleted = Deleted;
            dto.Translation = Translation;
            dto.IsFuzzy = IsFuzzy;
            dto.Value = Value;
            dto.IsFuzzy = IsFuzzy;
            dto.FuzzyValue = FuzzyValue;
            dto.IdRatingSet = idRatingSet;
            dto.IdMethod = idMethod;
            return dto;
        }

        public String ToString()
        {
            return String.Format("Id: {0} IdMethod: {1} IdSet: {2} Name:{3} Value:{4} IsFuzzy:{5}",
                Id, IdMethod, IdRatingSet, Translation.Name, (IsFuzzy ? (!String.IsNullOrEmpty(FuzzyValue) ? FuzzyValue : "") : Value.ToString()), IsFuzzy);
        }
    }
}