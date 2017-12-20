using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Errors.Presentation
{
    public interface IViewErrorMessage: lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean PreloadedDisplayError{ get; }
    }
}