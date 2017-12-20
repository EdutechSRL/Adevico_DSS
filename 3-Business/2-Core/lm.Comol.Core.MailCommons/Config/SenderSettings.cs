using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Comol.Core.MailCommons.Domain.Configurations
{
    [Serializable, DataContract]
    public class SenderSettings
    {
        [DataMember]
        public virtual Int32 IdLanguage { get; set; }
        [DataMember]
        public virtual String CodeLanguage { get; set; }
        [DataMember]
        public virtual Boolean IsDefault { get; set; }
        [DataMember]
        public virtual String SubjectPrefix { get; set; }
        [DataMember]
        public virtual String SubjectForSenderCopy { get; set; }
        [DataMember]
        public virtual String Signature { get; set; }
        [DataMember]
        public virtual String NoReplySignature { get; set; }
        [DataMember]
        public virtual Boolean IsMulti { get { return IdLanguage == 0 && CodeLanguage == "multi"; } }
        public SenderSettings()
        {
            SubjectPrefix = "";
            SubjectForSenderCopy = "Copy of";
            Signature = "";
            NoReplySignature = "";
        }
    }
}
