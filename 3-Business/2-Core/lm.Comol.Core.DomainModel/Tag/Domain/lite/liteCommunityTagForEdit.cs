using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    [Serializable]
    public class liteCommunityTagForEdit : lm.Comol.Core.DomainModel.BaseMetaInfo<long>
    {
        public virtual long IdTag { get; set; }
        public virtual Int32 IdCommunity { get; set; }
    }
}