using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Mail.Messages
{
    [Serializable]
    public class dtoMailTranslatedMessage
    {
        public virtual Int32 IdLanguage { get; set; }
        public virtual String CodeLanguage { get; set; }
        public virtual List<dtoBaseMessageRecipient> Recipients { get; set; }
        public virtual List<dtoBaseMessageRecipient> RemovedRecipients { get; set; }
        public virtual String Subject { get; set; }
        public virtual String Body { get; set; }
        public virtual Boolean Sent { get; set; }

        public dtoMailTranslatedMessage()
        {
            Recipients = new List<dtoBaseMessageRecipient>();
            RemovedRecipients = new List<dtoBaseMessageRecipient>();
        }
        public dtoMailTranslatedMessage(dtoBaseMessageRecipient recipient)
        {
            Recipients = new List<dtoBaseMessageRecipient>();
            RemovedRecipients = new List<dtoBaseMessageRecipient>();
            Recipients.Add(recipient);
        }
    }
}
