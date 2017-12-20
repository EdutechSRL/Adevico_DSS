using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    [Serializable(), CLSCompliant(true)]
    public class Ticket : DomainBaseObjectMetaInfo<long>
    {
        /// <summary>
        /// costruttore (inizializza la data di apertura)
        /// </summary>
        public Ticket()
        {
            OpenOn = System.DateTime.Now;
            Status = Domain.Enums.TicketStatus.open;
            Assignemts = new List<Assignment>();
            Messages = new List<Message>();
            IsBehalf = false;
            IsHideToOwner = false;
        }

        /// <summary>
        /// Titolo/Oggetto del ticket
        /// </summary>
        public virtual String Title { get; set; }
        /// <summary>
        /// Utente PROPRIETARIO del Ticket. (se lo metto in behalf, sarà il "destinatario)
        /// </summary>
        public virtual TicketUser Owner { get; set; }
        /// <summary>
        /// Data Apertura Ticket: se creato in BOZZA, rimesso a "Now()" all'apertura effettiva
        /// </summary>
        public virtual DateTime OpenOn { get; set; }
        /// <summary>
        /// Categoria iniziale
        /// </summary>
        public virtual Category CreationCategory { get; set; }
        /// <summary>
        /// Eventuale lista assegnazioni (corrente e passate)
        /// </summary>
        public virtual IList<Assignment> Assignemts { get; set; }
        /// <summary>
        /// comunità di creazione -> Diventa Community
        /// </summary>
        public virtual lm.Comol.Core.DomainModel.Community Community { get; set; }
        /// <summary>
        /// Se l'utente interno decide di salvarlo come bozza
        /// </summary>
        public virtual Boolean IsDraft { get; set; }
        /// <summary>
        /// Codice lingua riferimento
        /// </summary>
        public virtual String LanguageCode { get; set; }
        /// <summary>
        /// Stato ticket
        /// </summary>
        public virtual Enums.TicketStatus Status { get; set; }

        /// <summary>
        /// Stato del blocco del Ticket
        /// </summary>
        public virtual Enums.TicketCondition Condition { get; set; }

        /// <summary>
        /// Anteprima testo - PLAIN TEXT
        /// </summary>
        public virtual String Preview { get; set; }

        /// <summary>
        /// Tempo trascorso dall'apertura
        /// </summary>
        public virtual TimeSpan OpenTime
        {
            get
            {
                return System.DateTime.Now - OpenOn;
            }
        }

        public virtual bool IsBehalf { get; set; }
        public virtual bool IsHideToOwner { get; set; }

        /// <summary>
        /// Lista messaggi inseriti
        /// </summary>
        public virtual IList<Message> Messages { get; set; }

        public virtual DateTime? LastUserAccess { get; set; }
        public virtual DateTime? LastCreatorAccess { get; set; }

        public virtual Assignment LastAssignment { get; set; }

        /// <summary>
        /// Codice stanza: CALCOLATO!
        /// </summary>
        /// <remarks>Al momento restituisce l'ID!</remarks>
        public virtual String Code
        {
            get
            {
                DateTime createdOn = System.DateTime.Now;
                if (this.CreatedOn != null)
                    createdOn = (DateTime)this.CreatedOn;
                
                return string.Format("TK{0}-{1}", createdOn.ToString("yyyyMMdd"), Id);
            }
        }
    }
}
