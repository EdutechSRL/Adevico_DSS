using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace lm.Comol.Core.Notification.Domain
{
    [Serializable, DataContract(Name = "NotificationActionType")]
    public enum NotificationActionType
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        BySystem = 1,
        [EnumMember]
        ByTemplate = 2,
        [EnumMember]
        Ignore = 4
    }
}