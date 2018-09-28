using lm.Comol.Core.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Business
{
    public partial class ServiceProjectManagement : BaseCoreServices, lm.Comol.Core.Notification.Domain.iNotifiableService
    {
        public List<lm.Comol.Core.Notification.Domain.GroupMessages> GetDefaultNotificationMessages(Core.Notification.Domain.NotificationAction action, int idSenderUser, Core.Notification.Domain.WebSiteSettings webSiteSettings)
        {
            List<lm.Comol.Core.Notification.Domain.GroupMessages> messages = null;
            return messages;
        }

        public List<Core.Notification.Domain.dtoModuleNotificationMessage> GetNotificationMessages(Core.Notification.Domain.NotificationAction action, Core.Notification.Domain.NotificationChannel channel, Core.Notification.Domain.NotificationMode mode, int idSenderUser, Core.Notification.Domain.WebSiteSettings webSiteSettings)
        {
            List<lm.Comol.Core.Notification.Domain.dtoModuleNotificationMessage> messages = null;
            return messages;
        }
    }
}
