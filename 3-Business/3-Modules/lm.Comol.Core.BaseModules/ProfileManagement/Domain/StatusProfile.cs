using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public enum StatusProfile
    {
        AllStatus = -1,
        None =0,
        Active = 1,
        Disabled = 2,
        Waiting = 3,
    }
}
