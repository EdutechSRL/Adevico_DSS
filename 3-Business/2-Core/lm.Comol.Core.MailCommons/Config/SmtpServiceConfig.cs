using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Comol.Core.MailCommons.Domain.Configurations
{
    [Serializable, DataContract]
    public class SmtpServiceConfig
    {
        [DataMember]
        public virtual String Host { get; set; }
        [DataMember]
        public virtual Int32 Port { get; set; }
        [DataMember]
        public virtual Int32 MaxRecipients { get; set; }
        [DataMember]
        public virtual SmtpAuthenticationConfig Authentication { get; set; }
        //[DataMember]
        //public virtual SystemMailConfig System { get; set; }
        [DataMember]
        public virtual Boolean RelayAllowed { get; set; }
        [DataMember]
        public virtual Boolean DefaultHTMLmode { get; set; }
        [DataMember]
        public virtual lm.Comol.Core.MailCommons.Domain.Messages.UserMailAddress SystemSender { get; set; }
        [DataMember]
        public virtual lm.Comol.Core.MailCommons.Domain.Messages.UserMailAddress ErrorSender { get; set; }
        [DataMember]
        public virtual Boolean SendMailError { get; set; }
        [DataMember]
        public virtual List<SenderSettings> DefaultSettings { get; set; }
        
        public SmtpServiceConfig()
        {
            RelayAllowed = false;
            Port = 25;
            Authentication = new SmtpAuthenticationConfig();
            DefaultSettings = new List<SenderSettings>();
            ErrorSender = new Messages.UserMailAddress();
            SystemSender = new Messages.UserMailAddress();
        }


        public System.Net.Mail.MailAddress GetSystemSender()
        {
            return ((SystemSender==null || String.IsNullOrEmpty(SystemSender.Address)) ? null : (String.IsNullOrEmpty(SystemSender.DisplayName) ? new System.Net.Mail.MailAddress(SystemSender.Address) :   new System.Net.Mail.MailAddress(SystemSender.Address, SystemSender.DisplayName) ));
        }

        private SenderSettings GetSenderSettings(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            SenderSettings context = (DefaultSettings == null) ? new SenderSettings() : (DefaultSettings.Where(t => t.IdLanguage == idUserLanguage).Any()) ? DefaultSettings.Where(t => t.IdLanguage == idUserLanguage).FirstOrDefault() : DefaultSettings.Where(t => t.IdLanguage == idDefaultLanguage).FirstOrDefault();
            if (context == null && DefaultSettings.Any())
            {
                if (DefaultSettings.Where(s => s.IsDefault).Any())
                    context = DefaultSettings.Where(s => s.IsDefault).FirstOrDefault();
                else
                    context = DefaultSettings.FirstOrDefault();
            }
            return context;
        }
        public String GetSubjectPrefix(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            return GetSenderSettings(idUserLanguage, idDefaultLanguage).SubjectPrefix;
        }
        public String GetSubjectForSenderCopy(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            return GetSenderSettings(idUserLanguage, idDefaultLanguage).SubjectForSenderCopy;
        }
        public String GetSignature(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            return GetSenderSettings(idUserLanguage, idDefaultLanguage).Signature;
        }
        public String GetNoReplySignature(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            return GetSenderSettings(idUserLanguage, idDefaultLanguage).NoReplySignature;
        }
    }
}
