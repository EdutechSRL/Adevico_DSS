using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum ConfirmActions
    {
        None = 0,
        Apply = 1,
        Remove = 2,
        Hold = 3
    }
}