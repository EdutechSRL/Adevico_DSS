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
    public class TileEditPagePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private lm.Comol.Core.BaseModules.Tags.Business.ServiceTags servicetag;
            private ServiceTiles service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewEditPage View
            {
                get { return (IViewEditPage)base.View; }
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
            public TileEditPagePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public TileEditPagePresenter(iApplicationContext oContext, IViewEditPage view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Boolean add,long idTile,DashboardType dashboardType, Int32 idContainerCommunity)
        {
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || p == null)
                View.DisplaySessionTimeout();
            else
            {
                Int32 idTileCommunity = (dashboardType == DashboardType.Community) ? idContainerCommunity : 0;
                ModuleDashboard permissions = ModuleDashboard.CreatePortalmodule(p.TypeID);
                List<TileType> availableTypes = null;
                dtoEditTile tile = null;
                View.IdTile = idTile;
                if (add)
                {
                    availableTypes = Service.GetTileTypesAvailable(dashboardType);
                    tile = new dtoEditTile();
                    tile.Status = AvailableStatus.Draft;
                    tile.Type= availableTypes.FirstOrDefault();
                }
                else
                {
                    if (View.PreloadFromAdd)
                        View.DisplayTileAdded();
                    tile = Service.GetEditTile(idTile);
                    idTileCommunity = (tile == null) ? ((dashboardType == DashboardType.Community) ? idContainerCommunity : 0) : tile.IdCommunity;
                   
                }
                View.IdTileCommunity = idTileCommunity;
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
                if ((tile == null && (idTile > 0 || !add)) || (tile != null && tile.Deleted != BaseStatusDeleted.None && tile.Deleted != BaseStatusDeleted.Manual))
                {
                    View.DisplayUnknownTile();
                    if (!permissions.ManageTiles)
                        View.DisplayNoPermission(idContainerCommunity, CurrentIdModule);
                    else
                        View.SetBackUrl(RootObject.TileList(dashboardType, false, idContainerCommunity, idTile, View.PreloadIdDashboard, View.PreloadStep ));
                }
                else
                {

                    if (permissions.ManageTiles)
                    {
                        if (tile!=null && tile.Id > 0)
                        {
                            View.AllowVirtualUndelete = (tile.Deleted == BaseStatusDeleted.Manual);
                            View.AllowVirtualDelete = (tile.Deleted == BaseStatusDeleted.None);
                            View.AllowDisable = (tile.Deleted == BaseStatusDeleted.None && tile.Status == AvailableStatus.Available);
                            View.AllowEnable = (tile.Deleted == BaseStatusDeleted.None && tile.Status != AvailableStatus.Available);
                            View.SendUserAction(idContainerCommunity, Service.ServiceModuleID(), tile.Id, ModuleDashboard.ActionType.TileStartEditing);
                        }
                        else
                            View.SendUserAction(idContainerCommunity, Service.ServiceModuleID(), ModuleDashboard.ActionType.TileStartAdding);

                        View.AllowSave = (tile.Deleted == BaseStatusDeleted.None);
                        View.SetBackUrl(RootObject.TileList(dashboardType, false, idContainerCommunity, idTile, View.PreloadIdDashboard, View.PreloadStep));
                        switch (tile.Type)
                        {
                            case TileType.CommunityTag:
                                View.InitalizeEditor(tile, idTile, idContainerCommunity, idTileCommunity, ServiceTags.GetTagTranslation(tile.IdTags.FirstOrDefault(), UserContext.Language.Id).Title);
                                break;
                            case TileType.CombinedTags:
                                Language dLanguage = CurrentManager.GetDefaultLanguage();
                                List<lm.Comol.Core.Tag.Domain.liteTagItem> items = ServiceTags.CacheGetTags(Tag.Domain.TagType.Community, true).Where(t => t.Status == lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available || tile.IdTags.Contains(t.Id)).ToList();
                                List<lm.Comol.Core.DomainModel.TranslatedItem<long>> tags = items.Select(t => new lm.Comol.Core.DomainModel.TranslatedItem<long>() { Id = t.Id, Translation = t.GetTitle(UserContext.Language.Id, dLanguage.Id) }).ToList();
                                View.InitalizeEditor(tile, idTile, idContainerCommunity, idTileCommunity, tags, availableTypes);
                                break;
                            case TileType.CommunityType:
                                View.InitalizeEditor(tile, idTile, idContainerCommunity, idTileCommunity, Service.GetCommunityTypeName(tile.IdCommunityTypes.FirstOrDefault(), UserContext.Language.Id));
                                break;
                            default:
                                View.InitalizeEditor(tile, idTile, idContainerCommunity, idTileCommunity, availableTypes);
                                break;
                        }

                    }
                    else
                        View.DisplayNoPermission(idContainerCommunity, CurrentIdModule);
                }
            }
        }
    }
}