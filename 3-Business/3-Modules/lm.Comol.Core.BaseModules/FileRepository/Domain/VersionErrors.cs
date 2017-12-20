using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Domain
{
    [Serializable]
    public enum VersionErrors:int 
    {
        none = 0,
        unavailableItem = 1,
        nopermission = 2,
        unabletoadd = 3,
    }
}
