using lm.Comol.Core.Dashboard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation
{
    public interface IViewPageBase : IViewBase
    {
        #region "Preload"
            Int32 PreloadIdCommunity { get; }

        #endregion

        #region "Context"
            Int32 DashboardIdCommunity { get; set; }
        #endregion

        #region "Common"
            void SendUserAction(int idCommunity, int idModule,  ModuleDashboard.ActionType action);    
            void SendUserAction(int idCommunity, int idModule, long idDashboard, ModuleDashboard.ActionType action);
        #endregion            
    }
}