using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class LoginFormat : DomainBaseObject<long>
    {
        public virtual AuthenticationProvider Provider { get; set; }
        public virtual String Name { get; set; }
        public virtual Boolean isDefault { get; set; }
        public virtual String Before { get; set; }
        public virtual String After { get; set; }
    }
}