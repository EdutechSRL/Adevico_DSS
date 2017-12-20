using lm.Comol.Core.BaseModules.CommunityDiary.Domain;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace lm.Comol.Core.BaseModules.CommunityDiary.Business
{
    public partial class ServiceCommunityDiary
    {

        public List<RepositoryAttachmentUploadActions> GetAvailableUploadActions(ModuleCommunityDiary modulePermission, ModuleRepository repositoryPermissions)
        {
            List<RepositoryAttachmentUploadActions> actions = new List<RepositoryAttachmentUploadActions>();
            Boolean isModuleAdministrator = modulePermission.Administration | modulePermission.UploadFile | modulePermission.Edit;
            if (isModuleAdministrator)
            {
                actions.Add(RepositoryAttachmentUploadActions.uploadtomoduleitem);
                if (repositoryPermissions != null)
                {
                    if (repositoryPermissions.Administration || repositoryPermissions.UploadFile)
                        actions.Add(RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity);
                    if (repositoryPermissions.ViewItemsList)
                        actions.Add(RepositoryAttachmentUploadActions.linkfromcommunity);
                }
            }
            return actions;
        }

        public List<EventItemFile> AttachmentsAddFiles(long idEvent, long idEventItem, List<dtoModuleUploadedItem> files, Boolean visibleForItem)
        {
            List<EventItemFile> attachments = null;
            Boolean isInTransaction = Manager.IsInTransaction();
            try
            {
                if (!isInTransaction)
                    Manager.BeginTransaction();
                attachments = AttachmentsAddFiles(EventItemGet(idEventItem), files, visibleForItem);
                if (!isInTransaction)
                    Manager.Commit();
            }
            catch (Exception ex)
            {
                if (!isInTransaction)
                    Manager.RollBack();
                attachments = null;
            }
            return attachments;

        }
        public List<EventItemFile> AttachmentsAddFiles(CommunityEventItem eventItem, List<dtoModuleUploadedItem> items, Boolean visibleForItem)
        {
            List<EventItemFile> attachments = null;
            Boolean isInTransaction = Manager.IsInTransaction();
            try
            {
                if (!isInTransaction)
                    Manager.BeginTransaction();
                litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                if (items.Any() && eventItem != null && person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
                {
                    attachments = new List<EventItemFile>();
                    DateTime date = DateTime.Now;
                    foreach (dtoModuleUploadedItem item in items)
                    {
                        EventItemFile attachment = new EventItemFile();
                        attachment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress, date);
                        attachment.IdCommunity= eventItem.IdCommunityOwner;
                        attachment.IdEventOwner = (eventItem.EventOwner != null ? eventItem.EventOwner.Id : 0);
                        attachment.IdItemOwner = eventItem.Id;
                        attachment.Item = item.ItemAdded;
                        attachment.isVisible = visibleForItem;
                        attachment.Owner = person;
                        Manager.SaveOrUpdate(attachment);
                        liteModuleLink link = new liteModuleLink(item.Link.Description, item.Link.Permission, item.Link.Action);
                        link.CreateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, date);
                        link.DestinationItem = (ModuleObject)item.Link.ModuleObject;
                        link.AutoEvaluable = false;
                        link.SourceItem = ModuleObject.CreateLongObject(attachment.Id, attachment, (int)ModuleCommunityDiary.ObjectType.DiaryItemLinkedFile, attachment.IdCommunity, ModuleCommunityDiary.UniqueID, ServiceModuleID());
                        Manager.SaveOrUpdate(link);
                        attachment.Link = link;

                        if (item.ItemAdded.IsInternal)
                        {
                            if (item.ItemAdded.Module == null)
                                item.ItemAdded.Module = new lm.Comol.Core.FileRepository.Domain.ItemModuleSettings();
                            item.ItemAdded.Module.IdObject = attachment.Id;
                            item.ItemAdded.Module.IdObjectType = (int)ModuleCommunityDiary.ObjectType.DiaryItemLinkedFile;
                            Manager.SaveOrUpdate(item.ItemAdded);
                        }

                        Manager.SaveOrUpdate(attachment);
                        attachments.Add(attachment);
                    }
                }
                if (!isInTransaction)
                    Manager.Commit();

            }
            catch (Exception ex)
            {
                if (!isInTransaction)
                    Manager.RollBack();
                attachments = null;
            }
            return attachments;

        }
        public List<EventItemFile> AttachmentsLinkFiles(long idEvent, long idEventItem, List<ModuleActionLink> links, Boolean visibleForItem, Boolean? visibleForRepository)
        {
            List<EventItemFile> attachments = null;
            Boolean isInTransaction = Manager.IsInTransaction();
            try
            {
                if (!isInTransaction)
                    Manager.BeginTransaction();
                CommunityEventItem item = EventItemGet(idEventItem);
                litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                Boolean updateRepositoryItems = false;
                if (links.Any() && item != null && person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
                {
                    DateTime date = DateTime.Now;
                    attachments = new List<EventItemFile>();
                    foreach (ModuleActionLink link in links)
                    {
                        EventItemFile attachment = QueryAttachments(a => a.Deleted == BaseStatusDeleted.None && a.IdItemOwner == idEventItem).ToList().Where(a => a.Item == (liteRepositoryItem)link.ModuleObject.ObjectOwner).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (attachment == null)
                        {
                            attachment = new EventItemFile();
                            attachment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress, date);
                            attachment.Owner = person;
                            attachment.IdCommunity = item.IdCommunityOwner;
                            attachment.IdEventOwner = (item.EventOwner != null ? item.EventOwner.Id : 0);
                            attachment.IdItemOwner = item.Id;
                            attachment.Item = (liteRepositoryItem)link.ModuleObject.ObjectOwner;
                            attachment.isVisible = visibleForItem;
                            attachment.Version = null;
                            Manager.SaveOrUpdate(attachment);
                            if (!attachment.Item.IsInternal && visibleForRepository.HasValue && visibleForRepository.Value != attachment.Item.IsVisible)
                            {
                                attachment.Item.IsVisible = visibleForRepository.Value;
                                Manager.SaveOrUpdate(attachment.Item);
                                updateRepositoryItems = true;
                            }

                            liteModuleLink mLink = new liteModuleLink(link.Description, link.Permission, link.Action);
                            mLink.CreateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, date);
                            mLink.DestinationItem = (ModuleObject)link.ModuleObject;
                            mLink.AutoEvaluable = false;
                            mLink.SourceItem = ModuleObject.CreateLongObject(attachment.Id, attachment, (int)ModuleCommunityDiary.ObjectType.DiaryItemLinkedFile, attachment.IdCommunity, ModuleCommunityDiary.UniqueID, ServiceModuleID());
                            Manager.SaveOrUpdate(mLink);
                            attachment.Link = mLink;
                            Manager.SaveOrUpdate(attachment);
                        }
                        else if (attachment.Deleted != BaseStatusDeleted.None)
                        {
                            attachment.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress, date);
                            if (attachment.Item != null && attachment.Item.IsInternal && attachment.Item.Deleted != BaseStatusDeleted.None)
                            {
                                attachment.Item.Deleted = BaseStatusDeleted.None;
                                attachment.Item.UpdateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, date);
                                Manager.SaveOrUpdate(attachment.Item);
                            }
                            Manager.SaveOrUpdate(attachment);
                        }
                        attachments.Add(attachment);
                    }
                }
                if (!isInTransaction)
                    Manager.Commit();
                if (updateRepositoryItems && attachments.Any(a=> !a.Item.IsInternal))
                {
                    RepositoryIdentifier identifier = attachments.Where(a => !a.Item.IsInternal).Select(a => a.Item.Repository).FirstOrDefault();
                    foreach (EventItemFile attachment in attachments.Where(a=> !a.Item.IsInternal)){
                        Manager.Refresh(Manager.Get<lm.Comol.Core.FileRepository.Domain.RepositoryItem>(attachment.Item.Id));
                    }
                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.FileRepository.Domain.CacheKeys.Repository(identifier));
                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.FileRepository.Domain.CacheKeys.UsersViewOfRepository(identifier));
                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.FileRepository.Domain.CacheKeys.UsersSizeViewOfRepository(identifier));
                }
            }
            catch (Exception ex)
            {
                if (!isInTransaction)
                    Manager.RollBack();
                attachments = null;
            }
            return attachments;

        }
        public List<RepositoryItemLinkBase<long>> AttachmentsGetBaseLinkedFiles( long idEventItem, Boolean loadDeleted = false)
        {
            List<RepositoryItemLinkBase<long>> items = new List<RepositoryItemLinkBase<long>>();
            try
            {
                List<EventItemFile> attachments = QueryAttachments(i => i.Deleted == BaseStatusDeleted.None && i.IdItemOwner == idEventItem).ToList();

                attachments.ForEach(a => items.Add(new RepositoryItemLinkBase<long>()
                {
                    Deleted = a.Deleted,
                    File = new RepositoryItemObject(a.Item),
                    IdObjectLink = a.Id,
                    AlwaysLastVersion = true,
                    IsVisible = a.isVisible
                }));
            }
            catch (Exception ex)
            {

            }
            return items;
        }
        public List<RepositoryItemLink<long>> AttachmentsGetLinkedFiles(String unknownUser, long idEventItem, Boolean loadDeleted = false)
        {
            List<RepositoryItemLink<long>> items = new List<RepositoryItemLink<long>>();
            try
            {
                List<EventItemFile> attachments = QueryAttachments(i => i.Deleted == BaseStatusDeleted.None && i.IdItemOwner == idEventItem).ToList();

                attachments.ForEach(a => items.Add(new RepositoryItemLink<long>()
                {
                    IdCreatedBy = (a.CreatedBy != null ? a.CreatedBy.Id : 0),
                    CreatedBy = (a.CreatedBy != null ? a.CreatedBy.SurnameAndName : unknownUser),
                    CreatedOn = a.CreatedOn,
                    Deleted = a.Deleted,
                    File = new RepositoryItemObject(a.Item),
                    IdObjectLink = a.Id,
                    IdStatus = 0,
                    Link = a.Link,
                    IdModifiedBy = (a.ModifiedBy != null ? a.ModifiedBy.Id : 0),
                    ModifiedOn = a.ModifiedOn,
                    ModifiedBy = (a.ModifiedBy != null ? a.ModifiedBy.SurnameAndName : unknownUser),
                    IdOwner = (a.CreatedBy != null ? a.CreatedBy.Id : 0),
                    Owner = (a.CreatedBy != null ? a.CreatedBy.SurnameAndName : unknownUser),
                    IsVisible = a.isVisible
                }));

            }
            catch (Exception ex)
            {

            }
            List<RepositoryItemLink<long>> orderedFiles;
            orderedFiles = (from f in items where f.File == null || f.Link == null orderby f.CreatedOn select f).ToList();
            orderedFiles.AddRange((from f in items where f.File != null && f.Link != null orderby f.File.DisplayName select f).ToList());
            return orderedFiles;
        }

        private IQueryable<EventItemFile> QueryAttachments(Expression<Func<EventItemFile, bool>> conditions)
        {
            return (from i in Manager.GetIQ<EventItemFile>() select i).Where(conditions);
        }
        private Dictionary<Int32, String> GetUsers(List<Int32> idCreators, List<Int32> idOwners, List<Int32> idModifiers, String unknownUser)
        {
            List<Int32> idUsers = new List<Int32>();
            idUsers.AddRange(idCreators);
            idUsers.AddRange(idOwners);
            idUsers.AddRange(idModifiers);
            return GetUsers(idUsers.Distinct().ToList(), unknownUser);
        }
        private Dictionary<Int32, String> GetUsers(List<Int32> idUsers, String unknownUser)
        {
            List<litePerson> persons = Manager.GetLitePersons(idUsers);
            return idUsers.ToDictionary(i => i, i => persons.Where(p => p.Id == i).Select(p => p.SurnameAndName).DefaultIfEmpty(unknownUser).FirstOrDefault());
        }

        public Boolean AttachmentPhisicalDelete(CommunityEventItem item, EventItemFile attachment , String baseFilePath, String baseThumbnailPath)
        {
            Boolean result = (attachment == null);
            if (item != null && attachment!=null)
            {
                litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                Boolean isInTransaction = Manager.IsInTransaction();
                try
                {
                    List<string> filesToRemove = new List<string>();
                    if (!isInTransaction)
                        Manager.BeginTransaction();
                    if (attachment.Item != null && attachment.Item.IsInternal)
                    {
                        filesToRemove.Add(ServiceRepository.GetItemDiskFullPath(baseFilePath, attachment.Item));
                        filesToRemove.Add(ServiceRepository.GetItemThumbnailFullPath(baseThumbnailPath, attachment.Item));
                        Manager.DeletePhysical(attachment.Item);
                        if (attachment.Version != null)
                            Manager.DeletePhysical(attachment.Version);
                    }

                    if (attachment.Link !=null)
                        Manager.DeletePhysical(attachment.Link);
                    Manager.DeletePhysical(attachment);

                    item.ModifiedOn = DateTime.Now;
                    item.ModifiedBy = person;
                    Manager.SaveOrUpdate(item);

                    if (!isInTransaction)
                        Manager.Commit();
                    lm.Comol.Core.File.Delete.Files(filesToRemove);
                }
                catch (Exception ex)
                {
                    if (!isInTransaction)
                        Manager.RollBack();
                    result = false;
                }
            }
            return result;
        }
    }
}