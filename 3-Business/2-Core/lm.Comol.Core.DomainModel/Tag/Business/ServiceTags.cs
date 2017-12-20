using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

using NHibernate.Linq;
using lm.Comol.Core.Tag.Domain;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.DomainModel.Filters;
using lm.Comol.Core.DomainModel.Languages;

namespace lm.Comol.Core.Tag.Business
{
    public class ServiceTags : CoreServices 
    {
        protected const Int32 maxItemsForQuery = 500;
        protected iApplicationContext _Context;
        private lm.Comol.Core.Dashboard.Business.ServiceDashboard _ServiceDashboard;
        protected lm.Comol.Core.Dashboard.Business.ServiceDashboard ServiceDashboard { get { return (_ServiceDashboard == null) ? new lm.Comol.Core.Dashboard.Business.ServiceDashboard(_Context) : _ServiceDashboard; } }


        #region initClass
            public ServiceTags() :base() { }
            public ServiceTags(iApplicationContext oContext) :base(oContext.DataContext) {
                _Context = oContext;
                this.Manager = new BaseModuleManager(oContext.DataContext);
                this.UC = oContext.UserContext;
            }
            public ServiceTags(iDataContext oDC)
                : base(oDC)
            {
                this.Manager = new BaseModuleManager(oDC);
                _Context = new ApplicationContext() { DataContext = oDC };
            }
        #endregion

        #region "Permission"
            public int ServiceModuleID()
            {
                return this.Manager.GetModuleID(ModuleTags.UniqueCode);
            }
            public ModuleTags GetPermission(Int32 idCommunity)
            {
                Person person = Manager.GetPerson(UC.CurrentUserID);
                if (idCommunity <= 0)
                {
                    if (person == null)
                        return ModuleTags.CreatePortalmodule((int)UserTypeStandard.Guest);
                    else
                        return ModuleTags.CreatePortalmodule(person.TypeID);
                }
                else {
                    return new ModuleTags(this.Manager.GetModulePermission(UC.CurrentUserID, idCommunity, ServiceModuleID()));
                }
            }

        #endregion

        #region "Cache loading methods"
            /// <summary>
            /// Get all tags available
            /// </summary>
            /// <param name="type">type of tag to load</param>
            /// <param name="useCache">retrieve from cache or from DB</param>
            /// <returns></returns>
            public List<liteTagItem> CacheGetTags(TagType type, Boolean useCache)
            {
                List<liteTagItem> tags = null;
                tags = (useCache) ? lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<List<liteTagItem>>(CacheKeys.Tags(type)) : null;

                if (tags == null || !tags.Any())
                {
                    tags = (from t in Manager.GetIQ<liteTagItem>() where t.Deleted == BaseStatusDeleted.None select t).ToList();
                    Manager.DetachList(tags);
                    if (useCache)
                        CacheHelper.AddToCache<List<liteTagItem>>(CacheKeys.Tags(type), tags, CacheExpiration.Week);
                }
                return tags;
            }
            /// <summary>
            /// Get all association between tag and communities
            /// </summary>
            /// <param name="useCache">retrieve from cache or from DB</param>
            /// <returns></returns>
            public List<liteCommunityTag> CacheGetCommunityAssociation(Boolean useCache)
            {
                List<liteCommunityTag> links = null;
                links = (useCache) ? lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<List<liteCommunityTag>>(CacheKeys.AllCommunityTags) : null;

                if (links == null || !links.Any())
                {
                    links = (from t in Manager.GetIQ<liteCommunityTag>()
                             where t.Deleted == BaseStatusDeleted.None && t.Tag != null && t.Tag.Status == lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available 
                             select t).ToList();
                    Manager.DetachList(links);
                    if (useCache)
                        CacheHelper.AddToCache<List<liteCommunityTag>>(CacheKeys.AllCommunityTags, links, CacheExpiration.Day);
                }
                return links;
            }

