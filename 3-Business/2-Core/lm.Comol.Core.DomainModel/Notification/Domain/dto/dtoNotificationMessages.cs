using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using lm.Comol.Core.DomainModel;
using System.Runtime.Serialization;

namespace lm.Comol.Core.Notification.Domain
{
    [Serializable, DataContract]
    public class dtoNotificationMessages
    {
        [DataMember]
        public virtual lm.Comol.Core.Notification.Domain.NotificationChannel Channel { get; set; }
        [DataMember]
        public virtual List<lm.Comol.Core.DomainModel.Languages.dtoObjectTranslation> Translations { get; set; }
        [DataMember]
        public virtual lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings MailSettings { get; set; }

        public dtoNotificationMessages()
        {
            Translations = new List<DomainModel.Languages.dtoObjectTranslation>();
        }
        public dtoNotificationMessages(lm.Comol.Core.Notification.Domain.NotificationChannel channel)
        {
            Channel = channel;
        }

        public virtual Boolean IsValid()
        {
            return Translations != null && Translations.Any() && ((Channel != lm.Comol.Core.Notification.Domain.NotificationChannel.None && Channel != lm.Comol.Core.Notification.Domain.NotificationChannel.Mail) || (MailSettings != null && Channel == lm.Comol.Core.Notification.Domain.NotificationChannel.Mail));
        }
    }
}