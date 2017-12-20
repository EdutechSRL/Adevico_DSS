using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public enum SubmissionsOrder
    {
        None = 0,
        ByStatus = 1,
        ByType = 2,
        ByUser = 3,
        ByDate = 4,
        BySubmittedOn = 5,
        ByEvaluationStatus =6,
        ByEvaluationPoints = 7,
        ByEvaluationIndex = 8
    }
}