using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Comol.Core.Mail
{
    [Serializable, DataContract]
    public class SmtpConfig
    {
        [DataMember]
        public virtual String Host {get;set;}
        [DataMember]
        public virtual Int32 Port { get; set; }
        [DataMember]
        public virtual Int32 MaxRecipients { get; set; }
        [DataMember]
        public virtual SmtpAuthentication Authentication { get; set; }
        [DataMember]
        public virtual SystemMailConfig System{ get; set; }
        [DataMember]
        public virtual Boolean RelayAllowed { get; set; }
        [DataMember]
        public virtual Boolean DefaultHTMLmode { get; set; }
        public SmtpConfig() {
            RelayAllowed = false;
            Port = 25;
            Authentication = new SmtpAuthentication();
            System = new SystemMailConfig();
        }
    }

    [Serializable, DataContract]
    public class SystemMailConfig
    {
        [DataMember]
        public virtual MailAddress Sender { get; set; }
        [DataMember]
        public virtual List<SenderContextConfig> LanguagesContext { get; set; }

        public SystemMailConfig()
        {
            LanguagesContext = new List<SenderContextConfig>();
        }

        public SenderContextConfig GetSenderContext(Int32 idUserLanguage, Int32 idDefaultLanguage) {
            SenderContextConfig context = (LanguagesContext == null) ? new SenderContextConfig() : (LanguagesContext.Where(t => t.IdLanguage == idUserLanguage).Any()) ? LanguagesContext.Where(t => t.IdLanguage == idUserLanguage).FirstOrDefault() : LanguagesContext.Where(t => t.IdLanguage == idDefaultLanguage).FirstOrDefault();
            if (context == null && LanguagesContext.Any())
                context = LanguagesContext.FirstOrDefault();
            return context;
        }
        public String SubjectPrefix(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            return GetSenderContext(idUserLanguage, idDefaultLanguage).SubjectPrefix;
        }
        public String SubjectPrefix(Int32 idUserLanguage, lm.Comol.Core.DomainModel.Language dLanguage)
        {
            return SubjectPrefix(idUserLanguage, (dLanguage == null) ? 0 : dLanguage.Id);
        }
        public String SubjectForSenderCopy(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            return GetSenderContext(idUserLanguage, idDefaultLanguage).SubjectForSenderCopy;
        }
        public String SubjectForSenderCopy(Int32 idUserLanguage, lm.Comol.Core.DomainModel.Language dLanguage)
        {
            return SubjectForSenderCopy(idUserLanguage, (dLanguage == null) ? 0 : dLanguage.Id);
        }
        public String Signature(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            return GetSenderContext(idUserLanguage, idDefaultLanguage).Signature;
        }
        public String Signature(Int32 idUserLanguage, lm.Comol.Core.DomainModel.Language dLanguage)
        {
            return Signature(idUserLanguage, (dLanguage==null) ? 0 :dLanguage.Id);
        }
        public String NoReplySignature(Int32 idUserLanguage, lm.Comol.Core.DomainModel.Language dLanguage)
        {
            return NoReplySignature(idUserLanguage, (dLanguage == null) ? 0 : dLanguage.Id );
        }
        public String NoReplySignature(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            return GetSenderContext(idUserLanguage, idDefaultLanguage).NoReplySignature;
        }
    }

    [Serializable,DataContract]
    public class SenderContextConfig
    {
        [DataMember]
        public virtual Int32 IdLanguage { get; set; }
        [DataMember]
        public virtual String CodeLanguage { get; set; }
        [DataMember]
        public virtual Boolean IsDefault { get; set; }
        [DataMember]
        public virtual String SubjectPrefix { get; set; }
        [DataMember]
        public virtual String SubjectForSenderCopy { get; set; }
        [DataMember]
        public virtual String Signature { get; set; }
        [DataMember]
        public virtual String NoReplySignature { get; set; }
        public SenderContextConfig()
        {
            SubjectPrefix = "";
            SubjectForSenderCopy = "Copy of";
            Signature = "";
            NoReplySignature = "";
        }
    }
}