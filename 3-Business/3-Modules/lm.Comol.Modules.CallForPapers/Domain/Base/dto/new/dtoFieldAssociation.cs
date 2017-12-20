using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
     [Serializable]
    public class dtoFieldAssociation :dtoBase 
    {
        public virtual long IdField { get; set; }
        public virtual long IdSection { get; set; }
        public virtual String Name { get; set; }
        public virtual lm.Comol.Core.Authentication.ProfileAttributeType Attribute { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual FieldType Type { get; set; }
   
        public dtoFieldAssociation()
            : base()
        {
            Attribute = Core.Authentication.ProfileAttributeType.skip;
        }

        //public dtoFieldAssociation(long idField, String name, int display, FieldType type)
        //    : base()
        //{
        //    IdField = idField;
        //    DisplayOrder = display;
        //    Name = name;
        //    Type = type;
        //    Attribute = Core.Authentication.ProfileAttributeType.skip;
        //}

        public void SetAssociationInfo(long id, Core.Authentication.ProfileAttributeType attribute) {
            Id = id;
            Attribute = attribute;
        }

    }
}