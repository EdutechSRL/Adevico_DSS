using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Mail.Messages
{
    [Serializable()]
    public enum MessageOrder
    {
        None = 0,
        ByDate = 1,
        ByRecipientsNumber = 2,
        ByName = 3
    }
}