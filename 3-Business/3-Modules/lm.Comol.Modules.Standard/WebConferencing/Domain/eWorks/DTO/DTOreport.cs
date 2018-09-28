using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.eWorks.DTO
{
    public class DTOreport
    {
        //public Int32 UserNumber { get; set; }
        public String UserId { get; set; }
        public String UserName { get; set; }
        public DateTime StartDate { get; set; }
        public Int32 Duration { get; set; }
        public Decimal UserPercentage { get; set; }
        public Int32 Access { get; set; }
        public Int64 TotalTransimittedData { get; set; }
        public Int64 TotalReceivedData { get; set; }
        public Boolean IsHost { get; set; }
    }
}
