using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class dtoMacUrlProviderIdentifier
    {
        public virtual long IdProvider { get; set; }
        public virtual Boolean isEnabled { get; set; }
        public virtual ApplicationAttribute Application { get; set; }
        public virtual FunctionAttribute Function { get; set; }
    }
}