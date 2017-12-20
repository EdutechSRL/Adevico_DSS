using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.Tickets.Domain.Enums;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    [Serializable, CLSCompliant(true)]
    public class DTO_TicketListItemManager
    {

        public Int64 Id { get; set; }
        public string Code { get; set; }

        public String Title { get; set; }
        //public String FirstMessage { get; set; }
        public String LangCode { get; set; }
        public String CommunityName { get; set; }

        //public DTO_Assignment LastAssignment { get; set; }
        //public String AssignedTo { get; set; }
        //public Boolean AssignedIsCatagory { get; set; }

        //public liteAssignment LastAssignemnt { get; set; }

        //public liteCategory CreationCategory { get; set; }
        public String CreationCategoryName { get; set; }

        public DateTime SendedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Domain.Enums.TicketStatus Status { get; set; }

        public Boolean HasNews { get; set; }

        public Boolean IsForManager { get; set; }

        public liteAssignment LastAssignment { get; set; }
        //public String LastAssignment_UserName { get; set; }
        //public String LastAssignment_CategoryName { get; set; }
        //public Boolean LastAssignment_IsManager { get; set; }
        //public DateTime LastAssignment_CreatedOn { get; set; }
        public Boolean IsAsgnToMe { get; set; }

        //public Boolean IsMine { get; set; }

        public Boolean HasNoAnswers { get; set; }

        public Domain.Enums.TicketCondition Condition { get; set; }

        public int NumAttachments { get; set; }

        public Enums.CategoryFilterStatus CategoryFilter { get; set; }

        public DTO_TicketListItemManager()
        {
            Id = 0;
            Code = "TK00000000-0";
            Title = "";
            LangCode = "";
            CommunityName= "";
            //AssignedTo= "";
            //AssignedIsCatagory= true;
            
            //CurrentCategory= "";
            SendedOn = DateTime.Now;
            Status = Enums.TicketStatus.open;

            HasNews = false;
            IsAsgnToMe = false;
            HasNoAnswers = true;

            CategoryFilter = CategoryFilterStatus.None;
        }

        //public DTO_TicketListItemManager(liteTicket Ticket)
        //{
        //    Code = Ticket.Id;
        //    Title = Ticket.Title;
        //    LangCode = Ticket.LanguageCode;
        //    CommunityName = (Ticket.Community != null) ? Ticket.Community.Name : "";

        //    LastAssignment_UserName = Ticket.LastAssignment.AssignetToSurNameAndName;


        //    //AssignedTo= "";
        //    //AssignedIsCatagory= true;

        //    //CurrentCategory= "";
        //    SendedOn = DateTime.Now;
        //    Status = Enums.TicketStatus.open;

        //    HasNews = false;
        //}

    }
}
