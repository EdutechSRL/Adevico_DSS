using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.BaseModules;
using lm.Comol.Core.BaseModules.CommunityDiary.Domain;
using lm.Comol.Core.Business;
using lm.Comol.Core.Event.Business;
using lm.Comol.Core.File;
namespace lm.Comol.Core.BaseModules.CommunityDiary.Business
{
    public partial class ServiceCommunityDiary : CoreEventsService, iLinkedService
    {
        private const Int32 maxItemsForQuery = 1500;
        private const string UniqueCode = "SRVLEZ";
        private iApplicationContext _Context;
        private lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository _ServiceRepository;
        private lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository ServiceRepository
        {
            get
            {
                if (_ServiceRepository == null)
                    _ServiceRepository = new lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository(_Context);
                return _ServiceRepository;
            }
        }
        #region initClass
            public ServiceCommunityDiary() { }
            public ServiceCommunityDiary(iApplicationContext oContext)
            {
                Manager = new BaseModuleManager(oContext.DataContext);
                _Context = oContext;
                UC = oContext.UserContext;
                _ServiceRepository = new FileRepository.Business.ServiceRepository(oContext);
            }
            public ServiceCommunityDiary(iDataContext oDC)
            {
                Manager = new BaseModuleManager(oDC);
                _Context = new ApplicationContext();
                _Context.DataContext = oDC;
                this.UC = null;
                _ServiceRepository = new FileRepository.Business.ServiceRepository(_Context);
            }
        #endregion

        private Int32 _ServiceModuleID;
        public Int32 ServiceModuleID()
        {
            if (_ServiceModuleID<=0)
                _ServiceModuleID = Manager.GetModuleID(UniqueCode);
            return _ServiceModuleID;
        }



        #region "Permission"
            public ModuleCommunityDiary GetPermissions(Int32 idPerson, Int32 idCommunity)
            {
                ModuleCommunityDiary module = new ModuleCommunityDiary();
                if (idCommunity == 0)
                {
                    litePerson person = Manager.GetLitePerson(idPerson);
                    if (person != null && person.Id>0)
                        module = ModuleCommunityDiary.CreatePortalmodule(person.TypeID);
                }
                else
                    module = new ModuleCommunityDiary(Manager.GetModulePermission(idPerson, idCommunity, ServiceModuleID()));
                return module;
            }
            public ModuleCommunityDiary ServicePermission(Int32 idPerson, Int32 idCommunity)
            {
                return GetPermissions(idPerson, idCommunity);
            }


            public lm.Comol.Core.FileRepository.Domain.ModuleRepository GetCoreModuleRepository(Int32 idPerson, Int32 idCommunity)
            {
                lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier = lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create((idCommunity > 0 ? lm.Comol.Core.FileRepository.Domain.RepositoryType.Community : lm.Comol.Core.FileRepository.Domain.RepositoryType.Portal), idCommunity);
                return ServiceRepository.GetPermissions(identifier, idPerson);
            }

        #endregion

        #region "Management Item"
            public List<dtoDiaryItem> GetDtoDiaryItems(int idCommunity, Boolean ascendingLesson, ModuleCommunityDiary module, lm.Comol.Core.FileRepository.Domain.ModuleRepository moduleRepository, Boolean allVisibleItems, List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> actions, lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions dAction, String unknownUser)
            {
                List<dtoDiaryItem> items = new List<dtoDiaryItem>();
                litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                CommunityEventType eventType = GetDiaryEventType();
                if (idCommunity>0 && person != null)
                {
                    int lessnoNumber = 1;
                    items = (from item in CommunityEventItemsQuery(idCommunity, eventType, person, allVisibleItems).ToList()
                             select CreateDtoDiaryItem(person, item, allVisibleItems, module, moduleRepository, ref lessnoNumber, actions, dAction, unknownUser)).ToList();
                    if (!ascendingLesson)
                        items = items.OrderByDescending(i => i.LessonNumber).ToList();
                }
                return items;
            }
            public List<CommunityEventItem> GetDiaryItems(Int32 idCommunity, litePerson person, Boolean allVisibleItems)
            {
                CommunityEventType eventType = GetDiaryEventType();
                return CommunityEventItemsQuery(idCommunity, eventType, person, allVisibleItems).ToList();
            }

