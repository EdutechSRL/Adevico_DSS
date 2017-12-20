using lm.Comol.Core.FileRepository.Domain;
using System;
namespace lm.Comol.Core.DomainModel
{
    [Serializable(), CLSCompliant(true)]
    public class EventItemFile : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual litePerson Owner { get; set; }
        public virtual long IdItemOwner { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual long IdEventOwner { get; set; }
        public virtual liteRepositoryItem Item { get; set; }
        public virtual liteRepositoryItemVersion Version { get; set; }
        public virtual liteModuleLink Link { get; set; }
        public virtual Boolean isVisible { get; set; }
        public EventItemFile()
        {
        }
    }
}