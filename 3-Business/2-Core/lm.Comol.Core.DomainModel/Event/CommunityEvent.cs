using System;
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel
{
    [Serializable(), CLSCompliant(true)]
    public class CommunityEvent : lm.Comol.Core.DomainModel.DomainObject<long>
    {
        public virtual CommunityEvent FatherEvent { get; set; }
        public virtual String Name { get; set; }
        public virtual bool IsMacro { get; set; }
        public virtual bool IsVisible { get; set; }
        public virtual bool IsRepeat { get; set; }
        public virtual String Note { get; set; }
        public virtual String NotePlain { get; set; }
        public virtual string Place { get; set; }
        public virtual string Link { get; set; }
        public virtual CommunityEventType EventType { get; set; }
        public virtual DateTime ModifiedOn { get; set; }
        public virtual Int32 IdCommunityOwner { get; set; }
        public virtual int Year { get; set; }
        public virtual bool ForEver { get; set; }
        public virtual litePerson Owner { get; set; }
        public virtual long IdCloneOf { get; set; }
        public virtual IList<CommunityEventItem> Items { get; set; }
        public virtual Boolean AllowEdit { get; set; }
        public CommunityEvent()
        {
            Items = new List<CommunityEventItem>();
            ForEver = false;
            IsMacro = false;
            IsRepeat = false;
            IsVisible = true;
            Link = " ";
            Name = " ";
            Note = " ";
            Place = "";
            AllowEdit = true;
        }
    }
}