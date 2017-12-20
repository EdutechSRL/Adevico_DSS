using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Domain
{
    /// <summary>
    /// Available status
    /// </summary>
    [Serializable]
    public enum Status
    {
        Draft = 0,
        Active = 1,
        Expired = 2,
        VirtualDeleted = 4
    }
}