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
    public class MultimediaFileSettingsPresenter  : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewMultimediaFileSettings View
            {
                get { return (IViewMultimediaFileSettings)base.View; }
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
            public MultimediaFileSettingsPresenter(iApplicationContext oContext):base(oContext){
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public MultimediaFileSettingsPresenter(iApplicationContext oContext, IViewMultimediaFileSettings view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion
      
        public void InitView(){
            int IdUser = UserContext.CurrentUserID;
            int idCommunity = UserContext.CurrentCommunityID;
            long IdLink = View.PreloadedIdLink;
            long IdFile = View.PreloadedIdFile;
            View.BackUrl = View.PreloadedBackUrl;
            View.IdFile = IdFile;
            View.IdLink = IdLink;
            View.AllowSetDefaultDocument = false; 
            BaseCommunityFile item = Service.GetItem(IdFile);
            if (item != null && item.CommunityOwner != null)
                idCommunity = item.CommunityOwner.Id;
            if (UserContext.isAnonymous)
                View.SendToSessionExpiredPage(idCommunity, UserContext.Language.Code);
            else if (item == null)
                View.LoadFileNotExist();
            else if (item.RepositoryItemType == RepositoryItemType.Multimedia)
            {
                RepositoryItemPermission permissions = new RepositoryItemPermission();
                if (IdLink == 0 || item.GetType() == typeof(CommunityFile))
                {
                    CoreModuleRepository module = null;
                    if (item.CommunityOwner == null)
                        module = CoreModuleRepository.CreatePortalmodule(UserContext.UserTypeID);
                    else if (!item.IsInternal)
                        module = new CoreModuleRepository(CurrentManager.GetModulePermission(IdUser, item.CommunityOwner.Id, ModuleID));
                    else
                        module = new CoreModuleRepository();
                    if (Service.HasPermissionToSeeRepositoryItem(item.Id, module.Administration, module.Administration, IdUser))
                        permissions = new dtoDisplayItemRepository(item,module,IdUser, module.Administration).Permission;
                }
                else{
                    ModuleLink link = Service.GetModuleLink(IdLink);
                    permissions = View.GetModuleLinkPermission(link.SourceItem.CommunityID,IdUser,link);
                }
                if (permissions.EditSettings) {
                    MultimediaFileTransfer multimedia = Service.GetMultimediaFileTransfer(item);
                    if (multimedia == null)
                        View.LoadFileNotExist();
                    else if (multimedia.MultimediaIndexes.Count == 0)
                        View.LoadFileWithoutIndex(item);
                    else
                    {
                        View.AllowSetDefaultDocument = permissions.EditSettings;
                        View.LoadTree(item, (from m in multimedia.MultimediaIndexes select m.Fullname.Replace(item.UniqueID.ToString() + "\\", "")).ToList(), (multimedia.DefaultDocument == null) ? "" : multimedia.DefaultDocument.Fullname.Replace(item.UniqueID.ToString() + "\\", ""));
                    }
                }
                else
                    View.LoadFileNoPermission();
            }
            else
                View.LoadInvalidFileType(item);
        }

        public void SetDefaultDocument(String path) {
            MultimediaFileIndex index = Service.SetMultimediaFileDefaultIndex( View.IdFile,path);
        }
    }
}