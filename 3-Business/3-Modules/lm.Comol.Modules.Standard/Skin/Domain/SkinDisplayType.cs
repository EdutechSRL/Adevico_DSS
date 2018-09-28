using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain
{
    [Serializable,Flags ]
    public enum SkinDisplayType
    {
        Empty = 0,
        Portal = 1,
        Organization = 2,
        Community =4,
        Module = 8,
        NotAssociated = 16,
        CurrentCommunity = 32
    }
}
