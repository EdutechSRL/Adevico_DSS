using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Dashboard.Business;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public class DashboardTilesListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities service;
        private lm.Comol.Core.Tag.Business.ServiceTags servicetag;
        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;
        protected virtual IViewDashboardTilesList View
        {
            get { return (IViewDashboardTilesList)base.View; }
        }
        private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities Service
        {
            get
            {
                if (service == null)
                    service = new lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities(AppContext);
                return service;
            }
        }
        private lm.Comol.Core.Tag.Business.ServiceTags ServiceTags
        {
            get
            {
                if (servicetag == null)
                    servicetag = new lm.Comol.Core.Tag.Business.ServiceTags(AppContext);
                return servicetag;
            }
        }
        private Int32 CurrentIdModule
        {
            get
            {
                if (currentIdModule == 0)
                    currentIdModule = CurrentManager.GetModuleID(ModuleDashboard.UniqueCode);
                return currentIdModule;
            }
        }
        public DashboardTilesListPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public DashboardTilesListPresenter(iApplicationContext oContext, IViewDashboardTilesList view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView(Int32 idCommunity, DisplayNoticeboard noticeboard, litePageSettings pageSettings, UserCurrentSettings userSettings, long idDashboard, Boolean moreTiles)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(RootObject.LoadPortalView(UserContext.CurrentUserID, DashboardViewType.Tile, userSettings.GroupBy, userSettings.OrderBy, noticeboard, -1, -1, true, moreTiles, false));
            else
            {
                View.IdCurrentCommunity = idCommunity;
                View.IdCurrentDashboard = idDashboard;
                View.TileDisplayItems = pageSettings.MaxItems;
                View.MoreItemsAs = pageSettings.More;
                View.AutoUpdateLayout = pageSettings.AutoUpdateLayout;
                View.CurrentGroupItemsBy = userSettings.GroupBy;
                View.CurrentOrderItemsBy = userSettings.OrderBy;
                View.DisplayLessCommand = moreTiles;
                View.DisplayMoreCommand = !moreTiles;
                View.CurrentDisplayNoticeboard = noticeboard;
                View.TileRedirectOn = pageSettings.TileRedirectOn;
                InitializeView(noticeboard,pageSettings, userSettings, idDashboard, moreTiles);
            }
        }
        private void InitializeView(DisplayNoticeboard noticeboard, litePageSettings pageSettings, UserCurrentSettings userSettings, long idDashboard, Boolean moreTiles)
        {
            switch (userSettings.GroupBy)
            { 
                case GroupItemsBy.CommunityType:
                    InitializeTileView(pageSettings.TileRedirectOn,noticeboard, idDashboard, pageSettings.MaxItems, pageSettings.TileLayout, pageSettings.AutoUpdateLayout, userSettings.GroupBy, TileType.CommunityType);
                    break;
                case GroupItemsBy.Tile:
                    InitializeTileView(pageSettings.TileRedirectOn, noticeboard, idDashboard, pageSettings.MaxItems, pageSettings.TileLayout, pageSettings.AutoUpdateLayout, userSettings.GroupBy, TileType.CombinedTags);
                    break;
                case GroupItemsBy.Tag:
                    InitializeTagView(pageSettings.TileRedirectOn, noticeboard, idDashboard, pageSettings.MaxItems, pageSettings.TileLayout, pageSettings.AutoUpdateLayout);
                    break;
                case GroupItemsBy.None:
                    if (!View.IsPreview)
                        View.LoadDashboard(RootObject.LoadPortalView(UserContext.CurrentUserID, DashboardViewType.List, GroupItemsBy.None, OrderItemsBy.LastAccess, noticeboard));
                    break;
            }
        }
        private void InitializeTileView(DashboardViewType tileRedirectOn, DisplayNoticeboard noticeboard, long idDashboard, Int32 tileDisplayItems, TileLayout tLayout, Boolean autoUpdateLayout, GroupItemsBy groupBy, TileType type)
        {
            List<dtoLiteTile> tiles = (View.IsPreview) ? Service.TilesGetForDashboard(idDashboard, type, false) : Service.TilesGetForUser(UserContext.CurrentUserID, idDashboard, type);
            if (tiles == null || !tiles.Any())
                View.DisplayUnableToLoadTile(RootObject.LoadPortalView(UserContext.CurrentUserID, DashboardViewType.List, GroupItemsBy.None, OrderItemsBy.LastAccess, noticeboard));
            View.CurrentTileLayout = Service.GetTileLayout(noticeboard,tiles.Count, tLayout, autoUpdateLayout);
            LoadTiles(tileRedirectOn, noticeboard, idDashboard, tiles, tileDisplayItems, groupBy);
        }
        private void InitializeTagView(DashboardViewType tileRedirectOn, DisplayNoticeboard noticeboard, long idDashboard, Int32 tileDisplayItems, TileLayout tLayout, Boolean autoUpdateLayout)
        {
            List<dtoLiteTile> tiles = (View.IsPreview) ? Service.TilesGetForDashboard(idDashboard, TileType.CommunityTag, false) : Service.TilesGetForUser(UserContext.CurrentUserID, idDashboard, TileType.CommunityTag);
            if (tiles == null || !tiles.Any())
                View.DisplayUnableToLoadTile(RootObject.LoadPortalView(UserContext.CurrentUserID, DashboardViewType.List, GroupItemsBy.None, OrderItemsBy.LastAccess, noticeboard));
            View.CurrentTileLayout = Service.GetTileLayout(noticeboard, tiles.Count, tLayout, autoUpdateLayout);
            LoadTags(tileRedirectOn, noticeboard, idDashboard, tiles, tileDisplayItems, GroupItemsBy.Tag);
        }
        private void LoadTiles(DashboardViewType tileRedirectOn, DisplayNoticeboard noticeboard, long idDashboard, List<dtoLiteTile> tiles, Int32 tileDisplayItems, GroupItemsBy groupBy)
        {
            OrderItemsBy orderby = View.CurrentOrderItemsBy;
            Boolean displayLessCommand = View.DisplayLessCommand;

            Language l = CurrentManager.GetDefaultLanguage();

            displayLessCommand = (displayLessCommand) && tiles.Count() + ((noticeboard == DisplayNoticeboard.Hide) ? 0 : 1) > tileDisplayItems;
            List<dtoTileDisplay> items = null;
            if (View.IsPreview)
                items = tiles.Select(t => new dtoTileDisplay(t, UserContext.Language.Id, l.Id, true) { CommandUrl = RootObject.DashboardPreview(idDashboard, tileRedirectOn, groupBy, orderby, t.Tile.Id) }).ToList();
            else
                items = tiles.Select(t => new dtoTileDisplay(t, UserContext.Language.Id, l.Id) { CommandUrl = RootObject.LoadPortalView(UserContext.CurrentUserID, tileRedirectOn, groupBy, orderby, noticeboard, t.Tile.Id, -1, true, displayLessCommand) }).ToList();
            
            if (displayLessCommand)
            {
                View.LoadTiles(noticeboard,items);
                View.DisplayMoreCommand = false;
                View.DisplayLessCommand = true;
            }
            else
            {
                View.LoadTiles( noticeboard, items.Skip(0).Take(tileDisplayItems - ((noticeboard == DisplayNoticeboard.Hide) ? 0 : 1)).ToList());
                View.DisplayMoreCommand = tiles.Count() + ((noticeboard == DisplayNoticeboard.Hide) ? 0 : 1) > tileDisplayItems;
            }
        }
        private void LoadTags(DashboardViewType tileRedirectOn, DisplayNoticeboard noticeboard, long idDashboard, List<dtoLiteTile> tiles, Int32 tileDisplayItems, GroupItemsBy groupBy)
        {
            OrderItemsBy orderby = View.CurrentOrderItemsBy;
            Boolean displayLessCommand = View.DisplayLessCommand;
            Language l = CurrentManager.GetDefaultLanguage();

            displayLessCommand = displayLessCommand = (displayLessCommand) && tiles.Count() + ((noticeboard == DisplayNoticeboard.Hide) ? 0 : 1) > tileDisplayItems;
            List<dtoTileDisplay> items = null;
            if (View.IsPreview)
                items = tiles.Select(t => new dtoTileDisplay(t, UserContext.Language.Id, l.Id, true) { CommandUrl = RootObject.DashboardPreview(idDashboard, tileRedirectOn, groupBy, orderby, t.Tile.Id, t.Tile.GetAvailableIdTags().FirstOrDefault()) }).ToList();
            else
                items = tiles.Select(t => new dtoTileDisplay(t, UserContext.Language.Id, l.Id) { CommandUrl = RootObject.LoadPortalView(UserContext.CurrentUserID, tileRedirectOn, groupBy, orderby, noticeboard, t.Tile.Id, t.Tile.GetAvailableIdTags().FirstOrDefault(), true, displayLessCommand) }).ToList();
            if (displayLessCommand)
            {
                View.LoadTiles(noticeboard, items);
                View.DisplayMoreCommand = false;
                View.DisplayLessCommand = true;
            }
            {
                View.LoadTiles( noticeboard, items.Skip(0).Take(tileDisplayItems - ((noticeboard == DisplayNoticeboard.Hide) ? 0 : 1)).ToList());
                View.DisplayMoreCommand = tiles.Count() + ((noticeboard == DisplayNoticeboard.Hide) ? 0 : 1) > tileDisplayItems;
            }
        }
        public void ShowMoreTiles(DisplayNoticeboard noticeboard, long idDashboard, Int32 tileDisplayItems, TileLayout tLayout, Boolean autoUpdateLayout, GroupItemsBy groupBy, Boolean value)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(RootObject.LoadPortalView(UserContext.CurrentUserID, DashboardViewType.Tile, groupBy, View.CurrentOrderItemsBy, noticeboard, -1, -1, true, value, false));
            else
            {
                View.DisplayLessCommand = value;
                View.DisplayMoreCommand = !value;
                if (value)
                    View.SendUserAction(0, CurrentIdModule, idDashboard, ModuleDashboard.ActionType.TileDashboardMoreTiles);
                else
                    View.SendUserAction(0, CurrentIdModule, idDashboard, ModuleDashboard.ActionType.TileDashboardLessTiles);
                switch (groupBy)
                {
                    case GroupItemsBy.CommunityType:
                        InitializeTileView(View.TileRedirectOn,noticeboard,idDashboard, tileDisplayItems, tLayout, autoUpdateLayout, groupBy, TileType.CommunityType);
                        break;
                    case GroupItemsBy.Tile:
                        InitializeTileView(View.TileRedirectOn, noticeboard, idDashboard, tileDisplayItems, tLayout, autoUpdateLayout, groupBy, TileType.CombinedTags);
                        break;
                    case GroupItemsBy.Tag:
                        InitializeTileView(View.TileRedirectOn, noticeboard, idDashboard, tileDisplayItems, tLayout, autoUpdateLayout, groupBy, TileType.CommunityTag);
                        break;
                    case GroupItemsBy.None:
                        if (!View.IsPreview)
                            View.LoadDashboard(RootObject.LoadPortalView(UserContext.CurrentUserID, DashboardViewType.List, GroupItemsBy.None, OrderItemsBy.LastAccess, DisplayNoticeboard.OnRight));
                        break;
                }   
            }
        }
    }
}