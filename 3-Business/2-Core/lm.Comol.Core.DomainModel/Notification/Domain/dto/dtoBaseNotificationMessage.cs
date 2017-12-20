using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using lm.Comol.Core.DomainModel;
using System.Runtime.Serialization;

namespace lm.Comol.Core.Notification.Domain
{
    [Serializable,DataContract]
    [KnownType(typeof(dtoNotificationMessage))]
    [KnownType(typeof(dtoModuleNotificationMessage))]
    public class dtoBaseNotificationMessage
    {
        [DataMember]
        public virtual long IdTemplate { get; set; }
        [DataMember]
        public virtual long IdVersion { get; set; }
        [DataMember]
        public virtual lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation Translation { get; set; }
        [DataMember]
        public virtual lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings MailSettings { get; set; }

        public dtoBaseNotificationMessage() { }
    }
}