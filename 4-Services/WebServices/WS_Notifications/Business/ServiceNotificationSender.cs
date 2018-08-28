using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using lm.Notification.DataContract.Domain;
using lm.Notification.DataContract.Service;

namespace WS_Notifications.Business
{
    public class ServiceNotificationSender
    {

        private SRVnotification.iNotificationService RemoteService{
            get{
                SRVnotification.iNotificationService service;
                try{
                    service = new SRVnotification.iNotificationServiceClient(); 
                    }
                catch(Exception ex){
                    service = new FakeSRVnotification();
                    }
                
                return service;
            }
          
        }


        public void SendNotificationToCommunity(NotificationToCommunity notification)
        {
            //notification.SentDate = DateTime.Now;
            this.RemoteService.NotifyToCommunity(notification);
        }

        public void SendNotificationToPermission(NotificationToPermission notification)
        {
            //notification.SentDate = DateTime.Now;
            this.RemoteService.NotifyForPermission(notification);
        }

        public void SendNotificationToPerson(NotificationToPerson notification)
        {
            //notification.SentDate = DateTime.Now;
            this.RemoteService.NotifyToUsers(notification);
        }


        public Boolean isQueueAlive()
        {
            if (RemoteService.GetType() == typeof(FakeSRVnotification))
                return false;
            else
                return true;
        }

        public void DeleteNotification(System.Guid NotificationID)
        {

        }
        public void DeleteCommunityNotification(int UserID, int CommunityID)
        {

        }
        public void DeleteUserNotification(int UserID)
        {

        }


    }
}
