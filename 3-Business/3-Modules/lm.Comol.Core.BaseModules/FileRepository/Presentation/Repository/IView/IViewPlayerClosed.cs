using lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain;
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public interface IViewPlayerClosed : IViewItemPageBase
    {
        #region "Preload"
            Boolean PreloadIsSendAsError { get; }
            Boolean PreloadSaveCompleteness { get; }
            Boolean PreloadIsOnModal { get; }
            Boolean PreloadRefreshContainer { get; }
            Boolean PreloadSaveStatistics { get; }
            String PreloadSessionId { get; }
            Guid PreloadUniqueId { get; }
            Guid PreloadUniqueIdVersion { get; }
        #endregion

        String EduPathModuleCode { get; }
        void DisplayUnknownItem(ItemType type);
        void DisplayMessage(String name, String extension, ItemType type, PlayerErrors errorType);
        void DisplayMessage(String name, String extension, ItemType type, PlayerClosedMessage message);

        NHibernate.ISession GetScormSession(String connectionString);
        void DisplaySessionExpired();
        void SaveLinkEvaluation(lm.Comol.Core.FileRepository.Domain.ScormPackageUserEvaluation evaluation);
    }
}