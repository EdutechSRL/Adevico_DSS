using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.FileRepository.Business;
using lm.Comol.Core.FileRepository.Domain;
using lm.Comol.Core.BaseModules.FileRepository.Business;
using lm.Comol.Core.BaseModules.FileRepository.Domain;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public class MultimediaSettingsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceRepository service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewMultimediaSettings View
            {
                get { return (IViewMultimediaSettings)base.View; }
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
            public MultimediaSettingsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public MultimediaSettingsPresenter(iApplicationContext oContext, IViewMultimediaSettings view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idLink,long idItem,long idVersion, long idFolder, String path, Boolean setBackUrl, String backUrl)
        {
            RepositoryIdentifier rIdentifier = Service.ItemGetRepositoryIdentifier(idItem);
            Int32 idCommunity = UserContext.CurrentCommunityID;
            View.IdItem = idItem;
            View.IdVersion = idVersion;
            View.IdLink = idLink;
            View.IdCurrentFolder = idFolder;
            View.CurrentFolderIdentifierPath = path;
            if (setBackUrl && String.IsNullOrEmpty(backUrl))
            {
                backUrl = View.GetPreviousRelativeUrl();
                if (!IsValidPreviousUrl(backUrl))
                {
                    backUrl = "";
                    setBackUrl = false;
                }
                if (backUrl.StartsWith(RootObject.BaseRepositoryUrl(), StringComparison.InvariantCultureIgnoreCase) && rIdentifier!=null )
                {
                    cookieRepository cookie = View.GetRepositoryCookie(rIdentifier);
                    if (cookie != null)
                        backUrl = RootObject.RepositoryItems(rIdentifier.Type, rIdentifier.IdCommunity, 0, cookie.IdFolder, cookie.Type, cookie.ItemsOrderBy, cookie.Ascending, cookie.IdentifierPath);
                }
                SetLogoutUrl(View.GetCurrentUrl(), setBackUrl, backUrl);
            }
            else
                View.DefaultLogoutUrl = View.GetCurrentUrl();
            View.BackUrl = (setBackUrl ? backUrl : "");
            View.SetPageBackUrl(backUrl);
            View.PageIdentifier = Guid.NewGuid();
            if (SessionTimeout())
                return;

            View.IsInitialized = true;
            ModuleRepository.ActionType uAction = ModuleRepository.ActionType.None;

            if (rIdentifier == null)
            {
                uAction = ModuleRepository.ActionType.MultimedaSettingsTryToLoad;
                View.DisplayUnknownItem();
            }
            else
            {
                Int32 idCurrentUser = UserContext.CurrentUserID;
                liteRepositoryItem item = Service.ItemGet(idItem);
                liteRepositoryItemVersion version = Service.ItemGetVersion(idItem, idVersion);
                if (version == null)
                {
                    uAction = ModuleRepository.ActionType.MultimedaSettingsTryToLoad;
                    View.DisplayUnknownItem();
                }
                else if (version.Type != ItemType.Multimedia)
                {
                    idCommunity = version.IdCommunity;
                    View.DisplayMessage(version.DisplayName, version.Extension, version.Type, Domain.UserMessageType.multimediaSettingsInvalidType);
                    uAction = ModuleRepository.ActionType.MultimedaSettingsInvalidType;
                }
                else
                {
                    idVersion = version.Id;
                    View.IdVersion = idVersion;
                    idCommunity = version.IdCommunity;
                    if (version.Availability == ItemAvailability.available || version.Availability == ItemAvailability.waitingsettings){
                        ItemPermission permissions = null;

                        if (idLink == 0){
                            dtoDisplayRepositoryItem dto = Service.GetItemWithPermissions(idItem, idCurrentUser,item.Repository, View.GetUnknownUserName());
                            if (dto!=null)
                                permissions = dto.Permissions;
                        }
                        else
                        {
                            liteModuleLink link = CurrentManager.Get<liteModuleLink>(idLink);
                            if (link == null)
                                View.DisplayMessage(version.DisplayName, version.Extension, version.Type, Domain.UserMessageType.multimediaSettingsNoPermission);
                            else
                            {
                                permissions = View.GetLinkPermissions(link,idCurrentUser);
                                idCommunity = link.SourceItem.CommunityID;
                            }
                        }
                        if (permissions != null)
                        {
                            if (permissions.EditSettings)
                            {
                                if (version.Availability == ItemAvailability.waitingsettings)
                                    View.DisplayMessage(version.DisplayName, version.Extension, version.Type, Domain.UserMessageType.multimediaSettingsNoDefaultDocument);
                                List<dtoMultimediaFileObject> items = Service.MultimediaGetItems(idItem, idVersion);
                                View.AllowSave = (items != null && items.Any());
                                View.LoadItems(item.UniqueIdVersion.ToString(), item.DisplayName, items, (items == null ? null : items.Where(i => i.IsDefaultDocument).FirstOrDefault()));
                            }
                            else
                            {
                                uAction = ModuleRepository.ActionType.MultimedaSettingsNoPermissions;
                                View.DisplayMessage(version.DisplayName, version.Extension, version.Type, Domain.UserMessageType.multimediaSettingsNoPermission);
                            }
                        }
                        else
                        {
                            uAction = ModuleRepository.ActionType.MultimedaSettingsStatusError;
                            View.DisplayMessage(version.DisplayName, version.Extension, version.Type, Domain.UserMessageType.multimediaSettingsInvalidStatus, version.Availability);
                        }
                    }
                    else
                    {
                        uAction = ModuleRepository.ActionType.MultimedaSettingsStatusError;
                        View.DisplayMessage(version.DisplayName, version.Extension, version.Type, Domain.UserMessageType.multimediaSettingsInvalidStatus,version.Availability);
                    }
                }
                View.RepositoryIdentifier = CacheKeys.RepositoryIdentifierKey(rIdentifier.Type, rIdentifier.IdCommunity);
                View.RepositoryIdCommunity = rIdentifier.IdCommunity;
                View.RepositoryType = rIdentifier.Type;
                View.IsInitialized = true;
            }
            View.SendUserAction(idCommunity, Service.GetIdModule(), uAction);
        }

        #region "Actions"
            public void SetDefaultDocument(long idItem, long idVersion, long idDocument){
                if (!SessionTimeout())
                {
                    ModuleRepository.ActionType uAction = ModuleRepository.ActionType.MultimedaSettingsUnableToSetDefaultDocument;
                    Int32 idCommunity = View.RepositoryIdCommunity;
                    liteRepositoryItemVersion version = Service.ItemGetVersion(idItem, idVersion);
                    if (version == null)
                    {
                        View.DisplayUnknownItem();
                        View.AllowSave = false;
                    }
                    else
                    {
                        dtoMultimediaFileObject obj = Service.MultimediaGetItem(idDocument);
                        if (obj==null)
                            View.DisplayMessage(version.DisplayName, version.Extension, version.Type, Domain.UserMessageType.multimediaSettingsDocumentNotFound);
                        else
                        {
                            MultimediaFileObject file = Service.MultimediaSetDefaultItem(idItem, idVersion, idDocument);
                            if (file == null || !file.IsDefaultDocument)
                            {
                                uAction = ModuleRepository.ActionType.MultimedaSettingsUnableToSetDefaultDocument;
                                View.DisplayMessage(version.DisplayName, version.Extension, version.Type, Domain.UserMessageType.multimediaSettingsUnableToSave, version.Availability, obj.Fullname);
                            }
                            else
                            {
                                List<dtoMultimediaFileObject> items = Service.MultimediaGetItems(idItem, idVersion);
                                View.AllowSave = (items != null);
                                View.LoadItems(version.UniqueIdVersion.ToString(), version.DisplayName, items, (items == null ? null : items.Where(i => i.IsDefaultDocument).FirstOrDefault()));

                                uAction = ModuleRepository.ActionType.MultimedaSettingsSetDefaultDocument;
                                View.DisplayMessage(version.DisplayName, version.Extension, version.Type, Domain.UserMessageType.multimediaSettingsSaved, version.Availability, obj.Fullname);
                            }
                        }
                    }
                    View.SendUserAction(idCommunity, Service.GetIdModule(), uAction);
                }
            }

        #endregion
       
        public Boolean SessionTimeout()
        {
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout();
                return true;
            }
            else
                return false;
        }

        private Boolean IsValidPreviousUrl(String url)
        {
            return
                !url.Contains(lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.InternalLogin(false))
                && !url.Contains(lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.InternalShibbolethAuthenticationPage(false))
                && !url.Contains(lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.ShibbolethLogin(false))
                && !url.Contains(lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.TokenValidation(false))
                ;
        }
        private void SetLogoutUrl(String currentUrl, Boolean setBackUrl, String backUrl)
        {
            if (!setBackUrl)
                backUrl = "";

            View.DefaultLogoutUrl = RootObject.EditMultimediaSettings(View.PreloadIdItem, View.PreloadIdVersion, View.PreloadIdFolder, View.PreloadIdentifierPath,  setBackUrl, backUrl);
        }
    }
}