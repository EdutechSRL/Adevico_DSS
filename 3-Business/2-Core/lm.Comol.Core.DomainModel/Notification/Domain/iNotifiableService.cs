using lm.Comol.Core.Notification.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Notification.Domain
{
    public interface iNotifiableService
    {
        List<GroupMessages> GetDefaultNotificationMessages(NotificationAction action, Int32 idSenderUser, WebSiteSettings webSiteSettings);

        List<dtoModuleNotificationMessage> GetNotificationMessages(NotificationAction action, NotificationChannel channel, NotificationMode mode, Int32 idSenderUser, WebSiteSettings webSiteSettings);


        //[OperationContract(IsOneWay = true)]
        //void NotifyToUsers(String istanceIdentifier, Int32 idUser, Int32 idCommunity, String moduleCode, long idAction, lm.Comol.Core.Notification.Domain.NotificationChannel channel, lm.Comol.Core.Notification.Domain.NotificationMode mode, List<Int32> idUsers, List<long> moduleUsers = null, long idObject = 0, Int32 idObjectType = 0);
    }
}
