using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public enum UserByMessagesOrder
    {
        None = 0,
        ByStatus = 1,
        ByType = 2,
        ByUser = 3,
        ByMessageNumber = 4,
        ByAgency = 5
    }
}