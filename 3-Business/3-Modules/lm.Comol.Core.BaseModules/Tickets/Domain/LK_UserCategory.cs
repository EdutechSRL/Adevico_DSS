using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;


namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    public class LK_UserCategory : DomainBaseObjectMetaInfo<long>
    {
        public virtual TicketUser User { get; set; }
        public virtual Category Category { get; set; }
        public virtual Boolean IsManager { get; set; }
    }
}
