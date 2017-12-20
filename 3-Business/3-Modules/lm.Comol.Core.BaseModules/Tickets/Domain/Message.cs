using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    /// <summary>
    /// singolo messaggio di un Ticket
    /// </summary>
    [Serializable()]
    public class Message : DomainBaseObjectMetaInfo<long>
    {
        public Message()
        {
            CreatedOn = DateTime.Now;
            SendDate = DateTime.Now;
        }
        /// <summary>
        /// Testo meggassaggio (HTML)
        /// </summary>
        public virtual String Text { get; set; }
        /// <summary>
        /// Testo meggassaggio (HTML)
        /// </summary>
        public virtual String Preview { get; set; }
        /// <summary>
        /// Utente che ha creato il Ticket
        /// </summary>
        public virtual TicketUser Creator { get; set; }
        /// <summary>
        /// Data invio EFFETTIVO!
        /// </summary>
        /// <remarks>
        /// Serve "SOLO" per il primo messaggio, nel momento in cui il Ticket è creato "in bozza".
        /// Altrimenti corrisponde solo alla data di creazione!
        /// </remarks>
        public virtual DateTime SendDate { get; set; }
        /// <summary>
        /// Elenco eventuali allegati
        /// </summary>
        public virtual IList<TicketFile> Attachments { get; set; }
        /// <summary>
        /// Se TRUE visibile all'utente, altrimenti solo a Manager/Resolver coinvolti
        /// </summary>
        public virtual Boolean Visibility { get; set; }
        /// <summary>
        /// Ticket a cui appartiene
        /// </summary>
        public virtual Ticket Ticket { get; set; }
        /// <summary>
        /// Per Manager/Resolver.
        /// TRUE:  mostra il nome originale
        /// FALSE: mostra la relativa categoria
        /// </summary>
        public virtual Boolean ShowRealName { get; set; }

        /// <summary>
        /// SE ShowRealName == false,
        /// mostra il DisplayName
        /// </summary>
        public virtual String DisplayName { get; set; }

        /// <summary>
        /// Tipo messaggio - richiesta, risposta, systema, ...
        /// </summary>
        public virtual Enums.MessageType Type { get; set; }
        /// <summary>
        /// Tipo creatore messaggio (Ruolo)
        /// </summary>
        public virtual Enums.MessageUserType UserType { get; set; }



        /// <summary>
        /// Azione associata al messaggio
        /// </summary>
        public virtual Enums.MessageActionType Action { get; set; }

        /// <summary>
        /// Categoria corrente
        /// </summary>
        public virtual Domain.Category ToCategory { get; set; }
        /// <summary>
        /// Assegnatario corrente
        /// </summary>
        public virtual Domain.TicketUser ToUser { get; set; }
        /// <summary>
        /// Stato corrente
        /// </summary>
        public virtual Enums.TicketStatus ToStatus { get; set; }
        /// <summary>
        /// Indica se il messaggio è "in draft", ovvero non inviato.
        /// Serve per poter allegare file PRIMA dell'ionvio del messaggio.
        /// </summary>
        public virtual Boolean IsDraft { get; set; }

        public virtual bool IsBehalf { get; set; }


        public virtual Domain.Enums.TicketCondition ToCondition { get; set; }
    }
}
