using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Languages
{
    [Serializable]
    public class TitleDescriptionTranslation : lm.Comol.Core.DomainModel.DomainBaseObjectMetaInfo<long>
    {
        public virtual TitleDescriptionObjectTranslation DefaultTranslation { get; set; }
        public virtual IList<ItemBaseTranslation> Translations { get; set; }

        public TitleDescriptionObjectTranslation GetTranslation(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            TitleDescriptionObjectTranslation translation = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? DefaultTranslation : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Select(t => t.Translation).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.Translation).FirstOrDefault();
            if (translation == null)
                translation = DefaultTranslation;
            return translation;
        }
        public TitleDescriptionObjectTranslation GetTranslation(String userLanguageCode, Int32 idDefaultLanguage)
        {
            TitleDescriptionObjectTranslation translation = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? DefaultTranslation : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.LanguageCode == userLanguageCode).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.LanguageCode == userLanguageCode).Select(t => t.Translation).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.Translation).FirstOrDefault();
            if (translation == null)
                translation = DefaultTranslation;
            return translation;
        }
        public TitleDescriptionObjectTranslation GetTranslation(Int32 idUserLanguage, Int32 idDefaultLanguage, Boolean firstIsMulti, Boolean useFirstOccurence)
        {
            TitleDescriptionObjectTranslation translation = null;
            if (Translations == null || Translations.Any())
            {
                translation = Translations.Where(t => t.IdLanguage == idUserLanguage).FirstOrDefault().Translation;
            }
            if (translation == null && DefaultTranslation.IsValid() && firstIsMulti)
                translation = DefaultTranslation;
            if (translation == null && Translations.Any())
                translation = Translations.Where(t => t.IdLanguage == idDefaultLanguage).FirstOrDefault().Translation;
            if (translation == null && Translations.Any() && useFirstOccurence)
                translation = Translations.FirstOrDefault().Translation;
            return (translation == null) ? new TitleDescriptionObjectTranslation() {  Title = Id.ToString() } : translation;
        }

        public String GetTitle(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            String name = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? DefaultTranslation.Title : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Select(t => t.Translation.Title).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.Translation.Title).FirstOrDefault();
            if (String.IsNullOrEmpty(name))
                name = DefaultTranslation.Title;
            return name;
        }
        public String GetLowerTitle(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            String name = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? DefaultTranslation.Title : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Select(t => t.Translation.Title).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.Translation.Title).FirstOrDefault();
            if (String.IsNullOrEmpty(name))
                name = DefaultTranslation.Title;
            return (String.IsNullOrEmpty(name) ? "" : name.ToLower());
        }

        public TitleDescriptionTranslation() {
            DefaultTranslation = new TitleDescriptionObjectTranslation();
            Translations = new List<ItemBaseTranslation>();
        }
    }
}