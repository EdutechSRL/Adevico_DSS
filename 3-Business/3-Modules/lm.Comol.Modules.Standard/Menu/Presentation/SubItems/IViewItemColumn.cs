using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.Menu.Domain;
using lm.Comol.Core.DomainModel.Common;

namespace lm.Comol.Modules.Standard.Menu.Presentation
{
    public interface IViewItemColumn : iDomainView
    {
        long IdColumn { get; }
        dtoColumn GetColumn { get; }
        void InitalizeControl(dtoColumn column, Boolean allowEdit);
    }
}