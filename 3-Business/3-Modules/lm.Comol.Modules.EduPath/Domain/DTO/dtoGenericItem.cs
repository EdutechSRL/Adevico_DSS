using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoGenericItem
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }    
        public RoleEP RoleEP { get; set; }
        public PermissionEP PermissionEP { get; set; }
        public Status Status { get; set; }
        public Int64 Weight { get; set; }
        public virtual PolicySettings Policy { get; set; }
        public virtual DisplayPolicy Display { get; set; }
        public dtoGenericItem()
        {
            Id = 0;
        }
        public dtoGenericItem(Path oPath,Status PersonalStatus , RoleEP AssRoleEP)
        {
            Id = oPath.Id;
            Name = oPath.Name;
            Description = oPath.Description;
            Status = oPath.Status;
            this.RoleEP = AssRoleEP;
            PermissionEP = new PermissionEP(AssRoleEP);
            Weight = oPath.Weight;
            Policy = oPath.Policy;
            Display = (oPath.Policy != null ? oPath.Policy.DisplaySubActivity : DisplayPolicy.NoModal);
        }
        public dtoGenericItem(Unit oUnit, Status PersonalStatus, RoleEP AssRoleEP)
        {
            Id = oUnit.Id;
            Name = oUnit.Name;
            Description = oUnit.Description;
            Status = oUnit.Status;
            this.RoleEP = AssRoleEP;
            PermissionEP = new PermissionEP(AssRoleEP);
            Weight = oUnit.Weight;
            Display = DisplayPolicy.InheritedByPath;
        }
        public dtoGenericItem(Activity oActivity, Status PersonalStatus, RoleEP AssRoleEP)
        {
            Id = oActivity.Id;
            Name = oActivity.Name;
            Description = oActivity.Description;
            Status = oActivity.Status;
            this.RoleEP = AssRoleEP;
            PermissionEP = new PermissionEP(AssRoleEP);
            Weight = oActivity.Weight;
            Display = DisplayPolicy.InheritedByPath;
        }
        public dtoGenericItem(SubActivity oSubActivity, Status PersonalStatus, RoleEP AssRoleEP)
        {
            Id = oSubActivity.Id;
            Name = oSubActivity.Name;
            Description = oSubActivity.Description;
            Status = oSubActivity.Status;
            this.RoleEP = AssRoleEP;
            PermissionEP = new PermissionEP(AssRoleEP);
            Weight = oSubActivity.Weight;
            Display = oSubActivity.Display;
        }
        public dtoGenericItem(SubActivity oSubActivity,Status PersonalStatus)
        {
            Id = oSubActivity.Id;
            Name = oSubActivity.Name;
            Description = oSubActivity.Description;
            Status = oSubActivity.Status;
            this.RoleEP =RoleEP.None;
            PermissionEP = new PermissionEP(RoleEP);
            Weight = oSubActivity.Weight;
            Display = oSubActivity.Display;
        }
    }
}
