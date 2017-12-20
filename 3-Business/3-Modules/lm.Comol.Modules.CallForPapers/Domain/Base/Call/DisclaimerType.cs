using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public enum DisclaimerType
    {
        None = 0,
        Standard = 1,
        CustomSingleOption = 2,
        CustomMultiOptions = 3,
        CustomDisplayOnly = 4
    }
}