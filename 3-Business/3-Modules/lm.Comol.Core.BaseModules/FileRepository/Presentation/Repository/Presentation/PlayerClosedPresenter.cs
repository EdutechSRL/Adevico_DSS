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
    public class PlayerClosedPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private ServiceRepository service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewPlayerClosed View
            {
                get { return (IViewPlayerClosed)base.View; }
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
            public PlayerClosedPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public PlayerClosedPresenter(iApplicationContext oContext, IViewPlayerClosed view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion


        public void InitView(String playerSessionId,long idItem, long idVersion, ItemType type, long idLink, Boolean saveCompleteness, Boolean isOnModal, Boolean refreshContainer, Boolean saveStatistics) 
        {
            Guid playUniqueSessionId = Guid.NewGuid();
            Guid workingSessionId = UserContext.WorkSessionID;
            Int32 idUser = UserContext.CurrentUserID;
            View.IdItem = idItem;
            View.IdLink = idLink;
            View.IdVersion = idVersion;
            if (SessionTimeout())
                idUser = Service.ScormGetPlayIdUser(playerSessionId, idItem, idVersion);

            liteRepositoryItem item = Service.ItemGet(idItem);
            if (SessionTimeout() && idUser==0)
            {
                if (item == null)
                    View.DisplaySessionExpired();
                else
                    View.DisplayMessage(item.DisplayName, item.Extension, item.Type, Domain.PlayerClosedMessage.SessionExpired);
            }
            else
            {
                liteRepositoryItemVersion version = Service.ItemGetVersion(idItem, idVersion);
                if (version != null && version.Id != idVersion)
                    View.IdVersion = version.Id;
                if (item == null || version == null)
                    View.DisplayUnknownItem(type);
                else
                {
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
                            List<litePlayerSettings> players = Service.PlayerGetSettings();
                            if (players == null || (version != null && !players.Any(p => p.Id == version.IdPlayer && !String.IsNullOrEmpty(p.PlayUrl) && !String.IsNullOrEmpty(p.PlayerRenderUrl) && !String.IsNullOrEmpty(p.ModalPlayerRenderUrl))))
                                View.DisplayMessage(item.DisplayName, item.Extension, item.Type, Domain.PlayerErrors.PlayerUnavailable);
                            //else{
                            //    litePlayerSettings player = players.FirstOrDefault(p => p.Id == version.IdPlayer);
                            //    using (NHibernate.ISession session = View.GetScormSession(player.MappingPath)){

                            //        lm.Comol.Modules.ScormStat.Business.ScormService service = 
                            //            new Modules.ScormStat.Business.ScormService(AppContext, session);

                            //        DateTime referenceTime = DateTime.Now;

                            //        lm.Comol.Core.FileRepository.Domain.dtoPackageEvaluation dto = 
                            //            service.EvaluatePackage_NEW(
                            //                idUser, 
                            //                playerSessionId, 
                            //                item.Id, 
                            //                item.UniqueId, 
                            //                version.Id, 
                            //                version.UniqueIdVersion, 
                            //                out referenceTime);

                            //        if (dto != null && dto.IdItem > 0)
                            //        {
                            //            dto.IdLink = idLink;
                            //            lm.Comol.Core.FileRepository.Domain.ScormPackageUserEvaluation saved = Service.ScormSaveEvaluation(dto, idUser, referenceTime, true, true);
                            //            if (saved != null)
                            //            {
                            //                if (saveCompleteness && saved != null && idLink > 0 && saved.ModuleCode == View.EduPathModuleCode)
                            //                {
                            //                    View.SaveLinkEvaluation(saved);
                            //                    View.DisplayMessage(version.DisplayName, version.Extension, version.Type, Domain.PlayerClosedMessage.EvaluationSaved);
                            //                }
                            //                else
                            //                    View.DisplayMessage(version.DisplayName, version.Extension, version.Type, Domain.PlayerClosedMessage.Successful);
                            //            }
                            //            else
                            //                View.DisplayMessage(version.DisplayName, version.Extension, version.Type, Domain.PlayerClosedMessage.EvaluationNotSaved);
                            //        }
                            //        else
                            //            View.DisplayMessage(version.DisplayName, version.Extension, version.Type, Domain.PlayerClosedMessage.EvaluationNotSaved);
                                   
                            //    }
                            //}
                            break;
                    }
                }
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