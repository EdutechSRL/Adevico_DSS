using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.eWorks.DTO
{
    public class DTOKeyInfo
    {
        /// <summary>
        /// eMail dell'utente a cui è assegnata la chiave
        /// </summary>
        public String Email { get; set; }
        /// <summary>
        /// nomde dell'utente a cui è assegnata la chiave
        /// </summary>
        public String ClientName { get; set; }
        /// <summary>
        /// Se la chiave è di un host o partecipante
        /// </summary>
        public Boolean IsHost { get; set; }
        /// <summary>
        /// MeetingId della chiave, identifica il meeting e tutti i partecipanti
        /// </summary>
        public String MeetingId { get; set; }
        /// <summary>
        /// host/ip portale relativo alla chiave
        /// </summary>
        public String PortalName { get; set; }
        /// <summary>
        /// e-mail dell'host del meeting relativo alla chiave
        /// </summary>
        public String HosteMail { get; set; }
        /// <summary>
        /// nome dell'host del meeting relativo alla chiave
        /// </summary>
        public String HostClientName { get; set; }
    }
}
