using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Mail
{
    [Serializable]
    public enum SenderType
    {
        LoggedUser = 0,
        System = 1,
        OtherUser = 2
    }
}
