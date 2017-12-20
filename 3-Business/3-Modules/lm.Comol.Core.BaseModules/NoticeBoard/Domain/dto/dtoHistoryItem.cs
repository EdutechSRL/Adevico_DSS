using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Domain
{
    [Serializable]
    public class dtoHistoryItem
    {
        public virtual long Id {get;set;}
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual String ModifiedBy { get; set; }
        public virtual Boolean isDeleted{get;set;}
        public virtual lm.Comol.Core.DomainModel.ItemDisplayOrder DisplayAs { get; set; }
        public virtual Boolean Selected { get; set; }
        public dtoHistoryItem()
        {
            DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.item;
        }
        public dtoHistoryItem(liteHistoryItem item, String removedUser)
        {
            DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.item;
            Id = item.Id;
            isDeleted = item.isDeleted;
            ModifiedOn = item.DisplayDate; // (item.ModifiedOn.HasValue) ? item.ModifiedOn : item.CreatedOn;
            ModifiedBy = (item.ModifiedBy != null) ? item.ModifiedBy.SurnameAndName : ((item.ModifiedOn.HasValue) ? removedUser : (item.CreatedBy !=null) ? item.CreatedBy.SurnameAndName : removedUser);

        }        
    }
}
