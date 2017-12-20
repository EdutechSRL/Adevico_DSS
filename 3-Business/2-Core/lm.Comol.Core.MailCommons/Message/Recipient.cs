using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Comol.Core.MailCommons.Domain.Messages
{
     [Serializable, DataContract]
    public class Recipient : UserMailAddress
    {
        [DataMember]
        public RecipientType Type { get; set; }
        [DataMember]
        public Int32 IdRole { get; set; }
        [DataMember]
        public Int32 IdPerson { get; set; }
        [DataMember]
        public long IdUserModule { get; set; }
         
        [DataMember]
        public Int32 IdCommunity { get; set; }
        [DataMember]
        public Int32 IdLanguage { get; set; }
        [DataMember]
        public String LanguageCode { get; set; }
        [DataMember]
        public RecipientStatus Status { get; set; }

        [DataMember]
        public virtual long IdModuleObject { get; set; }
        [DataMember]
        public virtual Int32 IdModuleType { get; set; }
         
        public Recipient()
        {
            Type = RecipientType.To;
            Status = RecipientStatus.Available;
        }


        public override String ToString()
        {
            if (!String.IsNullOrEmpty(Address))
                return String.IsNullOrEmpty(DisplayName) ? Address : DisplayName + "<" + Address + ">";
            else
                return String.IsNullOrEmpty(DisplayName) ? "" : DisplayName;
        }

    }
}
