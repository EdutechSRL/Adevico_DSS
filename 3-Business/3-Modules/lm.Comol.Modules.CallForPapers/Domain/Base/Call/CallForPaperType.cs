using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public enum CallForPaperType
    {
        None = 0,
        CallForBids = 1,
        Conference = 2,
        RequestForMembership = 3
    }
}
