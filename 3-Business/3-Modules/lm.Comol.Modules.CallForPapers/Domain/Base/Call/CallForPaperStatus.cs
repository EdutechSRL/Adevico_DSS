using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public enum CallForPaperStatus
    {
         None = 0,
         Draft = 1,
         Published = 2,
         SubmissionOpened = 3,
         SubmissionClosed = 4,
         SubmissionsLimitReached = 5
    }
}