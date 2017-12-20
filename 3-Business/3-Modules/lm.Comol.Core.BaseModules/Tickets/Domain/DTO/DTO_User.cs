using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    [Serializable]
    [CLSCompliant(true)]
    public class DTO_User
    {
        /// <summary>
        /// ID utente Ticket
        /// </summary>
        public Int64 UserId { get; set; }
        /// <summary>
        /// ID utente COMOL.
        /// SE ESTERNO = -1
        /// </summary>
        public Int64 PersonId { get; set; }
        /// <summary>
        /// Nome utente
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Cognome utente
        /// </summary>
        public String SName { get; set; }
        /// <summary>
        /// Mail utente
        /// </summary>
        public String Mail { get; set; }
        /// <summary>
        /// Codice lingua utente
        /// </summary>
        public String LanguageCode { get; set; }

        public bool IsOwnerNotificationEnable { get; set; }

        public DTO_User()
        {
            UserId = 0;
            PersonId = 0;

            Name = "";
            SName = "";
            Mail  = "";

            LanguageCode = TicketService.LangMultiCODE;
        }
    }
}
