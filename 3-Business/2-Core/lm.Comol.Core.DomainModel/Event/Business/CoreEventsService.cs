using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

#region "impersonate declarations"
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Security;
using System.Security.Principal;
using System.Security.Permissions;
using System.Configuration;
using lm.Comol.Core.Events.Domain;
#endregion

namespace lm.Comol.Core.Event.Business
{
    public class CoreEventsService : CoreServices
    {
        private iApplicationContext _Context;
        #region initClass
        public CoreEventsService() { }
        public CoreEventsService(iApplicationContext oContext)
        {
            this.Manager = new BaseModuleManager(oContext.DataContext);
            _Context = oContext;
            this.UC = oContext.UserContext;
        }
        public CoreEventsService(iDataContext oDC)
        {
            this.Manager = new BaseModuleManager(oDC);

            _Context = new ApplicationContext();
            _Context.DataContext = oDC;
            this.UC = null;
        }
        #endregion

        #region "Events"

        public CommunityEventItem EventItemGet(long idItem)
        {
            CommunityEventItem oItem = null;
            try
            {
                oItem = Manager.Get<CommunityEventItem>(idItem);
            }
            catch (Exception ex)
            {
            }
            return oItem;
        }
        public string EventItemGetDescription(long ItemID)
        {
            string iResponse = "";

            try
            {
                iResponse = (from p in Manager.Linq<DescriptionEventItem>() where p.Id == ItemID select p.Description).FirstOrDefault();
            }
            catch (Exception ex)
            {
            }
            return iResponse;
        }
        public string EventItemGetDescription(CommunityEventItem oItem)
        {
            return EventItemGetDescription(oItem.Id);
        }
        public DescriptionEventItem EventItemGetDescriptionObject(long idEventItem)
        {
            return (from p in Manager.Linq<DescriptionEventItem>() where p.Id == idEventItem select p).Skip(0).Take(1).ToList().FirstOrDefault();
        }

        public List<CommunityEventItem> EventItemsGet(Int32 idCommunity, List<long> idItems)
        {
            return (from CommunityEventItem i in Manager.GetIQ<CommunityEventItem>() where idItems.Contains(i.Id) select i).ToList();
        }

        #region "File Item Link"