            private dtoDiaryItem CreateDtoDiaryItem(litePerson person, CommunityEventItem item, Boolean viewAlsoDeleted, ModuleCommunityDiary module, lm.Comol.Core.FileRepository.Domain.ModuleRepository moduleRepository, ref int lessionID, List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> actions, lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions dAction, String unknownUser)
            {
                dtoDiaryItem dtoItem = new dtoDiaryItem();
                dtoItem.CommunityId = item.IdCommunityOwner;
                DescriptionEventItem dObject = EventItemGetDescriptionObject(item.Id);
                if (dObject != null)
                {
                    dtoItem.Description = dObject.Description;
                    dtoItem.DescriptionPlain = dObject.DescriptionPlain;
                }
                else
                {
                    dtoItem.Description = "";
                    dtoItem.DescriptionPlain = "";
                }
                dtoItem.EventItem = item;
                dtoItem.Permission = GetItemPermission(person, item, module, moduleRepository);
                dtoItem.UploadActions = actions;
                dtoItem.DefaultUploadAction = dAction;
                dtoItem.Attachments = AttachmentsGet(person, item, viewAlsoDeleted, dtoItem.Permission, moduleRepository, unknownUser).Where(a => a.Attachment.File != null && a.Attachment.File.IsValid).ToList();
                dtoItem.Id = item.Id;
                dtoItem.IdEvent = (item.EventOwner != null ? item.EventOwner.Id : 0);
                dtoItem.LessonNumber = lessionID;
                lessionID += 1;
                return dtoItem;
            }
            public CoreItemPermission GetItemPermissionFromLink(long idLink)
            {
                CoreItemPermission permission = new CoreItemPermission();
                liteModuleLink link = Manager.Get<liteModuleLink>(idLink);
                if (link != null)
                {
                    EventItemFile itemFileLink = (from f in Manager.GetIQ<EventItemFile>() where f.Link != null && f.Link.Id == idLink && f.Deleted == BaseStatusDeleted.None select f).Skip(0).Take(1).ToList().FirstOrDefault();
                    if (itemFileLink != null)
                    {
                        CommunityEventItem item = EventItemGet(itemFileLink.IdItemOwner);
                        if (item != null)
                            permission = GetItemPermission(item, ServicePermission(UC.CurrentUserID, itemFileLink.IdCommunity), GetCoreModuleRepository(UC.CurrentUserID, itemFileLink.IdCommunity));
                    }
                }
                return permission;
            }
            public Int32 GetCommunityIdFromItemFileLink(long idLink)
            {
                Int32 idCommunity = -1;
                ModuleLink link = Manager.Get<ModuleLink>(idLink);
                if (link != null)
                {
                    Int32 result = (from f in Manager.GetIQ<EventItemFile>() where f.Link != null && f.Link.Id == idLink && f.Deleted == BaseStatusDeleted.None select f.IdCommunity).Skip(0).Take(1).ToList().FirstOrDefault();
                    if (result>0)
                        idCommunity = result;
                }
                return idCommunity;
            }
            public CoreItemPermission GetItemPermission(CommunityEventItem item, ModuleCommunityDiary modulePermissions, lm.Comol.Core.FileRepository.Domain.ModuleRepository moduleRepository)
            {
                litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                return GetItemPermission(person, item, modulePermissions, moduleRepository);
            }

            public CoreItemPermission GetItemPermission(litePerson person, CommunityEventItem item, ModuleCommunityDiary modulePermissions, lm.Comol.Core.FileRepository.Domain.ModuleRepository moduleRepository)
            {
                CoreItemPermission iResponse = new CoreItemPermission();
                if (item != null)
                {
                    Boolean AllowInternalEdit = item.AllowEdit;
                    iResponse.AllowViewFiles = (modulePermissions.ViewDiaryItems && (item.IsVisible || (item.IsVisible == false && item.Owner != null && item.Owner == person))) || modulePermissions.Administration;
                    iResponse.AllowAddFiles = AllowInternalEdit && ((item.Owner != null && item.Owner == person) || modulePermissions.Administration || modulePermissions.UploadFile);
                    iResponse.AllowDelete = AllowInternalEdit && ((modulePermissions.DeleteItem && item.Owner != null && item.Owner == person) || modulePermissions.Administration);
                    iResponse.AllowEdit = AllowInternalEdit && ((modulePermissions.DeleteItem && item.Owner != null && item.Owner == person) || modulePermissions.Administration);
                    iResponse.AllowUnDelete = AllowInternalEdit && ((modulePermissions.DeleteItem && item.Owner != null && item.Owner == person) || modulePermissions.Administration);
                }
                
                iResponse.AllowView = modulePermissions.Administration || modulePermissions.ViewDiaryItems;
                iResponse.AllowVirtualDelete = iResponse.AllowDelete;
                iResponse.AllowViewStatistics = iResponse.AllowViewFiles;
                iResponse.AllowFilePublish = (moduleRepository != null && (moduleRepository.Administration || moduleRepository.UploadFile));
                return iResponse;
            }

