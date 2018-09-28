using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain
{
    /// <summary>
    /// DTO con Id stanza e relativo codice
    /// </summary>
    public class WbAccessCode : DomainBaseObjectMetaInfo<long>
    {
        /// <summary>
        /// ID stanza
        /// </summary>
        public virtual Int64 RoomId { get; set; }
        /// <summary>
        /// Codice stanza
        /// </summary>
        public virtual String UrlCode { get; set; }
    }
}
