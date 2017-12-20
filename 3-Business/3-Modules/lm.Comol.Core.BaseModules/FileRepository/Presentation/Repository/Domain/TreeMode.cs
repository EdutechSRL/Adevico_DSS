using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain
{
    [Serializable]
    public enum TreeMode
    {
        noselect = 0,
        singleselect = 1,
        multiselect = 2,
        cascadeselect = 3,
        tristateselect = 4
    }
}
