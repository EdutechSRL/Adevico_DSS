using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using lm.Comol.Core.FileRepository.Domain;
using System.Linq.Expressions;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.FileRepository.Business
{
    public partial class ServiceFileRepository 
    {
        
        #region "ModuleLink"
            public Boolean ModuleLinkIsAutoEvaluable(long idlink)
            {
                return (from l in Manager.GetIQ<liteModuleLink>() where l.Id == idlink && l.Deleted == BaseStatusDeleted.None && l.AutoEvaluable select l.Id).Any();
            }
            public Int32 ModuleLinkGetIdSourceCommunity(long idlink)
            {
                return (from l in Manager.GetIQ<liteModuleLink>() where l.Id == idlink && l.Deleted == BaseStatusDeleted.None select l.SourceItem.CommunityID).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            public List<long> GetIdLinkedItemsBySource(List<long> idItems, String moduleCode)
            {
                if (idItems.Count <= maxItemsForQuery)
                    return GetQueryModuleLink(l => l.Deleted == BaseStatusDeleted.None && l.SourceItem.ServiceCode == moduleCode && l.DestinationItem.ServiceCode==ModuleRepository.UniqueCode && idItems.Contains(l.DestinationItem.ObjectLongID)).Select(q => q.DestinationItem.ObjectLongID).Distinct().ToList();
                else
                {
                    List<long> files = new List<long>();
                    Int32 pageIndex = 0;
                    List<long> idPagedItems = idItems.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    while (idPagedItems.Any())
                    {
                        files.AddRange(GetQueryModuleLink(l => l.Deleted == BaseStatusDeleted.None && l.SourceItem.ServiceCode == moduleCode && idPagedItems.Contains(l.DestinationItem.ObjectLongID)).Select(q => q.DestinationItem.ObjectLongID).Distinct().ToList());
                        pageIndex++;
                        idPagedItems = idItems.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    }
                    return files.Distinct().ToList();
                }
            }
            public List<long> GetIdLinkedItems(List<long> idItems)
            {
                if (idItems.Count <= maxItemsForQuery)
                    return GetQueryModuleLink(l => l.Deleted == BaseStatusDeleted.None && l.DestinationItem.ServiceCode == ModuleRepository.UniqueCode && idItems.Contains(l.DestinationItem.ObjectLongID)).Select(q => q.DestinationItem.ObjectLongID).Distinct().ToList();
                else
                {
                    List<long> files = new List<long>();
                    Int32 pageIndex = 0;
                    List<long> idPagedItems = idItems.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    while (idPagedItems.Any())
                    {
                        files.AddRange(GetQueryModuleLink(l => l.Deleted == BaseStatusDeleted.None && l.DestinationItem.ServiceCode == ModuleRepository.UniqueCode && idPagedItems.Contains(l.DestinationItem.ObjectLongID)).Select(q => q.DestinationItem.ObjectLongID).Distinct().ToList());
                        pageIndex++;
                        idPagedItems = idItems.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    }
                    return files.Distinct().ToList();
                }
            }
            public List<String> GetLinkedModules(long idItem)
            {
                return GetQueryModuleLink(l => l.Deleted == BaseStatusDeleted.None && l.DestinationItem.ServiceCode == ModuleRepository.UniqueCode
                    && idItem == l.DestinationItem.ObjectLongID).Select(l => l.SourceItem.ServiceCode).Distinct().ToList();
            }
            public Dictionary<long, List<String>> GetLinkedModules(List<long> idItems)
            {
                if (idItems == null || !idItems.Any())
                    return new Dictionary<long, List<String>>();
                else
                    idItems = idItems.Distinct().ToList();
                if (idItems.Count <= maxItemsForQuery)
                    return GetPagedLinkedModules(idItems);
                else
                {
                    Dictionary<long, List<String>> results = new Dictionary<long, List<String>>();
                    Int32 pageIndex = 0;
                    List<long> idPagedItems = idItems.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    while (idPagedItems.Any())
                    {
                        Dictionary<long, List<String>> pResults = GetPagedLinkedModules(idPagedItems);
                        if (pResults!=null && pResults.Keys.Any())
                            results = results.Concat(pResults).ToDictionary(l=> l.Key, l=> l.Value);
                        pageIndex++;
                        idPagedItems = idItems.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    }
                    return results;
                }
            }
            private Dictionary<long, List<String>> GetPagedLinkedModules(List<long> idItems)
            {
                return GetQueryModuleLink(l => l.Deleted == BaseStatusDeleted.None && l.DestinationItem.ServiceCode == ModuleRepository.UniqueCode && idItems.Contains(l.DestinationItem.ObjectLongID) ).Select(l=> new {IdItem = l.DestinationItem.ObjectLongID, ModuleCode = l.SourceItem.ServiceCode}) .ToList().GroupBy(
                l => l.IdItem).ToDictionary(l=> l.Key, l=> l.Select(i=> i.ModuleCode).Distinct().ToList());
            }
            public List<dtoLinkedItems> GetLinkedItems(List<long> idItems)
            {
                if (idItems.Count <= maxItemsForQuery)
                    return GetDefaultQueryModuleLink(idItems);
                else
                {
                    List<dtoLinkedItems> links = new List<dtoLinkedItems>();
                    Int32 pageIndex = 0;
                    List<long> idPagedItems = idItems.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    while (idPagedItems.Any())
                    {
                        links.AddRange(GetDefaultQueryModuleLink(idPagedItems));
                        pageIndex++;
                        idPagedItems = idItems.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    }
                    return links.GroupBy(l => l.ModuleCode).Select(k => new dtoLinkedItems() { ModuleCode = k.Key, IdModule = k.Select(l => l.IdModule).FirstOrDefault(), IdItems = k.SelectMany(l => l.IdItems).ToList().Distinct().ToList() }).ToList();
                }
            }
            private List<dtoLinkedItems> GetDefaultQueryModuleLink(List<long> idItems)
            {
                return GetQueryModuleLink(l => l.Deleted == BaseStatusDeleted.None && l.DestinationItem.ServiceCode == ModuleRepository.UniqueCode && idItems.Contains(l.DestinationItem.ObjectLongID)).ToList().GroupBy(
                l=>l.SourceItem.ServiceCode).Select(l=> new dtoLinkedItems() {  ModuleCode=l.Key, IdModule=l.Select(x=> x.SourceItem.ServiceID).FirstOrDefault(), IdItems= l.Select(ln=> ln.DestinationItem.ObjectLongID).Distinct().ToList()}).ToList();
            }
            private IQueryable<liteModuleLink> GetQueryModuleLink(Expression<Func<liteModuleLink, bool>> filters)
            {
                return (from q in Manager.GetIQ<liteModuleLink>() select q).Where(filters);
            }
        #endregion

        #region "Others"
            public Boolean IsValidPerson(Int32 idPerson)
            {
                litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                return (person != null && person.Id > 0 && (person.TypeID != (int)UserTypeStandard.Guest || person.TypeID != (int)UserTypeStandard.PublicUser));
            }
            public litePerson GetValidPerson(Int32 idPerson)
            {
                litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                return (person != null && person.Id > 0 && (person.TypeID != (int)UserTypeStandard.Guest || person.TypeID != (int)UserTypeStandard.PublicUser)) ? person :  null;
            }
            public Boolean IsValidAdministrator(Int32 idPerson)
            {
                litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                return person != null && person.Id > 0 && (person.TypeID  == (int)UserTypeStandard.Administrator || person.TypeID == (int)UserTypeStandard.SysAdmin);
            }
        #endregion
    }
}