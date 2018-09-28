using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewTasksListBase : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        #region "Context"
            dtoProjectContext PageContext { get; set; }
            String PortalName { get; }
            String UnknownUser { get; }
            Int32 IdContainerCommunity { get; set; }
            PageContainerType PageContainer { get; set; }
            PageListType PageType { get; set; }
            PageListType CurrentFromPage { get; set; }

            lm.Comol.Core.DomainModel.PagerBase Pager { get; set; }
            Int32 CurrentPageSize { get; set; }
            Boolean IsInitialized { get; set; }
            Int32 CurrentPageIndex { get; }
            Boolean CurrentAscending { get; set; }
            ProjectOrderBy CurrentOrderBy { get; set; }
        #endregion
        
        List<dtoMyAssignmentCompletion> GetMyTasksCompletion();


        void InitializeControl(dtoProjectContext context, Int32 idContainerCommunity, dtoItemsFilter filter, PageContainerType containerPage, PageListType fromPage, PageListType currentPage);
        void DisplayPager(Boolean display);
        void DisplayTasksCompletionSaved(List<dtoMyAssignmentCompletion> items, Int32 savedTasks, Int32 unsavedTasks, Boolean updateSummary);
        void LoadedNoTasks();
        void SaveMyCompletions();
        void SendUserAction(int idCommunity, int idModule, ModuleProjectManagement.ActionType action);
        void SendUserAction(int idCommunity, int idModule,long idProject, ModuleProjectManagement.ActionType action);
    }
}