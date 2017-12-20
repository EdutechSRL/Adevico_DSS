using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    public class dtoAttachmentFile : dtoBaseFile
    {
        public Int32 DisplayOrder { get; set; }
        public String Description { get; set; }
        public Boolean ForAll { get; set; }
        public List<long> SubmitterAssignments { get; set; }
        public dtoAttachmentFile() :base() {
            SubmitterAssignments = new List<long>();
        }

        public dtoAttachmentFile(AttachmentFile attachmentFile)
        {
            Id = attachmentFile.Id;
            Item = attachmentFile.Item;
            Deleted = attachmentFile.Deleted;
            ModuleLinkId = attachmentFile.Link.Id;
            DisplayOrder = attachmentFile.DisplayOrder;
            Description = attachmentFile.Description;
            ForAll = attachmentFile.ForAll;
            SubmitterAssignments = new List<long>();
        }
    }
}
