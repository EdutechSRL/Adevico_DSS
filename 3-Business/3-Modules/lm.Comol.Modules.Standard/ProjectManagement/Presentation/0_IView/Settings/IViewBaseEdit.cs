using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.ProjectManagement.Domain; 

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation 
{
    public interface IViewBaseEdit : IViewPageBaseEdit
    {
        void SetProjectsUrl(String url);
        void SetProjectMapUrl(String url);
        void SetDashboardUrl(String url, PageListType dashboard);
        void DisplaySessionTimeout();
        void LoadWizardSteps(long idProject, int idCommunity,Boolean personal,Boolean forPortal, List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoProjectManagementStep>> steps);
    }
}