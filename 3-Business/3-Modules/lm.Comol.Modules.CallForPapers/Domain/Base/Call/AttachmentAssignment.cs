using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class AttachmentAssignment : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual SubmitterType SubmitterType { get; set; }
        public virtual AttachmentFile Attachment { get; set; }
        public virtual BaseForPaper Call { get; set; }

         public AttachmentAssignment()
        {
            Deleted = BaseStatusDeleted.None;
        }
         public AttachmentAssignment(AttachmentFile file, SubmitterType submitterType)
        {
            SubmitterType = submitterType;
            Attachment = file;
            Deleted = BaseStatusDeleted.None;
        }
    }
}
