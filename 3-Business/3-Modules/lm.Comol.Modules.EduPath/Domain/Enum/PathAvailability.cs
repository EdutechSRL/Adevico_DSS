using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable,Flags]
    public enum PathAvailability : int
    {
        None = 0,
        Available = 1,
        Blocked = 2,
        WithOtherUserStatistics = 4,
        WithMyStatistics = 8,
        Deleted = 16,
        UnknownItem = 32
    }
}