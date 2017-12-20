using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Authentication
{
    public enum UrlUserTokenFormat
    {
        PrefixDateTimeLogin = 1,
        DateTimeLogin = 2,
        LoginDateTime = 3,
        PrefixLoginDateTime = 4,
        LongDateTime = 5
    };
}