        public List<dtoAttachmentItem> AttachmentsGet(litePerson person, CommunityEventItem item, Boolean viewAlsoDeleted, CoreItemPermission permissions, lm.Comol.Core.FileRepository.Domain.ModuleRepository moduleRepository, String unknownUser)
        {
            List<dtoAttachmentItem> items = null;
            try
            {
                if (item != null)
                {
                    items = new List<dtoAttachmentItem>();
                    List<dtoAttachment> attachments = AttachmentsGet(person, item, viewAlsoDeleted, unknownUser);
                    items = attachments.Where(a => a.IdModuleLink > 0).OrderBy(a => a.DisplayOrder).ThenBy(a => a.DisplayName).Select(a => new dtoAttachmentItem() { Attachment = a, Permissions = AttachmentGetPermissions(person, a, permissions, moduleRepository) }).ToList();
                }
            }
            catch (Exception ex)
            {
                items = null;
            }
            return items;
        }
        private List<dtoAttachment> AttachmentsGet(litePerson person, CommunityEventItem item, Boolean viewAlsoDeleted, String unknownUser)
        {
            List<dtoAttachment> items = (from fi in Manager.GetAll<EventItemFile>(fi => fi.IdItemOwner == item.Id && (viewAlsoDeleted || (!viewAlsoDeleted && fi.Deleted == BaseStatusDeleted.None)))
                                         select new dtoAttachment(fi, unknownUser)).ToList();

            List<dtoAttachment> orderedFiles = (from f in items where f.File == null || f.Link == null orderby f.CreatedOn select f).ToList();
            orderedFiles.AddRange((from f in items where f.File != null && f.Link != null orderby f.File.DisplayName select f).ToList());
            return orderedFiles;
        }
        private dtoAttachmentPermission AttachmentGetPermissions(litePerson person, dtoAttachment attachment, CoreItemPermission permissions, lm.Comol.Core.FileRepository.Domain.ModuleRepository moduleRepository)
        {
            dtoAttachmentPermission result = new dtoAttachmentPermission();
            if (attachment.File != null)
            {
                Boolean fileRepositoryOwner = (moduleRepository.Administration || (moduleRepository.EditOthersFiles || (moduleRepository.EditMyFiles && attachment.File.IdOwner == person.Id)));

                result.Download = (attachment.File != null && (attachment.File.IsDownloadable || attachment.File.Type == FileRepository.Domain.ItemType.Link)) && permissions.AllowViewFiles;
                result.Play = (attachment.File != null && (attachment.File.Type == FileRepository.Domain.ItemType.Multimedia || attachment.File.Type == FileRepository.Domain.ItemType.ScormPackage || attachment.File.Type == FileRepository.Domain.ItemType.VideoStreaming)) && permissions.AllowViewFiles;
                result.EditVisibility = permissions.AllowEdit;
                switch (attachment.File.Type)
                {
                    case FileRepository.Domain.ItemType.Multimedia:
                    case FileRepository.Domain.ItemType.ScormPackage:
                        result.ViewMyStatistics = permissions.AllowViewStatistics;
                        result.SetMetadata = permissions.AllowEdit && (attachment.File.IsInternal || fileRepositoryOwner);
                        result.ViewOtherStatistics = permissions.AllowEdit && (attachment.File.IsInternal || fileRepositoryOwner);
                        break;

                    case FileRepository.Domain.ItemType.Link:
                        result.Play = result.Download;
                        break;
                }
                result.Edit = false;

                result.EditRepositoryVisibility = permissions.AllowEdit && fileRepositoryOwner && !attachment.File.IsInternal;
                result.EditRepositoryPermission = permissions.AllowEdit && fileRepositoryOwner && !attachment.File.IsInternal;
                result.Delete = attachment.File.IsInternal && permissions.AllowDelete && (attachment.Deleted != BaseStatusDeleted.None); ;
                result.UnDelete = attachment.File.IsInternal && (attachment.Deleted != BaseStatusDeleted.None) && (permissions.AllowUnDelete);
                result.VirtualDelete = attachment.File.IsInternal && (attachment.Deleted == BaseStatusDeleted.None) && permissions.AllowUnDelete;
                result.Unlink = (!attachment.File.IsInternal && permissions.AllowAddFiles);
                result.Publish = permissions.AllowFilePublish && attachment.File.IsInternal && attachment.Deleted == BaseStatusDeleted.None;
            }
            return result;
        }

