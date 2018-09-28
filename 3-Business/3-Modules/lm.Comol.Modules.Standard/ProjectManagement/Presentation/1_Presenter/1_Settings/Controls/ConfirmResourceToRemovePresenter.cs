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
    public class ConfirmResourceToRemovePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewConfirmResourceToRemove View
            {
                get { return (IViewConfirmResourceToRemove)base.View; }
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
            public ConfirmResourceToRemovePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ConfirmResourceToRemovePresenter(iApplicationContext oContext, IViewConfirmResourceToRemove view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idProject, long idResource, String description =""){
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else {
                PmActivityPermission permissions = Service.GetProjectPermission(idProject, UserContext.CurrentUserID);
                Boolean allowDelete = HasPermission(permissions,PmActivityPermission.ManageResources);
                ProjectResource resource = Service.GetResourceForRemoving(idProject, idResource);
                View.AllowDelete = allowDelete;
                String name ="";
                if (resource == null)
                {
                    View.AllowDelete = false;
                    View.DisplayUnknownResource();
                }
                else
                {
                    name = (resource.Type== ResourceType.Internal) ? ((resource.Person == null) ? View.UnknownUser : resource.Person.SurnameAndName) : (String.IsNullOrEmpty(resource.LongName) ? resource.ShortName : resource.LongName);
                    if (allowDelete)
                        InitView(idProject, idResource, name, resource.AssignedActivities, resource.CompletedActivities, description);
                    else 
                        View.NoPermissionToRemoveResource(name);
                }
            }
        }
        public void InitView(long idProject, long idResource, String resourceName, long assignedTasks, long completedTasks, String description = "")
        {
            if (!String.IsNullOrEmpty(description) && !String.IsNullOrEmpty(resourceName))
                View.SetDescription((description.Contains("{0}") ? String.Format(description, resourceName):description),assignedTasks,completedTasks);
            View.IdProject = idProject;
            View.IdResource = idResource;

            List<RemoveAction> actions = new List<RemoveAction>();
            actions.Add(RemoveAction.FromAllAndRecalculateCompletion);
            actions.Add(RemoveAction.FromAllAssignments);
            actions.Add(RemoveAction.FromNotStartedAssignments);
                        
            if (completedTasks > 0) {
                actions.Add(RemoveAction.FromNotCompletedAssignmentsAndRecalculateCompletion);
                actions.Add(RemoveAction.FromNotCompletedAssignments);
            }
            View.ResourceName = resourceName;
            View.LoadAvailableActions(actions, RemoveAction.FromAllAndRecalculateCompletion);
        }

        public Boolean Delete(long idResource,  RemoveAction action) {
            return Service.SetResourceVirtualDelete(idResource,  true, action);
        }

        private Boolean HasPermission(PmActivityPermission permissions, PmActivityPermission permission)
        { 
            return lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)permissions, (long)permission);
        }
    }
}