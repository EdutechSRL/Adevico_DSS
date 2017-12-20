using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Federation.Domain
{
    public class liteFederation : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual int CommunityId { get; set; }
        public virtual Enums.FederationType Type { get; set; }

    }
}
