using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    public class ProjectActivityAssignment : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual litePerson Person { get; set; }
        public virtual ProjectResource Resource { get; set; }
        public virtual PmActivity Activity { get; set; }
        public virtual Project Project { get; set; }
        public virtual ProjectVisibility Visibility { get; set; }
        public virtual int Completeness { get; set; }
        public virtual ActivityRole Role { get; set; }
        public virtual long Permissions { get; set; }
        public virtual Boolean IsApproved { get; set; }

        public ProjectActivityAssignment()
        {
        }
    }
}