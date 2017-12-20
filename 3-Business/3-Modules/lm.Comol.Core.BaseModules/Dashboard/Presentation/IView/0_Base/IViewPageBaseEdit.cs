using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation
{
    public interface IViewPageBaseEdit : IViewPageBase
    {
        #region "Preload"
            long PreloadIdDashboard { get; }
        #endregion

        #region "Context"
            long IdDashboard { get; set; }
        #endregion

    
    }
}