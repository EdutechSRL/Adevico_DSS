using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain
{
    [Serializable]
    public enum PlayerErrors
    {
        InvalidType = 0,
        NoPermissionToPlay = 1,
        PlayerUnavailable = 2,
        InvalidSettings = 3,
        InvalidTransfer = 4,
        UnknownItem = 5
    }
}