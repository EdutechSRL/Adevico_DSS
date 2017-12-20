using System;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Comol.Core.MailCommons.Domain
{
    [DataContract(Name = "SubjectPrefixType"), Serializable]
    public enum SubjectPrefixType
    {
        [EnumMember]
        SystemConfiguration = 0,
        [EnumMember]
        None = 1
    }
}