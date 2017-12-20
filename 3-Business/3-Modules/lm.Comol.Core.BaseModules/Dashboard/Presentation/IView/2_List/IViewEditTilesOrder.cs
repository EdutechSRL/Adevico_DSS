using lm.Comol.Core.BaseModules.Tiles.Domain;
using lm.Comol.Core.Dashboard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewEditTilesOrder : IViewBaseEditSettings
    {
        WizardDashboardStep PreloadStep { get; }
        WizardDashboardStep CurrentStep { get; set; }
        void InitializeView(WizardDashboardStep step, String url, Boolean allowGenerate = false);
        void LoadTiles(List<dtoTileForReorder> tiles);
        void DisplayNoTiles();
        List<dtoTileForReorder> GetTiles();
    }
}