using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public enum PermissionsTranslation
    {
        UnknownUser = 0,
        CommunityPermission = 1,
        DenyedPermission = 2,
        AllowedPermission = 3,
        InheritedPermission = 4,
        NoPermission = 5,
        MoreInfoOnAdvancedDetails = 6
    }
}
