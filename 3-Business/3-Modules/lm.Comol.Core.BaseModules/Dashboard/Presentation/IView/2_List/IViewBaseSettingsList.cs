using lm.Comol.Core.BaseModules.Tiles.Domain;
using lm.Comol.Core.Dashboard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewBaseSettingsList : IViewBase
    {
        #region "Preload"
            Int32 PreloadIdCommunity { get; }
            Boolean PreloadRecycleBin { get; }
            DashboardType PreloadDashboardType { get; }
        #endregion
        Int32 IdContainerCommunity { get; set; }
        void SetBackUrl(String url);
        void SetRecycleUrl(String url);
        void SetAddUrl(String url);
        void SetTitle(DashboardType type, String name = "");
        void InitializeListControl(ModuleDashboard permissions, DashboardType type, Int32 idCommunity, Boolean loadFromRecycleBin);
        #region "Common"
            void SendUserAction(int idCommunity, int idModule, ModuleDashboard.ActionType action);
            void SendUserAction(int idCommunity, int idModule, long idDashboard, ModuleDashboard.ActionType action);
        #endregion          
    }
}