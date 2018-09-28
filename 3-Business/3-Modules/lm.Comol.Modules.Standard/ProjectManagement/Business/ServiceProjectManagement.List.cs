using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;
using lm.Comol.Modules.Standard.GraphTheory;
namespace lm.Comol.Modules.Standard.ProjectManagement.Business
{
    public partial class ServiceProjectManagement : BaseCoreServices
    {
        #region "Summary"
            public List<dtoTimelineSummary> GetSummary(Int32 idPerson, long idProject, Boolean forceRetrieve = false) {
                return GetSummary(idPerson, idProject, ProjectFilterBy.All, ItemListStatus.All,-100, forceRetrieve);
            }
            public List<dtoTimelineSummary> GetSummary(Int32 idPerson, ProjectFilterBy filter, ItemListStatus filterStatus, Int32 idCommunity = -100, Boolean forceRetrieve = false) {
                return GetSummary(idPerson, 0, filter, filterStatus, idCommunity, forceRetrieve);
            }
            private List<dtoTimelineSummary> GetSummary(Int32 idPerson, long idProject = 0, ProjectFilterBy filter = ProjectFilterBy.All, ItemListStatus filterStatus = ItemListStatus.All, Int32 idCommunity = -100, Boolean forceRetrieve = false)
            {
                List<dtoTimelineSummary> results = new List<dtoTimelineSummary>();
                try
                {
                    String key = (idProject == 0) ? CacheKeys.SummaryUser(idPerson, filter,  filterStatus, idCommunity) : CacheKeys.SummaryUser(idPerson, idProject);
                    results = lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<List<dtoTimelineSummary>>(key);
                    if (results == null || !results.Any() || forceRetrieve){
                        results = GetSummaryItems(idPerson, idProject, filter, filterStatus, idCommunity);
                        lm.Comol.Core.DomainModel.Helpers.CacheHelper.AddToCache<List<dtoTimelineSummary>>(key, results, lm.Comol.Core.DomainModel.Helpers.CacheExpiration.Day);
                    }
                }
                catch (Exception ex) {
                    results = null;
                }
                return results;
            }

            public List<dtoTimelineSummary> GetSummaryItems(Int32 idPerson,long idProject=0, ProjectFilterBy filter = ProjectFilterBy.All, ItemListStatus filterStatus = ItemListStatus.All, Int32 idCommunity = -100)
            {
                List<dtoTimelineSummary> results = new List<dtoTimelineSummary>();
                try
                {
                    dtoTimelineSummary rSummary = new dtoTimelineSummary() { DashboardPage = PageListType.DashboardResource, DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first };

                    IEnumerable<liteProjectActivityAssignment> assignments = GetAssigmentsForSummary(idPerson, idProject, filter, filterStatus, idCommunity);
                    if (assignments!=null)
                        rSummary.Activities.AddRange(GetTimeLineActivities(assignments,idProject, PageListType.DashboardResource));
                    results.Add(rSummary);
                    List<dtoTimelineActivity> mActivities = GetTimeLineActivitiesToManage(idPerson, idProject, filter, filterStatus, idCommunity);
                    if (mActivities.Any())
                        results.Add(new dtoTimelineSummary() { Activities = mActivities, DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.last, DashboardPage = PageListType.DashboardManager });
                    if (results.Count > 1) {
                        results.First().DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first;
                        results.Last().DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.last;
                    }
                }
                catch (Exception ex) {
                    results = null;
                }
                return results;
            }

