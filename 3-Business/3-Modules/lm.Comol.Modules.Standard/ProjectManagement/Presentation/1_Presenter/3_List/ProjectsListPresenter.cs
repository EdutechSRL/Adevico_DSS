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
    public class ProjectsListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
           #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewProjectsList View
            {
                get { return (IViewProjectsList)base.View; }
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
            public ProjectsListPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ProjectsListPresenter(iApplicationContext oContext, IViewProjectsList view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(TabListItem tabItem)
        {
            dtoProjectContext cContext = InitializeContext();
            View.CurrentListContext = cContext;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);

                if (tabItem == TabListItem.Resource || (tabItem != TabListItem.Resource && Service.GetAvailableTabs((p != null ? p.Id : 0), cContext, PageContainerType.ProjectsList).Contains(tabItem)))
                {
                    View.InitializeTopControls(cContext,cContext.IdCommunity, View.PreloadFromCookies, tabItem, PageContainerType.ProjectsList, View.PreloadGroupBy, View.PreloadFilterBy, View.PreloadFilterStatus);
                    ModuleProjectManagement pPermission = ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID);
                    ModuleProjectManagement mPermission = (cContext.isForPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(CurrentManager.GetModulePermission(UserContext.CurrentUserID, cContext.IdCommunity, CurrentIdModule));
                    if (mPermission.Administration || mPermission.CreatePersonalProject || mPermission.CreatePublicProject)
                        View.AllowAddProject((mPermission.CreatePersonalProject) ? RootObject.AddProject(cContext.IdCommunity, cContext.isForPortal, true, PageListType.ListManager, cContext.IdCommunity) : "", (mPermission.CreatePublicProject || mPermission.Administration) ? RootObject.AddProject(cContext.IdCommunity, cContext.isForPortal, false, PageListType.ListManager, cContext.IdCommunity) : "", (cContext.isForPortal ? "" : RootObject.AddProject(0, true, true, PageListType.ListManager, cContext.IdCommunity)));
                    View.SendUserAction(cContext.IdCommunity, CurrentIdModule, 0, GetDefaultAction(tabItem));
                }
                else
                    View.RedirectToUrl(RootObject.ProjectListResource(cContext.IdCommunity, cContext.isForPortal, cContext.isPersonal));

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
        private ModuleProjectManagement.ActionType GetDefaultAction(TabListItem tabItem) {
            switch (tabItem)
            {
                case TabListItem.Resource:
                    return ModuleProjectManagement.ActionType.LoadProjectsAsResource;
                case TabListItem.Manager:
                    return ModuleProjectManagement.ActionType.LoadProjectsAsManager;
                case TabListItem.Administration:
                    return ModuleProjectManagement.ActionType.LoadProjectsAsAdministrator;
                default:
                    return ModuleProjectManagement.ActionType.LoadProjectsGeneric;
            }
        }
    }
}
