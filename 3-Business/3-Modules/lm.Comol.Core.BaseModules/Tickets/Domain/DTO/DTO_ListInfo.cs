using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    /// <summary>
    /// Info lista ticket: # aperti, chiusi, etc...
    /// </summary>
    [Serializable, CLSCompliant(true)]
    public class DTO_ListInfo
    {
        public String DisplayUserName { get; set; }
        public int Draft { get; set; }
        public int Open { get; set; }
        public int WorkingOn { get; set; }
        public int Waiting { get; set; }
        public int SolvedClosed { get; set; }
        public int UnSolvedClosed { get; set; }

        public int BHDraft { get; set; }
        public int BHOpen { get; set; }
        public int BHWorkingOn { get; set; }
        public int BHWaiting { get; set; }
        public int BHSolvedClosed { get; set; }
        public int BHUnSolvedClosed { get; set; }

        public DTO_ListInfo()
        {
            DisplayUserName = "";

            Draft = 0;
            Open = 0;
            WorkingOn = 0;
            Waiting = 0;
            SolvedClosed = 0;
            UnSolvedClosed = 0;

            BHDraft = 0;
            BHOpen = 0;
            BHWorkingOn = 0;
            BHWaiting = 0;
            BHSolvedClosed = 0;
            BHUnSolvedClosed = 0;
        }
    }
}
