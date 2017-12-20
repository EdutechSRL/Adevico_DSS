using lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain;
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public interface IViewPlayer : IViewItemPageBase
    {
        #region "Preload"
            Boolean PreloadSaveCompleteness { get; }
            Boolean PreloadIsOnModal { get; }
            Boolean PreloadRefreshContainer { get; }
            Boolean PreloadSaveStatistics { get; }
            String PreloadLanguage { get; }
            Guid PreloadUniqueId { get; }
            Guid PreloadUniqueIdVersion { get; }
        #endregion

        #region "Context"
            Boolean SaveStatistics{get;set;}
            Int32 ItemIdCommunity { get; set; }
            Guid PlayUniqueSessionId { get; set; }
            Guid WorkingSessionId { get; set; }
            String PlaySessionId { get; set; }
            String EduPathModuleCode { get; }
            
        #endregion

        void DisplayUnknownItem();
        void DisplayMessage(String name, String extension, ItemType type, PlayerErrors errorType);
        void DisplayPlayUnavailable(String name, String extension, ItemType type, Boolean isSpecificVersion, ItemAvailability availability, ItemStatus status);
        void InitializePlayer(String renderPlayer, String externalPlayerUrl, String ajaxActionUrl, String name, ItemType type);
        Boolean HasPermissionForLink(Int32 idUser, long idLink, lm.Comol.Core.DomainModel.ModuleObject obj,ItemType type, Int32 idModule, String moduleCode);
        void SaveLinkEvaluation(Int32 idUser, long idLink);
        void SaveLinkEvaluation(Int32 idUser, long idLink, lm.Comol.Core.FileRepository.Domain.ScormPackageUserEvaluation evaluation);
        NHibernate.ISession GetScormSession(String connectionString);
        void DisplayClosingToolBar();
        void DisplaySessionExpired();
        void RedirectTo(String url);
    }
}