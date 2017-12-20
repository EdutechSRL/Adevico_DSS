using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.BaseModules.Tiles.Business;
using lm.Comol.Core.BaseModules.Tiles.Domain;

namespace lm.Comol.Core.BaseModules.Tiles.Presentation
{
    public class ViewTilePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private lm.Comol.Core.BaseModules.Tags.Business.ServiceTags servicetag;
            private ServiceTiles service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewTile View
            {
                get { return (IViewTile)base.View; }
            }
            private ServiceTiles Service
            {
                get
                {
                    if (service == null)
                        service = new ServiceTiles(AppContext);
                    return service;
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
            public ViewTilePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ViewTilePresenter(iApplicationContext oContext, IViewTile view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion
        public void InitView(long idTile, DashboardType dashboardType, Int32 idContainerCommunity)
        {
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || p == null)
                View.DisplaySessionTimeout();
            else
            {
                Int32 idTileCommunity = (dashboardType == DashboardType.Community) ? idContainerCommunity : 0;
                ModuleDashboard permissions = ModuleDashboard.CreatePortalmodule(p.TypeID);
                dtoEditTile tile = Service.GetEditTile(idTile);
                idTileCommunity = (tile == null) ? ((dashboardType == DashboardType.Community) ? idContainerCommunity : 0) : tile.IdCommunity;
                if (dashboardType == DashboardType.Community)
                {
                    View.IdContainerCommunity = idContainerCommunity;
                    if (idTileCommunity < 0)
                        idTileCommunity = UserContext.CurrentCommunityID;
                    if (idTileCommunity > 0 && (!permissions.Administration && !permissions.ManageTiles))
                        permissions = Service.GetPermission(idTileCommunity);
                }
                else
                {
                    idContainerCommunity = 0;
                    View.IdContainerCommunity = 0;
                }
                if ((tile == null && (idTile > 0)) || (tile != null && tile.Deleted != BaseStatusDeleted.None && tile.Deleted != BaseStatusDeleted.Manual))
                    View.DisplayUnknownTile();
                if (permissions.ManageTiles)
                {
                    View.SendUserAction(idContainerCommunity, Service.ServiceModuleID(), ModuleDashboard.ActionType.TileView);
                    View.SetBackUrl(RootObject.TileList(dashboardType, false, idContainerCommunity, idTile, View.PreloadIdDashboard, View.PreloadStep));


                    List<lm.Comol.Core.DomainModel.Languages.dtoLanguageItem> languages = CurrentManager.GetAllLanguages().OrderByDescending(l => l.isDefault).ThenBy(l => l.Name).Select(l =>
                        new lm.Comol.Core.DomainModel.Languages.dtoLanguageItem() { IdLanguage = l.Id, LanguageCode = l.Code, LanguageName = l.Name }).ToList();

                    languages.Insert(0, new lm.Comol.Core.DomainModel.Languages.dtoLanguageItem() { IdLanguage = 0, LanguageCode = View.GetDefaultLanguageCode(), LanguageName = View.GetDefaultLanguageName(), IsMultiLanguage = true });

                    switch (tile.Type)
                    {
                        case TileType.CommunityTag:
                            View.LoadTile(tile, ServiceTags.GetTagTranslation(tile.IdTags.FirstOrDefault(), UserContext.Language.Id).Title, languages, languages.Select(l => new dtoTileFullTranslation(l, tile)).ToList());
                            break;
                        case TileType.CombinedTags:
                            Language dLanguage = CurrentManager.GetDefaultLanguage();
                            List<lm.Comol.Core.Tag.Domain.liteTagItem> items = ServiceTags.CacheGetTags(Tag.Domain.TagType.Community, true).Where(t => t.Status == lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available || tile.IdTags.Contains(t.Id)).ToList();
                            List<lm.Comol.Core.DomainModel.TranslatedItem<long>> tags = items.Select(t => new lm.Comol.Core.DomainModel.TranslatedItem<long>() { Id = t.Id, Translation = t.GetTitle(UserContext.Language.Id, dLanguage.Id) }).ToList();
                            View.LoadTile(tile, tags, languages, languages.Select(l => new dtoTileFullTranslation(l, tile)).ToList());
                            break;
                        case TileType.CommunityType:
                            View.LoadTile(tile, Service.GetCommunityTypeName(tile.IdCommunityTypes.FirstOrDefault(), UserContext.Language.Id), languages, languages.Select(l => new dtoTileFullTranslation(l, tile)).ToList());
                            break;
                        default:
                            View.LoadTile(tile, languages, languages.Select(l => new dtoTileFullTranslation(l, tile)).ToList());
                            break;
                    }
                    View.SendUserAction(idContainerCommunity, Service.ServiceModuleID(), ModuleDashboard.ActionType.TileView);
                }
                else
                    View.DisplayNoPermission(idContainerCommunity, CurrentIdModule);
            }
        }
    }
}