using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable,Flags]
    public enum SubmitterTypeError
    {
        None = 0,
        InvalidName = 1,
        InvalidMaxNumber = 2,
        EmptyMaxNumber = 4,
        ErrorSavingData = 8,
        SubmitterUsed = 16
    }
}
