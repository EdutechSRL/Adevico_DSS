using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public enum SubmissionFilterStatus
    {
        All = 0,
        OnlySubmitted = 1,
        WaitingSubmission = 2,
        VirtualDeletedSubmission = 4,
        WithRevisions = 8,
        Accepted = 16,
        Rejected = 32
    }
}