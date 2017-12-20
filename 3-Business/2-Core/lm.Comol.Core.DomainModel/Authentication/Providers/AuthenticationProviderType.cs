using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public enum AuthenticationProviderType
    {
        None = 0,
        Internal = 1,
        Ldap = 2,
        Url = 3,
        ActiveDirectory= 4,
        WebService = 5,
        ExternalAPI = 6,
        Shibboleth = 7,
        UrlMacProvider = 8
    };
}
