using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.FileRepository.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable()]
    public class liteProjectAttachment
    {
        public virtual long Id { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public virtual liteRepositoryItem Item { get; set; }
        public virtual liteRepositoryItemVersion Version { get; set; }
        public virtual long IdActivity { get; set; }
        public virtual long IdProject { get; set; }
        public virtual liteModuleLink Link { get; set; }
        public virtual AttachmentType Type { get; set; }
        public virtual String Url { get; set; }
        public virtual String UrlName { get; set; }
        public virtual String Description { get; set; }
        public virtual IList<liteProjectAttachmentLink> SharedItems { get; set; }
        public virtual Boolean IsForProject { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual Int32 IdCreatedBy { get; set; }
        public liteProjectAttachment()
        {
            Deleted = BaseStatusDeleted.None;
            SharedItems = new List<liteProjectAttachmentLink>();
        }
    }
}