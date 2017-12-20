using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Mail.Messages
{
    [Serializable]
    public class MailMessage
    {
        public virtual long Id { get; set; }
        public Guid UniqueIdentifier { get; set; }
        public Guid FatherUniqueIdentifier { get; set; }

        public virtual IList<MailAttachment> Attachments { get; set; }
        public virtual IList<MessageTranslation> Translations { get; set; }
        public virtual MailTemplate Template { get; set; }
        public virtual lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings MailSettings { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual litePerson CreatedBy { get; set; }
        public virtual Boolean SentBySystem { get; set; }
        public virtual Ownership Ownership { get; set; }
      
        public virtual BaseStatusDeleted Deleted { get; set; }
        public MailMessage() {
            Attachments = new List<MailAttachment>();
            Translations = new List<MessageTranslation>();
        }
        public MailMessage(lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings settings, Ownership ownership)
        {
            Attachments = new List<MailAttachment>();
            Translations = new List<MessageTranslation>();
            Ownership = ownership;
            MailSettings = settings;
        }
    }
}
