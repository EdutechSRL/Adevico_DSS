using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    [Serializable, CLSCompliant(true)]
    public class liteTicket
    {
        public virtual Int64 Id { get; set; }

        public virtual String Title { get; set; }
        public virtual String LanguageCode { get; set; }
        public virtual Domain.Enums.TicketStatus Status { get; set; }

        public virtual liteCommunity Community { get; set; }
        public virtual liteAssignment LastAssignment { get; set; }
        public virtual liteCategory CreationCategory { get; set; }

        public virtual DateTime OpenOn { get; set; }

        public virtual Boolean HasNews { get; set; }
        public virtual Boolean IsForManager { get; set; }

        public virtual IList<liteMessage> Messages { get; set; }
        public virtual IList<liteAssignment> Assignments { get; set; }

        public virtual DateTime? ModifiedOn { get; set; }

        public virtual Enums.TicketCondition Condition { get; set; }

        /// <summary>
        /// Codice stanza: CALCOLATO!
        /// </summary>
        /// <remarks>Al momento restituisce l'ID!</remarks>
        public virtual String Code
        {
            get
            {
                return string.Format("TK{0}-{1}", this.OpenOn.ToString("yyyyMMdd"), Id);
            }
        }
    }


}
