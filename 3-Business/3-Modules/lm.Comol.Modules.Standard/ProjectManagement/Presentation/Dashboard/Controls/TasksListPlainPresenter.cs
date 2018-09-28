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
    public class TasksListPlainPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewTasksListPlain View
            {
                get { return (IViewTasksListPlain)base.View; }
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
            public TasksListPlainPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public TasksListPlainPresenter(iApplicationContext oContext, IViewTasksListPlain view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(dtoProjectContext context,Int32 idContainerCommunity, dtoItemsFilter filter, PageContainerType container, PageListType fromPage, PageListType currentPage, String unknownUser) 
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
                View.IsInitialized = true;
                if (filter.PageSize == 0)
                    filter.PageSize = View.CurrentPageSize;
                View.CurrentOrderBy = filter.OrderBy;
                View.CurrentAscending = filter.Ascending;
                LoadTasks(context, filter, container, fromPage, currentPage, filter.PageIndex, filter.PageSize, unknownUser);
            }
        }
        public void LoadTasks(dtoProjectContext context, dtoItemsFilter filter, PageContainerType container, PageListType fromPage, PageListType currentPage, Int32 pageIndex, Int32 pageSize, String unknownUser)
        {
            if (UserContext.isAnonymous)
            {
                View.DisplayPager(false);
                View.LoadedNoTasks();
            }
            else
                LoadTasks(Service.GetTasks(UserContext.CurrentUserID, filter, context.IdCommunity, container, fromPage, currentPage), context, filter, container, fromPage, currentPage, pageIndex, pageSize, unknownUser);
        }
        private void LoadTasks(List<dtoPlainTask> tasks, dtoProjectContext context, dtoItemsFilter filter, PageContainerType container, PageListType fromPage, PageListType currentPage, Int32 pageIndex, Int32 pageSize, String unknownUser)
        {
            Int32 idContainerCommunity = View.IdContainerCommunity;
            PagerBase pager = new PagerBase();
            pager.PageSize = pageSize;//Me.View.CurrentPageSize
            pager.Count = (tasks.Count > 0) ? tasks.Count - 1 : 0;
            pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
            View.Pager = pager;

            pageIndex = (pageIndex > pager.PageIndex && pager.PageIndex >= 0) ? pager.PageIndex : pageIndex;
            tasks = tasks.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            if (tasks.Any())
            {
                Dictionary<long, List<dtoResource>> resources = tasks.Select(t => t.ProjectInfo.Id).Distinct().ToDictionary(t => t, t => Service.GetAvailableResources(t, unknownUser));
                if (currentPage != PageListType.ProjectDashboardManager && currentPage != PageListType.ProjectDashboardResource)
                {
                    Int32 idContainer = View.IdContainerCommunity;
                    foreach (dtoPlainTask task in tasks.Where(t => t.ProjectInfo.Id > 0))
                    {
                        task.ProjectResources = (resources.ContainsKey(task.ProjectInfo.Id) ? resources[task.ProjectInfo.Id] : new List<dtoResource>());
                        task.ProjectInfo.ProjectDashboard = RootObject.ProjectDashboard(context, idContainer, task.ProjectInfo.Id, currentPage, currentPage, filter.TimeLine);
                    }
                }
                else
                {
                    foreach (dtoPlainTask task in tasks)
                    {
                        task.ProjectResources = (resources.ContainsKey(task.ProjectInfo.Id) ? resources[task.ProjectInfo.Id] : new List<dtoResource>());
                    }
                }
                View.LoadTasks(tasks);
            }
            else
                View.LoadedNoTasks();
            SendDefaultAction(currentPage, filter.IdProject, context.IdCommunity);
        }

        public void SaveMyCompletions(List<dtoMyAssignmentCompletion> items) {
            if (UserContext.isAnonymous)
            {
                View.DisplayPager(false);
                View.LoadedNoTasks();
            }
            else
            {
                Int32 tasks = items.Where(t=> t.MyCompletion.InEditMode).Count();
                Int32 savedTasks = 0;
                Int32 unsavedTasks = 0;
                Boolean updateSummary = false;

                if (tasks>0)
                {
                    Service.UpdateTasksCompletion(items.Where(t => t.MyCompletion.InEditMode).ToList(), ref savedTasks, ref unsavedTasks, ref updateSummary);
                    View.SendUserAction(View.IdContainerCommunity, CurrentIdModule, (unsavedTasks > 0) ? ModuleProjectManagement.ActionType.MyCompletionUnsaved : ModuleProjectManagement.ActionType.MyCompletionUnsaved);
                    View.DisplayTasksCompletionSaved(items, savedTasks, unsavedTasks, updateSummary);
                }
            }    
        }

        private void SendDefaultAction(PageListType currentPage, long idProject, Int32 idCommunity)
        {
            ModuleProjectManagement.ActionType action = ModuleProjectManagement.ActionType.LoadTasks;
            switch (currentPage)
            {
                case PageListType.ProjectDashboardManager:
                    action = ModuleProjectManagement.ActionType.LoadProjectTasksPlainAsManager;
                    break;
                case PageListType.ProjectDashboardResource:
                    action = ModuleProjectManagement.ActionType.LoadProjectTasksPlainAsResource;
                    break;
                case PageListType.DashboardAdministrator:
                    action = ModuleProjectManagement.ActionType.LoadTasksPlainAsAdministrator;
                    break;
                case PageListType.DashboardManager:
                    action = ModuleProjectManagement.ActionType.LoadTasksPlainAsManager;
                    break;
                case PageListType.DashboardResource:
                    action = ModuleProjectManagement.ActionType.LoadTasksPlainAsResource;
                    break;
            }
            switch(currentPage){
                case PageListType.ProjectDashboardManager:
                case PageListType.ProjectDashboardResource:
                    View.SendUserAction(idCommunity, CurrentIdModule,idProject, action);
                    break;
                default:
                    View.SendUserAction(View.IdContainerCommunity, CurrentIdModule, action);
                    break;
            }
        }
    }
}