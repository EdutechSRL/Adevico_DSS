using lm.Comol.Core.Dashboard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tiles.Presentation 
{
    public interface IViewEditPageBase : IViewBase
    {
        #region "Preload"
            long PreloadIdTile { get; }
            Int32 PreloadIdCommunity { get; }
            DashboardType PreloadDashboardType { get; }
            long PreloadIdDashboard { get; }
            WizardDashboardStep PreloadStep { get; }
        #endregion
        long IdTile { get; set; }
        Int32 IdTileCommunity { get; set; }
        Int32 IdContainerCommunity { get; set; }
        void SetBackUrl(String url);
        void DisplayUnknownTile();
    }
}