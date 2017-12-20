using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.MailCommons.Domain;

namespace lm.Comol.Core.Mail.Messages
{
    [Serializable]
    public class dtoModuleRecipientMessages : dtoBaseMessageRecipient
    {
        public virtual long Id { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public virtual Int32 MessageNumber { get; set; }
        public virtual long IdAgency { get; set; }
        public virtual String AgencyName { get; set; }
        public virtual String Name { get; set; }
        public virtual String Surname { get; set; }

        public virtual List<dtoUserMessageInfo> Messages { get; set; }
        public virtual Int32 LastIdLanguage { get { return (Messages == null || !Messages.Any()) ? 0 : Messages.OrderByDescending(m => m.Id).FirstOrDefault().IdLanguage; } }
        public virtual String LastLanguageCode { get { return (Messages == null || !Messages.Any()) ? "" : Messages.OrderByDescending(m => m.Id).FirstOrDefault().LanguageCode; } }

        public dtoModuleRecipientMessages()
        {
            Type = RecipientType.BCC;
            Messages = new List<dtoUserMessageInfo>();
        }
        public dtoModuleRecipientMessages(long id)
        {
            Id = id;
            Type = RecipientType.BCC;
            Messages = new List<dtoUserMessageInfo>();
        }
        public void UpdateAgencyInfo(long id, String name)
        {
            IdAgency = id;
            AgencyName = name;
        }
    }

    [Serializable]
    public class dtoUserMessageInfo
    {
        public virtual long Id { get; set; }
        public virtual Int32 IdLanguage { get; set; }
        public virtual String LanguageCode { get; set; }
        public virtual long IdModuleObject { get; set; }
        public virtual Int32 IdModuleType { get; set; }
        public virtual DateTime SentOn { get; set; }
        public virtual Boolean Sent { get; set; }
        public virtual String Subject { get; set; }
    }

    [Serializable]
    public class dtoDisplayUserMessageInfo
    {
        public virtual System.Guid RecipientIdentifier { get; set; }
        public virtual dtoUserMessageInfo Message { get; set; }
        public virtual Int32 IdPerson { get; set; }
    }
}