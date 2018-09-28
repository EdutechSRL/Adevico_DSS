using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class liteProjectActivityAssignment : DomainObject<long>
    {
        public virtual Int32 IdPerson { get; set; }
        //public virtual long IdResource { get; set; }
        public virtual liteResource Resource { get; set; }
        //public virtual long IdActivity { get { return (Activity != null) ? Activity.Id : 0; } }
        //public virtual long IdProject { get { return (Project != null) ? Project.Id : 0; } }
        public virtual litePmActivity Activity { get; set; }
        public virtual liteProjectSettings Project { get; set; }
        public virtual ProjectVisibility Visibility { get; set; }
        public virtual int Completeness { get; set; }
        public virtual ActivityRole Role { get; set; }
        public virtual long Permissions { get; set; }
        public virtual Boolean IsApproved { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public liteProjectActivityAssignment()
        {
        }
    }
}