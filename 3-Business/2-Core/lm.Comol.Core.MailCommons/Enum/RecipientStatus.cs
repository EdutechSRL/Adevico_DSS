using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Comol.Core.MailCommons.Domain
{
    [DataContract(Name = "RecipientStatus")]
    public enum RecipientStatus
    {
        [EnumMember]
        Available = 1,
        [EnumMember]
        Blocked = 2,
        [EnumMember]
        Waiting = 4,
        [EnumMember]
        All = 7
    }
}
