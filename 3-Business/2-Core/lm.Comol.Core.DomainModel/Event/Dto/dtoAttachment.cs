using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.FileRepository.Domain;
namespace lm.Comol.Core.Events.Domain
{
    public class dtoAttachment 
    {
        public long Id { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual long IdEventItem { get; set; }
        public virtual long IdEvent { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public virtual RepositoryItemObject File { get; set; }
        public long IdModuleLink { get; set; }
        public liteModuleLink Link { get; set; }
        public virtual String Description { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual Int32 IdCreatedBy { get; set; }  
        public virtual String CreatedBy { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual Int32 IdModifiedBy { get; set; }
        public virtual String ModifiedBy { get; set; }
        public virtual String DisplayName { get { return (File != null) ? File.DisplayName : Id.ToString(); } }
        public virtual Boolean IsVisible { get; set; }
        public virtual Int32 DisplayOrder { get; set; }
        public dtoAttachment()
        {

        }
        public dtoAttachment(EventItemFile itemFile, String unknownUser)
            : this(itemFile)
        {
            CreatedBy = (itemFile.CreatedBy == null ? unknownUser : itemFile.CreatedBy.SurnameAndName);
            ModifiedBy = (itemFile.ModifiedBy == null ? unknownUser : itemFile.ModifiedBy.SurnameAndName);
        }

        public dtoAttachment(EventItemFile itemFile)
        {
            Id = itemFile.Id;
            IdEvent = itemFile.IdEventOwner;
            IdEventItem = itemFile.IdItemOwner;
            IdCommunity = itemFile.IdCommunity;

            Link = itemFile.Link;
            IdModuleLink = (Link == null) ? 0 : Link.Id;
            if (itemFile.Item != null)
                File = new RepositoryItemObject(itemFile.Item);

            CreatedOn = itemFile.CreatedOn.Value;
            if (ModifiedOn.HasValue)
                ModifiedOn = itemFile.ModifiedOn.Value;
            Deleted = itemFile.Deleted;
            IsVisible = itemFile.isVisible;
        }
    }
}