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

    public class GroupMessages
    {
      
        [DataMember]
        public virtual Int32 IdCommunity { get; set; }
        [DataMember]
        public virtual ModuleObject ObjectOwner { get; set; }
        [DataMember]
        public virtual NotificationChannel Channel { get; set; }

        [DataMember]
        public virtual GroupSettings Settings { get; set; }
       
        [DataMember]
        public virtual List<dtoModuleTranslatedMessage> Messages { get; set; }
       
        public GroupMessages() {
            Settings = new GroupSettings();
            Messages = new List<dtoModuleTranslatedMessage>();
        }
        public GroupMessages(NotificationChannel channel)
        {
            Channel = channel;
            Settings = new GroupSettings();
            Messages = new List<dtoModuleTranslatedMessage>();
        }


        public virtual Boolean IsValid()
        {
            return Messages != null && ((Channel != NotificationChannel.None && Channel != lm.Comol.Core.Notification.Domain.NotificationChannel.Mail) || (Settings.Mail != null && Channel == lm.Comol.Core.Notification.Domain.NotificationChannel.Mail));
        }
    }
}