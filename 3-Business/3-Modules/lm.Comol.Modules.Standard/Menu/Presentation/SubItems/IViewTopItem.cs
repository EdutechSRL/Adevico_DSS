using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.Menu.Domain;
using lm.Comol.Core.DomainModel.Common;

namespace lm.Comol.Modules.Standard.Menu.Presentation
{
    public interface IViewTopItem : iDomainView
    {
        long IdItem { get; }
        dtoTopMenuItem GetTopItem { get; }
        void InitalizeControl(dtoTopMenuItem item, Boolean allowEdit);
    }
}