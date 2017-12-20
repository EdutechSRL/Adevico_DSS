using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoBaseRepositoryItem
    {
        #region "Identifiers"
            public virtual long Id { get; set; }
            public virtual long IdFolder { get; set; }

            public virtual System.Guid UniqueId { get; set; }
            public virtual long IdVersion { get; set; }
            public virtual System.Guid UniqueIdVersion { get; set; }
        #endregion

       #region "Data"
            public virtual Int32 IdCommunity { get; set; }
            public virtual ItemType Type { get; set; }
           
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
            public virtual String ContentType { get; set; }
            public virtual Boolean IsFile { get; set; }
            public virtual Int32 IdOwner { get; set; }
            public virtual String OwnerName { get; set; }
            public virtual DateTime CreatedOn { get; set; }
            public virtual Int32 IdModifiedBy { get; set; }
            public virtual String ModifiedBy { get; set; }
            public virtual DateTime? ModifiedOn { get; set; }
           
            public virtual long Size { get; set; }
            public virtual long VersionsSize { get; set; }
            public virtual long DeletedSize { get; set; }
            public virtual String Description { get; set; }
            public virtual String Tags { get; set; }
            public virtual Boolean IsDownloadable { get; set; }
            public virtual lm.Comol.Core.DomainModel.BaseStatusDeleted Deleted { get; set; }
            public virtual String Path { get; set; }
            public virtual String FullPath { get { return Path + DisplayName + (Type == ItemType.Folder ? "/" : ""); } }

            public virtual Int32 OrderByFolder { get { return (Type == ItemType.Folder) ? 0 : 1; } }
            public virtual Int32 OrderByFile { get { return (Type != ItemType.Folder) ? 0 : 1; } }
            public virtual RepositoryIdentifier Repository { get; set; }
        #endregion
        #region "Clone Of"
            public virtual long CloneOf { get; set; }
            public virtual System.Guid CloneOfUniqueId { get; set; }
        #endregion
        #region "Folder"
            public virtual Boolean AllowUpload { get; set; }
            public virtual FolderType FolderType { get; set; }
        #endregion
        #region "Visibility"
            public virtual Boolean IsVisible { get; set; }
            public virtual long DisplayOrder { get; set; }
            public virtual ItemAvailability Availability { get; set; }
            public virtual ItemStatus Status { get; set; }
        #endregion
        #region "Special data"
            public virtual Boolean HasVersions { get; set; }
            public virtual long RevisionsNumber { get; set; }

            public virtual DisplayMode DisplayMode { get; set; }
            public virtual String Thumbnail { get; set; }
            public virtual Boolean AutoThumbnail { get; set; }
            public virtual long PreviewTime { get; set; }
            public virtual long Time { get; set; }
            public virtual long Number { get; set; }
        #endregion
        #region "Statistics"
            public virtual long IdPlayer { get; set; }
            public virtual long Downloaded { get; set; }
        #endregion

        #region "Owned By Service"
            public virtual Boolean IsInternal { get; set; }
            public virtual ItemModuleSettings Module { get; set; }
            public virtual Boolean IsVirtual { get; set; }
        #endregion
        public dtoBaseRepositoryItem()
        {
        }

        public dtoBaseRepositoryItem(liteRepositoryItem item, lm.Comol.Core.DomainModel.litePerson owner = null, lm.Comol.Core.DomainModel.litePerson modifiedBy = null, String unknownUser = "")
        {
            Id = item.Id;
            Deleted = item.Deleted;
            IdFolder = item.IdFolder;
            UniqueId = item.UniqueId;
            IdVersion = item.IdVersion;
            UniqueIdVersion = item.UniqueIdVersion;
            IdCommunity = item.IdCommunity;
            Type = item.Type;
            Name = item.Name;
            Extension = item.Extension;
            Url = item.Url;
            ContentType = item.ContentType;
            IsFile = item.IsFile;
            IdOwner = item.IdOwner;
          

            if (owner == null || owner.TypeID == (int)lm.Comol.Core.DomainModel.UserTypeStandard.Guest)
                OwnerName = unknownUser;
            else
                OwnerName = owner.SurnameAndName;

            CreatedOn = (item.CreatedOn.HasValue ? item.CreatedOn.Value : (item.ModifiedOn.HasValue ? item.ModifiedOn.Value : DateTime.Now));
            IdModifiedBy = item.IdModifiedBy;
            if (owner == null || owner.TypeID == (int)lm.Comol.Core.DomainModel.UserTypeStandard.Guest)
                OwnerName = unknownUser;
            else
                OwnerName = owner.SurnameAndName;
            ModifiedOn = item.ModifiedOn;
            if (modifiedBy == null || modifiedBy.TypeID == (int)lm.Comol.Core.DomainModel.UserTypeStandard.Guest)
                ModifiedBy = unknownUser;
            else
                ModifiedBy = modifiedBy.SurnameAndName;

            Size = item.Size;
            VersionsSize = item.VersionsSize;
            DeletedSize = item.DeletedSize;
            Description = item.Description;
            Tags = item.Tags;
            IsDownloadable = item.IsDownloadable;
            CloneOf = item.CloneOf;
            CloneOfUniqueId = item.CloneOfUniqueId;
            Repository = item.Repository;
            #region "Folder"
            AllowUpload = item.AllowUpload;
            FolderType = (item.IsFile ? lm.Comol.Core.FileRepository.Domain.FolderType.none : Domain.FolderType.standard);
            #endregion
            #region "Visibility"
            IsVisible = item.IsVisible;
            DisplayOrder = item.DisplayOrder;
            Availability = item.Availability;
            Status = item.Status;
            #endregion
            #region "Special data"
            HasVersions = item.HasVersions;
            RevisionsNumber = item.RevisionsNumber;
            DisplayMode = item.DisplayMode;
            AutoThumbnail = item.AutoThumbnail;
            Thumbnail = item.Thumbnail;
            PreviewTime = item.PreviewTime;
            Time = item.Time;
            Number = item.Number;
            #endregion
            IdPlayer = item.IdPlayer;
            Downloaded = item.Downloaded;
            IsInternal = item.IsInternal;
            Module = item.Module;
            IsVirtual = item.IsVirtual;
        }
        public dtoBaseRepositoryItem(dtoRepositoryItem item)
        {
            Id = item.Id;
            Deleted = item.Deleted;
            IdFolder = item.IdFolder;
            UniqueId = item.UniqueId;
            IdVersion = item.IdVersion;
            UniqueIdVersion = item.UniqueIdVersion;
            IdCommunity = item.IdCommunity;
            Type = item.Type;
            Name = item.Name;
            Extension = item.Extension;
            Url = item.Url;
            ContentType = item.ContentType;
            IsFile = item.IsFile;
            IdOwner = item.IdOwner;
            OwnerName = item.OwnerName;
            CreatedOn = item.CreatedOn;
            IdModifiedBy = item.IdModifiedBy;
            ModifiedOn = item.ModifiedOn;
            ModifiedBy = item.ModifiedBy;
            Size = item.Size;
            VersionsSize = item.VersionsSize;
            DeletedSize = item.DeletedSize;
            Description = item.Description;
            Tags = item.Tags;
            IsDownloadable = item.IsDownloadable;
            CloneOf = item.CloneOf;
            CloneOfUniqueId = item.CloneOfUniqueId;
            Repository = item.Repository;
            #region "Folder"
            AllowUpload = item.AllowUpload;
            FolderType = (item.IsFile ? lm.Comol.Core.FileRepository.Domain.FolderType.none : Domain.FolderType.standard);
            #endregion
            #region "Visibility"
            IsVisible = item.IsVisible;
            DisplayOrder = item.DisplayOrder;
            Availability = item.Availability;
            Status = item.Status;
            #endregion
            #region "Special data"
            HasVersions = item.HasVersions;
            RevisionsNumber = item.RevisionsNumber;
            DisplayMode = item.DisplayMode;
            Thumbnail = item.Thumbnail;
            AutoThumbnail = item.AutoThumbnail;
            PreviewTime = item.PreviewTime;
            Time = item.Time;
            Number = item.Number;
            #endregion
            IdPlayer = item.IdPlayer;
            Downloaded = item.Downloaded;
            IsInternal = item.IsInternal;
            Module = item.Module;
            IsVirtual = item.IsVirtual;
            Path = item.Path;
        }

        public String ToString()
        {
            return "Id-" + Id + "-" + DisplayName + "-" + Type.ToString() + "-" + Path + FolderType.ToString() + "-" + Deleted.ToString();
        }
    }
}