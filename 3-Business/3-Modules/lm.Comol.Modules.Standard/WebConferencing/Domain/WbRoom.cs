using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain
{
    public class WbRoom : DomainBaseObjectMetaInfo<long>
    {
        /// <summary>
        /// Identificato stanza su sistema esterno
        /// </summary>
        public virtual String ExternalId { get; set; }
        /// <summary>
        /// Nome stanza
        /// </summary>
        public virtual String Name { get; set; }
        /// <summary>
        /// Descrizione della stanza
        /// </summary>
        public virtual String Description { get; set; }
        /// <summary>
        /// Data prevista inizio
        /// </summary>
        public virtual DateTime? StartDate { get; set; }
        /// <summary>
        /// Data prevista fine
        /// </summary>
        public virtual DateTime? EndDate { get; set; }
        /// <summary>
        /// Durata prevista (se non definita, calcolata da adata fine)
        /// </summary>
        public virtual int Duration { get; set; }
        /// <summary>
        /// Massimo utenti previsti (non vincolante)
        /// </summary>
        public virtual Int32 MaxAllowUsers { get; set; }
        /// <summary>
        /// Numero corrente utenti (iscritti, non effettivamente nella stanza)
        /// </summary>
        public virtual Int32 CurrentUsersNumber { get; set; }
        /// <summary>
        /// Se la stanza a VISIBILITA' a livello di piattaforma. Non influisce sulle politiche di accesso ed iscrizione.
        /// </summary>
        public virtual Boolean Public { get; set; }
        /// <summary>
        /// ID Comunità in cui la stanza è stata creata
        /// </summary>
        public virtual Int32 CommunityId { get; set; }
        /// <summary>
        /// Parametri avanzati stanza (dipendono dall'implementazione
        /// </summary>
        public virtual WbRoomParameter Parameter { get; set; }
        /// <summary>
        /// Tipo di stanza
        /// </summary>
        public virtual RoomType Type { get; set; }
        /// <summary>
        /// Permessi iscrizione/accesso per iscritti alla comunità
        /// </summary>
        public virtual SubscriptionType SubCommunity { get; set; }
        /// <summary>
        /// Permessi iscrizione/accesso per iscritti al sistema
        /// </summary>
        public virtual SubscriptionType SubSystem { get; set; }
        /// <summary>
        /// Permessi iscrizione/accesso per utenti esterni
        /// </summary>
        public virtual SubscriptionType SubExternal { get; set; }

        /// <summary>
        /// Lingua di default per la stanza (Uso futuro?)
        /// </summary>
        public virtual String LanguageCode { get; set; }

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
        /// Indica se la registrazione AV della stanza è attiva o no.
        /// </summary>
        public virtual Boolean Recording { get; set; }
        ///// <summary>
        ///// Se la stanza è abilitata. Parametro non utilizzato!
        ///// </summary>
        ///// <remarks>
        ///// Presente su dB, ma attualmente inutilizzato
        ///// </remarks>
        //public virtual Boolean IsEnable { get; set; }
        //public virtual Int64 TemplateId { get; set; }
        /// <summary>
        /// Per allineamento.
        /// Indica se nel nome è presente l'ID per la relativa gestione.
        /// </summary>
        public virtual Boolean hasIdInName { get; set; }
    }
}