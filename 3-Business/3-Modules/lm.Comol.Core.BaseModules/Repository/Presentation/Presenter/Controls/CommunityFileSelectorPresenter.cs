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
    public class CommunityFileSelectorPresenter  : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewCommunityFileSelector View
            {
                get { return (IViewCommunityFileSelector)base.View; }
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
            public CommunityFileSelectorPresenter(iApplicationContext oContext):base(oContext){
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public CommunityFileSelectorPresenter(iApplicationContext oContext, IViewCommunityFileSelector view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idCommunity, List<long> selectedFiles,Boolean loadIntoTree, Boolean showHiddenItems, Boolean forAdminPurpose,Boolean disableWaitingFiles, RepositoryItemType type){
            if( UserContext.isAnonymous)
                View.DisplayNoFilesFound();
            else{
                Community community = CurrentManager.GetCommunity(idCommunity);
                if (community==null && idCommunity>0)
                    View.DisplayNoFilesFound();
                else
                    LoadFiles(community, selectedFiles, loadIntoTree, showHiddenItems, forAdminPurpose, disableWaitingFiles,type);
            }
        }
        public List<ModuleActionLink> GetSelectedItemsActionLink(List<long> idItems){
            return Service.GetModuleActionLinkItems(idItems, (Int32)CoreModuleRepository.Base2Permission.DownloadFile, Service.ServiceModuleID());
        }

        private void LoadFiles(Community community, List<long> selectedFiles,Boolean loadIntoTree, Boolean showHiddenItems, Boolean forAdminPurpose,Boolean disableWaitingFiles, RepositoryItemType type){
            List<CommunityFile> files = new List<CommunityFile>();
            dtoFileFolder rep = new dtoFileFolder() { Id=0, Name="", isVisible=true};
            Person person = CurrentManager.GetPerson(UserContext.CurrentUserID);
            files = Service.GetCommunityItems(community,person, 0, showHiddenItems, forAdminPurpose);

            foreach (CommunityFile folder in  files.Where(f=> f.isFile== false).OrderBy(f=> f.Name).ToList()){
                dtoFileFolder dFolder = new dtoFileFolder() { Id= folder.Id, Name= folder.Name, isVisible= folder.isVisible};
                RecursivelyCreateFolder(dFolder, community, person, selectedFiles, loadIntoTree, showHiddenItems, forAdminPurpose, disableWaitingFiles,type);
                if (type == RepositoryItemType.None || (type != RepositoryItemType.None && dFolder.HasFiles))
                    rep.SubFolders.Add(dFolder);
             }
            List<long> idWaitingFiles = Service.GetIdWaitingFiles(files.Where(f=>f.RepositoryItemType== RepositoryItemType.ScormPackage || f.RepositoryItemType== RepositoryItemType.Multimedia).Select(f=> f.Id).ToList());
            rep.Files= files.Where(f=> (type== RepositoryItemType.None || f.RepositoryItemType==type) && (loadIntoTree || (!loadIntoTree && !selectedFiles.Contains(f.Id))) && f.isFile).OrderBy(f=> f.DisplayName).Select(
                f=> new dtoGenericFile(){
                 Extension= f.Extension, Id= f.Id, Name= f.Name, isVisible =f.isVisible,  Selected= selectedFiles.Contains(f.Id), Selectable= (!disableWaitingFiles || (disableWaitingFiles && !idWaitingFiles.Contains(f.Id)))
                }).ToList();
            View.LoadTree(rep);
        }

        private void RecursivelyCreateFolder(dtoFileFolder pfolder, Community community, Person person, List<long> selectedFiles, Boolean loadIntoTree, Boolean showHiddenItems, Boolean forAdminPurpose, Boolean disableWaitingFiles, RepositoryItemType type)
        {
            List<CommunityFile> files = Service.GetCommunityItems(community,CurrentManager.GetPerson(UserContext.CurrentUserID), pfolder.Id , showHiddenItems, forAdminPurpose);

            foreach (CommunityFile folder in  files.Where(f=> f.isFile== false).OrderBy(f=> f.Name).ToList()){
                dtoFileFolder dFolder = new dtoFileFolder() { Id= folder.Id, Name= folder.Name, isVisible= folder.isVisible};
                RecursivelyCreateFolder(dFolder, community, person, selectedFiles, loadIntoTree, showHiddenItems, forAdminPurpose, disableWaitingFiles,type);
                if (type == RepositoryItemType.None || (type != RepositoryItemType.None && dFolder.HasFiles))
                    pfolder.SubFolders.Add(dFolder);
            }
            List<long> idWaitingFiles = Service.GetIdWaitingFiles(files.Where(f => f.RepositoryItemType == RepositoryItemType.ScormPackage || f.RepositoryItemType == RepositoryItemType.Multimedia).Select(f => f.Id).ToList());
            pfolder.Files= files.Where(f=> (type== RepositoryItemType.None || f.RepositoryItemType==type) && (loadIntoTree || (!loadIntoTree && !selectedFiles.Contains(f.Id))) && f.isFile).OrderBy(f=> f.DisplayName).Select(
                f=> new dtoGenericFile(){
                    Extension = f.Extension,
                    Id = f.Id,
                    Name = f.Name,
                    isVisible = f.isVisible,
                    Selected = selectedFiles.Contains(f.Id),
                    Selectable = (!disableWaitingFiles || (disableWaitingFiles && !idWaitingFiles.Contains(f.Id)))
                }).ToList();
         }
    }
}