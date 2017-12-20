using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public enum ImportedAgencyColumn
    {
        skip = 1,
        name = 2,
        externalCode = 4,
        taxCode = 8,
        nationalCode = 16
    }
}