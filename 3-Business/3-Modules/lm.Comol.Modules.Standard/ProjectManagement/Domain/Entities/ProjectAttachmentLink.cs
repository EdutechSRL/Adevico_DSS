using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable()]
    public class ProjectAttachmentLink : DomainBaseObjectIdLiteMetaInfo<long>
    {
        public virtual ProjectAttachment Attachment { get; set; }
        public virtual PmActivity Activity { get; set; }
        public virtual Project Project { get; set; }
        public virtual long DisplayOrder { get; set; }
        public virtual String Description { get; set; }
        public virtual Boolean IsForProject { get; set; }
        public virtual AttachmentLinkType Type { get; set; }
        
        public ProjectAttachmentLink()
        {
            Deleted = BaseStatusDeleted.None;
        }

        public static ProjectAttachmentLink CreateFromAttachment(ProjectAttachment attachment, long displayOrder =0)
        {
            ProjectAttachmentLink link = new ProjectAttachmentLink();
            link.Activity = attachment.Activity;
            link.Attachment = attachment;
            link.IdCreatedBy = attachment.IdCreatedBy;
            link.CreatedOn = attachment.CreatedOn;
            link.CreatorIpAddress = attachment.CreatorIpAddress;
            link.CreatorProxyIpAddress = attachment.CreatorProxyIpAddress;
            link.Description = attachment.Description;
            link.DisplayOrder = displayOrder;
            link.IsForProject = attachment.IsForProject;
            link.IdModifiedBy = attachment.IdModifiedBy;
            link.ModifiedIpAddress = attachment.ModifiedIpAddress;
            link.ModifiedOn = attachment.ModifiedOn;
            link.ModifiedProxyIpAddress = attachment.ModifiedProxyIpAddress;
            link.Project = attachment.Project;
            link.Type = AttachmentLinkType.Owner;
            return link;
        }
    }
}