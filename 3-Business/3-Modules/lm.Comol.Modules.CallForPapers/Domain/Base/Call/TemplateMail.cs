using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class TemplateMail : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual String Name { get; set; }
        public virtual BaseForPaper Call { get; set; }
        public virtual String Subject { get; set; }
        public virtual String Body { get; set; }
        public virtual Int32 IdLanguage { get; set; }
        public virtual lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings MailSettings { get; set; }

        public TemplateMail()
        {
            Deleted = BaseStatusDeleted.None;
            MailSettings = new lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings();
        }
    }

    [Serializable()]
    public class SubmitterTemplateMail : TemplateMail
    {
        public virtual SubmitterType SubmitterType { get; set; }
        public SubmitterTemplateMail() :base()
        {
        }
    }
    
    [Serializable()]
    public class ManagerTemplateMail : TemplateMail
    {
        public virtual NotifyFields NotifyFields { get; set; }
        public virtual String NotifyTo { get; set; }
        public ManagerTemplateMail()
            : base()
        {
            NotifyFields = Domain.NotifyFields.Standard;
        }
    }

    [Serializable()]
    public enum NotifyFields { 
        None = 0,
        Standard = 1,
        Submitter = 2
    }
}