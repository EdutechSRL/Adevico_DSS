using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    [Serializable, CLSCompliant(true)]
    public class DTO_UserModify
    {
        public DTO_UserModify()
        {
            TicketId = -1;
            Code = "TK00000000-0";
            Status = Enums.TicketStatus.open;
            CategoryName = "";
            Notifications = new MailNotification();
            Messages = null;
            Errors = Enums.TicketEditUserErrors.none;
            IsReadOnly = true;
            IsBehalf = false;
            isOwner = false;
        }
        public Int64 TicketId { get; set; }
        public string Code { get; set; }
        public Domain.Enums.TicketStatus Status { get; set; }
        public String CategoryName { get; set; }
        public Domain.MailNotification Notifications { get; set; }
        public string Title { get; set; }
        public IList<DTO_UserModifyItem> Messages { get; set; }

        public Domain.Enums.TicketEditUserErrors Errors { get; set; }

        public Boolean IsClosed { get; set; }

        public String CurrentUserDisplayName { get; set; }
        public Domain.Enums.MessageUserType CurrentUserType { get; set; }

        /// <summary>
        /// Messaggio in DRAFT.
        /// Se non presente, ne viene creato uno vuoto.
        /// </summary>
        public Domain.Message DraftMessage { get; set; }

        public DateTime? LastUserAccess { get; set; }

        public Domain.Enums.TicketCondition Condition { get; set; }

        /// <summary>
        /// Indica SE il ticket è stato creato "per conto di..."
        /// </summary>
        public bool IsBehalf { get; set; }

        /// <summary>
        /// Indica se è visibile in sola lettura (Manager/Resolver che controllano ciò che l'utente vede)
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Utente non accesso + SysSettings + Behalf permission
        /// </summary>
        public bool ShowBehalf { get; set; }

        /// <summary>
        /// SE PUO' riassegnare a sè stesso
        /// </summary>
        public bool CanRemoveBehalf { get; set; }

        /// <summary>
        /// Ha permessi di behalf
        /// </summary>
        public bool BehalfRevoked { get; set; }

        /// <summary>
        /// Indica SE l'utente corrente è anche manager/resolver del Ticket, per lo switch tra vista utente e vista normale...
        /// </summary>
        public bool IsManagerOrResolver { get; set; }



        //Visibilità tasti navigazione
        public bool ShowToUserList { get; set; }
        public bool ShowToManagementList { get; set; }
        public bool ShowToBehalfList { get; set; }

        /// <summary>
        /// Indica SE il Ticket è visibile o meno all'utente
        /// </summary>
        public bool IsHideToOwner { get; set; }

        //NOTIFICATION
        public Domain.Enums.MailSettings CreatorMailSettings { get; set; }
        public bool IsDefaultNotCreator { get; set; }
        public Domain.Enums.MailSettings OwnerMailSettings { get; set; }
        public bool IsDefaultNotOwner { get; set; }

        public bool isOwner { get; set; }

        public bool IsOwnerNotificationEnable { get; set; }
        public bool IsCreatorNotificationEnable { get; set; }

    }
}
