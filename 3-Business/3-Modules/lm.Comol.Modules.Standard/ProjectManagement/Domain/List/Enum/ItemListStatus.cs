using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum ItemListStatus
    {
        All = 0,
        Active = 1,
        Completed = 2,
        Future = 4,
        Deleted = 5,
        Late = 6,
        DueForCompletion = 7,
        Draft = 8,
        Ignore = 9
    }
}