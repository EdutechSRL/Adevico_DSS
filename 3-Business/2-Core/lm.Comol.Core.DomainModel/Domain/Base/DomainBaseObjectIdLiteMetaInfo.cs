using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable()]
	public class DomainBaseObjectIdLiteMetaInfo<T> : DomainBaseObject<T>
	{
        public virtual Int32 IdCreatedBy { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual String CreatorProxyIpAddress { get; set; }
        public virtual String CreatorIpAddress { get; set; }
        public virtual Int32 IdModifiedBy { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual String ModifiedProxyIpAddress { get; set; }
        public virtual String ModifiedIpAddress { get; set; }

        public DomainBaseObjectIdLiteMetaInfo()
        {
            Deleted = DomainModel.BaseStatusDeleted.None;
        }
        public virtual void CreateMetaInfo(Int32 idPerson)
        {
            IdCreatedBy = idPerson;
            CreatedOn = DateTime.Now;
            UpdateMetaInfo(idPerson, BaseStatusDeleted.None);
        }
        public virtual void CreateMetaInfo(Int32 idPerson, String ipAddress, String proxyIpAddress, DateTime? date = null)
        {
            IdCreatedBy = idPerson;
            CreatedOn = (date.HasValue) ? date : DateTime.Now;
            CreatorIpAddress = ipAddress;
            CreatorProxyIpAddress = proxyIpAddress;
            UpdateMetaInfo(idPerson, BaseStatusDeleted.None);
        }
        public virtual void UpdateMetaInfo(Int32 idPerson, DateTime? date = null)
        {
            IdModifiedBy = idPerson;
            ModifiedOn = (date.HasValue) ? date.Value : DateTime.Now;
        }
        public virtual void UpdateMetaInfo(Int32 idPerson, BaseStatusDeleted delete, DateTime? date = null)
        {
            UpdateMetaInfo(idPerson);
            Deleted = delete;
        }

        public virtual void UpdateMetaInfo(Int32 idPerson, string ipAddress, string proxyIpAddress)
        {
            UpdateMetaInfo(idPerson);
            ModifiedIpAddress = ipAddress;
            ModifiedProxyIpAddress = proxyIpAddress;
        }
        public virtual void UpdateMetaInfo(Int32 idPerson, string ipAddress, string proxyIpAddress, DateTime date)
        {
            UpdateMetaInfo(idPerson, date);
            ModifiedIpAddress = ipAddress;
            ModifiedProxyIpAddress = proxyIpAddress;
        }
        public virtual void UpdateMetaInfo(Int32 idPerson, string ipAddress, string proxyIpAddress, DateTime? date = null)
        {
            UpdateMetaInfo(idPerson, date);
            ModifiedIpAddress = ipAddress;
            ModifiedProxyIpAddress = proxyIpAddress;
        }
        public virtual void SetDeleteMetaInfo(Int32 idPerson, String ipAddress, String proxyIpAddress, DateTime? date = null)
        {
            ModifiedIpAddress = ipAddress;
            ModifiedProxyIpAddress = proxyIpAddress;
            UpdateMetaInfo(idPerson, BaseStatusDeleted.Manual, date);
        }
        public virtual void RecoverMetaInfo(Int32 idPerson, String ipAddress, String proxyIpAddress, DateTime? date = null)
        {
            ModifiedIpAddress = ipAddress;
            ModifiedProxyIpAddress = proxyIpAddress;
            UpdateMetaInfo(idPerson, BaseStatusDeleted.None, date);
        }
    }
}