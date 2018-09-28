using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoProjectContext 
    {
        public virtual Boolean isPersonal { get; set; }
        public virtual Boolean isForPortal { get; set; }
        public virtual Int32 IdCommunity { get; set; }

        public virtual Boolean isValid { get { return (isForPortal && IdCommunity==0) || (!isForPortal && IdCommunity>0);}}

    }
}