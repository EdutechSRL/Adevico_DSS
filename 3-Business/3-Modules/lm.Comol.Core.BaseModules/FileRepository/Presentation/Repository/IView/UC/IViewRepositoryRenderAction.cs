using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation
{
    public interface IViewRepositoryRenderAction : lm.Comol.Core.ModuleLinks.IViewModuleRenderAction
    {
        #region "Context"
            String CssAvailability { get; set; }
            lm.Comol.Core.DomainModel.ContentView PreLoadedContentView { get; }
            String DestinationUrl { get; }
            String ItemIdentifier { get; set; }
            ItemAvailability Availability { get; set; }

            ItemType ItemType { get; set; }
            Boolean DisplayExtraInfo { get; set; }
            Boolean DisplayLinkedBy { get; set; }
            Boolean DisplayUploader { get; set; }
            Boolean DisplayTags { get; set; }
            String ExtraInfoDescription { get; set; }
        #endregion

        String GetUnknownUserName();
        DisplayMode OverrideItemDisplayMode { get; set; }

        #region "Display actions"
            void DisplayTextAction(String folderName, ModuleRepository.ActionType action);
            void DisplayTextAction(dtoDisplayObjectRepositoryItem obj);
            void DisplayActiveAction(String folderName, ModuleRepository.ActionType action);
            void DisplayActiveAction(dtoDisplayObjectRepositoryItem item, DisplayMode displayMode, String url, Boolean refreshContainerPage, Boolean onModalPage, Boolean saveObjectStatistics, Boolean saveOwnerStatistics);

            void DisplayActions(List<lm.Comol.Core.DomainModel.dtoModuleActionControl> actions);
        #endregion
        String GetIstanceBaseUrl();
        String GetIstanceFullUrl();

        String GetDisplayItemDescription(String name, String extension, String fullName, ItemType type, long size, String displaySize);
        void DisplayPlaceHolders(List<lm.Comol.Core.ModuleLinks.dtoPlaceHolder> items);
        String SanitizeLinkUrl(String url);
    }
}