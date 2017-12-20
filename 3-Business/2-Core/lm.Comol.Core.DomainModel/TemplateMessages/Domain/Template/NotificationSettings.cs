using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class NotificationSettings
    {
        public virtual long IdModuleAction { get; set; }
        public virtual Int32 IdModule { get; set; }
        public virtual String ModuleCode { get; set; }
        public virtual lm.Comol.Core.Notification.Domain.NotificationActionType ActionType { get; set; }
        public virtual lm.Comol.Core.Notification.Domain.NotificationChannel Channel { get; set; }
        public virtual lm.Comol.Core.Notification.Domain.NotificationMode Mode { get; set; }

        public NotificationSettings()
        {
            Mode = lm.Comol.Core.Notification.Domain.NotificationMode.None;
            ActionType = lm.Comol.Core.Notification.Domain.NotificationActionType.Ignore;
        }

        public void Dispose()
        {
  
        }
    }
}