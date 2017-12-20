using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class dtoTemplateAssignment : DomainBaseObject<long>
    {
        public virtual long IdVersion { get; set; }
        public virtual Boolean Clone { get; set; }
        public virtual Boolean Edit { get; set; }
        public virtual Boolean Use { get; set; }
        public virtual Boolean ChangePermission { get; set; }
        public virtual PermissionType Type { get; set; }

        public virtual String DisplayName { get; set; }
        public virtual Boolean IsDefault { get; set; }
        public virtual Boolean IsUnknown { get; set; }
        public virtual Boolean AllowEdit { get; set; }
        public dtoTemplateAssignment()
        {
            AllowEdit = true;
        }
        public dtoTemplateAssignment(VersionPermission p) {
            Id = p.Id;
            Deleted = p.Deleted;
            Clone = p.Clone;
            Edit = p.Edit;
            Use = p.See;
            ChangePermission = p.ChangePermission;
            Type = p.Type;
            IdVersion = (p.Version != null) ? p.Version.Id : 0;
            AllowEdit = true;
        }

        public long PermissionToLong() {
            return ((Use) ? 1 : 0) | ((Edit) ? 2 : 0) | ((Clone) ? 4 : 0) | ((ChangePermission) ? 8 : 0);
        }
        public void UpdatePermissions(dtoTemplateAssignment p)
        {
            if (p != null)
            {
                this.ChangePermission = p.ChangePermission;
                this.Edit = p.Edit;
                this.Clone = p.Clone;
                this.Use = p.Use;
            }
        }
    }
    public class dtoPortalAssignment : dtoTemplateAssignment
    {
        //public virtual List<dtoModuleAssignment> Modules { get; set; }
        public virtual List<dtoProfileTypeAssignment> ProfileTypes { get; set; }
        public virtual Boolean ForAllUsers { get { return (ProfileTypes == null || !ProfileTypes.Any()); } }
        public dtoPortalAssignment()
        {
            Type = PermissionType.Portal;
            ProfileTypes = new List<dtoProfileTypeAssignment>();
            //Modules = new List<dtoModuleAssignment>();
        }

        public void UpdateItemsDisplayname(Dictionary<Int32, String> translations)
        {
            foreach (dtoProfileTypeAssignment pType in ProfileTypes)
            {
                pType.UpdateDisplayname(translations);
            }
            ProfileTypes = ProfileTypes.OrderBy(p => p.DisplayName).ToList();
        }
        public void UpdateEditing(Boolean edit) {
            if (ProfileTypes != null)
                ProfileTypes.ForEach(p => p.AllowEdit = edit);
        }
    }

    [Serializable()]
    public class dtoCommunityAssignment : dtoTemplateAssignment
    {
        public virtual List<dtoRoleAssignment> Roles { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Boolean ForAllUsers { get; set; }
        public virtual Boolean IsEmpty { get { return !ForAllUsers && (Roles == null || !Roles.Any()); } }
        public dtoCommunityAssignment()
        {
            this.Type = PermissionType.Community;
            Roles = new List<dtoRoleAssignment>();
            //Modules = new List<dtoModuleAssignment>();
        }
        public dtoCommunityAssignment(VersionCommunityPermission cp) : base(cp)
        {
            this.Type = PermissionType.Community;
            Roles = new List<dtoRoleAssignment>();
            //Modules = new List<dtoModuleAssignment>();
        }
        public void UpdateItemsDisplayname(Dictionary<Int32, String> translations)
        {
            foreach (dtoRoleAssignment role in Roles)
            {
                role.UpdateDisplayname(translations);
            }
            Roles = Roles.OrderBy(p => p.DisplayName).ToList();
        }
        public void UpdateEditing(Boolean edit)
        {
            if (Roles != null)
                Roles.ForEach(p => p.AllowEdit = edit);
        }
    }

    [Serializable()]
    public class dtoPersonAssignment : dtoTemplateAssignment
    {
        public virtual Int32 IdPerson { get; set; }

        public virtual litePerson AssignedTo { get; set; }
        public override Boolean IsUnknown { get { return AssignedTo == null; } }
        public dtoPersonAssignment()
        {
            this.Type = PermissionType.Person;
        }

         public dtoPersonAssignment(VersionPersonPermission pa) : base(pa)
        {
            this.Type = PermissionType.Person;
            IdPerson = (pa.AssignedTo == null) ? 0 : pa.AssignedTo.Id;
            AssignedTo = pa.AssignedTo;
            Deleted = BaseStatusDeleted.None;
            Id = pa.Id;
            DisplayName = (pa.AssignedTo != null) ? pa.AssignedTo.SurnameAndName : "";
        }
    }

    [Serializable()]
    public class dtoProfileTypeAssignment : dtoTemplateAssignment
    {
        public virtual Int32 IdPersonType { get; set; }
        public dtoProfileTypeAssignment()
        {
            this.Type = PermissionType.ProfileType;
        }
        public dtoProfileTypeAssignment(VersionProfileTypePermission pt,Dictionary<Int32, String> translatedProfileTypes) : base(pt)
        {
            this.Type = PermissionType.ProfileType;
            IdPersonType = pt.AssignedTo;
            DisplayName = (translatedProfileTypes.ContainsKey(pt.AssignedTo) ? translatedProfileTypes[pt.AssignedTo] : pt.AssignedTo.ToString());
        }
        public void UpdateDisplayname(Dictionary<Int32, String> translations)
        {
            DisplayName = (translations.ContainsKey(IdPersonType)) ? translations[IdPersonType] : IdPersonType.ToString();
        }
    }

    [Serializable()]
    public class dtoRoleAssignment : dtoTemplateAssignment
    {
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdRole { get; set; }
        public dtoRoleAssignment()
        {
            this.Type = PermissionType.Role;
        }
        public dtoRoleAssignment(VersionRolePermission rp) : base(rp)
        {
            this.Type = PermissionType.Role;
        }
        public dtoRoleAssignment(VersionRolePermission rp, Dictionary<Int32, String> translations)
            : base(rp)
        {
            this.Type = PermissionType.Role;
            DisplayName = (translations.ContainsKey(IdRole)) ? translations[IdRole] : IdRole.ToString();
            IdCommunity = (rp.Community == null) ? 0 : rp.Community.Id;
            IdRole = (rp.AssignedTo == null) ? 0 : rp.AssignedTo.Id;
        }
        public void UpdateDisplayname(Dictionary<Int32, String> translations)
        {
            DisplayName = (translations.ContainsKey(IdRole)) ? translations[IdRole] : IdRole.ToString();
        }
    }


}