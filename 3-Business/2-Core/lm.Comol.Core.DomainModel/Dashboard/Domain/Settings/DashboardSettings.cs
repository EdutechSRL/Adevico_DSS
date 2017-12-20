using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class DashboardSettings : DomainBaseObjectLiteMetaInfo<long>, ICloneable, IDisposable
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual DashboardType Type { get; set; }

        /// <summary>
        /// Community owner, used only for CommunityDashboard
        /// </summary>
        public virtual liteCommunity Community { get; set; }
        public virtual ContainerSettings Container { get; set; }
       
        public virtual IList<PageSettings> Pages { get; set; }
        public virtual Boolean ForAll { get; set; }
        public virtual Boolean Active { get; set; }
        public virtual Boolean FullWidth { get; set; }
        public virtual lm.Comol.Core.Dashboard.Domain.AvailableStatus Status { get; set; }
        public virtual IList<DashboardAssignment> Assignments { get; set; }
        public DashboardSettings() {
            Container = new ContainerSettings();
            Pages = new List<PageSettings>();
            Assignments = new List<DashboardAssignment>();
        }
        public DashboardSettings(DashboardType type, DashboardSettings dSettings = null) {
            Container = new ContainerSettings(type, dSettings);
            Pages = new List<PageSettings>();
        }

        public virtual DashboardSettings BaseClone()
        {
            DashboardSettings clone = new DashboardSettings();
            clone.Type = Type;
            clone.Community = Community;
            clone.Container = (ContainerSettings)Container.Clone();
            clone.ForAll = ForAll;
            clone.FullWidth = FullWidth;
            clone.Active = (Active) ? false : Active;
            return clone;
        }
        public virtual object Clone()
        {
            DashboardSettings clone =BaseClone();
            clone.Pages = Pages.Select(p => (PageSettings)p.Clone()).ToList();
            if (Assignments.Any())
                clone.Assignments = Assignments.Where(a => a.Deleted == BaseStatusDeleted.None).Select(a => (DashboardAssignment)a.Clone()).ToList();
            return clone;
        }
        public virtual DashboardSettings Copy(litePerson person, String ipAddress, String proxyIpAddress)
        {
            return Copy(person, ipAddress, proxyIpAddress, null);
        }
        public virtual DashboardSettings Copy(litePerson person, String ipAddress, String proxyIpAddress, liteCommunity community = null)
        {
            DashboardSettings clone = BaseClone();
            clone.CreateMetaInfo(person, ipAddress, proxyIpAddress);
            if (community != null) {
                clone.Community = community;
                clone.Type = DashboardType.Community;
            }
            clone.Container = Container.Copy();
            clone.Pages = Pages.Where(p => p.Deleted == BaseStatusDeleted.None).Select(p => p.Copy(clone)).ToList();
            if (Assignments.Any())
                clone.Assignments = Assignments.Where(a => a.Deleted == BaseStatusDeleted.None).Select(a=> a.Copy(clone,person, ipAddress, proxyIpAddress)).ToList();
            return clone;
        }
        public virtual List<DashboardAssignment> GetAssignments(DashboardAssignmentType type)
        {
            return (Assignments == null) ? new List<DashboardAssignment>() : Assignments.Where(a => a.Deleted == BaseStatusDeleted.None  && a.Type == type).ToList();
        }

        public void Dispose()
        {
          
        }
    }
} 