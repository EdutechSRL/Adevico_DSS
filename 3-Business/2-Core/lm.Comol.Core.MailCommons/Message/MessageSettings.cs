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
    public class MessageSettings : IEquatable<MessageSettings>
    {
        [DataMember]
        public SenderUserType SenderType { get; set; }
        [DataMember]
        public SubjectPrefixType PrefixType { get; set; }
        [DataMember]
        public Boolean IsBodyHtml { get; set; }
        [DataMember]
        public Boolean NotifyToSender { get; set; }
        [DataMember]
        public Boolean CopyToSender { get; set; }
        [DataMember]
        public Signature Signature { get; set; }
        [DataMember]
        public virtual long IdSkin { get; set; }
       
     

        public MessageSettings()
        {
            PrefixType = SubjectPrefixType.SystemConfiguration;
            SenderType = SenderUserType.LoggedUser;
        }

        public virtual MessageSettings Copy()
        {
            MessageSettings settings = new MessageSettings();
            settings.SenderType = SenderType;
            settings.PrefixType = PrefixType;
            settings.IsBodyHtml = IsBodyHtml;
            settings.Signature = Signature;
            settings.NotifyToSender = NotifyToSender;
            settings.CopyToSender = CopyToSender;
            settings.IdSkin = IdSkin;
            return settings;
        }

        public bool Equals(MessageSettings other)
        {
            return other != null && SenderType == other.SenderType && PrefixType == other.PrefixType && IsBodyHtml == other.IsBodyHtml && Signature == other.Signature && NotifyToSender == other.NotifyToSender && CopyToSender == other.CopyToSender && IdSkin == other.IdSkin;
        }
    }
}