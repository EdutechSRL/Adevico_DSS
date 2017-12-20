using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.Tickets.Domain.Enums;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    [Serializable, CLSCompliant(true)]
    public class DTO_ManagerModify
    {
        public DTO_ManagerModify()
        {
           
            TicketId = 0;
            Code = "TK00000000-0";
            Title = "";

            CategoryCreationName = "";

            Status = Enums.TicketStatus.draft;
            CommunityName = "";

            CategoryCurrentId = 0;
            CategoryCurrentName = "";
            UserAssigned = "";
            UserAssignedId = 0;
            PersonAssignedId = 0;
            LastAssignmentId = 0;
            
            Errors = Enums.TicketEditManErrors.none;

            IsClosed = false;

            CurrentUserDisplayName = "";
            CurrentUserType = Enums.MessageUserType.none;

            Condition = Enums.TicketCondition.active;

            //Notification
            MailSettings  = MailSettings.none;
            IsDefault = true;
            IsActiveUser = false;
        }

        public Int64 TicketId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }

        public String CategoryCreationName { get; set; }

        public Domain.Enums.TicketStatus Status { get; set; }
        public String CommunityName { get; set; }

        public Int64 CategoryCurrentId { get; set; }
        public String CategoryCurrentName { get; set; }
        //public liteAssignment LastAssignment { get; set; }
        public String UserAssigned { get; set; }
        public Int64 LastAssignmentId { get; set; }

        public IList<DTO_UserModifyItem> Messages { get; set; }

        public Domain.Enums.TicketEditManErrors Errors { get; set; }

        public Boolean IsClosed { get; set; }

        public String CurrentUserDisplayName { get; set; }
        public Domain.Enums.MessageUserType CurrentUserType { get; set; }

        public object Categories { get; set; }

        public Domain.Message DraftMessage { get; set; }

        public IList<Domain.DTO.DTO_AttachmentItem> GetDraftAttachments()
        {
            if(DraftMessage == null || DraftMessage.Attachments == null || !DraftMessage.Attachments.Any())
                return null;

             //If Not IsNothing(TicketData.DraftMessage.Attachments) Then
             //       Attachments = (From atc As TK.Domain.TicketFile _
             //                      In TicketData.DraftMessage.Attachments _
             //                      Select New TK.Domain.DTO.DTO_AttachmentItem(atc)).ToList()
             //   End If

            //List<Domain.DTO.DTO_AttachmentItem> DraftAttach =
            return (from TicketFile atc in DraftMessage.Attachments.ToList()
                select new Domain.DTO.DTO_AttachmentItem(atc)).ToList();

        }

        public DateTime? LastUserAccess { get; set; }

        /// <summary>
        /// Stato del blocco del Ticket
        /// </summary>
        public virtual Enums.TicketCondition Condition { get; set; }

        //NOTIFICATION
        public Domain.Enums.MailSettings MailSettings { get; set; }
        public bool IsDefault { get; set; }
        /// <summary>
        /// Indica SE l'utente corrente è DIRETTAMENTE coinvolto nel Ticket.
        /// In caso contrario NON riceverà notifiche a meno di non impostarle a livello di ticket.
        /// Il flag serve per disattivare i NotificationSettings, mostrando invece la relativa label...
        /// </summary>
        public bool IsActiveUser { get; set; }

        public bool IsNotificationActive { get; set; }

        public Int64 UserAssignedId { get; set; }
        public Int32 PersonAssignedId { get; set; }
    }
}
