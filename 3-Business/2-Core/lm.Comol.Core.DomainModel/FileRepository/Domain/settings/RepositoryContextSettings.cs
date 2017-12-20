using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class RepositoryContextSettings : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual SettingsType Type { get; set; }
        public virtual Int32 IdCommunityType { get; set; }
        public virtual Int32 IdOrganization { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdPerson { get; set; }
        public virtual Int32 IdPersonType { get; set; }
       
        public virtual DiskSettings DiskSpace { get; set; }

        public virtual CustomizationLevel Customization { get; set; }

        public virtual Boolean AllowVersioning { get; set; }
        public virtual IList<ItemTypeSettings> ItemTypes { get; set; }
        public virtual PresetType DefaultView { get; set; }
        public virtual IList<ViewSettings> Views { get; set; }
        public virtual ItemStatus Status { get; set; }
        public virtual String AutoThumbnailForExtension { get; set; }
        public virtual Int32 AutoThumbnailWidth { get; set; }
        public virtual Int32 AutoThumbnailHeight { get; set; }
        
        public RepositoryContextSettings()
        {
            Type = SettingsType.Istance;
            Customization = CustomizationLevel.None;
            DiskSpace = new DiskSettings();
            Views = new List<ViewSettings>();
            DefaultView = PresetType.None;
            ItemTypes = new List<ItemTypeSettings>();
        }
        public static CustomizationLevel DefaultAllowedCustomization(SettingsType type)
        {
            switch (type)
            { 
                case SettingsType.Istance:
                    return CustomizationLevel.All;
                case SettingsType.Organization:
                    return CustomizationLevel.All;
                case SettingsType.Portal:
                    return CustomizationLevel.All;
                case SettingsType.Community:
                    return CustomizationLevel.ViewOptions | CustomizationLevel.AvailableSpace;
                case SettingsType.Profile:
                    return CustomizationLevel.ViewOptions;
            }
            return CustomizationLevel.None;
        }
    }
}