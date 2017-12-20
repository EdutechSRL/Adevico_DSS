using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class dtoBaseDashboardSettings : lm.Comol.Core.DomainModel.DomainBaseObject<long>
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual DashboardType Type { get; set; }
        public virtual AvailableStatus Status { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual String CommunityName { get; set; }
        public virtual Boolean ForAll { get; set; }
        public virtual Boolean Active { get; set; }
        public virtual List<dtoDashboardAssignment> Assignments { get; set; }
        public dtoBaseDashboardSettings()
        {
            Assignments = new List<dtoDashboardAssignment>();
        }
        public dtoBaseDashboardSettings(liteDashboardSettings s)
        {
            Id = s.Id;
            Deleted = s.Deleted;
            Name = s.Name;
            Description = s.Description;
            Type = s.Type;
            Status = s.Status;
            ForAll = s.ForAll;
            Active = s.Active;
            IdCommunity = s.IdCommunity;
            if (s.Assignments.Any())
                Assignments = s.Assignments.Where(a => a.Deleted == BaseStatusDeleted.None).Select(a =>
                            new dtoDashboardAssignment()
                            {
                                Id = a.Id,
                                IdPerson = a.IdPerson,
                                IdProfileType =a.IdProfileType,
                                IdRole = a.IdRole,
                                Type = a.Type
                            }).ToList();
            else
                Assignments = new List<dtoDashboardAssignment>();
        }
        public virtual List<dtoDashboardAssignment> GetAssignments(DashboardAssignmentType type)
        {
            return (Assignments == null) ? new List<dtoDashboardAssignment>() : Assignments.Where(a => a.Type == type).ToList();
        }
    }
} 