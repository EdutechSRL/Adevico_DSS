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
    public class ProjectDashboardPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewProjectDashboard View
            {
                get { return (IViewProjectDashboard)base.View; }
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
            public ProjectDashboardPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ProjectDashboardPresenter(iApplicationContext oContext, IViewProjectDashboard view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(TabListItem tabItem)
        {
            Int32 idCommunity = View.PreloadIdContainerCommunity;
            dtoProjectContext cContext = InitializeContext();
            long idProject = View.PreloadIdProject;
            View.DashboardContext = cContext;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout((idCommunity > -1) ? idCommunity : cContext.IdCommunity ,View.GetCurrentUrl());
            else
            {
                litePerson p = CurrentManager.GetLitePerson(UserContext.CurrentUserID);

                if (tabItem == TabListItem.Resource || (tabItem != TabListItem.Resource && Service.GetAvailableTabs((p != null ? p.Id : 0), cContext, PageContainerType.ProjectDashboard).Contains(tabItem)))
                {
                    View.InitializeProjectTopControl(cContext, idCommunity, View.PreloadFromCookies, tabItem, PageContainerType.ProjectDashboard, idProject, View.PreloadGroupBy, ItemListStatus.All, View.PreloadFilterStatus, View.PreloadTimeLine);
                    InitializeBackUrl(idProject, idCommunity,cContext, tabItem, View.PreloadFromPage);
                    View.SendUserAction(cContext.IdCommunity, CurrentIdModule, idProject, GetDefaultAction(tabItem));
                }
                else
                    View.RedirectToUrl(View.GetCurrentUrl().Replace(RootObject.GetDashboardPlainPage(idProject, PageListType.ProjectDashboardResource), RootObject.GetDashboardPlainPage(idProject,PageListType.DashboardResource)));
            }
        }

        private void InitializeBackUrl(long idProject,Int32 idCommunity,dtoProjectContext cContext,TabListItem tabItem, PageListType fromPage)
        {
            switch (tabItem)
            {
                case TabListItem.Manager:
                    switch (fromPage)
                    {
                        case PageListType.DashboardResource:
                            View.SetLinkToDashBoardAsResource(RootObject.DashboardFromCookies(cContext,idCommunity, PageListType.DashboardResource,idProject));
                            View.SetLinkToProjectsAsManager(RootObject.ProjectListResource(cContext.IdCommunity, cContext.isForPortal, cContext.isPersonal, false, idProject, ItemsGroupBy.Plain, View.PreloadFilterBy, View.PreloadFilterStatus, View.PreloadTimeLine, View.PreloadDisplay));
                            break;
                        case PageListType.ListResource:
                            View.SetLinkToProjectsAsManager(RootObject.ProjectListResource(idCommunity, cContext.isForPortal, cContext.isPersonal, true, idProject));
                            break;
                        case PageListType.DashboardManager:
                            View.SetLinkToDashBoardAsResource(RootObject.DashboardFromCookies(cContext,idCommunity, PageListType.DashboardManager,idProject));
                            View.SetLinkToProjectsAsResource(RootObject.ProjectListManager(cContext.IdCommunity, cContext.isForPortal, cContext.isPersonal, false, idProject, ItemsGroupBy.Plain, View.PreloadFilterBy, View.PreloadFilterStatus, View.PreloadTimeLine, View.PreloadDisplay));
                            break;
                        case PageListType.ListManager:
                            View.SetLinkToProjectsAsManager(RootObject.ProjectListManager(idCommunity, cContext.isForPortal, cContext.isPersonal, true, idProject));
                            break;
                        case PageListType.DashboardAdministrator:
                            View.SetLinkToDashBoardAsResource(RootObject.DashboardFromCookies(cContext,idCommunity, PageListType.DashboardManager,idProject));
                            View.SetLinkToProjectsAsResource(RootObject.ProjectListManager(cContext.IdCommunity, cContext.isForPortal, cContext.isPersonal, false, idProject, ItemsGroupBy.Plain, View.PreloadFilterBy, View.PreloadFilterStatus, View.PreloadTimeLine, View.PreloadDisplay));
                            break;
                        case PageListType.ListAdministrator:
                            View.SetLinkToProjectsAsResource(RootObject.ProjectListManager(cContext.IdCommunity, cContext.isForPortal, cContext.isPersonal, false, idProject, ItemsGroupBy.Plain, View.PreloadFilterBy, View.PreloadFilterStatus, View.PreloadTimeLine, View.PreloadDisplay));
                            break;
                    }
                    break;
                case TabListItem.Resource:
                    switch (fromPage)
                    {
                        case PageListType.DashboardResource:
                            View.SetLinkToDashBoardAsResource(RootObject.DashboardFromCookies(cContext, idCommunity,  PageListType.DashboardResource, idProject));
                            View.SetLinkToProjectsAsResource(RootObject.ProjectListResource(cContext.IdCommunity, cContext.isForPortal, cContext.isPersonal, false, idProject, ItemsGroupBy.Plain, View.PreloadFilterBy, View.PreloadFilterStatus, View.PreloadTimeLine, View.PreloadDisplay));
                            break;
                        case PageListType.ListResource:
                            View.SetLinkToProjectsAsResource(RootObject.ProjectListResource(idCommunity, cContext.isForPortal, cContext.isPersonal, true, idProject));
                            break;
                        case PageListType.DashboardManager:
                            View.SetLinkToDashBoardAsResource(RootObject.DashboardFromCookies(cContext, idCommunity,  PageListType.DashboardManager, idProject));
                            View.SetLinkToProjectsAsResource(RootObject.ProjectListManager(cContext.IdCommunity, cContext.isForPortal, cContext.isPersonal, false, idProject, ItemsGroupBy.Plain, View.PreloadFilterBy, View.PreloadFilterStatus, View.PreloadTimeLine, View.PreloadDisplay));
                            break;
                        case PageListType.ListManager:
                            View.SetLinkToProjectsAsResource(RootObject.ProjectListManager(cContext.IdCommunity, cContext.isForPortal, cContext.isPersonal, false, idProject, ItemsGroupBy.Plain, View.PreloadFilterBy, View.PreloadFilterStatus, View.PreloadTimeLine, View.PreloadDisplay));
                            break;
                        case PageListType.DashboardAdministrator:
                            View.SetLinkToDashBoardAsResource(RootObject.DashboardFromCookies(cContext, idCommunity, PageListType.DashboardManager, idProject));
                            View.SetLinkToProjectsAsResource(RootObject.ProjectListManager(cContext.IdCommunity, cContext.isForPortal, cContext.isPersonal, false, idProject, ItemsGroupBy.Plain, View.PreloadFilterBy, View.PreloadFilterStatus, View.PreloadTimeLine, View.PreloadDisplay));
                            break;
                        case PageListType.ListAdministrator:
                            View.SetLinkToProjectsAsResource(RootObject.ProjectListResource(cContext.IdCommunity, cContext.isForPortal, cContext.isPersonal, false, idProject, ItemsGroupBy.Plain, View.PreloadFilterBy, View.PreloadFilterStatus, View.PreloadTimeLine, View.PreloadDisplay));
                            break;
                    }
                    break;
            }
        }
        private dtoProjectContext InitializeContext()
        {
            dtoProjectContext cContext = new dtoProjectContext() { IdCommunity = View.PreloadIdCommunity, isPersonal = View.PreloadIsPersonal, isForPortal = View.PreloadForPortal };
            if (cContext.IdCommunity > 0 && cContext.isForPortal)
                cContext.isForPortal = false;
            else if (cContext.IdCommunity == 0 && !cContext.isForPortal)
            {
                cContext.IdCommunity = UserContext.CurrentCommunityID;
                cContext.isForPortal = !(cContext.IdCommunity > 0);
            }
            return cContext;
        }

        private ModuleProjectManagement.ActionType GetDefaultAction(TabListItem tabItem)
        {
            switch (tabItem)
            {
                case TabListItem.Resource:
                    return ModuleProjectManagement.ActionType.LoadProjectDashboardAsResource;
                default:
                    return ModuleProjectManagement.ActionType.LoadProjectDashboardAsManager;
            }
        }
    }
}