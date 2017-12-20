using lm.Comol.Core.MailCommons.Domain.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    /// <summary>
    /// Impostazioni per la conversione dei TAG
    /// </summary>
    [Serializable, CLSCompliant(true)]
    public class DTO_NotificationSettings
    {
        /// <summary>
        /// Codice lingua - x TUTTI
        /// </summary>
        public String LangCode {get;set;}
        /// <summary>
        /// Formato data/ora - x TUTTI
        /// </summary>
        public String DateTimeFormat { get; set; }
        /// <summary>
        /// Formato data e ora per tutte le lingue di piattaforma.
        /// </summary>
        public Dictionary<String, String> DateTimeFormats { get; set; }
        /// <summary>
        /// BaseUrl dell'applicazione - x TUTTI
        /// </summary>
        public String BaseUrl { get; set; }
        /// <summary>
        /// Internazionalizzazione dei valori di Domain.Enums.TicketStatus - SOLO TICKET
        /// </summary>
        public Dictionary<Domain.Enums.TicketStatus, String> AvailableTicketStatus { get; set; }
        /// <summary>
        /// Internazionalizzazione dei valori di Domain.Enums.CAtegoryType - SOLO CATEGORY
        /// </summary>
        public Dictionary<Domain.Enums.CategoryType, String> AvailableCategoryTypes { get; set; }
        /// <summary>
        /// Eventuale Template per la lista con lingua/nome/descrizione delle categorie - SOLO CATEGORY
        /// </summary>
        public String CategoriesTemplate { get; set; }


        public SmtpServiceConfig SmtpConfig { get; set; }
        
    }
}
