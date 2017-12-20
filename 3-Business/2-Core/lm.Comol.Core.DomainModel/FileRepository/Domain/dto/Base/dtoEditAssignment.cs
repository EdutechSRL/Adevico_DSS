using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoEditAssignment : IEquatable<dtoEditAssignment>
    {
        public virtual long Id { get; set; }
        public virtual String DisplayName { get; set; }
        public virtual dtoDisplayPermissions TranslatedPermissions { get; set; }
        public virtual long Permissions { get; set; }
        public virtual Boolean Denyed { get; set; }
        public virtual Boolean OldDenyed { get; set; }
        
        public virtual Boolean Inherited { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdRole { get; set; }
        public virtual Int32 IdPerson { get; set; }
        public virtual AssignmentType Type { get; set; }
        public virtual Boolean ReadOnly { get; set; }
        public virtual Boolean IsDeleted { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual Boolean IsAutoAdded { get; set; }
        public dtoEditAssignment()
        {
            TranslatedPermissions = new dtoDisplayPermissions() { Type = PermissionType.none, Value = "" };
        }

        public static dtoEditAssignment Create(ItemAssignments assignment, String translation = "")
        {
            dtoEditAssignment item = new dtoEditAssignment();
            item.Id = assignment.Id;
            item.Permissions = assignment.Permissions;
            item.Denyed = assignment.Denyed;
            item.Inherited = assignment.Inherited;
            item.IdCommunity = assignment.IdCommunity;
            item.IdRole = assignment.IdRole;
            item.IdPerson = assignment.IdPerson;
            item.Type = assignment.Type;
            item.SetPermissionsTranslation(translation);
            item.ReadOnly = assignment.Inherited;
            item.CreatedOn = assignment.CreatedOn;
            return item;
        }
        public void SetPermissionsTranslation(String translation){
            if (TranslatedPermissions==null)
                TranslatedPermissions = new dtoDisplayPermissions();
            TranslatedPermissions.Type= (Inherited ? PermissionType.inherited: PermissionType.none) | (Denyed ? PermissionType.denyed: PermissionType.allowed)
                | (Permissions==0 ? PermissionType.nopermissions : PermissionType.none );
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

        public bool Equals(dtoEditAssignment other)
        {
            return other.Type == Type && other.IdCommunity == IdCommunity && other.IdPerson == IdPerson && other.IdRole == other.IdRole && other.Inherited == Inherited;
        }
    }
}