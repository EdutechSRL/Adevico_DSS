using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Authentication
{
     [Serializable]
    public class dtoExternalCredentials
    {
        public virtual long IdentifierLong {get;set;}
        public virtual String IdentifierString { get; set; }
    }
}
