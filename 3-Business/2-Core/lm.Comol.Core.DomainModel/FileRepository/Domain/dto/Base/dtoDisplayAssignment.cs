using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoDisplayAssignment
    {
        public virtual long Id { get; set; }
        public virtual String DisplayName { get; set; }
        public virtual dtoDisplayPermissions TranslatedPermissions { get; set; }
        public virtual long Permissions { get; set; }
        public virtual Boolean Denyed { get; set; }
        public virtual Boolean Inherited { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdRole { get; set; }
        public virtual Int32 IdPerson { get; set; }
        public virtual AssignmentType Type { get; set; }
        public virtual Boolean HasMoreItems { get; set; }
        public virtual long MoreItems { get; set; }

        public dtoDisplayAssignment()
        {
            TranslatedPermissions = new dtoDisplayPermissions() { Type = PermissionType.none, Value = "" };
        }

        public static dtoDisplayAssignment Create(liteItemAssignments assignment, String translation="")
        {
            dtoDisplayAssignment item = new dtoDisplayAssignment();
            item.Id = assignment.Id;
            item.Permissions = assignment.Permissions;
            item.Denyed = assignment.Denyed;
            item.Inherited = assignment.Inherited;
            item.IdCommunity = assignment.IdCommunity;
            item.IdRole = assignment.IdRole;
            item.IdPerson = assignment.IdPerson;
            item.Type = assignment.Type;
            item.SetPermissionsTranslation(translation);
            return item;
        }

        public void SetPermissionsTranslation(String translation){
            if (TranslatedPermissions==null)
                TranslatedPermissions = new dtoDisplayPermissions();
            TranslatedPermissions.Type= (Inherited ? PermissionType.inherited: PermissionType.none) | (Denyed ? PermissionType.denyed: PermissionType.allowed)
                | (Permissions==0 ? PermissionType.nopermissions : PermissionType.none ) | (HasMoreItems ? PermissionType.needmoreinfo: PermissionType.none);
            TranslatedPermissions.Value = translation;
        }
        public virtual Int32 OrderByType()
        {
            switch (Type)
            {
                case AssignmentType.person:
                    return Int32.MaxValue;
                default:
                    return (Int32)Type;
            }
        }
        public virtual Int32 OrderByInherited()
        {
            return (Inherited ? 0 : 1);
        }
        public virtual Int32 OrderByDenyed()
        {
            return (Denyed ? 0 : 1);
        }
        public virtual Int32 OrderByMoreItems()
        {
            return (HasMoreItems ? 0 : 1);
        }
        public String ToString()
        {
            return "Id-" + Id + "-" + DisplayName + "-" + Type.ToString() + "- Inherited:" + Inherited.ToString() + "- Denyed:" + Denyed.ToString();
        }
    }

    [Serializable]
    public class dtoDisplayPermissions
    {
        public virtual PermissionType Type { get; set; }
        public virtual String Value { get; set; }


        public static dtoDisplayPermissions Create(long permissions,Boolean inherited, Boolean denyed,Boolean hasMoreItems, String translation)
        {
            dtoDisplayPermissions item = new dtoDisplayPermissions();
            item.Type = (inherited ? PermissionType.inherited : PermissionType.none) | (denyed ? PermissionType.denyed : PermissionType.allowed)
                | (permissions == 0 ? PermissionType.nopermissions : PermissionType.none) | (hasMoreItems ? PermissionType.needmoreinfo : PermissionType.none);
            item.Value = translation;
            return item;
        }

        public String ToString()
        {
            return "Type-" + Type.ToString() + "- Value:" + Value;
        }
    }
    [Serializable, Flags]
    public enum PermissionType: int { 
        none = 0,
        inherited = 1,
        denyed = 2,
        allowed = 4,
        nopermissions = 8,
        needmoreinfo = 16
    }
}