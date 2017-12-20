using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoItemToAdd
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
        public virtual String ContentType { get; set; }
        public virtual Boolean IsFile { get; set; }
        public virtual Int32 IdOwner { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual Int32 IdModifiedBy { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual long Size { get; set; }
        public virtual String Description { get; set; }
        public virtual String Tags { get; set; }
        public virtual Boolean IsDownloadable { get; set; }
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
        public virtual long PreviewTime { get; set; }
        public virtual long Time { get; set; }
        public virtual long Number { get; set; }
        #endregion
        #region "Statistics"
        public virtual long IdPlayer { get; set; }
        #endregion

        #region "Owned By Service"
        public virtual Boolean IsInternal { get; set; }
        public virtual ItemModuleSettings Module { get; set; }
        public virtual Boolean IsVirtual { get; set; }
        #endregion


        public dtoItemToAdd() { }
    }
}