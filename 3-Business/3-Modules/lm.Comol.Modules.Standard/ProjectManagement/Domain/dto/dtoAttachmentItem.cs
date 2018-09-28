using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoAttachmentItem 
    {
        public virtual dtoAttachment Attachment { get; set; }
        public virtual dtoAttachmentPermission Permissions { get; set; }

        public dtoAttachmentItem()
        {
            Permissions = new dtoAttachmentPermission();
        }
    }
}