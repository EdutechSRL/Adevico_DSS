using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    [Serializable]
    public enum AgencyVisibility
    {
        None = 0,
        NotDeleted = 1,
        Active = 2,
        Deleted = 4
    }
}
