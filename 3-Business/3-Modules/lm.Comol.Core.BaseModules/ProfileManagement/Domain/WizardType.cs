using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public enum WizardType
    {
        None = 0,
        Internal = 1,
        Shibboleth = 2,
        UrlProvider = 3,
        Ldap = 4,
        Administration = 5,
        MacUrl = 6
    }
}