using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class dtoInternalCredentials
    {
        public virtual String Login {get;set;}
        public virtual String Password { get; set; }
    }
}
