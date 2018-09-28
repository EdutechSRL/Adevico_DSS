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
    public class ProjectResourcesPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewProjectResources  View
            {
                get { return (IViewProjectResources)base.View; }
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
            public ProjectResourcesPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ProjectResourcesPresenter(iApplicationContext oContext, IViewProjectResources view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView() 
        {
            int idCommunity = View.PreloadIdCommunity;
            long idProject = View.PreloadIdProject;
            View.ProjectIdCommunity = idCommunity;
            View.IdProject = idProject;
            View.isPersonal = View.PreloadIsPersonal;
            View.forPortal = View.PreloadForPortal;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Project project = CurrentManager.Get<Project>(idProject);
                if (project == null)
                {
                    View.DisplayUnknownProject();
                    View.SendUserAction(UserContext.CurrentCommunityID, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectUnknown);
                }
                else
                {
                    View.isPersonal = project.isPersonal;
                    View.forPortal = project.isPortal;
                    View.ProjectIdCommunity = (!project.isPortal && project.Community != null) ? project.Community.Id : 0;
                    SetBackUrls(View.PreloadFromPage, View.PreloadIdContainerCommunity, project);
                    Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
                    ModuleProjectManagement permission = (project.isPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, CurrentIdModule));
                    PmActivityPermission taskpermission = Service.GetProjectPermission(idProject, UserContext.CurrentUserID);
                    //// calcolo permessi
                    if ((permission.Administration && !project.isPersonal) || (taskpermission & PmActivityPermission.ManageResources) == PmActivityPermission.ManageResources)
                    {
                        View.AllowAddExternalUser = true; //se il progetto e la sua comunità tornano, allora vado avanti 
                        View.AllowAddCommunityUser = (!project.isPortal && project.Community != null);
                        
                        View.AllowSave = true;
                        View.LoadWizardSteps(idProject, idCommunity, project.isPersonal, project.isPortal, Service.GetAvailableSteps(WizardProjectStep.ProjectUsers, idProject));
                        LoadProjectResources(idProject, true, View.UnknownUser, true, project.isPersonal);
                    }
                    else
                    {
                        ProjectResource resource = Service.GetResource(idProject, UserContext.CurrentUserID);
                        if (resource != null && resource.Visibility == ProjectVisibility.Full)
                            LoadProjectResources(idProject, false, View.UnknownUser, false, project.isPersonal);
                        else
                            View.DisplayNoPermission(idCommunity, currentIdModule);
                    }
                }
            }
        }

        private void SetBackUrls(PageListType fromPage, Int32 idContainerCommunity, Project project)
        {
            switch (fromPage)
            {
                case PageListType.ListAdministrator:
                case PageListType.ListManager:
                case PageListType.ListResource:
                    View.SetProjectsUrl(Service.GetBackUrl(fromPage, idContainerCommunity, (project==null) ? 0 : project.Id));
                    break;
                case PageListType.DashboardManager:
                case PageListType.DashboardResource:
                case PageListType.DashboardAdministrator:
                    if (project != null)
                        View.SetDashboardUrl(RootObject.DashboardFromCookies(new dtoProjectContext() { isPersonal = project.isPersonal, isForPortal = project.isPortal, IdCommunity = (project.Community == null) ? 0 : project.Community.Id }, idContainerCommunity, fromPage, project.Id), fromPage);
                    break;
                case PageListType.ProjectDashboardManager:
                case PageListType.ProjectDashboardResource:
                    if (project != null)
                        View.SetDashboardUrl(RootObject.ProjectDashboardFromCookies(new dtoProjectContext() { isPersonal = project.isPersonal, isForPortal = project.isPortal, IdCommunity = (project.Community == null) ? 0 : project.Community.Id }, idContainerCommunity, fromPage, project.Id), fromPage);
                    break;
            }
            if (project != null)
                View.SetProjectMapUrl(RootObject.ProjectMap(project.Id, (project.Community == null) ? 0 : project.Community.Id, project.isPortal, project.isPersonal, fromPage, idContainerCommunity));
        }
        public void SelectResourcesFromCommunity(long idProject,List<dtoProjectResource> resources,String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else {
                Service.SaveProjectResources(idProject, resources, unknownUser);
                View.InitializeControlToAddUsers(View.ProjectIdCommunity, Service.GetProjectIdPersons(idProject));
            }
        }
        public void SelectExternalResources(long idProject, List<dtoProjectResource> resources, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Service.SaveProjectResources(idProject, resources, unknownUser);
                View.InitializeControlToAddExternalResource();
            }
        }
        public void AddExternalResource(long idProject, List<dtoProjectResource> items, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                if (Service.SaveProjectResources(idProject, items, unknownUser))
                {
                    if (Service.AddExternalResource(idProject, ActivityRole.Resource) != null)
                    {
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ResourceAddExternal);
                        List<dtoProjectResource> resources = Service.GetProjectResources(idProject, unknownUser);
                        View.LoadResources(resources);
                        View.DisplayResourceAdded(1,resources.Where(r => lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)r.DisplayErrors, (long)EditingErrors.MultipleLongName)).Count(), resources.Where(r => lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)r.DisplayErrors, (long)EditingErrors.MultipleShortName)).Count());
                        View.LoadWizardSteps(idProject, View.ProjectIdCommunity, View.isPersonal, View.forPortal, Service.GetAvailableSteps(WizardProjectStep.ProjectUsers, idProject));
                    }
                    else
                        View.DisplayUnableToAddExternalResource();
                }
                else
                    View.DisplayUnableToSaveResources();
            }
        }
        public void AddExternalResources(long idProject, List<dtoProjectResource> items,  List<dtoExternalResource> addingResources,String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                if (Service.SaveProjectResources(idProject, items, unknownUser))
                {
                    if (Service.AddExternalResources(idProject,addingResources, ActivityRole.Resource) != null)
                    {
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, (addingResources.Count > 1) ? ModuleProjectManagement.ActionType.ResourcesAddExternal : ModuleProjectManagement.ActionType.ResourceAddExternal);
                        List<dtoProjectResource> resources = Service.GetProjectResources(idProject, unknownUser);
                        View.LoadResources(resources);
                        View.DisplayResourceAdded(addingResources.Count, resources.Where(r => lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)r.DisplayErrors, (long)EditingErrors.MultipleLongName)).Count(), resources.Where(r => lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)r.DisplayErrors, (long)EditingErrors.MultipleShortName)).Count());
                        View.LoadWizardSteps(idProject, View.ProjectIdCommunity, View.isPersonal, View.forPortal, Service.GetAvailableSteps(WizardProjectStep.ProjectUsers, idProject));
                    }
                    else
                        View.DisplayUnableToAddExternalResource();
                }
                else
                    View.DisplayUnableToSaveResources();
            }
        }
        public void AddInternalResources(long idProject, List<dtoProjectResource> resources, List<Int32> idPersons, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                if (Service.SaveProjectResources(idProject, resources, unknownUser, idPersons))
                {
                    View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, (idPersons.Count > 1) ? ModuleProjectManagement.ActionType.ResourcesAddInternal : ModuleProjectManagement.ActionType.ResourceAddInternal);
                    List<dtoProjectResource> cResources = Service.GetProjectResources(idProject, unknownUser);
                    View.LoadResources(cResources);
                    View.DisplayResourceAdded(idPersons.Count,cResources.Where(r => lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)r.DisplayErrors, (long)EditingErrors.MultipleLongName)).Count(), cResources.Where(r => lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)r.DisplayErrors, (long)EditingErrors.MultipleShortName)).Count());
                    View.LoadWizardSteps(idProject, View.ProjectIdCommunity, View.isPersonal, View.forPortal, Service.GetAvailableSteps(WizardProjectStep.ProjectUsers, idProject));
                }
                else
                {
                    switch (idPersons.Count)
                    {
                        case 0:
                            View.DisplayUnableToSaveResources();
                            break;
                        case 1:
                            View.DisplayUnableToAddInternalResource();
                            break;
                        default:
                            View.DisplayUnableToAddInternalResources();
                            break;
                    }
                }
            }
        }
        public void SaveSettings(long idProject, List<dtoProjectResource> items, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                if (Service.SaveProjectResources(idProject, items, unknownUser))
                {
                    View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ResourcesSaveSettings);
                    List<dtoProjectResource> resources = Service.GetProjectResources(idProject, unknownUser);
                    View.LoadResources(resources);
                    View.DisplaySavedSettings( resources.Where(r => lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)r.DisplayErrors, (long)EditingErrors.MultipleLongName)).Count(), resources.Where(r => lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)r.DisplayErrors, (long)EditingErrors.MultipleShortName)).Count());
                }
                else
                    View.DisplayUnableToSaveResources();
            }
        }

        public void VirtualDeleteResource(long idResource, String name)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                long assignedTask = 0;
                long completedTasks = 0;
                if (Service.SetResourceVirtualDelete(idResource,  true, ref assignedTask, ref completedTasks))
                {
                    View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, View.IdProject, ModuleProjectManagement.ActionType.ResourceVirtualDelete);
                    View.DisplayRemovedResource(name);
                    View.LoadWizardSteps(View.IdProject, View.ProjectIdCommunity, View.isPersonal, View.forPortal, Service.GetAvailableSteps(WizardProjectStep.ProjectUsers, View.IdProject));
                }
                else
                    View.DisplayUnableToRemoveResource(View.IdProject,idResource, name, assignedTask, completedTasks);
            }
        }
        public void ConstrainVirtualDeleteResource(long idResource,  String name, RemoveAction action) {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                if (Service.SetResourceVirtualDelete(idResource, true, action))
                {
                    switch (action)
                    {
                        case RemoveAction.FromNotCompletedAssignments:
                            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, View.IdProject, ModuleProjectManagement.ActionType.ResourceRemoveFromNotCompletedAssignments);
                            break;
                        case RemoveAction.FromNotStartedAssignments:
                            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, View.IdProject, ModuleProjectManagement.ActionType.ResourceRemoveFromNotStartedAssignments);
                            break;
                        case RemoveAction.FromAllAndRecalculateCompletion:
                            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, View.IdProject, ModuleProjectManagement.ActionType.ResourceRemoveFromAllAndRecalculateCompletion);
                            break;
                        case RemoveAction.FromNotCompletedAssignmentsAndRecalculateCompletion:
                            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, View.IdProject, ModuleProjectManagement.ActionType.ResourceRemoveFromNotCompletedAssignmentsAndRecalculateCompletion);
                            break;
                        case RemoveAction.FromAllAssignments:
                            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, View.IdProject, ModuleProjectManagement.ActionType.ResourceRemoveFromAllAssignments);
                            break;
                    }
                    View.DisplayRemovedResource(name);
                    View.LoadWizardSteps(View.IdProject, View.ProjectIdCommunity, View.isPersonal, View.forPortal, Service.GetAvailableSteps(WizardProjectStep.ProjectUsers, View.IdProject));
                }
                else
                    View.DisplayUnableToRemoveResource(name);
            }
        }

        private void LoadProjectResources(long idProject,Boolean allowEdit, String unknownUser, Boolean displayMessage, Boolean isPersonalProject)
        {
            List<dtoProjectResource> resources = Service.GetProjectResources(idProject, unknownUser);
            if (!allowEdit)
                resources.ForEach(r => r.AllowEdit = false);
            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, (allowEdit) ? ModuleProjectManagement.ActionType.ResourcesManage : ModuleProjectManagement.ActionType.ResourcesView);
            View.LoadResources(resources);
            if (displayMessage)
            {
                if (resources.Where(r => r.DisplayErrors != EditingErrors.None).Any())
                    View.DisplayErrors(resources.Where(r => lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)r.DisplayErrors, (long)EditingErrors.MultipleLongName)).Count(), resources.Where(r => lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)r.DisplayErrors, (long)EditingErrors.MultipleShortName)).Count());
                //else if (!resources.Where(r => r.ProjectRole != ActivityRole.ProjectOwner).Any() && !isPersonalProject)
                //    View.DisplayNoResources(); 
            }
        }
    }
}
