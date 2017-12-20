using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    [Serializable, CLSCompliant(true)]
    public class liteTranslation
    {
        public virtual Int64 Id { get; set; }
        public virtual String LanguageCode { get; set; }
        public virtual Int32 LanguageId { get; set; }
        public virtual String Name { get; set; }

        public virtual liteCategory Category { get; set; }

        /// <summary>
        /// Descrizione (alt sul nome nella treeview)
        /// </summary>
        public virtual String Description { get; set; }
    }
}
