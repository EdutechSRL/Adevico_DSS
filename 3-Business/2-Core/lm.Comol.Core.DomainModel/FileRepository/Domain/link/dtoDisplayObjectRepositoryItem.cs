using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoDisplayObjectRepositoryItem
    {
        #region Identifiers
            public virtual long IdItem { get; internal set; }
            public virtual Guid UniqueId { get; internal set; }
            public virtual long IdVersion { get; internal set; }
            public virtual Guid UniqueIdVersion { get; internal set; }
        #endregion

        #region objsource
            public virtual liteModuleLink Link { get; internal set; }
            public virtual liteRepositoryItem Item { get; internal set; }
            public virtual liteRepositoryItemVersion Version { get; internal set; }
        #endregion

        #region objInfo
            public virtual String DisplayName { get; internal set; }
            public virtual String Name { get; internal set; }
            public virtual String Extension { get; internal set; }
            public virtual String Url { get; internal set; }
            public virtual String Size { get; internal set; }
            public virtual ItemType Type { get; internal set; }
            public virtual DisplayMode DisplayMode { get; internal set; }
            public virtual Boolean AutoThumbnail { get; internal set; }
            public virtual ItemAvailability Availability { get; internal set; }
            public virtual Boolean IsDownlodable { get; internal set; }
        #endregion
        
       
        
        
        public virtual Int32 IdCreatedBy { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual String Owner { get; set; }
        public virtual Int32 IdLinkedBy { get; set; }
        public virtual DateTime? LinkedOn { get; internal set; }
        public virtual String LinkedBy { get; internal set; }
        public virtual String Description { get; internal set; }
        public virtual ItemPermission Permissions { get; set; }
        public virtual List<String> TagsList { get { return (Item == null || String.IsNullOrWhiteSpace(Item.Tags) ? new List<String>() : Item.Tags.Split(',').ToList()); } }

        public dtoDisplayObjectRepositoryItem(liteModuleLink link,liteRepositoryItem item, liteRepositoryItemVersion version)
        {
            LinkedBy = "";
            IdLinkedBy = 0;
            Permissions = new ItemPermission();
            Link = link;
            Item = item;
            Version = version;
            if (version != null)
            {
                Description = version.Description;
                Name = version.Name;
                DisplayName = version.DisplayName;
                IdCreatedBy = version.IdCreatedBy;
                CreatedOn = version.CreatedOn;
                IdVersion = version.Id;
                Type = version.Type;
                IdItem = item.Id;
                DisplayMode = version.DisplayMode;
                Url = version.Url;
                Extension = version.Extension;
                AutoThumbnail = version.AutoThumbnail;
                IdVersion = version.Id;
                UniqueId = item.UniqueId;
                UniqueIdVersion = version.UniqueIdVersion;
                IsDownlodable = item.IsDownloadable;
            }
            else if (item != null)
            {
                Description = item.Description;
                Name = item.Name;
                DisplayName = item.DisplayName;
                IdCreatedBy = item.IdCreatedBy;
                CreatedOn = item.CreatedOn;
                IdVersion = 0;
                UniqueIdVersion = Guid.Empty;
                Type = item.Type;
                IdItem = item.Id;
                DisplayMode = item.DisplayMode;
                Url = item.Url;
                Extension = item.Extension;
                AutoThumbnail = item.AutoThumbnail;
                IsDownlodable = item.IsDownloadable;
            }
            Availability = (Version != null ? Version.Availability : (Item != null ? Item.Availability : ItemAvailability.notavailable));
            Size = FolderSizeItem.FormatBytes((Version != null ? Version.Size : (Item != null ? Item.Size :0)));
        }

        public void SetDescription(String description)
        {
            Description = description;
        }
        public void SetLinkedInfo(Int32 idLinked, DateTime? linkedOn, String linkedBy)
        {
            IdLinkedBy = idLinked;
            LinkedOn = linkedOn;
            LinkedBy = linkedBy;
        }
        
        private Boolean IsAvailableByType(ItemType type, ItemAvailability available)
        {
            switch (type)
            {
                case ItemType.File:
                    return available != ItemAvailability.notavailable;
                case ItemType.Folder:
                    return available != ItemAvailability.notavailable;
                case ItemType.Link:
                    return available != ItemAvailability.notavailable;
                case ItemType.Multimedia:
                case  ItemType.ScormPackage:
                case ItemType.SharedDocument:
                case ItemType.VideoStreaming:
                    return (available == ItemAvailability.available || available == ItemAvailability.waitingsettings);
            }
            return false;
        }

    }
  }