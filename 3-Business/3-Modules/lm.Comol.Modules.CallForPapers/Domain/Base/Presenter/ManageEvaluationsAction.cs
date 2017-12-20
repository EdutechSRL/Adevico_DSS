using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public enum ManageEvaluationsAction {
        None = 0,
        CloseAll = 1,
        OpenAll = 2
    }

    [Serializable]
    public enum ManageEvaluationsDisplay
    {
        None = 0,
        BySubmission = 1,
        ByEvaluator = 2
    }
}