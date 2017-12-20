using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Mail.Messages
{
    [Serializable]
    public class dtoMessageRecipient : dtoBaseMessageRecipient
    {
        public virtual long Id { get; set; }
        public dtoMessageRecipient()
        {
            Type = lm.Comol.Core.MailCommons.Domain.RecipientType.BCC;
        }
    }
}
