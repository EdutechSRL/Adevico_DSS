using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum ProjectOrderBy
    {
        None = 0,
        Name = 1,
        Deadline = 2,
        Completion = 4,
        MyCompletion = 8,
        CommunityName = 16,
        EndDate = 32,
        TaskName = 64
    }
}