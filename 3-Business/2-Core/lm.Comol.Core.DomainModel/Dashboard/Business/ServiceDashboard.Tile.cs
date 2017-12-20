using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

using NHibernate.Linq;
using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.Dashboard.Business
{
    public partial class ServiceDashboard : CoreServices
    {
        #region "Loading Tiles"

            /// <summary>
            ///  Get all available tiles for a dashboard
            /// </summary>
            /// <param name="idDashboard"></param>
            /// <param name="useCache"></param>
            /// <returns></returns>
            private List<liteDashboardTileAssignment> CacheGetDashBoardTiles(long idDashboard,  Boolean useCache = true)
            {
                List<liteDashboardTileAssignment> tiles = null;
                tiles = (useCache) ? lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<List<liteDashboardTileAssignment>>(CacheKeys.DashboardTiles(idDashboard)) : null;
                if (tiles == null || !tiles.Any())
                {
                    tiles = (from t in Manager.GetIQ<liteDashboardTileAssignment>()
                             where t.Deleted == BaseStatusDeleted.None && t.IdDashboard== idDashboard && t.Status== AvailableStatus.Available 
                             select t).ToList().Where(t=> t.Tile !=null && t.Tile.Status== AvailableStatus.Available).OrderBy(t=> t.Tile.Type).ThenBy(t=> t.DisplayOrder).ToList();
                    Manager.DetachList(tiles);
                    if (useCache)
                        CacheHelper.AddToCache<List<liteDashboardTileAssignment>>(CacheKeys.DashboardTiles(idDashboard), tiles, CacheExpiration.Day);
                }
                return tiles;
            }

            /// <summary>
            ///     Get all available tiles for user and dashboard
            /// </summary>
            /// <param name="idUser"></param>
            /// <param name="idDashboard"></param>
            /// <param name="type"></param>
            /// <returns></returns>
            public List<dtoLiteTile> TilesGetForUser(Int32 idUser, long idDashboard, TileType type, Boolean useCache = true)
            {
                List<dtoLiteTile> results = null;
                try
                {
                    results = (useCache) ? lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<List<dtoLiteTile>>(CacheKeys.DashboardUserTiles(idDashboard, idUser, type)) : null;
                    if (results == null || !results.Any())
                    {
                        List<liteDashboardTileAssignment> tAssignments = CacheGetDashBoardTiles(idDashboard);
                        List<dtoLiteTile> aTiles = null;
                        switch (type)
                        {
                            case TileType.CombinedTags:
                                aTiles = tAssignments.Where(t => t.Tile != null && (t.Tile.Type == TileType.CommunityTag || t.Tile.Type == TileType.CombinedTags || t.Tile.Type == TileType.DashboardUserDefined)).Select(t => new dtoLiteTile() { DisplayOrder = t.DisplayOrder, Tile = t.Tile }).ToList();
                                break;
                            case TileType.CommunityTag:
                                aTiles = tAssignments.Where(t => t.Tile != null && t.Tile.Type == TileType.CommunityTag).Select(t => new dtoLiteTile() { DisplayOrder = t.DisplayOrder, Tile = t.Tile }).ToList();
                                break;
                            case TileType.Module:
                                aTiles = tAssignments.Where(t => t.Tile != null && (t.Tile.Type == TileType.Module || t.Tile.Type == TileType.UserDefined)).Select(t => new dtoLiteTile() { DisplayOrder = t.DisplayOrder, Tile = t.Tile }).ToList();
                                break;
                            default:
                                aTiles = tAssignments.Where(t => t.Tile != null && t.Tile.Type == type).Select(t => new dtoLiteTile() { DisplayOrder = t.DisplayOrder, Tile = t.Tile }).ToList();
                                break;
                        }
                        if (aTiles != null)
                        {
                            
                            switch (type)
                            {
                                case TileType.Module:
                                    break;
                                default:
                                    List<lm.Comol.Core.Tag.Domain.dtoCommunityTags> tLinks = null;
                                    switch (type)
                                    {

                                        case TileType.CommunityType:
                                            List<Int32> idTypes = (from s in Manager.GetIQ<liteSubscriptionInfo>() where s.IdPerson == idUser && s.IdRole > -1 select s.Community.IdTypeOfCommunity).Distinct().ToList();

                                            results = aTiles.Where(a => a.HasAnyCommunityType(idTypes)).ToList();

                                            break;
                                        case TileType.CommunityTag:
                                        case TileType.CombinedTags:
                                            List<Int32> idCommunities = (from s in Manager.GetIQ<liteSubscriptionInfo>() where s.IdPerson == idUser && s.IdRole > -1 select s.IdCommunity).ToList();
                                            tLinks= Service.CacheGetUserCommunitiesAssociation(idUser, idCommunities, (type == TileType.Module) ? Tag.Domain.TagType.Module : Tag.Domain.TagType.Community, true);
                                            if (idCommunities.Except(tLinks.Select(t => t.IdCommunity).ToList()).Any())
                                                tLinks = Service.CacheGetUserCommunitiesAssociation(idUser, idCommunities, (type == TileType.Module) ? Tag.Domain.TagType.Module : Tag.Domain.TagType.Community, false);
                                            results = aTiles.Where(a => a.Tile.Type == TileType.CommunityTag && a.Tile.GetAvailableIdTags().Count == 1 && tLinks.Where(l => l.HasTag(a.Tile.GetAvailableIdTags().FirstOrDefault())).Any()).ToList();
                                            if (type == TileType.CombinedTags)
                                            {
                                                results.AddRange(aTiles.Where(a => a.Tile.Type == TileType.CombinedTags && a.Tile.GetAvailableIdTags().Count > 1 && tLinks.Where(l => l.HasTags(a.Tile.GetAvailableIdTags())).Any()).ToList());
                                                results.AddRange(aTiles.Where(a => a.Tile.Type == TileType.DashboardUserDefined && a.Tile.NavigateUrl != "").ToList());
                                            }
                                            break;
                                    }
                                    break;
                            }
                            results = results.OrderBy(i => i.DisplayOrder).ToList();
                            if (useCache)
                                CacheHelper.AddToCache<List<dtoLiteTile>>(CacheKeys.DashboardUserTiles(idDashboard, idUser, type), results, CacheExpiration._12hours);
                        }
                    }
                  
                }
                catch (Exception ex)
                {

                }
                return results;
            }

            public List<dtoLiteTile> TilesGetForDashboard(long idDashboard, TileType type, Boolean useCache = true)
            {
                List<dtoLiteTile> results = null;
                try
                {
                    List<liteDashboardTileAssignment> tAssignments = CacheGetDashBoardTiles(idDashboard);
                    List<dtoLiteTile> aTiles = null;
                    switch (type)
                    {
                        case TileType.CombinedTags:
                            aTiles = tAssignments.Where(t => t.Tile != null && (t.Tile.Type == TileType.CommunityTag || t.Tile.Type == TileType.CombinedTags || t.Tile.Type == TileType.DashboardUserDefined)).Select(t => new dtoLiteTile() { DisplayOrder = t.DisplayOrder, Tile = t.Tile }).ToList();
                            break;
                        case TileType.CommunityTag:
                            aTiles = tAssignments.Where(t => t.Tile != null && t.Tile.Type == TileType.CommunityTag).Select(t => new dtoLiteTile() { DisplayOrder = t.DisplayOrder, Tile = t.Tile }).ToList();
                            break;
                        case TileType.Module:
                            aTiles = tAssignments.Where(t => t.Tile != null && (t.Tile.Type == TileType.Module || t.Tile.Type == TileType.UserDefined)).Select(t => new dtoLiteTile() { DisplayOrder = t.DisplayOrder, Tile = t.Tile }).ToList();
                            break;
                        default:
                            aTiles = tAssignments.Where(t => t.Tile != null && t.Tile.Type == type).Select(t => new dtoLiteTile() { DisplayOrder = t.DisplayOrder, Tile = t.Tile }).ToList();
                            break;
                    }
                    if (aTiles != null)
                    {

                        switch (type)
                        {
                            case TileType.Module:
                                break;
                            default:
                              
                                break;
                        }
                    }
                    results = aTiles.OrderBy(i => i.DisplayOrder).ToList();
                }
                catch (Exception ex)
                {

                }
                return results;
            }

        

            public TileLayout GetTileLayout(DisplayNoticeboard noticeboard, Int32 count, TileLayout tLayout, Boolean autoUpdateLayout)
            {
                switch (noticeboard)
                {
                    case DisplayNoticeboard.OnLeft:
                    case DisplayNoticeboard.OnRight:
                        count++;
                        break;
                }
                return GetTileLayout(count, tLayout, autoUpdateLayout);
            }
            public TileLayout GetTileLayout( Int32 count, TileLayout tLayout, Boolean autoUpdateLayout)
            {
                if (autoUpdateLayout && count < (12 / (int)tLayout))
                {
                    switch (count)
                    {
                        case 0:
                            return TileLayout.grid_6;
                        case 1:
                        case 2:
                            return TileLayout.grid_6;
                        case 3:
                            return TileLayout.grid_4;
                        case 4:
                            return TileLayout.grid_3;
                        case 5:
                            return TileLayout.grid_2;
                        default:
                            return TileLayout.grid_1;
                    }
                }
                else
                    return tLayout;
            }
        #endregion

          #region "Manage tiles"
            public Tile TileSetStatus(long idTile, lm.Comol.Core.Dashboard.Domain.AvailableStatus status)
            {
                return TileSetStatus(Manager.Get<Tile>(idTile), status);
            }
            public Tile TileSetStatus(Tile tile, lm.Comol.Core.Dashboard.Domain.AvailableStatus status)
            {
                Tile item = null;
                try
                {
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    if (tile!= null && person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
                    {
                        Manager.BeginTransaction();
                        tile.Status = status;
                        tile.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        Manager.Commit();
                        item = tile;
                        CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.AllUserDashboard);
                        CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.AllDashboardTiles);
                    }
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
                return item;
            }
            public Tile TileVirtualDelete(long idTile, Boolean delete)
            {
                return TileVirtualDelete(Manager.Get<Tile>(idTile), (delete) ? BaseStatusDeleted.Manual : BaseStatusDeleted.None);
            }
            public Tile TileVirtualDelete(long idTile, BaseStatusDeleted deleted){
                return TileVirtualDelete(Manager.Get<Tile>(idTile), deleted);
            }
            public Tile TileVirtualDelete(Tile tile, BaseStatusDeleted deleted)
            {
                Boolean isInTransaction = Manager.IsInTransaction();
                Tile result = null;
                try
                {
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    if (tile != null && person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
                    {
                        Boolean isCascade = (tile.Deleted == BaseStatusDeleted.Cascade);
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        tile.Deleted=deleted;
                        tile.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        Manager.SaveOrUpdate(tile);
                        List<DashboardTileAssignment> assignments = (from a in Manager.GetIQ<DashboardTileAssignment>()
                                                                     where a.Tile != null && a.Tile.Id == tile.Id
                                                                     select a).ToList();
                        if (isCascade && deleted== BaseStatusDeleted.None)
                        {
                            foreach (DashboardTileAssignment assignment in assignments.Where(a=> a.Deleted== BaseStatusDeleted.Automatic))
                            {
                                assignment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                assignment.Deleted = BaseStatusDeleted.None;
                            }
                        }
                        else if (!isCascade && deleted != BaseStatusDeleted.Cascade )
                        {
                            foreach (DashboardTileAssignment assignment in assignments)
                            {
                                assignment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                assignment.Deleted = (deleted == BaseStatusDeleted.Manual) ? (assignment.Deleted | BaseStatusDeleted.Cascade) : ((BaseStatusDeleted)((int)assignment.Deleted - (int)BaseStatusDeleted.Cascade));
                            }
                        }
                        else if (!isCascade && deleted == BaseStatusDeleted.Cascade)
                        {
                            foreach (DashboardTileAssignment assignment in assignments.Where(a=> a.Deleted== BaseStatusDeleted.None))
                            {
                                assignment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                assignment.Deleted = BaseStatusDeleted.Automatic;
                            }
                        }
                        if (!isInTransaction)
                            Manager.Commit();
                        result = tile;

                        CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.AllDashboardTiles);
                        CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.AllUserDashboard);
                    }
                }
                catch (Exception ex)
                {
                    result = null;
                    if (!isInTransaction)
                        Manager.RollBack();
                }
                return result;
            }

            public liteTile GetTileForCommunityType(Int32 idCommunityType)
            {
                liteTile tile = null;
                try {
                    tile = (from t in Manager.GetIQ<liteTile>()
                            where t.Deleted == BaseStatusDeleted.None && t.Type == TileType.CommunityType
                                && t.CommunityTypes != null 
                            select t).ToList().Where(t=> t.CommunityTypes.Contains(idCommunityType)).Skip(0).Take(1).ToList().FirstOrDefault();
                }
                catch (Exception ex)
                {


                }
                return tile;
            }
            public liteTile GetTile(long idTile)
            {
                liteTile tile = null;
                try
                {
                    tile = Manager.Get<liteTile>(idTile);
                }
                catch (Exception ex)
                {


                }
                return (tile!=null && tile.Id != idTile) ? null :tile;
            }
            public long GetTileDisplayOrder(long idTile,DashboardSettings dashboard, TileType type){
                long result = -1;
                long tDisplayOrder = (idTile<=0) ? 0 : (from t in Manager.GetIQ<liteDashboardTileAssignment>()
                                                    where t.Deleted== BaseStatusDeleted.None && t.IdDashboard == dashboard.Id  && idTile>0 && t.Tile != null  &&  t.Tile.Id == idTile select t.DisplayOrder).Skip(0).Take(1).ToList().FirstOrDefault();
                if (tDisplayOrder == 0)
                {
                    var query = (from t in Manager.GetIQ<liteDashboardTileAssignment>()
                                where t.Deleted == BaseStatusDeleted.None && t.IdDashboard == dashboard.Id && t.Tile != null
                                select t);
                    switch(dashboard.Type){
                        case DashboardType.AllCommunities:
                        case DashboardType.Community:
                            break;
                        case DashboardType.Portal:
                            switch (type)
                            {
                                case TileType.CommunityType:
                                    query = query.Where(t => t.Tile.Type == type);
                                    break;
                                default:
                                    query = query.Where(t => t.Tile.Type != TileType.CommunityType);
                                    break;
                            }
                            break;
                    }
                    return query.Select(t=>t.DisplayOrder).Max();
                }
                else
                    result = tDisplayOrder;
                return result;
            }
     
        #endregion
    }
}