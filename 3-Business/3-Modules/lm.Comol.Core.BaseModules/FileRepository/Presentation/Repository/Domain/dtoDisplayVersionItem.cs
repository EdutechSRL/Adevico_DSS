using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain
{
    [Serializable]
    public class dtoDisplayVersionItem{
        public virtual long Id {get;set;}
        public virtual long IdItem {get;set;}
        public virtual Guid UniqueIdItem {get;set;}
        public virtual Guid UniqueIdVersion {get;set;}
        public virtual String Name { get; set; }
        public virtual String Extension { get; set; }
        public virtual String Url { get; set; }
        public virtual String DisplayName
        {
            get
            {
                switch (Type)
                {
                    case ItemType.File:
                        return Name + Extension;
                    case ItemType.Multimedia:
                    case ItemType.ScormPackage:
                    case ItemType.Folder:
                        return Name;
                    case ItemType.Link:
                        return (String.IsNullOrWhiteSpace(Name) ? Url : Name);
                    default:
                        return Name;
                }
            }
        }
        
        public virtual ItemType Type {get;set;}
        public virtual DisplayMode DisplayMode {get;set;}
        public virtual String Thumbnail {get;set;}
        public virtual Boolean AutoThumbnail { get; set; }
        
        public virtual long DisplayOrder {get;set;}
        public virtual long Number { get; set; }
        
        public virtual long Downloaded { get; set; }
        
        public virtual long Size { get; set; }
        public virtual ItemAvailability Availability { get; set; }
        public virtual ItemStatus Status { get; set; }
        public virtual Boolean IsDownloadable { get; set; }
        public virtual Boolean IsDeleted { get; set; }
        public virtual Boolean IsActive { get; set; }


        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdCreatedBy { get; set; }
        public virtual Int32 IdModifiedBy { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual String Author { get; set; }
        public virtual String ModifyBy { get; set; }
        public virtual RepositoryIdentifier Repository { get; set; }
        public virtual VersionPermission Permissions { get; set; }

        public dtoDisplayVersionItem()
        {
            Permissions = new VersionPermission();
            Repository = new RepositoryIdentifier();
        }

        public dtoDisplayVersionItem(liteRepositoryItemVersion version)
        {
            Permissions = new VersionPermission();
            Repository = new RepositoryIdentifier();
            Id = version.Id;
            CreatedOn = version.CreatedOn;
            Availability = version.Availability;
            IsDeleted = version.Deleted!= DomainModel.BaseStatusDeleted.None;
            DisplayMode = version.DisplayMode;
            Downloaded = version.Downloaded;
            Extension = version.Extension;
            IdCommunity = version.IdCommunity;
            IdItem = version.IdItem;
            IdCreatedBy = version.IdCreatedBy;
            IdModifiedBy = version.IdModifiedBy;
            IsActive = version.IsActive;
            ModifiedOn = version.ModifiedOn;
            Name = version.Name;
            Number = version.Number;
            Size = version.Size;
            Status = version.Status;
            Thumbnail = version.Thumbnail;
            AutoThumbnail = version.AutoThumbnail;
            Repository = version.Repository;
            Type = version.Type;
            UniqueIdItem = version.UniqueIdItem;
            UniqueIdVersion = version.UniqueIdVersion;
            Url = version.Url;
        }

        public String GetSize(Int32 decimals = 2)
        {
            return FolderSizeItem.FormatBytes(Size, decimals);
        }
    }
}