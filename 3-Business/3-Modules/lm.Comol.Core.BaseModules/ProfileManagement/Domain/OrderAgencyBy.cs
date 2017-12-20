using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public enum OrderAgencyBy
    {
        None =0,
        Name = 1,
        TaxCode = 2,
        NationalCode = 3,
        ExternalCode = 4
    }
}