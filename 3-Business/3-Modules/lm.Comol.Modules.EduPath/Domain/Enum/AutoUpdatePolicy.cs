using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public enum AutoUpdatePolicy
    {
        None = 0,
        OnlyIfBetter = 1,
      //  OnlyIfWorst = 2,
        Always = 4
    }
}