
using System;
namespace lm.Comol.Core.DomainModel.Repository
{
	[Serializable(), CLSCompliant(true)]
	public class FilePlayInfo : DomainObject<long>
	{
		public virtual Person Owner{ get; set; }
		public virtual DateTime CreatedOn { get; set; }
		public virtual int DateZone { get; set; }
        public virtual int IdAction { get; set; }
		public virtual string WorkingSessionID { get; set; }
		public virtual BaseCommunityFile File{ get; set; }
		public virtual Community CommunityOwner{ get; set; }
        public virtual System.Guid FileUniqueID { get; set; }
        public virtual RepositoryItemType RepositoryItemType { get; set; }
        public FilePlayInfo()
		{
		}
	}
}