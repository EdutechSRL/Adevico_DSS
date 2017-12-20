
using System;
namespace lm.Comol.Core.DomainModel
{
	public interface IDomainBaseObjectMetaInfo<T> : iDomainBaseObject<T>
	{
        Person CreatedBy { get; set; }
        DateTime? CreatedOn { get; set; }
        String CreatorProxyIpAddress { get; set; }
        String CreatorIpAddress { get; set; }
        Person ModifiedBy { get; set; }
        DateTime? ModifiedOn { get; set; }
        String ModifiedProxyIpAddress { get; set; }
        String ModifiedIpAddress { get; set; }

        void CreateMetaInfo(Person user);
        void CreateMetaInfo(Person user, String IpAddress, String ProxyIpAddress, DateTime? date = null);
        void UpdateMetaInfo(Person user, DateTime? date = null);
        void UpdateMetaInfo(Person user, BaseStatusDeleted delete, DateTime? date = null);
        void UpdateMetaInfo(Person user, String IpAddress, String ProxyIpAddress);
	}
}