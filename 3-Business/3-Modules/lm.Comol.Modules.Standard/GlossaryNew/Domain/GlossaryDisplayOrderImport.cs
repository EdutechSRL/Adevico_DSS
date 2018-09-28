using System;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    [Serializable]
    public class GlossaryDisplayOrderImport : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual Int64 IdGlossary { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 DisplayOrder { get; set; }
        public virtual bool IsDefault { get; set; }
    }
}