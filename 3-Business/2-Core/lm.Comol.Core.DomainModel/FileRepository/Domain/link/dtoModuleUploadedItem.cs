using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable(), CLSCompliant(true)]
    public class dtoModuleUploadedItem
	{
        public dtoUploadedItem UploadedFile  {get;set;}
        public liteRepositoryItem ItemAdded { get; set; }
        public liteRepositoryItemVersion VersionAdded { get; set; }

        public virtual Boolean IsValid { get { return (UploadedFile != null && UploadedFile.IsValid); } }
        public virtual Boolean IsAdded { get { return (ItemAdded != null && ItemAdded.Id > 0); } }

        public virtual lm.Comol.Core.DomainModel.ModuleActionLink Link { get; set; }
        public virtual ItemUploadError Error { get; set; }
   
        public virtual ItemAvailability Availability { get { return (ItemAdded != null) ? ItemAdded.Availability : ItemAvailability.notuploaded; } }
        public virtual ItemStatus Status { get { return (ItemAdded != null) ? ItemAdded.Status : ItemStatus.Draft; } }
        public dtoModuleUploadedItem()
		{
            Error = ItemUploadError.None;
		}
        public dtoModuleUploadedItem(liteRepositoryItem item, liteRepositoryItemVersion version, ItemUploadError error)
        {
            Error = error;
            ItemAdded = item;
            VersionAdded = version;
        }
	}
}