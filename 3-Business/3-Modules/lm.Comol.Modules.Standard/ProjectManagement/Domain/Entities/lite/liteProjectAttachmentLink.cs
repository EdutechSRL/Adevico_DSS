using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable()]
    public class liteProjectAttachmentLink
    {
        public virtual long Id { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public virtual long IdAttachment { get; set; }
        public virtual long IdActivity { get; set; }
        public virtual long IdProject { get; set; }
        public virtual Boolean IsForProject { get; set; }
        public virtual long DisplayOrder { get; set; }
        public virtual String Description { get; set; }
        public virtual DateTime SharedOn { get; set; }
        public virtual Int32 IdSharedBy { get; set; }
        public virtual AttachmentLinkType Type { get; set; }
        public liteProjectAttachmentLink()
        {
            Deleted = BaseStatusDeleted.None;
        }
    }
}