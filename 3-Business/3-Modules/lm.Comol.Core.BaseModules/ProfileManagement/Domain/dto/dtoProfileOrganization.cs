using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public class dtoProfileOrganization
    {
        public virtual Int32 IdProfile { get; set; }
        public virtual Int32 IdOrganization { get; set; }
        public virtual String Name { get; set; }
        public virtual Boolean isDefault { get; set; }
    }
}