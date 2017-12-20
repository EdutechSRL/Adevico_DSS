using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Repository.Domain;
using lm.Comol.Core.DomainModel.Repository;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewModuleToRepository  : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean AjaxViewUpdate { get; set; }
        String SourceModuleCode { get; set; }
        Int32 SourceModuleIdAction { get; set; }
        Int32 IdCommunityRepository { get; set; }
        FileRepositoryType InternalFileType { get; set; }
        UserRepositoryAction CurrentAction { get; set; }
        
        List<long> UnloadItems { get; set; }


        void InitializeControl(Int32 idCommunity, String sourceModuleCode, Int32 sourceModuleIdAction, FileRepositoryType internalType);
        void InitializeControlRemovingFiles(Int32 idCommunity, String sourceModuleCode, Int32 sourceModuleIdAction, FileRepositoryType internalType, List<long> items);
        void LoadAvailableActions(List<UserRepositoryAction> actions, UserRepositoryAction dAction);
        void DisplayAction(UserRepositoryAction action);
        
        void InitializeCommunityUploader(long idFolder, Int32 idCommunity, CoreModuleRepository permission);
        void InitializeInternalUploader(Int32 idCommunity);

        void InitializeFileSelector(Int32 idCommunity, List<long> selectedFiles, Boolean showHiddenItems, Boolean forAdminPurpose, RepositoryItemType type);
        void LoadFolderSelector(long idExcludeFolder, long idFolder, Int32 idCommunity, Boolean showHiddenItems, Boolean forAdminPurpose);

        void AddInternalFileAction(Int32 idCommunity, Int32 idModule);
        void AddCommunityFileAction(Int32 idCommunity, Int32 idModule);
        void DisplaySessionTimeout(Int32 idCommunity, Int32 idModule);
        void LoadEmptyUploaders();
        void LoadErrorIDtype();
        void LoadErrorUploading(List<dtoUploadedFile> files);
        void ChangeDisplayAction(UserRepositoryAction action);
        void UploadedInternalFile(List<ModuleActionLink> items);
        void UpdateModuleInternalFile(List<ModuleLink> items);
        void RemoveUploadedInternalFiles(List<iModuleObject> items);
        void LinkCommunityFiles(List<ModuleLink> items);
        String GetActionTitle(UserRepositoryAction action);
        Boolean IsCreateActionAvailable { get; set; }
    }
}