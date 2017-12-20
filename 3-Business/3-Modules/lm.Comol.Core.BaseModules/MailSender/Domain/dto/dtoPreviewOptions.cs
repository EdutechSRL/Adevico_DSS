using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Mail;

namespace lm.Comol.Core.BaseModules.MailSender.Domain.dto
{
    [Serializable]
    public class dtoPreviewOptions
    {
        public lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings Settings { get; set; }
        public DateTime? SentOn  {get;set;}
        public long IdTemplate  {get;set;}
        public String TemplateName  {get;set;}
    }
}