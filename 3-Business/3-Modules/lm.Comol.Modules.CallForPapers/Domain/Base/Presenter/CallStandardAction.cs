using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public enum CallStandardAction
    {
        None = 0,
        List = 1,
        Edit = 2,
        Add = 4,
        Manage = 8,
        Evaluate = 16,
        ViewOwnSubmission = 32,
        ViewRevisions = 64,
        ManageRevisions = 128,
        PreviewCall = 256
    }
}
