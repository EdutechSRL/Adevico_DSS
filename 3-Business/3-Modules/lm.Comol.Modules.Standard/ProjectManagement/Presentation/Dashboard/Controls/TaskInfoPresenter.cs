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
    public class TaskInfoPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
           #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewTaskInfo View
            {
                get { return (IViewTaskInfo)base.View; }
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
            public TaskInfoPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public TaskInfoPresenter(iApplicationContext oContext, IViewTaskInfo view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion


        public void InitView(dtoPlainTask task, Boolean displayOthersCompletion)
        {
            View.IdTask = (task!=null) ? task.Id: 0;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                if (task == null)
                    View.DisplayUnknownTask();
                else
                {
                    View.IdAssignment = task.IdAssignment;
                    View.isCompleted = task.IsCompleted;
                    if (task.ProjectInfo.SetOthersCompletion || displayOthersCompletion){
                        Boolean allowEdit =task.ProjectInfo.SetOthersCompletion && !(task.IsCompleted && task.Completeness==100);
                        View.LoadSettings(task, ParseAssignments(task.Assignments,task.ProjectResources, allowEdit), allowEdit);
                    }
                    else if (task.ProjectInfo.ViewMyCompletion || task.ProjectInfo.SetMyCompletion)
                        View.LoadSettings(task, task.MyCompleteness, task.ProjectInfo.SetMyCompletion);
                    else
                        View.LoadSettings(task, true);
                }
            }
        }

        #region "Assignments"
            private List<dtoActivityCompletion> ParseAssignments(List<dtoActivityCompletion> assignments, List<dtoResource> resources, Boolean allowEdit)
            {
                foreach (dtoActivityCompletion assignment in assignments)
                {
                    dtoResource resource = resources.Where(r => r.IdResource == assignment.IdResource).FirstOrDefault();
                    if (resource != null)
                    {
                        assignment.IdPerson = resource.IdPerson;
                        assignment.DisplayName = resource.LongName;
                        assignment.AllowEdit = allowEdit;
                    }
                }
                return assignments;
            }
            public Boolean ConfirmCompletion(long idTask, ref Boolean updateSummary) {
                Boolean result = false;
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    dtoTaskCompletion tCompletion = Service.SetTaskCompletion(idTask, true, ref updateSummary);
                    if (tCompletion != null)
                        View.UpdateTaskCompletion(tCompletion);
                    result = (tCompletion != null);
                }
                return result;
            }

            public Boolean SaveSettings(long idTask, List<dtoActivityCompletion> assignments, ref Boolean updateSummary)
            {
                Boolean result = false;
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    dtoTaskCompletion tCompletion = Service.SetTaskCompletion(idTask, false, ref updateSummary, assignments);
                    if (tCompletion != null)
                        View.UpdateTaskCompletion(tCompletion);
                    result = (tCompletion != null);
                }
                return result;
            }
            public Boolean SaveSettings(long idTask, long idAssignment, Int32 completeness, ref Boolean updateSummary)
            {
                Boolean result = false;
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    dtoMyAssignmentCompletion item = Service.UpdateTaskCompletion(idTask, idAssignment, completeness, ref updateSummary);
                    if (item != null)
                        View.UpdateMyCompletion(item);
                    result = (item != null);
                }
                return result;
            }
        #endregion
        //private void ReloadData(long idProject, long idActivity, List<dtoLiteMapActivity> activities, Boolean reloadSettings = false)
        //{
        //    litePmActivity activity = Service.GetLiteActivity(idActivity);
        //    liteProjectSettings project = Service.GetProjectSettings((activity != null) ? activity.IdProject : 0);
        //    if (activity == null || project == null)
        //        View.DisplayUnknownActivity();
        //    else {
        //        dtoActivity dto = GetActivity(project, activity, activities);
        //        if (reloadSettings)
        //            View.ReloadSettings(dto, project.AllowEstimatedDuration, project.AllowMilestones);
        //    }
        //}

      


        //public void SaveActivity(long idProject, long idActivity, dtoActivity activity, List<long> idResources, List<dtoActivityLink> links, List<dtoActivityCompletion> assignments, Boolean isCompleted, DateTime? prStartDate = null, DateTime? prEndDate = null, DateTime? prDeadline = null)
        //{
        //    if (UserContext.isAnonymous)
        //        View.DisplaySessionTimeout();
        //    else
        //    {
        //        dtoField<DateTime?> startDate = new dtoField<DateTime?>() { Init = prStartDate };
        //        dtoField<DateTime?> endDate = new dtoField<DateTime?>() { Init = prEndDate };
        //        dtoField<DateTime?> deadLine = new dtoField<DateTime?>() { Init = prDeadline };

        //        ActivityEditingErrors errors = Service.SaveActivity(idProject, idActivity, activity, idResources, links, assignments, isCompleted, ref startDate, ref endDate, ref deadLine);
        //        if (errors== ActivityEditingErrors.None)
        //        {
        //            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ActivitySaved);
        //            View.DisplayActivitySaved(idActivity,startDate, endDate, deadLine);
        //        }
        //        else{
        //            View.DisplayUnableToSaveActivity();
        //            View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ActivityUnableToSave);
        //            }
        //    }
        //}
    }
}