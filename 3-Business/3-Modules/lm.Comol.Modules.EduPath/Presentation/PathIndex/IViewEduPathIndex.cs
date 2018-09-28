using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Modules.EduPath.Presentation
{
    public interface IViewEduPathIndex : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        void DisplaySessionTimeout();
    }
}