            public List<dtoCommunityTags> CacheGetUserCommunitiesAssociation(Int32 idUser, List<Int32> idCommunities, TagType type, Boolean useCache)
            {
                List<dtoCommunityTags> links = null;
                links = (useCache) ? lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<List<dtoCommunityTags>>(CacheKeys.UserCommunitiesTags(idUser,type)) : null;
                if (links == null || !links.Any())
                {
                    links = GetUserCommunitiesAssociation(idCommunities, type);
                    if (useCache)
                        CacheHelper.AddToCache<List<dtoCommunityTags>>(CacheKeys.UserCommunitiesTags(idUser,type), links, CacheExpiration._12hours);
                }
                return links;
            }
            private List<dtoCommunityTags> GetUserCommunitiesAssociation(List<Int32> idCommunities, TagType type)
            {
                Int32 pageIndex = 0;
                var idQuery = idCommunities.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                List<dtoCommunityTags> items = new List<dtoCommunityTags>();
                while (idQuery.Any())
                {
                    items.AddRange((from t in Manager.GetIQ<liteCommunityTag>()
                                           where t.Deleted == BaseStatusDeleted.None && idQuery.Contains(t.IdCommunity)
                                           && t.Tag != null && t.Tag.Status == lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available 
                                    select new { IdCommunity = t.IdCommunity, IdTag = t.Tag.Id  }).ToList().GroupBy(t => t.IdCommunity).Select(t => new dtoCommunityTags() { IdCommunity = t.Key, Tags = t.Select(a => a.IdTag).ToList() }).ToList());
                    pageIndex++;
                    idQuery = idCommunities.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                }
                return items;
            }


            public List<String> GetCommunityAssociationToString(Int32 idCommunity, Int32 idUserLanguage, Int32 idDefaultLanguage, Boolean useCache)
            {
                Dictionary<Int32, List<String>> tagDict =
                    GetCommunityAssociationToString(new List<Int32>() {idCommunity}, idUserLanguage, idDefaultLanguage,
                        useCache);
                
                if(tagDict != null && tagDict.ContainsKey(idCommunity))
                    return tagDict[idCommunity];

                return new List<string>();
                
                //return GetCommunityAssociationToString(new List<Int32>() { idCommunity}, idUserLanguage, idDefaultLanguage, useCache)[idCommunity];
            }
            public Dictionary<Int32, List<String>> GetCommunityAssociationToString(List<Int32> idCommunities,Int32 idUserLanguage, Int32 idDefaultLanguage, Boolean useCache)
            {
                Dictionary<Int32, List<String>> result = null;
                List<liteTagItem> tags = CacheGetTags(TagType.Community, useCache);
               
                if (tags != null || !tags.Any())
                {
                    Dictionary<Int32, List<long>> links = null;
                    List<liteCommunityTag> cTags = CacheGetCommunityAssociation(useCache);
                    //if (idCommunities.Count > maxItemsForQuery)
                    //    links = GetCommunitiesLinks(idCommunities);
                    //else
                    //    links = (from t in Manager.GetIQ<liteCommunityTag>()
                    //             where t.Deleted == BaseStatusDeleted.None && idCommunities.Contains(t.IdCommunity)
                    //             select new {IdCommunity= t.IdCommunity, IdTag = t.IdTag}).ToList().GroupBy(t => t.IdCommunity).ToDictionary(t => t.Key, t => t.Select(tt => tt.IdTag).ToList());
                    //result= links.ToDictionary(l=> l.Key, l=> tags.Where(t=>  l.Value.Contains(t.Id)).Select(t=> t.GetTitle(idUserLanguage,idDefaultLanguage)).OrderBy(t=> t).ToList());
                    result = cTags.Where(t => idCommunities.Contains(t.IdCommunity)).GroupBy(t => t.IdCommunity).ToDictionary(t => t.Key, t => tags.Where(tg => t.Select(tt => tt.Tag.Id).Contains(tg.Id)).Select(tg => tg.GetTitle(idUserLanguage, idDefaultLanguage)).OrderBy(tg => tg).ToList());
                }
                else
                    result = idCommunities.ToDictionary(i=> i, i=> new List<String>());
                return result;
            }
            private Dictionary<Int32, List<long>> GetCommunitiesLinks(List<Int32> idCommunities)
            {
                Int32 pageIndex = 0;
                var idQuery = idCommunities.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                List<liteCommunityTag> associations = new List<liteCommunityTag>();
                while (idQuery.Any())
                {
                    associations.AddRange((from t in Manager.GetIQ<liteCommunityTag>()
                                           where t.Deleted == BaseStatusDeleted.None && idQuery.Contains(t.IdCommunity) && t.Tag != null && t.Tag.Status == lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available 
                     select t).ToList());
                    pageIndex++;
                    idQuery = idCommunities.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                }
                return associations.GroupBy(t => t.IdCommunity).ToDictionary(t => t.Key, t => t.Select(tt => tt.Tag.Id).ToList());
            }
        #endregion

