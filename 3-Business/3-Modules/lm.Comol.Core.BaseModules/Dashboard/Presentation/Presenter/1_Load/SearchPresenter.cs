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
    public class SearchPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceDashboard service;
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities serviceCommunities;
            private lm.Comol.Core.Tag.Business.ServiceTags servicetag;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewSearchDashboard View
            {
                get { return (IViewSearchDashboard)base.View; }
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
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities ServiceCommunities
            {
                get
                {
                    if (serviceCommunities == null)
                        serviceCommunities = new lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities(AppContext);
                    return serviceCommunities;
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
            public SearchPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public SearchPresenter(iApplicationContext oContext, IViewSearchDashboard view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(DisplaySearchItems search, String searchText, Boolean forSubscription = false)
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

                liteDashboardSettings settings = Service.DashboardSettingsGet(DashboardType.Portal,0);
                UserCurrentSettings userSettings = GetUserSettings(settings);
                InitializeView(settings, userSettings, search, searchText, forSubscription);
            }
        }

        private UserCurrentSettings GetUserSettings(liteDashboardSettings settings)
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
                View.SaveCurrentCookie(userSettings);
            }
            return userSettings;
        }
        private void InitializeView(liteDashboardSettings settings, UserCurrentSettings userSettings, DisplaySearchItems search, String searchText, Boolean forSubscription = false)
        {
            List<dtoItemFilter<OrderItemsBy>> items = GetOrderByItems(settings, userSettings);
            Int32 idType = View.PreloadIdCommunityType;
            List<long> idTags = new List<long>();
            long idTile = View.PreloadIdTile;
            View.EnableFullWidth((settings == null) ? false : settings.FullWidth);
            View.InitalizeTopBar(settings, userSettings, searchText);
            liteTile tile = null;
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
            Int32 idUserLanguage = ((UserContext != null && UserContext.Language != null) ? UserContext.Language.Id : -2);
            Int32 idDefaultLanguage = CurrentManager.GetDefaultIdLanguage();

            List<lm.Comol.Core.DomainModel.Filters.Filter> fToLoad = ServiceCommunities.GetDefaultFilters(UserContext.CurrentUserID, searchText, idType, idTile, idTags,null, CommunityManagement.CommunityAvailability.Subscribed, -1).OrderBy(f => f.DisplayOrder).ToList();
            View.LoadDefaultFilters(fToLoad);
            if (fToLoad != null && fToLoad.Any())
            {
                lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters = new lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters(fToLoad, CommunityManagement.CommunityAvailability.Subscribed, idType,idTile );
                filters.IdcommunityType = idType;
                if(!String.IsNullOrEmpty(searchText))
                    filters.SearchBy = CommunityManagement.SearchCommunitiesBy.Contains;

                if (filters.IdcommunityType > -1)
                    tile = Service.GetTileForCommunityType(filters.IdcommunityType);
                else if (idTags.Any() && idTile > 0)
                    filters.IdTags = idTags;
                View.InitializeCommunitiesList(settings.Pages.Where(p => p.Type == DashboardViewType.Search).FirstOrDefault(), filters, items, tile, idUserLanguage, idDefaultLanguage);
            }
            else
            {
                if (idType>-1)
                    tile = Service.GetTileForCommunityType(idType);
                View.InitializeCommunitiesList(settings.Pages.Where(p => p.Type == DashboardViewType.Search).FirstOrDefault(), items, tile, idUserLanguage, idDefaultLanguage);
            }

            View.SendUserAction(0, CurrentIdModule, settings.Id, (search == DisplaySearchItems.Advanced) ? ModuleDashboard.ActionType.SearchAdvancedDashboardLoad : ModuleDashboard.ActionType.SearchSimpleDashboardLoad);
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

        public void ApplyFilters(lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                liteDashboardSettings settings = Service.DashboardSettingsGet(DashboardType.Portal, 0);
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
                View.ApplyFilters(page, filters, tile, ((UserContext !=null && UserContext.Language !=null) ? UserContext.Language.Id : -2) , CurrentManager.GetDefaultIdLanguage());
            }
        }
    }
}