using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.DomainModel.Helpers;
namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewDisplayRepositoryItem : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        ItemDisplayView DisplayView {get;set;}
        ItemDisplayMode DisplayMode {get;set;}
        ItemDescriptionDisplayMode DescriptionDisplayMode {get;set;}
        ItemAvailableCommand AvailableCommands {get;set;}
        String FolderNavigationUrl { get; set; }
        ItemAvailableCommand DisplayCommandText { get; set; }
        IconSize IconSize { get; set; }

        void InitializeControl(dtoDisplayItemRepository item, String currentUrl, int IdModule, int IdCommunity, int idAction);
        void InitializeControl(dtoDisplayItemRepository item, String currentUrl, int IdModule, int IdCommunity, int idAction, ItemDisplayView view, ItemDisplayMode mode, ItemDescriptionDisplayMode descriptionDisplay, ItemAvailableCommand commands);
        void InitializeControlForFolder(dtoDisplayItemRepository item, String url, int IdModule, int IdCommunity);
        void InitializeControlForFolder(dtoDisplayItemRepository item, String url, int IdModule, int IdCommunity, ItemDisplayView view, ItemDisplayMode mode, ItemDescriptionDisplayMode descriptionDisplay, ItemAvailableCommand commands);

        void DisplayItemName(dtoDisplayItemRepository item);
        void DisplayUnknownItem();
        void DisplayItem(dtoDisplayItemRepository item, String currentUrl, int IdModule, int IdCommunity, int idAction);
        void DisplayDeletedFile(String displayName,RepositoryItemType type, String extension);
        void DisplayMultimediaItem(dtoDisplayItemRepository item, String currentUrl, String defaultUrl, int IdModule, int IdCommunity, int idAction);
        void DisplayFolder(dtoDisplayItemRepository item, String url, int IdModule, int IdCommunity, int idAction);
    }
}