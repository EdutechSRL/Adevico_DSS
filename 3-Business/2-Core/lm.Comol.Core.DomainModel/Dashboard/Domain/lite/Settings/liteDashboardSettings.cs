using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class liteDashboardSettings : DomainBaseObject<long>, ICloneable, IDisposable
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual DashboardType Type { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual ContainerSettings Container { get; set; }
       
        public virtual IList<litePageSettings> Pages { get; set; }
        public virtual Boolean ForAll { get; set; }
        public virtual Boolean Active { get; set; }
        public virtual lm.Comol.Core.Dashboard.Domain.AvailableStatus Status { get; set; }
        public virtual Boolean FullWidth { get; set; }
        public virtual IList<liteDashboardAssignment> Assignments { get; set; }
        public virtual Int32 IdCreatedBy { get; set; }
        public virtual Int32 IdModifiedBy { get; set; }
        public virtual DateTime ModifiedOn { get; set; }
        public liteDashboardSettings() {
            Container = new ContainerSettings();
            Pages = new List<litePageSettings>();
            Assignments = new List<liteDashboardAssignment>();
        }

        public virtual Boolean IsAvailableFor(Int32 idUser, Int32 idRole, Int32 idProfileType){
            return Assignments.Where(a=> a.Deleted== BaseStatusDeleted.None && (a.IdPerson== idUser || a.IdRole== idRole || a.IdProfileType== idProfileType)).Any();
        }
       public virtual object Clone()
        {
            liteDashboardSettings clone = new liteDashboardSettings();
            clone.Type = Type;
            clone.IdCommunity = IdCommunity;
            clone.Container = (ContainerSettings)Container.Clone();
            clone.Pages = Pages.Select(p => (litePageSettings)p.Clone()).ToList();
            clone.ForAll = ForAll;
            clone.FullWidth = FullWidth;
            clone.Status = Status;
            clone.IdCreatedBy = IdCreatedBy;
            clone.IdModifiedBy = IdModifiedBy;
            clone.ModifiedOn = ModifiedOn;
            clone.Active = (Active) ? false : Active;
            if (Assignments.Any())
                clone.Assignments = Assignments.Where(a => a.Deleted == BaseStatusDeleted.None).Select(a => (liteDashboardAssignment)a.Clone()).ToList();
            clone.Deleted = Deleted;
            clone.Id = Id;
            return clone;
        }

       public virtual List<liteDashboardAssignment> GetAssignments(DashboardAssignmentType type)
       {
           return (Assignments == null) ? new List<liteDashboardAssignment>() : Assignments.Where(a => a.Deleted == BaseStatusDeleted.None && a.Type == type).ToList();
       }
        public void Dispose()
        {
          
        }

       
    }
} 