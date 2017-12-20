using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    [Flags, Serializable ]
    public enum ContentView
    {
        viewAll =0,
        hideHeader = 1,
        hideFooter = 2 ,
        hideModuleContent =4,
        hideMenu =8
    }
}
