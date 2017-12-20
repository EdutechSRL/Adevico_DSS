using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public interface IViewBase : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        void DisplaySessionTimeout();
        void DisplayNoPermission(Int32 idCommunity, Int32 idModule);
    }
}