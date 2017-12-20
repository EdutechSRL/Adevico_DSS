using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileStatistics.Presentation
{
    public interface IViewDisplayDetailScorm  : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        void InitializeNoPermission(int idCommunity);
        void BindData(lm.Comol.Modules.ScormStat.ColPerson Person);
    }
}
