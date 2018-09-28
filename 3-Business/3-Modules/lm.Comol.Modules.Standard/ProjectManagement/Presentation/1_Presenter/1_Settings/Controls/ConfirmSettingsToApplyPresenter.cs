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
    public class ConfirmSettingsToApplyPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewConfirmSettingsToApply View
            {
                get { return (IViewConfirmSettingsToApply)base.View; }
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
            public ConfirmSettingsToApplyPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ConfirmSettingsToApplyPresenter(iApplicationContext oContext, IViewConfirmSettingsToApply view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idProject,dtoProject dto, dtoProjectStatistics cStatistics, String description = "")
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else {
                PmActivityPermission permissions = Service.GetProjectPermission(idProject, UserContext.CurrentUserID);
                Boolean allowUpdate = HasPermission(permissions,PmActivityPermission.ManageProject);
                View.DisplayApply = allowUpdate;
                if (!String.IsNullOrEmpty(description))
                    View.SetDescription(description);
                if (allowUpdate)
                    LoadActions(dto, cStatistics);
                else
                    View.DisplayNoPermissionToApply();
            }
        }
        private void LoadActions(dtoProject dto, dtoProjectStatistics cStatistics)
        {
            dtoProjectSettingsAction actions = new dtoProjectSettingsAction();
            if (dto.StartDate != cStatistics.StartDate)
                actions.DateActions = new List<ConfirmActions>() {ConfirmActions.Apply, ConfirmActions.Hold};
            if (!dto.AllowMilestones && cStatistics.Milestones > 0)
                actions.MilestonesActions = new List<ConfirmActions>() {ConfirmActions.Apply, ConfirmActions.Hold};
            if (!dto.AllowSummary && cStatistics.Summaries > 0)
                actions.SummariesActions = new List<ConfirmActions>() {ConfirmActions.Apply, ConfirmActions.Hold};
            if (!dto.AllowEstimatedDuration && cStatistics.EstimatedActivities > 0)
                actions.EstimatedActions = new List<ConfirmActions>() {ConfirmActions.Apply, ConfirmActions.Hold};
            if (dto.DateCalculationByCpm && dto.DateCalculationByCpm != cStatistics.DateCalculationByCpm)
                actions.CpmActions = new List<ConfirmActions>() { ConfirmActions.Apply, ConfirmActions.Hold };
            else if (!dto.DateCalculationByCpm && dto.DateCalculationByCpm != cStatistics.DateCalculationByCpm)
                actions.ManualActions = new List<ConfirmActions>() { ConfirmActions.Apply, ConfirmActions.Hold };

            actions.EstimatedActivities = cStatistics.EstimatedActivities;
            actions.Activities = cStatistics.Activities;
            actions.Summaries = cStatistics.Summaries;
            actions.Milestones = cStatistics.Milestones;
            View.LoadActions(actions, cStatistics.StartDate, dto.StartDate);
        }

        private Boolean HasPermission(PmActivityPermission permissions, PmActivityPermission permission)
        { 
            return lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)permissions, (long)permission);
        }
    }
}