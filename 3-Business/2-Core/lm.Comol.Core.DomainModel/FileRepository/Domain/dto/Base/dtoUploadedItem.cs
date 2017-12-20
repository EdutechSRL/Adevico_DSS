using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoUploadedItem
    {
       public virtual long Id { get; set; }
       public virtual System.Guid UniqueId { get; set; }
       public String OriginalName { get; set; }
       public Boolean IsVisible { get; set; }

       #region "Data"
            public virtual ItemType Type { get; set; }
            public virtual String Name { get; set; }
            public virtual String Extension { get; set; }
            public virtual String DisplayName
            {
                get
                {
                    switch (Type)
                    {
                        case ItemType.Multimedia:
                        case ItemType.VideoStreaming:
                        case ItemType.ScormPackage:
                        case ItemType.File:
                            return Name + Extension;
                        case ItemType.Folder:
                            return Name;
                        default:
                            return Name;
                    }
                }
            }
            public virtual String ContentType { get; set; }
            public virtual long Size { get; set; }
            public virtual String Tags { get; set; }
       #endregion
        #region "Owned By Service"
            public virtual Boolean IsInternal { get { return Module == null; } }
            public virtual ItemModuleSettings Module { get; set; }
        #endregion
        public virtual String SavedPath { get; set; }
        public virtual String SavedFullPath { get; set; }
        public virtual String SavedFileName { get; set; }

        public virtual Boolean  AutoThumbnail { get; set; }
        public virtual String ThumbnailPath { get; set; }
        public virtual String ThumbnailFileName { get; set; }
        public virtual String ThumbnailFullPath { get { return ThumbnailPath + ThumbnailFileName; } }

        public virtual Boolean IsValid { get { return !String.IsNullOrWhiteSpace(SavedFileName) && !String.IsNullOrWhiteSpace(SavedPath) && !String.IsNullOrWhiteSpace(SavedFullPath); } }
        public virtual Boolean HasThumbnail { get { return !String.IsNullOrWhiteSpace(ThumbnailFileName) && !String.IsNullOrWhiteSpace(ThumbnailPath); } }
        public virtual Boolean IsRenamed { get { return String.Compare(OriginalName, DisplayName, true) == 0; } }
        public dtoUploadedItem() { }

    }
}