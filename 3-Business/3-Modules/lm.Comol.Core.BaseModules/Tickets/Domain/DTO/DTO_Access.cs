using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    public class DTO_Access
    {
        /// <summary>
        /// Indica SE il servizio è attivo
        /// </summary>
        /// <remarks>
        /// NON MAPPATO: controllo sul sistema
        /// </remarks>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Indica se è attivo il management delle categorie
        /// </summary>
        public virtual bool CanManageCategory { get; set; }
        /// <summary>
        /// Indica se è possibile visualizzare i Ticket.
        /// </summary>
        public virtual bool CanShowTicket { get; set; }
        /// <summary>
        /// Indica se è possibile modificare o creare Ticket.
        /// Se disattivo potrebbero essere visibili in sola lettura.
        /// </summary>
        public virtual bool CanEditTicket { get; set; }

        public virtual int MaxSended { get; set; }
        public virtual int MaxDraft { get; set; }
    }
}
