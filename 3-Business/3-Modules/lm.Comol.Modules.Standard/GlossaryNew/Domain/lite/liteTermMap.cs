using System;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    [Serializable]
    public class liteTermMap : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual String Name { get; set; }
        public virtual long IdGlossary { get; set; }
        public virtual Char FirstLetter { get; set; }
        public virtual Boolean IsPublished { get; set; }
    }
}