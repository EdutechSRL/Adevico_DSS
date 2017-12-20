using System.Net.Mail;
using System;
namespace lm.Comol.Core.Mail
{
    [Serializable]
    public class dtoRecipients
    {
        public MailAddress Mail;
        public lm.Comol.Core.MailCommons.Domain.RecipientType Type;
    }
}
