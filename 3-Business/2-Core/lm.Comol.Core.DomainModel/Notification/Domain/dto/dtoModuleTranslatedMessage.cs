using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using lm.Comol.Core.DomainModel;
using System.Runtime.Serialization;

namespace lm.Comol.Core.Notification.Domain
{
    [Serializable,DataContract]

    public class dtoModuleTranslatedMessage
    {
        [DataMember]
        public virtual lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation Translation { get; set; }
        [DataMember]
        public virtual List<lm.Comol.Core.MailCommons.Domain.Messages.Recipient> Recipients { get; set; }
        public dtoModuleTranslatedMessage() { }
        public dtoModuleTranslatedMessage(NotificationChannel channel)
        {
            Translation = new DomainModel.Languages.ItemObjectTranslation();
            Recipients = new List<MailCommons.Domain.Messages.Recipient>();
        }

    }
}