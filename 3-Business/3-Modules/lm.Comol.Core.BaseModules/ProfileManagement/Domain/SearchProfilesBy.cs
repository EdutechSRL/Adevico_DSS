using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public enum SearchProfilesBy
    {
        All = -1,
        None = 0,
        Contains = 1,
        Login = 2,
        Mail = 3,
        Name = 5,
        Surname = 6,
        TaxCode = 7
    }
}
