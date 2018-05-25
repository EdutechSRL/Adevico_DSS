using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using lm.Comol.Core.FileRepository.Domain;
using System.Linq.Expressions;

namespace lm.Comol.Core.FileRepository.Business
{
    public partial class ServiceFileRepository 
    {
        #region "Statistics"
            #region "Downloads Info"
                public Boolean StatisticsAddDownload(Int32 idPerson, RepositoryIdentifier rIdentifier, long idItem, Guid uniqueIdItem, long idVersion, Guid uniqueIdVersion, ItemType type, Int32 idCommunity)
                {
                    Boolean result = false;
                    try{
                        DownloadStatistics stat = new DownloadStatistics();
                        stat.CreatedIPaddress= UC.IpAddress;
                        stat.CreatedOn= DateTime.Now;
                        stat.CreatedProxyIPaddress= UC.ProxyIpAddress;
                        stat.IdCommunity= idCommunity;
                        stat.IdItem= idItem;
                        stat.IdPerson= idPerson;
                        stat.IdVersion= idVersion;
                        stat.ItemType= type;
                        stat.UniqueIdItem=uniqueIdItem;
                        stat.UniqueIdVersion=uniqueIdVersion;
                        stat.RepositoryIdCommunity = rIdentifier.IdCommunity;
                        stat.RepositoryIdPerson = rIdentifier.IdPerson;
                        stat.RepositoryType = rIdentifier.Type;
                        Manager.SaveOrUpdate(stat);
                        result = true;
                        Boolean isIntransaction = Manager.IsInTransaction();
                        if (!isIntransaction)
                            Manager.BeginTransaction();
                        liteRepositoryItem item = Manager.Get<liteRepositoryItem>(idItem);
                        if (item != null)
                        {
                            item.Downloaded++;
                            Manager.SaveOrUpdate(item);
                            liteRepositoryItemVersion version = Manager.Get<liteRepositoryItemVersion>(idVersion);
                            if (version != null)
                            {
                                version.Downloaded++;
                                Manager.SaveOrUpdate(version);
                            }
                        }
                        if (!isIntransaction)
                            Manager.Commit();
                    }
                    catch(Exception ex)
                    {

                    }
                    return result;
                }
                public Dictionary<long, long> DownloadStatisticsGetForRepository(RepositoryIdentifier identifier)
                {
                    return DownloadStatisticsGetForRepository(identifier.Type, identifier.IdCommunity, identifier.IdPerson);
                }
                public Dictionary<long, long> DownloadStatisticsGetForRepository(RepositoryType type, Int32 idCommunity, Int32 idUserRepository)
                {
                    return DownloadStatisticsGetQuery(type, idCommunity, idUserRepository).Select(i => i.IdItem).ToList().GroupBy(i => i).ToDictionary(i => i.Key, i => (long)i.Count());
                }
                public Dictionary<long, long> DownloadStatisticsGetForRepository(Int32 idCurrentUser, RepositoryType type, Int32 idCommunity, Int32 idUserRepository)
                {
                    return DownloadStatisticsGetQuery(type, idCommunity, idUserRepository, idCurrentUser).Select(i => i.IdItem).ToList().GroupBy(i => i).ToDictionary(i => i.Key, i => (long)i.Count());
                }
                public long DownloadsCountForItem(long idItem)
                {
                    return DownloadStatisticsGetQuery(s => s.IdItem == idItem).Count();
                }
                public long DownloadsCountForItem(long idItem,Int32 idCurrentUser)
                {
                    return DownloadStatisticsGetQuery(s => s.IdItem == idItem && s.IdPerson == idCurrentUser).Count();
                }
                public Dictionary<long, long> DownloadStatisticsGetFull(List<long> idItems, Int32 idCurrentUser)
                {
                    if (idItems.Count <= maxItemsForQuery)
                        return DownloadStatisticsGetQuery(idItems, idCurrentUser).Select(i=>i.IdItem).ToList().GroupBy(i => i).ToDictionary(i => i.Key, i => (long)i.Count());
                    else
                        return DownloadStatisticsGet(idItems, idCurrentUser).Select(i => i.IdItem).ToList().GroupBy(i => i).ToDictionary(i => i.Key, i => (long)i.Count());
                }
                public Dictionary<long, Dictionary<long,long>> GetDownloads(List<long> idItems, Int32 idCurrentUser)
                {
                    if (idItems.Count <= maxItemsForQuery)
                        return DownloadStatisticsGetQuery(idItems, idCurrentUser).Select(i => new { IdItem = i.IdItem, IdVersion = i.IdVersion }).ToList().GroupBy(i => i.IdItem).ToDictionary(i => i.Key, i => i.GroupBy(v => v.IdVersion).ToDictionary(v => v.Key, v => (long)v.Count()));
                    else
                        return DownloadStatisticsGet(idItems, idCurrentUser).GroupBy(i => i.IdItem).ToDictionary(i => i.Key, i => i.GroupBy(v => v.IdVersion).ToDictionary(v => v.Key, v => (long)v.Count()));
                }
                public List<DownloadStatistics> DownloadStatisticsGet(List<long> idItems, Int32 idCurrentUser)
                {
                    if (idItems.Count <= maxItemsForQuery)
                        return DownloadStatisticsGetQuery(idItems, idCurrentUser).ToList();
                    else
                    {
                        List<DownloadStatistics> results = new List<DownloadStatistics>();
                        Int32 pageIndex = 0;
                        List<long> idPagedItems = idItems.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                        while (idPagedItems.Any())
                        {
                            results.AddRange(DownloadStatisticsGetQuery(idItems, idCurrentUser).ToList());
                            pageIndex++;
                            idPagedItems = idItems.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                        }
                        return results;
                    }
                }
                public List<DownloadStatistics> DownloadStatisticsGetByVersion(List<long> idVersions, Int32 idCurrentUser)
                {
                    if (idVersions.Count <= maxItemsForQuery)
                        return DownloadStatisticsGetQueryByVersion(idVersions, idCurrentUser).ToList();
                    else
                    {
                        List<DownloadStatistics> results = new List<DownloadStatistics>();
                        Int32 pageIndex = 0;
                        List<long> idPagedItems = idVersions.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                        while (idPagedItems.Any())
                        {
                            results.AddRange(DownloadStatisticsGetQuery(idVersions, idCurrentUser).ToList());
                            pageIndex++;
                            idPagedItems = idVersions.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                        }
                        return results;
                    }
                }

