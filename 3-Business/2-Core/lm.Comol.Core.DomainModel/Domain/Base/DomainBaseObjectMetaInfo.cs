using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable()]
	public class DomainBaseObjectMetaInfo<T> : DomainBaseObject<T>, IDomainBaseObjectMetaInfo<T>
	{
        public virtual Person CreatedBy { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual String CreatorProxyIpAddress { get; set; }
        public virtual String CreatorIpAddress { get; set; }
        public virtual Person ModifiedBy { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual String ModifiedProxyIpAddress { get; set; }
        public virtual String ModifiedIpAddress { get; set; }

        public DomainBaseObjectMetaInfo(){
            Deleted = DomainModel.BaseStatusDeleted.None;
        }
        public virtual void CreateMetaInfo(Person person)
        {
            CreatedBy = person;
            CreatedOn = DateTime.Now;
            UpdateMetaInfo(person, BaseStatusDeleted.None);
        }
        public virtual void CreateMetaInfo(Person person, String IpAddress, String ProxyIpAddress, DateTime? date = null)
        {
            CreatedBy = person;
            CreatedOn = (date.HasValue)? date:DateTime.Now;
            CreatorIpAddress = IpAddress;
            CreatorProxyIpAddress = ProxyIpAddress;
            UpdateMetaInfo(person, BaseStatusDeleted.None);
        }
        public virtual void UpdateMetaInfo(Person user, DateTime? date = null) 
        {
            ModifiedBy = user;
            ModifiedOn = (date.HasValue) ? date.Value : DateTime.Now;
        }
        public virtual void UpdateMetaInfo(Person user, BaseStatusDeleted delete, DateTime? date = null)
        {
            UpdateMetaInfo(user);
            Deleted = delete;
        }

        public virtual void UpdateMetaInfo(Person user, string IpAddress, string ProxyIpAddress)
        {
            UpdateMetaInfo(user);
            ModifiedIpAddress = IpAddress;
            ModifiedProxyIpAddress = ProxyIpAddress;
        }
        public virtual void UpdateMetaInfo(Person user, string IpAddress, string ProxyIpAddress,DateTime date)
        {
            UpdateMetaInfo(user, date);
            ModifiedIpAddress = IpAddress;
            ModifiedProxyIpAddress = ProxyIpAddress;
        }
        public virtual void SetDeleteMetaInfo(Person person, String IpAddress, String ProxyIpAddress, DateTime? date = null)
        {
            ModifiedIpAddress = IpAddress;
            ModifiedProxyIpAddress = ProxyIpAddress;
            UpdateMetaInfo(person, BaseStatusDeleted.Manual, date);
        }
        public virtual void RecoverMetaInfo(Person person, String IpAddress, String ProxyIpAddress, DateTime? date = null )
        {
            ModifiedIpAddress = IpAddress;
            ModifiedProxyIpAddress = ProxyIpAddress;
            UpdateMetaInfo(person, BaseStatusDeleted.None, date);
        }
    }
}