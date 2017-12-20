using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class liteRepositorySettings : DomainBaseObject<long>
    {
        public virtual SettingsType Type { get; set; }
        public virtual Int32 IdCommunityType { get; set; }
        public virtual Int32 IdOrganization { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdPerson { get; set; }
        public virtual Int32 IdPersonType { get; set; }
        public virtual ItemStatus Status { get; set; }
        public virtual DiskSettings DiskSpace { get; set; }

        public virtual CustomizationLevel Customization { get; set; }
        public virtual Boolean AllowVersioning { get; set; }
        public virtual IList<liteItemTypeSettings> ItemTypes { get; set; }
        public virtual PresetType DefaultView { get; set; }
        public virtual IList<liteViewSettings> Views { get; set; }
        public virtual String AutoThumbnailForExtension { get; set; }
        public virtual Int32 AutoThumbnailWidth { get; set; }
        public virtual Int32 AutoThumbnailHeight { get; set; }
        
        public liteRepositorySettings()
        {
            Type = SettingsType.Istance;
            Customization = CustomizationLevel.None;
            DiskSpace = new DiskSettings();
            Views = new List<liteViewSettings>();
            DefaultView = PresetType.None;
            ItemTypes = new List<liteItemTypeSettings>();
        }

        public virtual Boolean IsGenericFor(RepositoryType type)
        {
            return (type == RepositoryType.Portal && Type == SettingsType.Portal) || (type == RepositoryType.Community && Type == SettingsType.Community);
        }

        public virtual Dictionary<ItemType, Boolean> GetDefaultAllowDownload()
        {
            return (from i in Enum.GetValues(typeof(ItemType)).OfType<ItemType>() where i != ItemType.None && i != ItemType.RootFolder select i).ToDictionary(i => i, i => ( i!= ItemType.Folder && (ItemTypes == null || !ItemTypes.Where(x => x.Type == i && x.Deleted == DomainModel.BaseStatusDeleted.None && !x.AllowDownload).Any())));
        }

        public virtual Dictionary<ItemType, DisplayMode> GetDefaultDisplayMode()
        {
            return (from i in Enum.GetValues(typeof(ItemType)).OfType<ItemType>() where i != ItemType.None && i != ItemType.RootFolder select i).ToDictionary(i => i, i => ((ItemTypes == null || !ItemTypes.Where(x => x.Type == i && x.Deleted == DomainModel.BaseStatusDeleted.None).Any()) ? DisplayMode.downloadOrPlay : ItemTypes.Where(x => x.Type == i && x.Deleted == DomainModel.BaseStatusDeleted.None).Select(x => x.DefaultDisplayMode).FirstOrDefault()));
        }
    }
}