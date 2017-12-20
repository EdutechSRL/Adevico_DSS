using System;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Core.DomainModel.Repository
{
	[Serializable(), CLSCompliant(true)]
	public class FileDownloadInfo : DomainObject<long>
	{
        public virtual Community CommunityOwner { get; set; }
        public virtual Person Downloader { get; set; }
        public virtual BaseCommunityFile File { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual System.Guid UniqueID { get; set; }
        public virtual RepositoryItemType RepositoryItemType { get; set; }
        public virtual string ServiceCode { get; set; }
        public virtual ModuleLink Link { get; set; }

		public FileDownloadInfo()
		{
			CreatedOn = DateTime.Now;
		}
	}
}