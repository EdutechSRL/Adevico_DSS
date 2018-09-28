using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.ProjectManagement.Domain; 

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation 
{
    public interface IViewEditProject : IViewBaseEdit
    {
        long PreloadRequiredActions { get; }
        long PreloadAddedActions { get; }
        Boolean PreloadAddedProject { get; }
        Boolean AllowSave { get; set; }
        dtoProject GetProject();
        String GetDefaultCalendarName();

        void InitializeProject(dtoProject project,Boolean hasActivities, dtoResource owner, List<ProjectAvailability> items, ProjectAvailability cAvailability, Boolean allowChangeOwner, Boolean allowChangeOwnerFromResource, Boolean allowChangeOwnerFromCommunity);

        void DisplayUnknownProject();
        void DisplayProjectSaved();
        void DisplayProjectSavingError();
        void DisplayProjectSavingError(DateTime? startDate, FlagDayOfWeek days);
        void DisplayAddedInfo(long  rActions, long cActions);
        void DisplayConfirmActions(dtoProject dto, dtoProjectStatistics cStatistics);
        void InitializeControlToSelectOwner(Int32 idCommunity, List<Int32> hideUsers);
        void InitializeControlToSelectOwnerFromProject(List<dtoProjectResource> resources);
        void UpdateDefaultResourceSelector(List<dtoResource> resources);
        void UpdateSettings(dtoProjectSettingsSelectedActions actions, DateTime? startDate, DateTime? endDate);
        void UpdateSettings(DateTime? startDate, DateTime? endDate);
        void DisplayUnableToChangeOwner();
        void DisplayOwnerChanged(String name);
    }
}
