using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Domain
{
    public class liteHistoryItem : lm.Comol.Core.DomainModel.DomainObject<long>
    {
        public virtual Int32 IdCommunity{get;set;}
        public virtual litePerson Owner{get;set;}
        public virtual Boolean isForPortal{get;set;}
        public virtual StyleSettings StyleSettings { get; set; }
        public virtual litePerson CreatedBy{get;set;}
        public virtual DateTime? CreatedOn{get;set;}
        public virtual Boolean isDeleted{get;set;}
        public virtual litePerson ModifiedBy{get;set;}
        public virtual DateTime? ModifiedOn{get;set;}
        public virtual DateTime DisplayDate { get; set; }
        public virtual System.Guid Image { get; set; }
        public virtual System.Guid Thumbnail { get; set; }
        public virtual Status Status { get; set; }
        public liteHistoryItem()
        {
            Status = Status.Draft;
        }
    }
}