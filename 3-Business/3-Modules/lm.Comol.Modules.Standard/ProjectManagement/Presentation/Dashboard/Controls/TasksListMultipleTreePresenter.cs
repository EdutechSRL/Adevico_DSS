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
    public class TasksListMultipleTreePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewTasksListMultipleTree View
            {
                get { return (IViewTasksListMultipleTree)base.View; }
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
            public TasksListMultipleTreePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public TasksListMultipleTreePresenter(iApplicationContext oContext, IViewTasksListMultipleTree view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(dtoProjectContext context, Int32 idContainerCommunity, dtoItemsFilter filter, PageContainerType container, PageListType fromPage, PageListType currentPage,String unknownUser) 
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
   
                View.CurrentOrderBy = ProjectOrderBy.CommunityName;
                View.CurrentAscending = true;
                filter.OrderBy = ProjectOrderBy.CommunityName;
                filter.Ascending = true;
                LoadTasks(unknownUser,context, idContainerCommunity, filter, container, fromPage, currentPage, filter.PageIndex, filter.PageSize);
            }
        }
        public void LoadTasks(String unknownUser, dtoProjectContext context, Int32 idContainerCommunity, dtoItemsFilter filter, PageContainerType container, PageListType fromPage, PageListType currentPage, Int32 pageIndex, Int32 pageSize)
        {
            if (UserContext.isAnonymous)
            {
                View.DisplayPager(false);
                View.LoadedNoTasks();
            }
            else
                LoadTasks(Service.GetTasks(UserContext.CurrentUserID, filter, context.IdCommunity, container, fromPage, currentPage),unknownUser, context,idContainerCommunity, filter, container, fromPage, currentPage, pageIndex, pageSize);
        }
        private void LoadTasks(List<dtoPlainTask> tasks, String unknownUser, dtoProjectContext context, Int32 idContainerCommunity, dtoItemsFilter filter, PageContainerType container, PageListType fromPage, PageListType currentPage, Int32 pageIndex, Int32 pageSize)
        {
            if (pageIndex < 0)
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
            GroupTasks(tasks, unknownUser,context, idContainerCommunity, filter, container, fromPage, currentPage, filter.TimeLine, pageIndex, pageSize, (pageIndex > -1 ? null : pager));
        }
        private void GroupTasks(List<dtoPlainTask> tasks, String unknownUser, dtoProjectContext context, Int32 idContainerCommunity, dtoItemsFilter filter, PageContainerType container, PageListType fromPage, PageListType currentPage, SummaryTimeLine timeline, Int32 pageIndex, Int32 pageSize, PagerBase pager = null)
        {
            if (tasks.Skip((pageIndex < 0 ? 0 : pageIndex) * pageSize).Take(pageSize).Any())
            {
                Dictionary<long, List<dtoResource>> resources = tasks.Select(t => t.ProjectInfo.Id).Distinct().ToDictionary(t => t, t => Service.GetAvailableResources(t, unknownUser));


                String pName = View.GetPortalName();
                String cName = View.GetUnknownCommunityName();
                tasks.Where(p => p.ProjectInfo.IdCommunity == 0).ToList().ForEach(p => p.ProjectInfo.CommunityName = pName);
                tasks.Where(p => p.ProjectInfo.IdCommunity < 0).ToList().ForEach(p => p.ProjectInfo.CommunityName = cName);
                tasks.ForEach(t => t.ProjectResources = (resources.ContainsKey(t.ProjectInfo.Id) ? resources[t.ProjectInfo.Id] : new List<dtoResource>()));

                View.LoadTasks(GenerateCommunityProjectTree(tasks, context, idContainerCommunity, filter, container, fromPage, currentPage, timeline, pageIndex, pageSize));
            }
            else
                View.LoadedNoTasks();
        }
        private List<dtoCommunityProjectTasksGroup> GenerateCommunityProjectTree(List<dtoPlainTask> tasks, dtoProjectContext context, Int32 idContainerCommunity, dtoItemsFilter filter, PageContainerType container, PageListType fromPage, PageListType currentPage, SummaryTimeLine timeline, Int32 pageIndex, Int32 pageSize)
        {
            List<dtoCommunityProjectTasksGroup> items = new List<dtoCommunityProjectTasksGroup>();
            Int32 cCount = View.GetCellsCount(currentPage);
            String sRow = View.GetStartRowId(ItemsGroupBy.Community);
            tasks.OrderBy(p => p.ProjectInfo.CommunityName).Skip(pageIndex * pageSize).Take(pageSize).GroupBy(p => p.ProjectInfo.VirtualCommunityName).OrderBy(p => p.Key).ToList().ForEach(
                p => items.Add(new dtoCommunityProjectTasksGroup()
                {
                    Id = p.FirstOrDefault().ProjectInfo.IdCommunity,
                    Name = p.FirstOrDefault().ProjectInfo.CommunityName,
                    Projects = GenerateProjectTree(p.ToList(), tasks, context, idContainerCommunity, filter, container, fromPage, currentPage, timeline, pageIndex, pageSize),
                    CellsCount = cCount,
                    IdRowFirstItem = sRow + p.First().Id.ToString(),
                    IdRowLastItem = sRow + p.Last().Id.ToString(),
                    IdRow = sRow + p.FirstOrDefault().ProjectInfo.IdCommunity.ToString()
                }));
            long idFirstCommunity = items.First().Projects.Select(p => p.ProjectInfo.IdCommunity).FirstOrDefault();
            long idLastCommunity = items.Last().Projects.Select(p => p.ProjectInfo.IdCommunity).FirstOrDefault();
            if (pageIndex > 0  && !items.Where(i=> i.Projects.Where(p=> p.PreviousPageIndex != -1).Any()).Any()  && tasks.OrderBy(p => p.ProjectInfo.VirtualCommunityName).Skip((pageIndex - 1) * pageSize).Where(p => p.ProjectInfo.IdCommunity == idFirstCommunity).Any())
            {
                items.First().PreviousPageIndex = pageIndex - 1;
            }
            if (!items.Where(i => i.Projects.Where(p => p.NextPageIndex != -1).Any()).Any() && tasks.OrderBy(p => p.ProjectInfo.VirtualCommunityName).Skip((pageIndex + 1) * pageSize).Where(p => p.ProjectInfo.IdCommunity == idLastCommunity).Any())
            {
                items.Last().NextPageIndex = pageIndex + 1;
            }

            return items;
        }
        private List<dtoTasksGroup> GenerateProjectTree(List<dtoPlainTask> projectTasks, List<dtoPlainTask> availableTasks, dtoProjectContext context, Int32 idContainerCommunity, dtoItemsFilter filter, PageContainerType container, PageListType fromPage, PageListType currentPage, SummaryTimeLine timeline, Int32 pageIndex, Int32 pageSize)
        {
            List<dtoTasksGroup> items = new List<dtoTasksGroup>();
            String cssClass = View.GetContainerCssClass(ItemsGroupBy.Project);
            Int32 cCount = View.GetCellsCount(currentPage);
            String sRow = View.GetStartRowId(ItemsGroupBy.Project);
            projectTasks.OrderBy(p => p.ProjectInfo.VirtualProjectName).GroupBy(p => p.ProjectInfo.VirtualProjectName).OrderBy(p => p.Key).ToList().ForEach(
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
            items.ForEach(i => UpdateGroups(i, context, idContainerCommunity, filter, container, fromPage, currentPage, timeline));
            long idFirstProject = items.First().Tasks.Select(p => p.ProjectInfo.Id).FirstOrDefault();
            long idLastProject = items.Last().Tasks.Select(p => p.ProjectInfo.Id).FirstOrDefault();
            if (pageIndex > 0 && availableTasks.OrderBy(p => p.ProjectInfo.VirtualProjectName).Skip((pageIndex - 1) * pageSize).Where(p => p.ProjectInfo.Id == idFirstProject).Any())
            {
                items.First().PreviousPageIndex = pageIndex - 1;
            }
            if (availableTasks.OrderBy(p => p.ProjectInfo.VirtualProjectName).Skip((pageIndex + 1) * pageSize).Where(p => p.ProjectInfo.Id == idLastProject).Any())
            {
                items.Last().NextPageIndex = pageIndex + 1;
            }

            return items;
        }

        private Int32 GetCurrentPageIndex(Int32 pageSize, List<dtoPlainTask> tasks, long idProject)
        {
            Int32 pageIndex = 0;
            if (tasks.Where(p => p.ProjectInfo.Id == idProject).Any())
            {
                while (tasks.Skip(pageIndex * pageSize).Take(pageSize).Any() && !tasks.Skip(pageIndex * pageSize).Take(pageSize).Where(p => p.ProjectInfo.Id == idProject).Any())
                {
                    pageIndex++;
                }
                return (tasks.Skip(pageIndex * pageSize).Take(pageSize).Any() ? pageIndex : -1);
            }
            else
                return -1;            
        }
        private void UpdateGroups(dtoTasksGroup item,  dtoProjectContext context, Int32 idContainerCommunity, dtoItemsFilter filter, PageContainerType container, PageListType fromPage, PageListType currentPage, SummaryTimeLine timeline)
        {
            foreach (dtoPlainTask task in item.Tasks)
            {
                task.IdFatherRow = item.IdRow;
                if (currentPage != PageListType.ProjectDashboardManager && currentPage != PageListType.ProjectDashboardResource)
                    task.ProjectInfo.ProjectDashboard = RootObject.ProjectDashboard(context, idContainerCommunity, task.ProjectInfo.Id, currentPage, currentPage, filter.TimeLine);
            }

            item.ProjectInfo= item.Tasks.First().ProjectInfo;
            if (item.HasPermission(PmActivityPermission.ManageProject))
            {
                item.ProjectInfo.ProjectUrls.Edit = RootObject.EditProject(item.ProjectInfo.Id, item.ProjectInfo.IdCommunity, item.ProjectInfo.isPortal, item.ProjectInfo.isPersonal, currentPage, idContainerCommunity);
                item.ProjectInfo.ProjectUrls.ProjectMap = RootObject.ProjectMap(item.ProjectInfo.Id, item.ProjectInfo.IdCommunity, item.ProjectInfo.isPortal, item.ProjectInfo.isPersonal, currentPage, idContainerCommunity);
            }
            else if (item.HasPermission(PmActivityPermission.ViewProjectMap))
                item.ProjectInfo.ProjectUrls.ProjectMap = RootObject.ViewProjectMap(item.ProjectInfo.Id, item.ProjectInfo.IdCommunity, item.ProjectInfo.isPortal, item.ProjectInfo.isPersonal, currentPage, idContainerCommunity);

        }


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
                    if (savedTasks > 0)
                    {
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
                    action = ModuleProjectManagement.ActionType.LoadProjectTasksGroupByCommunityProjectAsManager;
                    break;
                case PageListType.ProjectDashboardResource:
                    action = ModuleProjectManagement.ActionType.LoadProjectTasksGroupByCommunityProjectAsResource;
                    break;
                case PageListType.DashboardAdministrator:
                    action = ModuleProjectManagement.ActionType.LoadTasksGroupByCommunityProjectAsAdministrator;
                    break;
                case PageListType.DashboardManager:
                    action = ModuleProjectManagement.ActionType.LoadTasksGroupByCommunityProjectAsManager;
                    break;
                case PageListType.DashboardResource:
                    action = ModuleProjectManagement.ActionType.LoadTasksGroupByCommunityProjectAsResource;
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