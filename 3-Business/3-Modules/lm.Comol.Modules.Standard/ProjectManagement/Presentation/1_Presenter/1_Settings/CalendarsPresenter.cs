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
    public class CalendarsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewCalendars View
            {
                get { return (IViewCalendars)base.View; }
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
            public CalendarsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public CalendarsPresenter(iApplicationContext oContext, IViewCalendars view)
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
                Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
                SetBackUrls(View.PreloadFromPage, View.PreloadIdContainerCommunity, project, cContext);
                List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoProjectManagementStep>> steps = Service.GetAvailableSteps(WizardProjectStep.Calendars, idProject);
                View.LoadWizardSteps(idProject, cContext.IdCommunity, cContext.isPersonal, cContext.isForPortal, steps);
                if (project == null)
                    View.DisplayUnknownProject();
                else {
                    ModuleProjectManagement mPermission = (project.isPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(CurrentManager.GetModulePermission(UserContext.CurrentUserID, cContext.IdCommunity, CurrentIdModule));
                    PmActivityPermission pPermissions = Service.GetProjectPermission(idProject, UserContext.CurrentUserID);

                    View.LoadProjectAvailableDays(project.DaysOfWeek);
                    if ((mPermission.Administration && !project.isPersonal) || (pPermissions & PmActivityPermission.ManageProject) == PmActivityPermission.ManageProject)
                    {
                        View.AllowSave = false;
                        View.AllowAdd = false;
                        View.SendUserAction(cContext.IdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.CalendarsLoad);
                    }
                    else{
                        ProjectResource resource = Service.GetResource(idProject, UserContext.CurrentUserID);
                        if (resource != null && resource.Visibility == ProjectVisibility.Full){
                            View.SendUserAction(cContext.IdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.CalendarsLoad);
                        }
                        else
                            View.DisplayNoPermission(cContext.IdCommunity, CurrentIdModule);
                    }
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
                    View.SetProjectsUrl(Service.GetBackUrl(fromPage, idContainerCommunity,(project==null) ? 0: project.Id));
                    break;
                case PageListType.DashboardManager:
                case PageListType.DashboardResource:
                case PageListType.DashboardAdministrator:
                    if (project != null)
                        View.SetDashboardUrl(RootObject.DashboardFromCookies(cContext, idContainerCommunity, fromPage, project.Id), fromPage);
                    break;
                case PageListType.ProjectDashboardManager:
                case PageListType.ProjectDashboardResource:
                    if (project!=null)
                        View.SetDashboardUrl(RootObject.ProjectDashboardFromCookies(cContext, idContainerCommunity, fromPage, project.Id), fromPage);
                    break;
            }
            if (project != null)
                View.SetProjectMapUrl(RootObject.ProjectMap(project.Id, project.IdCommunity, project.isPortal, project.isPersonal, fromPage, idContainerCommunity));
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