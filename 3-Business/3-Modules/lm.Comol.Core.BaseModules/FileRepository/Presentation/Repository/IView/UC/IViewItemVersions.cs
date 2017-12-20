using lm.Comol.Core.BaseModules.FileRepository.Domain;
using lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain;
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation
{
    public interface IViewItemVersions : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        long IdRepositoryItem { get; set; }
        ItemType RepositoryItemType { get; set; }
        void InitializeControl(Boolean editMode, dtoDisplayRepositoryItem item);

        #region "Translations"
            Dictionary<FolderType, String> GetFolderTypeTranslation();
            Dictionary<ItemType, String> GetTypesTranslations();
            String GetUnknownUserName();
            String GetRootFolderFullname();
        #endregion

        void LoadVersions(List<dtoDisplayVersionItem> items);
        void LoadVersions(List<dtoDisplayVersionItem> items, dtoDisplayRepositoryItem item, dtoContainerQuota quota);

        void DisplayUserMessage(UserMessageType messageType);
        void DisplaySessionTimeout();
        void CurrentVersionUpdated();
        void CurrentVersionUpdated(dtoDisplayRepositoryItem item, Int32 thumbnailWidth, Int32 thumbnailHeight, String allowedExtensionsForPreview);
        void NotifyAddedVersion(Int32 idModule,long idFolder, String folderName, String folderUrl, dtoCreatedItem addedVersion);
        void SendUserAction(Int32 idCommunity, Int32 idModule, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType action, long idItem, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ObjectType objType);
    }
}