using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tiles.Domain
{
    [Serializable]
    public class dtoTileItem : lm.Comol.Core.DomainModel.DomainBaseObject<long>
    {
        public virtual dtoPermission Permissions { get; set; }
        public virtual lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation Translation { get; set; }
        public virtual List<dtoLanguageItem> Translations { get; set; }
        public virtual Int32 IdCreatedBy { get; set; }
        public virtual Int32 IdModifiedBy { get; set; }
        public virtual Int32 IdDisplayLanguage { get; set; }
        public virtual String CreatedBy { get; set; }
        public virtual String ModifiedBy { get; set; }
        public virtual String TranslatedType { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual Boolean HasDuplicatedTranslations { get; set; }
        public virtual lm.Comol.Core.Dashboard.Domain.AvailableStatus Status { get; set; }
        public virtual TileType Type { get; set; }
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
        public dtoTileItem()
        {
            Translations = new List<dtoLanguageItem>();
            Permissions = new dtoPermission();
            IdDisplayLanguage = -1;
        }

        public dtoTileItem(liteTile tile, ModuleDashboard permissions, Int32 idLanguage, List<Language> languages, Int32 idCurrentUser, List<Int32> idCommunityTypes)
        {
            Id = tile.Id;
            Deleted = tile.Deleted;
            CreatedOn = tile.CreatedOn;
            ModifiedOn = tile.ModifiedOn;
            IdCreatedBy = tile.IdCreatedBy;
            IdModifiedBy = tile.IdModifiedBy; 
            Status = tile.Status;
            IdDisplayLanguage = idLanguage;
            Translation = tile.GetTranslation(idLanguage);
            Translations = tile.Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.Translation != null && !String.IsNullOrEmpty(t.Translation.Title)).Select(t =>
                new dtoLanguageItem() { IdLanguage = t.IdLanguage, LanguageCode = t.LanguageCode, IsDefault = languages.Where(l => l.Id == t.IdLanguage && l.isDefault).Any(),  LanguageName = (languages.Where(l => l.Id == t.IdLanguage).Any() ? languages.Where(l => l.Id == t.IdLanguage).Select(l => l.Name).FirstOrDefault() : t.LanguageName) }).OrderByDescending(l=> l.IsMultiLanguage).ThenByDescending(l=> l.IsDefault).ThenBy(l=> l.LanguageName).ToList();

            Type = tile.Type;

            Boolean editingEnabled = (tile.Type != TileType.CommunityType || (tile.CommunityTypes != null && !tile.CommunityTypes.Where(i => idCommunityTypes.Contains(i)).Any()));

            Permissions = new dtoPermission();
          
            Permissions.AllowView = permissions.List || permissions.Administration || permissions.Edit;
            Permissions.AllowDelete =  editingEnabled && tile.Deleted == BaseStatusDeleted.Manual && (permissions.Administration || permissions.DeleteOther || (permissions.DeleteMy && idCurrentUser == tile.IdCreatedBy));
            Permissions.AllowVirtualDelete = editingEnabled && tile.Deleted == BaseStatusDeleted.None && (permissions.Administration || permissions.DeleteOther || (permissions.DeleteMy && idCurrentUser == tile.IdCreatedBy));
            Permissions.AllowUnDelete = editingEnabled && tile.Deleted == BaseStatusDeleted.Manual && (permissions.Administration || permissions.DeleteOther || (permissions.DeleteMy && idCurrentUser == tile.IdCreatedBy));
            Permissions.AllowEdit = tile.Deleted == BaseStatusDeleted.None && (permissions.Administration || permissions.Edit);
            Permissions.AllowSetAvailable = tile.Deleted == BaseStatusDeleted.None && tile.Status != lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available && (permissions.Administration || permissions.Edit);
            Permissions.AllowSetUnavailable = tile.Deleted == BaseStatusDeleted.None && tile.Status == lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available && (permissions.Administration || permissions.Edit);
        }
    }
}