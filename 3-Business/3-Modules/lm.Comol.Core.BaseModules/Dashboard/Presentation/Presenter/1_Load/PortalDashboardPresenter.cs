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
    public class PortalDashboardPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceDashboard service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewPortalDashboard View
            {
                get { return (IViewPortalDashboard)base.View; }
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
            private Int32 CurrentIdModule
            {
                get
                {
                    if (currentIdModule == 0)
                        currentIdModule = CurrentManager.GetModuleID(ModuleDashboard.UniqueCode);
                    return currentIdModule;
                }
            }
            public PortalDashboardPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public PortalDashboardPresenter(iApplicationContext oContext, IViewPortalDashboard view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(DashboardViewType view)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                if (UserContext.CurrentCommunityID > 0)
                {
                    Int32 idOrganization = CurrentManager.GetUserDefaultIdOrganization(UserContext.CurrentUserID);
                    View.GeneratePortalWebContext(idOrganization);
                    UserContext.CurrentCommunityID = 0;
                    UserContext.CurrentCommunityOrganizationID = idOrganization;
                    UserContext.WorkingCommunityID = 0;
                }

                Boolean moreTiles = View.LoadFromUrl && View.PreloadMoreTiles;
                liteDashboardSettings settings = Service.DashboardSettingsGet(DashboardType.Portal,0);
                UserCurrentSettings userSettings = GetUserSettings(view, settings);
                View.EnableFullWidth((settings == null) ? false : settings.FullWidth);
                View.InitalizeTopBar(settings, userSettings, moreTiles, View.PreloadSearchText);

               
                Boolean moreCommunities = View.LoadFromUrl && View.PreloadMoreCommunities;

                InitializeView(view, settings, userSettings, moreTiles, moreCommunities,userSettings.IdSelectedTile, userSettings.IdSelectedTag);
            }
        }

        private UserCurrentSettings GetUserSettings(DashboardViewType view, liteDashboardSettings settings)
        {
            UserCurrentSettings userSettings = View.GetCurrentCookie();
            if (userSettings == null)
            {
                userSettings = new UserCurrentSettings();
                userSettings.GroupBy = settings.Container.Default.GroupBy;
                userSettings.AfterUserLogon = settings.Container.Default.AfterUserLogon;
                userSettings.DefaultNoticeboard = settings.Container.Default.DefaultNoticeboard;
                userSettings.CombinedNoticeboard = settings.Container.Default.CombinedNoticeboard;
                userSettings.TileNoticeboard = settings.Container.Default.TileNoticeboard;
                userSettings.ListNoticeboard = settings.Container.Default.ListNoticeboard;
                userSettings.OrderBy = settings.Container.Default.OrderBy;
                userSettings.View = settings.Container.Default.View;
                userSettings.Ascending = (userSettings.OrderBy == OrderItemsBy.Name) ? true : false;
                userSettings.IdSelectedTile= View.PreloadIdTile;
                View.SaveCurrentCookie(userSettings);
            }
            if (View.LoadFromUrl){
                switch (view)
                {
                    case DashboardViewType.List:
                        userSettings.GroupBy = View.PreloadSettingsBase.GroupBy; //GroupItemsBy.None;
                        userSettings.OrderBy= View.PreloadSettingsBase.OrderBy;

                        break;
                    case DashboardViewType.Combined:
                        userSettings.GroupBy = View.PreloadSettingsBase.GroupBy;
                        if (userSettings.GroupBy== GroupItemsBy.None)
                            userSettings.GroupBy = settings.Container.Default.GroupBy;
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
                        break;
                    case DashboardViewType.Tile:
                        userSettings.GroupBy = View.PreloadSettingsBase.GroupBy;
                        if (userSettings.GroupBy == GroupItemsBy.None)
                            userSettings.GroupBy = (View.PreloadSettingsBase.GroupBy == GroupItemsBy.None) ? settings.Container.Default.GroupBy : View.PreloadSettingsBase.GroupBy;
                        break;
                }
                userSettings.Ascending = (userSettings.OrderBy == OrderItemsBy.Name) ? true : false;
                View.SaveCurrentCookie(userSettings);
            }
            return userSettings;
        }

        private void InitializeView(DashboardViewType view, liteDashboardSettings settings, UserCurrentSettings userSettings,Boolean moreTiles, Boolean moreCommunities, long idPreloadedTile , long idPreloadedTag)
        {
            if (!settings.Pages.Where(p => p.Type == view).Any())
                view = settings.Pages.Select(p=> p.Type).FirstOrDefault();
            List<dtoItemFilter<OrderItemsBy>> items = (view == DashboardViewType.Tile) ? null : GetOrderByItems(settings, userSettings); 
            switch (view)
            {
                case DashboardViewType.List:
                    View.SendUserAction(0, CurrentIdModule, settings.Id, ModuleDashboard.ActionType.ListDashboardLoad);
                    View.InitializeLayout(GetPlainLayout(settings, view), settings.Container.Default.GetNoticeboard(view));
                    View.InitializeCommunitiesList(settings.Pages.Where(p => p.Type == DashboardViewType.List).FirstOrDefault(), userSettings, items);
                    break;
                case DashboardViewType.Combined:
                    View.SendUserAction(0, CurrentIdModule, settings.Id, ModuleDashboard.ActionType.CombinedDashboardLoad);
                    View.InitializeLayout(GetPlainLayout(settings, view), settings.Container.Default.GetNoticeboard(view));
                    View.IntializeCombinedView(settings.Pages.Where(p => p.Type == DashboardViewType.Combined).FirstOrDefault(), userSettings, items, settings.Id,moreTiles,moreCommunities, idPreloadedTile);
                    break;
                case DashboardViewType.Tile:
                    View.SendUserAction(0, CurrentIdModule, settings.Id, ModuleDashboard.ActionType.TileDashboardLoad);
                    View.InitializeLayout(GetPlainLayout(settings, view), settings.Container.Default.GetNoticeboard(view));
                    View.IntializeTileView(0, settings.Container.Default.GetNoticeboard(view), settings.Pages.Where(p => p.Type == DashboardViewType.Tile).FirstOrDefault(), userSettings, settings.Id, moreTiles);
                    break;
                case DashboardViewType.Search:
                    DisplaySearchItems search = View.PreloadSearch;

                    View.SendUserAction(0, CurrentIdModule, settings.Id, (search == DisplaySearchItems.Advanced) ? ModuleDashboard.ActionType.SearchAdvancedDashboardLoad : ModuleDashboard.ActionType.SearchSimpleDashboardLoad);
                    View.InitializeLayout(GetPlainLayout(settings, view), DisplayNoticeboard.Hide);
                    break;
            }
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

        public void ChangeGroupBy(DashboardViewType view, GroupItemsBy groupBy)
        {
            liteDashboardSettings settings = Service.DashboardSettingsGet(DashboardType.Portal,0);
            UserCurrentSettings userSettings = View.GetCurrentCookie();
            if (userSettings == null)
            {
                userSettings = new UserCurrentSettings();
                userSettings.GroupBy = groupBy;
                userSettings.AfterUserLogon = settings.Container.Default.AfterUserLogon;
                userSettings.DefaultNoticeboard = settings.Container.Default.DefaultNoticeboard;
                userSettings.TileNoticeboard = settings.Container.Default.TileNoticeboard;
                userSettings.CombinedNoticeboard = settings.Container.Default.CombinedNoticeboard;
                userSettings.ListNoticeboard = settings.Container.Default.ListNoticeboard;
                userSettings.OrderBy = (groupBy == settings.Container.Default.GroupBy) ? settings.Container.Default.OrderBy : OrderItemsBy.LastAccess;
                userSettings.View = view;
                userSettings.Ascending = (userSettings.OrderBy == OrderItemsBy.Name) ? true : false;
            }
            else
            {
                if (userSettings.GroupBy != groupBy)
                {
                    switch (userSettings.GroupBy)
                    {
                        case GroupItemsBy.CommunityType:
                        case GroupItemsBy.Tile:
                            userSettings.IdSelectedTile =-1;
                            break;
                        case GroupItemsBy.Tag:
                            userSettings.IdSelectedTag = -1;
                            break;
                    }

                }

                userSettings.GroupBy = groupBy;
                userSettings.OrderBy = (groupBy == settings.Container.Default.GroupBy) ? settings.Container.Default.OrderBy : OrderItemsBy.LastAccess;
            }
            View.SaveCurrentCookie(userSettings);
            InitializeView(view, settings, userSettings, false, false, userSettings.IdSelectedTile, userSettings.IdSelectedTag);
        }
    }
}