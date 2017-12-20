using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    /// <summary>
    /// DTO ruolo utente
    /// </summary>
    [Serializable]
    [CLSCompliant(true)]
    public class DTO_UserRole
    {
        /// <summary>
        /// Utente
        /// </summary>
        public Domain.TicketUser User { get; set; }
        /// <summary>
        /// Se è manager (altrimenti resolver)
        /// </summary>
        public Boolean IsManager { get; set; }
        /// <summary>
        /// Costruttore
        /// </summary>
        public DTO_UserRole()
        {
            User = new TicketUser();
            IsManager = false;
        }
    }
}
