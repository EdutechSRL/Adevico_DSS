using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    public class liteTinyAssignment
    {
        public virtual Int64 Id { get; set; }
        public virtual Domain.Enums.AssignmentType Type { get; set; }
        public virtual Int64? TicketId { get; set; }
        public virtual Int64? CategoryId { get; set; }
        public virtual Int64? UserId { get; set; }
        public virtual bool IsCurrent { get; set; }
    }
}