            #region "GetItemsForSummary"
                public IEnumerable<liteProjectActivityAssignment> GetAssigmentsForSummary(Int32 idPerson, long idProject = 0, ProjectFilterBy filter = ProjectFilterBy.All, ItemListStatus filterStatus = ItemListStatus.All, Int32 idCommunity = -100)
                {
                    IEnumerable<liteProjectActivityAssignment> results = null;
                    try
                    {
                        var query =  (from a in Manager.GetIQ<liteProjectActivityAssignment>()
                                      where a.Deleted == BaseStatusDeleted.None && a.Completeness < 100 && a.IdPerson == idPerson && (idProject <= 0 || (a.Project != null && a.Project.Id == idProject))
                                          select a);
                        if (idProject < 1 && (filter != ProjectFilterBy.All || idCommunity != -100)){
                            switch (filter) { 
                                case ProjectFilterBy.AllPersonal:
                                    query = query.Where(a => a.Project.isPersonal);
                                    break;
                                case ProjectFilterBy.AllPersonalFromCurrentCommunity:
                                    query = query.Where(a => !a.Project.isPortal && a.Project.isPersonal && a.Project.IdCommunity== idCommunity);
                                    break;
                                case ProjectFilterBy.CurrentCommunity:
                                    query = query.Where(a => a.Project.IdCommunity == idCommunity);
                                    break;
                                case ProjectFilterBy.FromPortal:
                                     query = query.Where(a=> a.Project.isPortal && !a.Project.isPersonal );
                                     break;
                            }
                           
                        }
                        switch (filterStatus)
                        {
                            case ItemListStatus.All:
                                query = query.Where(a => a.Project.Availability != ProjectAvailability.Draft);
                                break;
                            case ItemListStatus.Active:
                                query = query.Where(a => a.Project.Availability == ProjectAvailability.Active || (a.Project.Status != ProjectItemStatus.completed && a.Project.Availability != ProjectAvailability.Suspended && a.Project.Availability != ProjectAvailability.Draft));
                                break;
                            case ItemListStatus.Completed:
                                query = query.Where(a => (a.Project.Status == ProjectItemStatus.completed && a.Project.Availability != ProjectAvailability.Draft));
                                break;
                            case ItemListStatus.Future:
                                query = query.Where(a => (a.Project.Status != ProjectItemStatus.completed && a.Project.Availability != ProjectAvailability.Draft && a.Project.StartDate.Value.Ticks > DateTime.Now.Date.Ticks));
                                break;
                        }
                        results = query;
                    }
                    catch (Exception ex)
                    {

                    }
                    return results;
                }
                public List<dtoTimelineActivity> GetTimeLineActivitiesToManage(Int32 idPerson, long idProject = 0, ProjectFilterBy filter = ProjectFilterBy.All, ItemListStatus filterStatus = ItemListStatus.All, Int32 idCommunity = -100)
                {
                    List<dtoTimelineActivity> results = new List<dtoTimelineActivity>();
                    try
                    {
                        IEnumerable<ProjectResource> query = (from r in Manager.GetIQ<ProjectResource>()
                                                           where r.Deleted == BaseStatusDeleted.None && r.Person != null && r.Person.Id == idPerson && (idProject < 1 || (idProject > 0 && r.Project != null && r.Project.Id == idProject))
                                                           select r);


                        if (idProject < 1 && (filter != ProjectFilterBy.All || filterStatus != ItemListStatus.All || idCommunity != -100))
                        {
                            switch (filter)
                            {
                                case ProjectFilterBy.AllPersonal:
                                    query = query.Where(a => a.Project.isPersonal && a.ProjectRole== ActivityRole.ProjectOwner);
                                    break;
                                case ProjectFilterBy.AllPersonalFromCurrentCommunity:
                                    query = query.Where(a => !a.Project.isPortal && a.ProjectRole == ActivityRole.ProjectOwner && a.Project.isPersonal && a.Project.Community != null && a.Project.Community.Id == idCommunity);
                                    break;
                                case ProjectFilterBy.CurrentCommunity:
                                    query = query.Where(a => a.Project.Community != null && ((a.ProjectRole == ActivityRole.ProjectOwner && !a.Project.isPersonal) || a.ProjectRole == ActivityRole.Manager) && a.Project.Community.Id == idCommunity);
                                    break;
                                case ProjectFilterBy.FromPortal:
                                    query = query.Where(a => a.Project.isPortal && ((a.ProjectRole == ActivityRole.ProjectOwner && !a.Project.isPersonal) || a.ProjectRole == ActivityRole.Manager) );
                                    break;
                            }
                            switch (filterStatus)
                            {
                                case ItemListStatus.All:
                                    query = query.Where(a => a.Project.Availability != ProjectAvailability.Draft);
                                    break;
                                case ItemListStatus.Draft:
                                    query = query.Where(a => a.Project.Availability == ProjectAvailability.Draft);
                                    break;
                                case ItemListStatus.Active:
                                    query = query.Where(a => a.Project.Availability == ProjectAvailability.Active || (a.Project.Status != ProjectItemStatus.completed && a.Project.Availability != ProjectAvailability.Suspended && a.Project.Availability != ProjectAvailability.Draft));
                                    break;
                                case ItemListStatus.Completed:
                                    query = query.Where(a => (a.Project.Status == ProjectItemStatus.completed && a.Project.Availability != ProjectAvailability.Draft));
                                    break;
                                case ItemListStatus.Future:
                                    query = query.Where(a => (a.Project.Status != ProjectItemStatus.completed && a.Project.Availability != ProjectAvailability.Draft && a.Project.StartDate.Value.Ticks > DateTime.Now.Date.Ticks));
                                    break;
                            }
                        }
                        results = GetTimeLineActivitiesToManage(query.ToList(),idProject, PageListType.DashboardManager);
                    }
                    catch (Exception ex)
                    {

                    }
                    return results;
                }
            #endregion
            #region "TimeLine Calculate"
                private List<dtoTimelineActivity> GetTimeLineActivitiesToManage(List<ProjectResource> resources, long idProject,PageListType pageType)
                {
                    List<dtoTimelineActivity> items = new List<dtoTimelineActivity>();
                    if (resources.Any())
                    {
                        DateTime dNow = DateTime.Now.Date;
                        Dictionary<SummaryTimeLine, long> days = GetDaysOfYear(dNow);
                        //IEnumerable<PmActivity> query = resources.Select(a=> a.Project.Activities.Where(a=> (a.Completeness<100 || (!a.IsCompleted && a.Project.ConfirmCompletion)) && a.Deleted == BaseStatusDeleted.None && !a.IsSummary).SelectMany(a=> a));
                        IEnumerable<PmActivity> query = resources.Select(r => r.Project.Activities.Where(a => (a.Completeness<100 ||(!a.IsCompleted && a.Project.ConfirmCompletion)) && a.Deleted == BaseStatusDeleted.None && !a.IsSummary).AsEnumerable<PmActivity>()).SelectMany(a=> a);
                        foreach (SummaryTimeLine t in GetAvailableTimeLines(query, days))
                        {
                            switch (t)
                            {
                                case SummaryTimeLine.Today:
                                    items.Add(new dtoTimelineActivity() { Quantity = CountActivities(query, UserActivityStatus.Starting, t, days), Status = UserActivityStatus.Starting, TimeLine = t, DashboardPage = pageType, IdProject =idProject  });
                                    break;
                                default:
                                    items.Add(new dtoTimelineActivity() { Quantity = CountActivities(query, UserActivityStatus.ToDo, t, days), Status = UserActivityStatus.ToDo, TimeLine = t, DashboardPage = pageType, IdProject = idProject });
                                    items.Add(new dtoTimelineActivity() { Quantity = CountActivities(query, UserActivityStatus.Expired, t, days), Status = UserActivityStatus.Expired, TimeLine = t, DashboardPage = pageType, IdProject = idProject });
                                    items.Add(new dtoTimelineActivity() { Quantity = CountActivities(query, UserActivityStatus.Expiring, t, days), Status = UserActivityStatus.Expiring, TimeLine = t, DashboardPage = pageType, IdProject = idProject });
                                    break;
                            }
                        }
                    }
                    else
                    {
                        items.Add(new dtoTimelineActivity() { Quantity = 0, Status = UserActivityStatus.Starting, TimeLine = SummaryTimeLine.Today, DashboardPage = pageType, IdProject = idProject });
                        items.Add(new dtoTimelineActivity() { Quantity = 0, Status = UserActivityStatus.ToDo, TimeLine = SummaryTimeLine.Week, DashboardPage = pageType, IdProject = idProject });
                        items.Add(new dtoTimelineActivity() { Quantity = 0, Status = UserActivityStatus.Expired, TimeLine = SummaryTimeLine.Week, DashboardPage = pageType, IdProject = idProject });
                        items.Add(new dtoTimelineActivity() { Quantity = 0, Status = UserActivityStatus.Expiring, TimeLine = SummaryTimeLine.Week, DashboardPage = pageType, IdProject = idProject });
                        items.Add(new dtoTimelineActivity() { Quantity = 0, Status = UserActivityStatus.ToDo, TimeLine = SummaryTimeLine.Month, DashboardPage = pageType, IdProject = idProject });
                        items.Add(new dtoTimelineActivity() { Quantity = 0, Status = UserActivityStatus.Expired, TimeLine = SummaryTimeLine.Month, DashboardPage = pageType, IdProject = idProject });
                        items.Add(new dtoTimelineActivity() { Quantity = 0, Status = UserActivityStatus.Expiring, TimeLine = SummaryTimeLine.Month, DashboardPage = pageType, IdProject = idProject });
                    }
                    return items;
                }
                private long CountActivities(IEnumerable<PmActivity> qActivities, UserActivityStatus status, SummaryTimeLine timeline, Dictionary<SummaryTimeLine, long> days)
                {
                    IEnumerable<PmActivity> query;
                    switch (status)
                    {
                        case UserActivityStatus.Starting:
                            query = qActivities.Where(a => a.EarlyStartDate.HasValue && a.Completeness ==0 && a.EarlyStartDate.Value.Ticks >= days[SummaryTimeLine.Today] && a.EarlyStartDate.Value.Ticks <= days[timeline]);
                            break;
                        case UserActivityStatus.ToDo:
                            query = qActivities.Where(a => a.EarlyStartDate.HasValue && !a.IsCompleted && a.EarlyStartDate.Value.Ticks >= days[SummaryTimeLine.Today] && a.EarlyStartDate.Value.Ticks <= days[timeline]);
                            break;
                        case UserActivityStatus.Expired:
                            query = qActivities.Where(a => a.EarlyFinishDate.HasValue && !a.IsCompleted  && a.EarlyFinishDate.Value.Ticks < days[SummaryTimeLine.Today]);
                            break;
                        case UserActivityStatus.Expiring:
                            query = qActivities.Where(a => a.EarlyFinishDate.HasValue && !a.IsCompleted && a.EarlyFinishDate.Value.Ticks >= days[timeline] && a.EarlyFinishDate.Value.Ticks <= days[timeline]);
                            break;
                        default:
                            return 0;
                    }
                    return query.Select(a => a.Id).Count();
                }
                private List<dtoTimelineActivity> GetTimeLineActivities(IEnumerable<liteProjectActivityAssignment> assignments, long idProject, PageListType view)
                {
                    List<dtoTimelineActivity> items = new List<dtoTimelineActivity>();
                    if (assignments.Any()){
                        DateTime dNow = DateTime.Now.Date;
                        Dictionary<SummaryTimeLine, long> days = GetDaysOfYear(dNow);
                        foreach (SummaryTimeLine t in GetAvailableTimeLines(assignments, days))
                        {
                            try
                            {
                                switch (t)
                                {
                                    case SummaryTimeLine.Today:
                                        items.Add(new dtoTimelineActivity() { Quantity = CountActivities(assignments, UserActivityStatus.Starting, t, days), Status = UserActivityStatus.Starting, TimeLine = t, DashboardPage = view, IdProject = idProject });
                                        break;
                                    default:
                                        items.Add(new dtoTimelineActivity() { Quantity = CountActivities(assignments, UserActivityStatus.ToDo, t, days), Status = UserActivityStatus.ToDo, TimeLine = t, DashboardPage = view, IdProject = idProject });
                                        items.Add(new dtoTimelineActivity() { Quantity = CountActivities(assignments, UserActivityStatus.Expired, t, days), Status = UserActivityStatus.Expired, TimeLine = t, DashboardPage = view, IdProject = idProject });
                                        items.Add(new dtoTimelineActivity() { Quantity = CountActivities(assignments, UserActivityStatus.Expiring, t, days), Status = UserActivityStatus.Expiring, TimeLine = t, DashboardPage = view, IdProject = idProject });
                                        break;
                                }
                            }
                            catch (Exception ex) { 
                            
                            }
                        }
                    }
                    else{
                        items.Add(new dtoTimelineActivity() { Quantity = 0, Status = UserActivityStatus.Starting, TimeLine = SummaryTimeLine.Today, DashboardPage = view, IdProject = idProject });
                        items.Add(new dtoTimelineActivity() { Quantity = 0, Status = UserActivityStatus.ToDo, TimeLine = SummaryTimeLine.Week, DashboardPage = view, IdProject = idProject });
                        items.Add(new dtoTimelineActivity() { Quantity = 0, Status = UserActivityStatus.Expired, TimeLine = SummaryTimeLine.Week, DashboardPage = view, IdProject = idProject });
                        items.Add(new dtoTimelineActivity() { Quantity = 0, Status = UserActivityStatus.Expiring, TimeLine = SummaryTimeLine.Week, DashboardPage = view, IdProject = idProject });
                        items.Add(new dtoTimelineActivity() { Quantity = 0, Status = UserActivityStatus.ToDo, TimeLine = SummaryTimeLine.Month, DashboardPage = view, IdProject = idProject });
                        items.Add(new dtoTimelineActivity() { Quantity = 0, Status = UserActivityStatus.Expired, TimeLine = SummaryTimeLine.Month, DashboardPage = view, IdProject = idProject });
                        items.Add(new dtoTimelineActivity() { Quantity = 0, Status = UserActivityStatus.Expiring, TimeLine = SummaryTimeLine.Month, DashboardPage = view, IdProject = idProject });
                    }
                    return items;
                }
                private long CountActivities(IEnumerable<liteProjectActivityAssignment> assignments, UserActivityStatus status, SummaryTimeLine timeline, Dictionary<SummaryTimeLine, long> days)
                {
                    var query = (from a in assignments select a);
                    switch (status){
                        case UserActivityStatus.Starting:
                            query = query.Where(a => a.Activity.EarlyStartDate.HasValue && a.Completeness ==0 && a.Activity.EarlyStartDate.Value.Ticks >= days[SummaryTimeLine.Today] && a.Activity.EarlyStartDate.Value.Ticks <= days[timeline]);
                            break;
                        case UserActivityStatus.ToDo:
                            query = query.Where(a => a.Activity.EarlyStartDate.HasValue && (!a.IsApproved || a.Completeness < 100) && a.Activity.EarlyStartDate.Value.Ticks >= days[SummaryTimeLine.Today] && a.Activity.EarlyStartDate.Value.Ticks <= days[timeline]);
                            break;
                        case UserActivityStatus.Expired:
                            query = query.Where(a => a.Activity.EarlyFinishDate.HasValue && (!a.IsApproved || a.Completeness < 100) && a.Activity.EarlyFinishDate.Value.Ticks < days[SummaryTimeLine.Today]);
                            break;
                        case UserActivityStatus.Expiring:
                            query = query.Where(a => a.Activity.EarlyFinishDate.HasValue && (!a.IsApproved || a.Completeness < 100) && a.Activity.EarlyFinishDate.Value.Ticks >= days[SummaryTimeLine.Today] && a.Activity.EarlyFinishDate.Value.Ticks <= days[timeline]);
                            break;
                        default:
                            return 0;
                    }
                    return query.Select(a=> a.Id).Count();
                }
                
