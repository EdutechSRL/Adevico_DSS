using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoCompletion 
    {
        public virtual Int32 Completeness { get; set; }
        public virtual Boolean IsCompleted { get; set; }
    }
}