using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.Menu.Domain;
using lm.Comol.Core.DomainModel.Common;

namespace lm.Comol.Modules.Standard.Menu.Presentation
{
    public interface IViewProfileTypeAssignments : iDomainView
    {
        List<int> GetSelectedTypes { get; }
        void InitalizeControl(List<int> selectedTypes, List<int> availableTypes, Boolean allowEdit);
    }
}