        public EventItemFile EventItemGetAttachment(long idAttachment)
        {
            EventItemFile attachment = null;
            try
            {
                attachment = Manager.Get<EventItemFile>(idAttachment);
            }
            catch (Exception ex)
            {
            }
            return attachment;
        }
        public void AttachmentEditVisibility(long IdItem, long idAttachment, Boolean visibleForModule, Boolean visibleForRepository)
        {
            AttachmentEditVisibility(Manager.Get<CommunityEventItem>(IdItem), Manager.Get<EventItemFile>(idAttachment), visibleForModule, visibleForRepository);
        }
        public void AttachmentEditVisibility(CommunityEventItem item, EventItemFile attachment, Boolean visibleForModule, Boolean visibleForRepository)
        {
            Boolean updateRepositoryItems = false;
            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
            if (person != null && attachment != null)
            {
                try
                {
                    Manager.BeginTransaction();
                    attachment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    attachment.isVisible = visibleForModule;
                    if (attachment.Item != null & !attachment.Item.IsInternal && attachment.Item.IsVisible != visibleForRepository)
                    {
                        attachment.Item.UpdateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, attachment.ModifiedOn);
                        attachment.Item.IsVisible = visibleForRepository;
                        Manager.SaveOrUpdate(attachment.Item);
                        updateRepositoryItems = true;
                    }

                    Manager.SaveOrUpdate(attachment);
                    item.ModifiedBy = attachment.ModifiedBy;
                    item.ModifiedOn = attachment.ModifiedOn.Value;
                    Manager.SaveOrUpdate(item);
                    Manager.Commit();
                    if (updateRepositoryItems)
                    {
                        Manager.Refresh(Manager.Get<lm.Comol.Core.FileRepository.Domain.RepositoryItem>(attachment.Item.Id));
                        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.FileRepository.Domain.CacheKeys.Repository(attachment.Item.Repository));
                        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.FileRepository.Domain.CacheKeys.UsersViewOfRepository(attachment.Item.Repository));
                        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.FileRepository.Domain.CacheKeys.UsersSizeViewOfRepository(attachment.Item.Repository));
                    }
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
            }
        }

        public void AttachmentVirtualDelete(long idAttachment)
        {
            AttachmentSetVirtualDelete(idAttachment, true);
        }
        public void AttachmentVirtualUndelete(long idAttachment)
        {
            AttachmentSetVirtualDelete(idAttachment, false);
        }
        public void AttachmentsVirtualDelete(List<long> idAttachments)
        {
            AttachmentsSetVirtualDelete(idAttachments, true);
        }
        public void AttachmentsVirtualUndelete(List<long> idAttachments)
        {
            AttachmentsSetVirtualDelete(idAttachments, false);
        }
        private void AttachmentSetVirtualDelete(long idAttachment, Boolean delete)
        {
            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
            EventItemFile attachment = Manager.Get<EventItemFile>(idAttachment);
            if (person != null && attachment != null)
            {
                try
                {
                    Manager.BeginTransaction();
                    attachment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    attachment.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                    CommunityEventItem item = EventItemGet(attachment.IdItemOwner);
                    if (item != null)
                    {
                        item.ModifiedBy = attachment.ModifiedBy;
                        item.ModifiedOn = attachment.ModifiedOn.Value;
                        Manager.SaveOrUpdate(item);
                    }
                    Manager.SaveOrUpdate(attachment);
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
            }

        }
        private void AttachmentsSetVirtualDelete(List<long> idAttachments, Boolean delete)
        {
            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
            List<EventItemFile> attachments = (from f in Manager.Linq<EventItemFile>() where idAttachments.Contains(f.Id) select f).ToList();
            if (person != null && attachments.Count > 0)
            {
                List<CommunityEventItem> items = EventItemsGet(attachments.Select(f => f.IdCommunity).FirstOrDefault(), attachments.Select(f => f.IdItemOwner).Distinct().ToList());
                try
                {

                    Manager.BeginTransaction();
                    foreach (EventItemFile attachment in attachments)
                    {
                        attachment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        attachment.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                        CommunityEventItem item = items.Where(i => i.Id == attachment.IdItemOwner).FirstOrDefault();
                        if (item != null)
                        {
                            item.ModifiedBy = attachment.ModifiedBy;
                            item.ModifiedOn = attachment.ModifiedOn.Value;
                            Manager.SaveOrUpdate(item);
                        }
                        Manager.SaveOrUpdate(attachment);
                    }

                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
            }
        }

        public Boolean UnLinkAttachment(long idAttachment)
        {
            return UnLinkAttachment(idAttachment, Manager.GetLitePerson(UC.CurrentUserID));
        }
        public Boolean UnLinkAttachment(long idAttachment, litePerson person)
        {
            Boolean unlinked = false;
            EventItemFile attachment = Manager.Get<EventItemFile>(idAttachment);
            if (person != null && person.Id > 0 && attachment != null && (attachment.Item == null || !attachment.Item.IsInternal))
            {
                try
                {
                    CommunityEventItem item = Manager.Get<CommunityEventItem>(attachment.IdItemOwner);
                    Manager.BeginTransaction();
                    if (item != null)
                    {
                        item.ModifiedBy = person;
                        item.ModifiedOn = DateTime.Now;
                        Manager.SaveOrUpdate(item);
                    }
                    Manager.DeleteGeneric(attachment);
                    Manager.Commit();
                    unlinked = true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
            }
            return unlinked;
        }

        #endregion

        #region "Save Event / event Item"
        private CommunityEvent CreateGenericEvent(Int32 idCommunity, CommunityEventItem item, CommunityEventType eventType, litePerson person)
        {
            CommunityEvent oEventItem = new CommunityEvent();
            oEventItem.IdCommunityOwner = idCommunity;
            oEventItem.ForEver = false;
            oEventItem.IsMacro = false;
            oEventItem.IsRepeat = false;
            oEventItem.IsVisible = item.IsVisible;
            oEventItem.Items = new List<CommunityEventItem>();
            oEventItem.Link = item.Link;

            if (item.ModifiedOn.Equals(DateTime.MinValue))
            {
                oEventItem.ModifiedOn = DateTime.Now;
                item.ModifiedOn = oEventItem.ModifiedOn;
            }
            else
                oEventItem.ModifiedOn = item.ModifiedOn;
            oEventItem.Name = "";
            ////DiaryItem.Title
            oEventItem.Note = item.Note;
            oEventItem.NotePlain = item.NotePlain;
            oEventItem.Owner = person;
            oEventItem.Place = item.Place;
            oEventItem.Year = item.StartDate.Year;
            oEventItem.EventType = eventType;
            if (item.StartDate.Month < 9)
                oEventItem.Year = oEventItem.Year - 1;
            return oEventItem;
        }
        public CommunityEvent AddMultipleItems(Int32 idCommunity, CommunityEventItem dtoItem, String description, String descriptionPlain, int ownerId, CommunityEventType eventType, DateTime startDate, DateTime endDate, List<dtoWeekDay> weekDays)
        {
            CommunityEvent communityEvent = null;
            try
            {
                litePerson person = Manager.GetLitePerson(ownerId);
                liteCommunity community = Manager.GetLiteCommunity(idCommunity);
                if ((community != null && idCommunity > 0) && person != null)
                {
                    List<dtoWeekDayRecurrence> itemsToInsert = dtoWeekDayRecurrence.CreateRecurrency(startDate, endDate, weekDays);
                    if (itemsToInsert.Count > 0)
                    {
                        Manager.BeginTransaction();
                        communityEvent = CreateGenericEvent(idCommunity, dtoItem, eventType, person);
                        Manager.SaveOrUpdate(communityEvent);
                        foreach (dtoWeekDayRecurrence recurrence in itemsToInsert)
                        {
                            CommunityEventItem eventItem = new CommunityEventItem() { IdCommunityOwner = idCommunity, EventOwner = communityEvent, Owner = person, CreatedBy = person, CreatedOn = DateTime.Now };
                            eventItem.ModifiedBy = eventItem.CreatedBy;
                            eventItem.ModifiedOn = eventItem.CreatedOn;
                            eventItem.Note = dtoItem.Note;
                            eventItem.NotePlain = dtoItem.NotePlain;
                            eventItem.Place = dtoItem.Place;
                            eventItem.Title = dtoItem.Title;
                            eventItem.StartDate = recurrence.StartDate;
                            eventItem.EndDate = recurrence.EndDate;
                            eventItem.ShowDateInfo = dtoItem.ShowDateInfo;
                            eventItem.IsVisible = dtoItem.IsVisible;
                            eventItem.Link = dtoItem.Link;
                            eventItem.AllowEdit = true;
                            Manager.SaveOrUpdate(eventItem);
                            if (!String.IsNullOrEmpty(description))
                            {
                                DescriptionEventItem descriptionItem = new DescriptionEventItem() { Id = eventItem.Id, Description = description };
                                descriptionItem.DescriptionPlain = descriptionPlain;
                                Manager.SaveOrUpdate(descriptionItem);
                            }
                            communityEvent.Items.Add(eventItem);
                        }
                        Manager.SaveOrUpdate(communityEvent);
                        Manager.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                communityEvent = null;
            }
            return communityEvent;
        }
        public CommunityEventItem SaveEventItem(int idCommunity, CommunityEventItem unsavedItem, String description, String descriptionPlain, int ownerId, int savedById, CommunityEventType eventType)
        {
            CommunityEventItem eventItem = null;

            litePerson owner = Manager.GetLitePerson(ownerId);
            litePerson savedBy = Manager.GetLitePerson(savedById);
            liteCommunity community = Manager.GetLiteCommunity(idCommunity);
            CommunityEvent eventOwner = unsavedItem.EventOwner;
            try
            {
                if (owner != null && savedBy != null && (community != null && idCommunity > 0))
                {
                    Manager.BeginTransaction();
                    if (unsavedItem.Id == 0)
                    {
                        eventItem = new CommunityEventItem();
                        eventItem.Owner = owner;
                        eventItem.CreatedBy = savedBy;
                        eventItem.CreatedOn = DateTime.Now;
                        eventItem.ModifiedBy = savedBy;
                        eventItem.ModifiedOn = eventItem.CreatedOn;
                        eventItem.IdCommunityOwner = idCommunity;
                        eventItem.AllowEdit = true;
                    }
                    else
                    {
                        eventItem = Manager.Get<CommunityEventItem>(unsavedItem.Id);
                        if (eventItem.Owner == null)
                        {
                            eventItem.Owner = savedBy;
                        }
                        eventItem.ModifiedBy = savedBy;
                        eventItem.ModifiedOn = DateTime.Now;
                    }
                    eventItem.Note = unsavedItem.Note;
                    eventItem.NotePlain = unsavedItem.NotePlain;
                    eventItem.Place = unsavedItem.Place;
                    eventItem.Title = unsavedItem.Title;
                    eventItem.StartDate = unsavedItem.StartDate;
                    eventItem.EndDate = unsavedItem.EndDate;
                    eventItem.ShowDateInfo = unsavedItem.ShowDateInfo;
                    eventItem.IsVisible = unsavedItem.IsVisible;
                    eventItem.Link = unsavedItem.Link;

                    if (unsavedItem.Id == 0)
                    {
                        eventOwner = CreateGenericEvent(idCommunity, eventItem, eventType, savedBy);
                        Manager.SaveOrUpdate(eventOwner);
                        eventItem.EventOwner = eventOwner;
                        if (eventOwner.Items == null)
                            eventOwner.Items = new List<CommunityEventItem>();
                        eventOwner.Items.Add(eventItem);
                        Manager.SaveOrUpdate(eventItem);
                        Manager.SaveOrUpdate(eventOwner);
                    }
                    else
                        Manager.SaveOrUpdate(eventItem);

                    DescriptionEventItem descriptionItem = Manager.Get<DescriptionEventItem>(eventItem.Id);
                    if (descriptionItem == null && !string.IsNullOrEmpty(description))
                        descriptionItem = new DescriptionEventItem() { Id = eventItem.Id };
                    if (descriptionItem != null)
                    {
                        descriptionItem.Description = description;
                        descriptionItem.DescriptionPlain = descriptionPlain;
                        Manager.SaveOrUpdate(descriptionItem);
                    }
                    Manager.Commit();
                }
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                eventItem = null;
            }

            return eventItem;
        }
        protected IEnumerable<CommunityEventItem> CommunityEventItemsQuery(Int32 idCommunity, CommunityEventType eventType, litePerson person, Boolean allVisibleItems)
        {
            IEnumerable<CommunityEventItem> query = (from item in Manager.GetAll<CommunityEventItem>(i => i.IdCommunityOwner == idCommunity && i.EventOwner != null && i.EventOwner.EventType == eventType
                                    && (allVisibleItems || (i.IsVisible || (!i.IsVisible && i.Owner != null && i.Owner.Id == person.Id))))
                                                     orderby item.StartDate, item.CreatedOn
                                                     select item);
            return query;
        }

        public void EditEventItemVisibility(long IdItem)
        {
            EditEventItemVisibility(Manager.Get<CommunityEventItem>(IdItem));
        }
        public void EditEventItemVisibility(CommunityEventItem item)
        {
            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
            if (person != null && item != null)
            {
                try
                {
                    Manager.BeginTransaction();
                    item.IsVisible = !item.IsVisible;
                    item.ModifiedBy = person;
                    item.ModifiedOn = DateTime.Now;
                    Manager.SaveOrUpdate(item);
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
            }

        }

        #endregion

        #endregion
    }
}