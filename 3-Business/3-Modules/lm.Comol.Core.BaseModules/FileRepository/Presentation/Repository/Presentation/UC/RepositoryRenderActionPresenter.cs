using lm.Comol.Core.BaseModules.FileRepository.Business;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.FileRepository.Domain;
using lm.Comol.Core.ModuleLinks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public class RepositoryRenderActionPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceRepository service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewRepositoryRenderAction View
            {
                get { return (IViewRepositoryRenderAction)base.View; }
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
            public RepositoryRenderActionPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public RepositoryRenderActionPresenter(iApplicationContext oContext, IViewRepositoryRenderAction view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

            public void InitView(Int32 idUser, dtoObjectRenderInitializer dto, DisplayActionMode dAction)
            {
                InitView(idUser, dto, (ValidateActionMode(dAction, DisplayActionMode.defaultAction) ? StandardActionType.Play : StandardActionType.None), dAction);
            }
            public void InitView(Int32 idUser, dtoObjectRenderInitializer dto, StandardActionType actionToDisplay, DisplayActionMode dAction)
            {
                InitializeControlByLink(false, idUser, dto, actionToDisplay, dAction);
            }
            public List<dtoModuleActionControl> InitRemoteControlView(Int32 idUser, dtoObjectRenderInitializer dto, StandardActionType actionsToDisplay, DisplayActionMode dAction)
            {
                return InitializeControlByLink((dAction == DisplayActionMode.none), idUser, dto, actionsToDisplay, dAction);
            }
            private List<dtoModuleActionControl> InitializeControlByLink(Boolean getOnlyActions, Int32 idUser, dtoObjectRenderInitializer dto, StandardActionType actionsToDisplay, DisplayActionMode dAction)
            {
                List<dtoModuleActionControl> actions = new List<dtoModuleActionControl>();
                if (!getOnlyActions && (dto.Link == null || dto.Link.DestinationItem == null))
                {
                    if (getOnlyActions)
                        return new List<dtoModuleActionControl>();
                    else
                        View.DisplayEmptyAction();
                }
                else if (dto.Link.DestinationItem.ServiceCode == CoreModuleRepository.UniqueID)
                {
                    View.ItemIdentifier = "link_" + dto.Link.Id.ToString();
                    actions = AnalyzeActions(getOnlyActions, dto, dAction, actionsToDisplay);
                }
                else if (!getOnlyActions)
                    View.DisplayEmptyAction();
                return actions;
            }

            private List<dtoModuleActionControl> AnalyzeActions(Boolean getOnlyActions, dtoObjectRenderInitializer dto, DisplayActionMode dAction, StandardActionType actionsToDisplay)
            {
                List<dtoModuleActionControl> actions = new List<dtoModuleActionControl>();
                if (dto.Link == null || dto.Link.DestinationItem == null)
                {
                    if (getOnlyActions)
                        return new List<dtoModuleActionControl>();
                    else
                        View.DisplayEmptyAction();
                }
                else
                {
                    if (!getOnlyActions)
                    {
                        View.IdRequiredAction = dto.Link.Action;
                        View.Display = dAction;
                    }
                    liteRepositoryItem item = (dto.Link.DestinationItem.ObjectLongID == 0 ? null : Service.ItemGet(dto.Link.DestinationItem.ObjectLongID));
                    View.ItemType = (item == null ? ItemType.File : item.Type);
                    liteRepositoryItemVersion version = null;
                    if (dto.Link.DestinationItem.ObjectIdVersion > 0)
                        version = Service.VersionGet(dto.Link.DestinationItem.ObjectIdVersion);
                    if (!getOnlyActions && item == null)
                        View.DisplayRemovedObject();
                    else if (item!=null)
                    {
                        liteRepositorySettings settings = Service.SettingsGetByRepositoryIdentifier(item.Repository);
                        dtoDisplayObjectRepositoryItem obj = Create(dto.Link, item, version);
                        View.ItemType = obj.Type;
                        if (dto.PlaceHolders.Where(p => !String.IsNullOrEmpty(p.Text)).Any() && (ValidateActionMode(dAction, DisplayActionMode.defaultAction) || ValidateActionMode(dAction, DisplayActionMode.text)))
                            View.DisplayPlaceHolders(dto.PlaceHolders.Where(p => !String.IsNullOrEmpty(p.Text)).ToList());
                        actions = AnalyzeModuleLinkItem(settings,obj, getOnlyActions, dto, dAction, actionsToDisplay);
                    }
                }
                return actions;
            }
            private dtoDisplayObjectRepositoryItem Create(liteModuleLink link, liteRepositoryItem item, liteRepositoryItemVersion version)
            {
                dtoDisplayObjectRepositoryItem dto = new dtoDisplayObjectRepositoryItem(link,item,version);
                String description = View.ExtraInfoDescription;
                if(View.DisplayExtraInfo && !String.IsNullOrWhiteSpace(description))
                    dto.SetDescription(description);
                if (View.DisplayLinkedBy || View.DisplayUploader)
                    Service.UpdateUserInfo(link, dto, View.GetUnknownUserName());
                return dto;
            }
            private List<dtoModuleActionControl> AnalyzeModuleLinkItem(liteRepositorySettings settings,dtoDisplayObjectRepositoryItem obj, Boolean getOnlyActions, dtoObjectRenderInitializer dto, DisplayActionMode dAction, StandardActionType actionsToDisplay)
            {
                List<dtoModuleActionControl> actions = new List<dtoModuleActionControl>();
                if (obj.Link != null)
                {
                    View.Availability = obj.Availability;
                    Boolean isReadyToPlay = (obj.Availability == ItemAvailability.available || obj.Availability== ItemAvailability.waitingsettings);
                    if (!getOnlyActions){
                        if ( ValidateActionMode(dAction, DisplayActionMode.text) || ValidateActionMode(dAction, DisplayActionMode.textDefault) || !isReadyToPlay)
                        {
                            View.Display = DisplayActionMode.text;
                            DisplayTextInfo(obj);
                        }
                        else if (ValidateActionMode(dAction, DisplayActionMode.defaultAction) || ValidateActionMode(dAction, DisplayActionMode.adminMode))
                            DisplayDefaultAction(settings, obj, dto, dAction);
                    }

                    if (isReadyToPlay || ValidateActionMode(dAction, DisplayActionMode.adminMode))
                        actions = GenerateActions(settings,obj, dto, dAction);
                    if (ValidateActionMode(dAction, DisplayActionMode.actions) && actionsToDisplay != StandardActionType.None)
                        View.DisplayActions(actions.Where(a => ((int)a.ControlType & (int)actionsToDisplay) > 0).ToList());
                }
                return actions;
            }

            private void DisplayTextInfo(dtoDisplayObjectRepositoryItem obj)
            {
                if (obj.Item.Type != ItemType.Folder)
                    View.DisplayTextAction(obj);
                else
                    View.DisplayTextAction(obj.Item.Name, ((ModuleRepository.ActionType)obj.Link.Action));
            }
            private void DisplayDefaultAction(
                liteRepositorySettings settings, 
                dtoDisplayObjectRepositoryItem obj, 
                dtoObjectRenderInitializer dto, 
                DisplayActionMode dAction)
            {
                Int32 idModule = obj.Link.SourceItem.ServiceID;
                String moduleCode = obj.Link.SourceItem.ServiceCode;
                View.SourceIdModule = idModule;
                View.SourceModuleCode = moduleCode;
                Int32 idCommunity = obj.Item.IdCommunity;
                Boolean notSaveStat = (dAction == DisplayActionMode.adminMode);
                DisplayMode mode = View.OverrideItemDisplayMode;
                if (mode == DisplayMode.none)
                    mode = obj.DisplayMode;
                switch (obj.Link.Action)
                {
                    case (int)ModuleRepository.ActionType.AddFolder:
                    case (int)ModuleRepository.ActionType.AddFile:
                    case (int)ModuleRepository.ActionType.AddLink:
                    case (int)ModuleRepository.ActionType.AddInternalFile:
                        View.DisplayActiveAction(obj.Name, ((ModuleRepository.ActionType)obj.Link.Action));
                        break;
                    case (int)ModuleRepository.ActionType.DownloadFile:
                        switch(obj.Type){
                            case ItemType.Link:
                                View.DisplayActiveAction(obj, mode, obj.Url, dto.RefreshContainerPage, false, dto.SaveObjectStatistics, dto.SaveOwnerStatistics); 
                                break;
                            default:
                                if (obj.Item.IsDownloadable)
                                    View.DisplayActiveAction(
                                        obj,
                                        mode, 
                                        RootObject.DownloadFromModule(
                                            "", 
                                            obj.IdItem, 
                                            obj.IdVersion,
                                            obj.DisplayName,
                                            mode,
                                            UserContext.WorkSessionID.ToString(),
                                            ((long)idModule), 
                                            obj.Link.Id, 
                                            (!dto.SaveObjectStatistics || notSaveStat)), 
                                        dto.RefreshContainerPage,
                                        false, 
                                        dto.SaveObjectStatistics, 
                                        dto.SaveOwnerStatistics); 
                                else
                                    DisplayTextInfo(obj);
                                break;
                        }
                        break;
                    case (int)ModuleRepository.ActionType.PlayFile:
                        String url = "";
                        switch (obj.Type) {
                            case ItemType.Multimedia:
                            case ItemType.ScormPackage:
                                Boolean onModalPage = dto.ForceOnModalPage || (mode == DisplayMode.downloadOrPlayOrModal || mode == DisplayMode.inModal);
                                Boolean setPrevious = dto.SetPreviousPage;
                                String backUrl = (setPrevious ? View.DestinationUrl : "");
                                setPrevious = setPrevious && !String.IsNullOrEmpty(backUrl);
                                url = RootObject.PlayFromModule(obj.IdItem, obj.UniqueId, obj.IdVersion, obj.UniqueIdVersion, obj.Type, obj.Link.Id, UserContext.Language.Code, (!dto.SaveObjectStatistics || notSaveStat), dto.RefreshContainerPage, onModalPage, dto.SaveOwnerStatistics, setPrevious, backUrl);

                                View.DisplayActiveAction(obj, mode, url, dto.RefreshContainerPage, onModalPage, dto.SaveObjectStatistics, dto.SaveOwnerStatistics);
                                break;
                            case ItemType.Link:
                                View.DisplayActiveAction(obj, mode, obj.Url, dto.RefreshContainerPage, false, dto.SaveObjectStatistics, dto.SaveOwnerStatistics);
                                break;
                            default:
                                DisplayTextInfo(obj);
                                break;
                        }
 
                        break;
                }
            }
            private List<dtoModuleActionControl> GenerateActions(liteRepositorySettings settings,dtoDisplayObjectRepositoryItem obj, dtoObjectRenderInitializer dto, DisplayActionMode dAction)
            {
                List<dtoModuleActionControl> actions = new List<dtoModuleActionControl>();
                Int32 idModule = obj.Link.SourceItem.ServiceID;
                String moduleCode = obj.Link.SourceItem.ServiceCode;
       
                String baseUrl = View.GetIstanceFullUrl();
                Boolean notSaveStat = (dAction == DisplayActionMode.adminMode);
                DisplayMode mode = View.OverrideItemDisplayMode;
                if (mode == DisplayMode.none)
                    mode = obj.DisplayMode;
                switch (obj.Type)
                {
                    case ItemType.Link:
                        actions.Add(new dtoModuleActionControl(View.SanitizeLinkUrl(obj.Url), StandardActionType.Play, true));
                        actions.Add(new dtoModuleActionControl(View.SanitizeLinkUrl(obj.Url), StandardActionType.DownloadItem, true));
                        break;
                    case ItemType.File:
                        if (obj.Availability == ItemAvailability.available)
                        {
                            actions.Add(new dtoModuleActionControl(RootObject.DownloadFromModule(baseUrl, obj.IdItem, obj.IdVersion, obj.DisplayName, mode, UserContext.WorkSessionID.ToString(), ((long)idModule), obj.Link.Id, (!dto.SaveObjectStatistics || notSaveStat)), StandardActionType.Play, true));
                            actions.Add(new dtoModuleActionControl(RootObject.DownloadFromModule(baseUrl, obj.IdItem, obj.IdVersion, obj.DisplayName, mode, UserContext.WorkSessionID.ToString(), ((long)idModule), obj.Link.Id, (!dto.SaveObjectStatistics || notSaveStat)), StandardActionType.DownloadItem, true));
                        }
                        break;
                    case ItemType.Multimedia:
                    case ItemType.ScormPackage:
                        if (obj.Availability == ItemAvailability.available)
                        {
                            Boolean onModalPage = dto.ForceOnModalPage || (mode == DisplayMode.downloadOrPlayOrModal || mode == DisplayMode.inModal);
                            Boolean setPrevious = dto.SetPreviousPage;
                            String backUrl = (setPrevious ? View.DestinationUrl : "");
                            setPrevious = setPrevious && !String.IsNullOrEmpty(backUrl);
                            String url = baseUrl + RootObject.PlayFromModule(obj.IdItem, obj.UniqueId, obj.IdVersion, obj.UniqueIdVersion, obj.Type, obj.Link.Id, UserContext.Language.Code, (!dto.SaveObjectStatistics || notSaveStat), dto.RefreshContainerPage, onModalPage, dto.SaveOwnerStatistics, setPrevious, backUrl);

                            actions.Add(new dtoModuleActionControl(url, StandardActionType.Play, !onModalPage) { OnModalPage = onModalPage, RefreshContainerPage = dto.RefreshContainerPage, SaveLinkStatistics = dto.SaveObjectStatistics });
                            if (obj.Type == ItemType.ScormPackage)
                            {
                                actions.Add(new dtoModuleActionControl(baseUrl + RootObject.Statistics(ItemStatisticType.Scorm, obj.IdItem, obj.Link.DestinationItem.ObjectIdVersion, obj.Link.Id), StandardActionType.ViewAdvancedStatistics, false));
                                actions.Add(new dtoModuleActionControl(baseUrl + RootObject.UserStatistics(ItemStatisticType.Scorm, UserContext.CurrentUserID, obj.IdItem, obj.Link.DestinationItem.ObjectIdVersion, obj.Link.Id), StandardActionType.ViewUserStatistics, false));
                                actions.Add(new dtoModuleActionControl(baseUrl + RootObject.MyStatistics(ItemStatisticType.Scorm, obj.IdItem, obj.Link.DestinationItem.ObjectIdVersion, obj.Link.Id), StandardActionType.ViewPersonalStatistics, false));
                            }
                        }
                        if (obj.Availability == ItemAvailability.available || obj.Availability == ItemAvailability.waitingsettings)
                        {
                            switch (obj.Type)
                            {
                                case ItemType.Multimedia:
                                    if (obj.Item.IsInternal)
                                        actions.Add(new dtoModuleActionControl(baseUrl + RootObject.EditMultimediaSettings(obj.IdItem, obj.Link.DestinationItem.ObjectIdVersion, obj.Link.Id), StandardActionType.EditMetadata, false));
                                    else
                                        actions.Add(new dtoModuleActionControl(baseUrl + RootObject.EditMultimediaSettings(obj.IdItem, obj.Link.DestinationItem.ObjectIdVersion, obj.Item.IdFolder, ""), StandardActionType.EditMetadata, false));
                                    break;
                                default:
                                    if (obj.Item.IsInternal)
                                        actions.Add(new dtoModuleActionControl(baseUrl + RootObject.EditScormSettings(obj.IdItem, obj.Link.DestinationItem.ObjectIdVersion, obj.Link.Id), StandardActionType.EditMetadata, false));
                                    else
                                        actions.Add(new dtoModuleActionControl(baseUrl + RootObject.EditScormSettings(obj.IdItem, obj.Link.DestinationItem.ObjectIdVersion, obj.Item.IdFolder, ""), StandardActionType.EditMetadata, false));
                                    break;
                            }
                        }
                        if (obj.Availability== ItemAvailability.available && obj.IsDownlodable )
                            actions.Add(new dtoModuleActionControl(RootObject.DownloadFromModule(baseUrl, obj.IdItem, obj.Link.DestinationItem.ObjectIdVersion, obj.DisplayName, mode, UserContext.WorkSessionID.ToString(), ((long)idModule), obj.Link.Id, (!dto.SaveObjectStatistics || notSaveStat)), StandardActionType.DownloadItem, true));
                        break;
                    case ItemType.Folder:
                        break;
                }
                return actions;
            }
            private Boolean ValidateActionMode(lm.Comol.Core.ModuleLinks.DisplayActionMode current, lm.Comol.Core.ModuleLinks.DisplayActionMode required)
            {
                return ((long)current & (long)required) > 0;
            }

            public String GetDescriptionByLink(ModuleLink link)
            {
                return (link == null || link.DestinationItem == null) ? "" : GetDescriptionByLink(link.Action, link.DestinationItem.ObjectLongID, link.DestinationItem.ObjectIdVersion);
            }
            public String GetDescriptionByLink(liteModuleLink link)
            {
                return (link == null || link.DestinationItem == null) ? "" : GetDescriptionByLink(link.Action,link.DestinationItem.ObjectLongID, link.DestinationItem.ObjectIdVersion);
            }
            private String GetDescriptionByLink(long idAction, long idItem, long idVersion)
            {
                liteRepositoryItemVersion version = Service.ItemGetVersion(idItem, idVersion);
                if (version == null)
                    return "";
                else
                {
                    if (version.Type != ItemType.Folder)
                        return View.GetDisplayItemDescription(version.DisplayName, version.Extension, version.DownloadFullName, version.Type, version.Size, FolderSizeItem.FormatBytes(version.Size));
                    else
                        return "";
                }
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