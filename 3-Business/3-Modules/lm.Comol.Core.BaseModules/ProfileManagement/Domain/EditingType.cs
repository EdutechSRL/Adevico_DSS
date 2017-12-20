using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public enum EditingType
    {
        None = 0,
        User = 1,
        FromManagement = 2,
        FromWebService = 3,
        FromTool =  4,
    }
}
