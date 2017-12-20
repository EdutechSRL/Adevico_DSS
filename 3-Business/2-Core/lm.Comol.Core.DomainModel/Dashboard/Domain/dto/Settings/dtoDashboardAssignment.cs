using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class dtoDashboardAssignment : lm.Comol.Core.DomainModel.DomainBaseObject<long>
    {
        public virtual String DisplayName { get; set; }
        public virtual Int32 IdProfileType { get; set; }
        public virtual Int32 IdRole { get; set; }
        public virtual Int32 IdPerson { get; set; }
        public virtual DashboardAssignmentType Type { get; set; }
    }
}