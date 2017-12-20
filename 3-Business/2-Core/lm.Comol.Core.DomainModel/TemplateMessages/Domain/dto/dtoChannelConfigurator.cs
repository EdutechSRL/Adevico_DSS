using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class dtoChannelConfigurator
    {
        public virtual System.Guid TemporaryId { get; set; }
        public virtual long IdSettings { get { return (Settings == null) ? 0 : Settings.Id; } }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public virtual dtoNotificationSettings Settings { get; set; }
        public virtual lm.Comol.Core.Notification.Domain.NotificationChannel Channel { get { return (Settings != null) ? Settings.Channel :  Notification.Domain.NotificationChannel.None; } }
        //public virtual List<NotificationMode> AvailableModes { get; set; }
        //public virtual List<String> AvailableModuleCodes { get; set; }
        public virtual Boolean AllowDelete { get; set; }
        public virtual Boolean IsEnabled { get { return (Settings != null) ? Settings.IsEnabled : false; }
            set {
                if (Settings != null)
                    Settings.IsEnabled = value;
            }
        }
        public dtoChannelConfigurator()
        {
            //AvailableModes = new List<NotificationMode>();
            //AvailableModuleCodes = new List<String>();
            AllowDelete = true;
        }
        public void Dispose()
        {
  
        }
    }
}