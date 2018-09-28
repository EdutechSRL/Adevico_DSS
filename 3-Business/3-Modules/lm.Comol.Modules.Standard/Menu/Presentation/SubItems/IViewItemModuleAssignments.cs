using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.Menu.Domain;
using lm.Comol.Core.DomainModel.Common;

namespace lm.Comol.Modules.Standard.Menu.Presentation
{
    public interface IViewItemModuleAssignments : iDomainView
    {
        int GetModuleId { get; }
        long GetPermission { get; }
        void InitalizeControl(Int32 idModule, long permissions, Boolean allowEdit);
    }
}