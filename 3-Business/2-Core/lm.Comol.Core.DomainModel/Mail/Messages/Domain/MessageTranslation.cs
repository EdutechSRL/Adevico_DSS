using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Mail.Messages
{
    [Serializable]
    public class MessageTranslation
    {
        public virtual long Id { get; set; } 
        public virtual Int32 IdLanguage { get; set; }
        public virtual String LanguageCode { get; set; }

        public virtual IList<MailAttachment> Attachments { get { return (Message == null || (Message != null && (Message.Attachments == null || (Message.Attachments != null && Message.Attachments.Any())))) ? new List<MailAttachment>() : Message.Attachments; } }
        public virtual IList<MailRecipient> Recipients { get; set; }
        public virtual MailMessage Message { get; set; }
        public virtual String Subject { get; set; }
        public virtual String Body { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual litePerson CreatedBy { get; set; }
        public virtual Boolean SentBySystem { get; set; }
        public virtual Ownership Ownership { get; set; }
      
        public virtual BaseStatusDeleted Deleted { get; set; }
        public MessageTranslation()
        {
            Recipients = new List<MailRecipient>();
            Ownership = new Ownership();
        }
        public MessageTranslation(Ownership ownership)
        {
            Recipients = new List<MailRecipient>();
            Ownership = ownership;
        }
    }
}
