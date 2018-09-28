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
    public class ProjectMapPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
           #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewProjectMap View
            {
                get { return (IViewProjectMap)base.View; }
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
            public ProjectMapPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ProjectMapPresenter(iApplicationContext oContext, IViewProjectMap view)
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
                    if ((mPermission.Administration && !project.isPersonal) || (pPermissions.HasFlag(PmActivityPermission.ManageProject)))
                    {
                        View.LoadAttachments(Service.GetProjectAttachments(idProject,0,false,View.UnknownUser,true));
                        View.AllowAddExternalUser = pPermissions.HasFlag(PmActivityPermission.ManageProject) || pPermissions.HasFlag(PmActivityPermission.ManageResources);
                        View.AllowAddCommunityUser = (!project.isPortal && project.IdCommunity>0) && (pPermissions.HasFlag(PmActivityPermission.ManageProject) || pPermissions.HasFlag(PmActivityPermission.ManageResources));
                        View.AllowSave = true;
                        View.AllowAddActivity = true;
                        View.AllowManageResources = (mPermission.Administration && !project.isPersonal) || (pPermissions.HasFlag(PmActivityPermission.ManageResources) || (pPermissions.HasFlag(PmActivityPermission.ManageProject)));
                        View.LoadProjectDateInfo(project, true);

                        List<dtoMapActivity> activities = Service.GetActivities(mPermission,pPermissions, project);
                        if (activities == null)
                        {
                            View.DisplayErrorGetActivitiesFromDB();
                            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectMapErrorFromDb);
                        }
                        else
                        {
                            View.InitializeControlForResourcesSelection(Service.GetProjectResources(idProject,View.UnknownUser));
                            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectMapForEditing);
                            View.LoadActivities(activities);
                        }
                        View.SetEditProjectUrl(RootObject.EditProject(project.Id, project.IdCommunity, project.isPortal, project.isPersonal, View.PreloadFromPage, View.IdContainerCommunity));
                    }
                    else
                    {
                        ProjectResource resource = Service.GetResource(idProject, UserContext.CurrentUserID);
                        if (pPermissions.HasFlag(PmActivityPermission.ViewProjectMap) && (resource != null && resource.Visibility == ProjectVisibility.Full))
                            View.RedirectToUrl(RootObject.ViewProjectMap(idProject, project.IdCommunity, project.isPortal, project.isPersonal, View.PreloadFromPage, View.PreloadIdContainerCommunity));
                        else
                            View.DisplayNoPermission(project.IdCommunity, currentIdModule);
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
        #region "Manage Resources"
            public void SelectResourcesFromCommunity(long idProject, String unknownUser)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                    View.InitializeControlToAddUsers(View.ProjectIdCommunity, Service.GetProjectIdPersons(idProject));
            }
            public void SelectExternalResources(long idProject, String unknownUser)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                    View.InitializeControlToAddExternalResource();
            }
            public void AddExternalResource(long idProject, String unknownUser)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    if (Service.AddExternalResource(idProject, ActivityRole.Resource) != null)
                    {
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ResourceAddExternal);
                        List<dtoProjectResource> cResources = Service.GetProjectResources(idProject, unknownUser);
                        View.InitializeControlForResourcesSelection(cResources);
                        View.DisplayResourceAdded(1, cResources.Where(r => lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)r.DisplayErrors, (long)EditingErrors.MultipleLongName)).Count(), cResources.Where(r => lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)r.DisplayErrors, (long)EditingErrors.MultipleShortName)).Count());
                    }
                    else
                        View.DisplayUnableToAddExternalResource();
                }
            }
            public void AddExternalResources(long idProject, List<dtoExternalResource> addingResources, String unknownUser)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    List<ProjectResource> aResources = Service.AddExternalResources(idProject, addingResources, ActivityRole.Resource);
                    if (aResources != null)
                    {
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, (addingResources.Count > 1) ? ModuleProjectManagement.ActionType.ResourcesAddExternal : ModuleProjectManagement.ActionType.ResourceAddExternal);
                        List<dtoProjectResource> cResources = Service.GetProjectResources(idProject, unknownUser);
                        View.InitializeControlForResourcesSelection(cResources);
                        View.DisplayResourceAdded(aResources.Count, cResources.Where(r => lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)r.DisplayErrors, (long)EditingErrors.MultipleLongName)).Count(), cResources.Where(r => lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)r.DisplayErrors, (long)EditingErrors.MultipleShortName)).Count());
                    }
                    else
                        View.DisplayUnableToAddExternalResource();
                }
            }
            public void AddInternalResources(long idProject, List<Int32> idPersons, String unknownUser)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    List<ProjectResource> aResources = Service.AddProjectInternalResources(idProject, idPersons, ActivityRole.Resource);
                    if (aResources!=null)
                    {
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, (idPersons.Count > 1) ? ModuleProjectManagement.ActionType.ResourcesAddInternal : ModuleProjectManagement.ActionType.ResourceAddInternal);
                        List<dtoProjectResource> cResources = Service.GetProjectResources(idProject, unknownUser);
                        View.InitializeControlForResourcesSelection(cResources);
                        View.DisplayResourceAdded(aResources.Count, cResources.Where(r => lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)r.DisplayErrors, (long)EditingErrors.MultipleLongName)).Count(), cResources.Where(r => lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)r.DisplayErrors, (long)EditingErrors.MultipleShortName)).Count());
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
            public void SaveProjectResources(long idProject, List<dtoProjectResource> items, String unknownUser)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    if (Service.SaveProjectResources(idProject, items, unknownUser))
                    {
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ResourcesSaveSettings);
                        List<dtoProjectResource> resources = Service.GetProjectResources(idProject, unknownUser);
                        View.ReloadResources(Service.GetResourcesForActivities(idProject, resources.Select(r=>new dtoResource(r)).ToList()));
                        View.InitializeControlForResourcesSelection(resources);

                        View.DisplaySavedSettings(resources.Where(r => lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)r.DisplayErrors, (long)EditingErrors.MultipleLongName)).Count(), resources.Where(r => lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)r.DisplayErrors, (long)EditingErrors.MultipleShortName)).Count());
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
                    long idProject = View.IdProject;
                    long assignedTask = 0;
                    long completedTasks = 0;
                    if (Service.SetResourceVirtualDelete(idResource, true, ref assignedTask, ref completedTasks))
                    {
                        List<dtoProjectResource> resources = Service.GetProjectResources(idProject, View.UnknownUser);
                        View.ReloadResources(Service.GetResourcesForActivities(idProject, resources.Select(r => new dtoResource(r)).ToList()));
                        View.InitializeControlForResourcesSelection(resources);
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ResourceVirtualDelete);
                        View.DisplayRemovedResource(name);
                    }
                    else
                        View.DisplayUnableToRemoveResource(idProject, idResource, name, assignedTask, completedTasks);
                }
            }
            public void ConstrainVirtualDeleteResource(long idResource, String name, RemoveAction action)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    List<long> idActivities = Service.GetIdActivitiesByResource(idResource);
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
                        View.InitializeControlForResourcesSelection(Service.GetProjectResources(View.IdProject, View.UnknownUser));
                        View.DisplayRemovedResource(name);
                    }
                    else
                        View.DisplayUnableToRemoveResource(name);
                }
            }
            public void LoadProjectResources(long idProject, String unknownUser)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
                    ModuleProjectManagement mPermission = (View.forPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(CurrentManager.GetModulePermission(UserContext.CurrentUserID, View.ProjectIdCommunity, CurrentIdModule));
                    Boolean allowEditResources = (!View.isPersonal && mPermission.Administration);
                    if (!allowEditResources){
                        PmActivityPermission pPermissions = Service.GetProjectPermission(idProject, UserContext.CurrentUserID);
                        allowEditResources = pPermissions.HasFlag( PmActivityPermission.ManageResources) || pPermissions.HasFlag( PmActivityPermission.ManageProject);
                    }
                    LoadProjectResources(idProject, allowEditResources, unknownUser, true);
                }
            }
            private void LoadProjectResources(long idProject, Boolean allowEdit, String unknownUser, Boolean displayMessage)
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
                    else if (!resources.Where(r => r.ProjectRole != ActivityRole.ProjectOwner).Any())
                        View.DisplayNoResources();
                }
            }
        #endregion

        #region "Manage Activities"
            public void SavedActivity(long idProject,long idActivity, List<dtoLiteMapActivity> activities,dtoField<DateTime?> startDate, dtoField<DateTime?> endDate,dtoField<DateTime?> deadLine )
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    foreach(dtoLiteMapActivity vActivity in activities.Where(a=> !String.IsNullOrEmpty(a.Current.Predecessors ) && !String.IsNullOrEmpty(a.Previous.Predecessors))){
                        vActivity.UpdateIdPredecessors(activities);
                    }
                    List<dtoMapActivity> dbActivities = Service.GetActivities(idProject, View.ProjectIdCommunity, CurrentIdModule, View.UnknownUser, activities);
                    if (dbActivities == null)
                    {
                        View.DisplayErrorGetActivitiesFromDB();
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectMapErrorFromDb);
                    }
                    else
                    {
                        View.DisplayActivitySaved(dbActivities.Where(a => a.IdActivity == idActivity).FirstOrDefault(), startDate, endDate,deadLine);
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectMapForEditing);
                        View.LoadActivities(dbActivities);
                    }
                }
            }
            public void SaveActivities(long idProject, List<dtoLiteMapActivity> activities)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else {
                    long updated = 0;
                    long alreadyModified = 0;
                    dtoField<DateTime?> startDate = new dtoField<DateTime?>();
                    dtoField<DateTime?> endDate = new dtoField<DateTime?>();
                    dtoField<DateTime?> deadLine = new dtoField<DateTime?>();
                    long toUpdate = activities.Where(a => a.InEditMode).Count();
                    List<dtoMapActivity> dbActivities = Service.SaveActivities(idProject, activities,View.InEditStartDate , View.InEditDeadline , ref startDate, ref endDate, ref deadLine, ref updated, ref alreadyModified, View.UnknownUser);
                    if (updated > 0 && updated == activities.Where(a => a.InEditMode).Count() || toUpdate == 0)
                    {
                        if (dbActivities == null)
                            View.DisplaySavedActivities();
                        else
                            View.DisplaySavedActivities(dbActivities.Where(a => a.InEditPredecessorsMode && a.Status == FieldStatus.error).Any(), dbActivities.Where(a => a.InEditPredecessorsMode && a.Status == FieldStatus.errorfatherlinked).Any(), dbActivities.Where(a => a.InEditPredecessorsMode && a.Status == FieldStatus.errorsummarylinked).Any());
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectMapSavedActivities);
                    }
                    else if (!activities.Where(a => a.InEditMode).Any())
                    {
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectMapErrorSavingActivities);
                        View.DisplaySavingActivitiesErrors(activities.Where(a => a.InEditMode).Count(), updated, alreadyModified);
                    }
                    if (dbActivities!=null)
                        View.LoadActivities(dbActivities,startDate,endDate,deadLine);
                }
            }
            public void EditActivity(long idProject, long idActivity, List<dtoLiteMapActivity> activities)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    Boolean saved = false;
                    long updated = 0;
                    long alreadyModified = 0;
                    dtoField<DateTime?> startDate = new dtoField<DateTime?>();
                    dtoField<DateTime?> endDate = new dtoField<DateTime?>();
                    dtoField<DateTime?> deadLine = new dtoField<DateTime?>();
                    long toUpdate = activities.Where(a => a.InEditMode).Count();
                    if (toUpdate > 0)
                    {
                        List<dtoMapActivity> dbActivities = Service.SaveActivities(idProject, activities, View.InEditStartDate, View.InEditDeadline, ref startDate, ref endDate, ref deadLine, ref updated, ref alreadyModified, View.UnknownUser);
                        if (updated > 0 && updated == activities.Where(a => a.InEditMode).Count())
                        {
                            saved = true;
                            if (dbActivities == null)
                                View.DisplaySavedActivities();
                            else
                                View.DisplaySavedActivities(dbActivities.Where(a => a.InEditPredecessorsMode && a.Status == FieldStatus.error).Any(), dbActivities.Where(a => a.InEditPredecessorsMode && a.Status == FieldStatus.errorfatherlinked).Any(), dbActivities.Where(a => a.InEditPredecessorsMode && a.Status == FieldStatus.errorsummarylinked).Any());
                            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectMapSavedActivities);
                            if (dbActivities != null)
                                View.LoadActivities(dbActivities, startDate, endDate, deadLine);
                        }
                        else if (!activities.Where(a => a.InEditMode).Any())
                        {
                            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectMapErrorSavingActivities);
                            View.DisplaySavingActivitiesErrors(activities.Where(a => a.InEditMode).Count(), updated, alreadyModified);
                        }
                    }
                    else
                        saved = true;
                    if (saved)
                    {
                        if (Service.GetActivity(idActivity) != null)
                            View.InitializeActivityControl(idActivity);
                        else
                            View.DisplayRemovedActivity();
                    }
                }
            }
            public void ToFaher(long idProject, long idActivity,  List<dtoLiteMapActivity> activities){
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else {
                    PmActivity activity = Service.GetActivity(idActivity);
                    if (activity == null)
                    {
                        List<dtoMapActivity> dbActivities = Service.GetActivities(idProject, View.ProjectIdCommunity, CurrentIdModule, View.UnknownUser, activities);
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ActivityToFatherErrors);
                        if (dbActivities != null)
                            View.LoadActivities(dbActivities);
                        else
                            View.DisplayErrorGetActivitiesFromDB();
                    }
                    else { 
                        long idParent = (activity.Parent !=null) ? ((activity.Parent.Parent==null) ? 0 : activity.Parent.Parent.Id ) : 0;
                        MoveActivityTo(idProject, idActivity, idParent,  activities, ModuleProjectManagement.ActionType.ActivityToFather);
                    }
                }
            }
            public void MoveActivityTo(long idProject, long idActivity, long idParent,  List<dtoLiteMapActivity> activities, ModuleProjectManagement.ActionType action = ModuleProjectManagement.ActionType.None )
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    dtoField<DateTime?> startDate = new dtoField<DateTime?>();
                    dtoField<DateTime?> endDate = new dtoField<DateTime?>();
                    dtoField<DateTime?> deadLine = new dtoField<DateTime?>();
                    if (action == ModuleProjectManagement.ActionType.None)
                        action = ModuleProjectManagement.ActionType.ActivityToChild;
                    Boolean moved = false;
                    List<dtoMapActivity> dbActivities = Service.MoveActivityTo(idProject, idActivity, idParent, activities, View.InEditStartDate, View.InEditDeadline, ref startDate, ref endDate, ref deadLine, View.UnknownUser, ref moved);
                    if (moved)
                    {
                        if (dbActivities == null)
                            View.DisplayActivityMoved();
                        else
                            View.DisplayActivityMoved(dbActivities.Where(a => a.InEditPredecessorsMode && a.Status == FieldStatus.error).Any(), dbActivities.Where(a => a.InEditPredecessorsMode && a.Status == FieldStatus.errorfatherlinked).Any(), dbActivities.Where(a => a.InEditPredecessorsMode && a.Status == FieldStatus.errorsummarylinked).Any());
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ActivityToChild);
                    }
                    else
                    {
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ActivityToChildErrors);
                        View.DisplayUnableToMoveActivity();
                    }
                    if (dbActivities != null)
                        View.LoadActivities(dbActivities, startDate, endDate, deadLine);

                }
            }

            public void SetResources(long idActivity, List<long> idResources, List<dtoLiteMapActivity> activities)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    Boolean complessionRecalculated = false;
                    dtoLiteMapActivity activity = activities.Where(a => a.IdActivity == idActivity).FirstOrDefault();
                    List<dtoResource> resources = Service.SetActivityResources(idActivity, idResources, ref complessionRecalculated, View.UnknownUser);
                    if (resources!=null)
                    {
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, View.IdProject, ModuleProjectManagement.ActionType.ActivitySetResources);
                        View.DisplaySavedActivityResources((activity != null) ? activity.Current.Name : "", idResources.Count);
                        View.ReloadResources(new Dictionary<long, List<dtoResource>> { { idActivity, resources } }, (complessionRecalculated) ? Service.GetActivityCompletionTree(idActivity) : null);
                    }
                    else {
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, View.IdProject, ModuleProjectManagement.ActionType.ActivityUnableSetResources);
                        View.DisplayUnableToSaveActivityResources((activity != null) ? activity.Current.Name : "");
                    }
                }
            }
            #region "Add"
                public void AddChildren(long idProject, long idActivity, Int32 number, Boolean linked, List<dtoLiteMapActivity> activities)
                {
                    AddActivities(AddActivityAction.AsChildren, idProject,idActivity, number, linked, 0, activities);
                }
                public void AddActivitiesAfter(long idProject, long idActivity, Int32 number, Boolean linked, List<dtoLiteMapActivity> activities)
                {
                    AddActivities(AddActivityAction.After, idProject, idActivity, number, linked, 0, activities);
                }
                public void AddActivitiesBefore(long idProject, long idActivity, Int32 number, List<dtoLiteMapActivity> activities)
                {
                    AddActivities(AddActivityAction.Before, idProject, idActivity, number, false, 0, activities);
                }
                public void AddActivitiesToProject(long idProject, Int32 number, Boolean linked, List<dtoLiteMapActivity> activities)
                {
                    AddActivities(AddActivityAction.ToProject, idProject,0, number, linked, 0, activities);
                }
                public void AddSummaryToProject(long idProject, Int32 number, Boolean linked, Int32 children, List<dtoLiteMapActivity> activities)
                {
                    AddActivities(AddActivityAction.ToProject, idProject, 0, number, linked, children, activities);
                }
                private void AddActivities(AddActivityAction action, long idProject, long idActivity, Int32 number, Boolean linked, Int32 children, List<dtoLiteMapActivity> activities)
                {
                    if (UserContext.isAnonymous)
                        View.DisplaySessionTimeout();
                    else
                    {
                        dtoField<DateTime?> startDate = new dtoField<DateTime?>();
                        dtoField<DateTime?> endDate = new dtoField<DateTime?>();
                        dtoField<DateTime?> deadLine = new dtoField<DateTime?>();

                        List<dtoMapActivity> dbActivities = Service.AddActivitiesToProject(action, idProject, idActivity,View.GetDefaultActivityName(), number,linked,children, activities, View.InEditStartDate, View.InEditDeadline, ref startDate, ref endDate, ref deadLine, View.UnknownUser);
                        if (dbActivities!=null && dbActivities.Where(a => a.IsNew).Any())
                        {
                            View.DisplayActivitiesAdded();
                            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectMapAddedActivities);
                        }
                        else
                        {
                            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectMapAddingActivityErrors);
                            View.DisplayUnableToAddActivities();
                        }
                        if (dbActivities != null)
                            View.LoadActivities(dbActivities, startDate, endDate, deadLine);
                    }
                }
            #endregion

            #region "Virtual Delete"
                public void ActivityVirtualDeleted(long idProject, long idActivity, List<dtoLiteMapActivity> activities)
                {
                    if (UserContext.isAnonymous)
                        View.DisplaySessionTimeout();
                    else
                    {
                        dtoLiteMapActivity activity = activities.Where(a => a.IdActivity == idActivity).FirstOrDefault();
                        setDelete(idActivity, activities);
                        View.ReloadCompletion(Service.GetActivityCompletionTree(idActivity));
                        View.DisplayActivityRemoved((activity != null) ? activity.Current.Name : "", GetChildrenCount(idActivity, activities));

                   
                        List<dtoMapActivity> dbActivities = Service.GetActivities(idProject, View.ProjectIdCommunity, CurrentIdModule, View.UnknownUser, activities);
                        if (dbActivities == null)
                        {
                            View.DisplayErrorGetActivitiesFromDB();
                            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectMapErrorFromDb);
                        }
                        else
                        {
                            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectMapForEditing);
                            View.LoadActivities(dbActivities);
                        }
                    }
                }
                public void VirtualDeleteActivity(long idProject, long idActivity, List<dtoLiteMapActivity> activities)
                {
                    if (UserContext.isAnonymous)
                        View.DisplaySessionTimeout();
                    else
                    {
                        dtoLiteMapActivity activity = activities.Where(a => a.IdActivity == idActivity).FirstOrDefault();
                        if (Service.SetActivityVirtualDelete(idActivity, true))
                        {
                            setDelete(idActivity,activities);
                            View.ReloadCompletion(Service.GetActivityCompletionTree(idActivity));
                            View.DisplayActivityRemoved((activity != null) ? activity.Current.Name : "", GetChildrenCount(idActivity, activities));
                            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ActivityVirtualDelete);
                        }
                        else
                        {
                            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ActivityVirtualDeleteErrors);
                            View.DisplayUnableToRemoveActivity((activity != null) ? activity.Current.Name : "", GetChildrenCount(idActivity, activities));
                        }
                        List<dtoMapActivity> dbActivities = Service.GetActivities(idProject, View.ProjectIdCommunity, CurrentIdModule, View.UnknownUser, activities);
                        if (dbActivities == null)
                        {
                            View.DisplayErrorGetActivitiesFromDB();
                            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectMapErrorFromDb);
                        }
                        else
                        {
                            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectMapForEditing);
                            View.LoadActivities(dbActivities);
                        }
                    }
                }
                private void setDelete(long idActivity,List<dtoLiteMapActivity> activities) {
                    dtoLiteMapActivity activity = activities.Where(a => a.IdActivity == idActivity).FirstOrDefault();
                    if (activity != null) {
                        activity.IsDeleted = true;
                        foreach (dtoLiteMapActivity child in activities.Where(a => a.IdParent == idActivity))
                        {
                            setChildrenDelete(child, activities);
                        }
                    }
                    foreach (dtoLiteMapActivity act in activities.Where(a => a.IsDeleted))
                    {
                        if (act.IsSummary)
                        {
                            act.Previous.PredecessorsIdString = "";
                            act.Previous.Predecessors = "";
                            act.Current.PredecessorsIdString = "";
                            act.Current.Predecessors = "";
                        }
                        else
                            act.UpdateIdPredecessors(activities);
                    }
                }
                private void setChildrenDelete(dtoLiteMapActivity activity, List<dtoLiteMapActivity> activities)
                {
                    activity.IsDeleted = true;
                    foreach (dtoLiteMapActivity child in activities.Where(a => a.IdParent == activity.IdActivity))
                    {
                        setChildrenDelete(child, activities);
                    }
                }

                private long GetChildrenCount(long idActivity, List<dtoLiteMapActivity> activities)
                {
                    long children = 0;
                    foreach (dtoLiteMapActivity child in activities.Where(a => a.IdParent == idActivity))
                    {
                        children += 1 + GetChildrenCount(child.IdActivity, activities);
                    }
                    return children;
               }
            #endregion

        #endregion
    }
}