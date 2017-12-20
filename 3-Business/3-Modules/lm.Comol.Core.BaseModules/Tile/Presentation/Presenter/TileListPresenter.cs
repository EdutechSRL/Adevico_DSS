using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.BaseModules.Tiles.Domain;

namespace lm.Comol.Core.BaseModules.Tiles.Presentation
{
    public class TileListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private lm.Comol.Core.BaseModules.Tags.Business.ServiceTags servicetag;
            private Dashboard.Business.ServiceDashboardCommunities serviceDashboardCommunities;
            private lm.Comol.Core.BaseModules.Tiles.Business.ServiceTiles service;

            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewTileList View
            {
                get { return (IViewTileList)base.View; }
            }
            private lm.Comol.Core.BaseModules.Tiles.Business.ServiceTiles Service
            {
                get
                {
                    if (service == null)
                        service = new lm.Comol.Core.BaseModules.Tiles.Business.ServiceTiles(AppContext);
                    return service;
                }
            }
            private Dashboard.Business.ServiceDashboardCommunities ServiceDashboardCommunities
            {
                get
                {
                    if (serviceDashboardCommunities == null)
                        serviceDashboardCommunities = new Dashboard.Business.ServiceDashboardCommunities(AppContext);
                    return serviceDashboardCommunities;
                }
            }
            private lm.Comol.Core.BaseModules.Tags.Business.ServiceTags ServiceTags
            {
                get
                {
                    if (servicetag == null)
                        servicetag = new lm.Comol.Core.BaseModules.Tags.Business.ServiceTags(AppContext);
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
            public TileListPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public TileListPresenter(iApplicationContext oContext, IViewTileList view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(ModuleDashboard permissions, DashboardType type, Int32 idCommunity, Boolean loadFromRecycleBin, long idTile=0, TileType preloadType = TileType.None )
        {
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || p == null)
                View.DisplaySessionTimeout();
            else
            {
               
                View.IdTilesCommunity = idCommunity;
                //dtoFilters filters = new dtoFilters() { DashboardType = type, IdCommunity = idCommunity, IdSelectedLanguage=-1, FromRecycleBin = loadFromRecycleBin };
                //if (!loadFromRecycleBin && preloadType != TileType.None)
                //    filters.IdTileType = (long)preloadType;
                Dictionary<searchFilterType,long> defaultValues = (from searchFilterType n in Enum.GetValues(typeof(searchFilterType)).AsQueryable() where n != searchFilterType.none select n).ToDictionary(t => t, t => (long)-1);
                if (!loadFromRecycleBin && preloadType != TileType.None)
                    defaultValues[searchFilterType.type] = (long)preloadType;

                List<lm.Comol.Core.DomainModel.Filters.Filter> filters = Service.GetDefaultFilters(type, p.Id, idCommunity, -1, idCommunity, loadFromRecycleBin, defaultValues).OrderBy(f=> f.DisplayOrder).ToList();
                View.LoadDefaultFilters(filters);
                InitializeView(permissions, new dtoFilters(type, idCommunity, loadFromRecycleBin, filters), idCommunity, idTile);
            }
        }

        private void InitializeView(ModuleDashboard permissions, dtoFilters filters, Int32 idCommunity, long idTile = 0)
        {
            View.IdSelectedTileLanguage = -1;
            List<lm.Comol.Core.Dashboard.Domain.dtoItemFilter<lm.Comol.Core.DomainModel.Languages.dtoLanguageItem>> languages = ServiceTags.GetLanguageSelectorItems(View.GetDefaultLanguageName(), View.GetDefaultLanguageCode());
            View.LoadLanguages(languages);
            View.FirstLoad = true;
            View.FirstLoadForLanguages = languages.ToDictionary(l => l.Value.IdLanguage, l => true);
            View.CurrentFilters = filters;
            LoadTiles(permissions, filters, idCommunity, 0, View.CurrentPageSize, idTile);
        }
        public void LoadTiles(dtoFilters filters, Int32 idCommunity, Int32 pageIndex, Int32 pageSize)
        {
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || p == null)
                View.DisplaySessionTimeout();
            else
            {
                ModuleDashboard permissions = ModuleDashboard.CreatePortalmodule(p.TypeID);
                if (filters.DashboardType == DashboardType.Community && idCommunity > 0 && (!permissions.Administration && !permissions.ManageTiles))
                    permissions = Service.GetPermission(idCommunity);
                LoadTiles(permissions, filters, idCommunity, pageIndex, pageSize);
            }
        }
        public void LoadTiles(ModuleDashboard permissions, dtoFilters filters, Int32 idCommunity, Int32 pageIndex, Int32 pageSize, long idTile = 0)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                if (filters.DashboardType== DashboardType.Portal)
                    View.LoadTilesInfo(Service.GetCommunityTypesWithoutTilesCount(), Service.GetUntranslatedTilesCount(filters));
                else
                    View.LoadTilesInfo(Service.GetUntranslatedTilesCount(filters));
                List<dtoTileItem> items = Service.GetTiles(UserContext.CurrentUserID,permissions, filters, View.GetUnknownUserName(), View.GetTranslatedTileTypes());
                if (items == null)
                    View.DisplayErrorLoadingFromDB();
                else
                {
                    Int32 itemsCount = items.Count();
                    PagerBase pager = new PagerBase();
                    pager.PageSize = pageSize;
                    pager.Count = (itemsCount > 0) ? itemsCount - 1 : 0;

                    if (pageIndex == 0 && idTile > 0 && items.Where(i=> i.Id==idTile).Any())
                    {
                        while (items.Skip(pageIndex * pageSize).Take(pageSize).Any() && !items.Skip(pageIndex * pageSize).Take(pageSize).Where(i=>i.Id == idTile).Any())
                        {
                            pageIndex++;
                        }
                        if (pageIndex > pager.LastPage - 1)
                            pageIndex = 0;
                        else if (pageIndex == pager.LastPage - 1 && !items.Skip(pageIndex * pageSize).Take(pageSize).Where(i => i.Id == idTile).Any())
                            pageIndex = 0;
                    }

                    pager.PageIndex = pageIndex;
                    View.Pager = pager;
                    String dCode = View.GetDefaultLanguageCode();
                    String dLanguage = View.GetDefaultLanguageName();

                    if (pageIndex == 0 && idTile > 0)
                    {
                        items = items.Skip(pager.PageIndex * pageSize).Take(pageSize).ToList();
                    }
                    else
                        items = items.Skip(pager.PageIndex * pageSize).Take(pageSize).ToList();
                    

                    View.AllowApplyFilters(!(View.FirstLoad && !items.Any()));
                    items.ForEach(i => i.Translations.Insert(0, new lm.Comol.Core.DomainModel.Languages.dtoLanguageItem() { IdLanguage = -1, IsMultiLanguage = true, LanguageCode = dCode, LanguageName = dLanguage }));

                    ModuleDashboard.ActionType action =  ModuleDashboard.ActionType.TilesPortalDashboardList;
                    switch(filters.DashboardType){
                        case DashboardType.AllCommunities:
                            action= ModuleDashboard.ActionType.TilesAllCommunitiesList;
                            break;
                        case DashboardType.Community:
                            action= ModuleDashboard.ActionType.TilesCommunityList;
                            break;
                    }
                    View.LoadTiles(items, filters.IdSelectedLanguage);
                    View.SendUserAction((filters.DashboardType == DashboardType.Community) ? idCommunity : 0, CurrentIdModule, action);
                    View.FirstLoad = false;
                    View.FirstLoadForLanguages[filters.IdSelectedLanguage] = false;
                }
            }
        }
        public void ApplyFilters(dtoFilters filters, Int32 idCommunity, Int32 pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                LoadTiles(filters, idCommunity, 0, pageSize);
            }
        }
        public void SetStatus(long idTile, AvailableStatus status, dtoFilters filters, Int32 idCommunity, Int32 pageIndex, Int32 pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleDashboard.ActionType action = (status == AvailableStatus.Available) ? ModuleDashboard.ActionType.TileEnable : ModuleDashboard.ActionType.TileDisable;
                Tile item = Service.TileSetStatus(idTile, status);
                if (item == null || item.Status != status)
                    action = (status == AvailableStatus.Available) ? ModuleDashboard.ActionType.TileUnableToEnable : ModuleDashboard.ActionType.TileUnableToDisable;
                View.DisplayMessage(action);
                View.SendUserAction((filters.DashboardType == DashboardType.Community) ? idCommunity : 0, CurrentIdModule, idTile, action);
                LoadTiles(filters, idCommunity, 0, pageSize);
            }
        }
        public void VirtualDelete(long idTile, Boolean delete, dtoFilters filters, Int32 idCommunity, Int32 pageIndex, Int32 pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleDashboard.ActionType action = (delete) ? ModuleDashboard.ActionType.TileVirtualDelete : ModuleDashboard.ActionType.TileVirtualUndelete;
                Tile item = Service.TileVirtualDelete(idTile, delete);
                if (item == null)
                    action = (delete) ? ModuleDashboard.ActionType.TileUnableToVirtualDelete : ModuleDashboard.ActionType.TileUnableToUndelete;
                View.DisplayMessage(action);
                View.SendUserAction((filters.DashboardType == DashboardType.Community) ? idCommunity : 0, CurrentIdModule, idTile, action);
                LoadTiles(filters, idCommunity, 0, pageSize);
            }
        }
        public void GenerateCommunityTypeTiles(dtoFilters filters,  Int32 idCommunity, Int32 pageIndex, Int32 pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleDashboard.ActionType action = ModuleDashboard.ActionType.TileAutoGenerateForCommunityTypes;
                Int32 count = Service.GetCommunityTypesWithoutTilesCount();
                if (count == 0)
                    action = ModuleDashboard.ActionType.TileAlreadyGeneratedForCommunityTypes;
                else
                {
                    List<Tile> tiles = Service.GenerateCommunityTypesTile();
                    if (tiles == null || tiles.Count != count)
                        action = ModuleDashboard.ActionType.TileUnableAutoGenerateForCommunityTypes;
                    else
                        View.HideCommunityTypesTileAutoGenerate(Service.GetCommunityTypesWithoutTilesCount() ==0);
                }
                View.DisplayMessage(action);
                View.SendUserAction((filters.DashboardType == DashboardType.Community) ? idCommunity : 0, CurrentIdModule,  action);


                Dictionary<searchFilterType, long> defaultValues = (from searchFilterType n in Enum.GetValues(typeof(searchFilterType)).AsQueryable() where n != searchFilterType.none select n).ToDictionary(t => t, t => (long)-1);
                if (filters.IdTileType != (long)TileType.CommunityType && filters.IdTileType != -1)
                {
                    filters.IdTileType = (long)TileType.CommunityType;
                    filters.StartWith = "";
                    filters.IdModifiedBy = UserContext.CurrentUserID;
                    filters.Status = AvailableStatus.Available;
                }
                defaultValues[searchFilterType.type] = filters.IdTileType;
                defaultValues[searchFilterType.status] = (long)filters.Status;
                defaultValues[searchFilterType.modifiedby] = (long)filters.IdModifiedBy;

                long letter = -1;
                switch(filters.StartWith){
                    case "#":
                        letter = -9;
                        break;
                    case "":
                        break;
                    default:
                        letter = (long)filters.StartWith[0];
                        break;
                }
                defaultValues[searchFilterType.letters] = letter;
                List<lm.Comol.Core.DomainModel.Filters.Filter> nfilters = Service.GetDefaultFilters(DashboardType.Portal,UserContext.CurrentUserID, idCommunity, -1, idCommunity, false, defaultValues).OrderBy(f => f.DisplayOrder).ToList();
                View.LoadDefaultFilters(nfilters);

                LoadTiles(filters, idCommunity, 0, pageSize);
            }
        }
     
    }
}