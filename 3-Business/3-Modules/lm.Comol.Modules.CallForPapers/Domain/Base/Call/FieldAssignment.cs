using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class FieldAssignment : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual SubmitterType SubmitterType { get; set; }
        public virtual FieldDefinition Field { get; set; }

         public FieldAssignment()
        {
            Deleted = BaseStatusDeleted.None;
        }
         public FieldAssignment(FieldDefinition field, SubmitterType submitterType)
        {
            SubmitterType = submitterType;
            Field = field;
            Deleted = BaseStatusDeleted.None;
        }
    }
}
