using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum ItemsGroupBy
    {
        None = 0,
        Community = 1,
        EndDate = 2,
        Plain = 4,
        Project = 8,
        CommunityProject = 16
    }
}