using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    [Serializable, CLSCompliant(true)]
    public class DTO_UserModifyItem
    {
        public Int64 MessageId { get; set; }
        public String UserDisplayName { get; set; }
        public Domain.Enums.MessageUserType UserType { get; set; }
        public Domain.Enums.MessageType MessageType { get; set; }
        public DateTime SendedOn { get; set; }
        public String MessagePreview { get; set; }
        public String MessageText { get; set; }
        public bool IsCloseMessage { get; set; }
        public bool IsVisible { get; set; }

        public bool IsBehalf { get; set; }

        /// <summary>
        /// Azione associata al messaggio
        /// </summary>
        public virtual Enums.MessageActionType Action { get; set; }

        /// <summary>
        /// Categoria corrente
        /// </summary>
        public virtual String ToCategory { get; set; }
        /// <summary>
        /// Assegnatario corrente
        /// </summary>
        public virtual String ToUser { get; set; }
        /// <summary>
        /// Stato corrente
        /// </summary>
        public virtual Enums.TicketStatus ToStatus { get; set; }
        /// <summary>
        /// Condizione corrente
        /// </summary>
        public virtual Enums.TicketCondition ToCondition { get; set; }
        /// <summary>
        /// Elenco allegati
        /// </summary>
        public virtual IList<DTO_AttachmentItem> Attachments { get; set; }

        public Boolean IsFirst { get; set; }
        public Boolean IsLast { get; set; }

        public virtual string CreatorName { get; set; }
    }
}
