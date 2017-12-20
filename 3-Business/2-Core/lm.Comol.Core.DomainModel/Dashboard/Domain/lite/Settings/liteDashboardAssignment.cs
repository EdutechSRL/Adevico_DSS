using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class liteDashboardAssignment :DomainBaseObjectMetaInfo<long>, ICloneable , IDisposable
    {
        public virtual long IdDashboard { get; set; }
        public virtual Int32 IdProfileType { get; set; }
        public virtual Int32 IdRole { get; set; }
        public virtual Int32 IdPerson { get; set; }
        public virtual DashboardAssignmentType Type { get; set; }
        public virtual object Clone()
        {
            liteDashboardAssignment clone = new liteDashboardAssignment();
            clone.IdProfileType = IdProfileType;
            clone.IdRole = IdRole;
            clone.IdPerson = IdPerson;
            clone.Type = Type;
            clone.IdDashboard = IdDashboard;
            clone.Deleted = Deleted;
            clone.Id = Id;
            return clone;
        }

        public void Dispose()
        {
            
        }
    }
}
