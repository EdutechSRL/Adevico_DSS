using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoMyAssignmentCompletion
    {
        public virtual long IdProject { get; set; }
        public virtual long IdTask { get; set; }
        public virtual long IdAssignment { get; set; }
        public virtual long IdResource { get; set; }
        public virtual dtoField<String> MyCompletion { get; set; }
        public virtual Int32 TaskCompletion { get; set; }
        public virtual ProjectItemStatus TaskStatus { get; set; }
        public virtual Boolean TaskIsCompleted { get; set; }
        public virtual Dictionary<ResourceActivityStatus, long> ProjectCompletion { get; set; }
        public dtoMyAssignmentCompletion() 
        {
            MyCompletion = new dtoField<String>();
            //ProjectCompletion = new Dictionary<ResourceActivityStatus, long>();
        }
    }
}