using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    [Serializable, CLSCompliant(true)]
    public class liteMessage
    {
        public virtual Int64 Id { get; set; }

        public virtual Domain.Enums.MessageType Type { get; set; }
        public virtual liteTicket Ticket { get; set; }

        public virtual DateTime? CreatedOn { get; set; }

        public virtual Boolean Visibility { get; set; }

        public virtual bool IsDraft { get; set; }
    }
}
