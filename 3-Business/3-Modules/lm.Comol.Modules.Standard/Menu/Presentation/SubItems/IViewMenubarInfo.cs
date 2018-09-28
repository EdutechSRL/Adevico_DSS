using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.Menu.Domain;
using lm.Comol.Core.DomainModel.Common;

namespace lm.Comol.Modules.Standard.Menu.Presentation
{
    public interface IViewMenubarInfo : iDomainView
    {
        long IdMenubar { get; }
        dtoMenubar GetMenubar{ get; }
        void InitalizeControl(dtoMenubar menubar, Boolean allowEdit);
    }
}