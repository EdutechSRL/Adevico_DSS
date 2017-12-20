using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class ProfileAttributeAssociation : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual BaseForPaper Call { get; set; }
        public virtual FieldDefinition Field { get; set; }
        public virtual lm.Comol.Core.Authentication.ProfileAttributeType  Attribute {get; set;}

        public ProfileAttributeAssociation()
        {
            Attribute = Core.Authentication.ProfileAttributeType.unknown;
        }
    }
}
