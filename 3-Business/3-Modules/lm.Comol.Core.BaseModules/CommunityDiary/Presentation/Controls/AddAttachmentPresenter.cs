using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.BaseModules.CommunityDiary.Business;
using lm.Comol.Core.BaseModules.CommunityDiary.Domain;

namespace lm.Comol.Core.BaseModules.CommunityDiary.Presentation
{
    public class AddAttachmentPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceCommunityDiary service;
            private lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository _ServiceRepository;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewAddAttachment View
            {
                get { return (IViewAddAttachment)base.View; }
            }
            private ServiceCommunityDiary Service
            {
                get
                {
                    if (service == null)
                        service = new ServiceCommunityDiary(AppContext);
                    return service;
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
            private Int32 CurrentIdModule
            {
                get
                {
                    if (currentIdModule == 0)
                        currentIdModule = CurrentManager.GetModuleID(ModuleCommunityDiary.UniqueID);
                    return currentIdModule;
                }
            }
            public AddAttachmentPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AddAttachmentPresenter(iApplicationContext oContext, IViewAddAttachment view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idEvent, long idEventItem, Int32 lessonNumber, lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions action, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier, lm.Comol.Core.FileRepository.Domain.ModuleRepository rPermissions = null)
        {
            if (!UserContext.isAnonymous)
            {
                View.CurrentAction = action;
                View.IdEvent = idEvent;
                View.IdEventItem = idEventItem;
                View.IdEventCommunity = identifier.IdCommunity;
                View.LessonNumber = lessonNumber;
                if (View.DisplayInfo)
                    DisplayEventItemInfo(idEventItem, lessonNumber);
                switch (action) { 
                    case Core.DomainModel.Repository.RepositoryAttachmentUploadActions.addurltomoduleitem:

                        View.InitializeUploaderControl(action,identifier);
                        break;
                    case Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem:
                        View.InitializeUploaderControl(action, identifier);
                        break;
                    case Core.DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity:
                        if (rPermissions == null)
                            rPermissions = ServiceRepository.GetPermissions(identifier, UserContext.CurrentUserID);
                        if (rPermissions != null && identifier.IdCommunity > 0)
                            View.InitializeLinkRepositoryItems(UserContext.CurrentUserID, rPermissions, identifier, Service.AttachmentsGetBaseLinkedFiles(idEventItem));
                        break;
                    case Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity:
                        if (rPermissions == null)
                            rPermissions = ServiceRepository.GetPermissions(identifier, UserContext.CurrentUserID);
                        if (rPermissions != null && identifier.IdCommunity > 0)
                            View.InitializeCommunityUploader(identifier);
                        break;
                    case Core.DomainModel.Repository.RepositoryAttachmentUploadActions.addurltomoduleitemandcommunity:
                        break;
                }
            }
            else
                View.CurrentAction = Core.DomainModel.Repository.RepositoryAttachmentUploadActions.none;
        }
        private void DisplayEventItemInfo(long idEventItem,  Int32 lessonNumber)
        {
            CommunityEventItem item = Service.EventItemGet(idEventItem);
            if (item != null)
                View.DisplayEventItemInfo(item.Title, item.StartDate, item.EndDate, item.ShowDateInfo, item.MinutesDuration, lessonNumber);
        }
        public void AddFilesToItem(long idEvent, long idEventItem,Int32 idCommunity,  Boolean visibleForItem)
        {
            if (UserContext.isAnonymous)
                View.DisplayWorkingSessionExpired();
            else
            {
                List<lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem> files = null;
                CommunityEventItem item = Service.EventItemGet(idEventItem);
                if (item != null)
                    files = View.UploadFiles(ModuleCommunityDiary.UniqueID, (Int32)ModuleCommunityDiary.ObjectType.DiaryItemLinkedFile, (Int32)ModuleCommunityDiary.ActionType.DownloadFileItem,  false);
                else {
                    View.DisplayEventItemNotFound();
                    View.SendUserAction(idCommunity, CurrentIdModule, idEventItem, ModuleCommunityDiary.ActionType.UnkownDiaryItem);
                    return;
                }

                if (files != null && files.Any(f => f.IsAdded))
                {
                    List<EventItemFile> attachments = Service.AttachmentsAddFiles(item, files.Where(f => f.IsAdded).ToList(), visibleForItem);
                    if (attachments == null)
                        View.DisplayItemsNotAdded();
                    else
                        View.DisplayItemsAdded();
                    View.SendUserAction(idCommunity, CurrentIdModule, idEventItem, (attachments == null) ? ModuleCommunityDiary.ActionType.AttachmentsNotAddedFiles : ModuleCommunityDiary.ActionType.AttachmentsAddedFiles);
                }
                else
                    View.DisplayNoFilesToAdd();
            }
        }
        public void AddCommunityFilesToItem(long idEvent, long idEventItem, Int32 idCommunity, Boolean visibleForItem, Boolean visibleForRepository)
        {
            if (UserContext.isAnonymous)
                View.DisplayWorkingSessionExpired();
            else
            {
                List<lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem> files = null;
                CommunityEventItem item = Service.EventItemGet(idEventItem);
                if (item != null)
                    files = View.UploadFiles(ModuleCommunityDiary.UniqueID, (Int32) ModuleCommunityDiary.ObjectType.DiaryItemLinkedFile, (Int32)ModuleCommunityDiary.ActionType.DownloadFileItem, true);
                else
                {
                    View.DisplayEventItemNotFound();
                    View.SendUserAction(idCommunity, CurrentIdModule, idEventItem, ModuleCommunityDiary.ActionType.UnkownDiaryItem);
                    return;
                }
                if (files != null && files.Any(f => f.IsAdded))
                    AddCommunityFilesToItem(idEvent, idEventItem, idCommunity, (files == null) ? null : files.Where(f => f.IsAdded).Select(f => f.Link).ToList(), visibleForItem, visibleForRepository);
                else
                    View.DisplayNoFilesToAdd();
            }
        }
        public void AddCommunityFilesToItem(long idEvent, long idEventItem, Int32 idCommunity, List<ModuleActionLink> links, Boolean visibleForItem)
        {
            if (UserContext.isAnonymous)
                View.DisplayWorkingSessionExpired();
            else if (links != null && links.Count > 0)
                AddCommunityFilesToItem(idEvent, idEventItem, idCommunity, links, visibleForItem, null);
            else
                View.DisplayNoFilesToAdd();
        }

        private void AddCommunityFilesToItem(long idEvent, long idEventItem, Int32 idCommunity, List<ModuleActionLink> links, Boolean visibleForItem, Boolean? visibleForRepository)
        {
            if (links != null && links.Count > 0)
            {
                List<EventItemFile> attachments = Service.AttachmentsLinkFiles(idEvent, idEventItem, links, visibleForItem, visibleForItem);
                if (attachments == null)
                    View.DisplayItemsNotAdded();
                else
                    View.DisplayItemsAdded();
                View.SendUserAction(idCommunity, CurrentIdModule, idEventItem, (attachments == null) ? ModuleCommunityDiary.ActionType.AttachmentsNotAddedFiles : ModuleCommunityDiary.ActionType.AttachmentsAddedFiles);
            }
            else
                View.DisplayNoFilesToAdd();
        }
    }
}