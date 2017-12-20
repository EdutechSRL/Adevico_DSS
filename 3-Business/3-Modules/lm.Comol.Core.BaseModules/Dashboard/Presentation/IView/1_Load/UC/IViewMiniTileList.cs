using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewMiniTileList : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        DisplayMoreItems MoreItemsAs { get; set; }
        Int32 MiniTileDisplayItems { get; set; }
        TileLayout CurrentTileLayout { get; set; }
        Boolean IsInitialized { get; set; }
        Boolean AutoUpdateLayout { get; set; }
        Boolean DisplayLessCommand { get; set; }
        Boolean DisplayMoreCommand { get; set; }
        DisplayNoticeboard CurrentDisplayNoticeboard { get; set; }
        long IdCurrentDashboard { get; set; }
        long IdCurrentTag { get; set; }
        long IdCurrentTile { get; set; }
        Boolean IsPreview { get; set; }
        GroupItemsBy CurrentGroupItemsBy { get; set; }
        OrderItemsBy CurrentOrderItemsBy { get; set; }
        void DisplaySessionTimeout(String url);
        void InitalizeControlForTile(litePageSettings pageSettings, UserCurrentSettings userSettings, List<dtoItemFilter<OrderItemsBy>> items, long idDashboard, Boolean moreTiles, Boolean moreCommunities, long preloadIdTile = -1);
        void LoadMiniTiles(List<dtoTileDisplay> items);
        void InitializeCommunitiesList(litePageSettings pSettings, UserCurrentSettings userSettings, List<dtoItemFilter<OrderItemsBy>> items, Boolean moreCommunities, dtoTileDisplay tile);
        void UpdateUserSettings(UserCurrentSettings settings);
        void DisplayErrorFromDB();
        void DisplayUnableToLoadTile(String url);
        void SendUserAction(int idCommunity, int idModule, long idDashboard, ModuleDashboard.ActionType action);
        void LoadDashboard(String url);
    }
}