                public List<long> DownloadedItems(List<long> idItems, Int32 idCurrentUser)
                {
                    if (idItems.Count <= maxItemsForQuery)
                        return DownloadStatisticsGetQuery(q => idItems.Contains(q.IdItem) && q.IdPerson == idCurrentUser).Select(s => s.IdItem).Distinct().ToList();
                    return DownloadStatisticsGet(idItems, idCurrentUser).Select(s => s.IdItem).Distinct().ToList();
                }
                public List<long> DownloadedVersions(List<long> idVersions, Int32 idCurrentUser)
                {
                    if (idVersions.Count <= maxItemsForQuery)
                        return DownloadStatisticsGetQuery(q => idVersions.Contains(q.IdVersion) && q.IdPerson == idCurrentUser).Select(s => s.IdVersion).Distinct().ToList();
                    return DownloadStatisticsGet(idVersions, idCurrentUser).Select(s => s.IdVersion).Distinct().ToList();
                }
                private IQueryable<DownloadStatistics> DownloadStatisticsGetQuery(List<long> idItems, Int32 idCurrentUser, Boolean alsoLink = false, long idLink = 0)
                {
                    return DownloadStatisticsGetQuery(q => idItems.Contains(q.IdItem) && q.IdPerson == idCurrentUser);
                }
                private IQueryable<DownloadStatistics> DownloadStatisticsGetQueryByVersion(List<long> idVersions, Int32 idCurrentUser, Boolean alsoLink = false, long idLink = 0)
                {
                    return DownloadStatisticsGetQuery(q => idVersions.Contains(q.IdVersion) && q.IdPerson == idCurrentUser);
                }
                private IQueryable<DownloadStatistics> DownloadStatisticsGetQuery(RepositoryType type, Int32 idCommunity, Int32 idUserRepository,Int32 idCurrentUser=0)
                {
                    return DownloadStatisticsGetQuery(q => q.RepositoryType == type && q.RepositoryIdPerson == idUserRepository
                        && q.RepositoryIdCommunity == idCommunity && (idCurrentUser<=0 || idCurrentUser==q.IdPerson));
                }
                public IQueryable<DownloadStatistics> DownloadStatisticsGetQuery(Expression<Func<DownloadStatistics, bool>> filters)
                {
                    return (from q in Manager.GetIQ<DownloadStatistics>() select q).Where(filters);
                }
            #endregion
            
