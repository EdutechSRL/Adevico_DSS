using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.PersonalInfo
{
    [Serializable]
    public enum PolicyType
    {
        none = 0,
        agreeOnly = 1,
        agreeDisagree = 2,
        acceptOnly = 4,
        acceptRefuse = 8
    }
}
