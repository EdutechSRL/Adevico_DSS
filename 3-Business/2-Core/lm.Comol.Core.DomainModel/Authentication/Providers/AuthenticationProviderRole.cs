using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class AuthenticationProviderRole : DomainBaseObject<long>
    {
        public virtual String Name { get; set; }
        public virtual String Alias { get; set; }
        public virtual String Code { get; set; }
        public virtual AuthenticationProvider Provider { get; set; }
        public virtual int IdProfile { get; set; }
        public virtual int IdOrganization { get; set; }

        public AuthenticationProviderRole()
        {

        }
        public AuthenticationProviderRole(String name, String alias, int idProfile)
        {
            Name = name;
            Alias = alias;
            IdProfile = idProfile;
        }
    } 
}