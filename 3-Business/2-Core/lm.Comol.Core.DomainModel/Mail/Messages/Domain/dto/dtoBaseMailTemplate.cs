using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Languages;

namespace lm.Comol.Core.Mail.Messages
{
    [Serializable]
    public class dtoBaseMailTemplate
    {
        public virtual long IdTemplate { get; set; }
        public virtual long IdVersion { get; set; }
        public virtual ItemObjectTranslation DefaultTranslation { get; set; }
        public virtual IList<dtoBaseMailTemplateContent> Translations { get; set; }
        public virtual lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings MailSettings { get; set; }
        public virtual Boolean IsTemplateCompliant { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public dtoBaseMailTemplate(lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings settings)
        {
            DefaultTranslation = new ItemObjectTranslation();
            Translations = new List<dtoBaseMailTemplateContent>();
            MailSettings = settings;
        }
        public ItemObjectTranslation GetTranslation(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            ItemObjectTranslation translation = (Translations == null || !Translations.Any()) ? DefaultTranslation : (Translations.Where(t => t.IdLanguage == idUserLanguage).Any()) ? Translations.Where(t => t.IdLanguage == idUserLanguage).Select(t => t.Translation).FirstOrDefault() : Translations.Where(t =>  t.IdLanguage == idDefaultLanguage).Select(t => t.Translation).FirstOrDefault();
            if (translation == null)
                translation = DefaultTranslation;
            return translation;
        }
        public ItemObjectTranslation GetTranslation(String userLanguageCode, Int32 idDefaultLanguage)
        {
            ItemObjectTranslation translation = (Translations == null || !Translations.Any()) ? DefaultTranslation : (Translations.Where(t => t.LanguageCode == userLanguageCode).Any()) ? Translations.Where(t => t.LanguageCode == userLanguageCode).Select(t => t.Translation).FirstOrDefault() : Translations.Where(t => t.IdLanguage == idDefaultLanguage).Select(t => t.Translation).FirstOrDefault();
            if (translation == null)
                translation = DefaultTranslation;
            return translation;
        }
        public String GetName(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            String name = (Translations == null || !Translations.Any()) ? DefaultTranslation.Name : (Translations.Where(t => t.IdLanguage == idUserLanguage).Any()) ? Translations.Where(t =>  t.IdLanguage == idUserLanguage).Select(t => t.Translation.Name).FirstOrDefault() : Translations.Where(t =>  t.IdLanguage == idDefaultLanguage).Select(t => t.Translation.Name).FirstOrDefault();
            if (String.IsNullOrEmpty(name))
                name = DefaultTranslation.Name;
            return name;
        }
    }
}