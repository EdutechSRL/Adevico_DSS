using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.ProjectManagement.Domain; 

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation 
{
    public interface IViewBase : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        void DisplaySessionTimeout();
        void DisplayNoPermission(int idCommunity, int idModule);
    }
}