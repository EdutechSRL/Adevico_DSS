using System;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    [Serializable]
    public class TermAttachment : DomainBaseObjectMetaInfo<long>
    {
        public TermAttachment()
        {
            Deleted = BaseStatusDeleted.None;
        }

        public virtual Int64 Id { get; set; }
        public virtual Term GlossaryTerm { get; set; }
        public virtual Int64? IdGlossary { get; set; }
        public virtual Int64? IdFile { get; set; }
        public virtual Int64? IdLink { get; set; }
        public virtual Int16? Type { get; set; }
        public virtual String Url { get; set; }
        public virtual String UrlName { get; set; }
        public virtual String Description { get; set; }
        //public virtual DateTime _Timestamp { get; set; }
        public virtual Int16? _Deleted { get; set; }
    }
}