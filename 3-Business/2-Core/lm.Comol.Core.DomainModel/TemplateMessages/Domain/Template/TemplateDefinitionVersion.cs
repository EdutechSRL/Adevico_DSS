using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class TemplateDefinitionVersion : DomainBaseObjectLiteMetaInfo<long>, IDisposable
    {
        public virtual TemplateDefinition Template { get; set; }
        public virtual lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation DefaultTranslation { get; set; }
        public virtual TemplateStatus Status { get; set; }
        public virtual litePerson Owner { get; set; }
        public virtual IList<TemplateTranslation> Translations { get; set; }
        public virtual IList<ChannelSettings> ChannelSettings { get; set; }
        public virtual IList<TemplateModuleContent> ModulesForContent { get; set; }
        public virtual IList<VersionPermission> Permissions { get; set; }
        public virtual Boolean HasShortText { get; set; }
        public virtual Boolean OnlyShortText { get; set; }
        public virtual Int32 Number { get; set; }

        public TemplateDefinitionVersion()
        {
            DefaultTranslation = new lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation();
            Translations = new List<TemplateTranslation>();
            ChannelSettings = new List<ChannelSettings>();
            ModulesForContent = new List<TemplateModuleContent>();
            Permissions = new List<VersionPermission>();
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
            if (Translations != null && Translations.Any(t => t.IdLanguage == idUserLanguage))
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
        public List<VersionPermission> AvailablePermission() {
            return (Permissions == null) ? new List<VersionPermission>() : Permissions.Where(p => p.Deleted == BaseStatusDeleted.None).ToList();
        }
        public List<VersionPersonPermission> AvailableUsersPermission()
        {
            return (Permissions == null) ? new List<VersionPersonPermission>() : Permissions.Where(p => p.Deleted == BaseStatusDeleted.None && p.Type == PermissionType.Person).Select(p => (VersionPersonPermission)p).ToList();
        }
        public List<VersionProfileTypePermission> AvailableProfilesPermission()
        {
            return (Permissions == null) ? new List<VersionProfileTypePermission>() : Permissions.Where(p => p.Deleted == BaseStatusDeleted.None && p.Type == PermissionType.ProfileType).Select(p => (VersionProfileTypePermission)p).ToList();
        }
        public List<VersionRolePermission> AvailableRolesPermission()
        {
            return (Permissions == null) ? new List<VersionRolePermission>() : Permissions.Where(p => p.Deleted == BaseStatusDeleted.None && p.Type == PermissionType.Role).Select(p => (VersionRolePermission)p).ToList();
        }
        public List<VersionCommunityPermission> AvailableCommunitiesPermission()
        {
            return (Permissions == null) ? new List<VersionCommunityPermission>() : Permissions.Where(p => p.Deleted == BaseStatusDeleted.None && p.Type == PermissionType.Community).Select(p => (VersionCommunityPermission)p).ToList();
        }
        //public List<VersionModulePermission> AvailableModulesPermission()
        //{
        //    return (Permissions == null) ? new List<VersionModulePermission>() : Permissions.Where(p => p.Deleted == BaseStatusDeleted.None && p.Type == PermissionType.Module).Select(p => (VersionModulePermission)p).ToList();
        //}


        public Boolean IsForPortal()
        {
            return (this.Template !=null && (this.Template.Type== TemplateType.System || (this.Template.Type== TemplateType.User && this.Template.OwnerInfo.Type!= OwnerType.Object && this.Template.OwnerInfo.Type!= OwnerType.Community)));
        }

        public virtual TemplateDefinitionVersion Copy(TemplateDefinition template, litePerson person, String ipAddrees, String ipProxyAddress, String namePrefix = "")
        {
            TemplateDefinitionVersion tv = new TemplateDefinitionVersion();
            tv.CreateMetaInfo(person, ipAddrees, ipProxyAddress);

            tv.Template = template;
            tv.DefaultTranslation = DefaultTranslation.Copy();
            if (!string.IsNullOrEmpty(namePrefix))
                tv.DefaultTranslation.Name = namePrefix + tv.DefaultTranslation.Name;
            tv.Status = Status;
            tv.Owner = person;
            tv.HasShortText = HasShortText;
            tv.OnlyShortText = OnlyShortText;
            if (Translations != null) {
                foreach (TemplateTranslation tt in Translations.Where(t => t.Deleted == BaseStatusDeleted.None).ToList()) {
                    tv.Translations.Add(tt.Copy(tv, person, ipAddrees, ipProxyAddress, namePrefix));
                }
            }
            if (ModulesForContent != null)
            {
                foreach (TemplateModuleContent mc in ModulesForContent.Where(t => t.Deleted == BaseStatusDeleted.None && t.IsActive).ToList())
                {
                    tv.ModulesForContent.Add(mc.Copy(tv, mc.IsActive));
                }
            }
            if (ChannelSettings != null)
            {
                foreach (ChannelSettings ns in ChannelSettings.Where(t => t.Deleted == BaseStatusDeleted.None).ToList())
                {
                    tv.ChannelSettings.Add(ns.Copy(tv, person, ipAddrees, ipProxyAddress));
                }
            }
            if (Permissions != null)
            {
                foreach (VersionPermission ns in Permissions.Where(t => t.Deleted == BaseStatusDeleted.None).ToList())
                {
                    switch (ns.Type) { 
                        case PermissionType.Community:
                            tv.Permissions.Add(((VersionCommunityPermission)ns).Copy(template, tv, person, ipAddrees, ipProxyAddress));
                            break;
                        case PermissionType.Module:
                            break;
                        case PermissionType.Owner:
                            tv.Permissions.Add(((VersionOwnerPermission)ns).Copy(template, tv, person, ipAddrees, ipProxyAddress));
                            break;
                        case PermissionType.Person:
                            tv.Permissions.Add(((VersionPersonPermission)ns).Copy(template, tv, person, ipAddrees, ipProxyAddress));
                            break;
                        case PermissionType.ProfileType:
                            tv.Permissions.Add(((VersionProfileTypePermission)ns).Copy(template, tv, person, ipAddrees, ipProxyAddress));
                            break;
                        case PermissionType.Role:
                            tv.Permissions.Add(((VersionRolePermission)ns).Copy(template, tv, person, ipAddrees, ipProxyAddress));
                            break;
                    }
                    
                }
            }
            return tv;
        }
        public void Dispose()
        {
           
        }
    }
}