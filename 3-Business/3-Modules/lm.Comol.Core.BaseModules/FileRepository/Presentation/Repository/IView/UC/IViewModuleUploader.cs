using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation
{
    public interface IViewModuleUploader : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        Boolean AllowAnonymousUpload {get;set;}
        Int32 IdUploaderUser {get;set;}
        Int32 MaxItems {get;set;}
        Int32 MaxFileInput {get;set;}
        RepositoryIdentifier RepositoryIdentifier {get;set;}
        void LoadItemTypes(List<ItemType> types);
        void DisableControl();
    }

    public interface IViewModuleInternalUploader : IViewModuleUploader
    {
        
        void InitializeControlForCommunity(Int32 idUploaderUser, Int32 idCommunity);
        void InitializeControlForPortal(Int32 idUploaderUser);
        void InitializeControl(Int32 idUploaderUser, RepositoryIdentifier repository);
        List<dtoModuleUploadedItem> AddModuleInternalFiles(Object obj, long idObject, Int32 idObjectType, String moduleCode, Int32 idModuleAjaxAction, Int32 idModuleAction = 0);
    }
    public interface IViewModuleCommunityUploader : IViewModuleUploader
    {
        #region "Translations"
            String GetRootFolderFullname();
            String GetRootFolderName();
            String GetUnknownUserName();
        #endregion
        Boolean DisplayErrorInline { get; set; }
        Boolean AllowUpload { get; set; }
        Boolean AllowUploadToFolder { get; set; }
        Boolean UsePublicUser { get; set; }
        Boolean UseAnonymousUser { get; set; }
        void InitializeControlForCommunity(long idFolder, Int32 idCommunity);
        void InitializeControlForPortal(long idFolder);
        void InitializeControl(long idFolder, RepositoryIdentifier repository);


        void LoadFolderSelector(RepositoryIdentifier repository, long idFolder, String folderName, dtoContainerQuota quota, List<dtoNodeFolderItem> folders);
        void InitializeQuotaInfo(long idFolder, dtoContainerQuota quota);
        void DisplayUploadQuotaInfo(long idFolder, dtoContainerQuota quota);
        void DisplayUploadUnavailable();
        void DisplayUploadQuotaUnavailable();
        void DisplayError(ItemUploadError err, String name = "", String extension = "", ItemType type = ItemType.None);
        void DisplayError(ItemUploadError err, String folderName, List<dtoUploadedItem> items);

        String GetRepositoryDiskPath();

        void NotifyAddedItems(Int32 idModule, long idFolder, String folderName, String folderUrl, List<liteRepositoryItem> items);
        void SendUserAction(Int32 idCommunity, Int32 idModule, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType action, long idItem, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ObjectType objType);
        List<dtoModuleUploadedItem> AddFilesToRepository( Object obj, long idObject, Int32 idObjectType, String moduleCode, Int32 idModuleAjaxAction, Int32 idModuleAction = 0);
    }
}