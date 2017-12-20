using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class RepositoryItemVersion : BaseRepositoryItem
    {
        public virtual RepositoryItem File { get; set; }
        public virtual System.Guid UniqueIdItem { get; set; }
        public virtual System.Guid UniqueIdVersion { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Boolean IsActive { get; set; }
        public virtual long IdPlayer { get; set; }

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
    }
}