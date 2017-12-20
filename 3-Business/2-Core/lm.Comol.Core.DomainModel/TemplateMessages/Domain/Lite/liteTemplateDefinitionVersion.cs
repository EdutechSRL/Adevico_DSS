using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class liteTemplateDefinitionVersion : DomainBaseObject<long>, IDisposable
    {
        public virtual liteTemplateDefinition Template { get; set; }
        public virtual lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation DefaultTranslation { get; set; }
        public virtual TemplateStatus Status { get; set; }
        public virtual litePerson Owner { get; set; }
        public virtual IList<liteTemplateTranslation> Translations { get; set; }
        public virtual IList<liteChannelSettings> ChannelSettings { get; set; }
        public virtual IList<liteTemplateModuleContent> ModulesForContent { get; set; }
        public virtual IList<liteVersionPermission> Permissions { get; set; }
        public virtual Boolean HasShortText { get; set; }
        public virtual Boolean OnlyShortText { get; set; }
        public virtual Int32 Number { get; set; }

        public liteTemplateDefinitionVersion()
        {
            DefaultTranslation = new lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation();
            Translations = new List<liteTemplateTranslation>();
            ChannelSettings = new List<liteChannelSettings>();
            ModulesForContent = new List<liteTemplateModuleContent>();
            Permissions = new List<liteVersionPermission>();
        }


        public List<String> GetModuleContentCodes() {
            return (ModulesForContent== null || (ModulesForContent.Any() && !ModulesForContent.Where(m=>m.Deleted== BaseStatusDeleted.None).Any())) ? new List<String>(): ModulesForContent.Where(m=>m.Deleted== BaseStatusDeleted.None).Select(m=>m.ModuleCode).ToList();
        }
        public lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation GetTranslation(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation translation = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? DefaultTranslation : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Select(t => t.Translation).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.Translation).FirstOrDefault();
            if (translation == null)
                translation = DefaultTranslation;
            return translation;
        }
        public lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation GetTranslation(String userLanguageCode, Int32 idDefaultLanguage)
        {
            lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation translation = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? DefaultTranslation : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.LanguageCode == userLanguageCode).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.LanguageCode == userLanguageCode).Select(t => t.Translation).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.Translation).FirstOrDefault();
            if (translation == null || !translation.IsValid())
                translation = DefaultTranslation;
            return translation;
        }
        public lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation GetTranslation(Int32 idUserLanguage, Int32 idDefaultLanguage, Boolean firstIsMulti, Boolean useFirstOccurence)
        {
            lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation translation = null;
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
            return (translation == null) ? new lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation() { Name = Id.ToString() } : translation;
        }

        public String GetName(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            String name = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? DefaultTranslation.Name : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Select(t => t.Translation.Name).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.Translation.Name).FirstOrDefault();
            if (String.IsNullOrEmpty(name))
                name = DefaultTranslation.Name;
            return name;
        }
        public String GetLowerName(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            String name = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? DefaultTranslation.Name : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Select(t => t.Translation.Name).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.Translation.Name).FirstOrDefault();
            if (String.IsNullOrEmpty(name))
                name = DefaultTranslation.Name;
            return (String.IsNullOrEmpty(name) ? "" : name.ToLower());
        }
        public List<liteVersionPermission> AvailablePermission()
        {
            return (Permissions == null) ? new List<liteVersionPermission>() : Permissions.Where(p => p.Deleted == BaseStatusDeleted.None).ToList();
        }
        public List<liteVersionPermission> AvailablePermission(PermissionType type)
        {
            return (Permissions == null) ? new List<liteVersionPermission>() : Permissions.Where(p => p.Type==type && p.Deleted == BaseStatusDeleted.None).ToList();
        }
        public Boolean IsForPortal()
        {
            return (this.Template !=null && (this.Template.Type== TemplateType.System || (this.Template.Type== TemplateType.User && this.Template.OwnerInfo.Type!= OwnerType.Object && this.Template.OwnerInfo.Type!= OwnerType.Community)));
        }

        public void Dispose()
        {
           
        }
    }
}