using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.PolicyManagement
{
    [Serializable]
    public class dtoUserPolicyInfo 
    {
        public virtual long Id { get; set; }
        public virtual long PolicyId { get; set; }
        public virtual Boolean Accepted { get; set; }
    }
}
