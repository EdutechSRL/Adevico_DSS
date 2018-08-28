using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using lm.Notification.Core.Domain;
using lm.Notification.Core.DataLayer;
using lm.Notification.DataContract.Service;
using NHibernate;
using NHibernate.Linq;
using Helpers;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using lm.Notification.DataContract.Domain;
using System.Data;

namespace WS_Notifications
{
    /// <summary>
    /// Summary description for NotificationManagement
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class NotificationManagement : System.Web.Services.WebService, iManagementService
    {

        #region Management Templates
            [WebMethod]
            public List<dtoTemplateMessage> AvailableTemplates(int ModuleID, int ActionID, dtoTemplateType tType)
            {
                List<dtoTemplateMessage> templates = null;
                using (ISession session = NHSessionHelper.GetSession())
                {
                    DatabaseContext dc = new lm.Notification.Core.DataLayer.DatabaseContext(session);
                    try
                    {
                        List<TemplateMessage> l = (from TemplateMessage t in dc.Templates
                                                   where
                                                       (ActionID == -9999 || t.ActionID == ActionID) && t.ModuleID == ModuleID && (int)t.Type == (int)tType
                                                   orderby t.LanguageID
                                                   select t).ToList<TemplateMessage>();
                        templates = (from TemplateMessage t in l
                                     orderby t.LanguageID
                                     select new dtoTemplateMessage(t)).ToList<dtoTemplateMessage>();
                    }
                    catch (Exception ex)
                    {
                        templates = new List<dtoTemplateMessage>();
                    }


                    return templates;
                }
            }

            [WebMethod]
            public long SaveTemplate(dtoTemplateMessage template)
            {
                using (ISession session = NHSessionHelper.GetSession())
                {
                    try
                    {
                        session.BeginTransaction();
                        DatabaseContext dc = new lm.Notification.Core.DataLayer.DatabaseContext(session);
                        TemplateMessage t = session.Get<TemplateMessage>(template.ID);

                        if (t == null || t.ID == 0)
                        {
                            t = new TemplateMessage();
                            t.ActionID = template.ActionID;
                            t.LanguageID = template.LanguageID;
                            t.ModuleCode = template.ModuleCode;
                            t.ModuleID = template.ModuleID;
                            t.Type = (TemplateType)template.Type;
                        }
                        t.Name = template.Name;
                        t.Message = template.Message;
                        if (template.Message == "" && t.ID == 0)
                            return 0;
                        else if (template.Message == "" && t.ID > 0)
                            session.Delete(t);
                        else
                            session.SaveOrUpdate(t);
                        session.CommitTransaction();
                        return t.ID ;
                    }
                    catch (Exception ex)
                    {
                        session.RollbackTransaction();
                        return 0;
                    }

                }
            }

            [WebMethod]
            public Boolean RemoveTemplate(long templateID)
            {
                using (ISession session = NHSessionHelper.GetSession())
                {
                    try
                    {
                        session.BeginTransaction();
                        DatabaseContext dc = new lm.Notification.Core.DataLayer.DatabaseContext(session);
                        TemplateMessage t = session.Get<TemplateMessage>(templateID);

                        if (t != null && t.ID > 0)
                        {
                            session.Delete(t);
                            session.CommitTransaction();
                            return true;
                        }
                        return false;

                    }
                    catch (Exception ex)
                    {
                        session.RollbackTransaction();
                        return false;
                    }

                }
            }
            [WebMethod]
            public List<dtoModule> AvailableModules()
            {
                List<dtoModule> modules = null;
                using (ISession session = NHSessionHelper.GetSession())
                {
                    try
                    {
                        List<NotificatedModule> n = (from NotificatedModule m in session.Linq<NotificatedModule>()
                                                     orderby m.Name
                                                     select m).ToList<NotificatedModule>();

                        modules = (from NotificatedModule m in n
                                   select new dtoModule(m)).ToList<dtoModule>();

                        //modules = (from NotificatedModule m in session.Linq<NotificatedModule>()
                        //           orderby m.Name
                        //           select new dtoModule(m) ).ToList<dtoModule>();
                    }
                    catch (Exception ex)
                    {
                        modules = new List<dtoModule>();
                    }
                    return modules;
                }

            }
        #endregion


        #region "Remove SystemNotification"
            [WebMethod]
            public Boolean RemoveNews(Guid NotificationID)
            {
                using (ISession session = NHSessionHelper.GetSession())
                {
                    try
                    {
                        session.BeginTransaction();
                        NotificationMessage nm = session.Get<NotificationMessage>(NotificationID);

                        if (nm != null && nm.ID != System.Guid.Empty)
                        {
                            session.Delete(nm);
                            session.CommitTransaction();
                            return true;
                        }
                        return false;

                    }
                    catch (Exception ex)
                    {
                        session.RollbackTransaction();
                        return false;
                    }

                }
            }
            [WebMethod]
            public Boolean RemoveCommunityNews(int CommunityID)
            {
                Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

                using (DbConnection connection = oDatabase.CreateConnection())
                {
                    connection.Open();
                    try
                    {
                        DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_RemoveCommunityNotifications");
                        oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int64, CommunityID);
                        oCommand.Connection = connection;
                        oCommand.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.EventLog.WriteEntry("NotificationToPersonRepository", ex.Message);
                        return false;
                    }
                } 
            }
            [WebMethod]
            public Boolean RemoveUserNews(int PersonID)
            {
                Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

                using (DbConnection connection = oDatabase.CreateConnection())
                {
                    connection.Open();
                    try
                    {
                        DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_RemoveUserNotifications");
                        oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int64, PersonID);
                        oCommand.Connection = connection;
                        oCommand.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.EventLog.WriteEntry("NotificationToPersonRepository", ex.Message);
                        return false;
                    }
                } 
            }
            [WebMethod]
            public Boolean RemoveUserNewsByCommunity(int PersonID, int CommunityID)
            {
                Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

                using (DbConnection connection = oDatabase.CreateConnection())
                {
                    connection.Open();
                    try
                    {
                        DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_RemoveUserCommunityNotifications");
                        oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int64, PersonID);
                        oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int64, CommunityID);
                        oCommand.Connection = connection;
                        oCommand.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.EventLog.WriteEntry("NotificationToPersonRepository", ex.Message);
                        return false;
                    }
                } 
            }
        #endregion

        #region "Remove Summary"
            [WebMethod]
            public Boolean RemoveNewsSummary(Guid NotificationID)
            {
                using (ISession session = NHSessionHelper.GetSession())
                {
                    try
                    {
                        session.BeginTransaction();
                        NotificationSummary nm = session.Get<NotificationSummary>(NotificationID);

                        if (nm != null && nm.ID != System.Guid.Empty)
                        {
                            session.Delete(nm);
                            session.CommitTransaction();
                            return true;
                        }
                        return false;

                    }
                    catch (Exception ex)
                    {
                        session.RollbackTransaction();
                        return false;
                    }

                }
            }
            [WebMethod]
            public Boolean RemoveCommunityNewsSummary(int CommunityID)
            {
                Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

                using (DbConnection connection = oDatabase.CreateConnection())
                {
                    connection.Open();
                    try
                    {
                        DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_RemoveCommunityNewsSummary");
                        oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int64, CommunityID);
                        oCommand.Connection = connection;
                        oCommand.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.EventLog.WriteEntry("RemoveCommunityNewsSummary", ex.Message);
                        return false;
                    }
                }
            }
            [WebMethod]
            public Boolean RemoveUserNewsSummary(int PersonID)
            {
                Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

                using (DbConnection connection = oDatabase.CreateConnection())
                {
                    connection.Open();
                    try
                    {
                        DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_RemoveUserNewsSummary");
                        oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int64, PersonID);
                        oCommand.Connection = connection;
                        oCommand.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.EventLog.WriteEntry("RemoveUserNewsSummary", ex.Message);
                        return false;
                    }
                }
            }
            [WebMethod]
            public Boolean RemoveUserNewsSummaryByCommunity(int PersonID, int CommunityID)
            {
                Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

                using (DbConnection connection = oDatabase.CreateConnection())
                {
                    connection.Open();
                    try
                    {
                        DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_RemoveUserNewsSummaryByCommunity");
                        oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int64, PersonID);
                        oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int64, CommunityID);
                        oCommand.Connection = connection;
                        oCommand.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.EventLog.WriteEntry("RemoveUserNewsSummaryByCommunity", ex.Message);
                        return false;
                    }
                }
            }
        #endregion

            #region "Community News"

            [WebMethod]
            public List<dtoModule> GetModulesWithNews(int PersonID, DateTime StartDay)
            {              
                return RetrieveModulesWithNews(PersonID,-1,StartDay);
            }

            [WebMethod]
            public List<dtoModule> GetCommunityModulesWithNews(int PersonID, int CommunityID, DateTime StartDay)
            {
                return RetrieveModulesWithNews(PersonID, CommunityID, StartDay);
            }


            [WebMethod]
                public List<dtoCommunityWithNews> GetPersonalCommunityWithNews(int PersonID)
                {
                    List<dtoCommunityWithNews> reminders = null;
                    using (ISession session = NHSessionHelper.GetSession())
                    {
                        try
                        {
                            session.BeginTransaction();
                            List<CommunityNewsSummary> news = (from CommunityNewsSummary r in session.Linq<CommunityNewsSummary>()
                                         where r.PersonID == PersonID && r.ActionCount > 0
                                         orderby r.CommunityID
                                         select r).ToList<CommunityNewsSummary>();

                            reminders = (from CommunityNewsSummary r in news select new dtoCommunityWithNews(r)).ToList<dtoCommunityWithNews>();
                            session.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            if (session.isInTransaction())
                                session.RollbackTransaction();
                            reminders = new List<dtoCommunityWithNews>();
                        }
                        return reminders;
                    }
                }
                [WebMethod]       
                public void UpdateCommunityNewsCount(dtoCommunityWithNews previous)
                {
                    CommunityNewsSummary reminder = null;
                    using (ISession session = NHSessionHelper.GetSession())
                    {
                        try
                        {
                            session.BeginTransaction();
                            reminder = (from CommunityNewsSummary r in session.Linq<CommunityNewsSummary>()
                                         where r.PersonID == previous.PersonID && r.CommunityID == previous.CommunityID
                                         orderby r.CommunityID
                                        select r).FirstOrDefault<CommunityNewsSummary>();
                            if (reminder != null) {
                                if (previous.LastUpdate == null || previous.LastUpdate == new DateTime()) {
                                    reminder.ActionCount = 0;
                                }
                                else if (previous.LastUpdate== reminder.LastUpdate){
                                    reminder.ActionCount = 0;
                                }
                                else if (previous.ActionCount < reminder.ActionCount) {
                                    reminder.ActionCount = reminder.ActionCount - previous.ActionCount;
                                }
                                reminder.LastUpdate = DateTime.Now;

                                
                                session.SaveOrUpdate(reminder);
                                session.CommitTransaction();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (session.isInTransaction())
                                session.RollbackTransaction();
                        }
                    }
                
                }
                [WebMethod]
                public List<DateTime> GetWeekDaysWithNews(DateTime StartDay, int PersonID, int CommunityID)
                {
                    return this.GetDaysWithNews(StartDay, StartDay.AddDays(-7), PersonID, CommunityID);
                    // List<DateTime> WeekDays = null;
                    //using (ISession session = NHSessionHelper.GetSession())
                    //{
                    //    try
                    //    {
                    //        DateTime PreviousWeek = StartDay.AddDays(-7);
                    //        session.CreateQuery("
                    //        //WeekDays = (from NotificationBase ns in session.Linq<NotificationBase>() 
                    //        //            join PersonNotification p in session.Linq<PersonNotification>()
                    //        //            on ns.UniqueNotifyID equals p.NotificationUniqueID 
                    //        //                where p.PersonID == PersonID && (CommunityID<0 || ns.CommunityID == CommunityID)
                    //        //                && (ns.Day >= PreviousWeek || ns.Day <= StartDay)
                    //        //             orderby ns.Day ascending
                    //        //                select ns.Day).Distinct().ToList<DateTime>();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        WeekDays = new List<DateTime>();
                    //    }
                    //}
                }
                [WebMethod]
                public List<DateTime> GetMonthDaysWithNews(DateTime StartDay, int PersonID, int CommunityID)
                {
                    //List<DateTime> MonthDays = null;
                    return this.GetDaysWithNews(StartDay, StartDay.AddDays(-30), PersonID, CommunityID);
                    //using (ISession session = NHSessionHelper.GetSession())
                    //{
                    //    try
                    //    {
                    //        DateTime PreviousMonth = StartDay.AddMonths(-1);
                    //        MonthDays = (from NotificationSummary ns in session.Linq<NotificationSummary>()
                    //                    where ns.PersonID == PersonID && (CommunityID < 0 || ns.CommunityID == CommunityID)
                    //                    && (ns.Day >= PreviousMonth || ns.Day <= StartDay)
                    //                    orderby ns.Day ascending
                    //                    select ns.Day).Distinct().ToList<DateTime>();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        MonthDays = new List<DateTime>();
                    //    }
                    //}

                    //return MonthDays;
                }
                [WebMethod]
                public List<dtoCommunitySummaryNotification> GetCommunitySummary(DateTime StartDay, int PersonID, int CommunityID, int LanguageID)
                {
                    List<dtoCommunitySummaryNotification> summaries=null;

                    Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

                    using (DbConnection connection = oDatabase.CreateConnection())
                    {
                        connection.Open();
                        try
                        {
                            DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_CommunityNewsSummaryRepository_GetCommunitySummary");
                            oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int32, CommunityID);
                            oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int32, PersonID);
                            oDatabase.AddInParameter(oCommand, "@StartDay", System.Data.DbType.DateTime, StartDay);
                            oDatabase.AddInParameter(oCommand, "@LanguageID", System.Data.DbType.Int32, LanguageID);
                            
                            oCommand.Connection = connection;
                            IDataReader reader = oCommand.ExecuteReader();
                            summaries = new List<dtoCommunitySummaryNotification>();
                            while (reader.Read())
                            {
                                dtoCommunitySummaryNotification summary = new dtoCommunitySummaryNotification();
                                summary.ID = (int)(long)reader.GetValue(reader.GetOrdinal("CommunityID"));
                                summary.Count = (int)reader.GetValue(reader.GetOrdinal("Totale"));
                                summary.Day = (DateTime)reader.GetValue(reader.GetOrdinal("DayData"));
                                summary.ModuleID = (int)reader.GetValue(reader.GetOrdinal("ModuleID"));
                                summaries.Add(summary);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.EventLog.WriteEntry("GetWeekDaysWithNews", ex.Message);
                            summaries = new List<dtoCommunitySummaryNotification>();
                        }
                    }
                    return summaries;
                }

                [WebMethod]
                public List<CommunityNews> GetCommunityNews(bool OnlySummary, DateTime StartDay, int PersonID, int CommunityID, int UserLanguageID, int DefaultLanguageID, int PageSize, int PageIndex)
                {
                    List<CommunityNews> iResponse = null;
                    using (ISession session = NHSessionHelper.GetSession())
                    {
                       try
                        {
                            IQueryable<PersonNotification> p;
                            p = (from n in session.Linq<PersonNotification>()
                                 where n.PersonID == PersonID && n.isDeleted == false && n.Day == StartDay && n.CommunityID == CommunityID
                                 orderby n.SentDate descending 
                                 select n).Skip(PageSize * PageIndex).Take(PageSize);
                            List<PersonNotification> pp = p.ToList<PersonNotification>();
                            IQueryable<NotificationBase> t;
                            t = (from n in session.Linq<NotificationBase>()
                                         where n.CommunityID == CommunityID && n.LanguageID == UserLanguageID && n.Day == StartDay
                                         select n);

                            List<NotificationBase> tt = t.ToList<NotificationBase>();
                            iResponse = (from NotificationBase nb in tt
                                         join PersonNotification pn in pp on nb.UniqueNotifyID equals pn.NotificationUniqueID
                                         select new CommunityNews() { CommunityID = CommunityID, Day = StartDay, Message = nb.Message, ModuleID = nb.ModuleID, SentDate = nb.SentDate, UniqueID = nb.UniqueNotifyID }).ToList<CommunityNews>();

                       }
                       catch (Exception ex)
                       {
                           iResponse = new List<CommunityNews>();
                       }
                    } 

                    return iResponse;
                }
                [WebMethod]
                public int GetCommunityNewsCount(bool OnlySummary, DateTime StartDay, int PersonID, int CommunityID, int UserLanguageID, int DefaultLanguageID)
                {
                    int iResponse = 0;
                    using (ISession session = NHSessionHelper.GetSession())
                    {
                        try
                        {
                            iResponse = (from n in session.Linq<PersonNotification>()
                                 where n.PersonID == PersonID && n.isDeleted == false && n.Day == StartDay && n.CommunityID == CommunityID
                                 select n.ID).Count();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    return iResponse;
                }

                [WebMethod]
                public List<CommunityNews> GetPersonCommunityNews(bool OnlySummary, DateTime StartDay, int PersonID, int UserLanguageID, int DefaultLanguageID, int PageSize, int PageIndex)
                {
                    return new List<CommunityNews>();
                }
                [WebMethod]
                public int GetPersonCommunityNewsCount(bool OnlySummary, DateTime StartDay, int PersonID, int UserLanguageID, int DefaultLanguageID)
                {
                    return 0;
                }


                private List<DateTime> GetDaysWithNews(DateTime StartDay, DateTime EndDay, int PersonID, int CommunityID)
                {
                    List<DateTime> WeekDays = null;
                    Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

                    using (DbConnection connection = oDatabase.CreateConnection())
                    {
                        connection.Open();
                        try
                        {
                            DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_CommunityNewsSummaryRepository_GetDaysWithNews");
                            oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int32, CommunityID);
                            oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int32, PersonID);
                            oDatabase.AddInParameter(oCommand, "@StartDay", System.Data.DbType.DateTime, StartDay);
                            oDatabase.AddInParameter(oCommand, "@EndDay", System.Data.DbType.DateTime, EndDay);
                            oCommand.Connection = connection;
                            IDataReader reader = oCommand.ExecuteReader();
                            WeekDays = new List<DateTime>();
                            while (reader.Read()) {
                                WeekDays.Add((DateTime)reader.GetValue(0));
                            }
                        }
                        catch (Exception ex)
                        {
                            //throw new Exception("GetDaysWithNews" + " " + ex.Message + " INNER=" + ex.StackTrace);
                            WeekDays = new List<DateTime>();
                        }
                    }
                    return WeekDays;
                }




                private List<dtoModule> RetrieveModulesWithNews(int PersonID, int CommunityID, DateTime StartDay)
                {
                    List<dtoModule> modules = new List<dtoModule>();
                    Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

                    using (DbConnection connection = oDatabase.CreateConnection())
                    {
                        connection.Open();
                        try
                        {
                            DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_RetrieveModulesWithNews");
                            oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int32, CommunityID);
                            oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int32, PersonID);
                            oDatabase.AddInParameter(oCommand, "@StartDay", System.Data.DbType.DateTime, StartDay);
                            oCommand.Connection = connection;
                            IDataReader reader = oCommand.ExecuteReader();
                            while (reader.Read())
                            {
                                dtoModule module = new dtoModule();
                                module.ID = (int)reader.GetValue(reader.GetOrdinal("NTFC_ModuleID"));
                                module.Name = (string)reader.GetValue(reader.GetOrdinal("NTFC_ModuleCode"));
                                //NTFC_ModuleCode, dbo.Notification.NTFC_ModuleID
                                  //   modules.Add((DateTime)reader.GetValue(0));
                                modules.Add(module);
                            }
                        }
                        catch (Exception ex)
                        {
                            //System.Diagnostics.EventLog.WriteEntry("RetrieveModulesWithNews", ex.Message);
                            throw new Exception(ex.Message + " INNER=" + ex.StackTrace);
                        }
                    }
                    return modules;
                }
            #endregion



                #region ALL NEWS !!!

                [WebMethod]
                public List<dtoNewsInfo> GetPortalAllNewsInfo(int PersonID, DateTime FromDay)
                {
                    return RetrieveAllNewsInfo(PersonID,-1,FromDay);
                }
                [WebMethod]
                public int GetPortalAllNewsCount(int PersonID, DateTime FromDay)
                {
                    return RetrieveAllNewsCount(PersonID, -1, FromDay);
                }

                [WebMethod]
                public List<dtoNewsInfo> GetCommunityAllNewsInfo(int PersonID, int CommunityID, DateTime FromDay)
                {
                    return RetrieveAllNewsInfo(PersonID, CommunityID, FromDay);
                }
                
                [WebMethod]
                public int GetCommunityAllNewsCount(int PersonID, int CommunityID, DateTime FromDay)
                {
                    return RetrieveAllNewsCount(PersonID, CommunityID, FromDay);
                }

                [WebMethod]
                public List<CommunityNews> GetNotifications(List<Guid> notificationsID, int UserLanguageID, int DefaultLanguageID)
                {
                    List<CommunityNews> iResponse = new List<CommunityNews>();
                    using (ISession session = NHSessionHelper.GetSession())
                    {
                        try
                        {
                            foreach (Guid ID in notificationsID)
                            {
                                NotificationBase notification = (from nb in session.Linq<NotificationBase>()
                                                                 where nb.UniqueNotifyID == ID && nb.LanguageID == UserLanguageID
                                                                 select nb).FirstOrDefault<NotificationBase>();
                                if (notification.UniqueNotifyID != System.Guid.Empty)
                                {
                                    CommunityNews news = new CommunityNews(notification.UniqueNotifyID, notification.CommunityID, notification.SentDate, notification.Day, notification.Message, notification.ModuleID);
                                    iResponse.Add(news);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            iResponse = new List<CommunityNews>();
                        }
                    }

                    return iResponse;
                }

                private int RetrieveAllNewsCount(int PersonID, int CommunityID, DateTime FromDay)
                {
                    Nullable<int> NewsCount = 0;
                    Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

                    using (DbConnection connection = oDatabase.CreateConnection())
                    {
                        string errore = "";
                        connection.Open();

                      
                        try
                        {
                            errore = "";
                            DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_RetrieveAllNewsCount");
                            oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int32, CommunityID);
                            oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int32, PersonID);
                            oDatabase.AddInParameter(oCommand, "@FromDay", System.Data.DbType.DateTime, FromDay);
                            oCommand.Connection = connection;

                            NewsCount = (Nullable<int>)oCommand.ExecuteScalar();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(errore + " " + ex.Message + " INNER=" + ex.StackTrace);
                            //System.Diagnostics.EventLog.WriteEntry("sp_RetrieveAllNewsCount", ex.Message);
                        }
                    }
                    if (NewsCount.HasValue)
                        return NewsCount.Value;
                    return 0;
                }
                private List<dtoNewsInfo> RetrieveAllNewsInfo(int PersonID, int CommunityID, DateTime FromDay)
                {
                    List<dtoNewsInfo> newsInfo = new List<dtoNewsInfo>();
                    Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

                    using (DbConnection connection = oDatabase.CreateConnection())
                    {
                        connection.Open();
                        try
                        {
                            DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_RetrieveAllNewsInfo");
                            oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int32, CommunityID);
                            oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int32, PersonID);
                            oDatabase.AddInParameter(oCommand, "@FromDay", System.Data.DbType.DateTime, FromDay);
                            oCommand.Connection = connection;
                            IDataReader reader = oCommand.ExecuteReader();
                            while (reader.Read())
                            {
                                dtoNewsInfo info = new dtoNewsInfo();
                                info.UniqueID = (Guid)reader.GetValue(reader.GetOrdinal("NTFP_NTFC_UniqueID"));
                                info.CommunityID = (int)(long)reader.GetValue(reader.GetOrdinal("NTFP_CommunityID"));
                                info.Day = (DateTime)reader.GetValue(reader.GetOrdinal("NTFP_Day"));
                                info.ModuleID = (int)reader.GetValue(reader.GetOrdinal("NTFC_ModuleID"));
                                info.SentDate = (DateTime)reader.GetValue(reader.GetOrdinal("NTFP_SentDate"));
                                newsInfo.Add(info);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message + " INNER=" + ex.StackTrace);
                            //System.Diagnostics.EventLog.WriteEntry("RetrieveAllNewsInfo", ex.Message);
                        }
                    }
                    return newsInfo;
                }

                #endregion

    }
}
