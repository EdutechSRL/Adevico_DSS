using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Authentication
{
    public class AuthenticationProviderTranslation : DomainBaseObject<long>
    {
        public virtual Language Language { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual String ForSubscribeName { get; set; }
        public virtual String ForSubscribeDescription { get; set; }
        public virtual String FieldLong { get; set; }
        public virtual String FieldString { get; set; }
        public virtual AuthenticationProvider Provider { get; set; }
    }
}
