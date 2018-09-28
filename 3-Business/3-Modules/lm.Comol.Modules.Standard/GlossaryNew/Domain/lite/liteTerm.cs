using System;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    [Serializable]
    public class liteTerm : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual long IdGlossary { get; set; }
        public virtual String Name { get; set; }
        public virtual string Description { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual ItemStatus Status { get; set; }
        public virtual Boolean IsPublic { get; set; }
        public virtual Boolean IsPublished { get; set; }
        public virtual Char FirstLetter { get; set; }
    }
}