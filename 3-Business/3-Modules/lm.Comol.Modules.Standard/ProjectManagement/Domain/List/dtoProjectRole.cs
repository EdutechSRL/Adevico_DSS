using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoProjectRole
    {
        public virtual String LongName { get; set; }
        public virtual String ShortName { get; set; }
        public virtual ActivityRole ProjectRole { get; set; }
        public dtoProjectRole() { }
    }
}