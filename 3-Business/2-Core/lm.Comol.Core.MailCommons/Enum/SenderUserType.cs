using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Comol.Core.MailCommons.Domain
{
    [DataContract(Name = "SenderUserType"), Serializable]
    public enum SenderUserType
    {
        [EnumMember]
        LoggedUser = 0,
        [EnumMember]
        System = 1,
        [EnumMember]
        OtherUser = 2
    }
}