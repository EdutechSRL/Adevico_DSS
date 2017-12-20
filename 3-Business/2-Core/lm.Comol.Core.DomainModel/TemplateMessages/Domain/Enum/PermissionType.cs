using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public enum PermissionType
    {
        Base = 0,
        Owner = 1,
        Community = 2,
        Role = 3,
        ProfileType = 4,
        Person = 5,
        Module = 6,
        Portal = 7
    }
}
