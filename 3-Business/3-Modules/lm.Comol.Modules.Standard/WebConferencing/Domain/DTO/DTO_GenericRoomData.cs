using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.DTO
{
    /// <summary>
    /// DTO dati generici stanza
    /// </summary>
    public class DTO_GenericRoomData
    {
        public DTO_GenericRoomData()
        {
            Recording = false;
        }

        public Int64 Id { get; set; }
        public bool HasIdInName { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Descrizione
        /// </summary>
        public String Description { get; set; }
        
        /// <summary>
        /// Data inizio previto
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Data fine prevista
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Durata
        /// </summary>
        public int Duration { get; set; } //Da definire...

        /// <summary>
        /// Massimo utenti previsti
        /// </summary>
        public Int32 MaxAllowUsers { get; set; }

        /// <summary>
        /// Stanza visibile in tutto il sistema
        /// </summary>
        public Boolean Public { get; set; }

        /// <summary>
        /// Permessi iscrizione iscritti comunità
        /// </summary>
        public virtual SubscriptionType SubCommunity { get; set; }
        /// <summary>
        /// Permessi iscrizione iscritti sistema
        /// </summary>
        public virtual SubscriptionType SubSystem { get; set; }
        /// <summary>
        /// Permessi iscrizione esterni
        /// </summary>
        public virtual SubscriptionType SubExternal { get; set; }

        /// <summary>
        /// Se inviare notifica via mail quando un utente viene Bloccato
        /// </summary>
        public virtual Boolean NotificationDisableUsr { get; set; }
        /// <summary>
        /// Se inviare notifica via mail quando un utente viene Sbloccato
        /// </summary>
        /// <remarks>
        /// Se utente esterno, con iscrizione su conferma, la mail viene inviata comunque
        /// </remarks>
        public virtual Boolean NotificationEnableUsr { get; set; }

        /// <summary>
        /// Se è attiva la registrazione per la stanza
        /// </summary>
        public virtual Boolean Recording { get; set; }

        public virtual Int64 TemplateId { get; set; }
    }
}
