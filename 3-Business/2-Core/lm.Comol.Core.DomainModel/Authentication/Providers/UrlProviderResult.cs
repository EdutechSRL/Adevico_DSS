using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public enum UrlProviderResult
    {
        InternalError = 0,
        ValidToken = 1,
        InvalidToken = 2,
        ExpiredToken = 3,
        NotEvaluatedToken= 4,
        UnknowToken = 5,
        InvalidIpAddress = 6
    };
}