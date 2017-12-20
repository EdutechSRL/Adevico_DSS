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
    public class ModuleToRepositoryPresenter  : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewModuleToRepository View
            {
                get { return (IViewModuleToRepository)base.View; }
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
            public ModuleToRepositoryPresenter(iApplicationContext oContext):base(oContext){
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ModuleToRepositoryPresenter(iApplicationContext oContext, IViewModuleToRepository view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idCommunity, String sourceModuleCode, Int32 sourceModuleIdAction,FileRepositoryType internalType, List<long> itemsToRemove){
            View.IdCommunityRepository = idCommunity;
            View.InternalFileType = internalType;
            View.SourceModuleIdAction = sourceModuleIdAction;
            View.SourceModuleCode = sourceModuleCode;
            View.UnloadItems = itemsToRemove;
            View.CurrentAction = UserRepositoryAction.None;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(idCommunity,CurrentManager.GetModuleID(sourceModuleCode));
            else{
                if (internalType== FileRepositoryType.None || internalType == FileRepositoryType.CommunityFile)
                    View.LoadErrorIDtype();
                else{
                    CoreModuleRepository module = Service.ServicePermission(UserContext.CurrentUserID, idCommunity);
                    View.LoadAvailableActions(GetAvailableActions(module, idCommunity), UserRepositoryAction.SelectAction);

                    if (idCommunity>0 && (module.Administration || module.UploadFile))
                        View.InitializeCommunityUploader(0,idCommunity,module);
                    View.InitializeInternalUploader(idCommunity);

                }
            }
        }
                    //        oFilePermission = Me.View.CommunityRepositoryPermission(CommunityID)
                    //If CommunityID > 0 Then
                    //    Me.View.AllowCommunityUpload = (oFilePermission.Administration OrElse oFilePermission.UploadFile)
                    //    Me.View.AllowCommunityLink = (oFilePermission.Administration OrElse oFilePermission.UploadFile OrElse oFilePermission.ListFiles OrElse oFilePermission.DownLoad)
                    //    If (oFilePermission.Administration OrElse oFilePermission.UploadFile) Then
                    //        Me.View.InitializeCommunityUploader(0, CommunityID, oFilePermission)
                    //    End If
                    //End If
                    //Me.View.InitializeInternalUploader(CommunityID)
                    //Me.View.InitializeFileSelector(CommunityID, FilesToRemove, (oFilePermission.Administration), (oFilePermission.Administration))
                    //Me.View.LoadActionsSelector()

        private List<UserRepositoryAction> GetAvailableActions(Int32 idCommunity)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(idCommunity, CurrentManager.GetModuleID(View.SourceModuleCode));

            return GetAvailableActions(Service.ServicePermission(UserContext.CurrentUserID, idCommunity),idCommunity);
        }
        private List<UserRepositoryAction> GetAvailableActions(CoreModuleRepository module, Int32 idCommunity)
        {
            List<UserRepositoryAction> actions = new List<UserRepositoryAction>();
            if (module.Administration || module.UploadFile)
                actions.Add(UserRepositoryAction.CommunityUploadPlay);
            if (module.Administration || module.UploadFile || module.ListFiles || module.DownLoad)
            {
                if (Service.HasItemType(idCommunity,UserContext.CurrentUserID, module.Administration, module.Administration, RepositoryItemType.FileStandard))
                    actions.Add(UserRepositoryAction.LinkForDownload);
                 if (Service.HasItemType(idCommunity,UserContext.CurrentUserID, module.Administration, module.Administration, RepositoryItemType.Multimedia))
                     actions.Add(UserRepositoryAction.LinkForMultimedia);
                 if (Service.HasItemType(idCommunity,UserContext.CurrentUserID, module.Administration, module.Administration, RepositoryItemType.ScormPackage))
                     actions.Add(UserRepositoryAction.LinkForScorm);
            }

            actions.Add(UserRepositoryAction.InternalUploadPlay);
            //actions.Add(ModuleToRepositoryAction.UploadFile);
            //actions.Add(ModuleToRepositoryAction.CreateFolder);
            return actions;
        }
        private Boolean HasPermissionForRepository(BaseCommunityFile item,  int IdUser, int IdCommunity, int idModule){
            Boolean result = false;
           
            CoreModuleRepository permissions = new CoreModuleRepository(CurrentManager.GetModulePermission(IdUser,IdCommunity,idModule));
            result = Service.HasPermissionToSeeRepositoryItem(item.Id, permissions.Administration, permissions.Administration, IdUser);
            return result;
        }

        public void ChangeAction(UserRepositoryAction action)
        {
            CoreModuleRepository module = Service.ServicePermission(UserContext.CurrentUserID, View.IdCommunityRepository);
            switch (action) {
                case UserRepositoryAction.LinkForDownload:
                    View.InitializeFileSelector(View.IdCommunityRepository, View.UnloadItems, module.Administration, module.Administration, RepositoryItemType.FileStandard);
                    break;
                case UserRepositoryAction.LinkForMultimedia:
                    View.InitializeFileSelector(View.IdCommunityRepository, View.UnloadItems, module.Administration, module.Administration, RepositoryItemType.Multimedia);
                    break;
                case UserRepositoryAction.LinkForScorm:
                    View.InitializeFileSelector(View.IdCommunityRepository, View.UnloadItems, module.Administration, module.Administration, RepositoryItemType.ScormPackage);
                    break;
            }
            View.DisplayAction(action);
        }
        public void AddCommunityFile(MultipleUploadResult<dtoUploadedFile> files, Boolean isForAllMembers) {
            if (files == null || files.UploadedFile.Count == 0)
                View.LoadEmptyUploaders();
            else {
                List<ModuleLink> links = (from f in files.UploadedFile select Service.CreateModuleLink(f.File, (Int32)CoreModuleRepository.Base2Permission.DownloadFile, Service.ServiceModuleID())).ToList();
                View.LinkCommunityFiles(links);
            }
        }
        public void AddInternalFile(MultipleUploadResult<dtoModuleUploadedFile> files)
        {
            if (files == null || files.UploadedFile.Count == 0)
                View.LoadEmptyUploaders();
            else
            {
                List<ModuleActionLink> links = (from f in files.UploadedFile select Service.CreateModuleActionLink(f.File, (Int32)CoreModuleRepository.Base2Permission.DownloadFile, Service.ServiceModuleID())).ToList();
                View.UploadedInternalFile(links);
            }
        }
        public void AddLinkToCommunityFile(List<long> items)
        {
            View.LinkCommunityFiles(Service.GetFileLinks(items,(Int32)CoreModuleRepository.Base2Permission.DownloadFile, Service.ServiceModuleID()));
        }
        //public void RemoveUploadedInternalFiles(List<iModuleObject> items, String communityaPath)
        //{
        //    List<long> files = (from f in items select ((ModuleInternalFile)f.ObjectOwner).Id).ToList();
        //    Service.dele
        //}
        //Public Sub (ByVal items As List(Of iModuleObject), ByVal CommunityPath As String)
        //    Dim oFiles As List(Of Long) = (From f In items Select CType(f.ObjectOwner, ModuleInternalFile).Id).ToList
        //    Me.CurrentManager.DeleteFiles(oFiles, CommunityPath)
        //End Sub
        public void UpdateModuleInternalFile(List<ModuleLink> items)
        {
            Service.UpdateModuleInternalFile(items, View.InternalFileType);
        }
        //public void UpdateModuleInternalFile (List<ModuleLink> items){
        //   //Me.CurrentManager.UpdateModuleInternalFile(items) 
        //}
    }
}




        //'Private Function ItemDefaultAction(ByVal Item As BaseCommunityFile) As Integer
        //'    If Item.isSCORM Then
        //'        Return UCServices.Services_File.ActionType.PlayFile
        //'    Else
        //'        Return UCServices.Services_File.ActionType.DownloadFile
        //'    End If
        //'End Function