using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    [Serializable]
    public class OrganizationAvailability : lm.Comol.Core.DomainModel.DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual TagItem Tag { get; set; }
        public virtual Boolean IsDefault { get; set; }
        public virtual Int32 IdOrganization { get; set; }

        public OrganizationAvailability()
        {
            IsDefault = false;
        }
        public virtual OrganizationAvailability Copy(TagItem tag, litePerson person, String ipAddress, String proxyIpAddress, DateTime? createdOn)
        {
            OrganizationAvailability clone = new OrganizationAvailability();
            clone.Tag = tag;
            clone.IdOrganization = IdOrganization;
            clone.IsDefault = IsDefault;
            clone.CreateMetaInfo(person, ipAddress, proxyIpAddress, createdOn);
            return clone;
        }
    }
}