using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.ProjectManagement.Domain; 

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation 
{
    public interface IViewEditActivity : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        long IdActivity { get; set; }
        long IdProject { get; set; }
        Int32 ProjectIdCommunity { get; set; }
        Boolean AllowEdit { get; set; }
        Boolean AllowDelete { get; set; }
        Boolean isCompleted { get; set; }
        String UnknownUser { get; }
        List<dtoLiteMapActivity> Activities { get; set; }
        dtoWorkingDay DefaultWorkingDay { get; set; }

        void InitializeControl(Int32 idProjectCommunity, long idActivity, List<dtoLiteMapActivity> activities);

        void DisplayUnknownActivity();
        void DisplaySessionTimeout();

        List<dtoActivityLink> LoaderLinks { get; set; }
        List<dtoActivityLink> CurrentLinks();
        List<dtoActivityCompletion> LoaderAssignments { get; set; }
        List<dtoActivityCompletion> CurrentAssignments();
        dtoActivity CurrentSettings();
        void LoadResources(Boolean allowEdit,List<dtoResource> resources, List<long> selected);
        void LoadSettings(dtoActivity activity, Boolean allowDurationEstimated, Boolean allowMilestones);
        void RemoveResourceFromSelection(long idResource);
        void ReloadSettings(dtoActivity activity, Boolean allowDurationEstimated, Boolean allowMilestones);
        void LoadAvailableLinks(Dictionary<long, String> activities, List<dtoActivityLink> links);
        void LoadCompletion(Int32 completion,Boolean isCompleted, List<dtoActivityCompletion> assignments , dtoActivityPermission permission);
        void ReloadCompletion(Int32 completion, Boolean isCompleted, List<dtoActivityCompletion> assignments);
        void LoadSummaryCompletion(Int32 completion, Boolean isCompleted, dtoActivityPermission permission);
        void DisplayLinkAdded(Int32 count);
        void DisplayUnableToAddLink();
        void DisplayLinkRemoved();
        void DisplayUnableToRemoveLink();
        //void DisplayLinksSaved();
        //void DisplayLinksUnsaved();
        void DisplayLinksInCycles(List<dtoActivityLink> items);
        void DisplayUnableToRemoveActivity(String name, long children);
        void DisplayUnableToSaveActivity();
        void DisplayActivitySaved(long idActivity, dtoField<DateTime?> startDate, dtoField<DateTime?> endDate,dtoField<DateTime?> deadLine);
        void SendUserAction(int idCommunity, int idModule, long idProject, ModuleProjectManagement.ActionType action);
    }
}
