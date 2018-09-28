using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [Serializable]
   
    public enum MenuBarType
    {
        None = 0,
        Portal = 1,
        GenericCommunity = 2,
        PortalAdministration = 3,
        ForCommunity = 4,
        ForCommunitiesTemplate = 5
    }
}
