using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewTasksListMultipleTree : IViewTasksListBase
    {
        String GetPortalName();
        String GetUnknownCommunityName();
        String GetContainerCssClass(ItemsGroupBy groupby);
        String GetStartRowId(ItemsGroupBy groupby);
        Int32 GetCellsCount(PageListType pageType);

        void DisplayTasksCompletionSaved(List<dtoMyAssignmentCompletion> items, Dictionary<long, Dictionary<ResourceActivityStatus, long>> projectCompletion, Int32 savedTasks, Int32 unsavedTasks, Boolean updateSummary);
        void LoadTasks(List<dtoCommunityProjectTasksGroup> communities);
    }
}