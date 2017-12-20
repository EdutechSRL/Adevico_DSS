using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class LDAPAuthenticationProvider : AuthenticationProvider
    {
        public LDAPAuthenticationProvider()
        {
            this.ProviderType = AuthenticationProviderType.Ldap;
        }
    } 
}