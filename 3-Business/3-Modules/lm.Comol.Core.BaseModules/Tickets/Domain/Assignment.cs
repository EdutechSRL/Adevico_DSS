using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    /// <summary>
    /// Storico assegnazione categorie/utenti
    /// </summary>
    [Serializable()]
    public class Assignment : DomainBaseObjectMetaInfo<long>
    {
        /// <summary>
        /// Ticket di riferimento
        /// </summary>
        public virtual Ticket Ticket { get; set; }
        ///// <summary>
        ///// Data Assegnazione
        ///// </summary>
        //public virtual DateTime Creation { get; set; }
        /// <summary>
        /// Eventuale utente assegnatario
        /// </summary>
        public virtual TicketUser AssignedTo { get; set; }
        /// <summary>
        /// Categoria assegnataria
        /// </summary>
        /// <remarks>
        /// LA CATEGORIA DEVE SEMPRE essere assegnata!!!!
        /// </remarks>
        public virtual Category AssignedCategory { get; set; }
        /// <summary>
        /// L'ultima assegnazione sarà sempre quella "corrente"
        /// </summary>
        public virtual Boolean IsCurrent { get; set; }


        /// <summary>
        /// Tipo assegnazione x migliorare ricerche
        /// </summary>
        public virtual Enums.AssignmentType Type { get; set; }
    }
}
