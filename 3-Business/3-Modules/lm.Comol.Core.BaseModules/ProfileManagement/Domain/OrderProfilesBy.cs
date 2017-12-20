using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public enum OrderProfilesBy
    {
        None =0,
        SurnameAndName = 1,
        AuthenticationProvider = 2,
        Status = 3,
        CreationDate = 4
    }
}