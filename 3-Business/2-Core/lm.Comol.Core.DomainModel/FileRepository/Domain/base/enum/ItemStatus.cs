using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public enum ItemStatus
    {
        Draft = 0,
        Active = 1,
        Replaced = 2,
        Removed = 3
    }
}