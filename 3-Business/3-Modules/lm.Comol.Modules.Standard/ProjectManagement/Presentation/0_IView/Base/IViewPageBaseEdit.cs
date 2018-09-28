using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewPageBaseEdit : IViewPageBase
    {
        #region "Preload"
            long PreloadIdProject { get; }
        #endregion

        #region "Context"
            long IdProject { get; set; }
        #endregion

    
    }
}