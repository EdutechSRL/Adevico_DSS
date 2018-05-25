using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.CommunityExpiration.Domain
{
    /// <summary>
    /// Configurazione di sistema/comunità
    /// </summary>
    public class ExpirationConfig : DomainBaseObjectLiteMetaInfo<long>
    {
        /// <summary>
        /// Id Ruolo
        /// </summary>
        public virtual int RoleId { get; set; }
        /// <summary>
        /// Id Comunità (portale = 0)
        /// </summary>
        public virtual int CommunityId { get; set; }
        /// <summary>
        /// Valore durata
        /// </summary>
        public virtual int Duration { get; set; }
        /// <summary>
        /// Id tipo comnuità
        /// </summary>
        public virtual int CommunityTypeId { get; set; }
    }
}
