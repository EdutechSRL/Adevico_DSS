using lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public interface IViewMultimediaSettings : IViewEditViewDetailsPageBase
    {
        #region "Context"
            Boolean AllowSave{ set; }
        #endregion
        
        #region "Messages"
            void DisplayUnknownItem();
            void DisplayMessage(String name, String extension, ItemType type, UserMessageType messageType, ItemAvailability status = ItemAvailability.available, String defaultDocument = "");
            void DisplayMessage(String name, String extension, ItemType type, UserMessageType messageType, dtoMultimediaFileObject obj);
        
        #endregion

            ItemPermission GetLinkPermissions(liteModuleLink link, Int32 idUser);
            Boolean HasPermissionForLink(Int32 idUser, long idLink, liteRepositoryItem item, liteRepositoryItemVersion version, Int32 idModule, String moduleCode);
            void LoadItems(String uniqueIdVersion,String filename, List<dtoMultimediaFileObject> items,dtoMultimediaFileObject selectedItem);
    }
}