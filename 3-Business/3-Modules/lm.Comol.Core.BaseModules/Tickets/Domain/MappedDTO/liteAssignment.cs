using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    [Serializable, CLSCompliant(true)]
    public class liteAssignment
    {
        public virtual Int64 Id { get; set; }

        public virtual liteTicket Ticket { get; set; }

        public virtual Domain.Enums.AssignmentType Type { get; set; }
        public virtual DateTime? CreatedOn { get; set; }

        public virtual liteUser AssignedTo { get; set; }
        public virtual liteCategory AssignedCategory { get; set; }

        public virtual String AssignetToSurNameAndName
        {
            get {
                if (AssignedTo == null)
                    return "";
                if (AssignedTo.Person != null)
                    return AssignedTo.Person.SurnameAndName;

                return AssignedTo.SurnameAndName;
            }
            set { }
        }

        public virtual bool IsCurrent { get; set; }
    }
}
