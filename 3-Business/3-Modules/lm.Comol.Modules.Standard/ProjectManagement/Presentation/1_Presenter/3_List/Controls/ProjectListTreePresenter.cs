using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;
using lm.Comol.Modules.Standard.ProjectManagement.Business;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public class ProjectListTreePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewProjectListTree View
            {
                get { return (IViewProjectListTree)base.View; }
            }
            private ServiceProjectManagement Service
            {
                get
                {
                    if (service == null)
                        service = new ServiceProjectManagement(AppContext);
                    return service;
                }
            }
            private Int32 CurrentIdModule
            {
                get
                {
                    if (currentIdModule == 0)
                        currentIdModule = CurrentManager.GetModuleID(ModuleProjectManagement.UniqueCode);
                    return currentIdModule;
                }
            }
            public ProjectListTreePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ProjectListTreePresenter(iApplicationContext oContext, IViewProjectListTree view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(dtoProjectContext context, dtoItemsFilter filter, PageListType currentPage) 
        {
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
            View.CurrentPageType = currentPage;
            if (UserContext.isAnonymous || p == null)
            {
                View.DisplayPager(false);
                View.LoadedNoProjects(currentPage);
            }
            else
            {
                if (!View.IsInitialized && filter.GroupBy == ItemsGroupBy.EndDate)
                    filter.PageIndex = -1;
                View.IsInitialized = true;
                View.IdCurrentCommunityForList = context.IdCommunity;
                if (filter.PageSize == 0)
                    filter.PageSize = View.CurrentPageSize;
                switch (filter.GroupBy) { 
                    case ItemsGroupBy.Community:
                        View.CurrentOrderBy = ProjectOrderBy.CommunityName;
                        View.CurrentAscending = true;
                        filter.OrderBy = ProjectOrderBy.CommunityName;
                        filter.Ascending = true;
                        break;
                    case ItemsGroupBy.EndDate:
                        View.CurrentOrderBy = ProjectOrderBy.EndDate;
                        View.CurrentAscending = false;
                        filter.OrderBy = ProjectOrderBy.EndDate;
                        filter.Ascending = false;
                        break;
                }
                View.CurrentGroupBy = filter.GroupBy;
                //View.CurrentAscending = filter.Ascending;
                LoadProjects(filter, filter.GroupBy, currentPage, context.IdCommunity, filter.PageIndex, filter.PageSize);
            }
        }
        public void LoadProjects(dtoItemsFilter filter, ItemsGroupBy groupBy, PageListType currentPage, Int32 idCommunity, Int32 pageIndex, Int32 pageSize)
        {
            if (UserContext.isAnonymous)
            {
                View.DisplayPager(false);
                View.LoadedNoProjects(currentPage);
            }
            else
                LoadProjects(Service.GetProjects(UserContext.CurrentUserID, filter, currentPage, idCommunity, View.RoleTranslations), groupBy, currentPage, filter.TimeLine, pageIndex, pageSize);
        }
        private void LoadProjects(List<dtoPlainProject> projects, ItemsGroupBy groupBy, PageListType currentPage, SummaryTimeLine timeline, Int32 pageIndex, Int32 pageSize)
        {
            if (pageIndex < 0 && groupBy != ItemsGroupBy.EndDate)
                pageIndex = 0;

            PagerBase pager = new PagerBase();
            pager.PageSize = pageSize;//Me.View.CurrentPageSize
            pager.Count = (projects.Count > 0) ? projects.Count - 1 : 0;

            if (pageIndex > -1)
            {
                pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
                View.Pager = pager;
            }
            GroupProjects(projects, groupBy, currentPage, timeline, pageIndex, pageSize, (pageIndex > -1 ? null : pager));
        }
        private void GroupProjects(List<dtoPlainProject> projects, ItemsGroupBy groupBy, PageListType currentPage, SummaryTimeLine timeline, Int32 pageIndex, Int32 pageSize, PagerBase pager = null)
        {
            Int32 idContainerCommunity = View.IdCurrentCommunityForList;

            if (projects.Skip( (pageIndex<0 ? 0 : pageIndex) * pageSize).Take(pageSize).Any()){
                String pName = View.GetPortalName();
                String cName = View.GetUnknownCommunityName();
                projects.Where(p => p.isPortal && p.IdCommunity == 0).ToList().ForEach(p => p.CommunityName = pName);
                projects.Where(p => !p.isPortal && p.IdCommunity < 0).ToList().ForEach(p => p.CommunityName = cName);

                List<dtoProjectsGroup> items = new List<dtoProjectsGroup>();
                switch (groupBy) { 
                    case ItemsGroupBy.Community:
                        items = GenerateCommunityTree(projects, idContainerCommunity, currentPage,timeline, pageIndex, pageSize);
                        break;
                    case ItemsGroupBy.EndDate:
                        items = GenerateEndDateTree(projects, idContainerCommunity, currentPage, timeline, pageIndex, pageSize, pager);
                        break;
                }

                View.LoadProjects(items, currentPage);
            }
            else
                View.LoadedNoProjects(currentPage);
            View.SendUserAction(View.IdCurrentCommunityForList, CurrentIdModule, GetDefaultAction(currentPage));
        }
        private List<dtoProjectsGroup> GenerateCommunityTree(List<dtoPlainProject> projects, Int32 idContainerCommunity, PageListType currentPage, SummaryTimeLine timeline, Int32 pageIndex, Int32 pageSize)
        {
            List<dtoProjectsGroup> items = new List<dtoProjectsGroup>();
            String cssClass = View.GetContainerCssClass(ItemsGroupBy.Community);
            Int32 cCount = View.GetCellsCount(currentPage);
            String sRow = View.GetStartRowId(ItemsGroupBy.Community);
            projects.OrderBy(p => p.CommunityName).Skip(pageIndex * pageSize).Take(pageSize).GroupBy(p => p.VirtualName).OrderBy(p => p.Key).ToList().ForEach(
                p => items.Add(new dtoProjectsGroup()
                {
                    Id = p.FirstOrDefault().IdCommunity,
                    Name = p.FirstOrDefault().CommunityName,
                    Projects = p.ToList(),
                    CssClass =cssClass,
                    CellsCount = cCount,
                    IdRowFirstItem = sRow + p.First().Id.ToString(),
                    IdRowLastItem = sRow + p.Last().Id.ToString(),
                    IdRow = sRow+ p.FirstOrDefault().IdCommunity.ToString()
                }));
            items.ForEach(i => UpdateProjects(i.IdRow, i.Projects, idContainerCommunity, currentPage, timeline));
            long idFirstCommunity = items.First().Projects.Select(p=> p.IdCommunity).FirstOrDefault();
            long idLastCommunity = items.Last().Projects.Select(p=> p.IdCommunity).FirstOrDefault();
            if (pageIndex > 0 &&  projects.OrderBy(p => p.VirtualName).Skip((pageIndex-1) * pageSize).Where(p=>p.IdCommunity== idFirstCommunity).Any()) {
                items.First().PreviousPageIndex = pageIndex - 1;
            }
            if (projects.OrderBy(p => p.VirtualName).Skip((pageIndex + 1) * pageSize).Where(p => p.IdCommunity == idLastCommunity).Any())
            {
                items.Last().NextPageIndex = pageIndex + 1;
            }

            return items;
        }
        private List<dtoProjectsGroup> GenerateEndDateTree(List<dtoPlainProject> projects, Int32 idContainerCommunity, PageListType currentPage, SummaryTimeLine timeline, Int32 pageIndex, Int32 pageSize, PagerBase pager)
        {
            List<dtoProjectsGroup> items = new List<dtoProjectsGroup>();
            String cssClass = View.GetContainerCssClass(ItemsGroupBy.EndDate);
            Int32 cCount = View.GetCellsCount(currentPage);
            String sRow = View.GetStartRowId(ItemsGroupBy.EndDate);

            Dictionary<TimeGroup,String> timeTranslations = View.GetTimeTranslations();
            List<dtoTimeGroup> timegroups = GetAvailableTimeGroups(projects.Select(p=> p.VirtualEndDate).ToList());
            foreach (dtoPlainProject p in projects) {
               p.TimeGroup = GetTimeGroupByDate(timegroups, p.VirtualEndDate);
            }
            if (pageIndex == -1 && pager!=null)
            {
                pageIndex = GetCurrentPageIndex(pageSize, projects);
                pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
                View.Pager = pager;
            }
            String dateTimePattern = View.GetDateTimePattern();
            Dictionary<Int32, String> monthNames = View.GetMonthNames();
            if (projects.Skip(pageIndex * pageSize).Take(pageSize).Any())
            {
                foreach (dtoTimeGroup t in GetAvailableTimeGroups(timegroups, projects.Skip(pageIndex * pageSize).Take(pageSize).Select(p => p.VirtualEndDate).ToList()))
                {
                    items.Add(new dtoProjectsGroup()
                    {
                        Id = ((long)t.TimeLine),
                        Name = GetTimeGroupDisplayName(timeTranslations[t.TimeLine], t, dateTimePattern, monthNames),
                        Projects = projects.Where(p => p.VirtualEndDate.Ticks >= t.FromTicks && p.VirtualEndDate.Ticks <= t.ToTicks).ToList(),
                        CssClass = cssClass,
                        CellsCount = cCount,
                        IdRowFirstItem = sRow + projects.Where(p => p.VirtualEndDate.Ticks >= t.FromTicks && p.VirtualEndDate.Ticks <= t.ToTicks).First().Id.ToString(),
                        IdRowLastItem = sRow + projects.Where(p => p.VirtualEndDate.Ticks >= t.FromTicks && p.VirtualEndDate.Ticks <= t.ToTicks).Last().Id.ToString(),
                        IdRow = sRow + ((long)t.TimeLine).ToString(),
                        Time = t
                    });
                }
                items.ForEach(i => UpdateProjects(i.IdRow, i.Projects, idContainerCommunity, currentPage,timeline));
                TimeGroup firstTimeGroup = items.First().Time.TimeLine;
                TimeGroup lastTimeGroup = items.Last().Time.TimeLine;
                if (pageIndex > 0 && projects.OrderBy(p => p.VirtualEndDate).Skip((pageIndex - 1) * pageSize).Where(p => p.TimeGroup.TimeLine == firstTimeGroup).Any())
                {
                    items.First().PreviousPageIndex = pageIndex - 1;
                }
                if (projects.OrderBy(p => p.VirtualEndDate).Skip((pageIndex + 1) * pageSize).Where(p => p.TimeGroup.TimeLine == lastTimeGroup).Any())
                {
                    items.Last().NextPageIndex = pageIndex + 1;
                }
            }
            return items;
        }
        private String GetTimeGroupDisplayName(String translation, dtoTimeGroup t, String dateTimePattern, Dictionary<Int32, String> monthNames)
        {
            switch (t.TimeLine) {
        //PreviousDaysInHalfYear = 2,
        // = 3,
        //PreviousDaysInQuarter = 5,

                case TimeGroup.PreviousMonth:
                case TimeGroup.ThisMonth:
                case TimeGroup.NextMonth:
                    return (String.IsNullOrEmpty(translation) ? translation : String.Format(translation, monthNames[t.Month]));
                case TimeGroup.PreviousHalfYear:
                case TimeGroup.PreviousQuarter:
                case TimeGroup.PreviousWeeks:
                case TimeGroup.PreviousWeek:
                case TimeGroup.PreviousDays:
                case TimeGroup.NextWeek:
                case TimeGroup.NextQuarter:
                case TimeGroup.NextHalfYear:
                    return (String.IsNullOrEmpty(translation) ? translation : String.Format(translation, t.FromDate.ToString(dateTimePattern) , t.ToDate.ToString(dateTimePattern)));
                case TimeGroup.Today:
                case TimeGroup.ThisWeek:
                case TimeGroup.ThisQuarter:
                case TimeGroup.ThisHalfYear:
               
                    return (String.IsNullOrEmpty(translation) ? translation : String.Format(translation, t.ToDate.ToString(dateTimePattern)));
                case TimeGroup.PreviousYear:
                case TimeGroup.PreviousYears:
                case TimeGroup.ThisYear:
                case TimeGroup.NextYears:
                    return (String.IsNullOrEmpty(translation) ? translation : String.Format(translation, t.Year));
            }
            return translation;
        }
        private dtoTimeGroup GetTimeGroupByDate(List<dtoTimeGroup> timegroups, DateTime projectDate) {
            return timegroups.Where(t => projectDate.Date.Ticks >= t.FromTicks && projectDate.Date.Ticks <= t.ToTicks).FirstOrDefault();
        }
        private List<dtoTimeGroup> GetAvailableTimeGroups(List<DateTime> pDates)
        {
            return GetAvailableTimeGroups(Service.GenerateTimeGroups(), pDates).ToList();
        }
        private List<dtoTimeGroup> GetAvailableTimeGroups(List<dtoTimeGroup> tGroups, List<DateTime> pDates)
        {
            return tGroups.Where(t => pDates.Where(d => d.Ticks >= t.FromTicks && d.Ticks <= t.ToTicks).Any()).ToList();
        }
        private Int32 GetCurrentPageIndex(Int32 pageSize, List<dtoPlainProject> projects) {
            Int32 pageIndex = GetCurrentPageIndex(pageSize, projects, TimeGroup.Today);
            if (pageIndex == -1) {
                pageIndex = GetCurrentPageIndex(pageSize, projects, TimeGroup.ThisWeek);
                if (pageIndex == -1)
                {
                    pageIndex = GetCurrentPageIndex(pageSize, projects, TimeGroup.ThisMonth);
                    if (pageIndex == -1)
                    {
                        pageIndex = GetCurrentPageIndex(pageSize, projects, TimeGroup.ThisQuarter);
                        if (pageIndex == -1)
                        {
                            pageIndex = GetCurrentPageIndex(pageSize, projects, TimeGroup.ThisHalfYear);
                            if (pageIndex == -1)
                            {
                                pageIndex = GetCurrentPageIndex(pageSize, projects, TimeGroup.PreviousDays);
                                if (pageIndex == -1)
                                {
                                    pageIndex = GetCurrentPageIndex(pageSize, projects, TimeGroup.PreviousWeek);
                                    if (pageIndex == -1)
                                    {
                                        pageIndex = GetCurrentPageIndex(pageSize, projects, TimeGroup.PreviousWeeks);
                                        if (pageIndex == -1)
                                        {
                                            pageIndex = GetCurrentPageIndex(pageSize, projects, TimeGroup.PreviousMonth);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (pageIndex == -1)
                pageIndex = 0;
            return pageIndex;
        }
        private Int32 GetCurrentPageIndex(Int32 pageSize, List<dtoPlainProject> projects , TimeGroup timeGroup)
        {
            Int32 pageIndex = 0;
            if (projects.Where(p => p.TimeGroup != null && p.TimeGroup.TimeLine == timeGroup).Any()) {
                while (projects.Skip(pageIndex * pageSize).Take(pageSize).Any() && !projects.Skip(pageIndex * pageSize).Take(pageSize).Where(p => p.TimeGroup != null && p.TimeGroup.TimeLine == timeGroup).Any()) {
                    pageIndex++;
                }
                return (projects.Skip(pageIndex * pageSize).Take(pageSize).Any() ? pageIndex : -1);
            }
            else
                return -1;            
        }
        private void UpdateProjects(String idFatherRow, List<dtoPlainProject> projects, Int32 idContainerCommunity, PageListType currentPage, SummaryTimeLine timeline)
        {
            foreach (dtoPlainProject p in projects)
            {
                p.IdFatherRow = idFatherRow;
                p.Urls.Edit = RootObject.EditProject(p.Id, p.IdCommunity, p.isPortal, p.isPersonal, currentPage, idContainerCommunity);
                p.Urls.EditResources = RootObject.ProjectResources(p.Id, p.IdCommunity, p.isPortal, p.isPersonal, currentPage, idContainerCommunity);
                p.Urls.PhisicalDelete = RootObject.PhisicalDeleteProject(p.Id, p.IdCommunity, p.isPortal, p.isPersonal, currentPage, idContainerCommunity);
                switch (currentPage)
                {
                    case PageListType.ListResource:
                        p.Urls.ProjectUrl = RootObject.ProjectDashboard(p.IdCommunity, p.isPortal, p.isPersonal, idContainerCommunity, p.Id, currentPage, PageListType.ProjectDashboardResource, timeline, GetDefaultUserActivityStatus(p), timeline);
                        break;
                    case PageListType.ListManager:
                        p.Urls.ProjectUrl = RootObject.ProjectDashboard(p.IdCommunity, p.isPortal, p.isPersonal, idContainerCommunity, p.Id, currentPage, PageListType.DashboardManager, timeline, GetDefaultUserActivityStatus(p), timeline);
                        break;
                    case PageListType.ListAdministrator:
                        p.Urls.ProjectUrl = RootObject.ProjectDashboard(p.IdCommunity, p.isPortal, p.isPersonal, idContainerCommunity, p.Id, currentPage, PageListType.DashboardManager, timeline, GetDefaultUserActivityStatus(p), timeline);
                        break;
                }
                if (p.Permissions.EditMap)
                    p.Urls.ProjectMap = RootObject.ProjectMap(p.Id, p.IdCommunity, p.isPortal, p.isPersonal, currentPage, idContainerCommunity);
                else if (p.Permissions.ViewMap)
                    p.Urls.ProjectMap = RootObject.ViewProjectMap(p.Id, p.IdCommunity, p.isPortal, p.isPersonal, currentPage, idContainerCommunity);
            }
        }
        private UserActivityStatus GetDefaultUserActivityStatus(dtoPlainProject p) {
            if (p.UserCompletion == null || !p.UserCompletion.Values.Where(v => v > 0).Any())
                return UserActivityStatus.Ignore;
            else {
                if (p.UserCompletion.ContainsKey(ResourceActivityStatus.late) && p.UserCompletion[ResourceActivityStatus.late] > 0)
                    return UserActivityStatus.Expired;
                else if (p.UserCompletion.ContainsKey(ResourceActivityStatus.started) && p.UserCompletion[ResourceActivityStatus.started] > 0)
                    return UserActivityStatus.Expiring;
                else if (p.UserCompletion.ContainsKey(ResourceActivityStatus.notstarted) && p.UserCompletion[ResourceActivityStatus.notstarted] > 0)
                    return UserActivityStatus.Starting;
                else
                    return UserActivityStatus.ToDo;
            }
        }
        public Boolean VirtualDeleteProject(long idProject, dtoItemsFilter filter, ItemsGroupBy groupBy, PageListType currentPage, Int32 idCommunity, Int32 pageIndex, Int32 pageSize)
        {
            Boolean result = false;
            if (UserContext.isAnonymous)
            {
                View.DisplayPager(false);
                View.LoadedNoProjects(currentPage);
            }
            else{
                result = Service.SetProjectVirtualDelete(idProject, true);
                if (result)
                    View.SendUserAction(View.IdCurrentCommunityForList, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectVirtualDelete);
                LoadProjects(Service.GetProjects(UserContext.CurrentUserID, filter, currentPage, idCommunity, View.RoleTranslations), groupBy, currentPage, filter.TimeLine, pageIndex, pageSize);
            }
            return result;
        }
        public Boolean VirtualUndeleteProject(long idProject, dtoItemsFilter filter, ItemsGroupBy groupBy, PageListType currentPage,  Int32 idCommunity, Int32 pageIndex, Int32 pageSize, ref long deleted)
        {
            Boolean result = false;
            if (UserContext.isAnonymous)
            {
                View.DisplayPager(false);
                View.LoadedNoProjects(currentPage);
            }
            else
            {
                result = Service.SetProjectVirtualDelete(idProject, false);
                if (result)
                    View.SendUserAction(View.IdCurrentCommunityForList, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectVirtualUndelete);
                List<dtoPlainProject> projects = Service.GetProjects(UserContext.CurrentUserID, filter, currentPage, idCommunity, View.RoleTranslations);
                deleted = projects.Count;
                if (deleted>0)
                    LoadProjects(Service.GetProjects(UserContext.CurrentUserID, filter, currentPage, idCommunity, View.RoleTranslations), groupBy, currentPage, filter.TimeLine, pageIndex, pageSize);
            }
            return result;
        }
        private ModuleProjectManagement.ActionType GetDefaultAction(PageListType currentPage)
        {
            switch (currentPage)
            {
                case PageListType.ListResource:
                    return ModuleProjectManagement.ActionType.LoadProjectsPlainAsResource;
                case PageListType.ListManager:
                    return ModuleProjectManagement.ActionType.LoadProjectsPlainAsManager;
                case PageListType.ListAdministrator:
                    return ModuleProjectManagement.ActionType.LoadProjectsPlainAsAdministrator;
                default:
                    return ModuleProjectManagement.ActionType.LoadProjectsPlain;
            }
        }
    }
}