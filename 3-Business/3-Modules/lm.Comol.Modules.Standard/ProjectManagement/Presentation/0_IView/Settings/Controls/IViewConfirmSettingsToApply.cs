using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation 
{
    public interface IViewConfirmSettingsToApply : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean isInitialized { get; set; }
        Boolean DisplayDescription { get; set; }
        Boolean DisplayCommands { get; set; }
        Boolean DisplayApply { get; set; }
        Boolean RaiseCommandEvents { get; set; }

        void InitializeControl(long idProject, dtoProject dto, dtoProjectStatistics cStatistics, String description = "");
        void SetDescription(String description);
        void DisplaySessionTimeout();
        void DisplayNoPermissionToApply();
        void LoadActions(dtoProjectSettingsAction actions, DateTime? currentDate = null, DateTime? newDate =null);
        dtoProjectSettingsSelectedActions GetSelectedActions();
    }
}