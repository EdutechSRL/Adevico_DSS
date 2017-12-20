using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public enum CallStatusForSubmitters
    {
        None = 0,
        SubmissionOpened = 1,
        Submitted = 2,
        SubmissionClosed = 3,
        Draft = 4,
        ToEvaluate = 5,
        Evaluated = 6,
        Revisions = 7,
        SubmissionsLimitReached = 8
    }
}
