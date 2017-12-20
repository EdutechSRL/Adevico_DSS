using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Domain
{
    public class NoticeboardMessage : lm.Comol.Core.DomainModel.DomainObject<long>, IEquatable<NoticeboardMessage>
        {
            public virtual Community Community{get;set;}
            public virtual Person Owner{get;set;}
            public virtual String Message{get;set;}
            public virtual String PlainText { get; set; }
        
            public virtual Boolean CreateByAdvancedEditor{get;set;}
            public virtual Boolean isForPortal{get;set;}
            public virtual MessageStyle Style { get; set; }

            public virtual DateTime DisplayDate { get; set; }
        

            public virtual Person CreatedBy{get;set;}
            public virtual DateTime? CreatedOn{get;set;}
            public virtual Boolean isDeleted{get;set;}
            public virtual Person ModifiedBy{get;set;}
            public virtual DateTime? ModifiedOn{get;set;}
            public virtual Status Status { get; set; }
        
            public virtual StyleSettings StyleSettings { get; set; }
            public virtual System.Guid Image { get; set; }
            public virtual System.Guid Thumbnail { get; set; }

            public NoticeboardMessage()
            {
                Message="";
                CreateByAdvancedEditor = true;
                isForPortal = false;
                Status = Status.Draft;
            }

            public virtual bool Equals(NoticeboardMessage other)
            {
 	           return this.Id.Equals(other.Id) && this.isForPortal.Equals(other.isForPortal);
            }
    }
}