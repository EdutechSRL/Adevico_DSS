using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    [Serializable, CLSCompliant(true)]
    public class liteDraftTicket
    {
        public virtual Int64 Id { get; set; }
        public virtual Int64 CreatorId { get; set; }
        public virtual Boolean IsDraft { get; set; }

        public virtual lm.Comol.Core.DomainModel.BaseStatusDeleted Deleted { get; set; }

    }
}
