using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Repository.Business;
using lm.Comol.Core.Business;
namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public class PublishIntoRepositoryFromModulePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
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
        protected virtual IViewPublishIntoRepositoryFromModuleItem View
        {
            get { return (IViewPublishIntoRepositoryFromModuleItem)base.View; }
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
        public PublishIntoRepositoryFromModulePresenter(iApplicationContext oContext):base(oContext){
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public PublishIntoRepositoryFromModulePresenter(iApplicationContext oContext, IViewPublishIntoRepositoryFromModuleItem view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public void InitView() { 
            //Dim CurrentItemID As Long = Me.View.PreloadedItemID
            //Me.View.CurrentItemID = CurrentItemID
            //Dim oItem As CommunityEventItem = Me.CurrentManager.GetEventItem(CurrentItemID)
            //Dim CommunityID As Integer = Me.UserContext.CurrentCommunityID
            //If Not IsNothing(oItem) AndAlso Not IsNothing(oItem.CommunityOwner) Then
            //    CommunityID = oItem.CommunityOwner.Id
            //End If
            String ModuleOwnerCode = View.PreloadedModuleOwnerCode;
            int ModuleOwnerID = CurrentManager.GetModuleID(ModuleOwnerCode);
            View.ModuleOwnerCode = ModuleOwnerCode;
            View.ModuleOwnerID = ModuleOwnerID;
            if (UserContext.isAnonymous || ModuleOwnerID == -1)
            {
                View.NoPermission(0, ModuleOwnerID == -1 ? ModuleID : ModuleOwnerID, ModuleOwnerID == -1 ? CoreModuleRepository.UniqueID : ModuleOwnerCode );
            }
            else{
                int IdCommunity = UserContext.CurrentCommunityID;
                View.SetBackUrl(View.PreloadedBackUrl);
                View.InitializeModuleInternalFileSelector(View.PreloadedItemId, View.PreloadedLinkId, ModuleOwnerCode);
                if (View.HasPermissionToSelectFile) {
                    IdCommunity = View.InternalFileSelectorIdCommunity;
                    CoreModuleRepository moduleRepository = new CoreModuleRepository();
                    if (IdCommunity > 0)
                    {
                        View.SelectedCommunityID = IdCommunity;
                        moduleRepository = Service.ServicePermission(UserContext.CurrentUserID, IdCommunity);
                    }
                    else
                    {
                        View.SelectedCommunityID = -1;
                        View.InitializeCommunitySelector();
                        moduleRepository.UploadFile = (from p in View.RepositoryPermissions where p.Permissions.Administration || p.Permissions.UploadFile select p.ID).Any();
                    }
                    if (moduleRepository.UploadFile || moduleRepository.Administration)
                    {
                        View.SelectedFolderId = -1;
                        View.SendInitAction(IdCommunity, ModuleOwnerID, ModuleOwnerCode);
                    }
                    else
                        View.NoPermissionToPublishFiles(IdCommunity, ModuleOwnerID, ModuleOwnerCode);

                }

              //Me.View.BackToManagement = CurrentItemID
              //  If IsNothing(oItem) Then
              //      Me.View.NoPermissionToManagementFiles()
              //  ElseIf HasPermission(oItem) Then
              //      Dim oFilePermission As New ModuleCommunityRepository
              //      If IsNothing(oItem.CommunityOwner) Then
              //          Me.View.InitCommunitySelection()
              //          oFilePermission.UploadFile = (Me.View.CommunitySelectionLoaded)
              //      Else
              //          oFilePermission = Me.View.CommunityRepositoryPermission(oItem.CommunityOwner.Id)
              //      End If
              //      If Not (oFilePermission.Administration OrElse oFilePermission.UploadFile) Then
              //          Me.View.NoPermissionToPublishFiles()
              //      Else
              //          Me.View.SelectedFolder = -1
              //          Me.View.SendActionInit(CommunityID, ModuleID, oItem.Id)
              //          LoadFilesToPublish(oItem)
              //      End If
              //  Else
              //      Me.View.NoPermissionToPublishFiles()
              //  End If
            }
        }

        public void LoadStep(WizardStep step, Boolean DirectionNext) {
            switch (step) { 
                case WizardStep.CommunitySelector:
                    if (DirectionNext)
                        LoadStepCommunitySelector();
                    else 
                        View.ShowWizardStep(WizardStep.CommunitySelector);
                    break;
                case WizardStep.FolderSelector:
                    LoadStepFolderSelector();
                    break;
            }  
        }
        public void ChangeSelectedCommunity(int IdCommunity) {
            if (IdCommunity != View.SelectedCommunityID)
            {
                CoreModuleRepository module = Service.ServicePermission(UserContext.CurrentUserID, View.SelectedCommunityID);
                if (module.Administration || module.UploadFile)
                {
                    View.InitializeFolderSelector(View.SelectedCommunityID, 0, module.Administration);
                    View.ShowWizardStep(WizardStep.FolderSelector);
                }
            }
            else
                View.ShowWizardStep(WizardStep.FolderSelector);
        }
        //Public Sub FindCommunityFolder(ByVal CommunityID As Integer)
        //    'Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)
        //    Dim oFilePermission As ModuleCommunityRepository = Me.View.CommunityRepositoryPermission(CommunityID)
        //    Me.View.SelectedCommunityID = CommunityID
        //    If CommunityID > 0 Then
        //        If Me.View.SelectedFolder = -1 Then
        //            Me.View.InitializeFolderSelector(CommunityID, 0, (oFilePermission.Administration))
        //        Else
        //            Me.View.ShowFoldersList()
        //        End If
        //    Else
        //        Me.View.ShowSelectCommunity()
        //    End If

        //End Sub
        private void LoadStepCommunitySelector(){
            int IdCommunity = View.InternalFileSelectorIdCommunity;
            Community community =CurrentManager.GetCommunity(IdCommunity);
            if (IdCommunity <= 0 || community == null)
                if (View.CommunitySelectorLoaded)
                    View.ShowWizardStep(WizardStep.CommunitySelector);
                else
                    View.InitializeCommunitySelector();
            else {
                View.SelectedCommunityID = IdCommunity;
                View.SetFolderInfo(community.Name);
                LoadStepFolderSelector();
            }
        }
        private void LoadStepFolderSelector(){
            CoreModuleRepository module = Service.ServicePermission(UserContext.CurrentUserID, View.SelectedCommunityID);
            if (module.Administration || module.UploadFile)
            {
                if (View.SelectedFolderId <= (long)0)
                    View.InitializeFolderSelector(View.SelectedCommunityID, 0, module.Administration);
                View.ShowWizardStep(WizardStep.FolderSelector);
            }
        }

        public void TryToPublish(long IdDestinationFolder) {
            List<long> IdLinks = View.SelectedItemFileLinksId;
            if (IdLinks.Count == 0)
                View.ShowWizardStep(WizardStep.FileSelector);
            else {
                List<BaseCommunityFile> existingFiles = new List<BaseCommunityFile>();
                List<BaseCommunityFile> internalFiles =  Service.GetModuleFiles(View.SelectedItemFilesId);

                Community community = CurrentManager.GetCommunity(View.SelectedCommunityID);
                foreach (ModuleInternalFile moduleFile in internalFiles) {
                    CommunityFile repositoryFile = Service.FindDuplicatedRepositoryItem(community, IdDestinationFolder, moduleFile);
                    if (repositoryFile != null)
                        existingFiles.Add(moduleFile);
                }
                List<dtoFileExist<long>> filesToChange = new List<dtoFileExist<long>>();
                if (existingFiles.Count > 0)
                {
                    filesToChange = (from ef in existingFiles
                                                              select new dtoFileExist<long> { Id = ef.Id, ExistFileName = ef.DisplayName, Extension = ef.Extension, ProposedFileName = Service.ProposedRepositoryItemName(ef,community,IdDestinationFolder) }).ToList();
                    View.LoadDuplicates(filesToChange);
                    View.ShowWizardStep(WizardStep.RenamedFileList);
                }
                else {
                    View.LoadDuplicates(filesToChange);
                    LoadStepSummary(internalFiles, new List<dtoModuleFileToPublish>());
                    View.ShowWizardStep(WizardStep.ConfirmPublish);
                }
            }
        }
        public void GotoPreviousFromSummary() {
            List<long> IdRenamedFiles = (from cf in View.GetRenamedModuleFiles() select cf.FileID).ToList();
            if (IdRenamedFiles.Count > 0)
                View.ShowWizardStep(WizardStep.RenamedFileList);
            else
                View.ShowWizardStep(WizardStep.FolderSelector);
        }
        public void LoadStepSummary()
        {
            long IdFolder = View.SelectedFolderId;
            List<dtoModuleFileToPublish> RenamedFiles = View.GetRenamedModuleFiles();
            List<long> IdRenamedFiles = (from cf in RenamedFiles select cf.FileID).ToList();
            List<long> IdFilesUnchanged = (from i in View.SelectedItemFilesId where  !IdRenamedFiles.Contains(i) select i).ToList();
            LoadStepSummary(Service.GetModuleFiles(IdFilesUnchanged), RenamedFiles);
        }
        private void LoadStepSummary(List<BaseCommunityFile> internalFiles, List<dtoModuleFileToPublish> RenamedFiles)
        {
            List<dtoModuleFileToPublish> filesToPublish = (from f in internalFiles
                                                           select new dtoModuleFileToPublish() { FileID = f.Id, FileName = f.Name, Extension = f.Extension, IsVisible = true }).ToList();
            if (RenamedFiles.Count > 0)
            {
                filesToPublish.AddRange(RenamedFiles);
            }
            String communityName = View.PortalHome;
            Community community = CurrentManager.GetCommunity(View.SelectedCommunityID);
            if (community != null)
                communityName = community.Name;
            View.LoadSummary(communityName, View.SelectedFolderName, filesToPublish);
            View.ShowWizardStep(WizardStep.ConfirmPublish);
        }
        public void PublishIntoCommunityRepository(String BaseFilePath, int IdCategory, List<dtoModuleFileToPublish> files) {
            int IdCommunity = View.SelectedCommunityID;
            Community community = CurrentManager.GetCommunity(IdCommunity);
            if (BaseFilePath == "" || files.Count == 0 || IdCommunity < 1 || community == null)
                View.ReturnToManagement();
            else {
                List<BaseCommunityFile> addedFiles = new List<BaseCommunityFile>();
                List<dtoFileExist<long>> filesToRename = Service.PublishFilesIntoRepository(BaseFilePath, IdCategory,community, View.SelectedFolderId, files, ref addedFiles);
                View.UpdateSelectedFilesId((from f in filesToRename select f.Id).ToList());
                
                String ParentFolder = View.BaseFolder;
                if (View.SelectedFolderId>0)
                    ParentFolder = Service.GetFolderName(View.SelectedFolderId);
                foreach (BaseCommunityFile addedFile in (from f in addedFiles where f!=null && f.Id>0 select f).ToList()){
                    View.NotifyRepositoryAdd(IdCommunity, ModuleID, addedFile, ParentFolder);
                }

                if (filesToRename.Count == 0)
                    View.ReturnToManagement();
                else
                {
                    View.LoadDuplicates(filesToRename);
                    View.ShowWizardStep(WizardStep.RenamedFileList);
                }
            }
        }
    }
}
