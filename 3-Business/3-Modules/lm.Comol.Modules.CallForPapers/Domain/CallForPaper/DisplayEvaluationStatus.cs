using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public enum DisplayEvaluationStatus
    {
        None = 0,
        ToEvaluate = 1,
        Evaluated = 2,
        Any = 3
    }
}