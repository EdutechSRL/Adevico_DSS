using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    [Serializable]
    public class dtoTagItem : lm.Comol.Core.DomainModel.DomainBaseObject<long>
    {
        public virtual dtoPermission Permissions { get; set; }
        public virtual lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation Translation { get; set; }
        public virtual List<dtoLanguageItem> Translations { get; set; }
        public virtual Int32 IdCreatedBy { get; set; }
        public virtual Int32 IdModifiedBy { get; set; }
        
        public virtual Int32 IdDisplayLanguage { get; set; }
        public virtual String CreatedBy { get; set; }
        public virtual String ModifiedBy { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        
        public virtual Boolean IsDefault { get; set; }
        public virtual Boolean IsSystem { get; set; }
        public virtual lm.Comol.Core.Dashboard.Domain.AvailableStatus Status { get; set; }
        public virtual long CommunityAssignments { get; set; }
        public virtual TagType Type { get; set; }
        public virtual Int32 IdModule { get; set; }
        public virtual String ModuleCode { get; set; }
        public virtual String GetFirstLetter()
        {
            String title = "";
            if (Translation!=null)
                title = Translation.Title;

            if (String.IsNullOrEmpty(title))
                return "";
            else
                return title[0].ToString().ToLower();
        }
        public dtoTagItem()
        {
            Translations = new List<dtoLanguageItem>();
            Permissions = new dtoPermission();
            IdDisplayLanguage = -1;
        }

        public dtoTagItem(liteTagItem tag, Dictionary<Int32, ModuleTags> permissions, Int32 idLanguage, List<Language> languages , Int32 idCurrentUser)
        {
            Id = tag.Id;
            Deleted = tag.Deleted;
            CreatedOn = tag.CreatedOn;
            ModifiedOn = tag.ModifiedOn;
            IsDefault = tag.IsDefault;
            IsSystem = tag.IsSystem;
            Status = tag.Status;
            IdDisplayLanguage = idLanguage;
            Translation = tag.GetTranslation(idLanguage);
            Translations = tag.Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.Translation!= null && !String.IsNullOrEmpty(t.Translation.Title)).Select(t =>
                new dtoLanguageItem() { IdLanguage = t.IdLanguage, LanguageCode = t.LanguageCode, IsDefault = languages.Where(l => l.Id == t.IdLanguage && l.isDefault).Any(), LanguageName = (languages.Where(l => l.Id == t.IdLanguage).Any() ? languages.Where(l => l.Id == t.IdLanguage).Select(l => l.Name).FirstOrDefault() : t.LanguageName) }).OrderByDescending(l => l.IsMultiLanguage).ThenByDescending(l => l.IsDefault).ThenBy(l => l.LanguageName).ToList();

            CommunityAssignments = tag.CommunityAssignments.Where(c=> c.Deleted== BaseStatusDeleted.None).Count();
            Type = tag.Type;
            IdModule = tag.IdModule;
            ModuleCode = tag.ModuleCode;
            IdCreatedBy = tag.IdCreatedBy;
            IdModifiedBy = tag.IdModifiedBy; 
            Permissions = new dtoPermission();
            Int32 idOrganization = 0;
            if (tag.IsSystem) 
                idOrganization =-3;
            else if (tag.Organizations != null && tag.Organizations.Where(t=> t.Deleted== BaseStatusDeleted.None && t.IsDefault && permissions.ContainsKey(t.IdOrganization)).Any())
                idOrganization = tag.Organizations.Where(t=> t.Deleted== BaseStatusDeleted.None && t.IsDefault && permissions.ContainsKey(t.IdOrganization)).Select(t=> t.IdOrganization).FirstOrDefault();

            if (permissions.ContainsKey(idOrganization))
            {
                Permissions.AllowView = permissions[idOrganization].List || permissions[idOrganization].Administration || permissions[idOrganization].Edit;
                Permissions.AllowAssignTo = (tag.Deleted == BaseStatusDeleted.None ) && (permissions[idOrganization].List || permissions[idOrganization].Administration || permissions[idOrganization].Edit);
                Permissions.AllowDelete = tag.Deleted == BaseStatusDeleted.Manual && (permissions[idOrganization].Administration || permissions[idOrganization].DeleteOther || (permissions[idOrganization].DeleteMy && idCurrentUser == IdCreatedBy));
                Permissions.AllowVirtualDelete = tag.Deleted == BaseStatusDeleted.None && (permissions[idOrganization].Administration || permissions[idOrganization].DeleteOther || (permissions[idOrganization].DeleteMy && idCurrentUser == IdCreatedBy));
                Permissions.AllowUnDelete = tag.Deleted == BaseStatusDeleted.Manual && (permissions[idOrganization].Administration || permissions[idOrganization].DeleteOther || (permissions[idOrganization].DeleteMy && idCurrentUser == IdCreatedBy));
                Permissions.AllowEdit = tag.Deleted == BaseStatusDeleted.None && (permissions[idOrganization].Administration || permissions[idOrganization].Edit);
                Permissions.AllowSetAvailable = tag.Deleted == BaseStatusDeleted.None && tag.Status != lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available && (permissions[idOrganization].Administration || permissions[idOrganization].Edit);
                Permissions.AllowSetUnavailable = tag.Deleted == BaseStatusDeleted.None && tag.Status == lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available && (permissions[idOrganization].Administration || permissions[idOrganization].Edit);
            }
        }
    }

}