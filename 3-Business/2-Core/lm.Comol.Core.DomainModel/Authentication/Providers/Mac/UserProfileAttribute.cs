using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class UserProfileAttribute : BaseUrlMacAttribute
    {
        public virtual ProfileAttributeType Attribute {get;set;}

        public UserProfileAttribute()
        {
            Type = UrlMacAttributeType.profile;
        }
    } 
}