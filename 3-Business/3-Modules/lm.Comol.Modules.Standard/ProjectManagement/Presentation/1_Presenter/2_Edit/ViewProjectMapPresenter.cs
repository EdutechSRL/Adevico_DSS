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
    public class ViewProjectMapPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
           #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewViewProjectMap View
            {
                get { return (IViewViewProjectMap)base.View; }
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
            public ViewProjectMapPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ViewProjectMapPresenter(iApplicationContext oContext, IViewViewProjectMap view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

            public void InitView()
            {
                long idProject = View.PreloadIdProject;
                dtoProjectContext cContext = new dtoProjectContext() { IdCommunity = View.PreloadIdCommunity, isPersonal = View.PreloadIsPersonal, isForPortal = View.PreloadForPortal };
                dtoProject project = InitializeContext(idProject, ref cContext);
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    if (project == null)
                    {
                        View.DisplayUnknownProject();
                        View.SendUserAction(UserContext.CurrentCommunityID, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectUnknown);
                    }
                    else
                    {
                        Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
                        ModuleProjectManagement mPermission = (project.isPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(CurrentManager.GetModulePermission(UserContext.CurrentUserID, cContext.IdCommunity, CurrentIdModule));
                        PmActivityPermission pPermissions = Service.GetProjectPermission(idProject, UserContext.CurrentUserID);
                        SetBackUrls(View.PreloadFromPage, View.PreloadIdContainerCommunity, project, cContext);
                        if ((mPermission.Administration && !project.isPersonal) || (pPermissions.HasFlag(PmActivityPermission.ViewProjectMap)))
                        {
                            PageListType fromPage = View.PreloadFromPage;
                            View.LoadProjectDateInfo(project, (fromPage != PageListType.ProjectDashboardResource && fromPage != PageListType.DashboardResource && fromPage != PageListType.ListResource) && ((mPermission.Administration && !project.isPersonal) || (pPermissions.HasFlag(PmActivityPermission.ManageProject))));

                            List<dtoMapActivity> activities = Service.GetActivities(mPermission, pPermissions, project);
                            if (activities == null)
                            {
                                View.DisplayNoActivities();
                                View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectMapErrorFromDb);
                            }
                            else
                            {
                                View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectMapView);
                                View.LoadActivities(activities);
                            }
                            if ((mPermission.Administration && !project.isPersonal) || (pPermissions.HasFlag(PmActivityPermission.ManageProject)))
                            {
                                View.SetEditProjectUrl(RootObject.EditProject(project.Id, project.IdCommunity, project.isPortal, project.isPersonal, View.PreloadFromPage, View.IdContainerCommunity));
                                View.SetEditMapUrl(RootObject.ProjectMap(project.Id, project.IdCommunity, project.isPortal, project.isPersonal, View.PreloadFromPage, View.IdContainerCommunity));
                            }
                            View.LoadAttachments(Service.GetProjectAttachments(idProject, 0, false, View.UnknownUser, true));
                        }
                        else
                            View.DisplayNoPermission(project.IdCommunity, currentIdModule);
                    }
                }
            }

            private void SetBackUrls(PageListType fromPage, Int32 idContainerCommunity, dtoProject project, dtoProjectContext cContext)
            {
                switch (fromPage)
                {
                    case PageListType.ListAdministrator:
                    case PageListType.ListManager:
                    case PageListType.ListResource:
                        View.SetProjectsUrl(Service.GetBackUrl(fromPage, idContainerCommunity, project.Id));
                        break;
                    case PageListType.DashboardManager:
                    case PageListType.DashboardResource:
                    case PageListType.DashboardAdministrator:
                        View.SetDashboardUrl(RootObject.DashboardFromCookies(cContext, idContainerCommunity, fromPage, project.Id), fromPage);
                        break;
                    case PageListType.ProjectDashboardManager:
                    case PageListType.ProjectDashboardResource:
                        View.SetDashboardUrl(RootObject.ProjectDashboardFromCookies(cContext, idContainerCommunity, fromPage, project.Id), fromPage);
                        break;
                }
            }
            private dtoProject InitializeContext(long idProject, ref dtoProjectContext cContex)
            {
                dtoProject project = Service.GetdtoProject(idProject);
                View.IdProject = idProject;
                if (project == null)
                {
                    Int32 idCommunity = (!cContex.isForPortal && cContex.IdCommunity < 1) ? UserContext.CurrentCommunityID : cContex.IdCommunity;
                    Community community = (idCommunity > 0) ? CurrentManager.GetCommunity(idCommunity) : null;
                    cContex.IdCommunity = (community != null) ? community.Id : 0;
                }
                else
                {
                    cContex.IdCommunity = project.IdCommunity;
                    View.forPortal = project.isPortal;
                    View.isPersonal = project.isPersonal;
                }

                View.ProjectIdCommunity = cContex.IdCommunity;
                View.forPortal = cContex.isForPortal;
                View.isPersonal = cContex.isPersonal;
                return project;
            }
    }
}