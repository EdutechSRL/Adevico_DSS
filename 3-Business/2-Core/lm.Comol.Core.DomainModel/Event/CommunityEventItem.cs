
using System;
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel
{
    [Serializable(), CLSCompliant(true)]
    public class CommunityEventItem : lm.Comol.Core.DomainModel.DomainObject<long>
    {
        public virtual CommunityEvent EventOwner { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual string Place { get; set; }
        public virtual bool IsVisible { get; set; }
        public virtual litePerson Owner { get; set; }
        public virtual litePerson CreatedBy { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual litePerson ModifiedBy { get; set; }
        public virtual DateTime ModifiedOn { get; set; }
        public virtual string Link { get; set; }
        public virtual long IdCloneOf { get; set; }
        public virtual String Note { get; set; }
        public virtual String NotePlain { get; set; }
        public virtual string Title { get; set; }
        public virtual bool ShowDateInfo { get; set; }
        public virtual Int32 IdCommunityOwner { get; set; }
        public virtual Boolean AllowEdit { get; set; }
        public virtual Int32 MinutesDuration { get; set; }
        public CommunityEventItem()
        {
            //Files = new List<EventFile>();
        }
    }
}