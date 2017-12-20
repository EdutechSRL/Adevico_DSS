using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    [Serializable]
    public class DTO_AttachmentItem
    {
        public long IdAttachment { get; set; }
        public virtual long IdMessage { get; set; }

        public virtual BaseStatusDeleted Deleted { get; set; }

        public virtual lm.Comol.Core.FileRepository.Domain.liteRepositoryItem Item { get; set; }
        public virtual lm.Comol.Core.FileRepository.Domain.liteRepositoryItemVersion Version { get; set; }
        public long ModuleLinkId { get; set; }
        public liteModuleLink Link { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual litePerson CreatedBy { get; set; }
        public virtual String DisplayName { get { return Item == null ? "" : Item.DisplayName; } }

        public virtual Domain.Enums.FileVisibility Visibility { get; set; }

        public DTO_AttachmentItem()
        {

        }

        public DTO_AttachmentItem(Domain.TicketFile SourceFile)
        {
            IdAttachment = SourceFile.Id;
            IdMessage = SourceFile.Message.Id;
            Deleted = SourceFile.Deleted;
            Item = SourceFile.Item;
            Version = SourceFile.Version;
            ModuleLinkId = (SourceFile.Link != null) ? SourceFile.Link.Id : 0;
            Link = SourceFile.Link;
            CreatedOn = SourceFile.CreatedOn.HasValue ? (DateTime)SourceFile.CreatedOn : DateTime.MinValue;
            CreatedBy = SourceFile.CreatedBy;
            Visibility = SourceFile.Visibility;
        }
    }
}
