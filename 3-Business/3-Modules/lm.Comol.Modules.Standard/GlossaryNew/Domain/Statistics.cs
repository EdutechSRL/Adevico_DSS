using System;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    [Serializable]
    public class Statistics : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual Int64 IdGlossary { get; set; }
        public virtual Int64 IdTerm { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdPerson { get; set; }
        public virtual DateTime ViewdOn { get; set; }
    }
}