using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    [Serializable, CLSCompliant(true)]
    public class DTO_TicketListItemUser
    {
        public Int64 Id { get; set; }
        public String Code { get; set; }
        public String Title { get; set; }
        public String FirstMessage { get; set; }
        public String CreationCategoryName { get; set; }
        public TimeSpan LifeTime { get; set; }
        public DateTime SendOn { get; set; }
        public DateTime? LastModify { get; set; }
        public DateTime CreateOn { get; set; }
        public Domain.Enums.TicketStatus Status { get; set; }

        public Boolean IsDraft { get; set; }
        public Boolean HasNews { get; set; }

        public Domain.Enums.TicketCondition Condition { get; set; }
        //public int AttachNum { get; set; }
        //public int MessagesNum { get; set; }

        /// <summary>
        /// Nome proprietario
        /// </summary>
        public String OwnerName { get; set; }

        /// <summary>
        /// SE è stato creato per conto di...
        /// </summary>
        public bool IsBehalf { get; set; }

        /// <summary>
        /// SE è stato creato per conto di ed è nascosto all'utente
        /// </summary>
        public bool IsHideToOwner { get; set; }

        public DTO_TicketListItemUser()
        {
            Code = "TK-00000000-0";
            Id = 0;
            Title = "";
            CreationCategoryName = "";
            LifeTime = new TimeSpan(0);
            SendOn = DateTime.Now;
            Status = Enums.TicketStatus.draft;
            IsDraft = true;
            HasNews = false;
            //LastUserAccess = DateTime.Now;

            //AttachNum = 0;
            //MessagesNum = 0;
        }
    }
}
