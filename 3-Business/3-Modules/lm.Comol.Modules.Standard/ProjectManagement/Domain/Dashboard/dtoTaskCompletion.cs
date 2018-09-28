using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoTaskCompletion
    {
        public virtual long IdTask { get; set; }
        public virtual Int32 Completion { get; set; }
        public virtual ProjectItemStatus Status { get; set; }
        public virtual Boolean IsCompleted { get; set; }
        public dtoTaskCompletion() 
        {
        }
    }
}