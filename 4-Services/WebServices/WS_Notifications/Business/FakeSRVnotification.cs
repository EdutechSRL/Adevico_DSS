using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using lm.Notification.DataContract.Service;
using lm.Notification.DataContract.Domain;

namespace WS_Notifications.Business
{
    public class FakeSRVnotification : SRVnotification.iNotificationService  
    {

        #region iNotificationService Members

        public void NotifyToCommunity(NotificationToCommunity Notification)
        {
            
        }

        public void NotifyToUsers(NotificationToPerson Notification)
        {
            
        }

        public void NotifyForPermission(NotificationToPermission Notification)
        {
            
        }

        #endregion

        #region iNotificationService Members


        public void RemoveNotification(Guid NotificationID)
        {
            throw new NotImplementedException();
        }

        #endregion


        public void NotifyForRoles(NotificationToRole Notification)
        {
            throw new NotImplementedException();
        }

        public void NotifyForPermissionItemGuid(NotificationToItemGuid Notification)
        {
            throw new NotImplementedException();
        }

        public void NotifyForPermissionItemLong(NotificationToItemLong Notification)
        {
            throw new NotImplementedException();
        }

        public void NotifyForPermissionItemInt(NotificationToItemInt Notification)
        {
            throw new NotImplementedException();
        }

        public void RemoveNotificationForUser(Guid NotificationID, int PersonID)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserNotification(Guid UserNotificationID, int PersonID)
        {
            throw new NotImplementedException();
        }

        public void ReadNotification(Guid NotificationID, int PersonID)
        {
            throw new NotImplementedException();
        }

        public void ReadUserNotification(Guid UserNotificationID, int PersonID)
        {
            throw new NotImplementedException();
        }

        public void ReadUserCommunityNotification(int CommunityID, int PersonID)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserCommunityNotification(int CommunityID, int PersonID)
        {
            throw new NotImplementedException();
        }
    }
}