            #region "Play"
                public Boolean StatisticsAddPlay(Int32 idPerson, RepositoryIdentifier rIdentifier, liteRepositoryItemVersion version, Int32 idCommunity, long idAction, String playSessionId)
                {
                    return StatisticsAddPlay(idPerson, rIdentifier, version.IdItem, version.UniqueIdItem, version.Id, version.UniqueIdVersion, version.Type, idCommunity, idAction, playSessionId);
                }
                public Boolean StatisticsAddPlay(Int32 idPerson, RepositoryIdentifier rIdentifier, long idItem, Guid uniqueIdItem, long idVersion, Guid uniqueIdVersion, ItemType type, Int32 idCommunity, long idAction, String playSessionId)
                {
                    Boolean result = false;
                    try
                    {
                        PlayStatistics stat = new PlayStatistics();
                        stat.CreatedIPaddress = UC.IpAddress;
                        stat.CreatedOn = DateTime.Now;
                        stat.CreatedProxyIPaddress = UC.ProxyIpAddress;
                        stat.WorkingSessionID = playSessionId;
                        stat.IdAction = idAction;
                        stat.IdCommunity = idCommunity;
                        stat.IdItem = idItem;
                        stat.IdPerson = idPerson;
                        stat.IdVersion = idVersion;
                        stat.ItemType = type;
                        stat.UniqueIdItem = uniqueIdItem;
                        stat.UniqueIdVersion = uniqueIdVersion;
                        stat.RepositoryIdCommunity = rIdentifier.IdCommunity;
                        stat.RepositoryIdPerson = rIdentifier.IdPerson;
                        stat.RepositoryType = rIdentifier.Type;
                        Manager.SaveOrUpdate(stat);
                    }
                    catch (Exception ex)
                    {

                    }
                    return result;
                }

                public Dictionary<long, long> PlayStatisticsGetFull(List<long> idItems)
                {
                    if (idItems.Count <= maxItemsForQuery)
                        return PlayStatisticsGetQuery(idItems).Select(i => i.IdItem).ToList().GroupBy(i => i).ToDictionary(i => i.Key, i => (long)i.Count());
                    else
                        return PlayStatisticsGet(idItems).Select(i => i.IdItem).ToList().GroupBy(i => i).ToDictionary(i => i.Key, i => (long)i.Count());
                }
                public Dictionary<long, long> PlayStatisticsGetFull(List<long> idItems, Int32 idCurrentUser)
                {
                    if (idItems.Count <= maxItemsForQuery)
                        return PlayStatisticsGetQuery(idItems, idCurrentUser).Select(i => i.IdItem).ToList().GroupBy(i => i).ToDictionary(i => i.Key, i => (long)i.Count());
                    else
                        return PlayStatisticsGet(idItems, idCurrentUser).Select(i => i.IdItem).ToList().GroupBy(i => i).ToDictionary(i => i.Key, i => (long)i.Count());
                }


                public List<PlayStatistics> PlayStatisticsGet(List<long> idItems)
                {
                    if (idItems.Count <= maxItemsForQuery)
                        return PlayStatisticsGetQuery(idItems).ToList();
                    else
                    {
                        List<PlayStatistics> results = new List<PlayStatistics>();
                        Int32 pageIndex = 0;
                        List<long> idPagedItems = idItems.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                        while (idPagedItems.Any())
                        {
                            results.AddRange(PlayStatisticsGetQuery(idItems).ToList());
                            pageIndex++;
                            idPagedItems = idItems.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                        }
                        return results;
                    }
                }
                public List<PlayStatistics> PlayStatisticsGet(List<long> idItems, Int32 idCurrentUser)
                {
                    if (idItems.Count <= maxItemsForQuery)
                        return PlayStatisticsGetQuery(idItems, idCurrentUser).ToList();
                    else
                    {
                        List<PlayStatistics> results = new List<PlayStatistics>();
                        Int32 pageIndex = 0;
                        List<long> idPagedItems = idItems.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                        while (idPagedItems.Any())
                        {
                            results.AddRange(PlayStatisticsGetQuery(idItems, idCurrentUser).ToList());
                            pageIndex++;
                            idPagedItems = idItems.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                        }
                        return results;
                    }
                }
                private IQueryable<PlayStatistics> PlayStatisticsGetQuery(List<long> idItems, Int32 idCurrentUser)
                {
                    return PlayStatisticsGetQuery(q => idItems.Contains(q.IdItem) && q.IdPerson == idCurrentUser);
                }
                private IQueryable<PlayStatistics> PlayStatisticsGetQuery(List<long> idItems)
                {
                    return PlayStatisticsGetQuery(q => idItems.Contains(q.IdItem));
                }
                private IQueryable<PlayStatistics> PlayStatisticsGetQuery(RepositoryType type, Int32 idCommunity, Int32 idUserRepository, Int32 idCurrentUser = 0)
                {
                    return PlayStatisticsGetQuery(q => q.RepositoryType == type && q.RepositoryIdPerson == idUserRepository
                        && q.RepositoryIdCommunity == idCommunity && (idCurrentUser <= 0 || idCurrentUser == q.IdPerson));
                }

                public IQueryable<PlayStatistics> PlayStatisticsGetQuery(Expression<Func<PlayStatistics, bool>> filters)
                {
                    return (from q in Manager.GetIQ<PlayStatistics>() select q).Where(filters);
                }


       
        #endregion
        #endregion
    }
}