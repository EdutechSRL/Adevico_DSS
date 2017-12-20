using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Comol.Core.MailCommons.Domain
{
    [Serializable, DataContract]
    public enum Signature
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        FromConfigurationSettings = 1,
        [EnumMember]
        FromTemplate = 2,
        [EnumMember]
        FromSkin = 3,
        [EnumMember]
        FromField = 4,
        [EnumMember]
        FromNoReplySettings = 5,
    }
}