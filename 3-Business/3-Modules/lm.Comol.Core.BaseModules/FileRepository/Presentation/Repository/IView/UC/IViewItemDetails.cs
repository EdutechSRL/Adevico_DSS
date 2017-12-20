using lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain;
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation
{
    public interface IViewItemDetails : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        long IdRepositoryItem { get; set; }
        ItemType RepositoryItemType { get; set; }
        void InitializeControl(Boolean editMode, dtoDisplayRepositoryItem item, Int32 thumbnailWidth, Int32 thumbnailHeight, String previewExtensions);

        #region "Translations"
            Dictionary<FolderType, String> GetFolderTypeTranslation();
            Dictionary<ItemType, String> GetTypesTranslations();
            String GetUnknownUserName();
            String GetRootFolderFullname();
        #endregion

        void DisplayUserMessage(UserMessageType messageType);



        void DisplaySessionTimeout();
        void DisplayDuplicateName(String name, ItemType type, String folderName);
        void DisplayDuplicateUrl(String url, String folderName);
        void DisplayUnknownItem(ItemAction action);
        void DisplayUnavailableItem(ItemAction action);
        void UpdateDownloads(long downloads, long myDownloads);
        void DisplayUpdateMessage(ItemAction action, Boolean executed, String name, String extension, ItemType type);
        void DisplayUpdateMessage(ItemSaving saving);
        void UpdateDefaultTags(List<String> tags);
        void UpdateDisplayInfo(dtoDisplayRepositoryItem item);

        void SendUserAction(Int32 idCommunity, Int32 idModule, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType action, long idItem, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ObjectType objType);
        void NotifyVisibilityChanged(Int32 idModule, long idFolder, String folderName, String folderUrl, liteRepositoryItem item);
    }
}