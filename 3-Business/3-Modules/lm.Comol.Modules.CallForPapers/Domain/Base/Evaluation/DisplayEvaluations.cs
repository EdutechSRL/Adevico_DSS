using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public enum DisplayEvaluations
    { 
        None = 0,
        Single = 1,
        ForUser = 2,
        ForSubmission = 3
    }
}