using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Mail
{
    [Serializable]
    public class dtoMailContent
    {

        public virtual String Subject { get; set; }
        public virtual String Body { get; set; }
        public virtual lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings Settings { get; set; }
    }
}
