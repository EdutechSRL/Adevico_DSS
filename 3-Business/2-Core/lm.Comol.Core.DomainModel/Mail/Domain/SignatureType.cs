using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Mail
{
    [Serializable]
    public enum SignatureType
    {
        None = 0,
        FromConfigurationSettings = 1,
        FromTemplate = 2,
        FromSkin = 3,
        FromField = 4,
        FromNoReplySettings = 5,
    }
}