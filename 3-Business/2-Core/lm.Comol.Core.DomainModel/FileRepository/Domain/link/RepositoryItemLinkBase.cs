using lm.Comol.Core.DomainModel;
using System;
namespace lm.Comol.Core.FileRepository.Domain
{
    [CLSCompliant(true)]
    public class RepositoryItemLinkBase<T>
    {
        public virtual T IdObjectLink { get; set; }
        public virtual RepositoryItemObject File { get; set; }
        public virtual Boolean AlwaysLastVersion { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public virtual Boolean IsVisible { get; set; }
    }

    public class RepositoryItemObject
    {
        public virtual long IdItem { get; set; }
        public virtual System.Guid UniqueId { get; set; }
        public virtual long IdVersion { get; set; }
        public virtual System.Guid UniqueIdVersion { get; set; }
        public virtual Int32 IdOwner { get; set; }
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
        public virtual Boolean IsDownloadable { get; set; }
        public virtual Boolean IsInternal { get; set; }
        public virtual Boolean IsVisible { get; set; }
        public virtual long Size { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }

        public virtual Boolean IsValid { get; set; }
        public RepositoryItemObject() { }

        public RepositoryItemObject(liteRepositoryItem item)
        {
            if (item != null)
            {
                IdItem = item.Id;
                UniqueId = item.UniqueId;
                IsFile = item.IsFile;
                Type = item.Type;
                Name = item.Name;
                Extension = item.Extension;
                Url = item.Url;
                ContentType = item.ContentType;
                Deleted = item.Deleted;
                IsDownloadable = item.IsDownloadable;
                IsInternal = item.IsInternal;
                IdOwner = item.IdOwner;
                IsVisible = item.IsVisible;
                IdVersion = item.IdVersion;
                UniqueIdVersion = item.UniqueIdVersion;
                Size = item.Size;
                IsValid = true;
            }
            else
            {
                Deleted = BaseStatusDeleted.Manual;
                IsValid = false;
            }
        }
        public RepositoryItemObject(liteRepositoryItem item, liteRepositoryItemVersion version) {
            if (item != null)
            {
                IdItem = item.Id;
                UniqueId = item.UniqueId;
                IsFile = item.IsFile;
                Type = item.Type;
                Name = item.Name;
                Extension = item.Extension;
                Url = item.Url;
                ContentType = item.ContentType;
                Deleted = item.Deleted;
                IsDownloadable = item.IsDownloadable;
                IsInternal = item.IsInternal;
                IdOwner = item.IdOwner;
                IsVisible = item.IsVisible;
                IsValid = true;
                IdVersion = item.IdVersion;
                UniqueIdVersion = item.UniqueIdVersion;
                Size = item.Size;
            }
            else
            {
                Deleted = BaseStatusDeleted.Manual;
                IsValid = false;
            }
            if (version != null)
            {
                IdVersion = version.Id;
                UniqueIdVersion = version.UniqueIdVersion;
                Size = version.Size;
                Name = version.Name;
                Extension = version.Extension;
                Url = version.Url;
                ContentType = version.ContentType;
                Deleted = version.Deleted;
            }
        }
    }
}