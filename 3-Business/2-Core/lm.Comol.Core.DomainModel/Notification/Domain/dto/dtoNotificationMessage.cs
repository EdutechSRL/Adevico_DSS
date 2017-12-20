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

    public class dtoNotificationMessage : dtoBaseNotificationMessage
    {
        [DataMember]
        public virtual NotificationChannel Channel { get; set; }
     
        public dtoNotificationMessage() { }
        public dtoNotificationMessage(NotificationChannel channel)
        {
            Channel = channel;
        }

        public virtual Boolean IsValid()
        {
            return Translation != null && ((Channel != NotificationChannel.None && Channel != lm.Comol.Core.Notification.Domain.NotificationChannel.Mail) || (MailSettings != null && Channel == lm.Comol.Core.Notification.Domain.NotificationChannel.Mail));
        }
    }
}