using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    [Serializable, CLSCompliant(true)]
    public class DTO_ListFilterManager
    {
        public DTO_ListFilterManager()
        {
            OrderField = Enums.TicketOrderManRes.lastModify;
            OrderAscending = false;

            Title = "";

            DateStart = null;
            DateEnd = null;
            DateField = Enums.TicketManagerDateFilter.LastModify;

            Status = Enums.TicketStatus.open;
            ShowAllStatus = true;

            PageIndex = 0;
            PageSize = 25;

            RecordTotal = 0;
        }

        public Boolean ShowAllStatus { get; set; }
        public Enums.TicketOrderManRes OrderField { get; set; }
        public Boolean OrderAscending { get; set; }



        public Enums.TicketStatus Status { get; set; }
        public String Title { get; set; }
        public Enums.TicketManagerListOnly OnlyTicket { get; set; }
        

        public String LanguageCode { get; set; }

        
        public Enums.TicketManagerDateFilter DateField { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public Int64 CategoryId { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RecordTotal { get; set; }



    }
}
