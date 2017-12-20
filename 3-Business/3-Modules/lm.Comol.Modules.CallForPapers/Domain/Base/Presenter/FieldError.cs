using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable, Flags ]
    public enum FieldError
    {
        None = 0,
        Mandatory = 1,
        Invalid = 2,
        InvalidFormat = 4,
        MoreOptions = 8,
        LessOptions = 16,
        InvalidOptions = 32,
        Unknown = 64,
        UserValueMissing = 128,
        TableReportOverValue = 256
    }
}
