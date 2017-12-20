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
    public class DashboardPreviewPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceDashboard service;
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities serviceDashboard;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewDashboardPreview View
            {
                get { return (IViewDashboardPreview)base.View; }
            }
            private ServiceDashboard Service
            {
                get
                {
                    if (service == null)
                        service = new ServiceDashboard(AppContext);
                    return service;
                }
            }

            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities ServiceDashboard
            {
                get
                {
                    if (serviceDashboard == null)
                        serviceDashboard = new lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities(AppContext);
                    return serviceDashboard;
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
            public DashboardPreviewPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public DashboardPreviewPresenter(iApplicationContext oContext, IViewDashboardPreview view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(WizardDashboardStep step, long idDashboard,Int32 idCommunity, DashboardType dashboardType)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.IdDashboard = idDashboard;
                View.CurrentStep = step;
                liteDashboardSettings settings = CurrentManager.Get<liteDashboardSettings>(idDashboard);
                Int32 idDashboardCommunity = (settings == null) ? ((dashboardType == DashboardType.Community) ? idCommunity : 0) : settings.IdCommunity;
                View.DashboardIdCommunity = idDashboardCommunity;
                if (settings == null)
                    View.DisplayUnknownDashboard();
                else
                {
                    View.InitializeSettingsInfo(settings);
                    UserCurrentSettings userSettings = GetUserSettings(step,settings);
                    View.CurrentSettings = userSettings;
                    View.EnableFullWidth((settings == null) ? false : settings.FullWidth);
                    InitalizeTopBar(settings, userSettings);
                    if (userSettings.View != DashboardViewType.Search)
                        LoadGroupBy(settings, userSettings);
                    InitializeView(settings, userSettings);
                    View.IsInitialized = true;
                }
            }
        }

        private UserCurrentSettings GetUserSettings(WizardDashboardStep step,liteDashboardSettings settings)
        {
            UserCurrentSettings userSettings = new UserCurrentSettings();
            userSettings.AfterUserLogon = settings.Container.Default.AfterUserLogon;
            userSettings.DefaultNoticeboard = settings.Container.Default.DefaultNoticeboard;
            userSettings.CombinedNoticeboard = settings.Container.Default.CombinedNoticeboard;
            userSettings.TileNoticeboard = settings.Container.Default.TileNoticeboard;
            userSettings.ListNoticeboard = settings.Container.Default.ListNoticeboard;
            userSettings.OrderBy = settings.Container.Default.OrderBy;

            switch(step){
                case WizardDashboardStep.CommunityTypes:
                    if (settings.Container.AvailableViews.Contains(DashboardViewType.Tile)){
                        userSettings.View = DashboardViewType.Tile;
                        userSettings.GroupBy =  GroupItemsBy.CommunityType;
                    }
                    break;
                case WizardDashboardStep.Modules:
                case WizardDashboardStep.Tiles:
                    if (settings.Container.AvailableViews.Contains(DashboardViewType.Tile)){
                        userSettings.View = DashboardViewType.Tile;
                        userSettings.GroupBy = View.PreloadGroupBy;
                        userSettings.OrderBy = View.PreloadOrderBy;
                        if (userSettings.GroupBy == GroupItemsBy.None )
                            userSettings.GroupBy =  GroupItemsBy.Tile;
                    }
                    break;
                //case WizardDashboardStep.None:
                //    userSettings.View = View.PreloadViewType;
                //    userSettings.GroupBy = View.PreloadGroupBy;
                //    if (userSettings.GroupBy== GroupItemsBy.None)
                //        userSettings.GroupBy = settings.Container.Default.GroupBy;
                //    break;
                default:
                    DashboardViewType pLoad = View.PreloadViewType;
                    userSettings.View = (pLoad == DashboardViewType.Search || pLoad == DashboardViewType.Combined) ? pLoad : settings.Container.Default.View;
                    userSettings.GroupBy = settings.Container.Default.GroupBy;
                    break;
            }
            switch (userSettings.GroupBy)
            {
                case GroupItemsBy.CommunityType:
                    userSettings.IdSelectedTile = View.PreloadIdTile;
                    userSettings.IdSelectedTag = -1;
                    break;
                case GroupItemsBy.Tag:
                    userSettings.IdSelectedTile = -1;
                    userSettings.IdSelectedTag = View.PreloadIdTag;
                    break;
                case GroupItemsBy.Tile:
                    userSettings.IdSelectedTile = View.PreloadIdTile;
                    userSettings.IdSelectedTag = -1;
                    break;
            }
            View.CurrentStep = step;
            userSettings.Ascending = (userSettings.OrderBy == OrderItemsBy.Name) ? true : false;
            return userSettings;
        }

        #region "TopBar"
            private void InitalizeTopBar(liteDashboardSettings settings, UserCurrentSettings userSettings)
            {
                View.InitializeSearch(settings.Container.Default.Search);
                LoadViews(settings, userSettings);
            }
            private void LoadViews(liteDashboardSettings settings, UserCurrentSettings userSettings)
            {
                List<dtoItemFilter<DashboardViewType>> views = settings.Container.AvailableViews.Where(v => v != DashboardViewType.Search).Select(v => new dtoItemFilter<DashboardViewType>() { Value = v, Selected = (v == userSettings.View) }).ToList();
                if (!views.Where(v => v.Selected).Any())
                    views.Add(new dtoItemFilter<DashboardViewType>() { Selected= true, Value= userSettings.View });
                if (views.Count > 1)
                {
                    views.FirstOrDefault().DisplayAs = ItemDisplayOrder.first;
                    views.LastOrDefault().DisplayAs = ItemDisplayOrder.last;
                }
                else if (views.Any())
                    views[0].DisplayAs = ItemDisplayOrder.first | ItemDisplayOrder.last;
                View.InitializeViewSelector(views);
            }
            private void LoadGroupBy(liteDashboardSettings settings, UserCurrentSettings userSettings)
            {
                switch (userSettings.View)
                {
                    case DashboardViewType.List:
                        break;
                    default:
                        List<dtoItemFilter<GroupItemsBy>> items = settings.Container.AvailableGroupBy.Select(v => new dtoItemFilter<GroupItemsBy>() { Value = v, Selected = (v == userSettings.GroupBy) }).ToList();
                        if (items.Any() && !items.Where(i=> i.Selected).Any()){
                            if (items.Where(i => i.Value != GroupItemsBy.None).Any())
                                items.Where(i => i.Value != GroupItemsBy.None).FirstOrDefault().Selected = true;
                            else
                                items.FirstOrDefault().Selected = true;
                        }

                        if (items.Count > 1)
                        {
                            items.FirstOrDefault().DisplayAs = ItemDisplayOrder.first;
                            items.LastOrDefault().DisplayAs = ItemDisplayOrder.last;
                        }
                        else if (items.Any())
                            items[0].DisplayAs = ItemDisplayOrder.first | ItemDisplayOrder.last;
                        View.InitializeGroupBySelector(items);
                        break;
                }
            }
        #endregion

        private void InitializeView(liteDashboardSettings settings, UserCurrentSettings userSettings)
        {
            if (!settings.Pages.Where(p => p.Type == userSettings.View).Any())
                userSettings.View = settings.Pages.Select(p=> p.Type).FirstOrDefault();
            List<dtoItemFilter<OrderItemsBy>> items = GetOrderByItems(settings, userSettings); 
            Dictionary<DashboardViewType, List<dtoItemFilter<OrderItemsBy>>> order = new  Dictionary<DashboardViewType, List<dtoItemFilter<OrderItemsBy>>>();
            order.Add(DashboardViewType.Tile, null);
            order.Add(DashboardViewType.Combined, items);
            order.Add(DashboardViewType.List, items);
            order.Add(DashboardViewType.Search, items);
            View.CurrentOrderItems = order;
            if (settings.Pages.Where(p => p.Type != DashboardViewType.Search && p.Type != DashboardViewType.Subscribe).Any())
                ChangeView(userSettings.View, settings, userSettings, order);
            else
                View.DisplayNoViewAvailable();
        }

        private List<dtoItemFilter<OrderItemsBy>> GetOrderByItems(liteDashboardSettings settings, UserCurrentSettings userSettings)
        {
            List<dtoItemFilter<OrderItemsBy>> items = settings.Container.AvailableOrderBy.Select(o => new dtoItemFilter<OrderItemsBy>() { Value = o, Selected = (o == userSettings.OrderBy) }).ToList();
            if (!items.Where(v => v.Selected).Any() && items.Any())
                items[0].Selected = true;
            if (items.Count > 1)
            {
                items.FirstOrDefault().DisplayAs = ItemDisplayOrder.first;
                items.LastOrDefault().DisplayAs = ItemDisplayOrder.last;
            }
            else if (items.Any())
                items[0].DisplayAs = ItemDisplayOrder.first | ItemDisplayOrder.last;
            return items;
        }
        private PlainLayout GetPlainLayout(liteDashboardSettings settings, DashboardViewType view)
        {
            PlainLayout layout = PlainLayout.box8box4;
            if (settings.Container.Default.GetNoticeboard(view) == DisplayNoticeboard.Hide)
                layout = PlainLayout.full;
            else if (settings.Pages.Where(p => p.Deleted == BaseStatusDeleted.None && p.Type == DashboardViewType.List).Any())
                layout = settings.Pages.Where(p => p.Deleted == BaseStatusDeleted.None && p.Type == DashboardViewType.List).FirstOrDefault().PlainLayout;
            return layout;
        }
        public void ChangeView(DashboardViewType view, long idDashboard,  UserCurrentSettings userSettings, Dictionary<DashboardViewType, List<dtoItemFilter<OrderItemsBy>>> order, String searchBy="")
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                liteDashboardSettings settings = CurrentManager.Get<liteDashboardSettings>(idDashboard);
                if (settings == null)
                    View.DisplayUnknownDashboard();
                else
                {
                    View.InitializeSettingsInfo(settings);
                    ChangeView(view, settings, userSettings, order, searchBy,true );
                }
            }
        }
        private void ChangeView(DashboardViewType view, liteDashboardSettings settings, UserCurrentSettings userSettings, Dictionary<DashboardViewType, List<dtoItemFilter<OrderItemsBy>>> order, String searchBy = "", Boolean firstLoad =false)
        {

            userSettings.View = view;
            View.CurrentSettings = userSettings;
            LoadViews(settings, userSettings);
            switch (view)
            {
                case DashboardViewType.List:
                    View.InitializeLayout(GetPlainLayout(settings, userSettings.View), settings.Container.Default.GetNoticeboard(userSettings.View));
                    View.InitializeCommunitiesList(settings.Pages.Where(p => p.Type == DashboardViewType.List).FirstOrDefault(), userSettings, order[view]);
                    break;
                case DashboardViewType.Combined:
                    if (firstLoad)
                        LoadGroupBy(settings, userSettings);
                    View.InitializeLayout(GetPlainLayout(settings, userSettings.View), settings.Container.Default.GetNoticeboard(userSettings.View));
                    View.IntializeCombinedView(settings.Pages.Where(p => p.Type == DashboardViewType.Combined).FirstOrDefault(), userSettings, order[view], settings.Id, View.PreloadIdTile);
                    break;
                case DashboardViewType.Tile:
                    if (firstLoad)
                        LoadGroupBy(settings, userSettings);
                    View.InitializeLayout(GetPlainLayout(settings, userSettings.View), settings.Container.Default.GetNoticeboard(userSettings.View));
                    View.IntializeTileView(0, settings.Container.Default.GetNoticeboard(userSettings.View), settings.Pages.Where(p => p.Type == DashboardViewType.Tile).FirstOrDefault(), userSettings, settings.Id);
                    break;
                case DashboardViewType.Search:
                    View.InitializeLayout(GetPlainLayout(settings, userSettings.View), DisplayNoticeboard.Hide);

                    liteTile tile = null;
                    long idTile = View.PreloadIdTile;
                    List<long> idTags = new List<long>();
                    Int32 idType = -1;
                    if (idTile > 0)
                    {
                        tile = Service.GetTile(idTile);
                        if (tile != null)
                        {
                            switch (tile.Type)
                            {
                                case TileType.CommunityType:
                                    if (tile.CommunityTypes != null)
                                    {
                                        idType = tile.CommunityTypes.FirstOrDefault();
                                        idTile = -1;
                                    }
                                    break;
                                case TileType.CommunityTag:
                                case TileType.CombinedTags:
                                    if (tile.Tags != null && tile.Tags.Any(t => t.Tag != null) && tile.Tags.Any(t => t.Deleted == BaseStatusDeleted.None))
                                        idTags.AddRange(tile.Tags.Where(t => t.Tag != null && t.Deleted == BaseStatusDeleted.None).Select(t => t.Tag.Id).ToList());
                                    break;
                            }
                        }
                    }
                    View.TagsToLoad = idTags;
                    lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters = null;
                    List<lm.Comol.Core.DomainModel.Filters.Filter> fToLoad = ServiceDashboard.GetDefaultFilters(UserContext.CurrentUserID, searchBy, idType, idTile, idTags, null, CommunityManagement.CommunityAvailability.Subscribed, -1).OrderBy(f => f.DisplayOrder).ToList();
                    if (fToLoad != null && fToLoad.Any())
                    {
                        filters = new lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters(fToLoad, CommunityManagement.CommunityAvailability.Subscribed, idType,idTile );
                        filters.IdcommunityType = idType;
                        if (!String.IsNullOrEmpty(searchBy))
                            filters.SearchBy = CommunityManagement.SearchCommunitiesBy.Contains;
                        filters.IdTile = idTile;
                        filters.IdTags = idTags;
                    }
                    else
                    {
                        filters = new lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters();
                        filters.IdcommunityType = idType;
                        filters.SearchBy = CommunityManagement.SearchCommunitiesBy.Contains;
                        filters.Value = searchBy;
                        filters.Availability = CommunityManagement.CommunityAvailability.Subscribed;
                        filters.IdOrganization = -1;
                        filters.IdTile = idTile;
                        filters.IdTags = idTags;
                    }
                    Int32 idUserLanguage = ((UserContext != null && UserContext.Language != null) ? UserContext.Language.Id : -2);
                    Int32 idDefaultLanguage = CurrentManager.GetDefaultIdLanguage();
                    View.InitializeSearchView(settings.Pages.Where(p => p.Type == DashboardViewType.Search).FirstOrDefault(), fToLoad, filters, order[DashboardViewType.Search], tile, idUserLanguage, idDefaultLanguage);
                    break;
            }
        }
        public void ChangeGroupBy(long idDashboard, DashboardViewType view, GroupItemsBy groupBy, UserCurrentSettings userSettings)
        {
            liteDashboardSettings settings = CurrentManager.Get<liteDashboardSettings>(idDashboard);
            if (settings == null)
                View.DisplayUnknownDashboard();
            else
            {
                View.InitializeSettingsInfo(settings);
                View.SelectedGroupBy = groupBy;
                userSettings.View = view;
                
                if (userSettings.GroupBy != groupBy)
                {
                    switch (userSettings.GroupBy)
                    {
                        case GroupItemsBy.CommunityType:
                        case GroupItemsBy.Tile:
                            userSettings.IdSelectedTile = -1;
                            break;
                        case GroupItemsBy.Tag:
                            userSettings.IdSelectedTag = -1;
                            break;
                    }

                }
                userSettings.GroupBy = groupBy;
                userSettings.OrderBy = (groupBy == settings.Container.Default.GroupBy) ? settings.Container.Default.OrderBy : OrderItemsBy.LastAccess;
                View.CurrentSettings = userSettings;
                ChangeView(userSettings.View, settings, userSettings, View.CurrentOrderItems);
            }
        }

        public void ApplyFilters(long idDashboard, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                liteDashboardSettings settings = CurrentManager.Get<liteDashboardSettings>(idDashboard);
                if (settings == null)
                    View.DisplayUnknownDashboard();
                else
                {
                    View.InitializeSettingsInfo(settings);
                    litePageSettings page = (settings != null) ? settings.Pages.Where(p => p.Type == DashboardViewType.Search).FirstOrDefault() : new litePageSettings() { Type = DashboardViewType.Search, MaxItems = 25, More = DisplayMoreItems.Hide, Noticeboard = DisplayNoticeboard.Hide, PlainLayout = PlainLayout.full };
                    liteTile tile = null;
                    long idTile = View.PreloadIdTile;
                    if (idTile > 0)
                        tile = Service.GetTile(idTile);
                    else if (filters.IdcommunityType > -1)
                        tile = Service.GetTileForCommunityType(filters.IdcommunityType);
                    List<long> tags = View.TagsToLoad;
                    if (tags != null && tags.Any() && idTile > 0)
                        filters.IdTags = tags;
                    Int32 idUserLanguage = ((UserContext != null && UserContext.Language != null) ? UserContext.Language.Id : -2);
                    View.ApplyFilters(page, filters, tile,idUserLanguage, CurrentManager.GetDefaultIdLanguage());
                }
            }
        }
    }
}