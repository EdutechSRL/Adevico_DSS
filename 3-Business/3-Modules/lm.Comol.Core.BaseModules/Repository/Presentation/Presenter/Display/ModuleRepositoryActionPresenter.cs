using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.Repository.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.Business;
using lm.Comol.Core.ModuleLinks;
using lm.Comol.Core.BaseModules.Repository.Domain;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public class ModuleRepositoryActionPresenter  : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private int _ModuleID;
            private ServiceCommunityRepository _Service;

            private int ModuleID
            {
                get
                {
                    if (_ModuleID <= 0)
                    {
                        _ModuleID = this.Service.ServiceModuleID();
                    }
                    return _ModuleID;
                }
            }
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewModuleRepositoryAction View
            {
                get { return (IViewModuleRepositoryAction)base.View; }
            }
            private ServiceCommunityRepository Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceCommunityRepository(AppContext);
                    return _Service;
                }
            }
            public ModuleRepositoryActionPresenter(iApplicationContext oContext):base(oContext){
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ModuleRepositoryActionPresenter(iApplicationContext oContext, IViewModuleRepositoryAction view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

            public void InitView(BaseCommunityFile item)
            {
                View.InsideOtherModule = false;
            }
            public void InitViewFromModule(BaseCommunityFile item)
            {
                View.InsideOtherModule = true;
            }
            public void InitView(dtoModuleDisplayActionInitializer dto){
                View.InsideOtherModule = true;
                InitializeControlByLink(dto, (Display(dto.Display, DisplayActionMode.defaultAction) ? StandardActionType.Play : StandardActionType.None) );
            }
            public void InitView(dtoModuleDisplayActionInitializer dto, StandardActionType actionsToDisplay)
            {
                View.InsideOtherModule = true;
                InitializeControlByLink(dto, actionsToDisplay);
            }
            public List<dtoModuleActionControl> InitRemoteControlView(dtoModuleDisplayActionInitializer dto, StandardActionType actionsToDisplay)
            {
                View.InsideOtherModule = true;
                return InitializeControlByLink(dto, actionsToDisplay);
            }
            private List<dtoModuleActionControl> InitializeControlByLink(dtoModuleDisplayActionInitializer dto, StandardActionType actionsToDisplay)
            {
                List<dtoModuleActionControl> actions = new List<dtoModuleActionControl>();
                if (dto.Link == null || dto.Link.DestinationItem == null) //|| UserContext.isAnonymous
                    View.DisplayEmptyAction();
                else if (dto.Link.DestinationItem.ServiceCode== CoreModuleRepository.UniqueID){
                    View.ItemIdentifier = "link_" + dto.Link.Id.ToString();
                    actions = AnalyzeActions(dto, actionsToDisplay);
                }
                else
                    View.DisplayEmptyAction();
                return actions;
            }

            private List<dtoModuleActionControl> AnalyzeActions(dtoModuleDisplayActionInitializer dto, StandardActionType actionsToDisplay)
            {
                List<dtoModuleActionControl> actions = new List<dtoModuleActionControl>();
                View.ContainerCSS = dto.ContainerCSS;
                View.IconSize = dto.IconSize;

                if (dto.Link == null || dto.Link.DestinationItem == null)
                    View.DisplayEmptyAction();
                else
                {
                    View.Display = dto.Display;
                    liteBaseCommunityFile item = CurrentManager.Get<liteBaseCommunityFile>(dto.Link.DestinationItem.ObjectLongID);
                    View.ItemType = RepositoryItemType.None; 
                    if (item == null || (dto.Link.DestinationItem.ObjectLongID==0 && (dto.Link.Action == (int)CoreModuleRepository.ActionType.CreateFolder || dto.Link.Action == (int)CoreModuleRepository.ActionType.CreateFolder)))
                        View.DisplayRemovedObject();
                    else{
                        View.ItemType = item.RepositoryItemType;
                        if (dto.PlaceHolders.Where(p => !String.IsNullOrEmpty(p.Text)).Any() && (Display(dto.Display, DisplayActionMode.defaultAction) || Display(dto.Display, DisplayActionMode.text)))
                            View.DisplayPlaceHolders(dto.PlaceHolders.Where(p => !String.IsNullOrEmpty(p.Text)).ToList());
                        actions = AnalyzeModuleLinkItem(dto.Link, item, dto.Display, actionsToDisplay);
                    }
                }
                return actions;
            }

            private List<dtoModuleActionControl> AnalyzeModuleLinkItem(ModuleLink link, liteBaseCommunityFile item, DisplayActionMode display, StandardActionType actionsToDisplay)
            {
                List<dtoModuleActionControl> actions = new List<dtoModuleActionControl>();
                if (item != null) {
                    Boolean isReadyToPlay = IsReadyToPlay(item);
                    View.IsReadyToPlay = isReadyToPlay;
                    if (Display(display, DisplayActionMode.text) || Display(display, DisplayActionMode.textDefault) || !isReadyToPlay)
                    {
                        View.Display = DisplayActionMode.text;
                        DisplayTextInfo(item, link.Action);
                    }
                    else if (Display(display, DisplayActionMode.defaultAction) || Display(display, DisplayActionMode.adminMode))
                        DisplayDefaultAction(link, item, display);
                    if (isReadyToPlay)
                        actions = GenerateActions(link, item);
                    if (Display(display, DisplayActionMode.actions) && actionsToDisplay != StandardActionType.None)
                        View.DisplayActions(actions.Where(a => ((int)a.ControlType & (int)actionsToDisplay) > 0).ToList());
                }
                return actions;
            }

            private Boolean IsReadyToPlay(liteBaseCommunityFile item)
            {
                switch(item.RepositoryItemType){
                    case RepositoryItemType.Multimedia:
                    case RepositoryItemType.ScormPackage:
                        return Service.GetItemTransferStatus(item.UniqueID) == DomainModel.Repository.TransferStatus.Completed;
                    default:
                        return true;
                }
            }




            private void DisplayTextInfo(liteBaseCommunityFile item, Int32 idAction)
            {
                if (item.isFile)
                    View.DisplayItem(item.DisplayName, item.Extension,item.Size, item.RepositoryItemType);
                else {
                    switch (idAction)
                    {
                        case (int)CoreModuleRepository.ActionType.CreateFolder:
                            View.DisplayCreateFolder(item.Name);
                            break;
                        case (int)CoreModuleRepository.ActionType.UploadFile:
                            View.DisplayUploadFile(item.Name);
                            break;
                        case 0:
                            View.DisplayFolder(item.Name);
                            break;
                    }
                }
            }
            private void DisplayDefaultAction(ModuleLink link, liteBaseCommunityFile item, DisplayActionMode display)
            {
                if (typeof(liteCommunityFile) == item.GetType())
                {
                    View.ServiceCode = link.DestinationItem.ServiceCode;
                    View.ServiceID = link.DestinationItem.ServiceID;
                }
                else
                {
                    View.ServiceCode = link.SourceItem.ServiceCode;
                    View.ServiceID = link.SourceItem.ServiceID;
                }
                int idCommunity = 0;
                if (item.CommunityOwner != null)
                    idCommunity = item.CommunityOwner.Id;

                Boolean notSaveStat = (display == DisplayActionMode.adminMode);
                switch (link.Action)
                {
                    case (int)CoreModuleRepository.ActionType.CreateFolder:
                        View.DisplayCreateFolder(item.Name);
                        break;
                    case (int)CoreModuleRepository.ActionType.UploadFile:
                        View.DisplayUploadFile(item.Name);
                        break;
                    case (int)CoreModuleRepository.ActionType.DownloadFile:
                        View.DisplayItem(item.DisplayName, RootObject.DownloadFileFromModule(item.Id, (long)UserContext.CurrentUserID, UserContext.Language.Code, UserContext.WorkSessionID, View.ServiceID, idCommunity, link.Id, notSaveStat), item.Extension, item.Size, item.RepositoryItemType);
                        break;
                    case (int)CoreModuleRepository.ActionType.PlayFile:
                        View.ServiceCode = link.SourceItem.ServiceCode;
                        View.ServiceID = link.SourceItem.ServiceID;
                        String url = "";
                        if (typeof(liteModuleInternalFile) == item.GetType()) {
                            liteModuleInternalFile internalFile = (liteModuleInternalFile)item;
                            url= (internalFile.RepositoryItemType== RepositoryItemType.ScormPackage) ?
                                View.GetUrlForPlayScorm(link.Id, internalFile.Id, internalFile.UniqueID, idCommunity, View.ServiceID, internalFile.ServiceActionAjax, notSaveStat)
                                :
                                View.GetUrlForMultimediaFile() + RootObject.PlayMultimediaFileFromModule(internalFile.Id, internalFile.UniqueID, UserContext.CurrentUserID, UserContext.Language.Code, notSaveStat, View.ServiceID, idCommunity, link.Id, internalFile.ServiceActionAjax, View.PreLoadedContentView);
                        }
                        else {
                            url = (item.RepositoryItemType == RepositoryItemType.ScormPackage) ?
                                View.GetUrlForPlayScorm(link.Id, item.Id, item.UniqueID, idCommunity, link.SourceItem.ServiceID, notSaveStat)
                                :
                                View.GetUrlForMultimediaFile() + RootObject.PlayMultimediaFile(link.Id, item.Id, item.UniqueID, UserContext.CurrentUserID, UserContext.Language.Code, notSaveStat, View.PreLoadedContentView);
                        }
                        View.DisplayPlayItem(item.Name,url,item.RepositoryItemType );
                        break;
                    case 0:
                        View.DisplayFolder(item.Name);
                        break;
                }
            }
            private List<dtoModuleActionControl> GenerateActions(ModuleLink link, liteBaseCommunityFile item)
            {
                List<dtoModuleActionControl> actions = new List<dtoModuleActionControl>();
                String serviceCode = link.DestinationItem.ServiceCode;
                Int32 IdModule = link.DestinationItem.ServiceID;
                Boolean allowDownload = View.AllowDownload(item.RepositoryItemType) && item.IsDownloadable && item.isFile;
                allowDownload = true;
                if (typeof(liteCommunityFile) != item.GetType())
                {
                    serviceCode = link.SourceItem.ServiceCode;
                    IdModule = link.SourceItem.ServiceID;
                }
                String baseUrl = View.GetBaseUrl;
                int idCommunity = 0;
                if (item.CommunityOwner != null)
                    idCommunity = item.CommunityOwner.Id;
                Boolean notSaveStat = (View.Display == DisplayActionMode.adminMode);
                switch (item.RepositoryItemType){
                    case RepositoryItemType.FileStandard:
                        actions.Add(new dtoModuleActionControl(RootObject.DownloadFileFromModule(item.Id, (long)UserContext.CurrentUserID, UserContext.Language.Code, UserContext.WorkSessionID, View.ServiceID, idCommunity, link.Id, notSaveStat), StandardActionType.Play, true));
                        actions.Add(new dtoModuleActionControl(RootObject.DownloadFileFromModule(item.Id, (long)UserContext.CurrentUserID, UserContext.Language.Code, UserContext.WorkSessionID, View.ServiceID, idCommunity, link.Id, notSaveStat), StandardActionType.DownloadItem, true));
                        break;
                    case RepositoryItemType.ScormPackage:
                        if (typeof(liteModuleInternalFile) == item.GetType()){
                            liteModuleInternalFile internalFile = (liteModuleInternalFile)item;
                            actions.Add(new dtoModuleActionControl(View.GetUrlForPlayScorm(link.Id, internalFile.Id, internalFile.UniqueID, idCommunity, link.SourceItem.ServiceID, internalFile.ServiceActionAjax, notSaveStat), StandardActionType.Play, true));
                        }
                        else
                            actions.Add(new dtoModuleActionControl(View.GetUrlForPlayScorm(link.Id, item.Id, item.UniqueID, idCommunity, IdModule, notSaveStat), StandardActionType.Play, true));
                        actions.Add(new dtoModuleActionControl(baseUrl + RootObject.EditScormPackageSettings(item.Id, link.Id, View.DestinationUrl, View.PreLoadedContentView), StandardActionType.EditMetadata, false));
                        actions.Add(new dtoModuleActionControl(baseUrl + RootObject.ManagementScormStatistics(item.Id, View.DestinationUrl, View.PreLoadedContentView), StandardActionType.ViewAdvancedStatistics, false));
                        actions.Add(new dtoModuleActionControl(baseUrl + RootObject.ManagementScormStatistics(item.Id, View.ForUserId, View.DestinationUrl, View.PreLoadedContentView), StandardActionType.ViewUserStatistics, false));

                        actions.Add(new dtoModuleActionControl(baseUrl + RootObject.UserScormStatistics(item.Id, View.DestinationUrl, View.PreLoadedContentView), StandardActionType.ViewPersonalStatistics, false));
                        if (allowDownload)
                            actions.Add(new dtoModuleActionControl(RootObject.DownloadFileFromModule(item.Id, (long)UserContext.CurrentUserID, UserContext.Language.Code, UserContext.WorkSessionID, View.ServiceID, idCommunity, link.Id, notSaveStat), StandardActionType.DownloadItem, true));
                        break;
                    case RepositoryItemType.Multimedia:
                    case RepositoryItemType.VideoStreaming:
                        if (typeof(liteModuleInternalFile) == item.GetType())
                        {
                            liteModuleInternalFile internalFile = (liteModuleInternalFile)item;
                            actions.Add(new dtoModuleActionControl(RootObject.PlayMultimediaFileFromModule(internalFile.Id, internalFile.UniqueID, UserContext.CurrentUserID, UserContext.Language.Code, notSaveStat, link.SourceItem.ServiceID, idCommunity, link.Id, internalFile.ServiceActionAjax, View.PreLoadedContentView), StandardActionType.Play, true));
                        }
                        else
                             actions.Add(new dtoModuleActionControl(RootObject.PlayMultimediaFile(link.Id, item.Id, item.UniqueID, UserContext.CurrentUserID, UserContext.Language.Code,notSaveStat, View.PreLoadedContentView), StandardActionType.Play, true));

                         actions.Add(new dtoModuleActionControl(baseUrl + RootObject.EditMultimediaFileSettings(item.Id, link.Id, View.DestinationUrl, View.PreLoadedContentView), StandardActionType.EditMetadata, false));

                         if (allowDownload)
                             actions.Add(new dtoModuleActionControl(RootObject.DownloadFileFromModule(item.Id, (long)UserContext.CurrentUserID, UserContext.Language.Code, UserContext.WorkSessionID, View.ServiceID, idCommunity, link.Id, notSaveStat), StandardActionType.DownloadItem, true));
                        break;
                    case RepositoryItemType.Folder:
                        break;
                }
                actions.Where(a=> a.LinkUrl.Contains("//")).ToList().ForEach(a=> a.LinkUrl=  a.LinkUrl.Replace("//","/"));
                return actions;
            }
            
            private Boolean Display(DisplayActionMode current, DisplayActionMode required)
            {
                return ((long)current & (long)required) > 0;
            }

            public String GetDescriptionByLink(ModuleLink link) {
                String result = "";
                if (link == null || link.DestinationItem == null)
                    result = "";
                else {
                    liteBaseCommunityFile item = CurrentManager.Get<liteBaseCommunityFile>(link.DestinationItem.ObjectLongID);
                    if (item == null)
                        result = "";
                    else {
                        if (item.isFile)
                            result = View.GetDisplayItemDescription(item.DisplayName, item.Extension, item.Size, item.RepositoryItemType);
                        else
                        {
                            switch (link.Action)
                            {
                                case (int)CoreModuleRepository.ActionType.CreateFolder:
                                    result = View.CreateFolderDescription(item.Name);
                                    break;
                                case (int)CoreModuleRepository.ActionType.UploadFile:
                                    result = View.UploadFileDescription(item.Name);
                                    break;
                                case 0:
                                    result = View.FolderDescription(item.Name);
                                    break;
                            }
                        }
                    }
                }
                return result;
            }
    }
}