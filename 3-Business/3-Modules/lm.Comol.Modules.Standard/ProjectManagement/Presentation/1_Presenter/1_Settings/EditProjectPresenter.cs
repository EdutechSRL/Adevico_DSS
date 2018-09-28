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
    public class EditProjectPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewEditProject View
            {
                get { return (IViewEditProject)base.View; }
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
            public EditProjectPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EditProjectPresenter(iApplicationContext oContext, IViewEditProject view)
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
                
                List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoProjectManagementStep>> steps = Service.GetAvailableSteps(WizardProjectStep.Settings, idProject);
                View.LoadWizardSteps(idProject, cContext.IdCommunity, cContext.isPersonal, cContext.isForPortal, steps);
                if (project == null)
                {
                    View.DisplayUnknownProject();
                    View.SendUserAction(UserContext.CurrentCommunityID, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectUnknown);
                }
                else {
                    ModuleProjectManagement mPermission = (project.isPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(CurrentManager.GetModulePermission(UserContext.CurrentUserID, cContext.IdCommunity, CurrentIdModule));
                    PmActivityPermission pPermissions = Service.GetProjectPermission(idProject, UserContext.CurrentUserID);

                    List<ProjectAvailability> availabilities = new List<ProjectAvailability>();
                    ProjectAvailability cAvailability =project.Availability;
                    Boolean hasActivities = Service.HasTasks(project.Id);
                    if (project.Availability == ProjectAvailability.Draft)
                        availabilities.Add(ProjectAvailability.Draft);
                    if (hasActivities)
                    {
                        availabilities.Add(ProjectAvailability.Active);
                        availabilities.Add(ProjectAvailability.Closed);
                        availabilities.Add(ProjectAvailability.Suspended);
                    }

                    Boolean allowChangeOwner = false;
                    Boolean allowChangeOwnerFromResource = false;
                    Boolean allowChangeOwnerFromCommunity = false;
                    if ((mPermission.Administration && !project.isPersonal) || (pPermissions & PmActivityPermission.ManageProject) == PmActivityPermission.ManageProject)
                    {
                        View.AllowSave = true;
                        View.SendUserAction(cContext.IdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectStartEditing);
                        if (View.PreloadAddedProject)
                            View.DisplayAddedInfo(View.PreloadRequiredActions, View.PreloadAddedActions);

                        allowChangeOwner = (mPermission.Administration && !project.isPersonal) || ((pPermissions & PmActivityPermission.ChangeOwner) == PmActivityPermission.ChangeOwner);
                        allowChangeOwnerFromResource = (project.Resources.Count>1);
                        allowChangeOwnerFromCommunity = (project.IdCommunity >0);
                        View.InitializeProject(project, hasActivities, project.Resources.Where(r => r.ProjectRole == ActivityRole.ProjectOwner).FirstOrDefault(), availabilities, cAvailability,allowChangeOwner,allowChangeOwnerFromResource, allowChangeOwnerFromCommunity);
                    }
                    else{
                        ProjectResource resource = Service.GetResource(idProject, UserContext.CurrentUserID);
                        if (resource != null && resource.Visibility == ProjectVisibility.Full){
                            View.InitializeProject(project, hasActivities, project.Resources.Where(r => r.ProjectRole == ActivityRole.ProjectOwner).FirstOrDefault(), availabilities, cAvailability,allowChangeOwner,allowChangeOwnerFromResource, allowChangeOwnerFromCommunity);
                            View.SendUserAction(cContext.IdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectViewSettings);
                        }
                        else
                            View.DisplayNoPermission(cContext.IdCommunity, CurrentIdModule);
                    }
                }
            }
        }
        private void SetBackUrls(PageListType fromPage, Int32 idContainerCommunity, dtoProject project,dtoProjectContext cContext) {
            switch (fromPage) { 
                case PageListType.ListAdministrator:
                case PageListType.ListManager:
                case PageListType.ListResource:
                    View.SetProjectsUrl(Service.GetBackUrl(fromPage, idContainerCommunity, (project==null) ? 0 : project.Id));
                    break;
                case PageListType.DashboardManager:
                case PageListType.DashboardResource:
                case PageListType.DashboardAdministrator:
                    if (project != null)
                        View.SetDashboardUrl(RootObject.DashboardFromCookies(cContext, idContainerCommunity, fromPage, project.Id), fromPage);
                    break;
                case PageListType.ProjectDashboardManager:
                case PageListType.ProjectDashboardResource:
                    if (project != null)
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
        public void SaveSettings(long idProject, dtoProject dto, dtoProjectSettingsSelectedActions actions = null)
        { 
           if (UserContext.isAnonymous) 
                View.DisplaySessionTimeout();
            else
            {
                if (dto == null){
                    View.DisplayUnknownProject();
                    View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectTryToSave);
                }
                else if (!dto.StartDate.HasValue || dto.DaysOfWeek == FlagDayOfWeek.None)
                {
                    View.DisplayProjectSavingError(dto.StartDate, dto.DaysOfWeek);
                    View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectTryToSave);
                }
                else {
                    dtoProjectStatistics cStatistics = Service.GetProjectStatistics(idProject);
                    if (isValid(dto, cStatistics) || actions!=null)
                    {
                        Project project = Service.SaveProject(dto, View.GetDefaultCalendarName(), actions);
                        if (project == null)
                        {
                            View.DisplayProjectSavingError();
                            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectTryToSave);
                        }
                        else
                        {
                            if (actions!=null)
                                View.UpdateSettings(actions, project.StartDate, project.EndDate);
                            else
                                View.UpdateSettings(project.StartDate, project.EndDate);
                            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectSaveSettings);
                            View.LoadWizardSteps(idProject, (project.Community == null) ? 0 : project.Community.Id, project.isPersonal, project.isPortal, Service.GetAvailableSteps(WizardProjectStep.Settings, idProject));
                            View.DisplayProjectSaved();
                        }
                    }
                    else
                        View.DisplayConfirmActions(dto,cStatistics);
                }
            }
        }

        private Boolean isValid(dtoProject dto,dtoProjectStatistics cStatistics)
        {
            Boolean result = (cStatistics.Summaries == 0 || dto.AllowSummary) && (cStatistics.Milestones == 0 || dto.AllowMilestones) && (cStatistics.EstimatedActivities == 0 || dto.AllowEstimatedDuration)
                && (dto.DateCalculationByCpm == cStatistics.DateCalculationByCpm) && (dto.StartDate == cStatistics.StartDate);

            return result;
        }
        public void RequireNewOwnerFromCommunity(long idProject)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else 
                View.InitializeControlToSelectOwner(View.ProjectIdCommunity, Service.GetProjectIdPersons(idProject));
        }
        public void RequireNewOwnerFromResources(long idProject,String unknownUser){
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
                View.InitializeControlToSelectOwnerFromProject( Service.GetAvailableResourcesForOwner(idProject, unknownUser));
        }
        public void SelectNewOwner(long idProject, long idResource, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else {
                ProjectResource resource = Service.SelectNewProjectOwner(idProject, idResource);
                if (resource == null)
                    View.DisplayUnableToChangeOwner();
                else
                {
                    View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectSelectOwnerFromResources);
                    View.DisplayOwnerChanged(resource.GetLongName());
                    View.UpdateDefaultResourceSelector(Service.GetAvailableResources(idProject, unknownUser));
                }
            }
        }
        public void AddNewOwner(long idProject, Int32 idPerson, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ProjectResource resource = Service.AddNewProjectOwner(idProject, idPerson);
                if (resource == null)
                    View.DisplayUnableToChangeOwner();
                else
                {
                    View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectSelectOwnerFromCommunity);
                    View.DisplayOwnerChanged(resource.GetLongName());
                    View.UpdateDefaultResourceSelector(Service.GetAvailableResources(idProject, unknownUser));
                }
            } 
        }

        
        private String GetPersonShortName(Person p)
        {
            String name = p.Name.Trim();
            return ((name.Length > 0) ? name[0].ToString() : p.Id.ToString()) + p.FirstLetter;
        }
    }
}