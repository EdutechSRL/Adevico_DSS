using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Events.Domain
{
    [Serializable]
    public class dtoAttachmentPermission
    {
        public virtual Boolean Delete { get; set; }
        public virtual Boolean VirtualDelete { get; set; }
        public virtual Boolean UnDelete { get; set; }
        public virtual Boolean Download { get; set; }
        public virtual Boolean Unlink { get; set; }
        public virtual Boolean Play { get; set; }
        public virtual Boolean Edit { get; set; }
        public virtual Boolean ViewMyStatistics { get; set; }
        public virtual Boolean ViewOtherStatistics { get; set; }
        public virtual Boolean SetMetadata { get; set; }
        public virtual Boolean EditVisibility { get; set; }
        public virtual Boolean Publish { get; set; }
        public virtual Boolean EditRepositoryVisibility { get; set; }
        public virtual Boolean EditRepositoryPermission { get; set; }
        public dtoAttachmentPermission()
        { 
        }
        //public dtoAttachmentPermission(dtoAttachmentFile attachment, dtoGenericPermission generic, long count)
        //    : base(attachment.Id)
        //{
        //    Attachment = attachment;
        //    Deleted = attachment.Deleted;
        //    AllowDelete = generic.AllowDelete  && Deleted != BaseStatusDeleted.None;
        //    AllowVirtualDelete = Deleted == BaseStatusDeleted.None && generic.AllowVirtualDelete && !(count>0);
        //    AllowUnDelete = Deleted != BaseStatusDeleted.None && generic.AllowUnDelete;
        //    SubmissionCount = count;
        //}
    }
}