using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Domain
{
    [Serializable]
    public enum Column:int 
    {
        displayorder = 0,
        selectitem = 1,
        name = 2,
        indicators = 3,
        date = 4,
        stats = 5,
        actions = 6,
    }
}
