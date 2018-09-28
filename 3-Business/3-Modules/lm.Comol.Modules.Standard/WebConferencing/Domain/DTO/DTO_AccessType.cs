using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.DTO
{
    /// <summary>
    /// Tipo di accesso per compilazione lista in management
    /// </summary>
    public class DTO_AccessType
    {
        /// <summary>
        /// ID: Attualmente non usaro, per implementazione tipi personalizzati
        /// </summary>
        public Int32 ID { get; set; }
        /// <summary>
        /// Nome visualizzato
        /// </summary>
        public String DisplayName { get; set; }
        /// <summary>
        /// Permessi iscrizione/accesso
        /// </summary>
        public WebConferencing.Domain.SubscriptionType SelectedType { get; set; }
        /// <summary>
        /// SE è di sistema (attualmente tutti TRUE)
        /// </summary>
        public Boolean IsSystem { get; set; }
    }
}
