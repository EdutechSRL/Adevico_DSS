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
    public class MiniTileListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities service;
        private lm.Comol.Core.Tag.Business.ServiceTags servicetag;
        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;
        protected virtual IViewMiniTileList View
        {
            get { return (IViewMiniTileList)base.View; }
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
        public MiniTileListPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public MiniTileListPresenter(iApplicationContext oContext, IViewMiniTileList view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView(litePageSettings pageSettings, UserCurrentSettings userSettings, List<dtoItemFilter<OrderItemsBy>> items, long idDashboard, Boolean moreTiles, Boolean moreCommunities, long idTile = -1, long idTag = -1)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(RootObject.LoadPortalView(UserContext.CurrentUserID, DashboardViewType.Combined, userSettings.GroupBy, userSettings.OrderBy, userSettings.GetNoticeboard(DashboardViewType.Combined), idTile, idTile, true, moreTiles, moreCommunities));
            else
            {
                View.CurrentDisplayNoticeboard = userSettings.GetNoticeboard(DashboardViewType.Combined);
                View.IdCurrentDashboard = idDashboard;
                View.MiniTileDisplayItems = pageSettings.MiniTileDisplayItems;
                View.MoreItemsAs = pageSettings.More;
                View.AutoUpdateLayout = pageSettings.AutoUpdateLayout;
                View.CurrentGroupItemsBy = userSettings.GroupBy;
                View.CurrentOrderItemsBy = userSettings.OrderBy;
                View.DisplayLessCommand = moreTiles;
                View.DisplayMoreCommand = !moreTiles;
                InitializeView( pageSettings, userSettings, items, idDashboard, moreTiles, moreCommunities, idTile, idTag);
            }
        }
        private void InitializeView( litePageSettings pageSettings, UserCurrentSettings userSettings, List<dtoItemFilter<OrderItemsBy>> items, long idDashboard, Boolean moreTiles, Boolean moreCommunities, long preloadIdTile = -1, long preloadIdTag = -1)
        {
            dtoTileDisplay selectedTile = null;
            long idTag = preloadIdTag;
            switch (userSettings.GroupBy)
            { 
                case GroupItemsBy.CommunityType:
                    InitializeTileView(idDashboard, pageSettings.MiniTileDisplayItems, pageSettings.TileLayout, pageSettings.AutoUpdateLayout, userSettings.GroupBy, userSettings.OrderBy, preloadIdTile, TileType.CommunityType, out selectedTile);
                    break;
                case GroupItemsBy.Tile:
                    InitializeTileView(idDashboard, pageSettings.MiniTileDisplayItems, pageSettings.TileLayout, pageSettings.AutoUpdateLayout, userSettings.GroupBy, userSettings.OrderBy, preloadIdTile, TileType.CombinedTags, out selectedTile);
                    break;
                case GroupItemsBy.Tag:
                    InitializeTagView(idDashboard, pageSettings.MiniTileDisplayItems, pageSettings.TileLayout, pageSettings.AutoUpdateLayout, userSettings.OrderBy, ref idTag);
                    break;
                case GroupItemsBy.None:
                    View.LoadDashboard(RootObject.LoadPortalView(UserContext.CurrentUserID, DashboardViewType.List, GroupItemsBy.None, OrderItemsBy.LastAccess, DisplayNoticeboard.OnRight));
                    break;
            }
            if (selectedTile!=null){
                userSettings.IdSelectedTile = selectedTile.Id;
                View.InitializeCommunitiesList(pageSettings, userSettings, items, moreCommunities, selectedTile);
            }
        }
        private void InitializeTileView(long idDashboard, Int32 miniTileDisplayItems, TileLayout tLayout, Boolean autoUpdateLayout, GroupItemsBy groupBy, OrderItemsBy orderBy, long preloadIdTile, TileType type, out  dtoTileDisplay selectedTile)
        {
            List<dtoLiteTile> tiles = (View.IsPreview) ? Service.TilesGetForDashboard(idDashboard, type, false) : Service.TilesGetForUser(UserContext.CurrentUserID, idDashboard, type);
            if (tiles != null)
                tiles = tiles.Where(t => t.Tile.Type != TileType.DashboardUserDefined && t.Tile.Type != TileType.UserDefined).ToList();
            if (tiles == null || !tiles.Any())
            {
                View.DisplayUnableToLoadTile(RootObject.LoadPortalView(UserContext.CurrentUserID, DashboardViewType.List, GroupItemsBy.None, OrderItemsBy.LastAccess, DisplayNoticeboard.OnRight));
                View.CurrentTileLayout = tLayout;
                selectedTile = null;
            }
            else
            {
                View.CurrentTileLayout = Service.GetTileLayout(tiles.Count, tLayout, autoUpdateLayout);
                LoadMiniTiles(idDashboard, tiles, miniTileDisplayItems, groupBy, orderBy, preloadIdTile, out selectedTile);
            }
        }
        private void InitializeTagView(long idDashboard, Int32 miniTileDisplayItems, TileLayout tLayout, Boolean autoUpdateLayout, OrderItemsBy orderBy, ref long preloadIdTag)
        {
            List<dtoLiteTile> tiles = Service.TilesGetForUser(UserContext.CurrentUserID, idDashboard, TileType.CommunityTag);
            if (tiles == null || !tiles.Any())
                View.DisplayUnableToLoadTile(RootObject.LoadPortalView(UserContext.CurrentUserID, DashboardViewType.List, GroupItemsBy.None, OrderItemsBy.LastAccess, DisplayNoticeboard.OnRight));
            View.CurrentTileLayout = Service.GetTileLayout(tiles.Count, tLayout, autoUpdateLayout);
            LoadMiniTags(tiles, miniTileDisplayItems, GroupItemsBy.Tag, orderBy, ref preloadIdTag);
        }
        private void LoadMiniTiles(long idDashboard, List<dtoLiteTile> tiles, Int32 miniTileDisplayItems, GroupItemsBy groupBy, OrderItemsBy orderBy, long preloadIdTile, out dtoTileDisplay selectedTile)
        {
            Boolean displayLessCommand = View.DisplayLessCommand;

            if(!tiles.Where(t=> t.Tile.Id== preloadIdTile).Any())
                preloadIdTile  = tiles.Select(t=> t.Tile.Id).FirstOrDefault();
            View.IdCurrentTile = preloadIdTile;
            Language l = CurrentManager.GetDefaultLanguage();

            displayLessCommand = (displayLessCommand) && tiles.Where(t => t.Tile.Id != preloadIdTile).Count() > miniTileDisplayItems;
            List<dtoTileDisplay> items = null;
            Boolean forPreview = View.IsPreview;
            if (forPreview)
            {
                items = tiles.Where(t => t.Tile.Id != preloadIdTile).Select(t => new dtoTileDisplay(t, UserContext.Language.Id, l.Id, forPreview) { CommandUrl = RootObject.DashboardPreview(idDashboard, DashboardViewType.Combined, groupBy, orderBy, t.Tile.Id) }).ToList();
            }
            else
                items = tiles.Where(t => t.Tile.Id != preloadIdTile).Select(t => new dtoTileDisplay(t, UserContext.Language.Id, l.Id) { CommandUrl = RootObject.LoadPortalView(UserContext.CurrentUserID, DashboardViewType.Combined, groupBy, orderBy, DisplayNoticeboard.OnRight, t.Tile.Id, -1, true, displayLessCommand, false) }).ToList();
            
            if (displayLessCommand)
            {
                View.LoadMiniTiles(items);
                View.DisplayMoreCommand = false;
                View.DisplayLessCommand = true;
            }
            else
            {
                View.LoadMiniTiles(items.Skip(0).Take(miniTileDisplayItems).ToList());
                View.DisplayMoreCommand = (tiles.Where(t => t.Tile.Id != preloadIdTile).Count() > miniTileDisplayItems);
            }
            selectedTile = tiles.Where(t => t.Tile.Id == preloadIdTile).Select(t => new dtoTileDisplay(t, UserContext.Language.Id, l.Id, forPreview)).FirstOrDefault();
        }
        private void LoadMiniTags(List<dtoLiteTile> tiles, Int32 miniTileDisplayItems, GroupItemsBy groupBy, OrderItemsBy orderBy, ref long preloadIdTag)
        {
            long idTag = preloadIdTag;
            Boolean displayLessCommand = View.DisplayLessCommand;
            if (!tiles.Where(t => t.HasOnlyThisIdTag(idTag)).Any())
                idTag = tiles.Where(t => t.HasOnlyOneTag()).Select(t => t.Tile.GetFirstIdTag()).FirstOrDefault();
            View.IdCurrentTag = idTag;
            Language l = CurrentManager.GetDefaultLanguage();

            displayLessCommand = (displayLessCommand) && tiles.Where(t => !t.HasOnlyThisIdTag(idTag)).Count() > miniTileDisplayItems;
            Boolean forPreview = View.IsPreview;
            List<dtoTileDisplay> items = tiles.Where(t => !t.HasOnlyThisIdTag(idTag)).Select(t => new dtoTileDisplay(t, UserContext.Language.Id, l.Id, forPreview) { CommandUrl = RootObject.LoadPortalView(UserContext.CurrentUserID, DashboardViewType.Combined, groupBy, orderBy, DisplayNoticeboard.OnRight, t.Tile.Id, -1, true, displayLessCommand, false) }).ToList();
            if (displayLessCommand)
            {
                View.LoadMiniTiles(items);
                View.DisplayMoreCommand = false;
                View.DisplayLessCommand = true;
            }
            {
                View.LoadMiniTiles(items.Skip(0).Take(miniTileDisplayItems).ToList());
                View.DisplayMoreCommand = (tiles.Where(t => !t.HasOnlyThisIdTag(idTag)).Count() > miniTileDisplayItems);
            }
            preloadIdTag = idTag;
        }
        public void ShowMoreCommunities(long idDashboard, Int32 miniTileDisplayItems, TileLayout tLayout, Boolean autoUpdateLayout, GroupItemsBy groupBy, OrderItemsBy orderBy, long idCurrentTile, Boolean value, Boolean moreCommunities)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(RootObject.LoadPortalView(UserContext.CurrentUserID, DashboardViewType.Combined, groupBy, orderBy, View.CurrentDisplayNoticeboard, idCurrentTile, View.IdCurrentTag, true, value, moreCommunities));
            else
            {
                View.DisplayLessCommand = value;
                View.DisplayMoreCommand = !value;
                if (value)
                    View.SendUserAction(0, CurrentIdModule,idDashboard, ModuleDashboard.ActionType.CombinedDashboardMoreTiles);
                else
                    View.SendUserAction(0, CurrentIdModule,idDashboard, ModuleDashboard.ActionType.CombinedDashboardLessTiles);
                dtoTileDisplay selectedTile = null;
                switch (groupBy)
                {
                    case GroupItemsBy.CommunityType:
                        InitializeTileView(idDashboard, miniTileDisplayItems, tLayout, autoUpdateLayout, groupBy, orderBy, idCurrentTile, TileType.CommunityType, out selectedTile);
                        break;
                    case GroupItemsBy.Tile:
                        InitializeTileView(idDashboard, miniTileDisplayItems, tLayout, autoUpdateLayout, groupBy, orderBy, idCurrentTile, TileType.CombinedTags, out selectedTile);
                        break;
                    case GroupItemsBy.Tag:
                        InitializeTileView(idDashboard, miniTileDisplayItems, tLayout, autoUpdateLayout, groupBy, orderBy, idCurrentTile, TileType.CommunityTag, out selectedTile);
                        break;
                    case GroupItemsBy.None:
                        View.LoadDashboard(RootObject.LoadPortalView(UserContext.CurrentUserID, DashboardViewType.List, GroupItemsBy.None, OrderItemsBy.LastAccess, DisplayNoticeboard.OnRight));
                        break;
                }   
            }
        }
        //View.IdCurrentCommunityType = idCommunityType;
        //View.IdCurrentRemoveCommunityType = idRemoveCommunityType;
        //View.IdCurrentTag = idTag;
        //View.IdCurrentTile = idTile;
        //View.PageType = pageSettings.Type;
        //View.DefaultPageSize = pageSettings.MaxMoreItems;
        //Int32 itemsCount = Service.GetSubscribedCommunitiesCount(UserContext.CurrentUserID, pageSettings.Type, idCommunityType, idRemoveCommunityType, idTile, idTag);
        //Int32 pageSize = InitializePageSize(pageSettings, itemsCount);
        //View.CurrentPageSize = pageSize;

        //OrderItemsBy orderBy = items.Where(i => i.Selected).Select(i => i.Value).FirstOrDefault();
        //Boolean ascending = false;
        //if (orderBy != userSettings.OrderBy)
        //{
        //    switch (orderBy)
        //    {
        //        //case OrderItemsBy.ActivatedOn:
        //        //case OrderItemsBy.ClosedOn:
        //        //case OrderItemsBy.CreatedOn:
        //        //case OrderItemsBy.LastAccess:
        //        case OrderItemsBy.Name:
        //            ascending = true;
        //            break;
        //    }
        //}
        //else
        //    ascending = userSettings.Ascending;

        //View.CurrentOrderBy = orderBy;
        //View.CurrentAscending = ascending;
        //LoadCommunities(pageSettings.Type, userSettings, 0, pageSize, orderBy, ascending, idCommunityType, idRemoveCommunityType, idTile, idTag);


        //private Int32 InitializePageSize(litePageSettings pageSettings, Int32 itemsCount)
        //{
        //    Int32 pageSize = pageSettings.MaxItems;
        //    if (pageSettings.Range != null && pageSettings.Range.IsInRange(itemsCount))
        //        pageSize = pageSettings.Range.DisplayItems;
        //    if (itemsCount > pageSize)
        //        View.DisplayMoreCommand = true;
        //    return pageSize;
        //}
        //public void LoadCommunities(DashboardViewType view,UserCurrentSettings userSettings, Int32 pageIndex, Int32 pageSize, OrderItemsBy orderBy, Boolean ascending, Int32 idCommunityType = -1, Int32 idRemoveCommunityType = -1, long idTile = -1, long idTag = -1)
        //{
        //    if (UserContext.isAnonymous)
        //        View.DisplaySessionTimeout();
        //    else
        //    {
        //        if (View.DisplayLessCommand)
        //        {
        //            Int32 itemsCount = Service.GetSubscribedCommunitiesCount(UserContext.CurrentUserID, view, idCommunityType, idRemoveCommunityType, idTile, idTag);
        //            PagerBase pager = new PagerBase();
        //            pager.PageSize = pageSize;//Me.View.CurrentPageSize
        //            pager.Count = (itemsCount > 0) ? itemsCount - 1 : 0;
        //            pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
        //            View.Pager = pager;
        //        }
        //        List<dtoSubscriptionItem> items = Service.GetSubscribedCommunities(UserContext.CurrentUserID, view, pageIndex, pageSize, orderBy, ascending, idCommunityType, idRemoveCommunityType, idTile, idTag);
        //        if (items != null)
        //        {
        //            List<Int32> withNews = View.GetIdcommunitiesWithNews(items.Where(i => i.Status != SubscriptionStatus.communityblocked && i.Status != SubscriptionStatus.blocked && i.Status != SubscriptionStatus.waiting).Select(i => i.Community.Id).ToList(), UserContext.CurrentUserID);
        //            if (withNews.Any())
        //                items.Where(i => withNews.Contains(i.Community.Id)).ToList().ForEach(i => i.HasNews = true);

        //            Language l = CurrentManager.GetDefaultLanguage();
        //            Dictionary<Int32, List<String>> tags = ServiceTags.GetCommunityAssociationToString(items.Select(i => i.Community.Id).ToList(), UserContext.Language.Id,l.Id, true);
        //            if (tags.Any())
        //            {
        //                foreach (dtoSubscriptionItem item in items.Where(i => tags.ContainsKey(i.Community.Id)))
        //                {
        //                    item.Community.Tags = tags[item.Community.Id];
        //                }
        //            }
                    

        //            View.LoadItems(items, orderBy, ascending );
        //            if (userSettings != null)
        //            {
        //                userSettings.Ascending = ascending;
        //                userSettings.OrderBy = orderBy;
        //                userSettings.View = view;
        //                View.UpdateUserSettings(userSettings);
        //            }
        //            View.SendUserAction(0, CurrentIdModule, ModuleDashboard.ActionType.ListDashboardLoadSubscribedComminities);
        //        }
        //        else
        //            View.DisplayErrorFromDB();
        //    }
        //}
       
    }
}