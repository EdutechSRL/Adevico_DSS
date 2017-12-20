using lm.Comol.Core.Dashboard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tiles.Presentation 
{
    public interface IViewPageBase : IViewBase
    {
        #region "Preload"
            Int32 PreloadIdCommunity { get; }
            Boolean PreloadRecycleBin { get; }
            DashboardType PreloadDashboardType { get; }
            long PreloadIdTile { get; }
            long PreloadIdDashboard { get; }
            WizardDashboardStep PreloadStep { get; }
        #endregion
        Int32 IdTilesCommunity { get; set; }
        Int32 IdContainerCommunity { get; set; }
        void SetBackUrl(String url);
        void SetRecycleUrl(String url);
        void SetAddUrl(String url);
        Boolean AllowCommunityTypesTileAutoGenerate { set; }
        void InitializeListControl(ModuleDashboard permissions, DashboardType type, Int32 idCommunity, Boolean loadFromRecycleBin, long idTile,TileType preloadType);
        void SetDashboardSettingsBackUrl(String url, String name);
    }
}