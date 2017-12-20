using lm.Comol.Core.BaseModules.Tiles.Domain;
using lm.Comol.Core.Dashboard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewSettingsList : IViewBase
    {
        #region "Context"
            Int32 IdContainerCommunity { get; set; }
            DashboardType CurrentType { get; set; }
            OrderSettingsBy CurrentOrderBy { get; set; }
            Boolean CurrentAscending { get; set; }
            Boolean FromRecycleBin { get; set; }
        #endregion

        #region "Common"
            void SendUserAction(int idCommunity, int idModule, ModuleDashboard.ActionType action);
            void SendUserAction(int idCommunity, int idModule, long idDashboard, ModuleDashboard.ActionType action);
        #endregion         
        String GetUnknownUserName();
        Dictionary<lm.Comol.Core.Dashboard.Domain.AvailableStatus, String> GetTranslatedStatus();
        void InitializeControl(ModuleDashboard permissions, DashboardType type, Int32 idCommunity, Boolean loadFromRecycleBin);
        void LoadSettings(List<dtoDashboardSettings> items);
        void DisplayErrorLoadingFromDB();
        void DisplayMessage(ModuleDashboard.ActionType action);
        void DisplayMessage(DashboardErrorType dbError);
    }
}