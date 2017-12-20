using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoTemplateMail:dtoBase 
    {

        public virtual long IdCallForPaper { get; set; }
        public virtual String Name { get; set; }
        public virtual String Subject { get; set; }
        public virtual String Body { get; set; }
        public virtual Int32 IdLanguage { get; set; }
        public virtual lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings MailSettings { get; set; }

        public dtoTemplateMail() : base() {
            MailSettings = new lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings();
        }
        public dtoTemplateMail(String subject, String body, Int32 idLanguage, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings settings, BaseStatusDeleted deleted)
            : base()
        {
            Deleted = deleted;
            Subject = subject;
            Body = body;
            IdLanguage = idLanguage;
            MailSettings = settings;
        }
    }

    [Serializable]
    public class dtoSubmitterTemplateMail : dtoTemplateMail
    {
        public List<long> SubmitterAssignments { get; set; }
        public dtoSubmitterTemplateMail()
            : base()
        {
            SubmitterAssignments = new List<long>();
        }
        public dtoSubmitterTemplateMail(long idSubmitterType)
            : this()
        {
            SubmitterAssignments.Add(idSubmitterType);
        }
        public dtoSubmitterTemplateMail(long idSubmitterType, String subject, String body, Int32 idLanguage, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings settings, BaseStatusDeleted deleted)
            : base(subject, body, idLanguage, settings,deleted)
        {
            SubmitterAssignments = new List<long>();
            SubmitterAssignments.Add(idSubmitterType);
        }
        public dtoSubmitterTemplateMail(List<long> submitters, String subject, String body, Int32 idLanguage, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings settings, BaseStatusDeleted deleted)
            : base(subject, body, idLanguage, settings, deleted)
        {
            SubmitterAssignments = submitters;
        }

        public void UpdateContent(dtoTemplateMail source) { 
            Subject = source.Subject;
            Body = source.Body;
            MailSettings = source.MailSettings;
        }
    }

    [Serializable]
    public class dtoManagerTemplateMail : dtoTemplateMail
    {
        public virtual NotifyFields NotifyFields { get; set; }
        public virtual String NotifyTo { get; set; }

        public dtoManagerTemplateMail()
            : base()
        {
        }
        public dtoManagerTemplateMail(NotifyFields fields, String notifyTo, String subject, String body, Int32 idLanguage, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings settings, BaseStatusDeleted deleted)
            : base(subject, body, idLanguage, settings, deleted)
        {
            NotifyFields = fields;
            NotifyTo = notifyTo;
        }
    }
}