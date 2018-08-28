using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using lm.Notification.DataContract.Domain;
using WS_Notifications.Business;

namespace WS_Notifications
{
    /// <summary>
    /// Summary description for NotificationSender
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class NotificationSender : System.Web.Services.WebService

    {
        private ServiceNotificationSender _ServiceSender = new ServiceNotificationSender();
        [WebMethod]
        public void SendNotificationToCommunity(NotificationToCommunity notification) {
            _ServiceSender.SendNotificationToCommunity(notification);
        }

        [WebMethod]
        public void SendNotificationToPermission(NotificationToPermission notification)
        {
            _ServiceSender.SendNotificationToPermission(notification);
        }

        [WebMethod]
        public void SendNotificationToPerson(NotificationToPerson notification)
        {
            _ServiceSender.SendNotificationToPerson(notification);
        }


        [WebMethod]
        public Boolean isQueueAlive() {
            return _ServiceSender.isQueueAlive();
        }

        [WebMethod]
        public void DeleteNotification(System.Guid NotificationID)
        {
            _ServiceSender.DeleteNotification(NotificationID);
        }
        [WebMethod]
        public void DeleteCommunityNotification(int CommunityID)
        {
            _ServiceSender.DeleteCommunityNotification(-1, CommunityID);
        }
        [WebMethod]
        public void DeleteUserNotification(int UserID)
        {
            _ServiceSender.DeleteUserNotification(UserID);
        }
        [WebMethod]
        public void DeleteUserCommunityNotification(int UserID, int CommunityID)
        {
            _ServiceSender.DeleteCommunityNotification(UserID, CommunityID);
        }



        [WebMethod]
        public void CreateNotificationTest(int number)
        {
            string Formato = "File_{0}";
            string FormatoBacheca = "Bacheca_{0}.bacheca";
            NotificationToCommunity notification = new NotificationToCommunity();
            notification.ActionID = 70007;
            notification.CommunityID = 1;
            notification.ModuleCode = "SRVMATER";
            notification.ModuleID = 1;
            notification.SentDate = DateTime.Now;
            notification.ValueParameters = new List<string>();
            notification.ValueParameters.Add("");
            notification.ValueParameters.Add("");
            notification.ValueParameters.Add("Facoltà di Economia");
            notification.NotificatedObjects = new List<dtoNotificatedObject>();
            notification.NotificatedObjects.Add(new dtoNotificatedObject() { FullyQualiFiedName = "file", ModuleCode = "SRVMATER", ModuleID = 1, ObjectID = "1", ObjectTypeID = 1 });


            for (int i = 0; i < number; i++) {
                notification.SentDate = notification.SentDate.AddHours(1);
                notification.ValueParameters[1] = string.Format(Formato,  i);
                notification.ValueParameters[0] = notification.ValueParameters[1] + ".download";
                this.SendNotificationToCommunity(notification);
            }
        

            NotificationToPerson notificationPerson = new NotificationToPerson();
            notificationPerson.ActionID = 12004;
            notificationPerson.CommunityID = 58;
            notificationPerson.ModuleCode = "SRVBACH";
            notificationPerson.ModuleID = 10;
            notificationPerson.SentDate = DateTime.Now;
            notificationPerson.ValueParameters = new List<string>();
            notificationPerson.ValueParameters.Add("");
            notificationPerson.ValueParameters.Add("Laboratorio Maieutiche");
            notificationPerson.PersonsID = new List<int>();
            notificationPerson.PersonsID.Add(1);
            notificationPerson.PersonsID.Add(3);
            notificationPerson.PersonsID.Add(149);
            notification.NotificatedObjects = new List<dtoNotificatedObject>();
            notification.NotificatedObjects.Add(new dtoNotificatedObject() { FullyQualiFiedName = "bacheca", ModuleCode = "SRVBACH", ModuleID = 10, ObjectID = "1", ObjectTypeID = 1 });

            for (int i = 0; i < number; i++)
            {
                notificationPerson.SentDate = notificationPerson.SentDate.AddHours(1);
                notificationPerson.ValueParameters[0] = string.Format(FormatoBacheca, i) + ""; ;
                this.SendNotificationToPerson(notificationPerson);
            }
        

            NotificationToPermission notificationPermission = new NotificationToPermission();
            notificationPermission.ActionID = 12006;
            notificationPermission.CommunityID = 58;
            notificationPermission.ModuleCode = "SRVBACH";
            notificationPermission.ModuleID = 10;
            notificationPermission.SentDate = DateTime.Now;
            notificationPermission.ValueParameters = new List<string>();
            notificationPermission.ValueParameters.Add("Laboratorio Maieutiche");
            notificationPermission.Permission  = 6;
            notification.NotificatedObjects = new List<dtoNotificatedObject>();
            notification.NotificatedObjects.Add(new dtoNotificatedObject() { FullyQualiFiedName = "bacheca", ModuleCode = "SRVBACH", ModuleID = 10, ObjectID = "1", ObjectTypeID = 1 });

            for (int i = 0; i < number; i++)
            {
                notificationPermission.SentDate = notificationPermission.SentDate.AddHours(1);
                this.SendNotificationToPermission(notificationPermission);
            }
          
        }

        [WebMethod]
        public void CreateOneNotification()
        {
            string Formato = "File_{0}";
            string FormatoBacheca = "Bacheca_{0}.bacheca";
            NotificationToCommunity notification = new NotificationToCommunity();
            notification.ActionID = 70007;
            notification.CommunityID = 1;
            notification.ModuleCode = "SRVMATER";
            notification.ModuleID = 1;
            notification.SentDate = DateTime.Now;
            notification.ValueParameters = new List<string>();
            notification.ValueParameters.Add("");
            notification.ValueParameters.Add("");
            notification.ValueParameters.Add("Facoltà di Economia");
            notification.NotificatedObjects = new List<dtoNotificatedObject>();
            notification.NotificatedObjects.Add(new dtoNotificatedObject() { FullyQualiFiedName = "file", ModuleCode = "SRVMATER", ModuleID = 1, ObjectID = "1", ObjectTypeID = 1 });

            for (int i = 0; i < 1; i++)
            {
                notification.SentDate = notification.SentDate.AddHours(1);
                notification.ValueParameters[1] = string.Format(Formato, i);
                notification.ValueParameters[0] = notification.ValueParameters[1] + ".download";
                this.SendNotificationToCommunity(notification);
            }
        
          
        }


        [WebMethod]
        public void TEST_CreateNotificationToPersons()
        {
            string Formato = "File_{0}";
            NotificationToPerson notification = new NotificationToPerson();
            notification.ActionID = 70007;
            notification.CommunityID = 1;
            notification.ModuleCode = "SRVMATER";
            notification.ModuleID = 1;
            notification.SentDate = DateTime.Now;
            notification.PersonsID.Add(1);
            notification.PersonsID.Add(2);
            notification.PersonsID.Add(3);
            notification.PersonsID.Add(4);
            notification.ValueParameters = new List<string>();
            notification.ValueParameters.Add("");
            notification.ValueParameters.Add("");
            notification.ValueParameters.Add("Facoltà di Economia");
            notification.NotificatedObjects = new List<dtoNotificatedObject>();
            notification.NotificatedObjects.Add(new dtoNotificatedObject() { FullyQualiFiedName = "file", ModuleCode = "SRVMATER", ModuleID = 1, ObjectID = "1", ObjectTypeID = 1 });

            for (int i = 0; i < 1; i++)
            {
                notification.SentDate = notification.SentDate.AddHours(1);
                notification.ValueParameters[1] = string.Format(Formato, i);
                notification.ValueParameters[0] = notification.ValueParameters[1] + ".download";
                this.SendNotificationToPerson(notification);
            }

    
        }

        [WebMethod]
        public void TESTmultiplo_CreateNotificationToPersons(int number)
        {
            for (int i = 0; i < number; i++)
            {
                this.TEST_CreateNotificationToPersons();
            }
           
        }


        [WebMethod]
        public void TEST_NotificaUtentiComunita(int CommunityID)
        {
            string Formato = "File_{0}";
            NotificationToCommunity notification = new NotificationToCommunity();
            notification.ActionID = 12004;
            notification.CommunityID = CommunityID;
            notification.ModuleCode = "SRVBACH";
            notification.ModuleID = 10;
            notification.SentDate = DateTime.Now;
            notification.ValueParameters = new List<string>();
            notification.ValueParameters.Add("650");
            notification.ValueParameters.Add("22:00:00");
            notification.ValueParameters.Add("Autore solitario");
            notification.NotificatedObjects = new List<dtoNotificatedObject>();
            notification.NotificatedObjects.Add(new dtoNotificatedObject() { FullyQualiFiedName = "file", ModuleCode = "SRVBACH", ModuleID = 10, ObjectID = "1", ObjectTypeID = 1 });

            this.SendNotificationToCommunity(notification);

        }

        [WebMethod]
        public void TEST_NotificaComunitaBachecaByPermission(int CommunityID)
        {
            string Formato = "File_{0}";
            NotificationToPermission notification = new NotificationToPermission();
            notification.ActionID = 12004;
            notification.CommunityID = CommunityID;
            notification.ModuleCode = "SRVBACH";
            notification.ModuleID = 10;
            notification.SentDate = DateTime.Now;
            notification.ValueParameters = new List<string>();
            notification.ValueParameters.Add("650");
            notification.ValueParameters.Add("22:00:00");
            notification.ValueParameters.Add("Autore solitario");
            notification.NotificatedObjects = new List<dtoNotificatedObject>();
            notification.NotificatedObjects.Add(new dtoNotificatedObject() { FullyQualiFiedName = "file", ModuleCode = "SRVBACH", ModuleID = 10, ObjectID = "1", ObjectTypeID = 1 });
            notification.Permission = 32;
            this.SendNotificationToPermission(notification);

        }
    }
}
