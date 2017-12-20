using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.FileRepository.Business;
using lm.Comol.Core.FileRepository.Domain;
using lm.Comol.Core.BaseModules.FileRepository.Business;
using lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public class ItemVersionsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private lm.Comol.Core.InLineTags.Business.ServiceInLineTags _ServiceInLineTags;
            private ServiceRepository service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewItemVersions View
            {
                get { return (IViewItemVersions)base.View; }
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
            public ItemVersionsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ItemVersionsPresenter(iApplicationContext oContext, IViewItemVersions view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Boolean editMode, dtoDisplayRepositoryItem item,String unknownUser,String repositoryPath)
        {
            if (!SessionTimeout())
            {
                LoadVersions(editMode, item, unknownUser, repositoryPath);
            }
        }
        public void AddVersionToFile(String istanceIdentifier, long idItem, dtoUploadedItem version, String unknownUser, String repositoryPath)
        {
            if (!SessionTimeout())
            {
                liteRepositoryItem  rItem = Service.ItemGet(idItem);
                Int32 idCommunity = UserContext.CurrentCommunityID;
                ModuleRepository.ObjectType oType = ModuleRepository.ObjectType.File;
                ModuleRepository.ActionType uAction = ModuleRepository.ActionType.VersionUnableToAdd;

                if (rItem != null)
                {
                    ModuleRepository module = Service.GetPermissions(rItem.Repository, UserContext.CurrentUserID);
                    Boolean reloadItems = false;
                    idCommunity = rItem.Repository.IdCommunity;
                    dtoDisplayRepositoryItem dItem = Service.GetItemWithPermissions(idItem, UserContext.CurrentUserID, rItem.Repository, View.GetUnknownUserName());
                    if (dItem == null)
                    {
                        View.DisplayUserMessage(UserMessageType.versionItemNoPermission);
                        uAction = ModuleRepository.ActionType.UnknownItemFound;
                    }
                    else
                    {
                        Boolean allowAdd = dItem.Permissions.GetActions().Contains(ItemAction.addVersion);
                        oType = ModuleRepository.GetObjectType(dItem.Type);
                        if (!allowAdd)
                        {
                            View.DisplayUserMessage(UserMessageType.versionItemNoPermission);
                            uAction = ModuleRepository.ActionType.UnavailableItem;
                        }
                        else
                        {
                            liteRepositorySettings settings = Service.SettingsGetByRepositoryIdentifier(rItem.Repository);

                            Service.ThumbnailsCreate(settings, dItem.UniqueId, version);
                            dtoCreatedItem addedVersion = Service.FileAddVersion(settings, module, repositoryPath, istanceIdentifier, idItem, version);
                            reloadItems = (addedVersion != null && addedVersion.IsAdded);

                            uAction = (reloadItems ? ModuleRepository.ActionType.VersionAddedToFile : ModuleRepository.ActionType.VersionUnableToAdd);
                            if (reloadItems)
                            {
                                View.DisplayUserMessage(UserMessageType.versionAdded);
                                dItem.IdVersion = addedVersion.Added.Id;
                                dItem.UniqueIdVersion = addedVersion.Added.UniqueIdVersion;
                                dItem.Thumbnail = addedVersion.Added.Thumbnail;
                                dItem.AutoThumbnail = addedVersion.Added.AutoThumbnail;
                                View.CurrentVersionUpdated(dItem, settings.AutoThumbnailWidth, settings.AutoThumbnailHeight, settings.AutoThumbnailForExtension);
                                String folderName = (dItem.IdFolder == 0 ? View.GetRootFolderFullname() : Service.FolderGetName(dItem.IdFolder));
                                View.NotifyAddedVersion(Service.GetIdModule(), dItem.IdFolder, folderName, RootObject.RepositoryItems(dItem.Repository.Type, dItem.Repository.IdCommunity,  dItem.Id, dItem.IdFolder), addedVersion);
                            }
                            else
                                View.DisplayUserMessage(UserMessageType.versionNotAdded);
                        }
                    }
                    if (reloadItems)
                        LoadVersions(true, dItem,unknownUser, repositoryPath );
                }
                View.SendUserAction(idCommunity, Service.GetIdModule(), uAction, idItem, oType);
            }
        }
        private void LoadVersions(liteRepositoryItem item, String unknownUser, String repositoryPath)
        {
            dtoDisplayRepositoryItem dItem = Service.GetItemWithPermissions(item.Id, UserContext.CurrentUserID, item.Repository, View.GetUnknownUserName());
            if (dItem == null)
            {
                View.DisplayUserMessage(UserMessageType.versionItemNoPermission);
                View.SendUserAction(item.IdCommunity, Service.GetIdModule(), ModuleRepository.ActionType.NoPermission, item.Id, ModuleRepository.GetObjectType(item.Type));
            }
            else
                LoadVersions(true, dItem, unknownUser, repositoryPath);
        }
        private void LoadVersions(Boolean editMode, dtoDisplayRepositoryItem item, String unknownUser, String repositoryPath)
        {
            Boolean allowAddVersion = editMode && item.Permissions.AddVersion;

            List<dtoDisplayVersionItem> items = Service.ItemGetVersions(editMode, item, unknownUser);
            if (!editMode)
                items = items.Where(i => !i.IsDeleted).ToList();

            if (!allowAddVersion)
                View.LoadVersions(items);
            else
                View.LoadVersions(items, item, Service.GetFolderQuota(repositoryPath, 0, item.Repository));
        }

        public void ExecuteAction(long idItem, long idVersion, ItemAction action, String unknownUser, String repositoryPath, String thumnailPath)
        {
            if (!SessionTimeout())
            {
                Boolean reloadItems = false;
                Int32 idCommunity = UserContext.CurrentCommunityID;
                ModuleRepository.ActionType uAction = ModuleRepository.ActionType.GenericError;
                liteRepositoryItemVersion version = Service.VersionGet(idVersion);
                if (version == null)
                {
                    View.DisplayUserMessage(UserMessageType.versionItemNotFound);
                    uAction = ModuleRepository.ActionType.UnknownItemFound;
                }
                else
                {
                    liteRepositoryItem item = Service.ItemGet(version.IdItem);
                    if (item != null)
                    {
                        String path = repositoryPath;
                        switch (item.Repository.Type)
                        {
                            case RepositoryType.Portal:
                                path += "\\0";
                                break;
                            case RepositoryType.Community:
                                path += "\\" + item.Repository.IdCommunity.ToString();
                                break;
                        }
                        Boolean executed = false;
                        switch (action)
                        {
                            case ItemAction.addVersion:
                                RepositoryItemVersion vr = Service.VersionSetActive(idVersion);
                                reloadItems = (vr!=null && vr.IsActive);
                                if (reloadItems)
                                    View.CurrentVersionUpdated();
                                View.DisplayUserMessage((reloadItems ? UserMessageType.versionPromoted : UserMessageType.versionNotPromoted));
                                uAction = (reloadItems) ? ModuleRepository.ActionType.VersionSetAsActive : ModuleRepository.ActionType.VersionUnableToSetAsActive;
                                break;
                        }
                        if (reloadItems)
                            LoadVersions(item,unknownUser, repositoryPath );
                    }
               
                }
                View.SendUserAction(idCommunity, Service.GetIdModule(), uAction, idVersion, ModuleRepository.ObjectType.VersionItem);
            }
        }
        private Boolean SessionTimeout()
        {
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout();
                return true;
            }
            else
                return false;
        }
    }
}