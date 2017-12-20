using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain.Templates
{
    [Serializable]
    public class TemplateRatingValue : DomainBaseObject<long>
    {
        public virtual TemplateRatingSet RatingSet { get; set; }
        public virtual ItemObjectTranslation DefaultTranslation { get; set; }
        public virtual IList<DssTemplateTranslation> Translations { get; set; }
        public virtual Double Value { get; set; }
        public virtual String FuzzyValue { get; set; }
        public virtual Boolean IsFuzzy { get; set; }

        public TemplateRatingValue()
        {
            DefaultTranslation = new ItemObjectTranslation();
            Translations = new List<DssTemplateTranslation>();
        }
        public ItemObjectTranslation GetTranslation(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            ItemObjectTranslation translation = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? DefaultTranslation : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Select(t => t.Translation).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.Translation).FirstOrDefault();
            if (translation == null)
                translation = DefaultTranslation;
            return translation;
        }
        public ItemObjectTranslation GetTranslation(String userLanguageCode, Int32 idDefaultLanguage)
        {
            ItemObjectTranslation translation = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? DefaultTranslation : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.LanguageCode == userLanguageCode).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.LanguageCode == userLanguageCode).Select(t => t.Translation).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.Translation).FirstOrDefault();
            if (translation == null || !translation.IsValid())
                translation = DefaultTranslation;
            return translation;
        }
    }
}
