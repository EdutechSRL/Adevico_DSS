using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace lm.Comol.Core.Notification.Domain
{
    [Serializable, DataContract(Name = "NotificationMode")]
    public enum NotificationMode
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        Automatic = 1,
        [EnumMember]
        Scheduling = 2,
        [EnumMember]
        Manual = 3
    }
}