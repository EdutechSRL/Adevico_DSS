using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewPublishIntoRepositoryFromModuleItem : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        long SelectedFolderId {get;set;}
        String SelectedFolderName {get;}
        int SelectedCommunityID {get;set;}
        String BaseFolder {get;}
        String PortalHome { get; }
        String PreloadedBackUrl { get; }
        String PreloadedModuleOwnerCode { get; }
        String PreloadedItemId { get; }
        String DeafultBackUrl { get; set; }
        long PreloadedLinkId { get; }
        int ModuleOwnerID { get; set; }
        String ModuleOwnerCode { get; set; }

        #region "File Selector"
            void InitializeModuleInternalFileSelector(string IdItem, long IdLink, string ModuleOwnerCode);
            Boolean HasPermissionToSelectFile { get; }
            List<ModuleCommunityPermission<CoreModuleRepository>> RepositoryPermissions { get; }
            int InternalFileSelectorIdCommunity { get; }
            void SetBackUrl(String url);
            List<long> SelectedItemFileLinksId { get; }
            List<long> SelectedItemFilesId { get; }
            void UpdateSelectedFilesId(List<long> filesID);
        #endregion

        #region "Community Selector"
            void InitializeCommunitySelector();
            Boolean CommunitySelectorLoaded { get; }
        #endregion

        #region "Folder Selector"
            void InitializeFolderSelector(int IdCommunity, long SelectedFolderID, Boolean ShowAlsoHidden);
            void SetFolderInfo(String communityName);
        #endregion

        #region "Duplicates"
            void LoadDuplicates(List<dtoFileExist<long>> files);
            List<dtoModuleFileToPublish> GetRenamedModuleFiles();
        #endregion
        #region "Wizard"

            void ShowWizardStep(WizardStep wstep);
        #endregion
            #region "Confirm / Summary"
            void LoadSummary(String communityName, String folderName, List<dtoModuleFileToPublish> files);
            #endregion

            void ReturnToManagement();
            void NoPermission(int IdCommunity, int IdModule, String moduleCode);
            void NoPermissionToPublishFiles(int IdCommunity, int IdModule, String moduleCode);
            void NoPermissionToManagementFiles(int IdCommunity, int IdModule, String moduleCode);

            void NotifyRepositoryAdd(int IdCommunity, int IdModule, BaseCommunityFile file, String FolderName);
            void SendInitAction(int IdCommmunity, int IdModule, String moduleCode);
        //      'Sub void NoPermission(int IdCommunity, int IdModule);NoPermissionToManagementFiles()
        //'Sub ()
        //'Sub NoPermissionToAccessPage(ByVal IdCommunity As Integer, ByVal IdModule As Integer)

    }
}
