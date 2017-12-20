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

    public class GroupSettings
    {

        [DataMember]
        public virtual TemplateSettings Template { get; set; }

        [DataMember]
        public virtual lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings Mail { get; set; }
        [DataMember]
        public virtual Boolean Save { get; set; }
        [DataMember]
        public virtual String AttachmentPath { get; set; }
        [DataMember]
        public virtual String AttachmentSavedPath { get; set; }
        [DataMember]
        public List<String> Attachments { get; set; }
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
        public GroupSettings() {
            Attachments = new List<string>();
            Template = new TemplateSettings();
            AttachmentPath = "";
            AttachmentSavedPath = "";
        }
        public GroupSettings(NotificationChannel channel)
        {
            Attachments = new List<string>();
            Template = new TemplateSettings();
            AttachmentPath = "";
            AttachmentSavedPath = "";
        }

    }
}