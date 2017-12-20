using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoCallForPaperAvailable :dtoBase 
    {
        public virtual dtoCallForPaper CallForPaper { get; set; }
        public virtual dtoSubmission Submission { get; set; }
        public virtual CallStatusForSubmitters Status { get; set; }

        public dtoCallForPaperAvailable()
            : base()
        { 
        }
        public dtoCallForPaperAvailable(long id)
            : base(id)
        {
        }

        public dtoCallForPaperAvailable(long id, CallStatusForSubmitters status, dtoCallForPaper callForPaper)
            : base(id)
        {
            Status = status;
            CallForPaper = callForPaper;
            Submission = new dtoSubmission(0);
        }

        public dtoCallForPaperAvailable(long id, CallStatusForSubmitters status, dtoCallForPaper callForPaper, dtoSubmission submission)
            : base(id)
        {
            Status = status;
            CallForPaper = callForPaper;
            if(submission==null)
                Submission = new dtoSubmission(0);
            else
                Submission = submission;
        }

    }
}