using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    [Serializable]
    public class CommunityTag : lm.Comol.Core.DomainModel.DomainBaseObjectLiteMetaInfo<long>, ICloneable, ITagBaseItem
    {
        public virtual TagItem Tag { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual object Clone()
        {
            CommunityTag clone = new CommunityTag();
            clone.Tag = Tag;
            clone.IdCommunity = IdCommunity;
            return clone;
        }

        public virtual CommunityTag Copy(TagItem tag, litePerson person, String ipAddress, String proxyIpAddress, DateTime? createdOn)
        {
             CommunityTag clone = new CommunityTag();
            clone.Tag = tag;
            clone.IdCommunity = IdCommunity;
            clone.CreateMetaInfo(person, ipAddress, proxyIpAddress,createdOn);
            return clone;
        }
    }
}