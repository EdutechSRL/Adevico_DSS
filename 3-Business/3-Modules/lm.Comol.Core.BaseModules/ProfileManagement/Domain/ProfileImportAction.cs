using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public enum ProfileImportAction
    {
        None = 0,
        Create = 1,
        AddToOtherOrganizations = 2,
        AddToCommunities = 4,
        SendMail=8
    }
}
