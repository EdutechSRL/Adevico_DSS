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
    public class ProjectListPlainPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewProjectListPlain View
            {
                get { return (IViewProjectListPlain)base.View; }
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
            public ProjectListPlainPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ProjectListPlainPresenter(iApplicationContext oContext, IViewProjectListPlain view)
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
                View.IsInitialized = true;
                View.IdCurrentCommunityForList = context.IdCommunity;
                if (filter.PageSize == 0)
                    filter.PageSize = View.CurrentPageSize;
                View.CurrentOrderBy = filter.OrderBy;
                View.CurrentAscending = filter.Ascending;
                LoadProjects(filter, currentPage, context.IdCommunity, filter.PageIndex, filter.PageSize);
            }
        }
        public void LoadProjects(dtoItemsFilter filter, PageListType currentPage,  Int32 idCommunity, Int32 pageIndex, Int32 pageSize)
        {
            if (UserContext.isAnonymous)
            {
                View.DisplayPager(false);
                View.LoadedNoProjects(currentPage);
            }
            else
               LoadProjects(Service.GetProjects(UserContext.CurrentUserID, filter, currentPage, idCommunity, View.RoleTranslations), currentPage,filter.TimeLine, pageIndex, pageSize);
        }
        public void LoadProjects(List<dtoPlainProject> projects, PageListType currentPage, SummaryTimeLine currentTimeline,  Int32 pageIndex, Int32 pageSize)
        {
            Int32 idContainerCommunity = View.IdCurrentCommunityForList;
            PagerBase pager = new PagerBase();
            pager.PageSize = pageSize;//Me.View.CurrentPageSize
            pager.Count = (projects.Count > 0) ? projects.Count - 1 : 0;
            pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
            View.Pager = pager;

            projects = projects.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            if (projects.Any())
            {
                foreach (dtoPlainProject p in projects)
                {
                    p.Urls.Edit = RootObject.EditProject(p.Id, p.IdCommunity, p.isPortal, p.isPersonal, currentPage, idContainerCommunity);
                    p.Urls.EditResources = RootObject.ProjectResources(p.Id, p.IdCommunity, p.isPortal, p.isPersonal, currentPage, idContainerCommunity);
                    p.Urls.PhisicalDelete = RootObject.PhisicalDeleteProject(p.Id, p.IdCommunity, p.isPortal, p.isPersonal, currentPage, idContainerCommunity);
                    switch (currentPage)
                    {
                        case PageListType.ListResource:
                            p.Urls.ProjectUrl = RootObject.ProjectDashboard(p.IdCommunity, p.isPortal, p.isPersonal,idContainerCommunity, p.Id,   currentPage, PageListType.ProjectDashboardResource, currentTimeline, GetDefaultUserActivityStatus(p), currentTimeline);
                            break;
                        case PageListType.ListManager:
                            p.Urls.ProjectUrl = RootObject.ProjectDashboard(p.IdCommunity, p.isPortal, p.isPersonal, idContainerCommunity, p.Id, currentPage, PageListType.DashboardManager, currentTimeline, GetDefaultUserActivityStatus(p), currentTimeline);
                            break;
                        case PageListType.ListAdministrator:
                            p.Urls.ProjectUrl = RootObject.ProjectDashboard(p.IdCommunity, p.isPortal, p.isPersonal, idContainerCommunity, p.Id, currentPage, PageListType.DashboardAdministrator, currentTimeline, GetDefaultUserActivityStatus(p), currentTimeline);
                            break;
                    }
                    if (p.Permissions.EditMap)
                        p.Urls.ProjectMap = RootObject.ProjectMap(p.Id, p.IdCommunity, p.isPortal, p.isPersonal, currentPage, idContainerCommunity);
                    else if (p.Permissions.ViewMap)
                        p.Urls.ProjectMap = RootObject.ViewProjectMap(p.Id, p.IdCommunity, p.isPortal, p.isPersonal, currentPage, idContainerCommunity);
                }
                View.LoadProjects(projects, currentPage);
            }
            else
                View.LoadedNoProjects(currentPage);
            View.SendUserAction(View.IdCurrentCommunityForList, CurrentIdModule,  GetDefaultAction(currentPage));
        }
        private UserActivityStatus GetDefaultUserActivityStatus(dtoPlainProject p)
        {
            if (p.UserCompletion == null || !p.UserCompletion.Values.Where(v => v > 0).Any())
                return UserActivityStatus.Ignore;
            else
            {
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
        public Boolean VirtualDeleteProject(long idProject, dtoItemsFilter filter, PageListType currentPage, Int32 idCommunity, Int32 pageIndex, Int32 pageSize)
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
                LoadProjects(Service.GetProjects(UserContext.CurrentUserID, filter, currentPage, idCommunity, View.RoleTranslations), currentPage, filter.TimeLine, pageIndex, pageSize);
            }
            return result;
        }
        public Boolean VirtualUndeleteProject(long idProject, dtoItemsFilter filter, PageListType currentPage,  Int32 idCommunity, Int32 pageIndex, Int32 pageSize, ref long deleted)
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
                    LoadProjects(Service.GetProjects(UserContext.CurrentUserID, filter, currentPage, idCommunity, View.RoleTranslations), currentPage, filter.TimeLine, pageIndex, pageSize);
            }
            return result;
        }

        public void LoadAttachments(long idProject, PageListType currentPage)
        {
            if (UserContext.isAnonymous)
            {
                View.DisplayPager(false);
                View.LoadedNoProjects(currentPage);
            }
            else
            {
                View.LoadAttachments(Service.GetProjectAttachments(idProject, 0, false, View.UnknownUserTranslation, true));
            }
        }
        private ModuleProjectManagement.ActionType GetDefaultAction(PageListType currentPage)
        {
            switch (currentPage)
            {
                case PageListType.ListResource:
                    return ModuleProjectManagement.ActionType.LoadProjectsGroupAsResource;
                case PageListType.ListManager:
                    return ModuleProjectManagement.ActionType.LoadProjectsGroupAsManager;
                case PageListType.ListAdministrator:
                    return ModuleProjectManagement.ActionType.LoadProjectsGroupAsAdministrator;
                default:
                    return ModuleProjectManagement.ActionType.LoadProjectsGroup;
            }
        }
    }
}