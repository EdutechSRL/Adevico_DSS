using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace lm.Comol.Modules.Standard.Skin.Domain
{
    [Serializable,Flags]
    public enum LoadItemsBy
    {
        None = 0,
        Module = 1,
        Creator = 2,
        Community =4,
        Object = 8
    }
}