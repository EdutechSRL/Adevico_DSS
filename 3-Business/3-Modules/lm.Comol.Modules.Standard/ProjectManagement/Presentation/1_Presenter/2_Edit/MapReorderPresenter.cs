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
    public class MapReorderPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
           #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewMapReorder View
            {
                get { return (IViewMapReorder)base.View; }
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
            public MapReorderPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public MapReorderPresenter(iApplicationContext oContext, IViewMapReorder view)
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
            if (IsSessionTimeout())
                return;
            else
            {
                SetBackUrls(View.PreloadFromPage, View.PreloadIdContainerCommunity, project, cContext);
                if (project == null)
                {
                    View.DisplayUnknownProject();
                    View.SendUserAction(View.PreloadIdContainerCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectUnknown);
                }
                else
                {
                    Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
                    ModuleProjectManagement mPermission = (project.isPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(CurrentManager.GetModulePermission(UserContext.CurrentUserID, cContext.IdCommunity, CurrentIdModule));
                    PmActivityPermission pPermissions = Service.GetProjectPermission(idProject, UserContext.CurrentUserID);

                    if ((mPermission.Administration && !project.isPersonal) || (pPermissions.HasFlag(PmActivityPermission.ManageProject)))
                    {
                        View.AllowSave = true;
                        LoadItems(idProject);
                        View.LoadProjectDateInfo(project, false);
                        View.SetEditProjectUrl(RootObject.EditProject(project.Id, project.IdCommunity, project.isPortal, project.isPersonal, View.PreloadFromPage, View.IdContainerCommunity));
                    }
                    else
                        View.DisplayNoPermission(project.IdCommunity, currentIdModule);
                }
            }
        }

        private void LoadItems(long idProject, ReorderAction dAction = ReorderAction.AskAlways) {
            LoadItems(idProject,Service.GetProjectTreeForReorder(idProject),dAction);
        }
        private void LoadItems(long idProject, List<dtoActivityTreeItem> tasks, ReorderAction dAction = ReorderAction.AskAlways)
        {
           
            List<ReorderAction> actions = new List<ReorderAction>();
            if (tasks == null)
                View.LoadAvailableActions(actions, ReorderAction.Ignore, false);
            else if (tasks.Where(t => t.HasLinks).Any())
            {
                actions.Add(ReorderAction.RemoveConflictPredecessors);
                actions.Add(ReorderAction.RemoveAllPredecessors);
                actions.Add(ReorderAction.AskAlways);
                View.LoadAvailableActions(actions, dAction, true);
            }
            else
                View.LoadAvailableActions(actions, ReorderAction.Ignore, true);
           
            View.LoadActivities(tasks);
            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ReorderLoadItems);
        }
        /// <summary>
        /// Set return url
        /// </summary>
        /// <param name="fromPage"></param>
        /// <param name="idContainerCommunity"></param>
        /// <param name="project"></param>
        /// <param name="cContext"></param>
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
        
        /// <summary>
        /// initialize project context (find id community, if is personal, if is for portal
        /// </summary>
        /// <param name="idProject"></param>
        /// <param name="cContex"></param>
        /// <returns></returns>
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
                cContex.IdCommunity = project.IdCommunity;

            return project;
        }
        
        /// <summary>
        /// Save new project date
        /// </summary>
        /// <param name="idProject">project id</param>
        /// <param name="editStartDate">new start date </param>
        /// <param name="editDeadLine">new deadline</param>
        public void SaveProjectDate(long idProject,DateTime? editStartDate, DateTime? editDeadLine) {
            if (IsSessionTimeout())
                return;
            else if (editStartDate != View.PreviousStartDate || editDeadLine != View.PreviousDeadline)
            {
                dtoField<DateTime?> startDate = new dtoField<DateTime?>();
                dtoField<DateTime?> endDate = new dtoField<DateTime?>();
                dtoField<DateTime?> deadLine = new dtoField<DateTime?>();

                dtoProject project = Service.SetProjectDates(idProject,editStartDate,editDeadLine, ref startDate, ref endDate, ref deadLine);
                if (project!=null)
                {
                    View.DisplaySavedDates(startDate,endDate,deadLine);
                    View.SendUserAction(project.IdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectSavedStartDateDeadline);
                }
                else
                {
                    View.DisplayErrorSavingDates();
                    View.SendUserAction(project.IdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectSavingErrorStartDateDeadline);
                }
            }
        }

        public void ApplyReorder(List<dtoReorderGraphActivity> activities, ReorderAction dAction = ReorderAction.Ignore)
        {
            long idProject = View.IdProject;
            if (IsSessionTimeout())
                return;
            else if (activities.Any())
            {
                activities = Service.AnalyzeActionsForReorder(idProject, activities);
                ReorderError result = AllowReorder(activities, dAction);
                if (result== ReorderError.None)
                    result = Service.ReorderActivities(idProject, activities, dAction);
                switch (result) { 
                    case ReorderError.None:
                        View.DisplayReorderCompleted();
                        //ModuleProjectManagement.ActionType uAction = ModuleProjectManagement.ActionType.ReorderApplied;
                        //switch (dAction) { 
                        //    case ReorderAction.RemoveAllPredecessors:
                        //        uAction = ModuleProjectManagement.ActionType.ReorderAppliedWithAllLinksRemoved;
                        //        break;
                        //    case ReorderAction.RemoveConflictPredecessors:
                        //        uAction = ModuleProjectManagement.ActionType.ReorderAppliedWithCyclesInvolvedLinksRemoved;
                        //        break;
                        //}
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ReorderApplied);
                        LoadItems(idProject, dAction);
                        break;
                    case ReorderError.ProjectMapChanged:
                        View.DisplayConfirmActions(result, new List<dtoReorderAction>() { new dtoReorderAction() { Action = ReorderAction.ReloadProjectMap }, new dtoReorderAction() { Action = ReorderAction.ReloadProjectMapWithReorderedItems, Selected=true } });
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ReorderProjectMapChanged);
                        LoadItems(idProject, activities, dAction);
                        break;
                    case ReorderError.DataAccess:
                        View.DisplayConfirmActions(result);
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ReorderDataSavingErrors);
                        LoadItems(idProject, activities, dAction);
                        break;
                    case ReorderError.InConflictPredecessorsFound:
                    case ReorderError.SummaryWithLinks:
                        View.DisplayConfirmActions(result, new List<dtoReorderAction>() { new dtoReorderAction(ReorderAction.RemoveConflictPredecessors,true), new dtoReorderAction(ReorderAction.RemoveAllPredecessors) });
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ReorderCyclesFound);
                        LoadItems(idProject, activities, dAction);
                        break;
                }
            }
            else
                LoadItems(idProject, dAction);
        }

        public void ConfirmReorder(List<dtoReorderGraphActivity> activities, ReorderAction dAction = ReorderAction.Ignore)
        {
            long idProject = View.IdProject;
            if (IsSessionTimeout())
                return;
            else{
                switch(dAction){
                    case ReorderAction.ReloadProjectMap:
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ReorderReloadProjectMap);
                        LoadItems(idProject,View.GetDefaultAction());
                        break;
                    case ReorderAction.ReloadProjectMapWithReorderedItems:
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ReorderReloadProjectMapWithReorderedItems);
                        LoadItems(idProject, activities, dAction);
                        break;
                    case ReorderAction.RemoveAllPredecessors:
                    case ReorderAction.RemoveConflictPredecessors:
                        activities = Service.AnalyzeActionsForReorder(idProject, activities);
                        if (activities.Where(a=> a.Status!= FieldStatus.removed).Any()){
                            if (Service.ReorderActivities(idProject, activities, dAction, true)== ReorderError.None){
                                View.DisplayReorderCompleted();
                                View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ReorderApplied);
                                LoadItems(idProject, dAction);
                            }
                            else
                                View.DisplayUnableToReorder();
                        }
                        else if (activities.Any())
                            View.DisplayNoActivitiesToReorder();
                        break;
                    default:
                        LoadItems(idProject, activities, dAction);
                        break;
                }
            }
        }

        private ReorderError AllowReorder(List<dtoReorderGraphActivity> activities, ReorderAction dAction = ReorderAction.Ignore) { 
            ReorderError result = Service.AllowReorder(activities);
            switch(result){
                case ReorderError.InConflictPredecessorsFound:
                case ReorderError.SummaryWithLinks:
                    result = (dAction== ReorderAction.RemoveAllPredecessors || dAction== ReorderAction.RemoveConflictPredecessors) ? ReorderError.None: result;
                    break;
            }
            return result;
        }

        private void LoadItems(long idProject, List<dtoReorderGraphActivity> activities, ReorderAction dAction = ReorderAction.AskAlways)
        {
            LoadItems(idProject, Service.GetProjectTreeForReorder(idProject, activities), dAction);
        }
        public Boolean IsSessionTimeout()
        {
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout();
                return true;
            }
            return false;
        }
    }
}