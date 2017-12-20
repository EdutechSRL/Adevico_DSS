using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public enum TimestampFormat
    {
        utc = 0,
        aaaammgghhmmss = 1,
        other = 2
    }
}
