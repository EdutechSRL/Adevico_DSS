using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewBaseList : IViewPageBase
    {
        #region "Preload"
            Boolean PreloadFromCookies { get; }
            ItemsGroupBy PreloadGroupBy { get; }
            ProjectFilterBy PreloadFilterBy { get; }
            ItemListStatus PreloadFilterStatus { get; }
            SummaryTimeLine PreloadTimeLine { get; }
            SummaryDisplay PreloadDisplay { get; }
        #endregion

        #region "Context"
            String PortalName { get; }
            dtoProjectContext CurrentListContext { get; set; }
            dtoItemsFilter LastFilterSettings { get; set; }
        #endregion

        #region "Common"
            void RedirectToUrl(String url);
            void DisplaySessionTimeout();
        #endregion            
    }
}