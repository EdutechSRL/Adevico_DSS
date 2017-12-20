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

    public class dtoModuleNotificationMessage : dtoBaseNotificationMessage
    {
        [DataMember]
        public virtual Int32 IdLanguage { get; set; }
        [DataMember]
        public virtual String LanguageCode { get; set; }
      
        /// <summary>
        /// Only for mail notification to set "message ID"
        /// </summary>
        [DataMember]
        public virtual System.Guid UniqueIdentifier { get; set; }
        /// <summary>
        /// Only for mail notification to set "reference message ID"
        /// </summary>
        [DataMember]
        public virtual System.Guid FatherUniqueIdentifier { get; set; }
        [DataMember]
        public virtual NotificationChannel Channel { get; set; }
        [DataMember]
        public virtual Boolean Save { get; set; }
        [DataMember]
        public virtual String AttachmentPath { get; set; }
        [DataMember]
        public virtual String AttachmentSavedPath { get; set; }
        [DataMember]
        public virtual Int32 IdCommunity { get; set; }
        [DataMember]
        public virtual ModuleObject ObjectOwner { get; set; }
        [DataMember]
        public virtual List<lm.Comol.Core.MailCommons.Domain.Messages.Recipient> Recipients { get; set; }
        [DataMember]
        public List<String> Attachments { get; set; }
        public dtoModuleNotificationMessage() {
            Recipients = new List<MailCommons.Domain.Messages.Recipient>();
            Attachments = new List<string>();
        }
        public dtoModuleNotificationMessage(NotificationChannel channel)
        {
            Channel = channel;
            Recipients = new List<MailCommons.Domain.Messages.Recipient>();
            Attachments = new List<string>();
        }

        public virtual Boolean IsValid()
        {
            return Translation != null && ((Channel != NotificationChannel.None && Channel != lm.Comol.Core.Notification.Domain.NotificationChannel.Mail) || (MailSettings != null && Channel == lm.Comol.Core.Notification.Domain.NotificationChannel.Mail));
        }
    }

}