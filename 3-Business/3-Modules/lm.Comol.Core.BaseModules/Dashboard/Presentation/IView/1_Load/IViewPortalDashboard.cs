using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewPortalDashboard : IViewDefaultDashboardLoader
    {
        long PreloadIdTile { get; }
        long PreloadIdTag { get; }
        Boolean LoadFromUrl { get; }
        Boolean PreloadMoreTiles { get; }
        Boolean PreloadMoreCommunities { get; }
        DisplaySearchItems PreloadSearch { get; }
        String PreloadSearchText { get; }
        PlainLayout CurrentLayout { get; set; }

        void InitalizeTopBar(liteDashboardSettings settings, UserCurrentSettings userSettings, Boolean moreTiles, String searchBy = "");
        void InitializeLayout(PlainLayout layout, DisplayNoticeboard display);
        void InitializeCommunitiesList(litePageSettings pSettings, UserCurrentSettings userSettings, List<dtoItemFilter<OrderItemsBy>> items);
        void IntializeCombinedView(litePageSettings pSettings, UserCurrentSettings userSettings, List<dtoItemFilter<OrderItemsBy>> items, long idDashboard, Boolean moreTiles, Boolean moreCommunities, long preloadIdTile = -1);
        void IntializeTileView(Int32 idCommunity, DisplayNoticeboard noticeboard, litePageSettings pSettings, UserCurrentSettings userSettings,long idDashboard, Boolean moreTiles);

        void EnableFullWidth(Boolean value);
    }
}