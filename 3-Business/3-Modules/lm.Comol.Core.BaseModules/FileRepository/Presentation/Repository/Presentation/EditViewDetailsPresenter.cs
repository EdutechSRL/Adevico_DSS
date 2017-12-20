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
    public class EditViewDetailsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceRepository service;
            private lm.Comol.Core.InLineTags.Business.ServiceInLineTags _ServiceInLineTags;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewEditViewDetails View
            {
                get { return (IViewEditViewDetails)base.View; }
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
            private lm.Comol.Core.InLineTags.Business.ServiceInLineTags ServiceTags
            {
                get
                {
                    if (_ServiceInLineTags == null)
                        _ServiceInLineTags = new lm.Comol.Core.InLineTags.Business.ServiceInLineTags(AppContext);
                    return _ServiceInLineTags;
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
            public EditViewDetailsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EditViewDetailsPresenter(iApplicationContext oContext, IViewEditViewDetails view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Boolean isEditPage,long idItem, long idFolder, String path, Boolean setBackUrl, String backUrl)
        {
            RepositoryIdentifier rIdentifier = Service.ItemGetRepositoryIdentifier(idItem);
            Int32 idCommunity = UserContext.CurrentCommunityID;
            View.IdItem = idItem;
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
                if (backUrl.StartsWith(RootObject.BaseRepositoryUrl(), StringComparison.InvariantCultureIgnoreCase ) && rIdentifier!=null)
                {
                    cookieRepository cookie = View.GetRepositoryCookie(rIdentifier);
                    if (cookie != null)
                        backUrl = RootObject.RepositoryItems(rIdentifier.Type, rIdentifier.IdCommunity, 0, cookie.IdFolder, cookie.Type, cookie.ItemsOrderBy, cookie.Ascending, cookie.IdentifierPath);
                }
                SetLogoutUrl(isEditPage,View.GetCurrentUrl(), setBackUrl, backUrl);
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
                View.InitializeHeader();
                uAction = (isEditPage) ? ModuleRepository.ActionType.EditDetailsTryToLoad : ModuleRepository.ActionType.ViewDetailsTryToLoad;
                View.DisplayUnknownItem();
            }
            else
            {
                Int32 idCurrentUser = UserContext.CurrentUserID;
                liteRepositorySettings settings = Service.SettingsGetByRepositoryIdentifier(rIdentifier);
               
                ModuleRepository module = Service.GetPermissions(rIdentifier, idCurrentUser);

                View.RepositoryIdentifier = CacheKeys.RepositoryIdentifierKey(rIdentifier.Type, rIdentifier.IdCommunity);
                View.RepositoryIdCommunity = rIdentifier.IdCommunity;
                idCommunity = rIdentifier.IdCommunity;
                View.RepositoryType = rIdentifier.Type;

                View.SetTitle(Service.ItemGetType(idItem));
                View.IsInitialized = true;
                if (settings != null)
                {
                    Boolean admin = module.ManageItems || module.Administration;
                    dtoDisplayRepositoryItem dto = Service.GetItemWithPermissionsAndStatistics(idItem, Service.GetAvailableRepositoryItems(settings, UserContext.CurrentUserID, rIdentifier.Type, rIdentifier.IdCommunity, View.GetUnknownUserName(), View.GetFolderTypeTranslation(), View.GetTypesTranslations(), module, admin, admin), UserContext.CurrentUserID);
                    if (dto != null)
                    {
                        if (!isEditPage && dto.Permissions.Edit)
                            View.SetUrlForEdit(RootObject.EditItem(idItem, idFolder, path, ItemAction.edit, (!String.IsNullOrWhiteSpace(backUrl)), backUrl));
                        else if (isEditPage )
                            View.SetUrlForView(RootObject.Details(idItem, idFolder, path, ItemAction.edit, (!String.IsNullOrWhiteSpace(backUrl)), backUrl));
                        if (!isEditPage || (isEditPage && dto.Permissions.Edit))
                        {
                            View.AllowSave = dto.Permissions.Edit && isEditPage;

                            View.AllowHideItem= dto.Permissions.Hide && isEditPage;
                            View.AllowShowItem = dto.Permissions.Show && isEditPage;
                            View.DisplayItemDetails(isEditPage, dto, settings.AutoThumbnailWidth, settings.AutoThumbnailHeight, settings.AutoThumbnailForExtension);
                            if (dto.Permissions.ViewPermission || dto.Permissions.EditPermission)
                                View.DisplayItemPermissions(isEditPage, dto);
                            if (isEditPage)
                                View.InitializeDefaultTags(ServiceTags.GetAvailableTags(rIdentifier.IdPerson,idCommunity,CurrentIdModule, ModuleRepository.UniqueCode));
                            else
                                View.InitializeHeader();
                            uAction = (isEditPage) ? ModuleRepository.ActionType.EditDetailsLoaded : ModuleRepository.ActionType.ViewDetailsLoaded;
                        }
                        else if (isEditPage)
                        {
                            View.SendUserAction(idCommunity, Service.GetIdModule(), ModuleRepository.ActionType.EditDetailsTryToLoad);
                            View.GoToUrl(RootObject.Details(idItem, idFolder, path, ItemAction.details, setBackUrl, backUrl));
                        }
                    }
                    else
                    {
                        if (isEditPage)
                            View.SetUrlForView(RootObject.Details(idItem, idFolder, path, ItemAction.edit, (!String.IsNullOrWhiteSpace(backUrl)), backUrl));

                        View.InitializeHeader();
                        View.DisplayNoPermission(idCommunity, Service.GetIdModule());
                        uAction = (isEditPage) ? ModuleRepository.ActionType.EditDetailsNoPermissions : ModuleRepository.ActionType.ViewDetailsNoPermissions;
                    }
                }
                else
                {
                    View.InitializeHeader();
                    uAction = (isEditPage) ? ModuleRepository.ActionType.EditDetailsTryToLoad : ModuleRepository.ActionType.ViewDetailsTryToLoad;
                    View.DisplayUnknownItem();
                }
            }
            View.SendUserAction(idCommunity, Service.GetIdModule(), uAction);
        }

        #region "Actions"
            public void CurrentVersionUpdated(long idItem) {
                if (!SessionTimeout())
                {
                    RepositoryIdentifier rIdentifier = Service.ItemGetRepositoryIdentifier(idItem);
                    if (rIdentifier == null)
                    {
                        View.InitializeHeader();
                        View.DisplayUnknownItem();
                    }
                    else
                    {
                        Int32 idCurrentUser = UserContext.CurrentUserID;
                        liteRepositorySettings settings = Service.SettingsGetByRepositoryIdentifier(rIdentifier);
                        ModuleRepository module = Service.GetPermissions(rIdentifier, idCurrentUser);
                        Boolean admin = module.ManageItems || module.Administration;
                       
                        dtoDisplayRepositoryItem dto = Service.GetItemWithPermissionsAndStatistics(idItem, Service.GetAvailableRepositoryItems(settings, UserContext.CurrentUserID, rIdentifier.Type, rIdentifier.IdCommunity, View.GetUnknownUserName(), View.GetFolderTypeTranslation(), View.GetTypesTranslations(), module, admin, admin), UserContext.CurrentUserID);
                        View.UpdateItemDetails( dto, settings.AutoThumbnailWidth, settings.AutoThumbnailHeight, settings.AutoThumbnailForExtension);
                    }
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
        private void SetLogoutUrl(Boolean isEditPage, String currentUrl, Boolean setBackUrl, String backUrl)
        {
            if (!setBackUrl)
                backUrl = "";

            View.DefaultLogoutUrl = (isEditPage ?
                                RootObject.EditItem(View.PreloadIdItem, View.PreloadIdFolder, View.PreloadIdentifierPath, ItemAction.edit, setBackUrl, backUrl)
                                :
                                RootObject.Details(View.PreloadIdItem, View.PreloadIdFolder, View.PreloadIdentifierPath, ItemAction.details, setBackUrl, backUrl)
                                );
        }
    }
}