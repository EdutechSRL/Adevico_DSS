using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public enum SourceType
    {
        None = 0,
        FileCSV = 1,
        RequestForMembership = 2,
        CallForPapers = 3,
        FileExcel = 4
    }
}