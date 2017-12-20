using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Mail.Messages
{
    [Serializable]
    public class MailAttachment
    {
        public virtual long Id { get; set; } 
        public virtual MailMessage Message { get; set; } 
        public virtual int DisplayOrder { get; set; }
        public virtual BaseCommunityFile File { get; set; }
        public virtual ModuleLink Link { get; set; }
        public virtual String DirectFilename { get; set; }
        public virtual String DirectFullname { get; set; }
        public virtual MailAttachmentType Type { get; set; }
        

        public virtual BaseStatusDeleted Deleted { get; set; }
        public MailAttachment() { }
    }
}