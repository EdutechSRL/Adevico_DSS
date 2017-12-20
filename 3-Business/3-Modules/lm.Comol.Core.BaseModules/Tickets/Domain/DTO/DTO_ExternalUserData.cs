using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    [Serializable, CLSCompliant(true)]
    public class DTO_ExternalUserData
    {
        /// <summary>
        /// ID Utente TICKET
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// Nome
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Cognome
        /// </summary>
        public String SName { get; set; }
        /// <summary>
        /// Codice linge
        /// </summary>
        public String LanguageCODE { get; set; }
        /// <summary>
        /// Mail utente
        /// </summary>
        public String Mail { get; set; }
        /// <summary>
        /// Se l'utente è interno o esterno
        /// </summary>
        public Boolean IsInternal { get; set; }
    }
}
