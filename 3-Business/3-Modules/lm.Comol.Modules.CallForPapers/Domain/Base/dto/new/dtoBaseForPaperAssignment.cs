using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class dtoCallAssignment : DomainBaseObject<long> 
    {
        public virtual long IdCall { get; set; }
        public virtual Boolean Deny {get;set;}
        public virtual String DisplayName { get; set; }
        public virtual CallAssignmentType Type { get; set; }
        public virtual Boolean IsDefault { get; set; }
        public virtual Boolean IsUnknown { get; set; }
    }

    public class dtoCallPortalAssignment : dtoCallAssignment
    {
        public virtual List<dtoCallPersonTypeAssignment> ProfileTypes { get; set; }
        public virtual Boolean ForAllUsers { get { return (ProfileTypes == null || !ProfileTypes.Any()); } }
        public dtoCallPortalAssignment() {
            Type = CallAssignmentType.Portal;
            ProfileTypes = new List<dtoCallPersonTypeAssignment>();
        }

        public void UpdateItemsDisplayname(Dictionary<Int32, String> translations)
        {
            foreach (dtoCallPersonTypeAssignment pType in ProfileTypes)
            {
                pType.UpdateDisplayname(translations);
            }
            ProfileTypes = ProfileTypes.OrderBy(p => p.DisplayName).ToList();
        }
    }

    [Serializable()]
    public class dtoCallCommunityAssignment : dtoCallAssignment
    {
        public virtual List<dtoCallRoleAssignment> Roles  { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Boolean ForAllUsers { get; set; }
        public virtual Boolean IsEmpty { get { return !ForAllUsers && (Roles == null || !Roles.Any()); } }
        public dtoCallCommunityAssignment()
        {
            this.Type = CallAssignmentType.Community;
            Roles = new List<dtoCallRoleAssignment>();
        }
        public void UpdateItemsDisplayname(Dictionary<Int32, String> translations)
        {
            foreach (dtoCallRoleAssignment role in Roles)
            {
                role.UpdateDisplayname(translations);
            }
            Roles = Roles.OrderBy(p => p.DisplayName).ToList();
        }
    }

    [Serializable()]
    public class dtoCallPersonAssignment : dtoCallAssignment
    {
        public virtual Int32 IdPerson { get { return (AssignedTo == null) ? 0 : AssignedTo.Id; } }

        public virtual litePerson AssignedTo { get; set; }
        public override Boolean IsUnknown {get{ return AssignedTo==null;}}
        public dtoCallPersonAssignment()
        {
            this.Type = CallAssignmentType.Person;
        }
    }

    [Serializable()]
    public class dtoCallPersonTypeAssignment : dtoCallAssignment
    {
        public virtual Int32 IdPersonType { get; set; }
        public dtoCallPersonTypeAssignment()
        {
            this.Type = CallAssignmentType.PersonType;
        }
        public void UpdateDisplayname(Dictionary<Int32,String> translations) {
            DisplayName = (translations.ContainsKey(IdPersonType)) ? translations[IdPersonType] : IdPersonType.ToString();
        }
    }
    
    [Serializable()]
    public class dtoCallRoleAssignment : dtoCallAssignment
    {
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdRole { get; set; }
        public dtoCallRoleAssignment() {
            this.Type = CallAssignmentType.Role;
        }
        public void UpdateDisplayname(Dictionary<Int32, String> translations)
        {
            DisplayName = (translations.ContainsKey(IdRole)) ? translations[IdRole] : IdRole.ToString();
        }
    }

    [Serializable()]
    public enum CallAssignmentType { 
        None = -1,
        Community = 0,
        Person = 2,
        Role = 1,
        PersonType = 3,
        SubmitterOfBaseForPaper = 4,
        Portal = 5
    }
}