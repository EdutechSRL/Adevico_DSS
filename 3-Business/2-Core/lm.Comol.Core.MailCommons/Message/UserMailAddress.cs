using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Comol.Core.MailCommons.Domain.Messages
{
     [Serializable, DataContract]
     [KnownType(typeof(Recipient))]
    public class UserMailAddress
    {
        [DataMember]
        public String Address;
        [DataMember]
        public String DisplayName;

        public UserMailAddress()
        {
        }
    }
}
