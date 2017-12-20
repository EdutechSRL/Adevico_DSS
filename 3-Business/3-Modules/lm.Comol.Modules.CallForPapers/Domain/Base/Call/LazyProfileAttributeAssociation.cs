using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class LazyProfileAttributeAssociation : DomainBaseObject<long>
    {
        public virtual Int64 IdCall { get; set; }
        public virtual Int64 IdField { get; set; }
        public virtual lm.Comol.Core.Authentication.ProfileAttributeType  Attribute {get; set;}

        public LazyProfileAttributeAssociation()
        {
            Attribute = Core.Authentication.ProfileAttributeType.unknown;
        }
    }
}
