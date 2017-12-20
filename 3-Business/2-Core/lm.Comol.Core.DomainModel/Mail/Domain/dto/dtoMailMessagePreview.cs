using lm.Comol.Core.MailCommons.Domain.Configurations;
using lm.Comol.Core.MailCommons.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Mail
{
    [Serializable]
    public class dtoMailMessagePreview
    {
        private Int32 IdUserLanguage { get; set; }
        private Int32 IdDefaultLanguage { get; set; }
        public virtual String Subject {
            get
            {
                return (Settings.PrefixType ==  MailCommons.Domain.SubjectPrefixType.None) ? Message.UserSubject : SmtpConfig.GetSubjectPrefix(IdUserLanguage,IdDefaultLanguage) + Message.UserSubject;
            }
        }
        public virtual String Body
        {
            get
            {
                return Message.Body;
            }
        }
        public virtual System.Net.Mail.MailAddress Sender{
            get
            {
                return (SmtpConfig.RelayAllowed) ? ((Settings.SenderType == MailCommons.Domain.SenderUserType.System || Message.FromUser == null) ? SmtpConfig.GetSystemSender() : Message.FromUser) : SmtpConfig.GetSystemSender();
            }
        }
        public virtual System.Net.Mail.MailAddress From{
            get
            {
                return (Settings.SenderType == MailCommons.Domain.SenderUserType.System) ? SmtpConfig.GetSystemSender() : Message.FromUser;
            }
        }
        public virtual Boolean SystemForUser
        {
            get
            {
                return  From != null && !From.Equals(Sender);
            }
        }
        public virtual Boolean IsEmpty { get; set; }
        private dtoMailMessage Message { get; set; }
        public virtual lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings Settings { get; set; }
        private SmtpServiceConfig SmtpConfig { get; set; }

        public dtoMailMessagePreview(Int32 idUserLanguage, lm.Comol.Core.DomainModel.Language dLanguage) {
            IsEmpty = true;
            IdUserLanguage =idUserLanguage;
            IdUserLanguage = (dLanguage == null) ? 0 : dLanguage.Id;
        }
        public dtoMailMessagePreview(Int32 idUserLanguage, lm.Comol.Core.DomainModel.Language dLanguage, dtoMailMessage message, MessageSettings settings, SmtpServiceConfig smtpConfig)
        {
            IdUserLanguage = idUserLanguage;
            IdUserLanguage = (dLanguage == null) ? 0 : dLanguage.Id;
            Message = message;
            Settings = settings;
            SmtpConfig = smtpConfig;
            IsEmpty = false;
        }
    }
}