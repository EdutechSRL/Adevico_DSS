using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    /// <summary>
    /// Utilizzata ESCLUSIVAMENTE per il recupero degli ID dei FIGLI.
    /// </summary>
    [Serializable, CLSCompliant(true)]
    public class liteCategoryId
    {
        /// <summary>
        /// Id Categoria
        /// </summary>
        public virtual Int64 Id { get; set; }

        /// <summary>
        /// Id Figli
        /// </summary>
        public virtual IList<liteCategoryId> Childrens { get; set; }

        //Id padre
        public virtual Int64? FatherId { get; set; }
    }
}
