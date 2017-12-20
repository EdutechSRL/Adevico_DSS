using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.Repository.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.Business;
using lm.Comol.Core.BaseModules.Repository.Domain;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public class MultipleFileUploaderPresenter  : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewMultipleFileUploader View
            {
                get { return (IViewMultipleFileUploader)base.View; }
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
            public MultipleFileUploaderPresenter(iApplicationContext oContext):base(oContext){
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public MultipleFileUploaderPresenter(iApplicationContext oContext, IViewMultipleFileUploader view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idFolder, Int32 idCommunity){
            if (idFolder!=0){
                BaseCommunityFile folder = Service.GetItem(idFolder);
                if (folder==null || folder.isFile)
                    idFolder = 0;
                else if (folder.CommunityOwner !=null && folder.CommunityOwner.Id != idCommunity)
                    idCommunity = folder.CommunityOwner.Id;
            }
            Community comm = CurrentManager.GetCommunity(idCommunity);
            if (idCommunity > 0 && comm==null)
                idCommunity= UserContext.CurrentCommunityID;
            View.IdCommunityRepository= idCommunity;
            View.CommunityName = (idCommunity==0 || comm == null) ? View.Portalname : comm.Name;

            View.AllowFolderChange = Service.CommunityHasFolders(idCommunity);
            View.IdFolder = idFolder;
            String path = View.BaseFolder;
            if (idFolder>0){
                BaseCommunityFile folder = Service.GetItem(idFolder);
                if (folder!=null)
                    path = Service.GetFolderPathName(folder.Id);
            }
            View.FilePath = path;
            View.DownlodableByCommunity = true;
            View.VisibleToDonwloaders = true;
            View.LoadFolderSelector(idFolder, idCommunity, false,false);
        }

        public dtoUploadedFile AddFileToRepository(CommunityFile file, String savedFile, String communityPath){
            dtoUploadedFile result = new dtoUploadedFile(file,ItemRepositoryStatus.CreationError);
            if (file!=null && lm.Comol.Core.File.Exists.File(savedFile)){
                CommunityFile savedItem = null;
                Int32 idCommunity = View.IdCommunityRepository;
                Community community = CurrentManager.GetCommunity(idCommunity);
                ItemRepositoryStatus status = ItemRepositoryStatus.None;

                if ((idCommunity>0 && community!=null ) || (community==null && idCommunity==0)){
                    file.Owner = CurrentManager.GetPerson(UserContext.CurrentUserID);
                    file.CommunityOwner = community;
                    file.CreatedOn = DateTime.Now;
                    file.ModifiedOn = file.CreatedOn;
                    file.ModifiedBy = file.Owner;

                    if (View.DownlodableByCommunity)
                        savedItem = Service.AddFile(file,communityPath, (long) CoreModuleRepository.Base2Permission.DownloadFile,ref status);
                    else
                        savedItem = Service.AddFile(file,file.Owner,communityPath, (long) CoreModuleRepository.Base2Permission.DownloadFile,ref status);
                    if (savedItem!=null)
                        result.Link = Service.CreateModuleActionLink(savedItem, (Int32) CoreModuleRepository.Base2Permission.DownloadFile,Service.ServiceModuleID());
                } 
                result.Status= status;
            }
            return result;
        }
        public String GetFolderName(long idFolder){
            return Service.GetFolderName(idFolder);
        }
    }
}
                   

