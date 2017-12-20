using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoAttachmentFilePermission :dtoBase 
    {
        public virtual dtoAttachmentFile Attachment { get; set; }
        public virtual Boolean AllowDelete { get; set; }
        public virtual Boolean AllowVirtualDelete { get; set; }
        public virtual Boolean AllowUnDelete { get; set; }
        public virtual long SubmissionCount{ get; set; }
        public dtoAttachmentFilePermission()
            : base()
        { 
        }
        public dtoAttachmentFilePermission(long id, dtoAttachmentFile attachment)
            : base(id)
        {
            Attachment = attachment;
            Deleted = attachment.Deleted;
        }
        public dtoAttachmentFilePermission(dtoAttachmentFile attachment, dtoGenericPermission generic, long count)
            : base(attachment.Id)
        {
            Attachment = attachment;
            Deleted = attachment.Deleted;
            AllowDelete = generic.AllowDelete  && Deleted != BaseStatusDeleted.None;
            AllowVirtualDelete = Deleted == BaseStatusDeleted.None && generic.AllowVirtualDelete && !(count>0);
            AllowUnDelete = Deleted != BaseStatusDeleted.None && generic.AllowUnDelete;
            SubmissionCount = count;
        }
    }
}