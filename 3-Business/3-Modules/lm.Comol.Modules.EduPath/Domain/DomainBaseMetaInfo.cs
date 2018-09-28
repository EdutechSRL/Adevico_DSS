using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using System.ComponentModel.DataAnnotations;

namespace lm.Comol.Modules.EduPath.Domain
{
    /// <summary>
    /// Layer Super Type + MetaInfo 
    /// </summary>
    [Serializable()]
    public abstract class DomainBaseMetaInfo:DomainBase
    {
        /// <summary>
        /// Creation Date
        /// </summary>
        [Required()]
        public virtual DateTime? CreatedOn { get; set; }
        
        /// <summary>
        /// Created By User
        /// </summary>
        [Required()]
        public virtual Person CreatedBy { get; set; }

        /// <summary>
        /// Creator's proxy IP Address
        /// </summary>
        public virtual String CreatorProxyIpAddress { get; set; }

        /// <summary>
        /// Creator's IP Address
        /// </summary>
        public virtual String CreatorIpAddress { get; set; }

        /// <summary>
        /// Modification Date
        /// </summary>
        public virtual DateTime? ModifiedOn { get; set; }
        
        /// <summary>
        /// Modified By User
        /// </summary>
        public virtual Person ModifiedBy { get; set; }

        /// <summary>
        /// Editor's proxy IP Address
        /// </summary>
        public virtual String ModifiedProxyIpAddress { get; set; }

        /// <summary>
        /// Editor's IP Address
        /// </summary>
        public virtual String ModifiedIpAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual void CreateMetaInfo(Person user)
        {
            this.CreatedBy = user;

            this.CreatedOn = DateTime.Now;

            UpdateMetaInfo(user, StatusDeleted.None);

        }
        public virtual void CreateMetaInfo(Person person, String IpAddress, String ProxyIpAddress)
        {
            CreatedBy = person;
            CreatedOn = DateTime.Now;
            CreatorIpAddress = IpAddress;
            CreatorProxyIpAddress = ProxyIpAddress;
            UpdateMetaInfo(person, StatusDeleted.None);
        }
        public virtual void UpdateMetaInfo(Person user)
        {
            this.ModifiedBy = user;

            this.ModifiedOn = DateTime.Now;
        }

        public virtual void UpdateMetaInfo(Person user, StatusDeleted delete)
        {

            UpdateMetaInfo(user);

            this.Deleted = delete;
        }
        public virtual void UpdateMetaInfo(Person user, string IpAddress, string ProxyIpAddress)
        {
            UpdateMetaInfo(user);
            ModifiedIpAddress = IpAddress;
            ModifiedProxyIpAddress = ProxyIpAddress;
        }
        public virtual void SetDeleteMetaInfo(Person person, String IpAddress, String ProxyIpAddress)
        {
            ModifiedIpAddress = IpAddress;
            ModifiedProxyIpAddress = ProxyIpAddress;
            UpdateMetaInfo(person, StatusDeleted.Manual);
        }
        public virtual void RecoverMetaInfo(Person person, String IpAddress, String ProxyIpAddress)
        {
            ModifiedIpAddress = IpAddress;
            ModifiedProxyIpAddress = ProxyIpAddress;
            UpdateMetaInfo(person, StatusDeleted.None);
        }
    }
}
