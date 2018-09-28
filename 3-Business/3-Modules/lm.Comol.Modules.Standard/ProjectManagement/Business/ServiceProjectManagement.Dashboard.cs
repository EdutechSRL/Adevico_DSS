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
        #region "Assigned Activities"
            public Boolean UserHasActivitiesDueForCompletion(Person p, dtoProjectContext context, dtoItemsFilter filter, PageListType pageType, SummaryTimeLine timeline, long idProject = 0)
            {
                return UserHasActivitiesByStatus(UserActivityStatus.Expiring, p, context, filter, pageType, timeline, idProject);
            }
            public Boolean UserHasLateActivities(Person p, dtoProjectContext context, dtoItemsFilter filter, PageListType pageType, SummaryTimeLine timeline, long idProject = 0) {
                return UserHasActivitiesByStatus(UserActivityStatus.Expired, p, context, filter, pageType, timeline, idProject);
            }
            private Boolean UserHasActivitiesByStatus(UserActivityStatus status, Person p, dtoProjectContext context, dtoItemsFilter filter, PageListType pageType, SummaryTimeLine timeline, long idProject = 0)
            {
                switch (pageType)
                {
                    case PageListType.ProjectDashboardManager:
                        return ProjectHasActivitiesByStatus(status, idProject, timeline);
                    case PageListType.DashboardAdministrator:
                        if (idProject > 0)
                            return ProjectHasActivitiesByStatus(status, idProject, timeline);
                        break;
                    default:
                        Int32 idCommunity = context.IdCommunity;
                        if (filter.FilterBy != ProjectFilterBy.CurrentCommunity && filter.FilterBy != ProjectFilterBy.AllPersonalFromCurrentCommunity)
                            idCommunity = -100;
                        List<dtoTimelineSummary> items = GetSummary(p.Id, idProject, filter.FilterBy, ItemListStatus.All, idCommunity);
                        if (pageType== PageListType.ProjectDashboardResource)
                            return (items != null && items.Where(i => i.DashboardPage ==  PageListType.DashboardResource && i.Activities.Where(a => a.TimeLine == timeline && a.Status == status).Any()).Any());
                        else
                            return (items != null && items.Where(i => i.DashboardPage == pageType && i.Activities.Where(a => a.TimeLine == timeline && a.Status == status).Any()).Any());
                }
                return false;
            }
            private Boolean ProjectHasActivitiesByStatus(UserActivityStatus status, long idProject, SummaryTimeLine timeline)
            {
                return ProjectHasActivitiesByStatus((from a in Manager.GetIQ<PmActivity>() where a.Deleted== BaseStatusDeleted.None && a.Project.Id == idProject && !a.IsSummary  select a), status,timeline, GetDaysOfYear( DateTime.Now));
            }
            private Boolean ProjectHasActivitiesByStatus(IEnumerable<PmActivity> qActivities, UserActivityStatus status, SummaryTimeLine timeline, Dictionary<SummaryTimeLine, long> days)
            {
                IEnumerable<PmActivity> query;
                switch (status)
                {
                    case UserActivityStatus.Starting:
                        query = qActivities.Where(a => a.EarlyStartDate.HasValue && a.Completeness == 0 && a.EarlyStartDate.Value.Ticks >= days[SummaryTimeLine.Today] && a.EarlyStartDate.Value.Ticks <= days[timeline]);
                        break;
                    case UserActivityStatus.ToDo:
                        query = qActivities.Where(a => a.EarlyStartDate.HasValue && !a.IsCompleted && a.EarlyStartDate.Value.Ticks >= days[SummaryTimeLine.Today] && a.EarlyStartDate.Value.Ticks <= days[timeline]);
                        break;
                    case UserActivityStatus.Expired:
                        query = qActivities.Where(a => a.EarlyFinishDate.HasValue && !a.IsCompleted && a.EarlyFinishDate.Value.Ticks < days[SummaryTimeLine.Today]);
                        break;
                    case UserActivityStatus.Expiring:
                        query = qActivities.Where(a => a.EarlyFinishDate.HasValue && !a.IsCompleted && a.EarlyFinishDate.Value.Ticks >= days[timeline] && a.EarlyFinishDate.Value.Ticks <= days[timeline]);
                        break;
                    default:
                        return false;
                }
                return query.Select(a => a.Id).Any();
            }
        #endregion

        #region "Get Activities"
            public List<dtoPlainTask> GetTasks(Int32 idPerson, dtoItemsFilter filter, Int32 idCommunity, PageContainerType container, PageListType fromPage, PageListType displayPage)
            {
                List<dtoPlainTask> tasks = new List<dtoPlainTask>();
                try
                {
                    Person person = Manager.GetPerson(idPerson);
                    switch (container)
                    { 
                        case PageContainerType.ProjectDashboard:
                            switch (displayPage)
                            {
                                case PageListType.ProjectDashboardManager:
                                    if (IsProjectManager(filter.IdProject, idPerson))
                                        tasks = GetTasksAsManager(idPerson, filter, idCommunity, container, fromPage, displayPage);
                                    else if (IsProjectAdministrator(filter.IdProject,person ))
                                        tasks = GetProjectTasksAsAdministrator(person, filter, idCommunity);
                                    break;
                                case PageListType.ProjectDashboardResource:
                                    tasks = GetTasksAsResource(idPerson, filter, idCommunity, container, fromPage, displayPage);
                                    break;
                            }
                            break;
                        case PageContainerType.Dashboard:
                            switch (displayPage)
                            {
                                case PageListType.DashboardManager:
                                    tasks = GetTasksAsManager(idPerson, filter, idCommunity,container, fromPage, displayPage);
                                    break;
                                case PageListType.DashboardAdministrator:
                                    tasks = GetTasksAsAdministrator(person, filter, idCommunity);
                                    break;
                                case PageListType.DashboardResource:
                                    tasks = GetTasksAsResource(idPerson, filter, idCommunity, container, fromPage, displayPage);
                                    break;
                            }
                            break;
                    }
                    #region "ApplyFilters"

                    switch (filter.GroupBy)
                    {
                        case ItemsGroupBy.Plain:
                            switch (filter.OrderBy)
                            {
                                case ProjectOrderBy.TaskName:
                                    if (filter.Ascending)
                                        tasks = tasks.OrderBy(p => p.Name).ThenByDescending(p => p.Deadline).ThenByDescending(p => p.EndDate).ToList();
                                    else
                                        tasks = tasks.OrderByDescending(p => p.Name).ThenByDescending(p => p.Deadline).ThenByDescending(p => p.EndDate).ToList();
                                    break;
                                case ProjectOrderBy.Completion:
                                    if (filter.Ascending)
                                        tasks = tasks.OrderBy(p => p.Completeness).ThenBy(p => p.Name).ToList();
                                    else
                                        tasks = tasks.OrderByDescending(p => p.IsCompleted).ThenByDescending(p => p.Completeness).ThenBy(p => p.Name).ToList();
                                    break;
                                case ProjectOrderBy.Deadline:
                                    if (filter.Ascending)
                                        tasks = tasks.OrderBy(p => p.VirtualDeadline).ThenBy(p => p.Name).ToList();
                                    else
                                        tasks = tasks.OrderBy(p => p.VirtualDeadline).ThenBy(p => p.Name).ToList();
                                    break;
                                case ProjectOrderBy.EndDate:
                                    if (filter.Ascending)
                                        tasks = tasks.OrderBy(p => p.VirtualEndDate).ThenBy(p => p.Name).ToList();
                                    else
                                        tasks = tasks.OrderByDescending(p => p.VirtualEndDate).ThenBy(p => p.Name).ToList();
                                    break;
                            }
                            break;
                        case ItemsGroupBy.EndDate:
                            if (filter.Ascending)
                                tasks = tasks.OrderBy(p => p.VirtualEndDate).ThenBy(p => p.Name).ToList();
                            else
                                tasks = tasks.OrderByDescending(p => p.VirtualEndDate).ThenBy(p => p.Name).ToList();

                            break;
                        case ItemsGroupBy.Community:
                            if (filter.Ascending)
                                tasks = tasks.OrderBy(p => p.ProjectInfo.VirtualCommunityName).ThenBy(p => p.VirtualDeadline).ToList();
                            else
                                tasks = tasks.OrderByDescending(p => p.ProjectInfo.VirtualCommunityName).ThenBy(p => p.VirtualDeadline).ToList();
                            break;
                        case ItemsGroupBy.Project:
                            if (filter.Ascending)
                                tasks = tasks.OrderBy(p => p.ProjectInfo.VirtualProjectName).ThenBy(p => p.VirtualDeadline).ToList();
                            else
                                 tasks = tasks.OrderByDescending(p => p.ProjectInfo.VirtualProjectName).ThenBy(p => p.VirtualDeadline).ToList();
                            break;
                    }
                    #endregion
                }
                catch (Exception ex)
                {

                }
                return tasks;
            }
            private List<dtoPlainTask> GetTasksAsManager(Int32 idPerson, dtoItemsFilter filter, Int32 idCommunity, PageContainerType container, PageListType fromPage, PageListType displayPage, List<dtoProjectGroupInfo> projectsInfo = null)
            {
                List<dtoPlainTask> tasks = new List<dtoPlainTask>();
                try
                {
                    IEnumerable<ProjectResource> query = GetResourcesAsManager(idPerson, filter, idCommunity, false);
                    DateTime today = DateTime.Now.Date;
                    Dictionary<SummaryTimeLine, long> days = GetDaysOfYear(today);
                    if (query.Count() < maxItemsForQuery)
                    {
                        Dictionary<long, ActivityRole> projectRoles = query.ToDictionary(r => r.Project.Id, r => r.ProjectRole);
                        tasks.AddRange(GetTasks(GetTasksQuery(projectRoles.Keys.ToList()), days, filter, projectRoles, container, displayPage));
                    }
                    else
                    {
                        Int32 pageIndex = 0;
                        var pInfo = query.Select(i => new { IdProject=i.Project.Id, Role= i.ProjectRole}).ToList();
                        while (pInfo.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).Any())
                        {
                            tasks.AddRange(GetTasks(GetTasksQuery(pInfo.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).Select(p => p.IdProject).ToList()), days, filter, pInfo.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToDictionary(p => p.IdProject, p => p.Role), container, displayPage));
                            pageIndex++;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return tasks;
            }

            #region "Get Task As Administrator"
                private Boolean IsProjectAdministrator(long idProject, Person person)
                {
                    Boolean result = false;
                    try
                    {
                        liteProjectSettings project = Manager.Get<liteProjectSettings>(idProject);
                        if (project != null && !project.isPersonal) {
                            ModuleProjectManagement mPermission = (project.isPortal) ? ModuleProjectManagement.CreatePortalmodule((person == null) ? (Int32)UserTypeStandard.Guest : person.TypeID) : new ModuleProjectManagement(Manager.GetModulePermission(person.Id, project.IdCommunity, GetIdModule()));
                            result = mPermission.Administration;
                        }
                    }
                    catch (Exception ex) { 
                    
                    }
                    return result;
                }

                private List<dtoPlainTask> GetProjectTasksAsAdministrator(Person person, dtoItemsFilter filter,Int32 idCommunity)
                {
                    List<dtoPlainTask> tasks = new List<dtoPlainTask>();
                    try
                    {
                        List<long> idProjects =  new List<long>() {{filter.IdProject}};
                        Dictionary<SummaryTimeLine, long> days = GetDaysOfYear(DateTime.Now.Date);
                        if (idProjects.Count() < maxItemsForQuery)
                            tasks.AddRange(GetTasks(GetTasksQuery(idProjects), days, filter, GetRolePermissions(ActivityRole.ProjectOwner)));
                        else
                        {
                            Int32 pageIndex = 0;
                            while (idProjects.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).Any())
                            {
                                tasks.AddRange(GetTasks(GetTasksQuery(idProjects.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList()), days, filter, GetRolePermissions(ActivityRole.ProjectOwner)));
                                pageIndex++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return tasks;
                }
                private List<dtoPlainTask> GetTasksAsAdministrator(Person person, dtoItemsFilter filter, Int32 idCommunity)
                {
                    List<dtoPlainTask> tasks = new List<dtoPlainTask>();
                    try
                    {
                        List<long> idProjects = GetIdProjectsForTasksAsAdministrator(person, filter, idCommunity);
                        Dictionary<SummaryTimeLine, long> days = GetDaysOfYear( DateTime.Now.Date);
                        if (idProjects.Count() < maxItemsForQuery)
                            tasks.AddRange(GetTasks(GetTasksQuery(idProjects), days, filter, GetRolePermissions(ActivityRole.ProjectOwner)));
                        else
                        {
                            Int32 pageIndex = 0;
                            while (idProjects.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).Any())
                            {
                                tasks.AddRange(GetTasks(GetTasksQuery(idProjects.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList()), days, filter, GetRolePermissions(ActivityRole.ProjectOwner)));
                                pageIndex++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return tasks;
                }
                private List<long> GetIdProjectsForTasksAsAdministrator(Person person, dtoItemsFilter filter, Int32 idCommunity)
                {
                    List<long> results = new List<long>();
                    List<long> idProjects = (from r in Manager.GetIQ<ProjectResource>()
                                             where r.Type == ResourceType.Internal && r.Person != null && r.Person.Id == person.Id
                                                 && (r.ProjectRole == ActivityRole.ProjectOwner || r.ProjectRole == ActivityRole.Manager)
                                             select r.Project.Id).ToList();

                    var query = (from p in Manager.GetIQ<Project>()
                                 where ((filter.ProjectsStatus == ItemListStatus.Deleted && p.Deleted == BaseStatusDeleted.Manual) || (filter.ProjectsStatus != ItemListStatus.Deleted && p.Deleted == BaseStatusDeleted.None))
                              && p.isPersonal == false && !idProjects.Contains(p.Id)
                                 select p);
                    query = query.Where(p => p.Availability != ProjectAvailability.Draft);


                    ModuleProjectManagement pPermissions = ModuleProjectManagement.CreatePortalmodule(person.TypeID);
                    ModuleProjectManagement cPermissions = new ModuleProjectManagement() { Administration = true };
                    switch (filter.FilterBy)
                    {
                        case ProjectFilterBy.All:

                            if (pPermissions.Administration)
                                results.AddRange(query.Where(p => p.isPortal).Select(p=>p.Id).ToList());
                            List<Int32> idCommunities = ServiceCommunityManagement.GetIdCommunityByModulePermissions(person.Id, new Dictionary<int, long>() { { GetIdModule(), cPermissions.GetPermissions() } });
                            if (idCommunities.Any())
                                results.AddRange(query.Where(p => !p.isPortal && p.Community != null && idCommunities.Contains(p.Community.Id)).Select(p => p.Id).ToList());
                            break;
                        case ProjectFilterBy.CurrentCommunity:
                            cPermissions = new ModuleProjectManagement(Manager.GetModulePermission(person.Id, idCommunity, idModule));
                            if (cPermissions.Administration)
                                results.AddRange(query.Where(p => !p.isPortal && p.Community != null && p.Community.Id == idCommunity).Select(p=>p.Id).ToList());
                            break;
                        case ProjectFilterBy.FromPortal:
                            if (pPermissions.Administration)
                                results.AddRange(query.Where(p => p.isPortal).Select(p => p.Id).ToList());
                            break;
                    }
                    return results;
                }
            #endregion

            #region "Methods to get Tasks as manager/administrator"
                #region "Base Query"
                    private IEnumerable<PmActivity> GetTasksBaseQuery()
                    {
                        return (from a in Manager.GetIQ<PmActivity>() where !a.IsSummary && a.Deleted == BaseStatusDeleted.None select a);
                    }
                    private IEnumerable<PmActivity> GetTasksQuery(long idProject)
                    {
                        return GetTasksBaseQuery().Where(a => a.Project.Id == idProject);
                    }
                    private IEnumerable<PmActivity> GetTasksQuery(List<long> idProjects)
                    {
                        return GetTasksBaseQuery().Where(a => idProjects.Contains(a.Project.Id));
                    }
                #endregion

                private List<dtoPlainTask> GetTasks(IEnumerable<PmActivity> query, Dictionary<SummaryTimeLine, long> days, dtoItemsFilter filter, Dictionary<long, ActivityRole> projectRoles, PageContainerType container, PageListType displayPage)
                {
                    List<dtoPlainTask> tasks = GetTasks(query, days, filter);
                    Dictionary<ActivityRole, PmActivityPermission> permissions = projectRoles.Values.Distinct().ToDictionary(v => v, v => GetRolePermissions(GetUserRoleByPage(v, container, displayPage)));
                    foreach (dtoPlainTask t in tasks)
                    {
                        t.SetPermissions(permissions[projectRoles[t.ProjectInfo.Id]]);
                    }
                    return tasks;
                }
                private List<dtoPlainTask> GetTasks(IEnumerable<PmActivity> query, Dictionary<SummaryTimeLine, long> days, dtoItemsFilter filter, PmActivityPermission permissions = PmActivityPermission.None)
                {
                    List<dtoPlainTask> tasks = new List<dtoPlainTask>();
                    switch (filter.ActivitiesStatus)
                    {
                        case ItemListStatus.Active:
                            query = query.Where(a => (!a.IsCompleted || a.Completeness < 100) && a.EarlyStartDate != null && a.EarlyStartDate.Value.Ticks >= days[SummaryTimeLine.Today] && a.EarlyStartDate.Value.Ticks <= days[filter.TimeLine]);
                            break;
                        case ItemListStatus.Completed:
                            query = query.Where(a => a.IsCompleted && a.Completeness == 100);
                            break;
                        case ItemListStatus.Future:
                            query = query.Where(a => (!a.IsCompleted || a.Completeness < 100) && a.EarlyStartDate != null && a.EarlyStartDate.Value.Ticks > days[filter.TimeLine]);
                            break;

                        case ItemListStatus.Late:
                            query = query.Where(a => (!a.IsCompleted || a.Completeness < 100) && ((a.EarlyStartDate != null && a.EarlyFinishDate.Value.Ticks < days[SummaryTimeLine.Today]) || (a.Deadline != null && a.Deadline.Value.Ticks < days[SummaryTimeLine.Today])));
                            break;
                        case ItemListStatus.DueForCompletion:
                            query = query.Where(a => (!a.IsCompleted || a.Completeness < 100) && a.EarlyStartDate != null && a.EarlyStartDate.Value.Ticks >= days[SummaryTimeLine.Today] && ((a.EarlyFinishDate.Value.Ticks <= days[filter.TimeLine]) || (a.Deadline != null && a.Deadline.Value.Ticks <= days[filter.TimeLine])));
                            break;
                        case ItemListStatus.Ignore:
                            switch (filter.UserActivitiesStatus)
                            {
                                case UserActivityStatus.Starting:
                                    query = query.Where(a => a.EarlyStartDate != null && a.Completeness == 0 && a.EarlyStartDate.Value.Ticks >= days[SummaryTimeLine.Today] && a.EarlyStartDate.Value.Ticks <= days[filter.UserActivitiesTimeLine]);
                                    break;
                                case UserActivityStatus.ToDo:
                                    query = query.Where(a => a.EarlyStartDate != null && !a.IsCompleted && a.EarlyStartDate.Value.Ticks >= days[SummaryTimeLine.Today] && a.EarlyStartDate.Value.Ticks <= days[filter.UserActivitiesTimeLine]);
                                    break;
                                case UserActivityStatus.Expired:
                                    query = query.Where(a => a.EarlyStartDate != null && !a.IsCompleted && a.EarlyFinishDate != null && a.EarlyFinishDate.Value.Ticks < days[SummaryTimeLine.Today]);
                                    break;
                                case UserActivityStatus.Expiring:
                                    query = query.Where(a => a.EarlyStartDate != null && !a.IsCompleted && a.EarlyFinishDate != null && a.EarlyFinishDate.Value.Ticks >= days[SummaryTimeLine.Today] && a.EarlyFinishDate.Value.Ticks <= days[filter.UserActivitiesTimeLine]);
                                    break;
                            }
                            break;
                    }
                    tasks = query.ToList().Select(t => new dtoPlainTask(t, permissions)).ToList();
                    return tasks;
                }
            #endregion

            #region "Get Task As Resource"
                private List<dtoPlainTask> GetTasksAsResource(Int32 idPerson, dtoItemsFilter filter, Int32 idCommunity, PageContainerType container, PageListType fromPage, PageListType displayPage)
                {
                    List<dtoPlainTask> tasks = new List<dtoPlainTask>();
                    try
                    {
                        Dictionary<ActivityRole, PmActivityPermission> permissions = (from e in Enum.GetValues(typeof(ActivityRole)).Cast<ActivityRole>() where e != ActivityRole.Manager && e != ActivityRole.ProjectOwner select e).ToDictionary(e => e, e => GetRolePermissions(e));
                        tasks = GetTasks( GetAssignmentsByUser(idPerson, filter, idCommunity),GetDaysOfYear(DateTime.Now.Date), filter,permissions);

                        Dictionary<Int32,String> cNames = null;
                        List<Int32> idCommunities = tasks.Select(t=> t.ProjectInfo.IdCommunity).Distinct().ToList();
                        if (idCommunities.Count> maxItemsForQuery)
                            cNames= (from c in Manager.GetIQ<Community>() select new {IdCommunity= c.Id, Name=c.Name}).ToList().Where(c=>idCommunities.Contains(c.IdCommunity)).ToDictionary(c=> c.IdCommunity, c=> c.Name);
                        else
                            cNames= (from c in Manager.GetIQ<Community>() where idCommunities.Contains(c.Id) select c).ToDictionary(c=> c.Id, c=> c.Name);
                        foreach (dtoPlainTask t in tasks.Where(t => !t.ProjectInfo.isPortal && !cNames.ContainsKey(t.ProjectInfo.IdCommunity)))
                        {
                            t.ProjectInfo.IdCommunity = -1;
                        }
                        foreach (dtoPlainTask t in tasks.Where(t => !t.ProjectInfo.isPortal && cNames.ContainsKey(t.ProjectInfo.IdCommunity)))
                        {
                            t.ProjectInfo.CommunityName = cNames[t.ProjectInfo.IdCommunity];
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return tasks;
                }
                private IEnumerable<liteProjectActivityAssignment> GetAssignmentsByUser(Int32 idPerson, dtoItemsFilter filter, Int32 idCommunity)
                {
                    IEnumerable<liteProjectActivityAssignment> query = (from r in Manager.GetIQ<liteProjectActivityAssignment>()
                                                                          where r.Deleted == BaseStatusDeleted.None && r.IdPerson== idPerson
                                                                          select r);
                    if (filter.IdProject > 0)
                        query = query.Where(a => a.Project.Id == filter.IdProject);
                    switch (filter.FilterBy)
                    {
                        case ProjectFilterBy.AllPersonal:
                            query = query.Where(r => r.Project != null && r.Project.isPersonal && r.Resource.ProjectRole == ActivityRole.ProjectOwner);
                            break;
                        case ProjectFilterBy.AllPersonalFromCurrentCommunity:
                            query = query.Where(r => r.Project != null && r.Project.isPersonal && !r.Project.isPortal && r.Project.IdCommunity== idCommunity && r.Resource.ProjectRole == ActivityRole.ProjectOwner);
                            break;
                        case ProjectFilterBy.CurrentCommunity:
                            query = query.Where(r => r.Project != null && !r.Project.isPortal && r.Project.IdCommunity == idCommunity);
                            break;
                        case ProjectFilterBy.FromPortal:
                            query = query.Where(r => r.Project != null && r.Project.isPortal && !r.Project.isPersonal);
                            break;
                    }
                    query = query.Where(r => r.Project.Availability != ProjectAvailability.Draft);
                    //switch (filter.Status)
                    //{
                    //    case ItemListStatus.All:
                    //        query = query.Where(r => r.Project.Availability != ProjectAvailability.Draft);
                    //        break;
                    //    case ItemListStatus.Future:
                    //        query = query.Where(r => (r.Project.Status != ProjectItemStatus.completed && r.Project.Availability != ProjectAvailability.Draft && r.Project.StartDate.Value.Ticks > DateTime.Now.Date.Ticks));
                    //        break;
                    //    case ItemListStatus.Active:
                    //        query = query.Where(r => r.Project != null && !r.Project.IsCompleted && r.Project.Availability == ProjectAvailability.Active);
                    //        break;
                    //    case ItemListStatus.Completed:
                    //        query = query.Where(r => r.Project != null && r.Project.IsCompleted);
                    //        break;
                    //}
                    return query;
                }
                private List<dtoPlainTask> GetTasks(IEnumerable<liteProjectActivityAssignment> query, Dictionary<SummaryTimeLine, long> days, dtoItemsFilter filter, Dictionary<ActivityRole, PmActivityPermission> permissions)
                {
                    List<dtoPlainTask> tasks = new List<dtoPlainTask>();
                    switch (filter.ActivitiesStatus)
                    {
                        case ItemListStatus.Active:
                            query = query.Where(a => (!a.IsApproved || a.Completeness < 100) && a.Activity.EarlyStartDate != null && a.Activity.EarlyStartDate.Value.Ticks >= days[SummaryTimeLine.Today] && a.Activity.EarlyStartDate.Value.Ticks <= days[filter.TimeLine]);
                            break;
                        case ItemListStatus.Completed:
                            query = query.Where(a => a.IsApproved && a.Completeness == 100);
                            break;
                        case ItemListStatus.Future:
                            query = query.Where(a => (!a.IsApproved || a.Completeness < 100) && a.Activity.EarlyStartDate != null && a.Activity.EarlyStartDate.Value.Ticks > days[filter.TimeLine]);
                            break;

                        case ItemListStatus.Late:
                            query = query.Where(a => (!a.IsApproved || a.Completeness < 100) && ((a.Activity.EarlyStartDate != null && a.Activity.EarlyFinishDate.Value.Ticks < days[SummaryTimeLine.Today]) || (a.Activity.Deadline != null && a.Activity.Deadline.Value.Ticks < days[SummaryTimeLine.Today])));
                            break;
                        case ItemListStatus.DueForCompletion:
                            query = query.Where(a => (!a.IsApproved || a.Completeness < 100) && a.Activity.EarlyStartDate != null && a.Activity.EarlyStartDate.Value.Ticks >= days[SummaryTimeLine.Today] && ((a.Activity.EarlyFinishDate.Value.Ticks <= days[filter.TimeLine]) || (a.Activity.Deadline != null && a.Activity.Deadline.Value.Ticks <= days[filter.TimeLine])));
                            break;
                        case ItemListStatus.Ignore:
                            switch (filter.UserActivitiesStatus)
                            {
                                case UserActivityStatus.Starting:
                                    query = query.Where(a => a.Activity.EarlyStartDate != null && a.Completeness == 0 && a.Activity.EarlyStartDate.Value.Ticks >= days[SummaryTimeLine.Today] && a.Activity.EarlyStartDate.Value.Ticks <= days[filter.UserActivitiesTimeLine]);
                                    break;
                                case UserActivityStatus.ToDo:
                                    query = query.Where(a => a.Activity.EarlyStartDate != null && (!a.IsApproved || a.Completeness <100) && a.Activity.EarlyStartDate.Value.Ticks >= days[SummaryTimeLine.Today] && a.Activity.EarlyStartDate.Value.Ticks <= days[filter.UserActivitiesTimeLine]);
                                    break;
                                case UserActivityStatus.Expired:
                                    query = query.Where(a => a.Activity.EarlyStartDate != null && (!a.IsApproved ||a.Completeness < 100) && a.Activity.EarlyFinishDate != null && a.Activity.EarlyFinishDate.Value.Ticks < days[SummaryTimeLine.Today]);
                                    break;
                                case UserActivityStatus.Expiring:
                                    query = query.Where(a => a.Activity.EarlyStartDate != null && (!a.IsApproved ||a.Completeness < 100) && a.Activity.EarlyFinishDate != null && a.Activity.EarlyFinishDate.Value.Ticks >= days[SummaryTimeLine.Today] && a.Activity.EarlyFinishDate.Value.Ticks <= days[filter.UserActivitiesTimeLine]);
                                    break;
                            }
                            break;
                    }                  
                    tasks = query.ToList().Select(a => new dtoPlainTask(a, permissions)).ToList();
                    return tasks;
                }
            #endregion
            public ActivityRole GetUserRoleByPage(ActivityRole role,PageContainerType container, PageListType currentPage)
            {
                switch (container) {
                    case PageContainerType.Dashboard:
                        if (currentPage== PageListType.DashboardResource)
                            return (role == ActivityRole.ProjectOwner || role == ActivityRole.Manager) ? ActivityRole.Resource : role;
                        else
                            return role;
                    case PageContainerType.ProjectDashboard:
                        if (currentPage == PageListType.ProjectDashboardResource)
                            return (role == ActivityRole.ProjectOwner || role == ActivityRole.Manager) ? ActivityRole.Resource : role;
                        else
                            return role;
                }
                return role;
            }

            public Dictionary<ResourceActivityStatus, long> GetResourceCompletion(long idProject, long idPerson) {
                ProjectResource resource = GetResource(idProject,idProject);
                return (resource != null) ? GetResourceCompletion(resource) : new Dictionary<ResourceActivityStatus, long>() { { ResourceActivityStatus.completed, 0 }, { ResourceActivityStatus.started, 0 }, { ResourceActivityStatus.confirmed, 0 }, { ResourceActivityStatus.late, 0 }, { ResourceActivityStatus.notstarted, 0 }, { ResourceActivityStatus.started, 0 } };
            }
            public Dictionary<ResourceActivityStatus, long> GetResourceCompletion(ProjectResource resource)
            {
                Dictionary<ResourceActivityStatus, long> projectCompletion = new Dictionary<ResourceActivityStatus, long>();
                long notStarted = resource.AssignedActivities;
                projectCompletion[ResourceActivityStatus.completed] = resource.CompletedActivities;
                notStarted -= resource.CompletedActivities;
                if (resource.Project != null && resource.Project.ConfirmCompletion)
                {
                    projectCompletion[ResourceActivityStatus.confirmed] = resource.ConfirmedActivities;
                    notStarted -= resource.ConfirmedActivities;
                }
                notStarted = notStarted - resource.LateActivities - resource.StartedActivities;
                if (notStarted < 0)
                    notStarted = 0;
                projectCompletion[ResourceActivityStatus.late] = resource.LateActivities;
                projectCompletion[ResourceActivityStatus.started] = resource.StartedActivities;
                projectCompletion[ResourceActivityStatus.notstarted] = notStarted;
                projectCompletion[ResourceActivityStatus.all] = resource.AssignedActivities;
                return projectCompletion;
            }
            public Dictionary<long,Dictionary<ResourceActivityStatus, long>> GetResourcesProjectCompletion(List<long> idResources)
            {
                Dictionary<long, Dictionary<ResourceActivityStatus, long>> pCompletion = new Dictionary<long, Dictionary<ResourceActivityStatus, long>>();
                foreach (ProjectResource resource in (from r in Manager.GetIQ<ProjectResource>() where idResources.Contains(r.Id ) select r)) {
                    pCompletion.Add(resource.Project.Id, GetResourceCompletion(resource));
                }

                return pCompletion;
            }
        #endregion
    }
}