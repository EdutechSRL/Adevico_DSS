using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.FileRepository.Domain;
namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    public class dtoAttachment 
    {
        public long IdAttachmentLink { get; set; }
        public long IdAttachment { get; set; }
        public virtual long IdActivity { get; set; }
        public virtual long IdProject { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }

        public virtual RepositoryItemObject File { get; set; }
        public long ModuleLinkId { get; set; }
        public liteModuleLink Link { get; set; }
        public virtual AttachmentType Type { get; set; }
        public virtual String Url { get; set; }
        public virtual String UrlName { get; set; }
        public virtual long DisplayOrder { get; set; }
        public virtual String Description { get; set; }
        public virtual Boolean IsForProject { get; set; }
        public virtual Boolean IsShared { get; set; }
        public virtual Boolean InSharing { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual Int32 IdCreatedBy { get; set; }  
        public virtual String CreatedBy { get; set; }  
        public virtual String DisplayName {get {
            switch(Type){
                case AttachmentType.url:
                    if (String.IsNullOrEmpty(UrlName))
                        return Url;
                    else
                        return UrlName;
                case AttachmentType.file:
                    return (File != null) ? File.DisplayName : IdAttachmentLink.ToString();
                default:
                    return IdAttachmentLink.ToString();
                }
    
            }}

        public dtoAttachment()
        {

        }
        public dtoAttachment(ProjectAttachmentLink link,Dictionary<Int32,String> users, String unknownUser): this(link)
        {
            CreatedBy = (users.ContainsKey(link.IdCreatedBy) ? users[link.IdCreatedBy] : unknownUser);
        }

        public dtoAttachment(ProjectAttachmentLink link)
        {
            IdAttachmentLink = link.Id;
            IdActivity = (link.Activity == null) ? 0 : link.Activity.Id;
            IdAttachment = (link.Attachment == null) ? 0 : link.Attachment.Id;
            if (IdAttachment > 0)
            {
                Link = link.Attachment.Link;
                ModuleLinkId = (Link == null) ? 0 : link.Attachment.Link.Id;
                Type = link.Attachment.Type;
                if (link.Attachment != null)
                    File = new RepositoryItemObject(link.Attachment.Item);
                Url = link.Attachment.Url;
                UrlName = link.Attachment.UrlName;
                IsShared = link.Attachment.SharedItems.Where(s => s.Deleted == BaseStatusDeleted.None && s.Type == AttachmentLinkType.Shared).Any();
            }
            IdProject = link.Project.Id;
            DisplayOrder = link.DisplayOrder;
            IsForProject = link.IsForProject;

            InSharing = (link.Type == AttachmentLinkType.Shared);
            CreatedOn = link.CreatedOn.Value;
            Deleted = link.Deleted;
        }
    }
}