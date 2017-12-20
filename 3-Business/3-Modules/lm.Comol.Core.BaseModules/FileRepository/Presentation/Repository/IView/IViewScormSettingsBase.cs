using lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.FileRepository.Domain;
using lm.Comol.Core.FileRepository.Domain.ScormSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public interface IViewScormSettingsBase : IViewEditViewDetailsPageBase
    {
        #region "Context"
            long IdSettings { get; set; }
        #endregion
        
        #region "Messages"
            void DisplayUnknownItem();
            void DisplayMessage(String name, String extension, ItemType type, UserMessageType messageType, ItemAvailability status = ItemAvailability.available);
        #endregion

        ItemPermission GetLinkPermissions(liteModuleLink link, Int32 idUser);
        Boolean HasPermissionForLink(Int32 idUser, long idLink, liteRepositoryItem item, liteRepositoryItemVersion version, Int32 idModule, String moduleCode);
        void LoadSettings(String uniqueIdVersion,String filename, dtoScormPackageSettings settings, Boolean isReadOnLy, Boolean isViewMode);
        List<dtoScormItemEvaluationSettings> GetSettings();
    }
}