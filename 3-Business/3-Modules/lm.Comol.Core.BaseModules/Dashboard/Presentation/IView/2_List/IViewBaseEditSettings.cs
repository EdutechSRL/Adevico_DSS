using lm.Comol.Core.BaseModules.Tiles.Domain;
using lm.Comol.Core.Dashboard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewBaseEditSettings : IViewBase
    {
        #region "Preload"
            long PreloadIdDashboard { get; }
            Int32 PreloadIdCommunity { get; }
            DashboardType PreloadDashboardType { get; }
            Boolean PreloadFromAdd { get; }
        #endregion
        long IdDashboard { get; set; }
        Int32 IdContainerCommunity { get; set; }
        DashboardType DashboardType { get; set; }
        Boolean AllowSave { set; }
        void SetBackUrl(String url);
        void SetPreviewUrl(String url);
        void DisplayUnknownDashboard();
        void DisplayDeletedDashboard();
        void DisplayMessage(ModuleDashboard.ActionType action);
        void DisplayMessage(DashboardErrorType dbError);
        void LoadWizardSteps(List<lm.Comol.Core.Wizard.dtoNavigableWizardItem<dtoDashboardStep>> steps);
        void LoadWizardSteps(List<lm.Comol.Core.Wizard.NavigableWizardItem<Int32>> steps);
      
        void SendUserAction(int idCommunity, int idModule, long idDashboard, ModuleDashboard.ActionType action);
        void SendUserAction(int idCommunity, int idModule, ModuleDashboard.ActionType action);
        void RedirectToUrl(String url);
    }
}