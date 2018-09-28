using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.ProjectManagement.Domain; 

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation 
{
    public interface IViewTaskInfo : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        long IdTask { get; set; }
        long IdAssignment { get; set; }
        PageListType CurrentPage { get; set; }
        Boolean isCompleted { get; set; }

        void InitializeControl(dtoPlainTask task, Boolean displayOthersCompletion, PageListType currentPage);

        List<dtoActivityCompletion> GetAssignments();

        void LoadSettings(dtoPlainTask task, List<dtoActivityCompletion> assignments, Boolean allowEdit);
        void LoadSettings(dtoPlainTask task, dtoField<String> myCompleteness, Boolean allowEdit);
        void LoadSettings(dtoPlainTask task, Boolean displayStatus);
        void LoadAttachments(Boolean allowDownload);
        void DisplayUnknownTask();
        void DisplaySessionTimeout();
        void SendUserAction(int idCommunity, int idModule, long idTask, ModuleProjectManagement.ActionType action);
        void UpdateMyCompletion(dtoMyAssignmentCompletion completion);
        void UpdateTaskCompletion(dtoTaskCompletion tCompletion);
    }
}