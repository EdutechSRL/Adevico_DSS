using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    /// <summary>
    /// Allegati. Da rivedere.
    /// </summary>
    [Serializable()]
    public class TicketFile : DomainBaseObjectLiteMetaInfo<long>
    {
        /// <summary>
        /// Nome file
        /// </summary>
        public virtual String Name { get; set; }
        /// <summary>
        /// Messaggio a cui appartiene
        /// </summary>
        public virtual Message Message { get; set; }
        /// <summary>
        /// Id Ticket a cui appartiene (x ricerche)
        /// </summary>
        public virtual Int64 TicketId { get; set; }

        public virtual lm.Comol.Core.FileRepository.Domain.liteRepositoryItem Item { get; set; }
        public virtual lm.Comol.Core.FileRepository.Domain.liteRepositoryItemVersion Version { get; set; }
        public virtual liteModuleLink Link { get; set; }

        public virtual Enums.FileVisibility Visibility { get; set; }

    }
}