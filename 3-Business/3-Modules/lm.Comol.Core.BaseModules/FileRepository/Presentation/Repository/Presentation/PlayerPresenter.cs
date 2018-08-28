using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.FileRepository.Business;
using lm.Comol.Core.FileRepository.Domain;
using lm.Comol.Core.BaseModules.FileRepository.Business;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public class PlayerPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private ServiceRepository service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewPlayer View
            {
                get { return (IViewPlayer)base.View; }
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
            public PlayerPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public PlayerPresenter(iApplicationContext oContext, IViewPlayer view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion


        public void InitView(String plattformId, long idItem,Guid uniqueId, long idVersion,Guid uniqueIdVersion, ItemType type,long idLink , String language, Boolean saveCompleteness, Boolean isOnModal, Boolean refreshContainer, Boolean saveStatistics) 
        {
            Guid playUniqueSessionId = Guid.NewGuid();
            Guid workingSessionId = UserContext.WorkSessionID;
            String playSessionId = plattformId + "_" + playUniqueSessionId.ToString();
            Int32 idUser = UserContext.CurrentUserID;
            View.IdItem = idItem;
            View.IdLink = idLink;
            View.IdVersion = idVersion;
            View.ItemType = type;
            View.SaveStatistics = saveStatistics;
            View.PlayUniqueSessionId = playUniqueSessionId;
            View.WorkingSessionId = workingSessionId;
            View.PlaySessionId = playSessionId;
            if (SessionTimeout())
            {
                if (isOnModal)
                    View.DisplaySessionExpired();
                else
                    View.DisplaySessionTimeout();
            }
            else if (uniqueId == Guid.Empty || uniqueIdVersion == Guid.Empty)
                InitViewForRedirect(idItem, uniqueId, idVersion, uniqueIdVersion, type, idLink, language, saveCompleteness, isOnModal, refreshContainer, saveStatistics);
            else
            {
                if (String.IsNullOrWhiteSpace(language))
                    language = "";
                liteRepositoryItem item = Service.ItemGet(idItem);
                liteRepositoryItemVersion version = Service.ItemGetVersion(idItem, idVersion);
                if (version != null && version.Id != idVersion)
                    View.IdVersion = version.Id;
                if (item == null)
                    View.DisplayUnknownItem();
                else
                {
                    liteFileTransfer fileTransfer = null;
                    type = item.Type;
                    View.ItemType = type;
                    switch (type)
                    {
                        case ItemType.File:
                        case ItemType.Folder:
                        case ItemType.Link:
                        case ItemType.SharedDocument:
                        case ItemType.VideoStreaming:
                            View.DisplayMessage(item.DisplayName, item.Extension, type, Domain.PlayerErrors.InvalidType);
                            break;
                        case ItemType.ScormPackage:
                        case ItemType.Multimedia:
                            liteModuleLink link = null;
                            Boolean hasPermissions = false;
                            View.ItemIdCommunity = item.IdCommunity;
                            fileTransfer = Service.FileTransferGet(idItem, version.Id);
                            if (idLink == 0)
                                hasPermissions = Service.HasPermissionToSeeItem(idUser, item, version, ModuleRepository.ActionType.PlayFile);
                            else
                            {
                                link = CurrentManager.Get<liteModuleLink>(idLink);
                                if (link == null)
                                {
                                    View.DisplayMessage(item.DisplayName, item.Extension, item.Type, Domain.PlayerErrors.NoPermissionToPlay);
                                    return;
                                }
                                else
                                {
                                    if (link.DestinationItem.ObjectIdVersion > 0 && version != null && version.Id != link.DestinationItem.ObjectIdVersion)
                                        version = Service.ItemGetVersion(idItem, link.DestinationItem.ObjectIdVersion);
                                    ModuleObject obj = ModuleObject.CloneObject(link.DestinationItem);
                                    obj.ObjectIdVersion = (version != null ? version.Id : obj.ObjectIdVersion);
                                    saveCompleteness = link.AutoEvaluable && saveCompleteness;
                                    hasPermissions = View.HasPermissionForLink(UserContext.CurrentUserID, idLink, obj,item.Type, link.SourceItem.ServiceID, link.SourceItem.ServiceCode);
                                    View.ItemIdCommunity = link.SourceItem.CommunityID;
                                }
                            }
                            Boolean playerLoaded = false;
                            if (version == null)
                                View.DisplayPlayUnavailable(item.DisplayName, item.Extension, type, (idVersion > 0), ItemAvailability.notavailable, item.Status);
                            else if (type == ItemType.Multimedia && fileTransfer != null && String.IsNullOrEmpty(fileTransfer.DefaultDocumentPath))
                                View.DisplayMessage(item.DisplayName, item.Extension, item.Type, Domain.PlayerErrors.InvalidSettings);
                            else if ((type == ItemType.Multimedia || type == ItemType.ScormPackage) && fileTransfer == null)
                                View.DisplayMessage(item.DisplayName, item.Extension, item.Type, Domain.PlayerErrors.InvalidTransfer);
                            else if (version.Availability == ItemAvailability.available)
                            {
                                playerLoaded = hasPermissions;
                                if (hasPermissions)
                                    LoadPlayer(idUser, playSessionId, workingSessionId, item, version, fileTransfer, link, language, saveCompleteness, isOnModal, refreshContainer, saveStatistics);
                                else
                                    View.DisplayMessage(item.DisplayName, item.Extension, item.Type, Domain.PlayerErrors.NoPermissionToPlay);
                            }
                            else
                                View.DisplayPlayUnavailable(item.DisplayName, item.Extension, type, (idVersion > 0 && item.HasVersions), item.Availability, item.Status);

                            if (!playerLoaded && !View.PreloadIsOnModal)
                            {
                                Boolean setBackUrl = View.PreloadSetBackUrl;
                                String backUrl = View.PreloadBackUrl;
                                View.BackUrl = (setBackUrl ? backUrl : "");
                                View.SetPageBackUrl(backUrl);
                            }
                            break;
                    }
                }
            }
        }


        public void InitViewForRedirect(long idItem, Guid uniqueId, long idVersion, Guid uniqueIdVersion, ItemType type, long idLink, String language, Boolean saveCompleteness, Boolean isOnModal, Boolean refreshContainer, Boolean saveStatistics)
        {
            liteRepositoryItem item = Service.ItemGet(idItem);
            liteRepositoryItemVersion version = Service.ItemGetVersion(idItem, idVersion);
            if (item == null)
                View.DisplayUnknownItem();
            else if (version==null)
                View.DisplayPlayUnavailable(item.DisplayName, item.Extension, type, (idVersion > 0), ItemAvailability.notavailable, item.Status);
            else
            {
                View.RedirectTo(RootObject.FullPlay(idItem, version.UniqueIdItem, version.Id, version.UniqueIdVersion, version.Type, idLink, language, !saveStatistics, refreshContainer, isOnModal, saveCompleteness, View.PreloadSetBackUrl, View.PreloadBackUrl));
            }
        }
        private void LoadPlayer(Int32 idUser,String playSessionId,Guid workingSessionId, liteRepositoryItem item, liteRepositoryItemVersion version, liteFileTransfer fileTransfer, liteModuleLink link, String language, Boolean saveCompleteness, Boolean isOnModal, Boolean refreshContainer, Boolean saveStatistics)
        {
            List<litePlayerSettings> players = Service.PlayerGetSettings();
            if (players == null || (version != null && !players.Any(p => p.Id == version.IdPlayer && !String.IsNullOrEmpty(p.PlayUrl) && !String.IsNullOrEmpty(p.PlayerRenderUrl) && !String.IsNullOrEmpty(p.ModalPlayerRenderUrl))))
                View.DisplayMessage(item.DisplayName, item.Extension, item.Type, Domain.PlayerErrors.PlayerUnavailable);
            else
            {
                String playUrl = "";
                String ajaxActionUrl = "";
                litePlayerSettings player = players.FirstOrDefault(p => p.Id == version.IdPlayer);
                Int32 idCommunity = View.ItemIdCommunity;
                Int32 idAction = (link == null) ? (int)ModuleRepository.ActionType.PlayFile : link.Action;
                if (saveStatistics)
                    Service.StatisticsAddPlay(idUser, item.Repository, version, idCommunity, (long)ModuleRepository.ActionType.PlayFile, playSessionId);

                ajaxActionUrl = RootObject.AjaxAction(idCommunity, item.Id, version.UniqueIdItem, version.Id, version.UniqueIdVersion,version.Type,(link== null ? 0 : link.Id),idAction, workingSessionId, playSessionId, isOnModal);
                switch (version.Type)
                {
                    case ItemType.Multimedia:
                        if (saveCompleteness && link !=null)
                            View.SaveLinkEvaluation(idUser,link.Id);

                           

                        playUrl = player.PlayUrl.Replace("#" + PlayerPlaceHolders.defaultDocumentPath + "#",System.Web.HttpUtility.HtmlEncode(fileTransfer.DefaultDocumentPath.Replace("\\", "/")));
                        playUrl += (!playUrl.Contains("?")) ? "?" : "&";

                        playUrl += QueryKeyNames.wSessionId.ToString() + "=" + playSessionId;
                        playUrl += "&" + QueryKeyNames.idUser.ToString() + "=" + idUser.ToString();
                        if (link != null)
                            playUrl += "&" + QueryKeyNames.idLink.ToString() + "=" + link.Id.ToString();
                        playUrl += "&" + QueryKeyNames.uniqueIdVersion.ToString() + "=" + version.UniqueIdVersion.ToString();
                        if (isOnModal)
                            View.DisplayClosingToolBar();
                        break;
                    //case ItemType.ScormPackage:
                    //    Service.ScormAddPendingEvaluation(item, version,UserContext.CurrentUserID, (link==null ? 0 : link.Id));
                    //    if (saveStatistics)
                    //    {
                    //        using (NHibernate.ISession session = View.GetScormSession(player.MappingPath))
                    //        {
                    //            lm.Comol.Modules.ScormStat.Business.ScormService service = new Modules.ScormStat.Business.ScormService(AppContext, session);
                    //            DateTime referenceTime = DateTime.Now;

                    //            lm.Comol.Core.FileRepository.Domain.dtoPackageEvaluation dto = service.EvaluatePackage_NEW(idUser, playSessionId, item.Id, item.UniqueId, version.Id, version.UniqueIdVersion, out referenceTime);
                                
                    //            if (dto != null)
                    //            {
                    //                dto.IdLink = (link == null) ? 0 : link.Id;
                    //                lm.Comol.Core.FileRepository.Domain.ScormPackageUserEvaluation saved = Service.ScormSaveEvaluation(dto, idUser, referenceTime, false, true);
                    //                if (saveCompleteness && saved != null && link != null)
                    //                {
                    //                    if (saved.ModuleCode == View.EduPathModuleCode && link.Id > 0)
                    //                        View.SaveLinkEvaluation(idUser, link.Id, saved);
                    //                }
                    //            }
                    //        }
                    //    }

                    //    playUrl = player.PlayUrl;
                    //    playUrl = playUrl.Replace("#" + PlayerPlaceHolders.idUser.ToString() + "#", idUser.ToString());
                    //    playUrl = playUrl.Replace("#" + PlayerPlaceHolders.courseId.ToString() + "#", version.UniqueIdVersion.ToString().Replace(" ", "%20").Replace("\\", "%2F").Replace("/", "%2F"));
                    //    playUrl = playUrl.Replace("#" + PlayerPlaceHolders.workingSessionId.ToString() + "#", playSessionId);
                    //    playUrl = playUrl.Replace("#" + PlayerPlaceHolders.dbIdentifier.ToString() + "#", player.DBidentifier);

                    //    if (!saveStatistics && !String.IsNullOrEmpty(player.NoSaveStatParameter))
                    //        playUrl += (playUrl.Contains("?") ? "&" : "?") + player.NoSaveStatParameter;
                    //    playUrl +=RootObject.PlayBaseParameters(!saveStatistics, refreshContainer, isOnModal, saveCompleteness);
                    //    playUrl += RootObject.UrlItemParameters(false, version.IdItem, version.Id, (link != null ? link.Id : 0));
                    //    playUrl += RootObject.UrlGuidParameters(version.UniqueIdItem, version.UniqueIdVersion);
                        
                    //    break;
                }
                View.InitializePlayer((isOnModal ? player.ModalPlayerRenderUrl : player.PlayerRenderUrl), playUrl, ajaxActionUrl, item.DisplayName, item.Type);
            }
                
        }
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
    }
}