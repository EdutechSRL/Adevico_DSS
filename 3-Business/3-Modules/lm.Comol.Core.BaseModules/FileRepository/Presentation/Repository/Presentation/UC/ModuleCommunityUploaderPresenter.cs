using lm.Comol.Core.BaseModules.FileRepository.Business;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public class ModuleCommunityUploaderPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceRepository service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewModuleCommunityUploader View
            {
                get { return (IViewModuleCommunityUploader)base.View; }
            }
            private ServiceRepository Service
            {
                get
                {
                    if (service == null)
                        service = new ServiceRepository(AppContext);
                    return service;
                }
            }
            private Int32 CurrentIdModule
            {
                get
                {
                    if (currentIdModule == 0)
                        currentIdModule = CurrentManager.GetModuleID(ModuleRepository.UniqueCode);
                    return currentIdModule;
                }
            }
            public ModuleCommunityUploaderPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ModuleCommunityUploaderPresenter(iApplicationContext oContext, IViewModuleCommunityUploader view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idFolder,RepositoryIdentifier identifier, Boolean allowAnonymousUpload, Boolean useAnonymous, Boolean usePublicUser )
        {
            Int32 idUploaderUser = UserContext.CurrentUserID;
            View.RepositoryIdentifier = identifier;
            if (identifier==null || (SessionTimeout() && !allowAnonymousUpload))
            {
                View.DisableControl();
                View.DisplayUploadUnavailable();
                return;
            }
            liteRepositorySettings settings = Service.SettingsGetByRepositoryIdentifier(identifier);
            if (settings != null)
            {
                litePerson uploader = CurrentManager.GetLitePerson(idUploaderUser);
                if (uploader == null && allowAnonymousUpload)
                {
                    if (useAnonymous)
                        uploader = CurrentManager.GetLiteUnknownUser();
                    else if (usePublicUser)
                        uploader = CurrentManager.GetLitePublicUser(identifier.IdCommunity);
                }
                else if (uploader!= null && (uploader.Id==0 || uploader.TypeID == (int)UserTypeStandard.PublicUser && !usePublicUser))
                {
                    uploader = null;
                }
                if (uploader != null)
                {
                    View.IdUploaderUser = uploader.Id;
                    View.LoadItemTypes(((settings.ItemTypes == null) ? new List<ItemType>() { ItemType.File } : settings.ItemTypes.Where(t => t.Deleted == BaseStatusDeleted.None && t.Type != ItemType.Folder && t.Type != ItemType.Link && t.Type != ItemType.RootFolder).Select(t => t.Type).Distinct().ToList()));
                    InitializeView(idFolder, identifier, Service.GetPermissions(identifier, idUploaderUser), settings, uploader);
                }
            }
            else
            {
                View.DisableControl();
                View.DisplayUploadUnavailable();
            }
        }
        private void InitializeView(long idFolder, RepositoryIdentifier identifier, ModuleRepository module, liteRepositorySettings settings, litePerson uploader)
        {
            Boolean admin = module.ManageItems || module.Administration;
            dtoContainerQuota quota = Service.FolderGetHomeAvailableSize(View.GetRepositoryDiskPath(), settings, module, identifier);
            View.AllowUploadToFolder = false;
            View.AllowUpload = (quota != null && quota.HasAllowedSpace() && (module.Administration || module.ManageItems || module.UploadFile));
            List<dtoNodeFolderItem> folders = Service.GetFoldersForUpload(View.GetRepositoryDiskPath(), idFolder, UserContext.CurrentUserID, identifier, module, View.GetUnknownUserName(), View.GetRootFolderFullname());
            dtoNodeFolderItem currentFolder = (idFolder == 0 ? null : folders.Where(f => f.Id == idFolder).FirstOrDefault());
            if (quota != null)
            {
                View.AllowUploadToFolder = (quota.HasAllowedSpace() && (module.Administration || module.ManageItems || module.UploadFile || (currentFolder != null && currentFolder.Selectable && Service.FolderAllowUpload(idFolder) && module.ViewItemsList)));
            }
            View.LoadFolderSelector(identifier, idFolder, (idFolder == 0 || currentFolder.Id == 0) ? View.GetRootFolderFullname() : currentFolder.Name, quota, folders);
        }

        public List<dtoModuleUploadedItem> AddFiles(String istanceIdentifier, Int32 idUploaderUser, Boolean allowAnonymousUpload,  Boolean alwaysLastVersion,RepositoryIdentifier identifier, long idFolder, List<dtoUploadedItem> files, Object obj, long idObject, Int32 idObjectType, String moduleCode, Int32 idModuleAjaxAction, Int32 idModuleAction = 0)
        {
            List<dtoModuleUploadedItem> items = new List<dtoModuleUploadedItem>();
            if (!SessionTimeout() || allowAnonymousUpload && idUploaderUser > 0)
            {
                litePerson person = Service.GetValidPerson(idUploaderUser);
                liteRepositorySettings settings = Service.SettingsGetByRepositoryIdentifier(identifier);
                ModuleRepository module = Service.GetPermissions(identifier, idUploaderUser);
                ModuleRepository.ObjectType oType = ModuleRepository.ObjectType.File;
                ModuleRepository.ActionType uAction = ModuleRepository.ActionType.UnableToAddFile;
                liteRepositoryItem item = (idFolder > 0 ? Service.ItemGet(idFolder) : null);
                if (person== null || (item == null && idFolder > 0))
                {
                    View.DisplayError(ItemUploadError.UnableToAddFileToUnknownFolder);
                    uAction = ModuleRepository.ActionType.UnknownItemFound;
                    InitializeView(idFolder, identifier, module, settings, CurrentManager.GetLitePerson(idUploaderUser));
                }
                else
                {
                    Boolean allowAdd = module.Administration || module.ManageItems || module.UploadFile;
                    dtoDisplayRepositoryItem dItem = Service.GetItemWithPermissions(idFolder, UserContext.CurrentUserID, identifier, View.GetUnknownUserName());

                    if (dItem != null)
                    {
                        oType = ModuleRepository.GetObjectType(dItem.Type);
                        allowAdd = allowAdd || dItem.Permissions.GetActions().Contains(ItemAction.upload);
                    }
                    else if (idFolder == 0)
                        oType = ModuleRepository.ObjectType.Folder;

                    String folderName = (idFolder == 0 ? View.GetRootFolderName() : dItem.Name);
                    if (!allowAdd)
                    {
                        View.DisplayError(ItemUploadError.MissingPermissionsToAddFile, folderName, "", ItemType.Folder);
                        uAction = ModuleRepository.ActionType.UnavailableItem;
                    }
                    else
                    {
                        Boolean executed = false;
                        items = Service.FileAddToRepository(settings, istanceIdentifier,identifier, person, alwaysLastVersion,  module, View.GetRepositoryDiskPath(), idFolder, files,  obj, idObject, idObjectType, moduleCode, idModuleAjaxAction, idModuleAction);
                        executed = (items != null && items.Any(a => a.IsAdded));

                        uAction = (executed ? (items.Any(a => !a.IsAdded) ? ModuleRepository.ActionType.UnableToAddSomeFile : ModuleRepository.ActionType.AddFile) : ModuleRepository.ActionType.UnableToAddFile);
                        if (executed)
                            View.NotifyAddedItems(Service.GetIdModule(), idFolder, folderName, RootObject.RepositoryItems(identifier.Type, identifier.IdCommunity, -1, (dItem == null ? 0 : dItem.Id)), items.Where(a => a.ItemAdded != null).Select(f => f.ItemAdded).ToList());
                        else
                            View.DisplayError(ItemUploadError.UnableToAddFile, folderName, (items == null) ? null : items.Where(a => !a.IsAdded).Select(a => a.UploadedFile).ToList());
                    }
                }
                View.SendUserAction(identifier.IdCommunity, Service.GetIdModule(), uAction, idFolder, oType);
            }
            return items;
        }

        private Boolean SessionTimeout()
        {
            if (UserContext.isAnonymous)
                return true;
            else
                return false;
        }
    }
}