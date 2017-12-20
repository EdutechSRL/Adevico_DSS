using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Tiles.Presentation 
{
    public interface IViewBase : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        void DisplaySessionTimeout();
        void DisplayNoPermission(int idCommunity, int idModule);

        #region "Common"
        void SendUserAction(int idCommunity, int idModule, ModuleDashboard.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idTile, ModuleDashboard.ActionType action);
        #endregion          
    }
}