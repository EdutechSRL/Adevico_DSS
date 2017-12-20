using lm.Comol.Core.DomainModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.CommunityManagement.Presentation
{
    public interface IViewPageBase : IViewBase
    {
        #region "Preload"
            Int32 PreloadIdCommunity { get; }
        #endregion

        #region "Context"
            Int32 IdCurrentCommunity { get; set; }
        #endregion

        #region "Common"
            void SendUserAction(Int32 idCommunity, Int32 idModule, ModuleCommunityManagement.ActionType action);
            void SendUserAction(Int32 idCommunity, Int32 idModule, Int32 idCurrentCommunity, ModuleCommunityManagement.ActionType action);
        #endregion            
    }
}