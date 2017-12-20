using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    [Serializable]
    public class CommunityConstraint : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual Int32 IdSource { get; set; }
        public virtual Int32 IdDestinationCommunity { get; set; }
        public virtual ModuleObject Object { get; set; }
        public virtual ConstraintType Type { get; set; }

        public CommunityConstraint()
        {

        }
    }
}