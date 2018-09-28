using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewBaseDashboard : IViewPageBase
    {
        #region "Preload"
            Boolean PreloadFromCookies { get; }
            SummaryTimeLine PreloadTimeLine { get; }
            ItemsGroupBy PreloadGroupBy { get; }
            ProjectFilterBy PreloadFilterBy { get; }
            ItemListStatus PreloadFilterStatus { get; }
            SummaryDisplay PreloadDisplay { get; }
            SummaryTimeLine PreloadActivityTimeline { get; }
            UserActivityStatus PreloadUserActivityStatus { get; }
        #endregion

        #region "Context"
            String PortalName { get; }
            dtoProjectContext DashboardContext { get; set; }
            dtoItemsFilter LastFilterSettings { get; set; }
        #endregion

        #region "Common"
            String GetCurrentUrl();
            void RedirectToUrl(String url);
            void DisplaySessionTimeout(Int32 idCommunity, String url);
        #endregion            
    }
}