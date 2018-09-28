using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.FileRepository.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable()]
    public class ProjectAttachment : DomainBaseObjectIdLiteMetaInfo<long>
    {
        public virtual liteRepositoryItem Item { get; set; }
        public virtual liteRepositoryItemVersion Version { get; set; }
        public virtual PmActivity Activity { get; set; }
        public virtual Project Project { get; set; }
        public virtual liteModuleLink Link { get; set; }
        public virtual AttachmentType Type { get; set; }
        public virtual String Url { get; set; }
        public virtual String UrlName { get; set; }
        public virtual String Description { get; set; }
        public virtual IList<ProjectAttachmentLink> SharedItems { get; set; }
        public virtual Boolean IsForProject { get; set; }
         public ProjectAttachment()
        {
            Deleted = BaseStatusDeleted.None;
            SharedItems = new List<ProjectAttachmentLink>();
        }
    }
}