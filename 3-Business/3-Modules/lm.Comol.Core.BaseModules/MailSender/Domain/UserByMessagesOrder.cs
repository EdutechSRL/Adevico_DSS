using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.MailSender
{
    [Serializable()]
    public enum UserByMessagesOrder
    {
        None = 0,
        ByRole = 1,
        ByProfileType = 2,
        ByUser = 3,
        ByMessageNumber = 4,
        ByAgency = 5,
        ByInternal = 6
    }
}