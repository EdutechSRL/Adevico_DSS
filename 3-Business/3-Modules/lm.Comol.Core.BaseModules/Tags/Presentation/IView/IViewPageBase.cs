using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.Tag.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tags.Presentation 
{
    public interface IViewPageBase : IViewBase
    {
        #region "Preload"
            Boolean PreloadForOrganization { get; }
            Boolean PreloadRecycleBin { get; }
        #endregion
        Int32 IdTagsCommunity { get; set; }
        void SetBackUrl(String url);
        void SetRecycleUrl(String url);
        void AllowAdd(Boolean allow);
        void AllowMultiple(Boolean allow);
        void InitializeListControl(ModuleTags permissions, Int32 idCommunity, Boolean fromRecycleBin = false, Boolean fromOrganization = false);
    }
}