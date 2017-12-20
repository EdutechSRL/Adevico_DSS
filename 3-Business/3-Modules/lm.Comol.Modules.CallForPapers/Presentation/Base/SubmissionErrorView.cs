using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public enum SubmissionErrorView
    {
        None = 0,
        SubmissionDeleted = 1,
        SubmissionsClosed = 2,
        GenericError = 3,
        SubmissionValueSaving=4,
        SubmissionFileSaving = 5,
        RequiredItems =6,
        SubmissionTimeExpired=7,
        InvalidFields = 8,
        UnavailableCall = 9,
        SubmissionEarlyTime = 10
    }
}
