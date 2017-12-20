using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.BaseModules.FileStatistics.Domain;

namespace lm.Comol.Core.BaseModules.FileStatistics.Presentation
{
    public interface IViewDisplayDetailfile : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        void InitializeNoPermission(int idCommunity);
        void BindData(dtoFileDetail FileDetail, Int32 Perm);
    }
}
