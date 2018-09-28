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
    public class TasksListTreePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewTasksListTree View
            {
                get { return (IViewTasksListTree)base.View; }
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
            public TasksListTreePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public TasksListTreePresenter(iApplicationContext oContext, IViewTasksListTree view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(dtoProjectContext context, Int32 idContainerCommunity, dtoItemsFilter filter, PageContainerType container, PageListType fromPage, PageListType currentPage, String unknownUser) 
        {
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
            View.PageContainer = container;
            View.PageType = currentPage;
            View.CurrentFromPage = fromPage;
            View.IdContainerCommunity = idContainerCommunity;
            View.PageContext = context;
            if (UserContext.isAnonymous || p == null)
            {
                View.DisplayPager(false);
                View.LoadedNoTasks();
            }
            else
            {
                if (!View.IsInitialized && filter.GroupBy == ItemsGroupBy.EndDate)
                    filter.PageIndex = -1;
                View.IsInitialized = true;
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
                    case ItemsGroupBy.Project:
                        View.CurrentOrderBy = ProjectOrderBy.Name;
                        View.CurrentAscending = true;
                        filter.OrderBy = ProjectOrderBy.Name;
                        filter.Ascending = true;
                        break;
                }
                View.CurrentGroupBy = filter.GroupBy;
                LoadTasks(unknownUser,context, idContainerCommunity, filter, container, fromPage, currentPage, filter.GroupBy, filter.PageIndex, filter.PageSize );
            }
        }
        public void LoadTasks(String unknownUser,dtoProjectContext context, Int32 idContainerCommunity, dtoItemsFilter filter, PageContainerType container, PageListType fromPage, PageListType currentPage, ItemsGroupBy groupBy, Int32 pageIndex, Int32 pageSize)
        {
            if (UserContext.isAnonymous)
            {
                View.DisplayPager(false);
                View.LoadedNoTasks();
            }
            else
                LoadTasks(Service.GetTasks(UserContext.CurrentUserID, filter, context.IdCommunity, container, fromPage, currentPage), unknownUser, context, idContainerCommunity, filter, container, fromPage, currentPage, groupBy, pageIndex, pageSize);
        }
        private void LoadTasks(List<dtoPlainTask> tasks, String unknownUser, dtoProjectContext context, Int32 idContainerCommunity, dtoItemsFilter filter, PageContainerType container, PageListType fromPage, PageListType currentPage, ItemsGroupBy groupBy, Int32 pageIndex, Int32 pageSize)
        {
            if (pageIndex < 0 && groupBy != ItemsGroupBy.EndDate)
                pageIndex = 0;

            PagerBase pager = new PagerBase();
            pager.PageSize = pageSize;//Me.View.CurrentPageSize
            pager.Count = (tasks.Count > 0) ? tasks.Count - 1 : 0;

            if (pageIndex > -1)
            {
                pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
                pageIndex = (pageIndex > pager.PageIndex && pager.PageIndex >= 0) ? pager.PageIndex : pageIndex;
                View.Pager = pager;
            }
            GroupTasks(tasks, unknownUser,context, idContainerCommunity, filter, container, fromPage, currentPage, groupBy, filter.TimeLine, pageIndex, pageSize, (pageIndex > -1 ? null : pager));
            SendDefaultAction(currentPage, filter.IdProject, context.IdCommunity);
        }
        private void GroupTasks(List<dtoPlainTask> tasks, String unknownUser, dtoProjectContext context, Int32 idContainerCommunity, dtoItemsFilter filter, PageContainerType container, PageListType fromPage, PageListType currentPage, ItemsGroupBy groupBy, SummaryTimeLine timeline, Int32 pageIndex, Int32 pageSize, PagerBase pager = null)
        {
            if (tasks.Skip((pageIndex < 0 ? 0 : pageIndex) * pageSize).Take(pageSize).Any())
            {
                Dictionary<long, List<dtoResource>> resources = tasks.Select(t => t.ProjectInfo.Id).Distinct().ToDictionary(t => t, t => Service.GetAvailableResources(t, unknownUser));

                String pName = View.GetPortalName();
                String cName = View.GetUnknownCommunityName();
                tasks.Where(p => p.ProjectInfo.IdCommunity == 0).ToList().ForEach(p => p.ProjectInfo.CommunityName = pName);
                tasks.Where(p => p.ProjectInfo.IdCommunity < 0).ToList().ForEach(p => p.ProjectInfo.CommunityName = cName);
                tasks.ForEach(t => t.ProjectResources = (resources.ContainsKey(t.ProjectInfo.Id) ? resources[t.ProjectInfo.Id] : new List<dtoResource>()));

                List<dtoTasksGroup> items = new List<dtoTasksGroup>();
                switch (groupBy) { 
                    case ItemsGroupBy.Community:
                        items = GenerateCommunityTree(tasks, unknownUser, context, idContainerCommunity, filter, container, fromPage, currentPage, timeline, pageIndex, pageSize);
                        break;
                    case ItemsGroupBy.EndDate:
                        items = GenerateEndDateTree(tasks, unknownUser, context, idContainerCommunity, filter, container, fromPage, currentPage, timeline, pageIndex, pageSize, pager);
                        break;
                    case ItemsGroupBy.Project:
                        items = GenerateProjectTree(tasks, unknownUser,context, idContainerCommunity, filter, container, fromPage, currentPage, timeline, pageIndex, pageSize);
                        break;
                }
                View.LoadTasks(items);
            }
            else
                View.LoadedNoTasks();
        }
        private List<dtoTasksGroup> GenerateCommunityTree(List<dtoPlainTask> tasks, String unknownUser, dtoProjectContext context, Int32 idContainerCommunity, dtoItemsFilter filter, PageContainerType container, PageListType fromPage, PageListType currentPage, SummaryTimeLine timeline, Int32 pageIndex, Int32 pageSize)
        {
            List<dtoTasksGroup> items = new List<dtoTasksGroup>();
            String cssClass = View.GetContainerCssClass(ItemsGroupBy.Community);
            Int32 cCount = View.GetCellsCount(currentPage, ItemsGroupBy.Community);
            String sRow = View.GetStartRowId(ItemsGroupBy.Community);
            tasks.OrderBy(p => p.ProjectInfo.CommunityName).Skip(pageIndex * pageSize).Take(pageSize).GroupBy(p => p.ProjectInfo.VirtualCommunityName).OrderBy(p => p.Key).ToList().ForEach(
                p => items.Add(new dtoTasksGroup()
                {
                    Id = p.FirstOrDefault().ProjectInfo.IdCommunity,
                    Name = p.FirstOrDefault().ProjectInfo.CommunityName,
                    Tasks = p.ToList(),
                    CssClass =cssClass,
                    CellsCount = cCount,
                    IdRowFirstItem = sRow + p.First().Id.ToString(),
                    IdRowLastItem = sRow + p.Last().Id.ToString(),
                    IdRow = sRow + p.FirstOrDefault().ProjectInfo.IdCommunity.ToString()
                }));
            items.ForEach(i => UpdateGroups(i, ItemsGroupBy.Community, context, idContainerCommunity, filter, container, fromPage,currentPage, timeline));
            long idFirstCommunity = items.First().Tasks.Select(p => p.ProjectInfo.IdCommunity).FirstOrDefault();
            long idLastCommunity = items.Last().Tasks.Select(p => p.ProjectInfo.IdCommunity).FirstOrDefault();
            if (pageIndex > 0 && tasks.OrderBy(p => p.ProjectInfo.VirtualCommunityName).Skip((pageIndex - 1) * pageSize).Where(p => p.ProjectInfo.IdCommunity == idFirstCommunity).Any())
            {
                items.First().PreviousPageIndex = pageIndex - 1;
            }
            if (tasks.OrderBy(p => p.ProjectInfo.VirtualCommunityName).Skip((pageIndex + 1) * pageSize).Where(p => p.ProjectInfo.IdCommunity == idLastCommunity).Any())
            {
                items.Last().NextPageIndex = pageIndex + 1;
            }

            return items;
        }
        private List<dtoTasksGroup> GenerateEndDateTree(List<dtoPlainTask> tasks, String unknownUser, dtoProjectContext context, Int32 idContainerCommunity, dtoItemsFilter filter, PageContainerType container, PageListType fromPage, PageListType currentPage, SummaryTimeLine timeline, Int32 pageIndex, Int32 pageSize, PagerBase pager)
        {
            List<dtoTasksGroup> items = new List<dtoTasksGroup>();
            String cssClass = View.GetContainerCssClass(ItemsGroupBy.EndDate);
            Int32 cCount = View.GetCellsCount(currentPage, ItemsGroupBy.EndDate);
            String sRow = View.GetStartRowId(ItemsGroupBy.EndDate);

            Dictionary<TimeGroup,String> timeTranslations = View.GetTimeTranslations();
            List<dtoTimeGroup> timegroups = GetAvailableTimeGroups(tasks.Select(p => p.VirtualEndDate).ToList());
            foreach (dtoPlainTask p in tasks)
            {
               p.TimeGroup = GetTimeGroupByDate(timegroups, p.VirtualEndDate);
            }
            if (pageIndex == -1 && pager!=null)
            {
                pageIndex = GetCurrentPageIndex(pageSize, tasks);
                pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
                View.Pager = pager;
            }
            String dateTimePattern = View.GetDateTimePattern();
            Dictionary<Int32, String> monthNames = View.GetMonthNames();
            if (tasks.Skip(pageIndex * pageSize).Take(pageSize).Any())
            {
                foreach (dtoTimeGroup t in GetAvailableTimeGroups(timegroups, tasks.Skip(pageIndex * pageSize).Take(pageSize).Select(p => p.VirtualEndDate).ToList()))
                {
                    items.Add(new dtoTasksGroup()
                    {
                        Id = ((long)t.TimeLine),
                        Name = GetTimeGroupDisplayName(timeTranslations[t.TimeLine], t, dateTimePattern, monthNames),
                        Tasks = tasks.Where(p => p.VirtualEndDate.Ticks >= t.FromTicks && p.VirtualEndDate.Ticks <= t.ToTicks).ToList(),
                        CssClass = cssClass,
                        CellsCount = cCount,
                        IdRowFirstItem = sRow + tasks.Where(p => p.VirtualEndDate.Ticks >= t.FromTicks && p.VirtualEndDate.Ticks <= t.ToTicks).First().Id.ToString(),
                        IdRowLastItem = sRow + tasks.Where(p => p.VirtualEndDate.Ticks >= t.FromTicks && p.VirtualEndDate.Ticks <= t.ToTicks).Last().Id.ToString(),
                        IdRow = sRow + ((long)t.TimeLine).ToString(),
                        Time = t
                    });
                }
                items.ForEach(i => UpdateGroups(i, ItemsGroupBy.EndDate, context, idContainerCommunity, filter, container, fromPage, currentPage, timeline));
                TimeGroup firstTimeGroup = items.First().Time.TimeLine;
                TimeGroup lastTimeGroup = items.Last().Time.TimeLine;
                if (pageIndex > 0 && tasks.OrderBy(p => p.VirtualEndDate).Skip((pageIndex - 1) * pageSize).Where(p => p.TimeGroup.TimeLine == firstTimeGroup).Any())
                {
                    items.First().PreviousPageIndex = pageIndex - 1;
                }
                if (tasks.OrderBy(p => p.VirtualEndDate).Skip((pageIndex + 1) * pageSize).Where(p => p.TimeGroup.TimeLine == lastTimeGroup).Any())
                {
                    items.Last().NextPageIndex = pageIndex + 1;
                }
            }
            return items;
        }
        private List<dtoTasksGroup> GenerateProjectTree(List<dtoPlainTask> tasks, String unknownUser, dtoProjectContext context, Int32 idContainerCommunity, dtoItemsFilter filter, PageContainerType container, PageListType fromPage, PageListType currentPage, SummaryTimeLine timeline, Int32 pageIndex, Int32 pageSize)
        {
            List<dtoTasksGroup> items = new List<dtoTasksGroup>();
            String cssClass = View.GetContainerCssClass(ItemsGroupBy.Project);
            Int32 cCount = View.GetCellsCount(currentPage, ItemsGroupBy.Project);
            String sRow = View.GetStartRowId(ItemsGroupBy.Project);
            tasks.OrderBy(p => p.ProjectInfo.VirtualProjectName).Skip(pageIndex * pageSize).Take(pageSize).GroupBy(p => p.ProjectInfo.VirtualProjectName).OrderBy(p => p.Key).ToList().ForEach(
                p => items.Add(new dtoTasksGroup()
                {
                    Id = p.FirstOrDefault().ProjectInfo.Id ,
                    Name = p.FirstOrDefault().ProjectInfo.Name,
                    Tasks = p.ToList(),
                    CssClass = cssClass,
                    CellsCount = cCount,
                    IdRowFirstItem = sRow + p.First().Id.ToString(),
                    IdRowLastItem = sRow + p.Last().Id.ToString(),
                    IdRow = sRow + p.FirstOrDefault().ProjectInfo.Id.ToString()
                }));
            items.ForEach(i => UpdateGroups(i, ItemsGroupBy.Project, context, idContainerCommunity, filter, container, fromPage, currentPage, timeline));
            long idFirstProject = items.First().Tasks.Select(p => p.ProjectInfo.Id).FirstOrDefault();
            long idLastProject = items.Last().Tasks.Select(p => p.ProjectInfo.Id).FirstOrDefault();
            if (pageIndex > 0 && tasks.OrderBy(p => p.ProjectInfo.VirtualProjectName).Skip((pageIndex - 1) * pageSize).Where(p => p.ProjectInfo.Id == idFirstProject).Any())
            {
                items.First().PreviousPageIndex = pageIndex - 1;
            }
            if (tasks.OrderBy(p => p.ProjectInfo.VirtualProjectName).Skip((pageIndex + 1) * pageSize).Where(p => p.ProjectInfo.Id  == idLastProject).Any())
            {
                items.Last().NextPageIndex = pageIndex + 1;
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
        private Int32 GetCurrentPageIndex(Int32 pageSize, List<dtoPlainTask> tasks) {
            Int32 pageIndex = GetCurrentPageIndex(pageSize, tasks, TimeGroup.Today);
            if (pageIndex == -1) {
                pageIndex = GetCurrentPageIndex(pageSize, tasks, TimeGroup.ThisWeek);
                if (pageIndex == -1)
                {
                    pageIndex = GetCurrentPageIndex(pageSize, tasks, TimeGroup.ThisMonth);
                    if (pageIndex == -1)
                    {
                        pageIndex = GetCurrentPageIndex(pageSize, tasks, TimeGroup.ThisQuarter);
                        if (pageIndex == -1)
                        {
                            pageIndex = GetCurrentPageIndex(pageSize, tasks, TimeGroup.ThisHalfYear);
                            if (pageIndex == -1)
                            {
                                pageIndex = GetCurrentPageIndex(pageSize, tasks, TimeGroup.PreviousDays);
                                if (pageIndex == -1)
                                {
                                    pageIndex = GetCurrentPageIndex(pageSize, tasks, TimeGroup.PreviousWeek);
                                    if (pageIndex == -1)
                                    {
                                        pageIndex = GetCurrentPageIndex(pageSize, tasks, TimeGroup.PreviousWeeks);
                                        if (pageIndex == -1)
                                        {
                                            pageIndex = GetCurrentPageIndex(pageSize, tasks, TimeGroup.PreviousMonth);
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
        private Int32 GetCurrentPageIndex(Int32 pageSize, List<dtoPlainTask> tasks, TimeGroup timeGroup)
        {
            Int32 pageIndex = 0;
            if (tasks.Where(p => p.TimeGroup != null && p.TimeGroup.TimeLine == timeGroup).Any()) {
                while (tasks.Skip(pageIndex * pageSize).Take(pageSize).Any() && !tasks.Skip(pageIndex * pageSize).Take(pageSize).Where(p => p.TimeGroup != null && p.TimeGroup.TimeLine == timeGroup).Any()) {
                    pageIndex++;
                }
                return (tasks.Skip(pageIndex * pageSize).Take(pageSize).Any() ? pageIndex : -1);
            }
            else
                return -1;            
        }
        private void UpdateGroups(dtoTasksGroup item, ItemsGroupBy groupBy, dtoProjectContext context, Int32 idContainerCommunity, dtoItemsFilter filter, PageContainerType container, PageListType fromPage, PageListType currentPage, SummaryTimeLine timeline)
        {
           
            foreach (dtoPlainTask task in item.Tasks)
            {
                task.IdFatherRow = item.IdRow;
                if (currentPage != PageListType.ProjectDashboardManager && currentPage != PageListType.ProjectDashboardResource)
                    task.ProjectInfo.ProjectDashboard = RootObject.ProjectDashboard(context, idContainerCommunity, task.ProjectInfo.Id, currentPage, currentPage, filter.TimeLine);
            }
            switch(groupBy){
                case ItemsGroupBy.Project:
                    item.ProjectInfo= item.Tasks.First().ProjectInfo;
                    if (item.HasPermission(PmActivityPermission.ManageProject))
                    {
                        item.ProjectInfo.ProjectUrls.Edit = RootObject.EditProject(item.ProjectInfo.Id, item.ProjectInfo.IdCommunity, item.ProjectInfo.isPortal, item.ProjectInfo.isPersonal, currentPage, idContainerCommunity);
                        item.ProjectInfo.ProjectUrls.ProjectMap = RootObject.ProjectMap(item.ProjectInfo.Id, item.ProjectInfo.IdCommunity, item.ProjectInfo.isPortal, item.ProjectInfo.isPersonal, currentPage, idContainerCommunity);
                    }
                    else if (item.HasPermission(PmActivityPermission.ViewProjectMap))
                        item.ProjectInfo.ProjectUrls.ProjectMap = RootObject.ViewProjectMap(item.ProjectInfo.Id, item.ProjectInfo.IdCommunity, item.ProjectInfo.isPortal, item.ProjectInfo.isPersonal, currentPage, idContainerCommunity);
                    break;
            }
        }
        //private void UpdateGroups(dtoProjectTasksGroup item, ItemsGroupBy groupBy, dtoProjectContext context, Int32 idContainerCommunity, dtoItemsFilter filter, PageContainerType container, PageListType fromPage, PageListType currentPage, SummaryTimeLine timeline)
        //{
        //    foreach (dtoPlainTask task in item.Tasks)
        //    {
        //        task.IdFatherRow = item.IdRow;
        //    }

        //    item.CommunityName = item.Tasks.FirstOrDefault().CommunityName;
        //    item.SetMyCompletion = item.Tasks.FirstOrDefault().SetMyCompletion;
        //    item.SetOthersCompletion = item.Tasks.FirstOrDefault().SetOthersCompletion;
        //    item.ViewMyCompletion = item.Tasks.FirstOrDefault().ViewMyCompletion;
        //    item.IdCommunity = item.Tasks.FirstOrDefault().IdCommunity;
        //    item.UserCompletion = item.Tasks.FirstOrDefault().UserCompletion;
        //    item.ProjectDashboard = RootObject.ProjectDashboard(context, idContainerCommunity, item.Id, currentPage, currentPage, filter.TimeLine);

        //    //        if 
        //    //        item.ProjectUrls.Edit = RootObject.EditProject(p.Id, p.IdCommunity, p.isPortal, p.isPersonal, currentPage, idContainerCommunity);
        //    //        if (item.ProjectPermissions.EditMap)
        //    //            item.ProjectUrls.ProjectMap = RootObject.ProjectMap(p.Id, p.IdCommunity, p.isPortal, p.isPersonal, currentPage, idContainerCommunity);
        //    //        else if (item.ProjectPermissions.ViewMap)
        //    //            item.ProjectUrls.ProjectMap = RootObject.ViewProjectMap(p.Id, p.IdCommunity, p.isPortal, p.isPersonal, currentPage, idContainerCommunity);
        //    //        break;
        //    //}
        //}

        public void SaveMyCompletions(List<dtoMyAssignmentCompletion> items)
        {
            if (UserContext.isAnonymous)
            {
                View.DisplayPager(false);
                View.LoadedNoTasks();
            }
            else
            {
                Int32 tasks = items.Where(t => t.MyCompletion.InEditMode).Count();
                Int32 savedTasks = 0;
                Int32 unsavedTasks = 0;
                Boolean updateSummary = false;
                if (tasks > 0)
                {
                    List<dtoMyAssignmentCompletion> toUpdate = items.Where(t => t.MyCompletion.InEditMode).ToList();
                    Service.UpdateTasksCompletion(toUpdate, ref savedTasks, ref unsavedTasks, ref updateSummary);
                    View.SendUserAction(View.IdContainerCommunity, CurrentIdModule, (unsavedTasks > 0) ? ModuleProjectManagement.ActionType.MyCompletionUnsaved : ModuleProjectManagement.ActionType.MyCompletionUnsaved);
                    if (savedTasks > 0 && View.CurrentGroupBy== ItemsGroupBy.Project) {
                        View.DisplayTasksCompletionSaved(toUpdate, Service.GetResourcesProjectCompletion(toUpdate.Select(i => i.IdResource).Distinct().ToList()), savedTasks, unsavedTasks, updateSummary);
                    }
                    else
                        View.DisplayTasksCompletionSaved(toUpdate, savedTasks, unsavedTasks, updateSummary);
                }
            }
        }

        private void SendDefaultAction(PageListType currentPage, long idProject, Int32 idCommunity)
        {
            ModuleProjectManagement.ActionType action = ModuleProjectManagement.ActionType.LoadTasks;
            switch (currentPage)
            {
                case PageListType.ProjectDashboardManager:
                    action = ModuleProjectManagement.ActionType.LoadProjectTasksGroupAsManager;
                    break;
                case PageListType.ProjectDashboardResource:
                    action = ModuleProjectManagement.ActionType.LoadProjectTasksGroupAsResource;
                    break;
                case PageListType.DashboardAdministrator:
                    action = ModuleProjectManagement.ActionType.LoadTasksGroupAsAdministrator;
                    break;
                case PageListType.DashboardManager:
                    action = ModuleProjectManagement.ActionType.LoadTasksGroupAsManager;
                    break;
                case PageListType.DashboardResource:
                    action = ModuleProjectManagement.ActionType.LoadTasksGroupAsResource;
                    break;
            }
            switch (currentPage)
            {
                case PageListType.ProjectDashboardManager:
                case PageListType.ProjectDashboardResource:
                    View.SendUserAction(idCommunity, CurrentIdModule, idProject, action);
                    break;
                default:
                    View.SendUserAction(View.IdContainerCommunity, CurrentIdModule, action);
                    break;
            }
        }
    }
}