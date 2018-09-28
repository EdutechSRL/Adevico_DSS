using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewMapReorder : IViewBaseProjectMap
    {
        #region "Context"
             Boolean AllowSave { get; set; }
             
        #endregion

        DateTime? PreviousStartDate { get; }
        DateTime? PreviousDeadline { get; }

        ReorderAction GetDefaultAction();
        void DisplayErrorSavingDates();
        void DisplaySavedDates(dtoField<DateTime?> startDate, dtoField<DateTime?> endDate, dtoField<DateTime?> deadLine);
        /// <summary>
        /// Load available actions for tasks reorder
        /// </summary>
        /// <param name="actions">list of actions</param>
        /// <param name="dAction">default action to select</param>
        /// <param name="allowSave">to enable or disable tasks reordering</param>
        void LoadAvailableActions(List<ReorderAction> actions, ReorderAction dAction, Boolean allowReorder);

        /// <summary>
        /// Load tasks for reorder
        /// </summary>
        /// <param name="activities"></param>
        void LoadActivities(List<dtoActivityTreeItem> activities);
        void DisplayReorderCompleted();
        void DisplayUnableToReorder();
        void DisplayNoActivitiesToReorder();
        void DisplayConfirmActions(ReorderError errorFound, List<dtoReorderAction> actions = null);
    }
}