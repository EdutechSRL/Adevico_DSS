using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public enum SearchAgencyBy
    {
        All = -1,
        None = 0,
        Contains = 1,
        Name = 2,
        TaxCode = 3,
        NationalCode = 4,
        ExternalCode = 5
    }
}
