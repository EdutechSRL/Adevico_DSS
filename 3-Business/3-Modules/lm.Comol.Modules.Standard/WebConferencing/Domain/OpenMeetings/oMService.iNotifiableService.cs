using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.OpenMeetings
{
    public partial class oMService : lm.Comol.Core.Notification.Domain.iNotifiableService
    {

        public List<lm.Comol.Core.Notification.Domain.GroupMessages> GetDefaultNotificationMessages(Core.Notification.Domain.NotificationAction action, int idSenderUser, Core.Notification.Domain.WebSiteSettings webSiteSettings)
        {
            throw new NotImplementedException();
        }

        public List<Core.Notification.Domain.dtoModuleNotificationMessage> GetNotificationMessages(Core.Notification.Domain.NotificationAction action, Core.Notification.Domain.NotificationChannel channel, Core.Notification.Domain.NotificationMode mode, int idSenderUser, Core.Notification.Domain.WebSiteSettings webSiteSettings)
        {
            throw new NotImplementedException();
        }
    }
}
