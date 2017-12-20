using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    [Serializable, CLSCompliant(true)]
    public class liteCommunity
    {
        public virtual Int32 Id { get; set; }
        public virtual String Name { get; set; }
    }
}
