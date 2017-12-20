using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class FieldOption : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual String Name { get; set; }
        public virtual String Value { get; set; }
        public virtual long DisplayOrder { get; set; }
        public virtual FieldDefinition Field { get; set; }
        public virtual Boolean IsDefault { get; set; }
        public virtual Boolean IsFreeValue { get; set; }
    
        public FieldOption()
        {
            this.Name = "";
            this.Value = "";
        }
    }
}
