using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class dtoBaseProviderAttribute
    {
        public virtual long Id { get; set; }
        public virtual String Name { get; set; }
        public virtual AttributeType Type { get; set; }

        public dtoBaseProviderAttribute()
        {

        }
        public dtoBaseProviderAttribute(String name, AttributeType type)
        {
            Name = name;
            Type = type;
        }
        public dtoBaseProviderAttribute(long id, String name, AttributeType type)
        {
            Id=id;
            Name = name;
            Type = type;
        }

        public dtoBaseProviderAttribute(AuthenticationProviderAttribute attribute)
        {
            Name = attribute.Name;
            Id = attribute.Id;
            Type = attribute.Type;
        }
    } 
}