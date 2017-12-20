using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    [Serializable, CLSCompliant(true)]
    public class liteManResTicketAccess
    {
        public virtual Int64 Id { get; set; }
        public virtual Int64 TicketId { get; set; }
        public virtual Int64 UserId { get; set; }
        public virtual DateTime LastAccess { get; set; }
    }
}