            public CommunityEventType GetDiaryEventType()
            {
                return Manager.Get<CommunityEventType>((int)StandardEventType.Lesson);
            }

            public Boolean PhisicalDeleteItem(CommunityEventItem item, String baseFilePath, String baseThumbnailPath)
            {
                Boolean result = (item == null);
                if (item != null)
                {
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        CommunityEvent eventOwner = item.EventOwner;
                        List<string> filesToRemove = new List<string>();
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        List<EventItemFile> attachments = QueryAttachments(a => a.IdItemOwner == item.Id).ToList();
                        List<liteModuleLink> links = attachments.Where(a => a.Link != null).Select(a => a.Link).ToList();

                        foreach (EventItemFile attachment in attachments.Where(a => a.Item != null && a.Item.IsInternal))
                        {
                            filesToRemove.Add(ServiceRepository.GetItemDiskFullPath(baseFilePath, attachment.Item));
                            filesToRemove.Add(ServiceRepository.GetItemThumbnailFullPath(baseThumbnailPath, attachment.Item));
                            Manager.DeletePhysical(attachment.Item);
                            if (attachment.Version != null)
                                Manager.DeletePhysical(attachment.Version);
                        }

                        if (links.Any())
                            Manager.DeletePhysicalList(links);
                        if (attachments.Any())
                            Manager.DeletePhysicalList(attachments);

                        if (eventOwner != null)
                            eventOwner.Items.Remove(item);

                        Manager.DeletePhysical(item);
                        if (eventOwner.Items.Count > 0)
                            Manager.SaveOrUpdate(eventOwner);
                        else
                            Manager.DeletePhysical(eventOwner);
                        if (!isInTransaction)
                            Manager.Commit();
                        Delete.Files(filesToRemove);
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
            public Boolean PhisicalDeleteCommunityDiary(Int32 idCommunity, String baseFilePath, String baseThumbnailPath)
            {
                Boolean deleted = false;
                try
                {
                    CommunityEventType eventType = GetDiaryEventType();
                    Manager.BeginTransaction();
                    List<String> filesToRemove = new List<String>();
 
                    List<CommunityEvent> diaryEvents = (from e in Manager.GetIQ<CommunityEvent>() where e.IdCommunityOwner == idCommunity && e.EventType == eventType && e.AllowEdit == true select e).ToList();
                    List<long> idEvents = diaryEvents.Select(i=> i.Id).ToList();
                    List<EventItemFile> items = null;
                    if (idEvents.Count<= maxItemsForQuery)
                        items = (from f in Manager.GetIQ<EventItemFile>() where f.IdCommunity == idCommunity && idEvents.Contains(f.IdEventOwner) select f).ToList();
                    else
                        items = (from f in Manager.GetIQ<EventItemFile>() where f.IdCommunity == idCommunity select f).ToList().Where(f=> idEvents.Contains(f.IdEventOwner)).ToList();
                    List<liteModuleLink> links = (from f in items where f.Link != null && !f.Item.IsInternal  select f.Link).ToList();
                    if (items.Any())
                    {

                        foreach (EventItemFile attachment in items.Where(i => i.Item.IsInternal))
                        {
                            filesToRemove.Add(ServiceRepository.GetItemDiskFullPath(baseFilePath, attachment.Item));
                            filesToRemove.Add(ServiceRepository.GetItemThumbnailFullPath(baseThumbnailPath, attachment.Item));
                            Manager.DeletePhysical(attachment.Link);
                            Manager.DeletePhysical(attachment.Item);
                            if (attachment.Version != null)
                                Manager.DeletePhysical(attachment.Version);
                        }
                        if (links.Any())
                            Manager.DeletePhysicalList(links);
                        Manager.DeletePhysicalList(items);
                    }
                    else if (links.Any())
                        Manager.DeletePhysicalList(links);
                    if (diaryEvents.Any())
                        Manager.DeletePhysicalList(diaryEvents);

                    Manager.Commit();
                    Delete.Files(filesToRemove);
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
                return deleted;
            }
        #endregion
    }
}