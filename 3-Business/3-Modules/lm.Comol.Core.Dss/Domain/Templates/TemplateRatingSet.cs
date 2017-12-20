using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain.Templates
{
    [Serializable]
    public class TemplateRatingSet : DomainBaseObjectIdLiteMetaInfo<long>
    {
        public virtual ItemObjectTranslation DefaultTranslation { get; set; }
        public virtual IList<DssTemplateTranslation> Translations { get; set; }
        public virtual IList<TemplateRatingValue> Values { get; set; }
        public virtual Boolean IsFuzzy { get; set; }
        public virtual Boolean IsForWeights { get; set; }
        
        public virtual AlgorithmType OnlyForAlgorithm { get; set; }
        public virtual Boolean IsDefault { get; set; }
        public TemplateRatingSet()
        {
            Translations = new List<DssTemplateTranslation>();
            DefaultTranslation = new ItemObjectTranslation();
            Values = new List<TemplateRatingValue>();
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