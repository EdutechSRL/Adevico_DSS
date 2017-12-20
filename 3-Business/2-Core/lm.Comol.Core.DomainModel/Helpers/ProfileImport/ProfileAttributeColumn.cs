
using System;
using System.Collections.Generic;
using lm.Comol.Core.Authentication;
namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class ProfileAttributeColumn 
    {

        public virtual ColumnType Type { get; set; }
        public virtual ProfileAttributeType Attribute { get; set; }
        public virtual int Number { get; set; }
        //public virtual String Name { get; set; }
        public virtual Boolean AllowEmpty {
            get
            {
                return (Attribute == ProfileAttributeType.address || Attribute == ProfileAttributeType.city || Attribute == ProfileAttributeType.companyRegion
                    || Attribute == ProfileAttributeType.companyAssociations || Attribute == ProfileAttributeType.companyCity || Attribute == ProfileAttributeType.companyReaNumber
                    || Attribute == ProfileAttributeType.skip || Attribute == ProfileAttributeType.unknown
                   || Attribute == ProfileAttributeType.telephoneNumber || Attribute == ProfileAttributeType.fax || Attribute == ProfileAttributeType.mobile
                   || Attribute == ProfileAttributeType.zipCode || Attribute == ProfileAttributeType.companyTaxCode);
            }
        }
        public virtual Boolean AllowDuplicate { 

            get {
                return (Attribute != ProfileAttributeType.externalId && Attribute != ProfileAttributeType.login &&  Attribute != ProfileAttributeType.mail 
                     && Attribute != ProfileAttributeType.taxCode);
            }
        }

        public virtual Boolean ValidateValue
        {

            get
            {
                return (Attribute == ProfileAttributeType.mail || Attribute == ProfileAttributeType.mail);
            }
        }

        //public virtual Boolean Empty { get { return string.IsNullOrEmpty(Value); } }
        public ProfileAttributeColumn(){
            Type = ColumnType.normal;
        }
    }
}