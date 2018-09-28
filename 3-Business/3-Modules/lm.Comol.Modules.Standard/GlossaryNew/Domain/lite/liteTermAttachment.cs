using System;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    [Serializable]
    public class liteTermAttachment : DomainBaseObjectLiteMetaInfo<long>
    {
        public liteTermAttachment()
        {
            Deleted = BaseStatusDeleted.None;
        }

        public virtual BaseCommunityFile File { get; set; }
        public virtual long IdTerm { get; set; }
        public virtual long IdGlossary { get; set; }
        public virtual ModuleLink Link { get; set; }
        public virtual TermAttachmentType Type { get; set; }
        public virtual String Url { get; set; }
        public virtual String UrlName { get; set; }
        public virtual String Description { get; set; }
    }
}