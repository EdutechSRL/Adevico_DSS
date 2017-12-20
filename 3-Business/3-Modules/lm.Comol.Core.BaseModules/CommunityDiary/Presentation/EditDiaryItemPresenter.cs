using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.CommunityDiary.Domain;
using lm.Comol.Core.BaseModules.CommunityDiary.Business;
using lm.Comol.Core.Business;
using lm.Comol.Core.Events.Domain;

namespace lm.Comol.Core.BaseModules.CommunityDiary.Presentation
{
    public class EditDiaryItemPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {

        #region "Standard Methods"
            private int _ModuleID;
            private ServiceCommunityDiary _Service;
            private lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository _ServiceRepository;

            private Int32 GetIdModule()
            {
                if (_ModuleID <= 0)
                    _ModuleID = Service.ServiceModuleID();
                return _ModuleID;
            }

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IviewDiaryItem View
            {
                get { return (IviewDiaryItem)base.View; }
            }

            private ServiceCommunityDiary Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceCommunityDiary(AppContext);
                    return _Service;
                }
            }
            private lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository ServiceRepository
            {
                get
                {
                    if (_ServiceRepository == null)
                        _ServiceRepository = new lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository(AppContext);
                    return _ServiceRepository;
                }
            }

            public EditDiaryItemPresenter(iApplicationContext oContext):base(oContext){
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EditDiaryItemPresenter(iApplicationContext oContext, IviewDiaryItem view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idCommunity, long idItem, String unknownUser, Boolean isForAdd = false)
        {
            idCommunity = (idCommunity == 0 ? UserContext.CurrentCommunityID : idCommunity);
            if (!UserContext.isAnonymous)
            {
                litePerson person = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                CommunityEventItem item = (idItem> 0  ? Service.EventItemGet(idItem) :null);
                if (item!=null && idCommunity != item.IdCommunityOwner)
                    idCommunity = item.IdCommunityOwner;
                View.IdCommunityDiary = idCommunity;
                View.IdModuleCommunityDiary = GetIdModule();
                View.IdModuleRepository = ServiceRepository.GetIdModule();
                liteCommunity community = CurrentManager.GetLiteCommunity(idCommunity);
                if ((community == null && idCommunity > 0) || (!isForAdd && idItem == 0))
                    View.NoPermission(idCommunity, GetIdModule());
                else
                {
                    if (item == null && !isForAdd)
                        View.ShowNoItemWithThisID(idCommunity, GetIdModule(), idItem);
                    else
                    {
                        long idEvent = ((item!=null && item.EventOwner != null )? item.EventOwner.Id : 0);
                        View.CurrentIdItem = idItem;
                        View.CurrentIdEvent = idEvent;
                        ModuleCommunityDiary module = Service.GetPermissions(UserContext.CurrentUserID, idCommunity);
                        if ((module.AddItem && isForAdd) || (module.Administration || module.Edit)){
                            String description = "";
                            if (idItem == 0)
                            {
                                item = new CommunityEventItem();
                                item.Title = "";
                                item.Note = "";
                                item.Owner = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                                item.ModifiedOn = DateTime.Now;
                                item.StartDate = new DateTime(item.ModifiedOn.Year, item.ModifiedOn.Month, item.ModifiedOn.Day, 8, 0, 0);
                                item.EndDate = new DateTime(item.ModifiedOn.Year, item.ModifiedOn.Month, item.ModifiedOn.Day, 11, 0, 0);
                                item.IsVisible = true;
                                item.Link = "";
                                item.Place = "";
                                item.Title = "";
                                item.IdCommunityOwner = idCommunity;
                                item.ShowDateInfo = true;
                            }
                            else
                                description = Service.EventItemGetDescription(item);

                            lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier = lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create((idCommunity > 0 ? lm.Comol.Core.FileRepository.Domain.RepositoryType.Community : lm.Comol.Core.FileRepository.Domain.RepositoryType.Portal), idCommunity);
                           
                            View.RepositoryIdentifier = identifier;
                            String communityName = (community != null ? community.Name : View.GetPortalNameTranslation());
                            List<dtoAttachmentItem> attachments = null;
                            if (idItem > 0)
                            {
                                lm.Comol.Core.FileRepository.Domain.ModuleRepository moduleRepository = ServiceRepository.GetPermissions(identifier, UserContext.CurrentUserID);
                                List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> availableActions = Service.GetAvailableUploadActions(module, moduleRepository);
                                lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions dAction = (availableActions == null || !availableActions.Any()) ? lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.none : availableActions.FirstOrDefault();
                                View.InitializeAttachmentsControl(idEvent, idItem, availableActions, dAction);

                                attachments = Service.AttachmentsGet(person, item, true, Service.GetItemPermission(person, item, module, moduleRepository), moduleRepository, unknownUser);
                                View.AllowEdit = true;
                                View.AllowFileManagement = availableActions.Any();
                            }
                            else
                            {
                                View.AllowEdit = module.Administration || module.AddItem;
                                View.AllowFileManagement = false;
                            }
                            View.LoadItem(item, description, communityName, attachments);
                        }
                        else
                            View.NoPermission(idCommunity, GetIdModule());
                    }
                    View.SetBackToDiary(idCommunity, idItem);
                }
            }
            else
                View.DisplaySessionTimeout(idCommunity, idItem);
        }

        public void SaveItem(Int32 idCommunity, long idItem, CommunityEventItem dto, String description, String descriptionPlain)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(idCommunity, idItem);
            else
            {
                CommunityEventItem item = SaveDiaryItem(idCommunity, idItem, dto, description, descriptionPlain);
                if (item == null)
                    View.SetBackToDiary(idCommunity, idItem);
                else
                {
                    View.CurrentIdItem = item.Id;
                    View.SendToItemsList(idCommunity, item.Id);
                }
            }
        }
        private CommunityEventItem SaveDiaryItem(Int32 idCommunity, long idItem, CommunityEventItem dto, String description, String descriptionPlain)
        {
            CommunityEventItem previousItem = null;
            CommunityEventItem savedItem = null;
            bool NotifyEdit = false;

            if (dto.StartDate > dto.EndDate)
                dto.EndDate = (dto.ShowDateInfo ? dto.StartDate : dto.StartDate.AddHours(2));
            DateTime oldStartDate = dto.StartDate;
            DateTime oldEndDate = dto.EndDate;
            if (idItem == 0)
                dto.Id = 0;
            else
            {
                dto.Id = idItem;
                previousItem = Service.EventItemGet(idItem);
                if (previousItem != null)
                {
                    oldStartDate = previousItem.StartDate;
                    oldEndDate = previousItem.EndDate;
                }
                NotifyEdit = previousItem.Note != dto.Note || Service.EventItemGetDescription(previousItem) != description || previousItem.Title != dto.Title || previousItem.StartDate != dto.StartDate || previousItem.EndDate != dto.EndDate || previousItem.Place != dto.Place || previousItem.Link != dto.Link;
            }
            CommunityEventType type = Service.GetDiaryEventType();
            return Service.SaveEventItem(idCommunity, dto, description, descriptionPlain, UserContext.CurrentUser.Id, UserContext.CurrentUser.Id, type);
        }

        public void GetSavedItemData(Int32 idCommunity, long idItem)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(idCommunity, idItem);
            else
            {
                CommunityEventItem item = Service.EventItemGet(idItem);
                if (item!= null)
                    View.UpdateItemData(item.StartDate, item.EndDate);
            }
        }
        public void SaveItem(Int32 idCommunity, long idItem, CommunityEventItem dto, DateTime startDate, DateTime endDate, List<dtoWeekDay> weekDays, String description, String descriptionPlain)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(idCommunity, idItem);
            else
            {
                CommunityEventType type = Service.GetDiaryEventType();
                CommunityEvent eventObj = Service.AddMultipleItems(idCommunity, dto, description, descriptionPlain, UserContext.CurrentUserID, type, startDate, endDate, weekDays);

                if (eventObj == null)
                    View.SetBackToDiary(idCommunity, 0);
                else if (eventObj.Items != null && eventObj.Items.Count > 0)
                    View.SendToItemsList(idCommunity, eventObj.Items.FirstOrDefault().Id);
                else
                    View.SetBackToDiary(idCommunity, 0);
            }
        }
        public void EditAttachmentVisibility(long idItem, long idAttachment, Int32 idCommunity, Boolean visibleForModule, Boolean visibleForRepository, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(idCommunity, idItem);
            else
            {
                litePerson person = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                if (person == null || person.Id == 0)
                    View.DisplaySessionTimeout(idCommunity, idItem);
                else
                {
                    ModuleCommunityDiary module = Service.GetPermissions(UserContext.CurrentUserID, idCommunity);
                    CommunityEventItem item = Service.EventItemGet(idItem);
                    EventItemFile attachment = Service.EventItemGetAttachment(idAttachment);
                    if (item != null && attachment != null)
                    {
                        Service.AttachmentEditVisibility(item, attachment, visibleForModule, visibleForRepository);
                        ReloadAttachments(person, item, module, View.RepositoryIdentifier, unknownUser);
                    }
                    else
                        View.LoadAttachments(null);
                }
            }
        }
        private void ReloadAttachments(litePerson person, CommunityEventItem item, ModuleCommunityDiary module, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier, String unknownUser)
        {
            long idEvent = (item.EventOwner != null ? item.EventOwner.Id : 0);
            lm.Comol.Core.FileRepository.Domain.ModuleRepository moduleRepository = ServiceRepository.GetPermissions(identifier, UserContext.CurrentUserID);
            List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> availableActions = Service.GetAvailableUploadActions(module, moduleRepository);
            lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions dAction = (availableActions == null || !availableActions.Any()) ? lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.none : availableActions.FirstOrDefault();
            View.InitializeAttachmentsControl(idEvent, item.Id, availableActions, dAction);

            View.LoadAttachments(Service.AttachmentsGet(person, item, true, Service.GetItemPermission(person, item, module, moduleRepository), moduleRepository, unknownUser));
        }

        public void ReloadAttachments(long idItem, Int32 idCommunity, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(idCommunity, idItem);
            else
            {

                litePerson person = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                if (person == null || person.Id == 0)
                    View.DisplaySessionTimeout(idCommunity, idItem);
                else
                {
                    ModuleCommunityDiary module = Service.GetPermissions(UserContext.CurrentUserID, idCommunity);
                    CommunityEventItem item = Service.EventItemGet(idItem);
                    if (item != null)
                        ReloadAttachments(person, item, module, View.RepositoryIdentifier, unknownUser);
                    else
                        View.LoadAttachments(null);
                }
            }
        }

        public void UnlinkRepositoryItem(long idItem, long idAttachment, Int32 idCommunity, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(idCommunity, idItem);
            else
            {
               
                litePerson person = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                if (person == null || person.Id == 0)
                    View.DisplaySessionTimeout(idCommunity, idItem);
                else
                {
                    ModuleCommunityDiary module = Service.GetPermissions(UserContext.CurrentUserID, idCommunity);
                    CommunityEventItem item = Service.EventItemGet(idItem);
                    EventItemFile attachment = Service.EventItemGetAttachment(idAttachment);
                    if (item != null && attachment != null)
                    {
                        Service.UnLinkAttachment(idAttachment,person);
                        ReloadAttachments(person, item, module, View.RepositoryIdentifier, unknownUser);
                    }
                    else if (item != null)
                        ReloadAttachments(person, item, module, View.RepositoryIdentifier, unknownUser);
                    else
                        View.LoadAttachments(null);
                }
            }
        }
        public void VirtualDeleteUndelete(long idItem, long idAttachment, Int32 idCommunity, String unknownUser,Boolean virtualDelete)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(idCommunity, idItem);
            else
            {

                litePerson person = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                if (person == null || person.Id == 0)
                    View.DisplaySessionTimeout(idCommunity, idItem);
                else
                {
                    ModuleCommunityDiary module = Service.GetPermissions(UserContext.CurrentUserID, idCommunity);
                    CommunityEventItem item = Service.EventItemGet(idItem);
                    EventItemFile attachment = Service.EventItemGetAttachment(idAttachment);
                    if (item != null && attachment != null)
                    {
                        if (virtualDelete)
                            Service.AttachmentVirtualDelete(idAttachment);
                        else
                            Service.AttachmentVirtualUndelete(idAttachment);

                        ReloadAttachments(person, item, module, View.RepositoryIdentifier, unknownUser);
                    }
                    else if (item!=null)
                        ReloadAttachments(person, item, module, View.RepositoryIdentifier, unknownUser);
                    else
                        View.LoadAttachments(null);
                }
            }
        }

        public void PhisicalDelete(long idItem, long idAttachment, Int32 idCommunity, String baseFilePath, String baseThumbnailPath, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(View.IdCommunityDiary);
            else
            {
               
                litePerson person = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                if (person == null || person.Id == 0)
                    View.DisplaySessionTimeout(idCommunity, idItem);
                else
                {
                    ModuleCommunityDiary module = Service.GetPermissions(UserContext.CurrentUserID, idCommunity);
                    CommunityEventItem item = Service.EventItemGet(idItem);
                    EventItemFile attachment = Service.EventItemGetAttachment(idAttachment);
                    if (item != null && attachment != null)
                    {
                        if (module.Edit || module.Administration)
                            Service.AttachmentPhisicalDelete(item, attachment, baseFilePath, baseThumbnailPath);
                        ReloadAttachments(person, item, module, View.RepositoryIdentifier, unknownUser);
                    }
                    else if (item != null)
                        ReloadAttachments(person, item, module, View.RepositoryIdentifier, unknownUser);
                }
            }
        }
	}
}