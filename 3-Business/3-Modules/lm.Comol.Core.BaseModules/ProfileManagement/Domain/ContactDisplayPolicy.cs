using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public enum ContactDisplayPolicy
    {
        None = 0,
        Mail = 1,
        IstantMessaging = 2,
        All = 4
    }
}