using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Presentation
{
    public interface IViewBaseEduPath : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        /*String ServiceTitle { get; set; }*/

        void DisplayWrongPageAccess();
        void DisplayNoPermission();
        void DisplaySessionTimeout();

        void SendUserAction(int idCommunity, int idModule, ModuleEduPath.ActionType action);
    }
}
