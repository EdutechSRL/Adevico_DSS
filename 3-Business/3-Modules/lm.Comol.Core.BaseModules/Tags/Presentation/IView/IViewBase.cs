using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Tag.Domain;

namespace lm.Comol.Core.BaseModules.Tags.Presentation 
{
    public interface IViewBase : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        void DisplaySessionTimeout();
        void DisplayNoPermission(int idCommunity, int idModule);

        #region "Common"
        void SendUserAction(int idCommunity, int idModule, ModuleTags.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idTag, ModuleTags.ActionType action);
        #endregion          
    }
}