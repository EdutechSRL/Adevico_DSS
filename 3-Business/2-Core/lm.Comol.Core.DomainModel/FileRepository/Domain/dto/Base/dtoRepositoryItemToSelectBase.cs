using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoRepositoryItemToSelectBase
    {
        public virtual long Id { get; set; }
        public virtual long IdFolder { get; set; }
        public virtual System.Guid UniqueId { get; set; }
        public virtual long IdVersion { get; set; }
        public virtual System.Guid UniqueIdVersion { get; set; }
        public virtual Boolean IgnoreVersion  { get; set; }
        public virtual ItemType Type { get; set; }           
        public virtual String Name { get; set; }
        public virtual String Url { get; set; }
        public virtual String Extension { get; set; }
        public virtual String DisplayName  { get; set; }
        public virtual long Size { get; set; }
        public virtual ItemModuleSettings Module { get; set; }
        public virtual Boolean Selectable { get; set; }
        public virtual Boolean Selected { get; set; }
        public String GetSize(Int32 decimals = 2)
        {
            return FolderSizeItem.FormatBytes(Size, decimals);
        }
        public dtoRepositoryItemToSelectBase()
        {
            Selectable = true;
            IgnoreVersion = true;
        }

        public dtoRepositoryItemToSelectBase(liteRepositoryItem item, Boolean selectable, Boolean selected)
        {
            Id = item.Id;
            IdFolder = item.IdFolder;
            UniqueId = item.UniqueId;
            IdVersion = item.IdVersion;
            UniqueIdVersion = item.UniqueIdVersion;
            IgnoreVersion = true;
            Type = item.Type;
            Name = item.Name;
            Extension = item.Extension;
            DisplayName = item.DisplayName;
            Url = item.Url;
            Size = item.Size;
            Module = item.Module;
            Url = item.Url;
            Selectable = selectable;
            Selected = selected;
        }
    }
  }