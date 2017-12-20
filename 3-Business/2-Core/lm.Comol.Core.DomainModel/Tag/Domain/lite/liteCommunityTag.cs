using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    [Serializable]
    public class liteCommunityTag : lm.Comol.Core.DomainModel.DomainBaseObject<long>, ICloneable
    {
        public virtual liteTag Tag { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual long IdTag { get { return (Tag == null) ? 0 : Tag.Id; } }

        
        public virtual object Clone()
        {
            liteCommunityTag clone = new liteCommunityTag();
            clone.Tag = (liteTag)Tag.Clone();
            clone.IdCommunity = IdCommunity;
            clone.Id = Id;
            clone.Deleted = Deleted;
            return clone;
        }
    }
}