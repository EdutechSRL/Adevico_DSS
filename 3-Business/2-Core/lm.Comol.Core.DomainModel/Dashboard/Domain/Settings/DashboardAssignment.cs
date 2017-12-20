using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class DashboardAssignment : DomainBaseObjectLiteMetaInfo<long>, ICloneable, IDisposable
    {
        public virtual DashboardSettings Dashboard { get; set; }
        public virtual Int32 IdProfileType { get; set; }
        public virtual Int32 IdRole { get; set; }
        public virtual Int32 IdPerson { get; set; }
        public virtual DashboardAssignmentType Type { get; set; }
        public virtual object Clone()
        {
            DashboardAssignment clone = new DashboardAssignment();
            clone.IdProfileType = IdProfileType;
            clone.IdRole = IdRole;
            clone.IdPerson = IdPerson;
            clone.Type = Type;
            return clone;
        }
        public virtual DashboardAssignment Copy(DashboardSettings dashboard,litePerson person, String ipAddress, String proxyIpAddress)
        {
            DashboardAssignment item = (DashboardAssignment)Clone();
            item.CreateMetaInfo(person, ipAddress, proxyIpAddress);
            item.Dashboard = dashboard;
            return item;
        }
        public void Dispose()
        {
            
        }
    }
}
