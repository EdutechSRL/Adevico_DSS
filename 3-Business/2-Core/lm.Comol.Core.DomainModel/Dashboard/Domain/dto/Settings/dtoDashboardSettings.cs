using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class dtoDashboardSettings : lm.Comol.Core.DomainModel.DomainBaseObject<long>
    {
        public virtual dtoPermission Permissions { get; set; } 
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual DashboardType Type { get; set; }
        public virtual AvailableStatus Status { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual String CommunityName { get; set; }
        public virtual Boolean ForAll { get; set; }
        public virtual Boolean Active { get; set; }
        public virtual Boolean FullWidth { get; set; }
        public virtual List<dtoDashboardAssignment> Assignments { get; set; }
        public virtual Int32 IdCreatedBy { get; set; }
        public virtual Int32 IdModifiedBy { get; set; }
        public virtual String ModifiedBy { get; set; }
        public virtual String TranslatedStatus { get; set; }
        
        public virtual DateTime ModifiedOn { get; set; }
        public dtoDashboardSettings() {
            Assignments = new List<dtoDashboardAssignment>();
            Permissions = new dtoPermission();
        }
        public dtoDashboardSettings(liteDashboardSettings s, ModuleDashboard permissions, Int32 idCurrentUser, Dictionary<lm.Comol.Core.Dashboard.Domain.AvailableStatus, String> status)
        {
            Id = s.Id;
            Deleted = s.Deleted;
            Name = s.Name;
            Description = s.Description;
            Type = s.Type;
            Status = s.Status;
            ForAll = s.ForAll;
            Active = s.Active;
            FullWidth = s.FullWidth;
            IdCommunity = s.IdCommunity;
            IdCreatedBy = s.IdCreatedBy;
            IdModifiedBy = s.IdModifiedBy;
            ModifiedOn = s.ModifiedOn;
            TranslatedStatus = status[s.Status];
            if (s.Assignments.Any())
                Assignments = s.Assignments.Where(a => (s.Deleted == BaseStatusDeleted.None && a.Deleted == BaseStatusDeleted.None) || (s.Deleted == BaseStatusDeleted.Manual && a.Deleted == BaseStatusDeleted.Cascade)).Select(a =>
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

            Boolean editingEnabled = true;// (s.Type != sType.CommunityType || (s.CommunityTypes != null && !s.CommunityTypes.Where(i => idCommunityTypes.Contains(i)).Any()));

            Permissions = new dtoPermission();

            Permissions.AllowView = permissions.List || permissions.Administration || permissions.Edit;
            Permissions.AllowDelete = editingEnabled && s.Deleted == BaseStatusDeleted.Manual && (permissions.Administration || permissions.DeleteOther || (permissions.DeleteMy && idCurrentUser == s.IdCreatedBy));
            Permissions.AllowVirtualDelete = editingEnabled && !(s.ForAll && s.Active) && s.Deleted == BaseStatusDeleted.None && (permissions.Administration || permissions.DeleteOther || (permissions.DeleteMy && idCurrentUser == s.IdCreatedBy));
            Permissions.AllowUnDelete = editingEnabled && s.Deleted == BaseStatusDeleted.Manual && (permissions.Administration || permissions.DeleteOther || (permissions.DeleteMy && idCurrentUser == s.IdCreatedBy));
            Permissions.AllowEdit = s.Deleted == BaseStatusDeleted.None && (permissions.Administration || permissions.Edit);
            Permissions.AllowSetAvailable = (s.Pages != null && s.Pages.Where(p=> p.Deleted== BaseStatusDeleted.None).Any() ) && s.Deleted == BaseStatusDeleted.None && s.Status != lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available && (permissions.Administration || permissions.Edit);
            Permissions.AllowSetUnavailable =!(s.ForAll && s.Active) && s.Deleted == BaseStatusDeleted.None && s.Status == lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available && (permissions.Administration || permissions.Edit);
            Permissions.AllowClone = s.Deleted == BaseStatusDeleted.None && (permissions.Administration ||  permissions.Clone);
        }
    }
} 