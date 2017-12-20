using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.FileRepository.Domain;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class AttachmentFile : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual int DisplayOrder { get; set; }
        public virtual BaseForPaper Call { get; set; }
        public virtual liteRepositoryItem Item { get; set; }
        public virtual liteModuleLink Link { get; set; }
        public virtual Boolean ForAll { get; set; }
        public virtual String Description { get; set; }
        public AttachmentFile()
        {
            Deleted = BaseStatusDeleted.None;
            ForAll = true;
        }
        public AttachmentFile(BaseForPaper callForPaper, lm.Comol.Core.FileRepository.Domain.liteRepositoryItem item)
            : this()
        {
            Call = callForPaper;
            Item = item;
        }
    }
}