        #region "Manage"

          

            //private Dictionary<Int32, ModuleTags> GetPermissions(Person person, Boolean forOrganization)
            //{
            //    Dictionary<Int32, ModuleTags> permissions = new Dictionary<Int32, ModuleTags>();
            //    List<Organization> organizations = ServiceCommunityManagement.GetAvailableOrganizations(person.Id, SearchCommunityFor.Subscribed);
            //    permissions.Add(-3, ModuleTags.CreatePortalmodule(person.TypeID));

            //    if (organizations != null && organizations.Any())
            //    {
            //        if (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrator)
            //            organizations.ForEach(o => permissions.Add(o.Id, permissions[-3]));
            //        else
            //        {
            //            List<Int32> communities = (from c in Manager.GetIQ<liteCommunityInfo>()
            //                                       where c.IdFather == 0
            //                                           && organizations.Where(o => o.Id == c.IdOrganization).Any()
            //                                       select c.Id).ToList();
            //            communities.ForEach(c => permissions.Add(c., GetPermission(c)));
            //        }
            //    }
            //    return permissions;
            //}

            public TagItem SetStatus(long idTag, lm.Comol.Core.Dashboard.Domain.AvailableStatus status)
            {
                TagItem item = null;
                try
                {
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    if (person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
                    {
                        item = Manager.Get<TagItem>(idTag);
                        item.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        Manager.BeginTransaction();
                        item.Status = status;
                        Manager.Commit();
                        if (item.Type == TagType.Community)
                        {
                            CacheHelper.PurgeCacheItems(CacheKeys.AllCommunityTags);
                            CacheHelper.PurgeCacheItems(CacheKeys.AllUserCommunitiesTags);
                        }
                        CacheHelper.PurgeCacheItems(CacheKeys.Tags(item.Type));
                        if (item.MyTile.Status != Dashboard.Domain.AvailableStatus.Available && status != Dashboard.Domain.AvailableStatus.Draft)
                            ServiceDashboard.TileSetStatus(item.MyTile, status);
                    }
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
                return item;
            }
            public TagItem VirtualDelete(long idTag, Boolean delete)
            {
                TagItem item = null;
                try {
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    item = Manager.Get<TagItem>(idTag);
                    if (item != null && person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
                    {
                        Manager.BeginTransaction();
                        item.Deleted = (delete) ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                        item.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);

                        if (item.Translations.Any())
                        {
                            foreach (TagTranslation t in item.Translations) {
                                t.Deleted = delete ? (t.Deleted | BaseStatusDeleted.Cascade) : (BaseStatusDeleted)((int)t.Deleted - (int)BaseStatusDeleted.Cascade);
                                t.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            }
                            Manager.SaveOrUpdateList(item.Translations);
                        }
                        if (item.MyTile != null)
                        {
                            ServiceDashboard.TileVirtualDelete(item.MyTile, delete ? (item.MyTile.Deleted | BaseStatusDeleted.Cascade) : ((BaseStatusDeleted)((int)item.MyTile.Deleted - (int)BaseStatusDeleted.Cascade)));
                        }
                        Manager.SaveOrUpdate(item);
                        Manager.Commit();
                        if (item.Type == TagType.Community)
                        {
                            CacheHelper.PurgeCacheItems(CacheKeys.AllCommunityTags);
                            CacheHelper.PurgeCacheItems(CacheKeys.AllUserCommunitiesTags);
                        }
                        CacheHelper.PurgeCacheItems(CacheKeys.Tags(item.Type));
                    }
                }
                catch (Exception ex)
                {
                    item = null;
                    Manager.RollBack();
                }
                return item;
            }
            public List<lm.Comol.Core.Dashboard.Domain.dtoItemFilter<dtoLanguageItem>> GetLanguageSelectorItems(String dLanguageName, String dLanguageCode)
            {
                List<lm.Comol.Core.Dashboard.Domain.dtoItemFilter<dtoLanguageItem>> items = Manager.GetAllLanguages().OrderByDescending(l => l.isDefault).ThenBy(l => l.Name).Select(l =>
                    new lm.Comol.Core.Dashboard.Domain.dtoItemFilter<dtoLanguageItem>() { Selected = false, Value = new dtoLanguageItem() { IdLanguage = l.Id, LanguageCode = l.Code, LanguageName = l.Name } }).ToList();

                items.Insert(0, new lm.Comol.Core.Dashboard.Domain.dtoItemFilter<dtoLanguageItem>() { Selected = false, Value = new dtoLanguageItem() { IdLanguage = -1, LanguageCode = dLanguageCode, LanguageName = dLanguageName, IsMultiLanguage = true } });
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
            public Int32 GetCommunitiesWithNoTags(Boolean useCache = true )
            {
                Int32 count = 0;
                try
                {
                    List<Int32> idCommunities = (from c in Manager.GetIQ<liteCommunity>() select c.Id).ToList();
                    List<Int32> idTagsCommunities = CacheGetCommunityAssociation(useCache).Select(l=> l.IdCommunity).ToList();
                    count = idCommunities.Where(i=> !idTagsCommunities.Any(t=> t==i)).Count();
                }
                catch (Exception ex)
                {

                }
                if (count < 0)
                    count = 0;

                return count;
            }
            public Int32 GetUntranslatedTagsCount(Int32 idLanguage,Boolean fromRecycleBin,Boolean useCache = true)
            {
                Int32 count = 0;
                try
                {
                    if (idLanguage > 0)
                    {
                        BaseStatusDeleted deleted = (fromRecycleBin) ? BaseStatusDeleted.Manual: BaseStatusDeleted.None;
                        count = (from t in Manager.GetIQ<liteTagItem>()
                                                 where t.Deleted == deleted
                                                 select t).ToList().Where(t => t.Translations == null
                                                     || !t.Translations.Where(tr => tr.Deleted == BaseStatusDeleted.None && tr.IdLanguage == idLanguage).Any()
                                                     || t.Translations.Where(tr => tr.Deleted == BaseStatusDeleted.None && tr.IdLanguage == idLanguage && String.IsNullOrEmpty(tr.Translation.Title)).Any()
                                                     ).Count();
                                                 

                    }
                }
                catch (Exception ex)
                {

                }
                return count;
            }
            public Boolean HasDefaultDuplicate(String title, String shortTitle,TagType type)
            {
                Boolean result = false;
                try
                {
                    title = String.IsNullOrEmpty(title) ? "" : title.ToLower();
                    shortTitle = String.IsNullOrEmpty(shortTitle) ? "" : shortTitle.ToLower();
                    result = (from t in Manager.GetIQ<liteTagItem>() where t.Deleted == BaseStatusDeleted.None && t.Type== type select t).ToList().Where(t=> t.DefaultTranslation.IsTranslationEqual(title, shortTitle, false )).Any();
                }
                catch (Exception ex) { 
                
                
                }
                return result;
            }
            public Boolean HasDefaultDuplicate(long idTag, String title, TagType type, Int32 idOrganization =0)
            {
                Boolean result = false;
                try
                {
                    title = String.IsNullOrEmpty(title) ? "" : title.ToLower();
                    if (idOrganization <1)
                        result = (from t in Manager.GetIQ<liteTagItem>() 
                                  where t.Id !=idTag && t.Deleted == BaseStatusDeleted.None  && t.IsSystem && t.Type == type select t).ToList().Where(t => t.DefaultTranslation.IsTitleEqual(title)).Any();
                    else
                        result = (from t in Manager.GetIQ<liteTagItem>()
                                  where t.Id != idTag && t.Deleted == BaseStatusDeleted.None && !t.IsSystem && t.Type == type
                                  select t).ToList().Where(t => t.Organizations.Where(o=> o.Deleted== BaseStatusDeleted.None && o.IsDefault && o.IdOrganization==idOrganization).Any() && t.DefaultTranslation.IsTitleEqual(title)).Any();
                }
                catch (Exception ex)
                {


                }
                return result;
            }
        #endregion
    }
}