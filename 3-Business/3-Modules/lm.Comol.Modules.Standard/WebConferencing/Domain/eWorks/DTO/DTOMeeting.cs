using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.eWorks.DTO
{
    public class DTOMeeting
    {
        public String MeetingId { get; set; }
        public String MasterKey { get; set; }
        public String Subject { get; set; }
        public DateTime StartDate { get; set; }
        public Int64 DurationSEC { get; set; }
        public Int32 PartecipantCount { get; set; }
        public String Host { get; set; }
        public Boolean ShowCase { get; set; }
    }
}
