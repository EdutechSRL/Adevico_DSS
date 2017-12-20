using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoRepositoryItemToSelect : dtoRepositoryItemToSelectBase
    {
        public virtual DateTime DisplayDate { get; set; }
        public virtual String Path { get; set; }
        public virtual ItemAvailability Availability { get; set; }
        public virtual long Downloads { get; set; }
        public virtual long Plays { get; set; }
        public virtual long MyPlays { get; set; }
        public virtual long MyDownloads { get; set; }

        public virtual Int32 OrderByFolder { get { return (Type == ItemType.Folder) ? 0 : 1; } }
        public virtual Int32 OrderByFile { get { return (Type != ItemType.Folder) ? 0 : 1; } }
        public dtoRepositoryItemToSelect(): base()
        {

        }

        public dtoRepositoryItemToSelect(liteRepositoryItem item, String path, Boolean selectable, Boolean selected): base(item,selectable,selected)
        {
            DisplayDate = (item.ModifiedOn.HasValue ? item.ModifiedOn.Value : item.CreatedOn.Value);
            Availability = item.Availability;
            Downloads = item.Downloaded;
            Path = path;
        }
        public dtoRepositoryItemToSelect(liteRepositoryItem item, String path,long download, long plays,  Boolean selectable, Boolean selected)
            : base(item, selectable, selected)
        {
            DisplayDate = (item.ModifiedOn.HasValue ? item.ModifiedOn.Value : item.CreatedOn.Value);
            Availability = item.Availability;
            Plays = plays;
            Downloads = download;
            Path = path;
        }
    }
  }