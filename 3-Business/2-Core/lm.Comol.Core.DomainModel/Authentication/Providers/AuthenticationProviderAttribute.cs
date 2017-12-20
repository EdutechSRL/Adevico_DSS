using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class AuthenticationProviderAttribute : DomainBaseObject<long>
    {
        public virtual String Name { get; set; }
        public virtual String Alias { get; set; }
        public virtual String Description { get; set; }
        public virtual AttributeType Type { get; set; }
        public virtual AuthenticationProvider Provider { get; set; }
        public AuthenticationProviderAttribute()
        {

        }
        public AuthenticationProviderAttribute(String name, AttributeType type)
        {
            Name = name;
            Type = type;
        }
        public AuthenticationProviderAttribute(String name, String alias, AttributeType type)
        {
            Name = name;
            Alias = alias;
            Type = type;
        }
        //public AuthenticationProviderAttribute(String name, String alias, AuthenticationProviderAttribute type,AuthenticationProvider provider)
        //{
        //    Name = name;
        //    Alias = alias;
        //    Type = type;
        //    Provider = Provider;
        //}
    } 
}