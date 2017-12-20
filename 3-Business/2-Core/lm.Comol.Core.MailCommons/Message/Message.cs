using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace lm.Comol.Core.MailCommons.Domain.Messages
{
    [Serializable, DataContract]
    public class Message
    {
        [DataMember]
        public Int32 IdLanguage { get; set; }
        [DataMember]
        public Guid UniqueIdentifier { get; set; }
        [DataMember]
        public Guid FatherUniqueIdentifier { get; set; }
        [DataMember]
        public String Subject { get; set; }
        [DataMember]
        public String Body { get; set; }
        [DataMember]
        public String PlainBody { get; set; }
       
        [DataMember]
        public List<String> Attachments { get; set; }
        [DataMember]
        public List<Recipient> Recipients { get; set; }
        [DataMember]
        public MessageSettings Settings { get; set; }
        public Message()
        {
            Attachments = new List<String>();
            Recipients = new List<Recipient>();
            Settings = new MessageSettings() { PrefixType = SubjectPrefixType.SystemConfiguration, Signature = Signature.FromConfigurationSettings, SenderType = SenderUserType.LoggedUser };
        }
        public Message(Boolean copyToSender, Boolean notifyToSender, SenderUserType sType, SubjectPrefixType sPrefix = SubjectPrefixType.SystemConfiguration, Signature signature = Signature.FromConfigurationSettings)
        {
            Attachments = new List<String>();
            Recipients = new List<Recipient>();
            Settings = new MessageSettings() { PrefixType = sPrefix, Signature = signature, SenderType = sType, NotifyToSender = notifyToSender, CopyToSender = copyToSender };
        }
    }
}