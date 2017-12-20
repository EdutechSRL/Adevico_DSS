using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoBaseItem
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
           
            public virtual long Size { get; set; }
            public virtual long VersionsSize { get; set; }
            public virtual long DeletedSize { get; set; }
            public virtual lm.Comol.Core.DomainModel.BaseStatusDeleted Deleted { get; set; }

            public virtual Int32 OrderByFolder { get { return (Type == ItemType.Folder) ? 0 : 1; } }
            public virtual Int32 OrderByFile { get { return (Type != ItemType.Folder) ? 0 : 1; } }
        #endregion
        #region "Clone Of"
            public virtual long CloneOf { get; set; }
            public virtual System.Guid CloneOfUniqueId { get; set; }
        #endregion
        #region "Visibility"
            public virtual Boolean IsVisible { get; set; }
            public virtual long DisplayOrder { get; set; }
            public virtual ItemAvailability Availability { get; set; }
            public virtual ItemStatus Status { get; set; }
        #endregion
    
        #region "Owned By Service"
            public virtual Boolean IsInternal { get; set; }
            public virtual ItemModuleSettings Module { get; set; }
            public virtual Boolean IsVirtual { get; set; }
        #endregion
        public virtual RepositoryIdentifier Repository { get; set; }
        public dtoBaseItem()
        {
        }
        public dtoBaseItem(liteRepositoryItem item)
        {
            Availability = item.Availability;
            CloneOf = item.CloneOf;
            CloneOfUniqueId = item.CloneOfUniqueId;
            ContentType = item.ContentType;
            DeletedSize = item.DeletedSize;
            DisplayOrder = item.DisplayOrder;
            Extension = item.Extension;
            Id = item.Id;
            Deleted = item.Deleted;
            IdCommunity = item.IdCommunity;
            IdFolder = item.IdFolder;
            IdVersion = item.IdVersion;
            IsFile = item.IsFile;
            IsInternal = item.IsInternal;
            IsVirtual = item.IsVirtual;
            IsVisible = item.IsVisible;
            Module = item.Module;
            Name = item.Name;
            Size = item.Size;
            Status = item.Status;
            Type = item.Type;
            UniqueId = item.UniqueId;
            UniqueIdVersion = item.UniqueIdVersion;
            Url = item.Url;
            VersionsSize = item.VersionsSize;
            Repository=item.Repository;
        }

        public String ToString()
        {
            return "Id-" + Id + "-" + DisplayName + "-" + Type.ToString()  + "-" + Deleted.ToString();
        }
    }
}