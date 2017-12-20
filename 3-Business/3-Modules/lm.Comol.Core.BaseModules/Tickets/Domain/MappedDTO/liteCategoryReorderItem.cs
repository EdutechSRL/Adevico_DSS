using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    [Serializable, CLSCompliant(true)]
    public class liteCategoryReorderItem : DomainBaseObjectMetaInfo<long>
    {
        //public virtual Int64 CategoryId { get; set; }
        public virtual int Order { get; set; }
        public virtual Int64? FatherId { get; set; }
        //public virtual liteUser User { get; set; }
        //public virtual liteCategoryTreeItem Category { get; set; }
        /// <summary>
        /// Solo per riordino. Indica che l'elemento appartiene al ramo di default.
        /// </summary>
        public virtual Boolean DefCateFamily { get; set; }

    }
}