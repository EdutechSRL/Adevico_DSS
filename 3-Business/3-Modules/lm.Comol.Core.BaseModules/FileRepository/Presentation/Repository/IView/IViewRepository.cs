using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public interface IViewRepository : IViewRepositoryPageBase
    {
        #region "Preload"
            long PreloadIdFolder { get; }
            lm.Comol.Core.FileRepository.Domain.FolderType PreloadFolderType { get; }
            lm.Comol.Core.FileRepository.Domain.OrderBy PreloadOrderBy { get; }
            Boolean PreloadAscending { get; }
            Boolean PreloadLoadFromCookies { get; }
        #endregion

        #region "Context"
              lm.Comol.Core.FileRepository.Domain.OrderBy CurrentOrderBy { get; set; }
            Boolean CurrentAscending { get; set; }
            List<lm.Comol.Core.BaseModules.FileRepository.Domain.Column> Columns { get; set; }
            long IdCurrentFolder { get; set; }
            String CurrentPath { get; set; }
            Boolean IsInitialized { get; set; }

            FolderType CurrentFolderType { get; set; }
            String CurrentFolderIdentifierPath { get; set; }
            List<long> CurrentSelectedItems { get; set; }
            String RepositoryIdentifier { get; set; }
        #endregion
        void SetTitle(RepositoryType type, String name = "", Boolean isCurrent = false,Boolean settingsFound= false);


        #region "Initializers"
            void InitializePresets(List<ViewOption> availableOptions, PresetType currentSet, List<PresetType> presets = null);
            void InitializeHeaderSettings(Boolean isGeneric, PresetType currentSet, Dictionary<PresetType, List<ViewOption>> availableOptions, Dictionary<PresetType, List<ViewOption>> activeOptions, Int32 idCommunity = -1, Int32 idPerson = -1);
            void InitializeTree(dtoDisplayRepositoryItem currentFolder, List<dtoDisplayRepositoryItem> items, RepositoryIdentifier identifier, String repositoryIdentifier);
            void InitializeBreadCrumb(List<dtoFolderItem> folders,String rootUrl, OrderBy orderBy, Boolean ascending);
            void InitializeFolderCommands(List<ItemAction> actions, dtoContainerQuota quota,long idFatherFolder,String fatherpath,FolderType fatherType, String folderName, String folderFatherUrl = "", String folderFatherName = "", List<dtoNodeFolderItem> folders=null, List<ItemType> types = null);
            void InitializeAvailableOrderBy(List<OrderBy> items, OrderBy selected);
            void HideOrderBySelector();
            void HideBreadCrumb();
            void LoadDiskStatistics(List<dtoFolderSize> items);
            void DisplayFolderInfo(dtoFolderSize folderInfo);
            void HideFolderInfo();
        #endregion

        #region "Update View"
            void UpdateBreadCrumbUrls(OrderBy orderBy, Boolean ascending);
            void UpdateTreeUrls(OrderBy orderBy, Boolean ascending);
            void UpdateFolderCommandsUrls(OrderBy orderBy, Boolean ascending);
            void SetFoldersCookies(List<long> folder);
        #endregion

        void LoadItems(List<dtoDisplayRepositoryItem> items,Boolean hideReorderColumn, Boolean displayPath, List<lm.Comol.Core.BaseModules.FileRepository.Domain.Column> availableColumns);

        #region "Messages"
            void DisplayUnknownCommunity();
            void DisplayUnknownUser();
            void DisplayUnknownItem(ItemAction action, Boolean multiple = false);
            void DisplayUnavailableItem(ItemAction action);
            void DisplayUnavailableItem(ItemAction action,String folderName);
            void DisplayUnavailableItems(ItemAction action);

            void DisplayMessage(ItemAction action, Boolean executed, String name, String extension, ItemType type, String startFolder = "", String endFolder = "");

            void DisplayMessage(ItemAction action, Boolean executed, Dictionary<ItemType, long> executedItems, Dictionary<ItemType, long> startItems, String startFolder = "", String endFolder = "");
            void DisplayUnableToExecute(ItemAction action);
            void DisplaySameDirectory();
            void DisplayUnableToInitialize(ItemAction action, String name, String extension, ItemType type);
            void DisplayMoveItemSelector(long idItem, long idFolder, String folderName, List<dtoNodeFolderItem> folders, String name, String extension, ItemType type);
            void DisplayUnavailableSpace(String name, String extension, ItemType type, long size, dtoFolderTreeItem folder);
        #endregion

        #region "Translations"
            Dictionary<FolderType, String> GetFolderTypeTranslation();
            Dictionary<ItemType, String> GetTypesTranslations();
            String GetUnknownUserName();
            String GetRootFolderFullname();
            String GetRootFolderName();
        #endregion

     
      
        String GetCurrentUrl();
        List<long> GetSelectedItems();



        void DisplayAddedFolders(String folderName, long added, long notAdded);
        void DisplayUnableToAddFolders(String folderName, long cFolders);

        void DisplayAddedLinks(String folderName, long added, long notAdded);
        
        void DisplayUnableToAddLinks(String folderName, long cLinks);
        void DisplayAddedFiles(String folderName, long added, long notAdded);
        void DisplayUnableToAddFiles(String folderName, long count);
        void DisplayDeletedItems(List<dtoItemToDelete> items);
        void DisplayDeletedItem(dtoItemToDelete item);

        void DisplayAddVersion(dtoDisplayRepositoryItem item, dtoContainerQuota quota);
        void DisplayAddedVersion(String folderName,  dtoCreatedItem addedVersion);
       
        void GoToUrl(String url);
        void NotifyAddedItems(long idFolder, String folderName, String folderUrl, List<RepositoryItem>items);
        void NotifyAddedVersion(long idFolder, String folderName, String folderUrl, dtoCreatedItem addedVersion);
        void NotifyVisibilityChanged(long idFolder, String folderName, String folderUrl,liteRepositoryItem item);
        void NotifyVisibilityChanged(long idFolder, String folderName, String folderUrl, List<liteRepositoryItem> items);
        void NotifyVirtualDelete(long idFolder, String folderName, String folderUrl, liteRepositoryItem item, Boolean isDeleted);
        void NotifyVirtualDelete(long idFolder, String folderName, String folderUrl, List<liteRepositoryItem> items, Boolean isDeleted);
        void NotifyDelete(String folderUrl, dtoItemToDelete item);
        void NotifyDelete(String folderUrl, List<dtoItemToDelete> items);

        //SCORM:
        void SaveLinkEvaluation(lm.Comol.Core.FileRepository.Domain.ScormPackageUserEvaluation evaluation);
        NHibernate.ISession GetScormSession(String connectionString);
        TimeSpan GetScormStatDelay();
    }
}