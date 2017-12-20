using System;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable(), CLSCompliant(true)]
    public class DownloadStatistics : BaseItemIdentifiers
	{
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdPerson { get; set; }
        public virtual ItemType ItemType { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual String CreatedIPaddress { get; set; }
        public virtual String CreatedProxyIPaddress { get; set; }
        public virtual RepositoryType RepositoryType { get; set; }
        public virtual Int32 RepositoryIdCommunity { get; set; }
        public virtual Int32 RepositoryIdPerson { get; set; }
        public DownloadStatistics()
		{
			CreatedOn = DateTime.Now;
		}
	}
}