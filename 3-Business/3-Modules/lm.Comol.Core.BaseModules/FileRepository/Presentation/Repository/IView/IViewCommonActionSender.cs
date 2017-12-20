using lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain;
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public interface IViewCommonActionSender : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        #region "Preload"
            long PreloadIdItem { get; }
            long PreloadIdVersion { get; }
            long PreloadIdLink { get; }
            ItemType PreloadItemType { get; }
            long PreloadIdAction { get; }
            Guid PreloadWorkingSessionId { get; }
            String PreloadPlaySessionId { get; }
            Int32 PreloadIdCommunity { get; }
            Boolean PreloadIsOnModal { get; }
            Boolean PreloadRefreshContainer { get; }
        #endregion

        #region "Context"
            long IdItem { get; set; }
            long IdVersion { get; set; }
            long IdLink { get; set; }
            ItemType ItemType { get; set; }
            long IdAction { get; set; }
            Guid UniqueId { get; set; }
            Guid UniqueIdVersion { get; set; }
            Guid WorkingSessionId { get; set; }
            String ConnectionString { get; set; }
            String PlaySessionId { get; set; }
        #endregion

        void InitializeContext(long idItem,long idVersion, long idLink, Guid uniqueId, Guid uniqueIdVersion);
        void StopTimer();
    }
}