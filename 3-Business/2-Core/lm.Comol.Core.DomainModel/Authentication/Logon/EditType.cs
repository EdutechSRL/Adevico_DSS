using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public enum EditType
    {
        none = 0,
        user = 1,
        admin = 2,
        reset = 4,
        retrieve = 8,
        oneTime = 16
    }
}
