using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public class dtoProfileAttributeType
    {
        public virtual ProfileAttributeType Attribute { get; set; }
        public virtual Boolean Mandatory { get; set; }
        public virtual UserTypeStandard  UserType { get; set; }
        public virtual List<ProfileAttributeType> AlternativeAttributes { get; set; }
        public virtual Boolean HasAlternative { get { return !(AlternativeAttributes==null || AlternativeAttributes.Count==0);}}
        public dtoProfileAttributeType() { 
            List<ProfileAttributeType> AlternativeAttributes  = new List<ProfileAttributeType>();
        }

        public dtoProfileAttributeType(ProfileAttributeType attribute)
        {
            Attribute = attribute;
            Mandatory = true;
            UserType = UserTypeStandard.AllUserType;
            List<ProfileAttributeType> AlternativeAttributes = new List<ProfileAttributeType>();
        }
        public dtoProfileAttributeType(ProfileAttributeType attribute, Boolean mandatory)
        {
            Attribute = attribute;
            Mandatory = mandatory;
            UserType = UserTypeStandard.AllUserType;
            List<ProfileAttributeType> AlternativeAttributes = new List<ProfileAttributeType>();
        }
        public dtoProfileAttributeType(ProfileAttributeType attribute, UserTypeStandard userType)
        {
            Attribute = attribute;
            Mandatory = true;
            UserType = userType;
            List<ProfileAttributeType> AlternativeAttributes = new List<ProfileAttributeType>();
        }
        public dtoProfileAttributeType(ProfileAttributeType attribute, Boolean mandatory,UserTypeStandard userType) :this()
        {
            Attribute = attribute;
            Mandatory = mandatory;
            UserType = userType;
            List<ProfileAttributeType> AlternativeAttributes = new List<ProfileAttributeType>();
        }
        public dtoProfileAttributeType(ProfileAttributeType attribute, Boolean mandatory, UserTypeStandard userType,List<ProfileAttributeType> alternatives)
        {
            Attribute = attribute;
            Mandatory = mandatory;
            UserType = userType;
            this.AlternativeAttributes = alternatives;
        }
    }
}