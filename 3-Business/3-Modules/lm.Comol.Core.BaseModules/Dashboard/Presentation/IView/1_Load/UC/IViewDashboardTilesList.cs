using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewDashboardTilesList : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        DisplayMoreItems MoreItemsAs { get; set; }
        Int32 TileDisplayItems { get; set; }
        TileLayout CurrentTileLayout { get; set; }
        DisplayNoticeboard CurrentDisplayNoticeboard { get; set; }
        Boolean IsInitialized { get; set; }
        Boolean AutoUpdateLayout { get; set; }
        Boolean DisplayLessCommand { get; set; }
        Boolean DisplayMoreCommand { get; set; }
        Boolean IsPreview { get; set; }
        DashboardViewType TileRedirectOn { get; set; }
        long IdCurrentDashboard { get; set; }
        Int32 IdCurrentCommunity { get; set; }
        GroupItemsBy CurrentGroupItemsBy { get; set; }
        OrderItemsBy CurrentOrderItemsBy { get; set; }
        void DisplaySessionTimeout(String url);
        void InitalizeControl(Int32 idCommunity,DisplayNoticeboard noticeboard, litePageSettings pageSettings, UserCurrentSettings userSettings, long idDashboard, Boolean moreTiles);
        void LoadTiles( DisplayNoticeboard noticeboard, List<dtoTileDisplay> items);
        void UpdateUserSettings(UserCurrentSettings settings);
        void DisplayErrorFromDB();
        void DisplayUnableToLoadTile(String url);
        void SendUserAction(int idCommunity, int idModule, long idDashboard, ModuleDashboard.ActionType action);
        void LoadDashboard(String url);
    }
}