using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    public enum SubmissionStatus
    {
        none = 0,
        draft = 1,
        submitted = 2,
        accepted = 3,
        rejected = 4,
        waitingValuation = 5,
        valuating = 6,
        valuated = 7,
        deleted = 8,
        tosubmit = 9,
        waitforsignature = 20
    }
}
