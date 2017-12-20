using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Comol.Core.Notification.Domain
{
    [Serializable, Flags, DataContract(Name = "NotificationChannel")]
    public enum NotificationChannel
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        Mail = 1,
        [EnumMember]
        CommunityNews = 2,
        [EnumMember]
        PersonalNews = 4,
        [EnumMember]
        Memo = 8
    }
}