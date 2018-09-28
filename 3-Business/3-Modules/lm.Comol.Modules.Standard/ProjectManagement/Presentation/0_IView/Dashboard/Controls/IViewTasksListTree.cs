using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewTasksListTree : IViewTasksListBase
    {
        #region "Context"
            ItemsGroupBy CurrentGroupBy { get; set; }
        #endregion
        Dictionary<Int32, String> GetMonthNames();
        String GetDateTimePattern();
        String GetPortalName();
        String GetUnknownCommunityName();
        String GetContainerCssClass(ItemsGroupBy groupBy);
        String GetStartRowId(ItemsGroupBy groupBy);
        Int32 GetCellsCount(PageListType pageType, ItemsGroupBy groupBy);
        Dictionary<TimeGroup,String> GetTimeTranslations();

        void LoadTasks(List<dtoTasksGroup> groups);
        void DisplayTasksCompletionSaved(List<dtoMyAssignmentCompletion> items, Dictionary<long,Dictionary<ResourceActivityStatus, long>> projectCompletion, Int32 savedTasks, Int32 unsavedTasks, Boolean updateSummary);
    }
}