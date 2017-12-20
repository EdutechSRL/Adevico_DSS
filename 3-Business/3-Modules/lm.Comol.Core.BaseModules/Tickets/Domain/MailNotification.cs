using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    /// <summary>
    /// Impostazioni notifiche per l'utente
    /// </summary>
    [Serializable(), CLSCompliant(true)]
    public class MailNotification : DomainBaseObjectMetaInfo<long>
    {
        /// <summary>
        /// Proprietario
        /// </summary>
        public virtual TicketUser User { get; set; }
        /// <summary>
        /// Ticket relativo, se relativo al ticket
        /// </summary>
        public virtual Ticket Ticket { get; set; }
        /// <summary>
        /// Se a livello di portale (Ticket e CommunityId verranno ignorati)
        /// </summary>
        public virtual Boolean IsPortal { get; set; }
        /// <summary>
        /// Se a livello di comunità (Ticket verrà ignorato)
        /// </summary>
        public virtual Int32 IdCommunity { get; set; }
        /// <summary>
        /// Impostazioni
        /// </summary>
        public virtual Enums.MailSettings Settings { get; set; }
        //public virtual Enums.MailSettings ManagerSettings { get; set; }

        public virtual Boolean IsDefaultUser { get; set; }
        public virtual Boolean IsDefaultManager { get; set; } 


        public MailNotification()
        {
            User = null;
            Ticket = null;
            IsPortal = false;
            IdCommunity = 0;
            Settings = 0;
            //ManagerSettings = 0;
            IsDefaultUser = true;
            IsDefaultManager = true;
        }
    }
}
