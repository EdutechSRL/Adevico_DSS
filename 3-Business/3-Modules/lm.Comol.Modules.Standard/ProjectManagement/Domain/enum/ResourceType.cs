using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum ResourceType
    {
        None = 0,
        Internal = 1,
        External =2,
        Removed =3
    }
}