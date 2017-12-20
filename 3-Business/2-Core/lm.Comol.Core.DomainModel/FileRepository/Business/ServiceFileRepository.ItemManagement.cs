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
      

        #region "Get Item"
            public String FolderGetName(long idItem)
            {
                return GetQuery<liteRepositoryItem>().Where(i => i.Id == idItem && i.Type == ItemType.Folder).Select(i => i.DisplayName).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            public String ItemGetName(long idItem)
            {
                return GetQuery<liteRepositoryItem>().Where(i => i.Id == idItem).Select(i => i.DisplayName).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            public ItemType ItemGetType(long idItem)
            {
                ItemType result = ItemType.None;
                return GetQuery<liteRepositoryItem>().Where(i => i.Id == idItem).Select(i => i.Type).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            public liteRepositoryItem ItemGet(long idItem)
            {
                return Manager.Get<liteRepositoryItem>(idItem);
            }
            public liteRepositoryItemVersion VersionGet(long idVersion)
            {
                return Manager.Get<liteRepositoryItemVersion>(idVersion);
            }
            public liteRepositoryItemVersion ItemGetVersion(long idItem, long idVersion = 0)
            {
                liteRepositoryItemVersion version = null;
                if (idVersion > 0)
                    version = GetQueryVersions(v => v.Id == idVersion).Skip(0).Take(1).FirstOrDefault();
                else
                    version = GetQueryVersions(v => v.IdItem== idItem && v.Deleted== DomainModel.BaseStatusDeleted.None && v.Status== ItemStatus.Active).Skip(0).Take(1).FirstOrDefault();
                return version;
            }
            public IQueryable<liteRepositoryItemVersion> GetQueryVersions(RepositoryIdentifier identifier)
            {
                return GetQueryVersions(v => v.Repository.Type == identifier.Type && (identifier.Type== RepositoryType.Community && v.Repository.IdCommunity == identifier.IdCommunity));
            }
            public IQueryable<liteRepositoryItemVersion> GetQueryVersions(Expression<Func<liteRepositoryItemVersion, bool>> filters)
            {
                return (from q in Manager.GetIQ<liteRepositoryItemVersion>() select q).Where(filters);
            }
           
        #endregion

        #region "Management"
            #region "Hide/Show"
                public List<liteRepositoryItem> ItemsSetVisibility(List<long> idItems, Boolean visible, RepositoryType type, Int32 idCommunity)
                {
                    List<liteRepositoryItem> items = null;
                    try
                    {
                        litePerson person = GetValidPerson(UC.CurrentUserID);
                        List<liteRepositoryItem> cItems = (idItems.Count<=maxItemsForQuery ? GetQuery(type, idCommunity).Where(i=> idItems.Contains(i.Id)).ToList() : GetQuery(type, idCommunity).ToList().Where(i=> idItems.Contains(i.Id)).ToList());
                        if (cItems.Any(c=> c.Deleted== BaseStatusDeleted.None) && person != null)
                        {
                            items = new List<liteRepositoryItem>();
                            foreach (liteRepositoryItem item in cItems.Where(i => i.Deleted== BaseStatusDeleted.None))
                            {
                                Manager.BeginTransaction();
                                item.UpdateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress);
                                item.IsVisible = visible;
                                Manager.SaveOrUpdate(item);
                                Manager.Refresh(Manager.Get<RepositoryItem>(item.Id));
                                Manager.Commit();
                                items.Add(item);
                            }
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.Repository(type, idCommunity));
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersViewOfRepository(type, idCommunity));
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersSizeViewOfRepository(type, idCommunity));
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return items;
                }
                public liteRepositoryItem ItemSetVisibility(long idItem, Boolean visible, RepositoryType type, Int32 idCommunity)
                {
                    liteRepositoryItem item = null;
                    try
                    {
                        item = Manager.Get<liteRepositoryItem>(idItem);
                        if (item != null)
                        {
                            litePerson person = GetValidPerson(UC.CurrentUserID);
                            if (person != null)
                            {
                                Manager.BeginTransaction();
                                item.UpdateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress);
                                item.IsVisible = visible;
                                Manager.SaveOrUpdate(item);
                                Manager.Commit();
                                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.Repository(type,idCommunity));
                                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersViewOfRepository(type,idCommunity));
                                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersSizeViewOfRepository(type,idCommunity));
                                Manager.Refresh(Manager.Get<RepositoryItem>(item.Id));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return item;
                }
            #endregion

            #region "Set item settings"
                public ItemSaving ItemSetBaseSettings(long idItem, String description, String name, String url, DisplayMode? mode, Boolean isVisible, Boolean allowUpload, List<String> tags)
                {
                    return ItemSetBaseSettings(Manager.Get<RepositoryItem>(idItem), description, name, url, mode,isVisible,  allowUpload,  tags);
                }
                public ItemSaving ItemSetBaseSettings(RepositoryItem source, String description, String name, String url, DisplayMode? mode, Boolean isVisible, Boolean allowUpload, List<String> tags)
                {
                    ItemSaving saving = ItemSaving.None;
                    try
                    {
                        if (source != null)
                        {
                            litePerson person = GetValidPerson(UC.CurrentUserID);
                            if (person != null)
                            {
                                Boolean changedVersion = false;
                                Boolean changeTags = false;
                                Manager.BeginTransaction();
                                RepositoryItemVersion version = Manager.Get<RepositoryItemVersion>(source.IdVersion);
                                if (version != null && !version.IsActive)
                                {
                                    changedVersion = true;
                                    version = (from v in Manager.GetIQ<RepositoryItemVersion>() where v.Deleted == BaseStatusDeleted.None && v.UniqueIdItem == source.UniqueId && v.IsActive select v).OrderByDescending(v => v.Id).Skip(0).Take(1).ToList().FirstOrDefault();
                                }
                                if (version != null)
                                {
                                    source.Description = description;
                                    source.IsVisible = isVisible;
                                    IEnumerable<RepositoryItem> items = GetFullQuery(source.Repository.Type, source.Repository.IdCommunity);
                                    switch (source.Type)
                                    {
                                        case ItemType.Folder:
                                            List<String> brothersFolder = items.Where(i => i.Id != source.Id && i.IdFolder == source.IdFolder && i.Type == ItemType.Folder && i.Deleted== BaseStatusDeleted.None ).Select(f => f.Name).ToList();
                                            if (ItemHasDuplicate(brothersFolder, name, ItemType.Folder))
                                                saving = ItemSaving.NameDuplicate;
                                            else
                                                source.Name = name;
                                            source.AllowUpload = allowUpload;
                                            break;
                                        case ItemType.Link:
                                            List<String> brothersLinks = items.Where(i => i.Id != source.Id && i.IdFolder == source.IdFolder && i.Type == ItemType.Link && i.Deleted == BaseStatusDeleted.None).Select(f => f.DisplayName).ToList();
                                            if (ItemHasDuplicate(brothersLinks, name, ItemType.Link))
                                                saving = ItemSaving.NameDuplicate;
                                            else
                                                source.Name = name;
                                            brothersLinks = items.Where(i => i.Id != source.Id && i.IdFolder == source.IdFolder && i.Type == ItemType.Link && i.Deleted == BaseStatusDeleted.None).Select(f => f.Url).ToList();
                                            if (ItemHasDuplicate(brothersLinks, url, ItemType.Link))
                                                saving = (saving == ItemSaving.None ? ItemSaving.UrlDuplicate : ItemSaving.NameAndUrlDuplicate);
                                            else
                                                source.Url = url;
                                            break;
                                        default:
                                            List<String> brothersFile = items.Where(i => i.Id != source.Id && i.IdFolder == source.IdFolder && i.Type == ItemType.File && i.Deleted == BaseStatusDeleted.None).Select(f => f.DisplayName).ToList();
                                            if (ItemHasDuplicate(brothersFile, name, ItemType.File, source.Extension))
                                                saving = ItemSaving.NameDuplicate;
                                            else
                                                source.Name = name;
                                            break;

                                    }
                                    if (saving == ItemSaving.None)
                                        saving = ItemSaving.Saved;
                                    source.Description = description;
                                    version.Description = description;
                                    version.Name = source.Name;
                                    if (mode.HasValue)
                                    {
                                        source.DisplayMode = mode.Value;
                                        version.DisplayMode = mode.Value;
                                    }
                                    if (tags.Any())
                                    {
                                        changeTags = (source.Tags != String.Join(",", tags));
                                        source.Tags = String.Join(",", tags);
                                    }
                                    else
                                        source.Tags = "";
                                    source.UpdateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress);
                                    version.UpdateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress);
                                    if (changedVersion)
                                    {
                                        source.IdVersion = version.Id;
                                        source.UniqueIdVersion = version.UniqueIdVersion;
                                    }
                                    Manager.SaveOrUpdate(source);
                                    Manager.SaveOrUpdate(version);
                                    Manager.Commit();
                                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.Repository(source.Repository));
                                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersViewOfRepository(source.Repository));
                                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersSizeViewOfRepository(source.Repository));
                                    if (changeTags && ServiceTags != null)
                                    {
                                        ServiceTags.SaveTags(person, source.Repository.IdCommunity, GetIdModule(), ModuleRepository.UniqueCode, tags);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        saving = ItemSaving.None;
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return saving;
                }
                #endregion
           
            #region "Virtual Delete/ undelete"
                public List<liteRepositoryItem> ItemsVirtualDelete(List<long> idItems, ItemAction action, RepositoryType type, Int32 idCommunity)
                {
                    return ItemsVirtualDelete(idItems, (action == ItemAction.virtualdelete) ? BaseStatusDeleted.Manual : BaseStatusDeleted.None, type, idCommunity);
                }
                public liteRepositoryItem ItemVirtualDelete(long idItem, ItemAction action, RepositoryType type, Int32 idCommunity)
                {
                    return ItemVirtualDelete(idItem, (action == ItemAction.virtualdelete) ? BaseStatusDeleted.Manual : BaseStatusDeleted.None, type, idCommunity);
                }
                public liteRepositoryItem ItemVirtualDelete(long idItem, BaseStatusDeleted deleted, RepositoryType type, Int32 idCommunity)
                {
                    List<liteRepositoryItem> results = ItemsVirtualDelete(new List<long>() { idItem }, deleted, type, idCommunity);
                    return (results==null) ? null : results.FirstOrDefault();
                }
                public List<liteRepositoryItem> ItemsVirtualDelete(List<long> idItems, BaseStatusDeleted deleted, RepositoryType type, Int32 idCommunity)
                {
                    List<liteRepositoryItem> items = null;
                    try
                    {
                        litePerson person = GetValidPerson(UC.CurrentUserID);
                        List<liteRepositoryItem> rItems = GetQuery(type, idCommunity).ToList();
                        if (rItems.Any(i => idItems.Contains(i.Id)) && person != null)
                        {
                            items = new List<liteRepositoryItem>();
                            foreach (liteRepositoryItem rItem in rItems.Where(i => idItems.Contains(i.Id)).OrderByDescending(i => i.IsFile))
                            {
                                if (ItemVirtualDelete(person,rItem, deleted, rItems))
                                    items.Add(rItem);
                            }
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.Repository(type, idCommunity));
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersViewOfRepository(type, idCommunity));
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersSizeViewOfRepository(type, idCommunity));
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return items;
                }
                private Boolean ItemVirtualDelete(litePerson person, liteRepositoryItem item, BaseStatusDeleted deleted, List<liteRepositoryItem> allItems)
                {
                    Boolean result = false;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (item != null)
                        {
                            DateTime date = DateTime.Now;
                            if (!isInTransaction)
                                Manager.BeginTransaction();
                            item.UpdateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, date);
                            item.Deleted = deleted;
                            if (deleted == BaseStatusDeleted.None)
                            {
                                if (ItemHasDuplicate(item, item.IdFolder, allItems))
                                    item.Name = ItemGetSuggestedName(item, allItems);
                                RecalculateDisplayOrderForFolder(item.IdFolder, allItems, person.Id, date);
                            }
                            if (item.Type == ItemType.Folder)
                                CascadeVirtualDelete(person.Id, item, allItems, deleted, date);

                            if (item.IdFolder > 0)
                                RecalculateDeletedSize(allItems.Where(i=> i.Id == item.IdFolder).FirstOrDefault(), allItems, true);
                            Manager.SaveOrUpdate(item);
                            if (!isInTransaction)
                                Manager.Commit();
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                            Manager.RollBack();
                        result = false;
                    }
                    return result;
                }
                private void CascadeVirtualDelete(Int32 idPerson,liteRepositoryItem father, IEnumerable<liteRepositoryItem> allItems, BaseStatusDeleted deleted,  DateTime date)
                {
                    foreach (liteRepositoryItem child in allItems.Where(i => i.IdFolder == father.Id )) 
                    {
                        BaseStatusDeleted actual = child.Deleted;
                        child.UpdateMetaInfo(idPerson, UC.IpAddress, UC.ProxyIpAddress, date);
                        child.Deleted = (deleted == BaseStatusDeleted.Manual) ? (actual | BaseStatusDeleted.Cascade) : ((BaseStatusDeleted)((int)actual - (int)BaseStatusDeleted.Cascade));
                        if (child.Type == ItemType.Folder)
                            CascadeVirtualDelete(idPerson,child, allItems.Where(i => i.IdFolder == child.Id && i.Deleted != deleted), deleted, date);
                        Manager.SaveOrUpdate(child);
                    }
                    RecalculateDeletedSize(father, allItems);
                }
                protected void RecalculateDeletedSize(liteRepositoryItem item, IEnumerable<liteRepositoryItem> allItems, Boolean alsoFathers = false)
                {
                    if (item != null)
                    {
                        item.DeletedSize = 0;
                        if (allItems.Any(i => i.IdFolder == item.Id))
                        {
                            item.DeletedSize = allItems.Where(i => i.IdFolder == item.Id && i.Type != ItemType.Folder && i.Deleted != BaseStatusDeleted.None).DefaultIfEmpty(new liteRepositoryItem()).Sum(i => i.SizeWithVersions);
                            item.DeletedSize += allItems.Where(i => i.IdFolder == item.Id && i.Type == ItemType.Folder).DefaultIfEmpty(new liteRepositoryItem()).Sum(i => i.DeletedSize);
                            item.Size = allItems.Where(i => i.IdFolder == item.Id && i.Deleted == BaseStatusDeleted.None).DefaultIfEmpty(new liteRepositoryItem()).Sum(i => i.Size);
                            item.VersionsSize = allItems.Where(i => i.IdFolder == item.Id  && i.Deleted == BaseStatusDeleted.None).DefaultIfEmpty(new liteRepositoryItem()).Sum(i => i.VersionsSize);
                        }
                        if (item.IdFolder > 0 && alsoFathers)
                            RecalculateDeletedSize(allItems.Where(i => i.Id == item.IdFolder).FirstOrDefault(), allItems, alsoFathers);
                    }
                }
                public void RecalculateReposistorySize(long diskAvailableSpace, RepositoryType type, Int32 idCommunity)
                {
                    liteRepositorySettings settings = SettingsGetDefault(type, idCommunity);
                    List<liteRepositoryItem> rItems = GetQuery(type, idCommunity).ToList();
                    RecalculateReposistorySize(settings, diskAvailableSpace,rItems, UC.CurrentUserID, DateTime.Now);
                }

                protected void RecalculateReposistorySize(liteRepositorySettings settings, long diskAvailableSpace, List<liteRepositoryItem> items, Int32 idPerson = -1, DateTime? date = null)
                {
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (idPerson == -1)
                        {
                            litePerson person = GetValidPerson(UC.CurrentUserID);
                            idPerson = (person != null ? person.Id : -1);
                        }
                        if (idPerson != -1 && items != null && items.Any(i => i.Deleted == BaseStatusDeleted.None))
                        {
                            if (!date.HasValue)
                                date = DateTime.Now;
                            if (!isInTransaction)
                                Manager.BeginTransaction();
                            List<liteRepositoryItem> folders = new List<liteRepositoryItem>();
                            foreach (liteRepositoryItem folder in items.Where(i => i.Type == ItemType.Folder && i.Deleted == BaseStatusDeleted.None))
                            {
                                folder.Size = items.Where(i =>  i.Deleted == BaseStatusDeleted.None).Select(i => i.Size).DefaultIfEmpty(0).Sum();
                                folder.VersionsSize = items.Where(i => i.Type != ItemType.Folder && i.Deleted == BaseStatusDeleted.None).Select(i => i.VersionsSize).DefaultIfEmpty(0).Sum();
                                folder.DeletedSize = items.Where(i => i.Type != ItemType.Folder && i.Deleted == BaseStatusDeleted.Manual).Select(i => i.Size + i.VersionsSize).DefaultIfEmpty(0).Sum();
                                RecalculateChildrenSize(folder, items.Where(i => i.Type == ItemType.Folder),idPerson,date);

                                folders.Add(folder);
                            }

                            if (folders.Any())
                                Manager.SaveOrUpdateList(folders);
                            if (!isInTransaction)
                                Manager.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                            Manager.RollBack();
                    }
                }
                private void RecalculateChildrenSize(liteRepositoryItem father, IEnumerable<liteRepositoryItem> items, Int32 idPerson = -1, DateTime? date = null)
                {
                    foreach (liteRepositoryItem folder in items.Where(i => i.Type == ItemType.Folder && i.IdFolder == father.Id))
                    {
                        RecalculateChildrenSize(folder, items.Where(i => i.Type == ItemType.Folder), idPerson, date);

                        folder.Size = items.Where(i => i.Deleted == BaseStatusDeleted.None).Select(i => i.Size).DefaultIfEmpty(0).Sum();
                        folder.VersionsSize = items.Where(i => i.Type != ItemType.Folder && i.Deleted == BaseStatusDeleted.None).Select(i => i.VersionsSize).DefaultIfEmpty(0).Sum();
                        folder.DeletedSize = items.Where(i => i.Type != ItemType.Folder && i.Deleted == BaseStatusDeleted.Manual).Select(i => i.Size + i.VersionsSize).DefaultIfEmpty(0).Sum();

                        folder.DeletedSize += items.Where(i => i.Type == ItemType.Folder && i.IdFolder == father.Id && i.Deleted == BaseStatusDeleted.Manual).Select(i => i.DeletedSize).DefaultIfEmpty(0).Sum();
                        if (folder.DeletedSize < 0)
                            folder.DeletedSize = 0;
                        father.DeletedSize += folder.DeletedSize;
                    }

                       
                }
            #endregion

            #region "Display Order"
                protected void RecalculateDisplayOrderForFolder(long idFolder, List<liteRepositoryItem> items,Int32 idPerson = -1, DateTime? date=null)
                {
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (idPerson==-1){
                            litePerson person = GetValidPerson(UC.CurrentUserID);
                            idPerson = (person != null ? person.Id : -1);
                        }
                        if (idPerson!=-1 &&items != null && items.Any(i => i.IdFolder == idFolder && i.Deleted == BaseStatusDeleted.None))
                        {
                            if (!date.HasValue)
                                date = DateTime.Now;
                            if (!isInTransaction)
                                Manager.BeginTransaction();

                            long displayOrder = 1;
                            List<liteRepositoryItem> itemsToSave = new List<liteRepositoryItem>();
                            foreach (liteRepositoryItem item in items.Where(i => i.IdFolder == idFolder && i.Deleted == BaseStatusDeleted.None).OrderBy(i => i.DisplayOrder).ThenBy(i=> i.IsFile).ThenBy(i=> i.Id))
                            {
                                item.DisplayOrder = displayOrder++;
                                item.UpdateMetaInfo(idPerson, UC.IpAddress, UC.ProxyIpAddress, date.Value);
                                itemsToSave.Add(item);
                            }
                            if (itemsToSave.Any())
                                Manager.SaveOrUpdateList(itemsToSave);
                            if (!isInTransaction)
                                Manager.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                            Manager.RollBack();
                    }
                }


                protected void RecalculateDisplayOrder(List<liteRepositoryItem> items, Int32 idPerson = -1, DateTime? date = null)
                {
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (idPerson == -1)
                        {
                            litePerson person = GetValidPerson(UC.CurrentUserID);
                            idPerson = (person != null ? person.Id : -1);
                        }
                        if (idPerson != -1 && items != null && items.Any(i => i.Deleted == BaseStatusDeleted.None))
                        {
                            if (!date.HasValue)
                                date = DateTime.Now;
                            if (!isInTransaction)
                                Manager.BeginTransaction();

                            RecalculateDisplayOrder(0, items.Where(i => i.Deleted == BaseStatusDeleted.None), idPerson, date);
                            if (!isInTransaction)
                                Manager.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                            Manager.RollBack();
                    }
                }

                private void RecalculateDisplayOrder(long idFolder, IEnumerable<liteRepositoryItem> items, Int32 idPerson = -1, DateTime? date = null, Boolean forInternalUpdate = false )
                {
                    long displayOrder = 1;
                    List<liteRepositoryItem> itemsToSave = new List<liteRepositoryItem>();
                    foreach (liteRepositoryItem item in items.Where(i => i.IdFolder == idFolder && i.Deleted == BaseStatusDeleted.None).OrderBy(i => i.DisplayOrder).ThenBy(i => i.IsFile ?0:1).ThenBy(i => i.Id))
                    {
                        item.DisplayOrder = displayOrder++;
                        if (!forInternalUpdate)
                            item.UpdateMetaInfo(idPerson, UC.IpAddress, UC.ProxyIpAddress, date.Value);
                        itemsToSave.Add(item);
                        if (items.Any(i => i.IdFolder == item.Id && i.Deleted == BaseStatusDeleted.None))
                            RecalculateDisplayOrder(item.Id, items.Where(i => i.IdFolder != i.IdFolder));
                    }
                    if (itemsToSave.Any())
                        Manager.SaveOrUpdateList(itemsToSave);
                }

                private long GetCurrentDisplayOrder(long idFolder, IEnumerable<RepositoryItem> items)
                {
                    long displayOrder = 1;
                    if (items.Any(i => i.IdFolder == idFolder && i.Deleted == BaseStatusDeleted.None))
                        displayOrder = items.Where(i => i.IdFolder == idFolder && i.Deleted == BaseStatusDeleted.None).Select(i => i.DisplayOrder).Max() +1;
                    return displayOrder;
                }
            #endregion

            #region "Add"
                public List<RepositoryItem> FolderAddToRepository(long idFolder, List<dtoFolderName> names, RepositoryType type, Int32 idCommunity)
                {
                    List<RepositoryItem> folders = null;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        litePerson person = GetValidPerson(UC.CurrentUserID);
                        if (person !=null ){
                            DateTime date = DateTime.Now;
                            folders = new List<RepositoryItem>();
                       
                            IEnumerable<RepositoryItem> items = GetFullQuery(type, idCommunity);
                            List<String> brothersFolder = items.Where(i => i.IdFolder == idFolder && i.Type == ItemType.Folder && i.Deleted == BaseStatusDeleted.None).Select(f => f.Name).ToList();

                            foreach(dtoFolderName dto in names.Where(n=> n.IsValid)){
                                long displayOrder = GetCurrentDisplayOrder(idFolder,items);
                                if (ItemHasDuplicate(brothersFolder, dto.Name, ItemType.Folder))
                                {
                                    dto.OriginalName = dto.Name;
                                    dto.Name = ItemGetSuggestedName(brothersFolder, dto.Name, ItemType.Folder);
                                }

                                try
                                {
                                    var queryAssignments = GetQueryAssignments(type, idCommunity).Where(a=> a.Deleted== BaseStatusDeleted.None && a.Inherited);
                                    if (!isInTransaction)
                                        Manager.BeginTransaction();
                      
                                    RepositoryItem item = RepositoryItem.CreateFolder(idFolder, dto, person, date, UC.IpAddress, UC.ProxyIpAddress, type, idCommunity);
                                    item.DisplayOrder = displayOrder;
                                    Manager.SaveOrUpdate(item);
                                    RepositoryItemVersion version = item.CreateFirstVersion();
                                    Manager.SaveOrUpdate(version);
                                    item.IdVersion = version.Id;
                                    Manager.SaveOrUpdate(item);

                                    if (!isInTransaction)
                                        Manager.Commit();

                                    long permissions = (long)((item.AllowUpload) ? ModuleRepository.Base2Permission.DownloadOrPlay | ModuleRepository.Base2Permission.Upload : ModuleRepository.Base2Permission.DownloadOrPlay);
                                    ItemAssignments assignment = AssignmentAddCommunity(item.Id, item.Repository, item.IdCommunity, false, false, permissions, person, date);
                                    if (idFolder == 0)
                                        assignment = AssignmentAddCommunity(item.Id, item.Repository, item.IdCommunity, false, true, permissions, person, date);
                                    else
                                        ApplyInheritedAssignment(item, queryAssignments, permissions, person, date);
                                 
                                    brothersFolder.Add(dto.Name);
                                    dto.Id = item.Id;
                                    folders.Add(item);
                                }
                                catch (Exception ex)
                                {
                                    if (!isInTransaction)
                                        Manager.RollBack();
                                }
                            }
                           
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.Repository(type, idCommunity));
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersViewOfRepository(type, idCommunity));
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersSizeViewOfRepository(type, idCommunity));
                        }
                    }
                    catch (Exception ex)
                    {
                        folders = null;
                    }
                    return folders;
                }
                public List<RepositoryItem> LinkAddToRepository(long idFolder, List<dtoUrlItem> itemsToAdd, RepositoryType type, Int32 idCommunity)
                {
                    List<RepositoryItem> links = null;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        litePerson person = GetValidPerson(UC.CurrentUserID);
                        if (person != null)
                        {
                            DateTime date = DateTime.Now;
                            links = new List<RepositoryItem>();

                            IEnumerable<RepositoryItem> items = GetFullQuery(type, idCommunity);
                            List<String> brothersLinks = items.Where(i => i.IdFolder == idFolder && i.Type == ItemType.Link && i.Deleted == BaseStatusDeleted.None).Select(f => f.DisplayName).ToList();

                            foreach (dtoUrlItem dto in itemsToAdd.Where(n => n.IsValid))
                            {
                                long displayOrder = GetCurrentDisplayOrder(idFolder, items);
                                if (ItemHasDuplicate(brothersLinks, dto.Name, ItemType.Link))
                                {
                                    dto.OriginalName = dto.Name;
                                    dto.Name = ItemGetSuggestedName(brothersLinks, dto.Name, ItemType.Link,"", dto.Address);
                                }

                                try
                                {
                                    var queryAssignments = GetQueryAssignments(type, idCommunity).Where(a => a.Deleted == BaseStatusDeleted.None && a.Inherited);
                                    if (!isInTransaction)
                                        Manager.BeginTransaction();
      
                                    RepositoryItem item = RepositoryItem.CreateLink(idFolder, dto, person, date, UC.IpAddress, UC.ProxyIpAddress, type, idCommunity);
                                    item.DisplayOrder = displayOrder;
                                    Manager.SaveOrUpdate(item);
 
                                    RepositoryItemVersion version = item.CreateFirstVersion();
                                    Manager.SaveOrUpdate(version);
                                    item.IdVersion = version.Id;
                                    Manager.SaveOrUpdate(item);
                                    if (!isInTransaction)
                                        Manager.Commit();

                                    long permissions = (long)ModuleRepository.Base2Permission.DownloadOrPlay;
                                    ItemAssignments assignment = AssignmentAddCommunity(item.Id, item.Repository, item.IdCommunity, false, false, permissions, person, date);
                                    if (idFolder == 0)
                                        assignment = AssignmentAddCommunity(item.Id, item.Repository, item.IdCommunity, false, true, permissions, person, date);
                                    else
                                        ApplyInheritedAssignment(item, queryAssignments, permissions, person, date);
                                   
                                    brothersLinks.Add(dto.Name);
                                    dto.Id = item.Id;
                                    links.Add(item);
                                }
                                catch (Exception ex)
                                {
                                    if (!isInTransaction)
                                        Manager.RollBack();
                                }
                            }

                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.Repository(type, idCommunity));
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersViewOfRepository(type, idCommunity));
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersSizeViewOfRepository(type, idCommunity));
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return links;
                }

                public List<dtoCreatedItem> FileAddToRepository(liteRepositorySettings settings, dtoFolderTreeItem folderQuota, String istance, long idFolder, List<dtoUploadedItem> itemsToAdd, RepositoryType type, Int32 idCommunity)
                {
                    return FileAddToRepository(settings, folderQuota, istance, idFolder, GetFullQuery(type, idCommunity), itemsToAdd, type, idCommunity);
                }
                public List<dtoCreatedItem> FileAddToRepository(liteRepositorySettings settings, dtoFolderTreeItem folderQuota, String istance, long idFolder, IEnumerable<RepositoryItem> rpItems, List<dtoUploadedItem> itemsToAdd, RepositoryType type, Int32 idCommunity)
                {
                    List<dtoCreatedItem> files = null;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        litePerson person = GetValidPerson(UC.CurrentUserID);
                        if (person != null )
                        {
                            DateTime date = DateTime.Now;
                            files = new List<dtoCreatedItem>();
                            List<String> brothersFile = rpItems.Where(i => i.IdFolder == idFolder && i.Type != ItemType.Folder && i.Type != ItemType.Link && i.Deleted == BaseStatusDeleted.None).Select(f => f.DisplayFullName).ToList();
                            RepositoryItem father = rpItems.FirstOrDefault(i => i.Id == idFolder);
                            List<RepositoryItem> fathers = (idFolder == 0) ? new List<RepositoryItem>() : GetFullReverseFathers(idFolder, rpItems);
                            files.AddRange(itemsToAdd.Where(i => !i.IsValid).Select(i => new dtoCreatedItem() { ToAdd = i, Error = ItemUploadError.UnableToSaveFile }).ToList());
                            Dictionary<ItemType, Boolean> defaultDownload = settings.GetDefaultAllowDownload();
                            Dictionary<ItemType, DisplayMode> defaultDisplayMode = settings.GetDefaultDisplayMode();
                            List<litePlayerSettings> players = (itemsToAdd.Any(i => i.IsValid && (i.Type == ItemType.Multimedia || i.Type == ItemType.ScormPackage || i.Type == ItemType.VideoStreaming)) ? PlayerGetSettings() : null);
                            foreach (dtoUploadedItem dto in itemsToAdd.Where(i => i.IsValid))
                            {
                                Boolean allowedByQuota = false;
                                ItemUploadError error = ItemUploadError.None;
                                RepositoryItem item = null;
                                allowedByQuota = folderQuota.ValidateSpace(dto.Size, true);
                                if (allowedByQuota)
                                {
                                    if (idFolder == 0 || father != null)
                                    {
                                        long displayOrder = GetCurrentDisplayOrder(idFolder, rpItems);
                                        if (ItemHasDuplicate(brothersFile, dto.Name, ItemType.File,dto.Extension))
                                        {
                                            dto.OriginalName = dto.DisplayName;
                                            dto.Name = ItemGetSuggestedName(brothersFile, dto.Name, ItemType.File, dto.Extension);
                                        }
                                        try
                                        {
                                            var queryAssignments = GetQueryAssignments(type, idCommunity).Where(a => a.Deleted == BaseStatusDeleted.None && a.Inherited);
                                            if (!isInTransaction)
                                                Manager.BeginTransaction();
                                            item = RepositoryItem.CreateFile(idFolder, dto,defaultDownload[dto.Type], defaultDisplayMode[dto.Type], person, date, UC.IpAddress, UC.ProxyIpAddress, type, idCommunity);
                                            item.DisplayOrder = displayOrder;

                                            item.Thumbnail = dto.ThumbnailFileName;
                                            if (!String.IsNullOrWhiteSpace(dto.ThumbnailFileName))
                                            {
                                                item.AutoThumbnail = true;
                                                item.DisplayMode = DisplayMode.downloadOrPlayOrModal;
                                            }
                                            Manager.SaveOrUpdate(item);
                                            switch (item.Type)
                                            {
                                                case ItemType.VideoStreaming:
                                                case ItemType.ScormPackage:
                                                case ItemType.Multimedia:
                                                    item.IdPlayer = (players!=null ? players.Where(p=> p.Type==item.Type && p.EnableForUse && p.EnableForPlay).OrderByDescending(p=>p.Id).Select(p=> p.Id).FirstOrDefault() : 0);
                                                    break;
                                            }
                                            /// add folder version
                                            RepositoryItemVersion version = item.CreateFirstVersion();
                                            Manager.SaveOrUpdate(version);
                                            item.IdVersion = version.Id;
                                            Manager.SaveOrUpdate(item);

                                            switch (item.Type)
                                            {
                                                case ItemType.VideoStreaming:
                                                case ItemType.ScormPackage:
                                                case ItemType.Multimedia:
                                                    FileTransferAdd(item, dto, person, date);
                                                    break;
                                            }

                                            if (!isInTransaction)
                                                Manager.Commit();
                                            brothersFile.Add(dto.Name);
                                            dto.Id = item.Id;

                                            folderQuota.Size += item.Size;
                                            if (fathers != null && fathers.Any())
                                            {
                                                isInTransaction = isInTransaction || isInTransaction;
                                                if (!isInTransaction)
                                                    Manager.BeginTransaction();
                                                RecalculateFatherSize(fathers, item.Size, item.VersionsSize, person.Id, date);
                                                if (!isInTransaction)
                                                    Manager.Commit();
                                            }


                                            long permissions = (long)ModuleRepository.Base2Permission.DownloadOrPlay;
                                            ItemAssignments assignment = AssignmentAddCommunity(item.Id, item.Repository, item.IdCommunity, false, false, permissions, person, date);
                                            if (idFolder == 0)
                                                assignment = AssignmentAddCommunity(item.Id, item.Repository, item.IdCommunity, false, true, permissions, person, date);
                                            else
                                                ApplyInheritedAssignment(item, GetQuery<ItemAssignments>().Where(a => a.Deleted == BaseStatusDeleted.None && a.Inherited), permissions, person, date);

                                            if (isInTransaction)
                                            {
                                                brothersFile.Add(dto.Name);
                                                dto.Id = item.Id;
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            if (!isInTransaction)
                                                Manager.RollBack();
                                            if (item != null && dto.Id ==0)
                                                item = null;
                                        }
                                    }
                                    else
                                        error = ItemUploadError.UnableToAddFileToUnknownFolder;
                                }
                                else if (folderQuota.ValidateUploadSize(dto.Size))
                                    error = ItemUploadError.UnavailableRepositorySpace;
                                else
                                    error = ItemUploadError.InvalidFileSize;
                                files.Add(new dtoCreatedItem() { Added = item, ToAdd = dto, Error = error });

                            }
                            if (files.Any(i => i.IsValid && i.IsAdded && (i.Added.Type == ItemType.Multimedia || i.Added.Type == ItemType.VideoStreaming || i.Added.Type == ItemType.ScormPackage)))
                            {
                                FileTransferNotifyToTransferService(istance);
                            }
                            if (files.Any(i => i.IsAdded))
                            {
                                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.Repository(type, idCommunity));
                                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersViewOfRepository(type, idCommunity));
                                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersSizeViewOfRepository(type, idCommunity));
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return files;
                }
                protected  void RecalculateFatherSize(List<RepositoryItem> fathers, long size, long versionsize, Int32 idPerson, DateTime date)
                {
                    foreach (RepositoryItem father in fathers)
                    {
                        try{
                            father.Size += size;
                            father.VersionsSize += versionsize;
                            Manager.SaveOrUpdate(father);
                        }
                        catch(Exception ex){

                        }
                    }
                }
                

            #endregion
            #region "Versions"

                private List<liteRepositoryItemVersion> VersionsGet(RepositoryIdentifier identifier, List<long> idVersions)
                {
                    if (idVersions.Count < maxItemsForQuery)
                        return GetQueryVersions(v => idVersions.Contains(v.Id)).ToList();
                    else
                        return GetQueryVersions(identifier).ToList().Where(v => idVersions.Contains(v.Id)).ToList();
                }

                public Dictionary<long, List<String>> FilesGetAllVersionFilesName(List<long> idFiles)
                {
                    if (idFiles == null || !idFiles.Any())
                        return new Dictionary<long, List<String>>();
                    else
                        idFiles = idFiles.Distinct().ToList();
                    if (idFiles.Count <= maxItemsForQuery)
                        return FilesGetVersionPagedFilesName(idFiles);
                    else
                    {
                        Dictionary<long, List<String>> results = new Dictionary<long, List<String>>();
                        Int32 pageIndex = 0;
                        List<long> idPagedItems = idFiles.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                        while (idPagedItems.Any())
                        {
                            Dictionary<long, List<String>> pResults = FilesGetVersionPagedFilesName(idPagedItems);
                            if (pResults != null && pResults.Keys.Any())
                                results = results.Concat(pResults).ToDictionary(l => l.Key, l => l.Value);
                            pageIndex++;
                            idPagedItems = idFiles.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                        }
                        return results;
                    }
                }
                private Dictionary<long, List<String>> FilesGetVersionPagedFilesName(List<long> idFiles)
                {
                    return GetQueryVersions(v => idFiles.Contains(v.IdItem) && v.Type != ItemType.Link).Select(v => new { IdItem = v.IdItem, UniqueIdVersion = v.UniqueIdVersion, Extension = v.Extension }).ToList().GroupBy(v => v.IdItem).ToDictionary(v => v.Key, l => l.Select(v => v.UniqueIdVersion.ToString() + v.Extension).ToList());
                }
                protected dtoCreatedItem FileAddVersion(liteRepositorySettings settings, RepositoryItem item, dtoUploadedItem versionToAdd, dtoFolderTreeItem folderQuota, String istance)
                {
                    return FileAddVersion(settings,item, versionToAdd, folderQuota, istance, GetFullQuery(item.Repository.Type, item.Repository.IdCommunity));
                }
                protected dtoCreatedItem FileAddVersion(liteRepositorySettings settings,RepositoryItem item, dtoUploadedItem versionToAdd, dtoFolderTreeItem folderQuota, String istance, IEnumerable<RepositoryItem> rpItems)
                {
                    dtoCreatedItem fileVersion = null;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        litePerson person = GetValidPerson(UC.CurrentUserID);
                        if (person != null)
                        {
                            if (settings.AllowVersioning)
                            {
                                List<String> mLinks = GetLinkedModules(item.Id);
                                RepositoryItem father = rpItems.FirstOrDefault(i => i.Id == item.IdFolder);
                                Boolean allowedByQuota = item.IsInternal || folderQuota.ValidateSpace(versionToAdd.Size, true);
                                if (allowedByQuota)
                                {
                                    DateTime date = DateTime.Now;
                                    List<RepositoryItem> fathers = (item.IdFolder == 0) ? new List<RepositoryItem>() : GetFullReverseFathers(item.IdFolder, rpItems);
                                    if (!isInTransaction)
                                        Manager.BeginTransaction();

                                    RepositoryItemVersion version = RepositoryItem.CreateNewVersion(item, versionToAdd,person.Id, UC.IpAddress, UC.ProxyIpAddress);
                                    switch (item.Type)
                                    {
                                        case ItemType.Multimedia:
                                        case ItemType.ScormPackage:
                                            List<litePlayerSettings> players = PlayerGetSettings();
                                            item.IdPlayer = (players != null ? players.Where(p => p.Type == item.Type && p.EnableForUse && p.EnableForPlay).OrderByDescending(p => p.Id).Select(p => p.Id).FirstOrDefault() : 0);
                                            version.IdPlayer = item.IdPlayer;
                                            version.Availability = ItemAvailability.transfer;
                                            break;
                                        default:
                                            version.Availability = ItemAvailability.available;
                                            break;
                                    }
                                    Manager.SaveOrUpdate(version);
                                    item.IdVersion = version.Id;
                                    item.UniqueIdVersion = version.UniqueIdVersion;
                                    item.VersionsSize = item.VersionsSize + item.Size;
                                    item.Size = version.Size;
                                    item.Number = version.Number;
                                    if (!String.IsNullOrWhiteSpace(version.Thumbnail))
                                    {
                                        item.AutoThumbnail = true;
                                    }

                                    Manager.SaveOrUpdate(item);
                                    switch (item.Type)
                                    {
                                        case ItemType.VideoStreaming:
                                        case ItemType.ScormPackage:
                                        case ItemType.Multimedia:
                                            FileTransferAdd(item, versionToAdd, person, date);
                                            break;
                                    }
                                    List<RepositoryItemVersion> versions = (from v in Manager.GetIQ<RepositoryItemVersion>()
                                                                            where v.File != null && v.File.Id == item.Id
                                                                                && v.Status == ItemStatus.Active && v.Id != version.Id
                                                                            select v).ToList();
                                        
                                       
                                    if (versions.Any())
                                    {
                                        foreach (RepositoryItemVersion v in versions)
                                        {
                                            v.Status = ItemStatus.Replaced;
                                            v.IsActive = false;
                                            v.UpdateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, date);
                                        }
                                        Manager.SaveOrUpdateList(versions);
                                          
                                    }
                                    if (!isInTransaction)
                                        Manager.Commit();
                                    fileVersion = new dtoCreatedItem() { Added = item, ToAdd = versionToAdd };
                                      

                                    if (fathers != null && fathers.Any())
                                    {
                                        isInTransaction = isInTransaction || isInTransaction;
                                        if (!isInTransaction)
                                            Manager.BeginTransaction();
                                        RecalculateFatherSize(fathers, item.Size, item.VersionsSize, person.Id, date);
                                        if (!isInTransaction)
                                            Manager.Commit();
                                    }

                                }
                                else
                                    fileVersion = new dtoCreatedItem() { Added = null, ToAdd = versionToAdd, Error = ItemUploadError.UnavailableRepositorySpace };
                            }
                            else
                                fileVersion = new dtoCreatedItem() { Added = null, ToAdd = versionToAdd, Error = ItemUploadError.VersioningNotAllowed };


                            if (fileVersion.IsValid && fileVersion.IsAdded && (item.Type == ItemType.Multimedia || item.Type == ItemType.VideoStreaming || item.Type == ItemType.ScormPackage))
                            {
                                FileTransferNotifyToTransferService(istance);
                            }
                            if (fileVersion.IsAdded)
                            {
                                RefreshLiteItem(item.Id, fileVersion.Added.Id );
                                if (!item.IsInternal)
                                {
                                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.Repository(item.Repository.Type, item.Repository.IdCommunity));
                                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersViewOfRepository(item.Repository.Type, item.Repository.IdCommunity));
                                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersSizeViewOfRepository(item.Repository.Type, item.Repository.IdCommunity));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return fileVersion;
                }

                protected void RefreshLiteItem(long idItem,long idVersion=0)
                {
                    liteRepositoryItem lItem = Manager.Get<liteRepositoryItem>(idItem);
                    if (lItem != null)
                        Manager.Refresh(lItem);
                    liteRepositoryItemVersion lVersion = (idVersion==0 ? null :Manager.Get<liteRepositoryItemVersion>(idVersion));
                    if (lVersion != null)
                        Manager.Refresh(lVersion);
                }
                protected void RefreshFullItem(long idItem, long idVersion = 0)
                {
                    RepositoryItem lItem = Manager.Get<RepositoryItem>(idItem);
                    if (lItem != null)
                        Manager.Refresh(lItem);
                    RepositoryItemVersion lVersion = (idVersion == 0 ? null : Manager.Get<RepositoryItemVersion>(idVersion));
                    if (lVersion != null)
                        Manager.Refresh(lVersion);
                }
            #endregion


            #region "Refresh"
                public void ItemsRefreshAvailability(IEnumerable<dtoDisplayRepositoryItem> files, RepositoryIdentifier identifier)
                {
                    List<long> idVersions = files.Select(f => f.IdVersion).ToList();
                    List<liteRepositoryItemVersion> versions = null;

                    identifier = identifier ?? files.Select(f => f.Repository).FirstOrDefault();
                    if (identifier != null)
                    {
                        versions = (from v in Manager.GetIQ<liteRepositoryItemVersion>()
                                    where v.Repository.IdCommunity == identifier.IdCommunity && v.Repository.IdPerson == identifier.IdPerson
                                    && v.Repository.Type == identifier.Type
                                    select v).ToList().Where(v => idVersions.Contains(v.Id)).ToList();
                        if (versions.Any())
                        {
                            versions.ForEach(v => Manager.Refresh(v));
                            Dictionary<long, ItemAvailability> availability = versions.ToDictionary(v => v.Id, v => v.Availability);
                            foreach (dtoDisplayRepositoryItem file in files)
                            {
                                if (availability.ContainsKey(file.IdVersion))
                                {
                                    file.Availability = availability[file.IdVersion];
                                    file.Permissions.EditSettings = file.Permissions.CanEditSettings && dtoDisplayRepositoryItem.IsAvailableByType(file.Type, file.Availability);
                                }
                            }
                        }
                    }
                }
                public void ItemRefreshAvailability(dtoDisplayRepositoryItem file)
                {
                    if (file != null)
                    {
                        liteRepositoryItemVersion version =VersionGet(file.IdVersion);
                        if (version!=null)
                        {
                            Manager.Refresh(version);
                            file.Availability = version.Availability;
                            file.Permissions.EditSettings = file.Permissions.CanEditSettings && dtoDisplayRepositoryItem.IsAvailableByType(file.Type, file.Availability);
                        }
                    }
                }
            #endregion

            #region "Common"
                public RepositoryIdentifier ItemGetRepositoryIdentifier(long idItem)
                {
                    RepositoryIdentifier result = null;
                    try
                    {
                        liteRepositoryItem item = Manager.Get<liteRepositoryItem>(idItem);
                        if (item != null)
                            result = item.Repository;
                    }
                    catch (Exception ex)
                    {

                    }
                    return result;
                }
                protected String ItemGetSuggestedName(liteRepositoryItem item, IEnumerable<liteRepositoryItem> folderItems)
                {
                    List<String> names = new List<String>();
                    switch (item.Type)
                    {
                        case ItemType.Link:
                        case ItemType.Folder:
                            names = folderItems.Where(i => i.Type == item.Type && i.Id != item.Id && i.IdFolder == item.IdFolder && i.Deleted== BaseStatusDeleted.None).Select(i => i.DisplayName.ToLower()).ToList();
                            break;
                        default:
                            names = folderItems.Where(i => i.Type == item.Type && i.Id != item.Id && i.IdFolder == item.IdFolder && i.Deleted == BaseStatusDeleted.None).Select(i => i.Name + i.Extension).Where(s => !String.IsNullOrWhiteSpace(s)).Select(s => s.ToLower()).ToList();
                            break;
                    }
                    return ItemGetSuggestedName(names, item.Name, item.Type, item.Extension, item.Url); ;
                }
                protected  Boolean ItemHasDuplicate(liteRepositoryItem item, long idFolder, IEnumerable<liteRepositoryItem> folderItems)
                {
                    switch (item.Type)
                    {
                        case ItemType.Link:
                            return folderItems.Any(i => i.Type == item.Type && item.Deleted== BaseStatusDeleted.None && i.Id != item.Id && i.IdFolder == idFolder && item.DisplayName == i.DisplayName);
                        case ItemType.Folder:
                            return folderItems.Any(i => i.Type == item.Type && item.Deleted == BaseStatusDeleted.None && i.Id != item.Id && i.IdFolder == idFolder && item.Name == i.Name);
                        default:
                            return folderItems.Any(i => i.Type == item.Type && item.Deleted == BaseStatusDeleted.None && i.Id != item.Id && i.IdFolder == idFolder && item.Name + item.Extension == i.Name + i.Extension);
                    }
                }
                protected String ItemGetSuggestedName(RepositoryItem item, IEnumerable<RepositoryItem> folderItems)
                {
                    List<String> names = new List<String>();
                    switch (item.Type)
                    {
                        case ItemType.Link:
                        case ItemType.Folder:
                            names = folderItems.Where(i => i.Type == item.Type && i.Id != item.Id && i.IdFolder == item.IdFolder && i.Deleted== BaseStatusDeleted.None).Select(i => i.DisplayName.ToLower()).ToList();
                            break;
                        default:
                            names = folderItems.Where(i => i.Type == item.Type && i.Id != item.Id && i.IdFolder == item.IdFolder && i.Deleted == BaseStatusDeleted.None).Select(i => i.Name + i.Extension).Where(s => !String.IsNullOrWhiteSpace(s)).Select(s => s.ToLower()).ToList();
                            break;
                    }

                    return ItemGetSuggestedName(names, item.Name, item.Type, item.Extension, item.Url); ;
                }
                protected Boolean ItemHasDuplicate(RepositoryItem item, long idFolder, IEnumerable<RepositoryItem> folderItems)
                {
                    switch (item.Type)
                    {
                        case ItemType.Link:
                            return folderItems.Any(i => i.Type == item.Type && item.Deleted== BaseStatusDeleted.None && i.Id != item.Id && i.IdFolder == idFolder && item.DisplayName == i.DisplayName);
                        case ItemType.Folder:
                            return folderItems.Any(i => i.Type == item.Type && item.Deleted == BaseStatusDeleted.None && i.Id != item.Id && i.IdFolder == idFolder && item.Name == i.Name);
                        default:
                            return folderItems.Any(i => i.Type == item.Type && item.Deleted == BaseStatusDeleted.None && i.Id != item.Id && i.IdFolder == idFolder && item.Name + item.Extension == i.Name + i.Extension);
                    }
                }
                protected String ItemGetSuggestedName(List<String> names,String name, ItemType type,String extension="", String url="" )
                {
                    String resultName = name;
                    String itemName = name;
                    switch (type)
                    {
                        case ItemType.Link:
                            itemName = (String.IsNullOrWhiteSpace(name) ? url : name);
                            break;
                        case ItemType.Folder:
                            break;
                        default:
                            itemName = name + extension;
                            break;
                    }
                    itemName = String.IsNullOrEmpty(itemName) ? itemName : itemName.ToLower();
                    long index = 1;
                    String prefix = " ({0})";
                    if (names.Any())
                        names = names.Where(n=> !String.IsNullOrWhiteSpace(n)).Select(n=> n.ToLower()).ToList();
                    switch (type)
                    {
                        case ItemType.Link:
                            while (names.Contains(itemName))
                            {
                                resultName = (String.IsNullOrEmpty(name) ? (url + String.Format(prefix, index++)) : (name + String.Format(prefix, index++)));
                                itemName = resultName.ToLower();
                            }
                            break;
                        case ItemType.Folder:
                            while (names.Contains(itemName))
                            {
                                resultName = name + String.Format(prefix, index++);
                                itemName = resultName.ToLower();
                            }
                            break;
                        default:
                            while (names.Contains(itemName))
                            {
                                resultName = name + String.Format(prefix, index++);
                                itemName = resultName.ToLower() + extension;
                            }
                            break;
                    }

                    return resultName;
                }
                protected Boolean ItemHasDuplicate(List<String> names, String name, ItemType type, String extension = "", String url = "")
                {
                    switch (type)
                    {
                        case ItemType.Link:
                            return (String.IsNullOrWhiteSpace(name) ? names.Contains(url) :names.Contains(name));
                        case ItemType.Folder:
                            return names.Contains(name);
                        default:
                            return names.Contains(name + extension);
                    }
                }
            #endregion

        #endregion
    }
}