                private List<SummaryTimeLine> GetAvailableTimeLines(IEnumerable<PmActivity> query, Dictionary<SummaryTimeLine, long> days)
                {
                    return GetAvailableTimeLines(query.Where(a => a.EarlyStartDate.HasValue).Select(a => a.EarlyStartDate.Value).Union(query.Where(a => a.EarlyFinishDate.HasValue).Select(a => a.EarlyFinishDate.Value)).ToList(), days);
                }
                private List<SummaryTimeLine> GetAvailableTimeLines(IEnumerable<liteProjectActivityAssignment> assignments, Dictionary<SummaryTimeLine, long> days)
                {
                    return GetAvailableTimeLines(assignments.Where(a => a.Activity.EarlyStartDate.HasValue).Select(a => a.Activity.EarlyStartDate.Value ).Union(assignments.Where(a => a.Activity.EarlyFinishDate.HasValue).Select(a => a.Activity.EarlyFinishDate.Value)).Union(assignments.Where(a => a.Activity.Deadline.HasValue).Select(a => a.Activity.Deadline.Value)).ToList(),  days);
                }
                public List<SummaryTimeLine> GetAvailableTimeLines(List<DateTime> datetime, Dictionary<SummaryTimeLine, long> days)
                {
                    List<SummaryTimeLine> items = new List<SummaryTimeLine>();
                    DateTime dNow = DateTime.Now.Date;
               
                    items.Add(SummaryTimeLine.Today);
                    items.Add(SummaryTimeLine.Week);
                    items.Add(SummaryTimeLine.Month);
                    if (datetime.Any()){
                        List<long> daysOfYear = datetime.GroupBy(d => d.Date.Ticks).Where(d => d.Key > days[SummaryTimeLine.Today]).Select(d => d.Key).ToList();
                        //if (days.Any())
                        //    items.Add(SummaryTimeLine.Quarter);
                        if (daysOfYear.Where(d => d >= days[SummaryTimeLine.Month]).Any())
                            items.Add(SummaryTimeLine.HalfYear);
                        if (daysOfYear.Where(d => d >= days[SummaryTimeLine.HalfYear]).Any())
                            items.Add(SummaryTimeLine.Year);
                    }
                    return items;
                }
                private Dictionary<SummaryTimeLine, long> GetDaysOfYear(DateTime startDate) {
                    Dictionary<SummaryTimeLine, long> days = new Dictionary<SummaryTimeLine, long>();
                    days.Add(SummaryTimeLine.Today, startDate.Ticks);

                    int delta = Convert.ToInt32(startDate.DayOfWeek);
                    delta = delta == 0 ? delta + 7 : delta;
                    DateTime sunday = startDate.AddDays(7 - delta);

                    days.Add(SummaryTimeLine.Week, sunday.Ticks);
                    days.Add(SummaryTimeLine.Month, new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month)).Ticks);
                    days.Add(SummaryTimeLine.Quarter, GetDateTimeToLong(startDate.Month, startDate.Year, 3));
                    days.Add(SummaryTimeLine.HalfYear, GetDateTimeToLong(startDate.Month, startDate.Year, 6));
                    days.Add(SummaryTimeLine.Year, GetDateTimeToLong(startDate.Month, startDate.Year, 12));

                    //if (startDate.Month + 3 > 12)
                    //{

                    //    days.Add(SummaryTimeLine.HalfYear, new DateTime(startDate.Year, 6, DateTime.DaysInMonth(startDate.Year, 6)).Ticks);
                    //    days.Add(SummaryTimeLine.Year, new DateTime(startDate.Year, 12, DateTime.DaysInMonth(startDate.Year, 12)).Ticks);
                    //}
                    //else
                    //{
                    //    days.Add(SummaryTimeLine.HalfYear, new DateTime(startDate.Year, 6, DateTime.DaysInMonth(startDate.Year, 6)).Ticks);
                       
