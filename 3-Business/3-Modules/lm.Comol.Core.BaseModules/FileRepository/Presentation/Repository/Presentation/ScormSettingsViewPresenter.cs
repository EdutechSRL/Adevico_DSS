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
using lm.Comol.Core.FileRepository.Domain.ScormSettings;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public class ScormSettingsViewPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceRepository service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewScormSettingsView View
            {
                get { return (IViewScormSettingsView)base.View; }
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
            public ScormSettingsViewPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ScormSettingsViewPresenter(iApplicationContext oContext, IViewScormSettingsView view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idSettings, long idLink,long idItem, long idVersion)
        {
            RepositoryIdentifier rIdentifier = Service.ItemGetRepositoryIdentifier(idItem);
            Int32 idCommunity = UserContext.CurrentCommunityID;
            View.IdItem = idItem;
            View.IdVersion = idVersion;
            View.IdLink = idLink;
            View.PageIdentifier = Guid.NewGuid();
            if (SessionTimeout())
                return;

            View.IsInitialized = true;
            ModuleRepository.ActionType uAction = ModuleRepository.ActionType.None;

            if (rIdentifier == null)
            {
                uAction = ModuleRepository.ActionType.ScormSettingsTryToLoad;
                View.DisplayUnknownItem();
            }
            else
            {
                Int32 idCurrentUser = UserContext.CurrentUserID;
                liteRepositoryItem item = Service.ItemGet(idItem);
                liteRepositoryItemVersion version = Service.ItemGetVersion(idItem, idVersion);
                if (version == null)
                {
                    uAction = ModuleRepository.ActionType.ScormSettingsTryToLoad;
                    View.DisplayUnknownItem();
                }
                else if (version.Type != ItemType.ScormPackage)
                {
                    idCommunity = version.IdCommunity;
                    View.DisplayMessage(version.DisplayName, version.Extension, version.Type, Domain.UserMessageType.scormSettingsInvalidType);
                    uAction = ModuleRepository.ActionType.ScormSettingsInvalidType;
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
                                View.DisplayMessage(version.DisplayName, version.Extension, version.Type, Domain.UserMessageType.scormSettingsNoPermission);
                            else
                            {
                                permissions = View.GetLinkPermissions(link,idCurrentUser);
                                idCommunity = link.SourceItem.CommunityID;
                            }
                        }
                        if (permissions != null && (permissions.Edit || permissions.EditSettings || permissions.ViewMyStatistics || permissions.ViewOtherStatistics))
                        {
                            uAction = ModuleRepository.ActionType.ScormSettingsView;
                            if (version.Availability == ItemAvailability.waitingsettings)
                                View.DisplayMessage(version.DisplayName, version.Extension, version.Type, Domain.UserMessageType.scormSettingsWaitingToSet);
                            dtoScormPackageSettings settings = Service.ScormPackageGetDtoCompletionSettings(idItem, idVersion, idSettings);
                            if (settings == null)
                                View.DisplayMessage(version.DisplayName, version.Extension, version.Type, Domain.UserMessageType.scormSettingsNotFound);
                            else
                            {
                                View.IdSettings = settings.Id;
                                View.LoadSettings(item.UniqueIdVersion.ToString(), item.DisplayName, settings,true,true);
                            }
                        }
                        else
                        {
                            uAction = ModuleRepository.ActionType.ScormSettingsNoPermissions;
                            View.DisplayMessage(version.DisplayName, version.Extension, version.Type, Domain.UserMessageType.scormSettingsNoPermission);
                        }
                    }
                    else{
                        uAction = ModuleRepository.ActionType.ScormSettingsStatusError;
                        View.DisplayMessage(version.DisplayName, version.Extension, version.Type, Domain.UserMessageType.scormSettingsInvalidStatus,version.Availability);
                    }
                }

                View.RepositoryIdentifier = CacheKeys.RepositoryIdentifierKey(rIdentifier.Type, rIdentifier.IdCommunity);
                View.RepositoryIdCommunity = rIdentifier.IdCommunity;
                View.RepositoryType = rIdentifier.Type;
                View.IsInitialized = true;
            }
            View.SendUserAction(idCommunity, Service.GetIdModule(), uAction);
        }
        public Boolean SessionTimeout()
        {
            if (UserContext.isAnonymous)
            {
                View.HideItemsForSessionTimeout();
                return true;
            }
            else
                return false;
        }
    }
}