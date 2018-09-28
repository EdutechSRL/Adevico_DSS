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
    public class DashboardPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewDashboard View
            {
                get { return (IViewDashboard)base.View; }
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
            public DashboardPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public DashboardPresenter(iApplicationContext oContext, IViewDashboard view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(TabListItem tabItem)
        {
            Int32 idCommunity = View.PreloadIdContainerCommunity;
            View.IdContainerCommunity = idCommunity;
            dtoProjectContext cContext = InitializeContext();
            View.DashboardContext = cContext;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout((idCommunity > -1) ? idCommunity : cContext.IdCommunity ,View.GetCurrentUrl());
            else
            {
                Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);

                if (tabItem == TabListItem.Resource || (tabItem != TabListItem.Resource && Service.GetAvailableTabs((p != null ? p.Id : 0), cContext, PageContainerType.Dashboard).Contains(tabItem)))
                {
                    View.InitializeTopControls(cContext, cContext.IdCommunity, View.PreloadFromCookies, tabItem, PageContainerType.Dashboard, View.PreloadGroupBy, View.PreloadFilterBy, ItemListStatus.All, View.PreloadFilterStatus);
                    switch (tabItem)
                    {
                        case TabListItem.Manager:
                            switch (View.PreloadFromPage)
                            {
                                case PageListType.ListResource:
                                    View.SetLinkToProjectsAsManager(RootObject.ProjectListResource(cContext.IdCommunity, cContext.isForPortal, cContext.isPersonal, false, 0, ItemsGroupBy.Plain, View.PreloadFilterBy, View.PreloadFilterStatus, View.PreloadTimeLine, View.PreloadDisplay));
                                    break;
                                case PageListType.ListManager:
                                    View.SetLinkToProjectsAsManager(Service.GetBackUrl(View.PreloadFromPage, View.PreloadIdContainerCommunity, 0));
                                    break;
                                case PageListType.ListAdministrator:
                                    View.SetLinkToProjectsAsManager(RootObject.ProjectListManager(cContext.IdCommunity, cContext.isForPortal, cContext.isPersonal, false, 0, ItemsGroupBy.Plain, View.PreloadFilterBy, View.PreloadFilterStatus, View.PreloadTimeLine, View.PreloadDisplay));
                                    break;
                                default:
                                    View.SetLinkToProjectsAsManager(RootObject.ProjectListResource(cContext.IdCommunity, cContext.isForPortal, cContext.isPersonal, false, 0, ItemsGroupBy.Plain, View.PreloadFilterBy, View.PreloadFilterStatus, View.PreloadTimeLine, View.PreloadDisplay));
                                    break;
                            }
                            break;
                        case TabListItem.Resource:
                            switch (View.PreloadFromPage)
                            {
                                case PageListType.ListResource:
                                    View.SetLinkToProjectsAsResource(Service.GetBackUrl(View.PreloadFromPage, View.PreloadIdContainerCommunity, 0));
                                    break;
                                //case PageListType.ListManager:
                                //    View.SetLinkToProjectsAsResource(RootObject.ProjectListResource(cContext.IdCommunity, cContext.isForPortal, cContext.isPersonal, false, 0, ItemsGroupBy.Plain, View.PreloadFilterBy, View.PreloadFilterStatus, View.PreloadTimeLine, View.PreloadDisplay));
                                //    break;
                                //case PageListType.ListAdministrator:
                                //    View.SetLinkToProjectsAsResource(RootObject.ProjectListResource(cContext.IdCommunity, cContext.isForPortal, cContext.isPersonal, false, 0, ItemsGroupBy.Plain, View.PreloadFilterBy, View.PreloadFilterStatus, View.PreloadTimeLine, View.PreloadDisplay));
                                //    break;
                                 default:
                                    View.SetLinkToProjectsAsResource(RootObject.ProjectListResource(cContext.IdCommunity, cContext.isForPortal, cContext.isPersonal, false, 0, ItemsGroupBy.Plain, View.PreloadFilterBy, View.PreloadFilterStatus, View.PreloadTimeLine, View.PreloadDisplay));
                                    break;
                            }
                            break;
                    }
                    View.SendUserAction(cContext.IdCommunity, CurrentIdModule, 0, GetDefaultAction(tabItem));
                }
                else
                    View.RedirectToUrl(View.GetCurrentUrl().Replace(RootObject.GetDashboardPlainPage(PageListType.DashboardManager), RootObject.GetDashboardPlainPage(PageListType.DashboardResource)));
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
                    return ModuleProjectManagement.ActionType.LoadDashboardAsResource;
                case TabListItem.Manager:
                    return ModuleProjectManagement.ActionType.LoadDashboardAsManager;
                case TabListItem.Administration:
                    return ModuleProjectManagement.ActionType.LoadDashboardAsManager;
                default :
                    return ModuleProjectManagement.ActionType.LoadProjectsGeneric;
            }
        }
    }
}