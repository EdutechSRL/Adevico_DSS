using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    [Serializable, CLSCompliant(true)]
    public class DTO_Ticket
    {
        public Int64 TicketId { get; set; }

        public String Code { get; set; }

        public string LanguageCode { get; set; }
        /// <summary>
        /// Titolo/Oggetto del ticket
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// Testo meggassaggio (HTML)
        /// </summary>
        public String Text { get; set; }

        /// <summary>
        /// Anteprima testo - SOLO testo!
        /// </summary>
        public String Preview { get; set; }
        
        public bool IsDraft { get; set; }

        //public Enums.MailSettings MailSettings { get; set; }

        public Int64 CategoryId { get;set; }

        public int CommunityId { get; set; }

        //Solo per Action!
        public Int64 CreatorId { get; set; }

        //Attachment al primo messaggio
        public IList<Domain.DTO.DTO_AttachmentItem> Attachments { get; set; }

        //Owner
        public String OwnerName { get; set; }
        public String OwnerSName { get; set; }
        public String OwnerMail { get; set; }

        public Int64 OwnerId { get; set; }

        //Creator
        public String Creator { get; set; }
        
        public DTO_Ticket()
        {
            Title = "";
            Text = "";
            IsDraft = true;
            //MailSettings = 0;
            CategoryId = 0;
            Code = "";
            TicketId = 0;
        }

        /// <summary>
        /// All'invio, l'ID messaggio in DRAFT diventa l'ID dell'ultimo messaggio per invio notifiche: behalf se behalf, altrimenti primo messaggio!
        /// </summary>
        public Int64 DraftMsgId { get; set; }

        /// <summary>
        /// Indica SE il ticket corrente è stato creato "per conto di..."
        /// </summary>
        public bool IsBehalf { get; set; }

        /// <summary>
        /// Indica se l'utente corrente è il proprietario del Ticket.
        /// </summary>
        public bool IsOwner { get; set; }

        /// <summary>
        /// Indica se sarà visibile (e notificabile) all'utente
        /// </summary>
        public bool HideToOwner { get; set; }


        /// <summary>
        /// NOTIFICATION
        /// </summary>
        public Domain.Enums.MailSettings MailSettings { get; set; }


        public bool IsOwnerNotificationActive { get; set; }

    }
}
