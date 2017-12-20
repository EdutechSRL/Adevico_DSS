using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class ShibbolethAuthenticationProvider : AuthenticationProvider
    {
        public virtual IList<AuthenticationProviderAttribute> Attributes { get; set; }
        public virtual IList<AuthenticationProviderRole> Roles { get; set; }
        public virtual ShibbolethRemoteApi RemoteApi { get; set; }
        
        public ShibbolethAuthenticationProvider()
        {
            ProviderType = AuthenticationProviderType.Shibboleth;
            LogoutMode = Authentication.LogoutMode.logoutMessage;
        }
    } 
}