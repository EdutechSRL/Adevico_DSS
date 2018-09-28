using System;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    [Serializable]
    public class TermSerialized : DomainBaseObject<long>
    {
        public virtual Int64 IdGlossary { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual String DescriptionText { get; set; }
        public virtual ItemStatus Status { get; set; }
        public virtual char FirstLetter { get; set; }
        public virtual bool IsPublic { get; set; }
        public virtual bool IsPublished { get; set; }
    }
}