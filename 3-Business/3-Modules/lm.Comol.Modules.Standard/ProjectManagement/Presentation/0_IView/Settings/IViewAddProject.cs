using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.ProjectManagement.Domain; 

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation 
{
    public interface IViewAddProject : IViewBaseEdit
    {
        Boolean AllowAdd { get; set; }
        

        dtoProject GetProjectToAdd();
        String GetDefaultProjectName();
        String GetDefaultCalendarName();
        String GetDefaultActivityName();
        void InitializeProject(dtoProject project, dtoResource owner, List<ProjectAvailability> items, ProjectAvailability cAvailability, Int32 activitiesToAdd=0);
        
        void DisplayProjectAddError();
        void DisplayProjectAddError(DateTime? startDate, FlagDayOfWeek days);
        void DisplayNotAvailableComunity();
        void DisplayNotAvailableForAddPortalProject();
        void DisplayNotAvailableForAddCommunityProject();
        void DisplayNotAvailableForAddPersonalProject();
      
        void RedirectToEdit(long idProject, int idCommunity, Boolean forPortal, Boolean isPersonal, long rActions=0, long cActions=0);
       
    }
}
