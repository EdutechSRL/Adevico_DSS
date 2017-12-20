using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable,Flags]
    public enum TransferPolicy
    {
        none = 0,
        skipAnalysis = 1,
        deletePreviousFiles = 2,
        deletePreviousAnalysis = 4,
        keepPreviousAnalysis = 8,
    }
}
