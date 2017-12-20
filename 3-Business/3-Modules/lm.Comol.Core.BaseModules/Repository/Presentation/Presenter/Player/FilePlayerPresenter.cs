using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.Repository.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.Business;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public class FilePlayerPresenter  : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewFilePlayer View
            {
                get { return (IViewFilePlayer)base.View; }
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
            public FilePlayerPresenter(iApplicationContext oContext):base(oContext){
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public FilePlayerPresenter(iApplicationContext oContext, IViewFilePlayer view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitializeContext(){
             View.PlayerFileUniqueID= View.PlayerFileUniqueID;

            int idCommunity = View.PreloadedIdCommunity;
            if (idCommunity <= 0)
            {
                idCommunity = UserContext.CurrentCommunityID;
                BaseCommunityFile item = Service.GetItem(View.PlayerIdFile);
                if (item !=null && item.CommunityOwner !=null)
                    idCommunity= item.CommunityOwner.Id;
            }
            View.PlayerIdCommunity= idCommunity;
            View.PlayerIdFile= View.PreloadedIdFile;
            View.PlayerIdLink= View.PreloadedIdLink;
            View.PlayerIdModule= View.PreloadedIdModule;
            View.PlayerItemTypeID = View.PreloadedItemTypeID;
            View.PlayerSavingStatistics = View.PreloadedSavingStatistics;
            int idModule = View.PreloadedIdModule;
            String moduleCode = CurrentManager.GetModuleCode(idModule);

            if (String.IsNullOrEmpty(moduleCode) || idModule ==0){
                idModule = ModuleID; //CurrentManager.GetModuleID(CoreModuleRepository.UniqueID);
                moduleCode = CoreModuleRepository.UniqueID;
            }
            View.PlayerIdModule= idModule;
            View.PlayerModuleCode= moduleCode;
        }

        public void InitView(String plattformId)
        {
            long idLink = View.PlayerIdLink;
            int IdUser = UserContext.CurrentUserID;

            if (UserContext.isAnonymous)
                View.SendToSessionExpiredPage(View.PlayerIdCommunity, View.PreloadedLanguage);
            else{
                Guid playUniqueSessionId = Guid.NewGuid();
                View.PlayUniqueSessionId = playUniqueSessionId;
                String playSessionId = plattformId + "_" + playUniqueSessionId.ToString();
                View.PlaySessionId = playSessionId;

                Boolean allowPlay = false;
                BaseCommunityFile item = Service.GetItem(View.PlayerIdFile);
                if (item==null)
                    View.LoadFileNotExist();
                else{
                    if (idLink == 0 && View.PlayerIdModule == ModuleID)
                    {
                        int idCommunity = View.PlayerIdCommunity;
                        if (item.CommunityOwner != null && idCommunity == 0)
                        {
                            idCommunity = item.CommunityOwner.Id;
                            View.PlayerIdCommunity = idCommunity;
                        }
                        allowPlay = HasPermissionForRepository(item, IdUser, idCommunity, View.PlayerIdModule);
                    }
                    else if (idLink > 0)
                        allowPlay = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)CoreModuleRepository.Base2Permission.DownloadFile, View.GetPermissionToLink(IdUser, idLink, item, View.PlayerIdCommunity));

                    if (!allowPlay)
                        View.LoadFileNoPermission();
                    else if (item.RepositoryItemType== DomainModel.Repository.RepositoryItemType.FileStandard && item.RepositoryItemType== DomainModel.Repository.RepositoryItemType.Folder)
                        View.InvalidFileTypeToPlay(item.RepositoryItemType);
                    else if (Service.GetItemTransferStatus(item.UniqueID) ==  DomainModel.Repository.TransferStatus.Completed){
                        //if (item.RepositoryItemType== DomainModel.Repository.RepositoryItemType.ScormPackage)
                        Boolean autoEvaluate = (from l in CurrentManager.GetIQ<ModuleLink>() where l.Id == idLink && l.AutoEvaluable select l.Id).Any() && View.PlayerSavingStatistics;
                        
                        if (View.PlayerSavingStatistics)
                            Service.SaveUserAccessToFile(playSessionId, IdUser, item, idLink);
                        switch (item.RepositoryItemType) { 
                            case DomainModel.Repository.RepositoryItemType.ScormPackage:
                                View.LoadFileIntoPlayer(playSessionId,View.PlayerWorkingSessionID, item.UniqueID, IdUser, idLink, item.Id, item.RepositoryItemType);
                                break;
                            case DomainModel.Repository.RepositoryItemType.Multimedia:
                                MultimediaFileTransfer fileTransfer = Service.GetMultimediaFileTransfer(item);
                                if (fileTransfer == null || fileTransfer.DefaultDocument == null)
                                    View.InvalidFileTypeToPlay(item.RepositoryItemType);
                                else
                                {
                                    View.LoadMultimediaFileIntoPlayer(playSessionId,View.PlayerWorkingSessionID, item.UniqueID, IdUser, idLink, item.Id, fileTransfer.DefaultDocumentPath.Replace("\\", "/"));
                                    if (autoEvaluate)
                                        View.SaveLinkEvaluation(idLink, IdUser);
                                }
                                break;
                        }
                    }
                    else
                        View.LoadFileNoReadyToPlay(item.RepositoryItemType, Service.GetItemTransferStatus(item.UniqueID));

                }
            }
        }
        
        private Boolean HasPermissionForRepository(BaseCommunityFile item,  int IdUser, int IdCommunity, int idModule){
            Boolean result = false;
           
            CoreModuleRepository permissions = new CoreModuleRepository(CurrentManager.GetModulePermission(IdUser,IdCommunity,idModule));
            result = Service.HasPermissionToSeeRepositoryItem(item.Id, permissions.Administration, permissions.Administration, IdUser);
            return result;
        }
    }
}