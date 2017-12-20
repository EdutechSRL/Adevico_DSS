using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.MailSender
{
    [Serializable]
    public enum PreviewMode
    {
        None = 0,
        MailSent = 1,
        MailToSend = 2,
        MailReceived = 3,
        TemplateDisplay = 4
    }
}
