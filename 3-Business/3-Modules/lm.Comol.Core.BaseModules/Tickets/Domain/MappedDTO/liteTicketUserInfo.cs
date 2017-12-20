using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    [Serializable, CLSCompliant(true)]
    public class liteTicketUserInfo
    {
        public virtual Int64 Id { get; set; }
        public virtual Int64 OwnerId { get; set; }
        public virtual Int64 _CreatedById { get; set; }


        public virtual Domain.Enums.TicketStatus Status { get; set; }

        public virtual bool IsBehalf { get; set; }
        public virtual bool IsHide { get; set; }
        public virtual bool IsDraft { get; set; }
    }
}