                    //}
                    //days.Add(SummaryTimeLine.Year, new DateTime(year, 12, DateTime.DaysInMonth(year, 12)).Ticks);
                    return days;
                }
                private long GetDateTimeToLong(Int32 month, Int32 year, Int32 addMonth)
                {
                    if (month + addMonth > 12)
                    {
                        month = (month + addMonth) - 12;
                        year++;
                    }
                    else
                        month += addMonth;
                    return new DateTime(year, month, DateTime.DaysInMonth(year, month)).Ticks;
                }
            #endregion

        #endregion

        #region "List"
            public String GetBackUrl(PageListType pageType, Int32 idContainerCommunity, long idProject) {
                switch (pageType) { 
                    case PageListType.Ignore:
                    case PageListType.None:
                        return "";
                    case PageListType.ListAdministrator:
                        return RootObject.ProjectListAdministrator(idContainerCommunity, (idContainerCommunity == 0), false, true, idProject);
                    case PageListType.ListManager:
                        return RootObject.ProjectListManager(idContainerCommunity, (idContainerCommunity == 0), false, true, idProject);
                    case PageListType.ListResource:
                        return RootObject.ProjectListResource(idContainerCommunity, (idContainerCommunity == 0), false, true, idProject);
                    
                    //case PageListType.DashboardManager:
                    //    return RootObject.ProjectListManager(idContainerCommunity, (idContainerCommunity == 0), false, false, idProject, ItemsGroupBy.Plain, ProjectFilterBy.);
                    //case PageListType.DashboardResource:
                    //case PageListType.DashboardAdministrator:

                    default:
                        return "";
                }   
            }
            public List<TabListItem> GetAvailableTabs(Int32 idPerson, dtoProjectContext cContext, PageContainerType containerType)
            {
                List<TabListItem> tabs = new List<TabListItem>() { TabListItem.Resource };
                Person p = Manager.GetPerson(idPerson);
                if (HasProjectToManage(idPerson, ItemDeleted.Ignore))
                    tabs.Add(TabListItem.Manager);
                if (containerType == PageContainerType.ProjectsList)
                {
                    ModuleProjectManagement mPermission = (cContext.isForPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(Manager.GetModulePermission(idPerson, cContext.IdCommunity, GetIdModule()));
                    if (mPermission.Administration)
                        tabs.Add(TabListItem.Administration);
                    else if (HasOtherProjectsToManage(idPerson))
                        tabs.Add(TabListItem.Administration);
                }
                else if (containerType == PageContainerType.ProjectDashboard && !tabs.Contains(TabListItem.Manager)) {
                    ModuleProjectManagement mPermission = (cContext.isForPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(Manager.GetModulePermission(idPerson, cContext.IdCommunity, GetIdModule()));
                    if (mPermission.Administration)
                        tabs.Add(TabListItem.Manager);
                }
                return tabs;
            }
            public List<ProjectFilterBy> GetAvailableFilterBy(Int32 idPerson, dtoProjectContext context, PageListType pageType)
            {
                List<ProjectFilterBy> items = new List<ProjectFilterBy>();
                items.Add(ProjectFilterBy.All);

                if (pageType != PageListType.ListAdministrator && HasPersonalProject(idPerson, ItemDeleted.Ignore))
                    items.Add(ProjectFilterBy.AllPersonal);
                if (context.IdCommunity > 0)
                {
                    Community c = Manager.GetCommunity(context.IdCommunity);
                    if (c != null)
                    {
                        if (pageType != PageListType.ListAdministrator && HasPersonalProject(idPerson, ItemDeleted.Ignore, context.IdCommunity))
                            items.Add(ProjectFilterBy.AllPersonalFromCurrentCommunity);
                        items.Add(ProjectFilterBy.CurrentCommunity);
                    }
                }
                items.Add(ProjectFilterBy.FromPortal);
                return items;
            }
            public List<ItemListStatus> GetAvailableFilterStatus(Person p, dtoProjectContext context, dtoItemsFilter filter, PageContainerType containerType,PageListType pageType, SummaryTimeLine timeline, long idProject =0)
            {
                List<ItemListStatus> items = new List<ItemListStatus>() { ItemListStatus.All, ItemListStatus.Active, ItemListStatus.Completed, ItemListStatus.Future };
                switch (containerType)
                {
                    case PageContainerType.ProjectDashboard:
                    case PageContainerType.Dashboard:
                        if (UserHasActivitiesDueForCompletion(p, context,filter,pageType ,  timeline, idProject))
                            items.Insert(2,ItemListStatus.DueForCompletion);
                        if (UserHasLateActivities(p, context, filter, pageType , timeline, idProject))
                            items.Insert((items.Contains(ItemListStatus.DueForCompletion) ? 3 : 2),ItemListStatus.Late);
                        break;
                    case PageContainerType.ProjectsList:
                        #region "ProjectList"
                        switch (pageType)
                        {
                            case PageListType.ListManager:
                                items.Insert(1, ItemListStatus.Draft);
                                switch (filter.FilterBy)
                                {
                                    case ProjectFilterBy.All:
                                        if (HasProjectToManage(p.Id, ItemDeleted.Yes, -1))
                                            items.Add(ItemListStatus.Deleted);
                                        break;
                                    case ProjectFilterBy.AllPersonal:
                                        if (HasPersonalProject(p.Id, ItemDeleted.Yes))
                                            items.Add(ItemListStatus.Deleted);
                                        break;
                                    case ProjectFilterBy.AllPersonalFromCurrentCommunity:
                                        if (HasPersonalProject(p.Id, ItemDeleted.Yes, context.IdCommunity))
                                            items.Add(ItemListStatus.Deleted);
                                        break;
                                    case ProjectFilterBy.CurrentCommunity:
                                        if (HasProjectToManage(p.Id, ItemDeleted.Yes, context.IdCommunity))
                                            items.Add(ItemListStatus.Deleted);
                                        break;
                                    case ProjectFilterBy.FromPortal:
                                        if (HasProjectToManage(p.Id, ItemDeleted.Yes, 0))
                                            items.Add(ItemListStatus.Deleted);
                                        break;
                                }
                                break;
                            case PageListType.ListAdministrator:
                                items.Insert(1, ItemListStatus.Draft);
                                switch (filter.FilterBy)
                                {
                                    case ProjectFilterBy.All:
                                        if (HasOtherProjectsToManage(p, ItemDeleted.Yes, -1))
                                            items.Add(ItemListStatus.Deleted);
                                        break;
                                    case ProjectFilterBy.CurrentCommunity:
                                        if (HasOtherProjectsToManage(p, ItemDeleted.Yes, context.IdCommunity))
                                            items.Add(ItemListStatus.Deleted);
                                        break;
                                    case ProjectFilterBy.FromPortal:
                                        if (HasOtherProjectsToManage(p, ItemDeleted.Yes, 0))
                                            items.Add(ItemListStatus.Deleted);
                                        break;
                                }
                                break;
                        }
                        #endregion
                        break;
                }
                return items;
            }
            public List<dtoPlainProject> GetProjects(Int32 idPerson, dtoItemsFilter filter, PageListType view, Int32 idCommunity, Dictionary<ActivityRole, String> roleTranslations)
            {
                List<dtoPlainProject> projects = new List<dtoPlainProject>();
                try
                {
                    Person person = Manager.GetPerson(idPerson);
                    switch (view)
                    {
                        case PageListType.ListManager:
                            projects = GetProjectsAsManager(idPerson, filter, idCommunity, roleTranslations);
                            if (projects.Any()){
                                Dictionary<ActivityRole, PmActivityPermission> permissions = projects.Select(p => p.GetMajorRole(view)).Distinct().ToDictionary(r => r, r => GetRolePermissions(r));
                                foreach (dtoPlainProject project in projects) {
                                    project.UpdatePermissions(permissions[project.GetMajorRole(view)],view);
                                }
                            }
                            
                            break;
                        case PageListType.ListAdministrator:
                            projects = GetProjectsAsAdministrator(person, filter, idCommunity, roleTranslations);
                            break;
                        case PageListType.ListResource:
                            projects = GetProjectsAsResource(idPerson, filter, idCommunity, roleTranslations);
                            break;
                    }
                    switch (filter.GroupBy) { 
                        case ItemsGroupBy.Plain:
                            switch (filter.OrderBy) { 
                                case ProjectOrderBy.Name:
                                    if (filter.Ascending)
                                        projects = projects.OrderBy(p => p.Name).ThenByDescending(p => p.Deadline).ThenByDescending(p => p.EndDate).ToList();
                                    else
                                        projects = projects.OrderByDescending(p => p.Name).ThenByDescending(p => p.Deadline).ThenByDescending(p => p.EndDate).ToList();
                                    break;
                                case ProjectOrderBy.Completion:
                                    if (filter.Ascending)
                                        projects = projects.OrderBy(p => p.Completeness).ThenBy(p => p.Name).ToList();
                                    else
                                        projects = projects.OrderByDescending(p => p.IsCompleted).ThenByDescending(p => p.Completeness).ThenBy(p => p.Name).ToList();
                                    break;
                                case ProjectOrderBy.Deadline:
                                    if (filter.Ascending)
                                        projects = projects.OrderBy(p => p.VirtualDeadline).ThenBy(p => p.Name).ToList();
                                    else
                                        projects = projects.OrderByDescending(p => p.VirtualDeadline).ThenBy(p => p.Name).ToList();
                                    break;
                                case ProjectOrderBy.EndDate:
                                    if (filter.Ascending)
                                        projects = projects.OrderBy(p => p.VirtualEndDate).ThenBy(p => p.Name).ToList();
                                    else
                                        projects = projects.OrderByDescending(p => p.VirtualEndDate).ThenBy(p => p.Name).ToList();
                                    break;
                            }
                            break;
                        case ItemsGroupBy.EndDate:

                            if (filter.Ascending)
                                projects = projects.OrderBy(p => p.VirtualEndDate).ThenBy(p => p.Name).ToList();
                            else
                                projects = projects.OrderByDescending(p => p.VirtualEndDate).ThenBy(p => p.Name).ToList();
                   
                            break;
                    }
                }
                catch (Exception ex)
                {

                }
                return projects;
            }

            private List<dtoPlainProject> GetProjectsAsResource(Int32 idPerson, dtoItemsFilter filter, Int32 idCommunity, Dictionary<ActivityRole, String> roleTranslations)
            {
                List<dtoPlainProject> projects = new List<dtoPlainProject>();
                try
                {
                    IEnumerable<ProjectResource> query = GetProjectAsResourceByUser(idPerson, filter, idCommunity);
                    foreach (ProjectResource resource in query)
                    {
                        projects.Add(dtoPlainProject.CreateForResource(resource.Project, resource, roleTranslations, GetRolePermissions(resource.ProjectRole)));
                    }
                }
                catch (Exception ex)
                {

                }
                return projects;
            }
            private IEnumerable<ProjectResource> GetProjectAsResourceByUser(Int32 idPerson, dtoItemsFilter filter, Int32 idCommunity)
            {
                List<long> idProjects = (from r in Manager.GetIQ<ProjectActivityAssignment>()
                                            where r.Deleted == BaseStatusDeleted.None && r.Person != null && r.Person.Id == idPerson
                                            select r.Project.Id).Distinct().ToList();

                IEnumerable<ProjectResource> query = (from r in Manager.GetIQ<ProjectResource>()
                                                      where r.Deleted == BaseStatusDeleted.None && r.Type == ResourceType.Internal && r.Person != null && r.Person.Id == idPerson
                                                     && (idProjects.Contains(r.Project.Id))
                                                      select r);
                switch (filter.FilterBy)
                {
                    case ProjectFilterBy.AllPersonal:
                        query = query.Where(r => r.Project != null && r.Project.isPersonal && r.ProjectRole == ActivityRole.ProjectOwner);
                        break;
                    case ProjectFilterBy.AllPersonalFromCurrentCommunity:
                        query = query.Where(r => r.Project != null && r.Project.isPersonal && !r.Project.isPortal && r.Project.Community != null && r.Project.Community.Id == idCommunity && r.ProjectRole == ActivityRole.ProjectOwner);
                        break;
                    case ProjectFilterBy.CurrentCommunity:
                        query = query.Where(r => r.Project != null && !r.Project.isPortal && r.Project.Community != null && r.Project.Community.Id == idCommunity);
                        break;
                    case ProjectFilterBy.FromPortal:
                        query = query.Where(r => r.Project != null && r.Project.isPortal && !r.Project.isPersonal);
                        break;
                }
                switch (filter.ProjectsStatus)
                {
                    case ItemListStatus.All:
                        query = query.Where(r => r.Project.Availability != ProjectAvailability.Draft);
                        break;
                    case ItemListStatus.Future:
                        query = query.Where(r => (r.Project.Status != ProjectItemStatus.completed && r.Project.Availability != ProjectAvailability.Draft && r.Project.StartDate.Value.Ticks > DateTime.Now.Date.Ticks));
                        break;
                    case ItemListStatus.Active:
                        query = query.Where(r => r.Project != null && !r.Project.IsCompleted && r.Project.Availability == ProjectAvailability.Active);
                        break;
                    case ItemListStatus.Completed:
                        query = query.Where(r => r.Project != null && r.Project.IsCompleted);
                        break;
                }
                return query;
            }
            private List<dtoPlainProject> GetProjectsAsManager(Int32 idPerson, dtoItemsFilter filter, Int32 idCommunity, Dictionary<ActivityRole, String> roleTranslations)
            {
                List<dtoPlainProject> projects = new List<dtoPlainProject>();
                try
                {
                    IEnumerable<ProjectResource> query = GetResourcesAsManager(idPerson, filter, idCommunity);
                    foreach (ProjectResource r in query)
                    {
                        projects.Add(new dtoPlainProject(r.Project, r.ProjectRole, r.Visibility, roleTranslations, r.ProjectRole != ActivityRole.Resource && (from a in Manager.GetIQ<liteProjectActivityAssignment>() where a.Deleted == BaseStatusDeleted.None && a.IdPerson == idPerson && a.Project != null && a.Project.Id == r.Project.Id select a).Any()));
                    }
                }
                catch (Exception ex)
                {

                }
                return projects;
            }
            private IEnumerable<ProjectResource> GetResourcesAsManager(Int32 idPerson, dtoItemsFilter filter, Int32 idCommunity, Boolean applyFilterByStatus = true )
            {
                IEnumerable<ProjectResource> query = (from r in Manager.GetIQ<ProjectResource>()
                                                        where
                                                        ((filter.ProjectsStatus == ItemListStatus.Deleted && r.Project.Deleted == BaseStatusDeleted.Manual && r.Deleted == BaseStatusDeleted.Cascade) || (filter.ProjectsStatus != ItemListStatus.Deleted && r.Deleted == BaseStatusDeleted.None)) && r.Type == ResourceType.Internal && r.Person != null && r.Person.Id == idPerson
                                                        && (r.ProjectRole == ActivityRole.ProjectOwner || r.ProjectRole == ActivityRole.Manager)
                                                        && (filter.IdProject <=0 || (filter.IdProject > 0 && r.Project.Id== filter.IdProject))
                                                        select r);
                switch (filter.FilterBy)
                {
                    case ProjectFilterBy.AllPersonal:
                        query = query.Where(r => r.Project != null && r.Project.isPersonal && r.ProjectRole == ActivityRole.ProjectOwner);
                        break;
                    case ProjectFilterBy.AllPersonalFromCurrentCommunity:
                        query = query.Where(r => r.Project != null && r.Project.isPersonal && !r.Project.isPortal && r.Project.Community != null && r.Project.Community.Id == idCommunity && r.ProjectRole == ActivityRole.ProjectOwner);
                        break;
                    case ProjectFilterBy.CurrentCommunity:
                        query = query.Where(r => r.Project != null && !r.Project.isPortal && r.Project.Community != null && r.Project.Community.Id == idCommunity);
                        break;
                    case ProjectFilterBy.FromPortal:
                        query = query.Where(r => r.Project != null && r.Project.isPortal && !r.Project.isPersonal);
                        break;
                }
                if (applyFilterByStatus)
                {
                    switch (filter.ProjectsStatus)
                    {
                        case ItemListStatus.Draft:
                            query = query.Where(r => r.Project != null && r.Project.Availability == ProjectAvailability.Draft);
                            break;
                        case ItemListStatus.Future:
                            query = query.Where(r => (r.Project.Status != ProjectItemStatus.completed && r.Project.Availability != ProjectAvailability.Draft && r.Project.StartDate.Value.Ticks > DateTime.Now.Date.Ticks));
                            break;
                        case ItemListStatus.Active:
                            query = query.Where(r => r.Project != null && !r.Project.IsCompleted && r.Project.Availability == ProjectAvailability.Active);
                            break;
                        case ItemListStatus.Completed:
                            query = query.Where(r => r.Project != null && r.Project.IsCompleted);
                            break;
                    }
                }
                else
                    query = query.Where(r => r.Project != null && r.Project.Availability == ProjectAvailability.Active);
                return query;
            }
            private List<dtoPlainProject> GetProjectsAsAdministrator(Person person, dtoItemsFilter filter, Int32 idCommunity, Dictionary<ActivityRole, String> roleTranslations)
            {
                List<dtoPlainProject> projects = new List<dtoPlainProject>();
                List<long> idProjects = (from r in Manager.GetIQ<ProjectResource>()
                                            where r.Type == ResourceType.Internal && r.Person != null && r.Person.Id == person.Id
                                                && (r.ProjectRole == ActivityRole.ProjectOwner || r.ProjectRole == ActivityRole.Manager)
                                            select r.Project.Id).ToList();

                var query = (from p in Manager.GetIQ<Project>()
                             where ((filter.ProjectsStatus == ItemListStatus.Deleted && p.Deleted == BaseStatusDeleted.Manual) || (filter.ProjectsStatus != ItemListStatus.Deleted && p.Deleted == BaseStatusDeleted.None))
                             && p.isPersonal == false && !idProjects.Contains(p.Id)
                                select p);
                switch (filter.ProjectsStatus)
                {
                    case ItemListStatus.Future:
                        query = query.Where(p => (p.Status != ProjectItemStatus.completed && p.Availability != ProjectAvailability.Draft && p.StartDate.Value.Ticks > DateTime.Now.Date.Ticks));
                        break;
                    case ItemListStatus.Active:
                        query = query.Where(p => !p.IsCompleted && p.Availability == ProjectAvailability.Active);
                        break;
                    case ItemListStatus.Draft:
                        query = query.Where(p => p.Availability == ProjectAvailability.Draft);
                        break;
                    case ItemListStatus.Completed:
                        query = query.Where(p => p.IsCompleted);
                        break;
                }

                ModuleProjectManagement pPermissions = ModuleProjectManagement.CreatePortalmodule(person.TypeID);
                ModuleProjectManagement cPermissions = new ModuleProjectManagement() { Administration = true };
                switch (filter.FilterBy)
                {
                    case ProjectFilterBy.All:
                        
                        if (pPermissions.Administration)
                        {
                            foreach (Project project in query.Where(p => p.isPortal).ToList())
                            {
                                projects.Add(new dtoPlainProject(pPermissions, project, ProjectVisibility.Full, roleTranslations, (from a in Manager.GetIQ<liteProjectActivityAssignment>() where a.Deleted == BaseStatusDeleted.None && a.IdPerson == person.Id && a.Project != null && a.Project.Id == project.Id select a).Any()));
                            }
                        }
                        List<Int32> idCommunities = ServiceCommunityManagement.GetIdCommunityByModulePermissions(person.Id, new Dictionary<int, long>() { { GetIdModule(), cPermissions.GetPermissions() } });
                        if (idCommunities.Any())
                        {
                            foreach (Project project in query.Where(p => !p.isPortal && p.Community != null && idCommunities.Contains(p.Community.Id)).ToList())
                            {
                                projects.Add(new dtoPlainProject(cPermissions, project, ProjectVisibility.Full, roleTranslations, (from a in Manager.GetIQ<liteProjectActivityAssignment>() where a.Deleted == BaseStatusDeleted.None && a.IdPerson == person.Id && a.Project != null && a.Project.Id == project.Id select a).Any()));
                            }
                        }
                        break;
                    case ProjectFilterBy.CurrentCommunity:
                        cPermissions = new ModuleProjectManagement(Manager.GetModulePermission(person.Id, idCommunity, idModule));
                        if (cPermissions.Administration)
                        {
                            foreach (Project project in query.Where(p => !p.isPortal && p.Community != null && p.Community.Id == idCommunity).ToList())
                            {
                                projects.Add(new dtoPlainProject(cPermissions, project, ProjectVisibility.Full, roleTranslations, (from a in Manager.GetIQ<liteProjectActivityAssignment>() where a.Deleted == BaseStatusDeleted.None && a.IdPerson == person.Id && a.Project != null && a.Project.Id == project.Id select a).Any()));
                            }
                        }
                        break;
                    case ProjectFilterBy.FromPortal:
                        if (pPermissions.Administration)
                        {
                            foreach (Project project in query.Where(p => p.isPortal).ToList())
                            {
                                projects.Add(new dtoPlainProject(pPermissions, project, ProjectVisibility.Full, roleTranslations, (from a in Manager.GetIQ<liteProjectActivityAssignment>() where a.Deleted == BaseStatusDeleted.None && a.IdPerson == person.Id && a.Project != null && a.Project.Id == project.Id select a).Any()));
                            }
                        }
                        break;
                }
                return projects;
            }

            #region "TimeGroups"
                public List<dtoTimeGroup> GenerateTimeGroups() {
                    return GenerateTimeGroups(DateTime.Now.Date);
                }
                public List<dtoTimeGroup> GenerateTimeGroups(DateTime sDate)
                {
                    List<dtoTimeGroup> items = new List<dtoTimeGroup>();
                    long maxTicks = DateTime.MaxValue.Ticks;
                    long minTicks = DateTime.MinValue.Ticks;
                    long dTicks = sDate.Ticks;
                    items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.Today, FromTicks = dTicks, ToTicks = dTicks, Month = sDate.Month, Year = sDate.Year, FromDate =sDate, ToDate = sDate  });

                    items.AddRange(GenerateTimeGroupsForNext(sDate,dTicks));
                    items.AddRange(GenerateTimeGroupsForPrevious(sDate,dTicks-1));          
                    return items;
                }
                private List<dtoTimeGroup> GenerateTimeGroupsForPrevious(DateTime sDate, long dTicks)
                {
                    List<dtoTimeGroup> items = new List<dtoTimeGroup>();
                    int delta = Convert.ToInt32(sDate.DayOfWeek);
                    if (sDate.DayOfWeek == DayOfWeek.Sunday)
                        delta = -6;
                    else
                        delta = -delta + 1;
                    
                    DateTime lastDay = sDate.AddDays( delta);
                    if (lastDay.DayOfYear != sDate.DayOfYear)
                    {
                        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.PreviousDays, FromTicks = lastDay.Ticks, ToTicks = dTicks, Year = lastDay.Year, Month = lastDay.Month, FromDate = lastDay, ToDate = sDate.AddDays(-1) });
                        dTicks = lastDay.Ticks - 1;
                    }
                    lastDay = lastDay.AddDays(-7);
                    if (lastDay.Month == sDate.Month)
                    {
                        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.PreviousWeek, FromTicks = lastDay.Ticks, ToTicks = dTicks, Year = lastDay.Year, Month = lastDay.Month, FromDate = lastDay, ToDate = lastDay.AddDays(+6) });
                        dTicks = lastDay.Ticks - 1;
                        if (lastDay.Day != 1)
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousWeeks, sDate.Year, sDate.Month, ref dTicks));
                    }
                    else if (lastDay.Month ==12 && sDate.Month == 1) {
                        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.PreviousWeek, FromTicks = lastDay.Ticks, ToTicks = dTicks, Year = lastDay.Year, Month = lastDay.Month, FromDate = lastDay, ToDate = new DateTime(dTicks+1) });
                        dTicks = lastDay.Ticks - 1;
                    }
                    if (lastDay.Year == sDate.Year && sDate.Month > 1)
                        items.AddRange(GenerateTimeGroupsForPreviousMonth((lastDay.Month == sDate.Month) ? lastDay.Month - 1 : lastDay.Month, lastDay.Year, dTicks));
                    else
                        items.AddRange(GenerateTimeGroupsForPreviousYear(sDate.Year - 1, dTicks, true));
                    return items;
                }
                private List<dtoTimeGroup> GenerateTimeGroupsForPreviousYear(Int32 year, long dTicks, Boolean addLastMonthOfYear = false)
                {
                    List<dtoTimeGroup> items = new List<dtoTimeGroup>();
                    long minTicks = DateTime.MinValue.Ticks;
                    DateTime lastDay = DateTime.Now;
                    if (addLastMonthOfYear)
                        items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousMonth, year, 12,ref  dTicks));
                    items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousQuarter, year, 9, ref dTicks));
                    items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousHalfYear, year, 6, ref dTicks));
                    items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousYear, year, 1, ref dTicks));
                    items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousYears, year - 1, 12, ref dTicks, minTicks));
                
                    return items;
                }
                private List<dtoTimeGroup> GenerateTimeGroupsForPreviousMonth(Int32 month, Int32 year, long dTicks)
                {
                    List<dtoTimeGroup> items = new List<dtoTimeGroup>();
                    long minTicks = DateTime.MinValue.Ticks;
                    DateTime lastDay = DateTime.Now;
                    switch (month)
                    {
                        case 1:
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousMonth, year, month, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousYears, year - 1, 12, ref dTicks, minTicks));
                            break;
                        case 2:
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousMonth, year, month, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousDaysInQuarter, year, 1, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousYears, year - 1, 12, ref dTicks, minTicks));
                            break;
                        case 3:
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousMonth, year, month, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousDaysInQuarter, year, 1, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousYears, year - 1, 12, ref dTicks, minTicks));
                            break;
                        case 4:
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousMonth, year, month, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousQuarter, year, 1, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousYears, year - 1, 12, ref dTicks, minTicks));
                            break;
                        case 5:
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousMonth, year, month, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousDaysInQuarter, year, 4, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousQuarter, year, 1, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousYears, year - 1, 12, ref dTicks, minTicks));
                            break;
                        case 6:
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousMonth, year, month, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousDaysInQuarter, year, 4, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousQuarter, year, 1, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousYears, year - 1, 12, ref dTicks, minTicks));
                            break;
                        case 7:
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousMonth, year, month, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousQuarter, year, 4, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousYear, year, 1, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousYears, year-1, 12, ref dTicks, minTicks));
                            break;
                        case 8:
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousMonth, year, month, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousDaysInQuarter, year, 7, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousQuarter, year, 4, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousHalfYear, year,1, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousYears, year - 1, 12, ref dTicks, minTicks));
                            break;
                        case 9:
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousMonth, year, month, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousDaysInQuarter, year, 7, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousQuarter, year, 4, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousHalfYear, year,1, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousYears, year - 1, 12, ref dTicks, minTicks));
                            break;
                        case 10:
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousMonth, year, month, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousDaysInHalfYear, year, 6, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousYear, year, 1, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousYears, year-1, 12, ref dTicks, minTicks));
                            break;
                        case 11:
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousMonth, year, month, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousDaysInQuarter, year, 10, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousHalfYear, year, 6, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousYear, year, 1, ref dTicks));
                            items.Add(CreatePreviousTimeGroup(TimeGroup.PreviousYears, year-1, 12, ref dTicks, minTicks));
                            break;
                    }
                    return items;
                }
                private List<dtoTimeGroup> GenerateTimeGroupsForNext(DateTime sDate, long dTicks)
                {
                    List<dtoTimeGroup> items = new List<dtoTimeGroup>();
                    int delta = Convert.ToInt32(sDate.DayOfWeek);
                    delta = delta == 0 ? delta + 7 : delta;
                    DateTime lastDay = sDate;
                    if (delta != 7)
                    {
                        lastDay = sDate.AddDays(7 - delta);
                        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.ThisWeek, FromTicks = dTicks + 1, ToTicks = lastDay.Ticks, Month = lastDay.Month, Year = lastDay.Year, FromDate = sDate.AddDays(1), ToDate = lastDay });
                    }
                    dTicks = lastDay.Ticks + 1;
                    lastDay = lastDay.AddDays(7);
                    items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.NextWeek, FromTicks = dTicks, ToTicks = lastDay.Ticks, Month = lastDay.Month, Year = lastDay.Year, ToDate = lastDay, FromDate = lastDay.AddDays(-6) });
                    dTicks = lastDay.Ticks + 1;
                    if (lastDay.Year == sDate.Year && sDate.Month < 12)
                    {
                        items.Add(CreateNextTimeGroup(TimeGroup.ThisMonth, sDate.Year, sDate.Month, ref dTicks));
                        items.AddRange(GenerateTimeGroupsForMonth(lastDay.Month, lastDay.Year, dTicks));
                    }
                    else
                        items.AddRange(GenerateTimeGroupsForYear(sDate.Year + 1, dTicks, true));
                    return items;
                }
                private List<dtoTimeGroup> GenerateTimeGroupsForYear(Int32 year, long dTicks, Boolean addFirstMonthOfYear = false)
                {
                    List<dtoTimeGroup> items = new List<dtoTimeGroup>();
                    long maxTicks = DateTime.MaxValue.Ticks;
                    DateTime lastDay = DateTime.Now;
                    if (addFirstMonthOfYear)
                        items.Add(CreateNextTimeGroup(TimeGroup.NextMonth, year, 1, ref dTicks));
                    items.Add(CreateNextTimeGroup(TimeGroup.NextQuarter, year, 3, ref dTicks));
                    items.Add(CreateNextTimeGroup(TimeGroup.NextHalfYear, year, 6, ref dTicks));
                    items.Add(CreateNextTimeGroup(TimeGroup.NextYears, year, 12, ref dTicks, maxTicks));
                    return items;
                }
                private List<dtoTimeGroup> GenerateTimeGroupsForMonth(Int32 month, Int32 year, long dTicks)
                {
                    List<dtoTimeGroup> items = new List<dtoTimeGroup>();
                    long maxTicks = DateTime.MaxValue.Ticks;
                    DateTime lastDay = DateTime.Now;
                    switch (month)
                    {
                        case 1:
                            items.Add(CreateNextTimeGroup(TimeGroup.NextMonth, year, month + 1, ref dTicks));
                            items.Add(CreateNextTimeGroup(TimeGroup.ThisQuarter, year, 3, ref dTicks));
                            items.AddRange(GenerateTimeGroupsForMonth(3, year, dTicks));
                            break;
                        case 2:
                            items.Add(CreateNextTimeGroup(TimeGroup.NextMonth, year, month + 1, ref dTicks));
                            items.AddRange(GenerateTimeGroupsForMonth(3, year, dTicks));
                            break;
                        case 3:
                            items.Add(CreateNextTimeGroup(TimeGroup.NextQuarter, year, 6, ref dTicks));
                            items.Add(CreateNextTimeGroup(TimeGroup.NextHalfYear, year, 12, ref dTicks));
                            items.Add(CreateNextTimeGroup(TimeGroup.NextYears, year + 1, 1, ref dTicks, maxTicks));
                            break;
                        case 4:
                            items.Add(CreateNextTimeGroup(TimeGroup.NextMonth, year, month + 1, ref dTicks));
                            items.Add(CreateNextTimeGroup(TimeGroup.ThisQuarter, year, 6, ref dTicks));
                            items.Add(CreateNextTimeGroup(TimeGroup.NextQuarter, year, 9, ref dTicks));
                            items.Add(CreateNextTimeGroup(TimeGroup.NextHalfYear, year, 12, ref dTicks));
                            items.Add(CreateNextTimeGroup(TimeGroup.NextYears, year + 1, 1, ref dTicks, maxTicks));
                            break;
                        case 5:
                            items.Add(CreateNextTimeGroup(TimeGroup.NextMonth, year, month + 1, ref dTicks));
                            items.Add(CreateNextTimeGroup(TimeGroup.NextQuarter, year, 9, ref dTicks));
                            items.Add(CreateNextTimeGroup(TimeGroup.ThisYear, year, 12, ref dTicks));
                            items.Add(CreateNextTimeGroup(TimeGroup.NextYears, year + 1, 1, ref dTicks, maxTicks));
                            break;
                        case 6:
                            items.Add(CreateNextTimeGroup(TimeGroup.NextQuarter, year, 9, ref dTicks));
                            items.Add(CreateNextTimeGroup(TimeGroup.NextHalfYear, year, 12, ref dTicks));
                            items.Add(CreateNextTimeGroup(TimeGroup.NextYears, year + 1, 1, ref dTicks, maxTicks));
                            break;
                        case 7:
                            items.Add(CreateNextTimeGroup(TimeGroup.NextMonth, year, month + 1, ref dTicks));
                            items.Add(CreateNextTimeGroup(TimeGroup.ThisQuarter, year, 9, ref dTicks));
                            items.AddRange(GenerateTimeGroupsForMonth(9, year, dTicks));
                            break;
                        case 8:
                            items.Add(CreateNextTimeGroup(TimeGroup.NextMonth, year, month + 1, ref dTicks));
                            items.AddRange(GenerateTimeGroupsForMonth(9, year, dTicks));
                            break;
                        case 9:
                            items.Add(CreateNextTimeGroup(TimeGroup.NextQuarter, year, 12, ref dTicks));
                            items.Add(CreateNextTimeGroup(TimeGroup.NextYears, year + 1, 1, ref dTicks, maxTicks));
                            break;
                        case 10:
                            items.Add(CreateNextTimeGroup(TimeGroup.ThisQuarter, year, 12, ref dTicks));
                            items.Add(CreateNextTimeGroup(TimeGroup.NextYears, year + 1, month, ref dTicks, maxTicks));
                            break;
                        case 11:
                            items.Add(CreateNextTimeGroup(TimeGroup.NextMonth, year, month + 1, ref dTicks));
                            items.Add(CreateNextTimeGroup( TimeGroup.NextYears, year + 1, month, ref dTicks, maxTicks));
                            break;
                        case 12:
                            items.AddRange(GenerateTimeGroupsForYear(year + 1, dTicks, false));
                            break;
                    }
                    return items;
                }

                private dtoTimeGroup CreateNextTimeGroup(TimeGroup tGroup,Int32 year, Int32 month, ref long dTicks, long maxTicks=0)
                {
                    DateTime lastDay = (maxTicks>0) ? DateTime.MaxValue : new DateTime(year, month, DateTime.DaysInMonth(year, month));
                    dtoTimeGroup item = new dtoTimeGroup() { TimeLine = tGroup, FromTicks = dTicks, ToTicks = (maxTicks == 0) ? lastDay.Ticks : maxTicks, Month = month, Year = year, FromDate =new DateTime(dTicks), ToDate = lastDay };

                    dTicks = lastDay.Ticks + 1;
                    return item;
                }
                private dtoTimeGroup CreatePreviousTimeGroup(TimeGroup tGroup, Int32 year, Int32 month, ref long dTicks, long minTicks = -1)
                {
                    DateTime lastDay = (minTicks != -1) ? DateTime.MinValue : new DateTime(year, month, 1);
                    dtoTimeGroup item = new dtoTimeGroup() { TimeLine = tGroup, FromTicks = (minTicks == -1) ? lastDay.Ticks : minTicks, ToTicks = dTicks, Month = month, Year = year, FromDate = lastDay , ToDate = new DateTime(dTicks)};

                    dTicks = lastDay.Ticks -1;
                    return item;
                }
                //public List<dtoTimeGroup> GenerateDefaultTimeGroups(DateTime sDate)
                //{
                //    List<dtoTimeGroup> items = new List<dtoTimeGroup>();
                //    long maxTicks = DateTime.MaxValue.Ticks;
                //    long minTicks = DateTime.MinValue.Ticks;
                //    long dTicks = sDate.Ticks;

                //    items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.Today, FromTicks = dTicks, ToTicks = dTicks });
                //    dTicks++;

                //    int delta = Convert.ToInt32(sDate.DayOfWeek);
                //    delta = delta == 0 ? delta + 7 : delta;
                //    DateTime lastDay = sDate.AddDays(7 - delta);
                //    items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.Week, FromTicks = dTicks, ToTicks = lastDay.Ticks });

                //    dTicks = lastDay.Ticks + 1;
                //    items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.NextWeek, FromTicks = dTicks, ToTicks = lastDay.AddDays(7).Ticks });
                //    dTicks = lastDay.AddDays(7).Ticks + 1;

                //    lastDay = new DateTime(sDate.Year, sDate.Month, DateTime.DaysInMonth(sDate.Year, sDate.Month));
                //    items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.CurrentMonth, FromTicks = dTicks, ToTicks = lastDay.Ticks });
                //    dTicks = lastDay.Ticks + 1;

                //    if (sDate.Month ==12) {
                //        lastDay = new DateTime(sDate.Year+1, 1, DateTime.DaysInMonth(sDate.Year+1, 1));
                //        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.Month, FromTicks = dTicks, ToTicks = lastDay.Ticks });
                //        dTicks = lastDay.Ticks + 1;

                //        lastDay = new DateTime(sDate.Year + 1, 3, DateTime.DaysInMonth(sDate.Year + 1, 3));
                //        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.Quarter, FromTicks = dTicks, ToTicks = lastDay.Ticks });
                //        dTicks = lastDay.Ticks + 1;

                //        lastDay = new DateTime(sDate.Year + 1, 6, DateTime.DaysInMonth(sDate.Year + 1, 6));
                //        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.HalfYear, FromTicks = dTicks, ToTicks = lastDay.Ticks });
                //        dTicks = lastDay.Ticks + 1;
                //        lastDay = new DateTime(sDate.Year + 1, 12, DateTime.DaysInMonth(sDate.Year + 1, 12));
                //        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.AfterYear, FromTicks = dTicks, ToTicks = lastDay.Ticks });
                //        dTicks = lastDay.Ticks + 1;
                //    }
                //    else {
                //        dTicks = new DateTime(sDate.Year, 1, 1).Ticks;
                //        for (Int32 m = 3; m <= 12; m + 3) {
                //            lastDay = new DateTime(sDate.Year , m, DateTime.DaysInMonth(sDate.Year , m));
                //            items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.Quarter, FromTicks = dTicks, ToTicks = lastDay.Ticks });
                //            dTicks = lastDay.Ticks + 1;
                //        }
                //        dTicks = new DateTime(sDate.Year, 1, 1).Ticks;
                //        for (Int32 m = 1; m <= 12; m + 1)
                //        {
                //            lastDay = new DateTime(sDate.Year, m, DateTime.DaysInMonth(sDate.Year, m));
                //            items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.Month, FromTicks = dTicks, ToTicks = lastDay.Ticks });
                //            dTicks = lastDay.Ticks + 1;
                //        }
                //        dTicks = new DateTime(sDate.Year, 1, 1).Ticks;
                //        for (Int32 m = 6; m <= 12; m + 6)
                //        {
                //            lastDay = new DateTime(sDate.Year, m, DateTime.DaysInMonth(sDate.Year, m));
                //            items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.HalfYear, FromTicks = dTicks, ToTicks = lastDay.Ticks });
                //            dTicks = lastDay.Ticks + 1;
                //        }

                //        lastDay = new DateTime(sDate.Year, 12, DateTime.DaysInMonth(sDate.Year, 12));
                //        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.Year, FromTicks = dTicks, ToTicks = lastDay.Ticks });
                //        dTicks = lastDay.Ticks + 1;
                //        lastDay = new DateTime(sDate.Year+1, 1, 1);
                //        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.AfterYear, FromTicks = dTicks, ToTicks = lastDay.Ticks });
                //        dTicks = lastDay.Ticks + 1;
                //    }
                   

                //    dTicks = lastDay.Ticks + 1;

                //    items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.NextWeek, FromTicks = dTicks, ToTicks = lastDay.AddDays(7).Ticks });
                //    dTicks = lastDay.AddDays(7).Ticks + 1;

                //    lastDay = new DateTime(sDate.Year, sDate.Month, DateTime.DaysInMonth(sDate.Year, sDate.Month));
                //    items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.Month, FromTicks = dTicks, ToTicks = lastDay.Ticks });
                //    dTicks = lastDay.Ticks + 1;
                //    Int32 quarterMonth = 3;
                //    while (sDate.Month < quarterMonth)
                //    {
                //        quarterMonth += 3;
                //    }
                //    if (quarterMonth > 12)
                //        quarterMonth = 12;
                //    lastDay = new DateTime(sDate.Year, quarterMonth, DateTime.DaysInMonth(sDate.Year, quarterMonth));
                //    items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.Quarter, FromTicks = dTicks, ToTicks = lastDay.Ticks });
                //    dTicks = lastDay.Ticks + 1;

                //    Int32 halfYearMonth = 6;
                //    if (sDate.Month > 6)
                //        halfYearMonth = 12;
                //    if (sDate.Month <= 9)
                //    {
                //        lastDay = new DateTime(sDate.Year, halfYearMonth, DateTime.DaysInMonth(sDate.Year, halfYearMonth));
                //        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.HalfYear, FromTicks = dTicks, ToTicks = lastDay.Ticks });
                //        dTicks = lastDay.Ticks + 1;
                //    }

                //    if (halfYearMonth < 12)
                //    {
                //        lastDay = new DateTime(sDate.Year, 12, DateTime.DaysInMonth(sDate.Year, 12));
                //        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.Year, FromTicks = dTicks, ToTicks = lastDay.Ticks });
                //        dTicks = lastDay.Ticks + 1;
                //    }
                //    items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.AfterYear, FromTicks = dTicks, ToTicks = maxTicks });

                //    dTicks = sDate.Ticks - 1;
                //    delta = Convert.ToInt32(sDate.DayOfWeek);
                //    delta = delta == 0 ? delta + 7 : delta;
                //    lastDay = sDate.AddDays((7 - delta) - 7);
                //    if (lastDay.DayOfYear != sDate.DayOfYear)
                //    {
                //        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.PreviousDays, FromTicks = lastDay.Ticks, ToTicks = dTicks });
                //        dTicks = lastDay.Ticks - 1;
                //    }
                //    lastDay = lastDay.AddDays(-7);
                //    items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.PreviousWeek, FromTicks = lastDay.Ticks, ToTicks = dTicks });
                //    dTicks = lastDay.Ticks - 1;


                //    if (lastDay.Day != 1)
                //    {
                //        lastDay = new DateTime(sDate.Year, sDate.Month, 1);
                //        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.PreviousWeeks, FromTicks = lastDay.Ticks, ToTicks = dTicks });
                //        dTicks = lastDay.Ticks - 1;
                //    }
                //    if (lastDay.Month > 1)
                //    {
                //        lastDay = new DateTime(sDate.Year, sDate.Month - 1, 1);
                //        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.PreviousMonth, FromTicks = lastDay.Ticks, ToTicks = dTicks });
                //        dTicks = lastDay.Ticks - 1;
                //    }

                //    qPosition = sDate.Month % 3;
                //    if (qPosition != 0 && lastDay.Month != sDate.Month - qPosition)
                //    {
                //        lastDay = new DateTime(sDate.Year, sDate.Month - qPosition, 1);
                //        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.CurrentQuarter, FromTicks = lastDay.Ticks, ToTicks = dTicks });
                //        dTicks = lastDay.Ticks - 1;

                //        if (lastDay.Month > 3)
                //        {
                //            lastDay = new DateTime(sDate.Year, lastDay.Month - 3, 1);
                //            items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.PreviousQuarter, FromTicks = lastDay.Ticks, ToTicks = dTicks });
                //            dTicks = lastDay.Ticks - 1;
                //        }
                //    }
                //    else if (qPosition == 0)
                //    {
                //        if (sDate.Month == 3)
                //            lastDay = new DateTime(sDate.Year, sDate.Month - 2, 1);
                //        else
                //            lastDay = new DateTime(sDate.Year, sDate.Month - 3, 1);
                //        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.PreviousQuarter, FromTicks = lastDay.Ticks, ToTicks = dTicks });
                //        dTicks = lastDay.Ticks - 1;
                //    }

                //    if (lastDay.Month == 6)
                //    {
                //        lastDay = new DateTime(sDate.Year, 1, 1);
                //        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.PreviousHalfYear, FromTicks = dTicks, ToTicks = lastDay.Ticks });
                //        dTicks = lastDay.Ticks - 1;
                //    }
                //    else if (lastDay.Month > 6)
                //    {
                //        if (lastDay.Month >= 9)
                //        {
                //            lastDay = new DateTime(sDate.Year, 6, 1);
                //            items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.CurrentHalfYear, FromTicks = dTicks, ToTicks = lastDay.Ticks });
                //            dTicks = lastDay.Ticks - 1;
                //        }

                //        lastDay = new DateTime(sDate.Year, 1, 1);
                //        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.PreviousHalfYear, FromTicks = dTicks, ToTicks = lastDay.Ticks });
                //        dTicks = lastDay.Ticks + 1;
                //    }
                //    else if (lastDay.Month > 3)
                //    {
                //        lastDay = new DateTime(sDate.Year, 1, 1);
                //        items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.CurrentHalfYear, FromTicks = dTicks, ToTicks = lastDay.Ticks });
                //        dTicks = lastDay.Ticks + 1;

                //    }
                //    lastDay = new DateTime(sDate.Year, 1, 1);
                //    items.Add(new dtoTimeGroup() { TimeLine = TimeGroup.Previous, FromTicks = minTicks, ToTicks = lastDay.Ticks - 1 });

                //    return items;
                //}

                //public List<TimeGroup> GetAvailableTimeGroups(DateTime sDate)
                //{
                //    List<TimeGroup> items = new List<TimeGroup>();

                //    return items;
                //}
                //public List<TimeGroup> GetAvailableNextTimeGroups(Int32 year,Int32 month, Int32 day)
                //{
                //    List<TimeGroup> items = new List<TimeGroup>();
                //    switch (month) { 
                //        case 1:
                //            items.Add(TimeGroup.Month);
                //            items.Add(TimeGroup.CurrentQuarter);
                //            items.Add(TimeGroup.Quarter);
                //            items.Add(TimeGroup.HalfYear);
                //            items.Add(TimeGroup.AfterYear);
                //            break;
                //        case 2,4,5,7,:
                //            items.Add(TimeGroup.Month);
                //            items.Add(TimeGroup.CurrentQuarter);
                //            items.Add(TimeGroup.HalfYear);
                //            items.Add(TimeGroup.AfterYear);
                //        case 3:

                //        case 12:
                //            items.Add(TimeGroup.Quarter);
                //            items.Add(TimeGroup.HalfYear);
                //            items.Add(TimeGroup.AfterYear);
                //            break;
                //    }
                //    return items;
                //}

            #endregion
        #endregion

    }
}